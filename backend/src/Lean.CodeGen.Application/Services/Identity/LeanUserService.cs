using System;
using System.Linq.Expressions;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Lean.CodeGen.Application.Dtos.Identity;
using Lean.CodeGen.Application.Dtos.Identity.Login;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Domain.Entities.Identity;
using Lean.CodeGen.Domain.Interfaces.Repositories;
using Lean.CodeGen.Domain.Validators;
using Mapster;
using Lean.CodeGen.Common.Options;
using Lean.CodeGen.Application.Services.Base;
using Lean.CodeGen.Application.Services.Security;
using Microsoft.Extensions.Options;
using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Common.Extensions;
using Lean.CodeGen.Common.Security;
using Lean.CodeGen.Common.Exceptions;
using Microsoft.Extensions.Logging;

namespace Lean.CodeGen.Application.Services.Identity;

/// <summary>
/// 用户服务实现
/// </summary>
public class LeanUserService : LeanBaseService, ILeanUserService
{
  private readonly ILeanRepository<LeanUser> _userRepository;
  private readonly ILeanRepository<LeanUserRole> _userRoleRepository;
  private readonly ILeanRepository<LeanUserDept> _userDeptRepository;
  private readonly ILeanRepository<LeanUserPost> _userPostRepository;
  private readonly ILeanRepository<LeanLoginExtend> _loginExtendRepository;
  private readonly ILeanRepository<LeanDeviceExtend> _deviceExtendRepository;
  private readonly IHttpContextAccessor _httpContextAccessor;
  private readonly LeanUniqueValidator<LeanUser> _uniqueValidator;
  private readonly IOptions<LeanSecurityOptions> _securityOptions;
  private readonly ITokenService _tokenService;
  private readonly ILogger<LeanUserService> _logger;

  public LeanUserService(
      ILeanRepository<LeanUser> userRepository,
      ILeanRepository<LeanUserRole> userRoleRepository,
      ILeanRepository<LeanUserDept> userDeptRepository,
      ILeanRepository<LeanUserPost> userPostRepository,
      ILeanRepository<LeanLoginExtend> loginExtendRepository,
      ILeanRepository<LeanDeviceExtend> deviceExtendRepository,
      IHttpContextAccessor httpContextAccessor,
      ILeanSqlSafeService sqlSafeService,
      IOptions<LeanSecurityOptions> securityOptions,
      ITokenService tokenService,
      ILogger<LeanUserService> logger)
      : base(sqlSafeService, securityOptions, logger)
  {
    _userRepository = userRepository;
    _userRoleRepository = userRoleRepository;
    _userDeptRepository = userDeptRepository;
    _userPostRepository = userPostRepository;
    _loginExtendRepository = loginExtendRepository;
    _deviceExtendRepository = deviceExtendRepository;
    _httpContextAccessor = httpContextAccessor;
    _uniqueValidator = new LeanUniqueValidator<LeanUser>(userRepository);
    _securityOptions = securityOptions;
    _tokenService = tokenService;
    _logger = logger;
  }

  /// <summary>
  /// 创建用户
  /// </summary>
  public async Task<LeanApiResult<long>> CreateAsync(LeanCreateUserDto input)
  {
    return await ExecuteInTransactionAsync(async () =>
    {
      // 验证用户输入
      await ValidateUserInputAsync(input);

      // 验证密码强度
      var (isValid, message) = LeanPassword.ValidatePasswordStrength(input.Password);
      if (!isValid)
      {
        throw new LeanValidationException(message);
      }

      // 创建用户实体
      var user = input.Adapt<LeanUser>();
      user.Salt = LeanPassword.GenerateSalt();
      user.Password = LeanPassword.HashPassword(input.Password, user.Salt);
      user.CreateTime = DateTime.Now;

      await _userRepository.BeginTransactionAsync();

      try
      {
        // 创建用户
        var id = await _userRepository.CreateAsync(user);

        // 创建用户关联
        await CreateUserRelationsAsync(id, input);

        await _userRepository.CommitAsync();

        LogAudit("CreateUser", $"创建用户: {user.UserName}");
        return LeanApiResult<long>.Ok(id);
      }
      catch
      {
        await _userRepository.RollbackAsync();
        throw;
      }
    }, "CreateUser");
  }

