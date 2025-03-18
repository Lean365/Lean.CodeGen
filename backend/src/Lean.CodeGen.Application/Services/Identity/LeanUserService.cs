using Lean.CodeGen.Application.Dtos.Identity;
using Lean.CodeGen.Application.Services.Base;
using Lean.CodeGen.Common.Excel;
using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Common.Exceptions;
using Lean.CodeGen.Common.Extensions;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Domain.Entities;
using Lean.CodeGen.Domain.Entities.Identity;
using Lean.CodeGen.Domain.Interfaces.Repositories;
using Lean.CodeGen.Domain.Validators;
using Mapster;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Lean.CodeGen.Common.Security;

namespace Lean.CodeGen.Application.Services.Identity
{
  /// <summary>
  /// 用户服务实现
  /// </summary>
  /// <remarks>
  /// 提供用户管理相关的业务功能，包括：
  /// 1. 用户的增删改查
  /// 2. 用户状态管理
  /// 3. 密码管理
  /// 4. 用户导入导出
  /// 5. 用户角色和部门关联管理
  /// </remarks>
  public class LeanUserService : LeanBaseService, ILeanUserService
  {
    /// <summary>
    /// 用户仓储接口
    /// </summary>
    /// <remarks>
    /// 用于用户实体的持久化操作
    /// </remarks>
    private readonly ILeanRepository<LeanUser> _userRepository;

    /// <summary>
    /// 用户部门关联仓储接口
    /// </summary>
    /// <remarks>
    /// 用于管理用户与部门的关联关系
    /// </remarks>
    private readonly ILeanRepository<LeanUserDept> _userDeptRepository;

    /// <summary>
    /// 用户角色关联仓储接口
    /// </summary>
    /// <remarks>
    /// 用于管理用户与角色的关联关系
    /// </remarks>
    private readonly ILeanRepository<LeanUserRole> _userRoleRepository;

    /// <summary>
    /// 用户登录扩展关联仓储接口
    /// </summary>
    /// <remarks>
    /// 用于管理用户登录扩展的关联关系
    /// </remarks>
    private readonly ILeanRepository<LeanLoginExtend> _loginExtendRepository;

    /// <summary>
    /// 用户唯一性验证器
    /// </summary>
    /// <remarks>
    /// 用于验证用户名、邮箱、手机号等字段的唯一性
    /// </remarks>
    private readonly LeanUniqueValidator<LeanUser> _uniqueValidator;

    /// <summary>
    /// 日志记录器
    /// </summary>
    private readonly ILogger _logger;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="userRepository">用户仓储接口</param>
    /// <param name="userDeptRepository">用户部门关联仓储接口</param>
    /// <param name="userRoleRepository">用户角色关联仓储接口</param>
    /// <param name="loginExtendRepository">用户登录扩展关联仓储接口</param>
    /// <param name="context">基础服务上下文</param>
    /// <param name="logger">日志记录器</param>
    public LeanUserService(
        ILeanRepository<LeanUser> userRepository,
        ILeanRepository<LeanUserDept> userDeptRepository,
        ILeanRepository<LeanUserRole> userRoleRepository,
        ILeanRepository<LeanLoginExtend> loginExtendRepository,
        ILogger logger,
        LeanBaseServiceContext context)
        : base(context)
    {
      _userRepository = userRepository;
      _userDeptRepository = userDeptRepository;
      _userRoleRepository = userRoleRepository;
      _loginExtendRepository = loginExtendRepository;
      _logger = logger;
      _uniqueValidator = new LeanUniqueValidator<LeanUser>(_userRepository);
    }

    /// <summary>
    /// 创建用户
    /// </summary>
    /// <param name="input">用户创建参数</param>
    /// <returns>创建成功的用户信息</returns>
    /// <remarks>
    /// 创建新用户时会进行以下操作：
    /// 1. 验证用户名唯一性
    /// 2. 验证邮箱唯一性（如果提供）
    /// 3. 验证手机号唯一性（如果提供）
    /// 4. 设置默认密码
    /// 5. 生成密码盐值
    /// 6. 记录审计日志
    /// </remarks>
    public async Task<LeanUserDto> CreateAsync(LeanUserCreateDto input)
    {
      return await ExecuteInTransactionAsync(async () =>
      {
        // 验证用户名唯一性
        await _uniqueValidator.ValidateAsync(x => x.UserName, input.UserName);

        // 验证邮箱唯一性
        if (!string.IsNullOrEmpty(input.Email))
        {
          await _uniqueValidator.ValidateAsync(x => x.Email, input.Email);
        }

        // 验证手机号唯一性
        if (!string.IsNullOrEmpty(input.PhoneNumber))
        {
          await _uniqueValidator.ValidateAsync(x => x.PhoneNumber, input.PhoneNumber);
        }

        // 创建用户实体
        var user = input.Adapt<LeanUser>();
        var (hashedPassword, salt) = LeanPassword.CreatePassword(SecurityOptions.DefaultPassword);
        user.Password = hashedPassword;
        user.Salt = salt;

        // 插入用户记录
        await _userRepository.CreateAsync(user);

        LogAudit("CreateUser", $"创建用户: {user.UserName}");
        return await GetAsync(user.Id);
      }, "创建用户");
    }

    /// <summary>
    /// 更新用户
    /// </summary>
    /// <param name="input">用户更新参数</param>
    /// <returns>更新后的用户信息</returns>
    /// <remarks>
    /// 更新用户信息时会进行以下操作：
    /// 1. 检查用户是否存在
    /// 2. 检查是否为内置用户
    /// 3. 验证邮箱唯一性（如果更新）
    /// 4. 验证手机号唯一性（如果更新）
    /// 5. 记录审计日志
    /// </remarks>
    public async Task<LeanUserDto> UpdateAsync(LeanUserUpdateDto input)
    {
      return await ExecuteInTransactionAsync(async () =>
      {
        var user = await _userRepository.GetByIdAsync(input.Id);
        if (user == null)
        {
          throw new LeanException("用户不存在");
        }

        if (user.IsBuiltin == 1)
        {
          throw new LeanException("内置用户不能修改");
        }

        // 验证邮箱唯一性
        if (!string.IsNullOrEmpty(input.Email))
        {
          await _uniqueValidator.ValidateAsync(x => x.Email, input.Email, input.Id);
        }

        // 验证手机号唯一性
        if (!string.IsNullOrEmpty(input.PhoneNumber))
        {
          await _uniqueValidator.ValidateAsync(x => x.PhoneNumber, input.PhoneNumber, input.Id);
        }

        // 更新用户信息
        input.Adapt(user);
        await _userRepository.UpdateAsync(user);

        LogAudit("UpdateUser", $"更新用户: {user.UserName}");
        return await GetAsync(user.Id);
      }, "更新用户");
    }

    /// <summary>
    /// 删除用户
    /// </summary>
    /// <param name="input">用户删除参数</param>
    /// <remarks>
    /// 删除用户时会进行以下操作：
    /// 1. 检查用户是否存在
    /// 2. 检查是否为内置用户
    /// 3. 删除用户部门关联
    /// 4. 删除用户角色关联
    /// 5. 删除用户记录
    /// 6. 记录审计日志
    /// </remarks>
    public async Task DeleteAsync(LeanUserDeleteDto input)
    {
      await ExecuteInTransactionAsync(async () =>
      {
        var user = await _userRepository.GetByIdAsync(input.Id);
        if (user == null)
        {
          throw new LeanException("用户不存在");
        }

        if (user.IsBuiltin == 1)
        {
          throw new LeanException("内置用户不能删除");
        }

        // 删除用户部门关联
        await _userDeptRepository.DeleteAsync(x => x.UserId == input.Id);

        // 删除用户角色关联
        await _userRoleRepository.DeleteAsync(x => x.UserId == input.Id);

        // 删除用户
        await _userRepository.DeleteAsync(user);

        LogAudit("DeleteUser", $"删除用户: {user.UserName}");
      }, "删除用户");
    }