  /// <summary>
  /// 更新用户
  /// </summary>
  public async Task<LeanApiResult> UpdateAsync(LeanUpdateUserDto input)
  {
    return await ExecuteInTransactionAsync(async () =>
    {
      var user = await GetUserByIdAsync(input.Id);
      if (user == null)
      {
        throw new LeanNotFoundException($"用户 {input.Id} 不存在");
      }

      // 验证用户输入
      await ValidateUserInputAsync(input, input.Id);

      input.Adapt(user);
      user.UpdateTime = DateTime.Now;

      await _userRepository.BeginTransactionAsync();

      try
      {
        await _userRepository.UpdateAsync(user);
        await UpdateUserRelationsAsync(user.Id, input);

        await _userRepository.CommitAsync();

        LogAudit("UpdateUser", $"更新用户: {user.UserName}");
        return LeanApiResult.Ok();
      }
      catch
      {
        await _userRepository.RollbackAsync();
        throw;
      }
    }, "UpdateUser");
  }

  /// <summary>
  /// 删除用户
  /// </summary>
  public async Task<LeanApiResult> DeleteAsync(long id)
  {
    return await ExecuteInTransactionAsync(async () =>
    {
      var user = await GetUserByIdAsync(id);
      if (user == null)
      {
        throw new LeanNotFoundException($"用户 {id} 不存在");
      }

      if (user.IsBuiltin == LeanBuiltinStatus.Yes)
      {
        throw new LeanBusinessException($"用户 {user.UserName} 是内置用户，不能删除");
      }

      await _userRepository.BeginTransactionAsync();

      try
      {
        await DeleteUserRelationsAsync(id);
        await _userRepository.DeleteAsync(u => u.Id == id);

        await _userRepository.CommitAsync();

        LogAudit("DeleteUser", $"删除用户: {user.UserName}");
        return LeanApiResult.Ok();
      }
      catch
      {
        await _userRepository.RollbackAsync();
        throw;
      }
    }, "DeleteUser");
  }