    /// <summary>
    /// 获取用户信息
    /// </summary>
    /// <param name="id">用户ID</param>
    /// <returns>用户详细信息</returns>
    /// <remarks>
    /// 获取用户信息时会包含：
    /// 1. 用户基本信息
    /// 2. 用户关联的部门ID列表
    /// 3. 用户的主部门ID
    /// 4. 用户关联的角色ID列表
    /// </remarks>
    public async Task<LeanUserDto> GetAsync(long id)
    {
      var user = await _userRepository.GetByIdAsync(id);
      if (user == null)
      {
        throw new LeanException("用户不存在");
      }

      var result = user.Adapt<LeanUserDto>();

      // 获取用户部门
      var userDepts = await _userDeptRepository.GetListAsync(x => x.UserId == id);
      result.DeptIds = userDepts.Select(x => x.DeptId).ToList();
      result.PrimaryDeptId = userDepts.FirstOrDefault(x => x.IsPrimary == 1)?.DeptId;

      // 获取用户角色
      var userRoles = await _userRoleRepository.GetListAsync(x => x.UserId == id);
      result.RoleIds = userRoles.Select(x => x.RoleId).ToList();

      return result;
    }

    /// <summary>
    /// 分页查询用户
    /// </summary>
    /// <param name="input">查询参数</param>
    /// <returns>分页查询结果</returns>
    /// <remarks>
    /// 支持以下查询条件：
    /// 1. 用户名（模糊匹配）
    /// 2. 真实姓名（模糊匹配）
    /// 3. 手机号（模糊匹配）
    /// 4. 用户状态
    /// 5. 创建时间范围
    /// 查询结果按创建时间倒序排序
    /// </remarks>
    public async Task<LeanPageResult<LeanUserDto>> QueryAsync(LeanUserQueryDto input)
    {
      using (LogPerformance("查询用户列表"))
      {
        // 构建查询条件
        Expression<Func<LeanUser, bool>> predicate = x => true;

        if (!string.IsNullOrEmpty(input.UserName))
        {
          var userName = CleanInput(input.UserName);
          predicate = predicate.And(x => x.UserName.Contains(userName));
        }

        if (!string.IsNullOrEmpty(input.RealName))
        {
          var realName = CleanInput(input.RealName);
          predicate = predicate.And(x => x.RealName.Contains(realName));
        }

        if (!string.IsNullOrEmpty(input.PhoneNumber))
        {
          var phoneNumber = CleanInput(input.PhoneNumber);
          predicate = predicate.And(x => x.PhoneNumber.Contains(phoneNumber));
        }

        if (input.UserStatus.HasValue)
        {
          predicate = predicate.And(x => x.UserStatus == input.UserStatus);
        }

        if (input.StartTime.HasValue)
        {
          predicate = predicate.And(x => x.CreateTime >= input.StartTime);
        }

        if (input.EndTime.HasValue)
        {
          predicate = predicate.And(x => x.CreateTime <= input.EndTime);
        }

        // 执行分页查询
        var result = await _userRepository.GetPageListAsync(
            predicate,
            input.PageSize,
            input.PageIndex,
            x => x.CreateTime,
            false);

        var list = result.Items.Adapt<List<LeanUserDto>>();
        return new LeanPageResult<LeanUserDto>
        {
          Total = result.Total,
          Items = list,
          PageIndex = input.PageIndex,
          PageSize = input.PageSize
        };
      }
    }

    /// <summary>
    /// 修改用户状态
    /// </summary>
    /// <param name="input">状态修改参数</param>
    /// <remarks>
    /// 修改用户状态时会进行以下操作：
    /// 1. 检查用户是否存在
    /// 2. 检查是否为内置用户
    /// 3. 更新用户状态
    /// 4. 记录审计日志
    /// </remarks>
    public async Task ChangeStatusAsync(LeanUserChangeStatusDto input)
    {
      await ExecuteInTransactionAsync(async () =>
      {
        var user = await _userRepository.GetByIdAsync(input.Id);
        if (user == null)
        {
          throw new LeanException("用户不存在");
        }

        if (user.IsBuiltin == 1)
        {
          throw new LeanException("内置用户不能修改状态");
        }

        // 更新状态
        user.UserStatus = input.UserStatus;
        await _userRepository.UpdateAsync(user);
        LogAudit("ChangeUserStatus", $"修改用户状态: {user.UserName}, 状态: {input.UserStatus}");
      }, "修改用户状态");
    }

    /// <summary>
    /// 重置用户密码
    /// </summary>
    /// <param name="input">重置密码参数</param>
    /// <returns>操作结果</returns>
    public async Task ResetPasswordAsync(LeanUserResetPasswordDto input)
    {
      try
      {
        var loginExtend = await _loginExtendRepository.FirstOrDefaultAsync(l => l.UserId == input.Id);
        if (loginExtend == null)
        {
          throw new LeanException("用户不存在");
        }

        // 更新用户密码
        var user = await _userRepository.GetByIdAsync(input.Id);
        if (user == null)
        {
          throw new LeanException("用户不存在");
        }

        // 验证新密码强度
        var (isValid, message) = LeanPassword.ValidatePasswordStrength(input.NewPassword);
        if (!isValid)
        {
          throw new LeanException($"新密码不符合要求: {message}");
        }

        // 生成新的盐值和密码
        var (hashedPassword, salt) = LeanPassword.CreatePassword(input.NewPassword);
        user.Password = hashedPassword;
        user.Salt = salt;
        await _userRepository.UpdateAsync(user);

        // 重置登录扩展信息
        loginExtend.PasswordErrorCount = 0;
        loginExtend.LockStatus = 0;
        loginExtend.UnlockTime = null;
        await _loginExtendRepository.UpdateAsync(loginExtend);

        LogAudit("ResetUserPassword", $"重置用户密码: {user.UserName}");
      }
      catch (Exception ex)
      {
        Logger.Error(ex, "重置用户密码失败");
        throw new LeanException($"重置用户密码失败：{ex.Message}");
      }
    }

    /// <summary>
    /// 修改用户密码
    /// </summary>
    /// <param name="input">修改密码参数</param>
    /// <remarks>
    /// 修改用户密码时会进行以下操作：
    /// 1. 检查用户是否存在
    /// 2. 检查是否为内置用户
    /// 3. 验证旧密码（TODO）
    /// 4. 更新新密码
    /// 5. 生成新的密码盐值
    /// 6. 记录审计日志
    /// </remarks>
    public async Task ChangePasswordAsync(LeanUserChangePasswordDto input)
    {
      await ExecuteInTransactionAsync(async () =>
      {
        var user = await _userRepository.GetByIdAsync(input.Id);
        if (user == null)
        {
          throw new LeanException("用户不存在");
        }

        if (user.IsBuiltin == 1)
        {
          throw new LeanException("内置用户不能修改密码");
        }

        // 验证旧密码
        if (!LeanPassword.VerifyPassword(input.OldPassword, user.Password, user.Salt))
        {
          throw new LeanException("旧密码不正确");
        }

        // 验证新密码强度
        var (isValid, message) = LeanPassword.ValidatePasswordStrength(input.NewPassword);
        if (!isValid)
        {
          throw new LeanException($"新密码不符合要求: {message}");
        }

        var (hashedPassword, salt) = LeanPassword.CreatePassword(input.NewPassword);
        user.Password = hashedPassword;
        user.Salt = salt;
        await _userRepository.UpdateAsync(user);

        LogAudit("ChangeUserPassword", $"修改用户密码: {user.UserName}");
      }, "修改用户密码");
    }

    /// <summary>
    /// 导出用户数据
    /// </summary>
    /// <param name="input">导出参数</param>
    /// <returns>导出的文件字节数组</returns>
    public async Task<byte[]> ExportAsync(LeanUserExportDto input)
    {
      var queryInput = new LeanUserQueryDto
      {
        UserName = input.UserName,
        RealName = input.RealName,
        PhoneNumber = input.PhoneNumber,
        UserStatus = input.UserStatus,
        PageSize = int.MaxValue,
        PageIndex = 1
      };

      var users = await QueryAsync(queryInput);
      var exportDtos = users.Items.Select(x => new LeanUserExportDto
      {
        UserName = x.UserName,
        RealName = x.RealName,
        EnglishName = x.EnglishName,
        Nickname = x.Nickname,
        Email = x.Email,
        PhoneNumber = x.PhoneNumber,
        UserStatus = x.UserStatus,
        IsBuiltin = x.IsBuiltin,
        CreateTime = x.CreateTime
      }).ToList();
      return LeanExcelHelper.Export(exportDtos);
    }