  /// <summary>
  /// 批量删除用户
  /// </summary>
  public async Task<LeanApiResult> BatchDeleteAsync(List<long> ids)
  {
    try
    {
      foreach (var id in ids)
      {
        var result = await DeleteAsync(id);
        if (!result.Success)
        {
          return LeanApiResult.Error($"删除用户失败: {result.Message}");
        }
      }

      return LeanApiResult.Ok();
    }
    catch (Exception ex)
    {
      return LeanApiResult.Error($"批量删除用户失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 获取用户信息
  /// </summary>
  public async Task<LeanApiResult<LeanUserDto>> GetAsync(long id)
  {
    return await ExecuteInTransactionAsync(async () =>
    {
      using var perf = LogPerformance($"GetUser_{id}");

      var user = await GetUserByIdAsync(id);
      if (user == null)
      {
        throw new LeanNotFoundException($"用户 {id} 不存在");
      }

      var result = user.Adapt<LeanUserDto>();
      await FillUserRelationsAsync(result);

      return LeanApiResult<LeanUserDto>.Ok(result);
    }, "GetUser");
  }

  /// <summary>
  /// 分页查询用户
  /// </summary>
  public async Task<LeanApiResult<LeanPageResult<LeanUserDto>>> GetPageAsync(LeanQueryUserDto input)
  {
    return await ExecuteInTransactionAsync(async () =>
    {
      using var perf = LogPerformance("GetUserPage");

      var predicate = BuildUserQueryPredicate(input);
      var (total, items) = await _userRepository.GetPageListAsync(
        predicate,
        input.PageSize,
        input.PageIndex,
        u => u.CreateTime,
        false);

      var result = new LeanPageResult<LeanUserDto>
      {
        Total = total,
        Items = items.Select(u => u.Adapt<LeanUserDto>()).ToList()
      };

      foreach (var item in result.Items)
      {
        await FillUserRelationsAsync(item);
      }

      return LeanApiResult<LeanPageResult<LeanUserDto>>.Ok(result);
    }, "GetUserPage");
  }

  /// <summary>
  /// 修改用户状态
  /// </summary>
  public async Task<LeanApiResult> SetStatusAsync(LeanChangeUserStatusDto input)
  {
    try
    {
      var user = await GetUserByIdAsync(input.Id);
      if (user == null)
      {
        return LeanApiResult.Error($"用户 {input.Id} 不存在");
      }

      user.UserStatus = input.UserStatus;
      user.UpdateTime = DateTime.Now;

      await _userRepository.UpdateAsync(user);

      return LeanApiResult.Ok();
    }
    catch (Exception ex)
    {
      return LeanApiResult.Error($"修改用户状态失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 重置密码
  /// </summary>
  public async Task<LeanApiResult> ResetPasswordAsync(LeanResetUserPasswordDto input)
  {
    try
    {
      var user = await GetUserByIdAsync(input.Id);
      if (user == null)
      {
        return LeanApiResult.Error($"用户 {input.Id} 不存在");
      }

      await UpdateUserPasswordAsync(user, input.NewPassword);

      return LeanApiResult.Ok();
    }
    catch (Exception ex)
    {
      return LeanApiResult.Error($"重置密码失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 修改密码
  /// </summary>
  public async Task<LeanApiResult> ChangePasswordAsync(LeanChangeUserPasswordDto input)
  {
    try
    {
      var user = await GetUserByIdAsync(input.Id);
      if (user == null)
      {
        return LeanApiResult.Error($"用户 {input.Id} 不存在");
      }

      if (!LeanPassword.VerifyPassword(input.OldPassword, user.Salt, user.Password))
      {
        return LeanApiResult.Error("旧密码不正确");
      }

      await UpdateUserPasswordAsync(user, input.NewPassword);

      return LeanApiResult.Ok();
    }
    catch (Exception ex)
    {
      return LeanApiResult.Error($"修改密码失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 导入用户
  /// </summary>
  public async Task<LeanApiResult<LeanImportUserResultDto>> ImportAsync(List<LeanImportTemplateUserDto> users)
  {
    var result = new LeanImportUserResultDto();

    try
    {
      foreach (var user in users)
      {
        try
        {
          if (!await ValidateUserInputAsync(user.UserName, user.Email, user.PhoneNumber))
          {
            result.AddError(user.UserName, "输入参数验证失败");
            continue;
          }

          var newUser = user.Adapt<LeanUser>();
          newUser.CreateTime = DateTime.Now;
          newUser.Salt = LeanPassword.GenerateSalt();
          newUser.Password = LeanPassword.HashPassword(SecurityOptions.DefaultPassword, newUser.Salt);

          await _userRepository.CreateAsync(newUser);
          result.SuccessCount++;
          result.TotalCount++;
        }
        catch (Exception ex)
        {
          result.AddError(user.UserName, ex.Message);
        }
      }

      return LeanApiResult<LeanImportUserResultDto>.Ok(result);
    }
    catch (Exception ex)
    {
      return LeanApiResult<LeanImportUserResultDto>.Error($"导入用户失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 导出用户
  /// </summary>
  public async Task<LeanApiResult<byte[]>> ExportAsync(LeanExportUserDto input)
  {
    try
    {
      Expression<Func<LeanUser, bool>> predicate = BuildUserQueryPredicate(input);

      if (!input.IsExportAll && input.SelectedIds.Any())
      {
        predicate = LeanExpressionExtensions.And(predicate, u => input.SelectedIds.Contains(u.Id));
      }

      var (total, items) = await _userRepository.GetPageListAsync(predicate, input.PageSize, input.PageIndex);
      var userDtos = items.Select(u => u.Adapt<LeanUserDto>()).ToList();

      foreach (var item in userDtos)
      {
        await FillUserRelationsAsync(item);
      }

      var bytes = await ExportToExcel(userDtos);
      return LeanApiResult<byte[]>.Ok(bytes);
    }
    catch (Exception ex)
    {
      return LeanApiResult<byte[]>.Error($"导出用户失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 获取用户的所有权限（包括角色继承的权限）
  /// </summary>
  public async Task<LeanApiResult<LeanUserPermissionsDto>> GetUserAllPermissionsAsync(long userId)
  {
    try
    {
      var user = await GetUserByIdAsync(userId);
      if (user == null)
      {
        return LeanApiResult<LeanUserPermissionsDto>.Error($"用户 {userId} 不存在");
      }

      // 获取用户的角色
      var userRoles = await _userRoleRepository.GetListAsync(ur => ur.UserId == userId);
      var roleIds = userRoles.Select(ur => ur.RoleId).ToList();

      // 获取用户的部门
      var userDepts = await _userDeptRepository.GetListAsync(ud => ud.UserId == userId);
      var deptIds = userDepts.Select(ud => ud.DeptId).ToList();

      // 获取用户的岗位
      var userPosts = await _userPostRepository.GetListAsync(up => up.UserId == userId);
      var postIds = userPosts.Select(up => up.PostId).ToList();

      var result = new LeanUserPermissionsDto
      {
        UserId = userId,
        RoleIds = roleIds,
        DeptIds = deptIds,
        PostIds = postIds,
        MenuIds = new List<long>(),
        ApiIds = new List<long>()
      };

      return LeanApiResult<LeanUserPermissionsDto>.Ok(result);
    }
    catch (Exception ex)
    {
      return LeanApiResult<LeanUserPermissionsDto>.Error($"获取用户权限失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 验证用户是否有指定权限
  /// </summary>
  public async Task<LeanApiResult<bool>> ValidateUserPermissionAsync(string permission, long userId)
  {
    try
    {
      var user = await GetUserByIdAsync(userId);
      if (user == null)
      {
        return LeanApiResult<bool>.Error($"用户 {userId} 不存在");
      }

      // 获取用户的所有权限
      var permissionsResult = await GetUserAllPermissionsAsync(userId);
      if (!permissionsResult.Success)
      {
        return LeanApiResult<bool>.Error(permissionsResult.Message);
      }

      // TODO: 根据权限编码判断用户是否有权限
      var hasPermission = true; // 临时返回true，需要实现具体的权限判断逻辑

      return LeanApiResult<bool>.Ok(hasPermission);
    }
    catch (Exception ex)
    {
      return LeanApiResult<bool>.Error($"验证用户权限失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 批量验证用户权限
  /// </summary>
  public async Task<LeanApiResult<Dictionary<string, bool>>> ValidateUserPermissionsAsync(List<string> permissions, long userId)
  {
    try
    {
      var result = new Dictionary<string, bool>();
      foreach (var permission in permissions)
      {
        var validationResult = await ValidateUserPermissionAsync(permission, userId);
        if (!validationResult.Success)
        {
          return LeanApiResult<Dictionary<string, bool>>.Error(validationResult.Message);
        }
        result[permission] = validationResult.Data;
      }

      return LeanApiResult<Dictionary<string, bool>>.Ok(result);
    }
    catch (Exception ex)
    {
      return LeanApiResult<Dictionary<string, bool>>.Error($"批量验证用户权限失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 验证用户是否有权限访问指定资源
  /// </summary>
  public async Task<LeanApiResult<bool>> ValidateUserResourceAccessAsync(LeanValidateUserResourceAccessDto input)
  {
    try
    {
      var user = await GetUserByIdAsync(input.UserId);
      if (user == null)
      {
        return LeanApiResult<bool>.Error($"用户 {input.UserId} 不存在");
      }

      // 获取用户的所有权限
      var permissionsResult = await GetUserAllPermissionsAsync(input.UserId);
      if (!permissionsResult.Success)
      {
        return LeanApiResult<bool>.Error(permissionsResult.Message);
      }

      // TODO: 根据资源类型和操作类型判断用户是否有权限访问
      var hasPermission = true; // 临时返回true，需要实现具体的权限判断逻辑

      return LeanApiResult<bool>.Ok(hasPermission);
    }
    catch (Exception ex)
    {
      return LeanApiResult<bool>.Error($"验证用户资源访问权限失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 获取用户直接分配的菜单权限列表
  /// </summary>
  public async Task<LeanApiResult<List<long>>> GetUserDirectMenusAsync(long userId)
  {
    try
    {
      var user = await GetUserByIdAsync(userId);
      if (user == null)
      {
        return LeanApiResult<List<long>>.Error($"用户 {userId} 不存在");
      }

      // TODO: 从用户-菜单关联表中获取用户直接分配的菜单ID列表
      var menuIds = new List<long>();

      return LeanApiResult<List<long>>.Ok(menuIds);
    }
    catch (Exception ex)
    {
      return LeanApiResult<List<long>>.Error($"获取用户直接菜单权限失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 分配用户直接菜单权限
  /// </summary>
  public async Task<LeanApiResult> AssignUserMenusAsync(LeanAssignUserMenuDto input)
  {
    try
    {
      var user = await GetUserByIdAsync(input.UserId);
      if (user == null)
      {
        return LeanApiResult.Error($"用户 {input.UserId} 不存在");
      }

      // TODO: 更新用户-菜单关联表

      return LeanApiResult.Ok();
    }
    catch (Exception ex)
    {
      return LeanApiResult.Error($"分配用户菜单权限失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 获取用户直接分配的API权限列表
  /// </summary>
  public async Task<LeanApiResult<List<long>>> GetUserDirectApisAsync(long userId)
  {
    try
    {
      var user = await GetUserByIdAsync(userId);
      if (user == null)
      {
        return LeanApiResult<List<long>>.Error($"用户 {userId} 不存在");
      }

      // TODO: 从用户-API关联表中获取用户直接分配的API ID列表
      var apiIds = new List<long>();

      return LeanApiResult<List<long>>.Ok(apiIds);
    }
    catch (Exception ex)
    {
      return LeanApiResult<List<long>>.Error($"获取用户直接API权限失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 分配用户直接API权限
  /// </summary>
  public async Task<LeanApiResult> AssignUserApisAsync(LeanAssignUserApiDto input)
  {
    try
    {
      var user = await GetUserByIdAsync(input.UserId);
      if (user == null)
      {
        return LeanApiResult.Error($"用户 {input.UserId} 不存在");
      }

      // TODO: 更新用户-API关联表

      return LeanApiResult.Ok();
    }
    catch (Exception ex)
    {
      return LeanApiResult.Error($"分配用户API权限失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 获取当前登录用户信息
  /// </summary>
  public async Task<LeanApiResult<LeanUserDto>> GetCurrentUserInfoAsync()
  {
    try
    {
      var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      if (string.IsNullOrEmpty(userId) || !long.TryParse(userId, out var id))
      {
        return LeanApiResult<LeanUserDto>.Error("未获取到用户信息");
      }

      // 获取用户基本信息
      var user = await GetUserByIdAsync(id);
      if (user == null)
      {
        return LeanApiResult<LeanUserDto>.Error("用户不存在");
      }

      if (user.UserStatus != LeanUserStatus.Normal)
      {
        return LeanApiResult<LeanUserDto>.Error("用户状态异常");
      }

      // 转换为DTO
      var result = user.Adapt<LeanUserDto>();

      // 填充关联信息
      await FillUserRelationsAsync(result);

      // 获取登录扩展信息
      var loginExtend = await _loginExtendRepository.FirstOrDefaultAsync(le => le.UserId == id);
      if (loginExtend != null)
      {
        result.LastLoginInfo = new LeanUserLoginInfo
        {
          LastLoginIp = loginExtend.LastLoginIp,
          LastLoginTime = loginExtend.LastLoginTime,
          LastLoginLocation = loginExtend.LastLoginLocation
        };
      }

      return LeanApiResult<LeanUserDto>.Ok(result);
    }
    catch (Exception ex)
    {
      return LeanApiResult<LeanUserDto>.Error($"获取当前用户信息失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 用户登录
  /// </summary>
  public async Task<LeanLoginResultDto> LoginAsync(LeanLoginDto input)
  {
    try
    {
      // 查找用户
      var user = await _userRepository.FirstOrDefaultAsync(u => u.UserName == input.UserName);
      if (user == null)
      {
        throw new LeanException("用户名或密码错误");
      }

      // 获取登录扩展信息
      var loginExtend = await _loginExtendRepository.FirstOrDefaultAsync(le => le.UserId == user.Id);
      if (loginExtend == null)
      {
        loginExtend = new LeanLoginExtend
        {
          UserId = user.Id,
          LoginStatus = LeanLoginStatus.Normal
        };
        await _loginExtendRepository.CreateAsync(loginExtend);
      }

      // 检查用户状态
      if (loginExtend.LoginStatus != LeanLoginStatus.Normal)
      {
        throw new LeanException("账户已被锁定或禁用");
      }

      // 检查密码错误次数
      if (loginExtend.PasswordErrorCount >= _securityOptions.Value.MaxPasswordAttempts)
      {
        var lockEndTime = loginExtend.LastPasswordErrorTime?.AddMinutes(_securityOptions.Value.PasswordLockMinutes);
        if (lockEndTime.HasValue && DateTime.Now < lockEndTime)
        {
          var remainingMinutes = (lockEndTime.Value - DateTime.Now).TotalMinutes;
          throw new LeanException($"密码错误次数过多，账号已被锁定，请{remainingMinutes:F0}分钟后重试");
        }
      }

      // 验证密码
      var password = LeanPassword.HashPassword(input.Password, user.Salt);
      if (password != user.Password)
      {
        loginExtend.PasswordErrorCount++;
        if (loginExtend.PasswordErrorCount >= _securityOptions.Value.MaxPasswordAttempts)
        {
          loginExtend.LastPasswordErrorTime = DateTime.Now;
          loginExtend.LoginStatus = LeanLoginStatus.Locked;
        }
        await _loginExtendRepository.UpdateAsync(loginExtend);
        throw new LeanException("用户名或密码错误");
      }

      // 更新登录信息
      loginExtend.PasswordErrorCount = 0;
      loginExtend.LastLoginTime = DateTime.Now;
      loginExtend.LastLoginIp = input.LoginIp;
      loginExtend.LastLoginLocation = input.LoginLocation;
      loginExtend.LastLoginType = LeanLoginType.Password;
      loginExtend.LastBrowser = input.Browser;
      loginExtend.LastOs = input.Os;

      // 如果是首次登录，记录首次登录信息
      if (!loginExtend.FirstLoginTime.HasValue)
      {
        loginExtend.FirstLoginTime = DateTime.Now;
        loginExtend.FirstLoginIp = input.LoginIp;
        loginExtend.FirstLoginLocation = input.LoginLocation;
        loginExtend.FirstLoginType = LeanLoginType.Password;
        loginExtend.FirstBrowser = input.Browser;
        loginExtend.FirstOs = input.Os;
      }

      await _loginExtendRepository.UpdateAsync(loginExtend);

      // 更新设备信息
      if (!string.IsNullOrEmpty(input.DeviceId))
      {
        var device = await _deviceExtendRepository.FirstOrDefaultAsync(d => d.DeviceId == input.DeviceId);
        if (device == null)
        {
          device = new LeanDeviceExtend
          {
            UserId = user.Id,
            DeviceId = input.DeviceId,
            DeviceType = LeanDeviceType.Other,
            DeviceStatus = LeanDeviceStatus.Normal,
            LastLoginTime = DateTime.Now,
            LastLoginIp = input.LoginIp,
            Browser = input.Browser,
            Os = input.Os
          };
          await _deviceExtendRepository.CreateAsync(device);
        }
        else
        {
          device.LastLoginTime = DateTime.Now;
          device.LastLoginIp = input.LoginIp;
          device.Browser = input.Browser;
          device.Os = input.Os;
          await _deviceExtendRepository.UpdateAsync(device);
        }
      }

      // 生成令牌
      var claims = new[]
      {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim(ClaimTypes.GivenName, user.RealName),
        new Claim("UserType", user.UserType.ToString())
      };

      return new LeanLoginResultDto
      {
        AccessToken = _tokenService.CreateToken(claims),
        RefreshToken = _tokenService.CreateRefreshToken(),
        ExpiresIn = _securityOptions.Value.Jwt.ExpireMinutes * 60,
        User = user.Adapt<LeanUserDto>()
      };
    }
    catch (Exception ex)
    {
      throw new LeanException($"登录失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 创建用户关联信息
  /// </summary>
  private async Task CreateUserRelationsAsync(long userId, LeanCreateUserDto input)
  {
    await UpdateUserRolesAsync(userId, input.RoleIds);
    await UpdateUserDeptsAsync(userId, input.DeptIds, input.PrimaryDeptId);
    await UpdateUserPostsAsync(userId, input.PostIds, input.PrimaryPostId);
  }

  /// <summary>
  /// 验证用户输入
  /// </summary>
  private async Task<bool> ValidateUserInputAsync(LeanCreateUserDto input)
  {
    // 确保非空参数不为 null
    if (string.IsNullOrEmpty(input.UserName))
    {
      return false;
    }

    // 过滤掉可能为 null 的参数
    var inputs = new[] { input.UserName }
        .Concat(input.Email != null ? new[] { input.Email } : Array.Empty<string>())
        .Concat(input.PhoneNumber != null ? new[] { input.PhoneNumber } : Array.Empty<string>())
        .ToArray();

    if (!ValidateInput(inputs))
    {
      return false;
    }

    try
    {
      await _uniqueValidator.ValidateAsync(
          (u => u.UserName, input.UserName, null, $"用户名 {input.UserName} 已存在"),
          (u => u.PhoneNumber, input.PhoneNumber, null, input.PhoneNumber != null ? $"手机号 {input.PhoneNumber} 已存在" : null),
          (u => u.Email, input.Email, null, input.Email != null ? $"邮箱 {input.Email} 已存在" : null)
      );
      return true;
    }
    catch
    {
      return false;
    }
  }

  /// <summary>
  /// 验证用户输入（更新时）
  /// </summary>
  private async Task<bool> ValidateUserInputAsync(LeanUpdateUserDto input, long userId)
  {
    // 确保非空参数不为 null
    if (string.IsNullOrEmpty(input.UserName))
    {
      return false;
    }

    // 过滤掉可能为 null 的参数
    var inputs = new[] { input.UserName }
        .Concat(input.Email != null ? new[] { input.Email } : Array.Empty<string>())
        .Concat(input.PhoneNumber != null ? new[] { input.PhoneNumber } : Array.Empty<string>())
        .ToArray();

    if (!ValidateInput(inputs))
    {
      return false;
    }

    try
    {
      await _uniqueValidator.ValidateAsync(
          (u => u.UserName, input.UserName, userId, $"用户名 {input.UserName} 已存在"),
          (u => u.PhoneNumber, input.PhoneNumber, userId, input.PhoneNumber != null ? $"手机号 {input.PhoneNumber} 已存在" : null),
          (u => u.Email, input.Email, userId, input.Email != null ? $"邮箱 {input.Email} 已存在" : null)
      );
      return true;
    }
    catch
    {
      return false;
    }
  }

  /// <summary>
  /// 验证用户输入（导入时）
  /// </summary>
  private async Task<bool> ValidateUserInputAsync(string userName, string? email, string? phoneNumber)
  {
    // 确保非空参数不为 null
    if (string.IsNullOrEmpty(userName))
    {
      return false;
    }

    // 过滤掉可能为 null 的参数
    var inputs = new[] { userName }
        .Concat(email != null ? new[] { email } : Array.Empty<string>())
        .Concat(phoneNumber != null ? new[] { phoneNumber } : Array.Empty<string>())
        .ToArray();

    if (!ValidateInput(inputs))
    {
      return false;
    }

    try
    {
      await _uniqueValidator.ValidateAsync(
          (u => u.UserName, userName, null, $"用户名 {userName} 已存在"),
          (u => u.PhoneNumber, phoneNumber, null, phoneNumber != null ? $"手机号 {phoneNumber} 已存在" : null),
          (u => u.Email, email, null, email != null ? $"邮箱 {email} 已存在" : null)
      );
      return true;
    }
    catch
    {
      return false;
    }
  }

  /// <summary>
  /// 更新用户关联信息
  /// </summary>
  private async Task UpdateUserRelationsAsync(long userId, LeanUpdateUserDto input)
  {
    await UpdateUserRolesAsync(userId, input.RoleIds);
    await UpdateUserDeptsAsync(userId, input.DeptIds, input.PrimaryDeptId);
    await UpdateUserPostsAsync(userId, input.PostIds, input.PrimaryPostId);
  }

  /// <summary>
  /// 更新用户角色关系
  /// </summary>
  private async Task UpdateUserRolesAsync(long userId, List<long> roleIds)
  {
    await _userRoleRepository.DeleteAsync(ur => ur.UserId == userId);
    if (roleIds?.Any() == true)
    {
      var userRoles = roleIds.Select(roleId => new LeanUserRole
      {
        UserId = userId,
        RoleId = roleId,
        CreateTime = DateTime.Now
      }).ToList();
      await _userRoleRepository.CreateRangeAsync(userRoles);
    }
  }

  /// <summary>
  /// 更新用户部门关系
  /// </summary>
  private async Task UpdateUserDeptsAsync(long userId, List<long> deptIds, long primaryDeptId)
  {
    await _userDeptRepository.DeleteAsync(ud => ud.UserId == userId);
    if (deptIds?.Any() == true)
    {
      var userDepts = deptIds.Select(deptId => new LeanUserDept
      {
        UserId = userId,
        DeptId = deptId,
        IsPrimary = deptId == primaryDeptId ? LeanPrimaryStatus.Yes : LeanPrimaryStatus.No,
        CreateTime = DateTime.Now
      }).ToList();
      await _userDeptRepository.CreateRangeAsync(userDepts);
    }
  }

  /// <summary>
  /// 更新用户岗位关系
  /// </summary>
  private async Task UpdateUserPostsAsync(long userId, List<long> postIds, long primaryPostId)
  {
    await _userPostRepository.DeleteAsync(up => up.UserId == userId);
    if (postIds?.Any() == true)
    {
      var userPosts = postIds.Select(postId => new LeanUserPost
      {
        UserId = userId,
        PostId = postId,
        IsPrimary = postId == primaryPostId ? LeanPrimaryStatus.Yes : LeanPrimaryStatus.No,
        CreateTime = DateTime.Now
      }).ToList();
      await _userPostRepository.CreateRangeAsync(userPosts);
    }
  }

  /// <summary>
  /// 删除用户关联信息
  /// </summary>
  private async Task DeleteUserRelationsAsync(long userId)
  {
    await _userRoleRepository.DeleteAsync(ur => ur.UserId == userId);
    await _userDeptRepository.DeleteAsync(ud => ud.UserId == userId);
    await _userPostRepository.DeleteAsync(up => up.UserId == userId);
  }

  /// <summary>
  /// 填充用户关联信息
  /// </summary>
  private async Task FillUserRelationsAsync(LeanUserDto user)
  {
    var userRoles = await _userRoleRepository.GetListAsync(ur => ur.UserId == user.Id);
    user.RoleIds = userRoles.Select(ur => ur.RoleId).ToList();

    var userDepts = await _userDeptRepository.GetListAsync(ud => ud.UserId == user.Id);
    user.DeptIds = userDepts.Select(ud => ud.DeptId).ToList();
    user.PrimaryDeptId = userDepts.FirstOrDefault(ud => ud.IsPrimary == LeanPrimaryStatus.Yes)?.DeptId;

    var userPosts = await _userPostRepository.GetListAsync(up => up.UserId == user.Id);
    user.PostIds = userPosts.Select(up => up.PostId).ToList();
    user.PrimaryPostId = userPosts.FirstOrDefault(up => up.IsPrimary == LeanPrimaryStatus.Yes)?.PostId;
  }

  /// <summary>
  /// 构建用户查询条件
  /// </summary>
  private Expression<Func<LeanUser, bool>> BuildUserQueryPredicate(LeanQueryUserDto input)
  {
    Expression<Func<LeanUser, bool>> predicate = u => true;

    if (!string.IsNullOrEmpty(input.UserName))
    {
      var userName = CleanInput(input.UserName);
      predicate = LeanExpressionExtensions.And(predicate, u => u.UserName.Contains(userName));
    }

    if (!string.IsNullOrEmpty(input.RealName))
    {
      var realName = CleanInput(input.RealName);
      predicate = LeanExpressionExtensions.And(predicate, u => u.RealName.Contains(realName));
    }

    if (!string.IsNullOrEmpty(input.PhoneNumber))
    {
      var phoneNumber = CleanInput(input.PhoneNumber);
      if (phoneNumber != null)  // 添加 null 检查
      {
        predicate = LeanExpressionExtensions.And(predicate, u => u.PhoneNumber != null && u.PhoneNumber.Contains(phoneNumber));
      }
    }

    if (input.UserStatus.HasValue)
    {
      predicate = LeanExpressionExtensions.And(predicate, u => u.UserStatus == input.UserStatus.Value);
    }

    if (input.StartTime.HasValue)
    {
      predicate = LeanExpressionExtensions.And(predicate, u => u.CreateTime >= input.StartTime.Value);
    }

    if (input.EndTime.HasValue)
    {
      predicate = LeanExpressionExtensions.And(predicate, u => u.CreateTime <= input.EndTime.Value);
    }

    return predicate;
  }

  /// <summary>
  /// 更新用户密码
  /// </summary>
  private async Task UpdateUserPasswordAsync(LeanUser user, string newPassword)
  {
    var (isValid, message) = LeanPassword.ValidatePasswordStrength(newPassword);
    if (!isValid)
    {
      throw new Exception(message);
    }

    user.Salt = LeanPassword.GenerateSalt();
    user.Password = LeanPassword.HashPassword(newPassword, user.Salt);
    user.UpdateTime = DateTime.Now;

    await _userRepository.UpdateAsync(user);
  }

  /// <summary>
  /// 导出到Excel
  /// </summary>
  private async Task<byte[]> ExportToExcel(List<LeanUserDto> users)
  {
    // TODO: 实现Excel导出
    await Task.CompletedTask; // 添加 await 以避免 CS1998 警告
    return Array.Empty<byte>();
  }

  /// <summary>
  /// 获取用户
  /// </summary>
  private async Task<LeanUser?> GetUserByIdAsync(long id)
  {
    return await _userRepository.GetByIdAsync(id);
  }
}