    /// <summary>
    /// 导入用户
    /// </summary>
    /// <param name="file">导入文件</param>
    /// <returns>导入结果</returns>
    /// <remarks>
    /// TODO: 实现用户数据导入功能
    /// </remarks>
    public async Task<LeanUserImportResultDto> ImportAsync(LeanFileInfo file)
    {
      var result = new LeanUserImportResultDto();

      try
      {
        // 读取Excel文件
        var bytes = new byte[file.Stream.Length];
        await file.Stream.ReadAsync(bytes, 0, (int)file.Stream.Length);
        var importResult = LeanExcelHelper.Import<LeanUserImportDto>(bytes);

        foreach (var item in importResult.Data)
        {
          if (await _userRepository.AnyAsync(x => x.UserName == item.UserName))
          {
            result.AddError(item.UserName, $"用户名 {item.UserName} 已存在");
            continue;
          }

          var user = item.Adapt<LeanUser>();
          user.Password = SecurityOptions.DefaultPassword;
          user.Salt = Guid.NewGuid().ToString("N");
          user.UserStatus = 2;
          user.IsBuiltin = 0;
          await _userRepository.CreateAsync(user);
          result.SuccessCount++;
        }
        return result;
      }
      catch (Exception ex)
      {
        _logger.Error(ex, "导入用户失败");
        result.ErrorMessage = $"导入失败：{ex.Message}";
        return result;
      }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <returns>模板文件字节数组</returns>
    /// <remarks>
    /// TODO: 实现获取用户导入模板功能
    /// </remarks>
    public async Task<byte[]> GetImportTemplateAsync()
    {
      var template = new List<LeanUserImportDto>
    {
      new LeanUserImportDto
      {
        UserName = "示例用户",
        RealName = "张三",
        Email = "zhangsan@example.com",
        PhoneNumber = "13800138000"
      }
    };

      return await Task.FromResult(LeanExcelHelper.Export(template));
    }

    /// <summary>
    /// 解锁用户
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="unlockReason">解锁原因</param>
    /// <returns>操作结果</returns>
    public async Task<LeanApiResult> UnlockUserAsync(long userId, string unlockReason)
    {
      try
      {
        var loginExtend = await _loginExtendRepository.FirstOrDefaultAsync(l => l.UserId == userId);
        if (loginExtend == null)
        {
          return LeanApiResult.Error("用户不存在", LeanErrorCode.Status404NotFound, LeanBusinessType.Update);
        }

        // 检查用户是否已解锁
        if (loginExtend.LockStatus == 0) // 正常状态
        {
          return LeanApiResult.Error("用户未锁定", LeanErrorCode.Status400BadRequest, LeanBusinessType.Update);
        }

        // 更新解锁状态
        loginExtend.LockStatus = 0; // 正常状态
        loginExtend.UnlockTime = DateTime.Now;
        loginExtend.UnlockReason = unlockReason;
        loginExtend.PasswordErrorCount = 0;

        await _loginExtendRepository.UpdateAsync(loginExtend);

        // TODO: 发送解锁通知

        return LeanApiResult.Ok(LeanBusinessType.Update);
      }
      catch (Exception ex)
      {
        Logger.Error(ex, "解锁用户失败");
        return LeanApiResult.Error($"解锁用户失败：{ex.Message}", LeanErrorCode.Status500InternalServerError, LeanBusinessType.Update);
      }
    }

    /// <summary>
    /// 锁定用户
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <returns>操作结果</returns>
    public async Task<LeanApiResult> LockUserAsync(long userId)
    {
      try
      {
        var loginExtend = await _loginExtendRepository.FirstOrDefaultAsync(l => l.UserId == userId);
        if (loginExtend == null)
        {
          return LeanApiResult.Error("用户不存在", LeanErrorCode.Status404NotFound, LeanBusinessType.Update);
        }

        // 检查用户是否已锁定
        if (loginExtend.LockStatus == 1) // 临时锁定
        {
          return LeanApiResult.Error("用户已锁定", LeanErrorCode.Status400BadRequest, LeanBusinessType.Update);
        }

        // 更新锁定状态
        loginExtend.LockStatus = 1; // 临时锁定
        loginExtend.LockTime = DateTime.Now;
        loginExtend.PasswordErrorCount = 0;
        loginExtend.LockReason = "管理员锁定";

        await _loginExtendRepository.UpdateAsync(loginExtend);

        // TODO: 发送锁定通知

        return LeanApiResult.Ok(LeanBusinessType.Update);
      }
      catch (Exception ex)
      {
        Logger.Error(ex, "锁定用户失败");
        return LeanApiResult.Error($"锁定用户失败：{ex.Message}", LeanErrorCode.Status500InternalServerError, LeanBusinessType.Update);
      }
    }
  }
}