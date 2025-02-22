using System.Linq.Expressions;
using Lean.CodeGen.Application.Dtos.Identity;
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
using Microsoft.Extensions.Logging;

namespace Lean.CodeGen.Application.Services.Identity;

/// <summary>
/// 角色服务实现
/// </summary>
public class LeanRoleService : LeanBaseService, ILeanRoleService
{
  private readonly ILeanRepository<LeanRole> _roleRepository;
  private readonly ILeanRepository<LeanRoleMenu> _roleMenuRepository;
  private readonly ILeanRepository<LeanRoleApi> _roleApiRepository;
  private readonly ILeanRepository<LeanRoleMutex> _roleMutexRepository;
  private readonly ILeanRepository<LeanRolePrerequisite> _rolePrerequisiteRepository;
  private readonly LeanUniqueValidator<LeanRole> _uniqueValidator;
  private readonly ILogger<LeanRoleService> _logger;

  public LeanRoleService(
      ILeanRepository<LeanRole> roleRepository,
      ILeanRepository<LeanRoleMenu> roleMenuRepository,
      ILeanRepository<LeanRoleApi> roleApiRepository,
      ILeanRepository<LeanRoleMutex> roleMutexRepository,
      ILeanRepository<LeanRolePrerequisite> rolePrerequisiteRepository,
      ILeanSqlSafeService sqlSafeService,
      IOptions<LeanSecurityOptions> securityOptions,
      ILogger<LeanRoleService> logger)
      : base(sqlSafeService, securityOptions, logger)
  {
    _roleRepository = roleRepository;
    _roleMenuRepository = roleMenuRepository;
    _roleApiRepository = roleApiRepository;
    _roleMutexRepository = roleMutexRepository;
    _rolePrerequisiteRepository = rolePrerequisiteRepository;
    _uniqueValidator = new LeanUniqueValidator<LeanRole>(roleRepository);
    _logger = logger;
  }

  /// <summary>
  /// 创建角色
  /// </summary>
  public async Task<LeanApiResult<long>> CreateAsync(LeanCreateRoleDto input)
  {
    try
    {
      if (!await ValidateRoleInputAsync(input.RoleName, input.RoleCode))
      {
        return LeanApiResult<long>.Error("输入参数验证失败");
      }

      var role = input.Adapt<LeanRole>();
      role.CreateTime = DateTime.Now;

      await _roleRepository.CreateAsync(role);

      return LeanApiResult<long>.Ok(role.Id);
    }
    catch (Exception ex)
    {
      return LeanApiResult<long>.Error($"创建角色失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 更新角色
  /// </summary>
  public async Task<LeanApiResult> UpdateAsync(LeanUpdateRoleDto input)
  {
    try
    {
      var role = await GetRoleByIdAsync(input.Id);
      if (role == null)
      {
        return LeanApiResult.Error($"角色 {input.Id} 不存在");
      }

      if (!await ValidateRoleInputAsync(input.RoleName, input.RoleCode, input.Id))
      {
        return LeanApiResult.Error("输入参数验证失败");
      }

      input.Adapt(role);
      role.UpdateTime = DateTime.Now;

      await _roleRepository.UpdateAsync(role);

      return LeanApiResult.Ok();
    }
    catch (Exception ex)
    {
      return LeanApiResult.Error($"更新角色失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 删除角色
  /// </summary>
  public async Task<LeanApiResult> DeleteAsync(long id)
  {
    try
    {
      var role = await GetRoleByIdAsync(id);
      if (role == null)
      {
        return LeanApiResult.Error($"角色 {id} 不存在");
      }

      if (role.IsBuiltin == LeanBuiltinStatus.Yes)
      {
        return LeanApiResult.Error($"角色 {role.RoleName} 是内置角色，不能删除");
      }

      await DeleteRoleRelationsAsync(id);
      await _roleRepository.DeleteAsync(r => r.Id == id);

      return LeanApiResult.Ok();
    }
    catch (Exception ex)
    {
      return LeanApiResult.Error($"删除角色失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 批量删除角色
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
          return result;
        }
      }

      return LeanApiResult.Ok();
    }
    catch (Exception ex)
    {
      return LeanApiResult.Error($"批量删除角色失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 获取角色信息
  /// </summary>
  public async Task<LeanApiResult<LeanRoleDto>> GetAsync(long id)
  {
    try
    {
      var role = await GetRoleByIdAsync(id);
      if (role == null)
      {
        return LeanApiResult<LeanRoleDto>.Error($"角色 {id} 不存在");
      }

      var result = role.Adapt<LeanRoleDto>();

      return LeanApiResult<LeanRoleDto>.Ok(result);
    }
    catch (Exception ex)
    {
      return LeanApiResult<LeanRoleDto>.Error($"获取角色信息失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 分页查询角色
  /// </summary>
  public async Task<LeanApiResult<LeanPageResult<LeanRoleDto>>> GetPageAsync(LeanQueryRoleDto input)
  {
    try
    {
      var predicate = BuildRoleQueryPredicate(input);
      var (total, items) = await _roleRepository.GetPageListAsync(predicate, input.PageSize, input.PageIndex);

      var result = new LeanPageResult<LeanRoleDto>
      {
        Total = total,
        Items = items.Select(r => r.Adapt<LeanRoleDto>()).ToList()
      };

      return LeanApiResult<LeanPageResult<LeanRoleDto>>.Ok(result);
    }
    catch (Exception ex)
    {
      return LeanApiResult<LeanPageResult<LeanRoleDto>>.Error($"查询角色失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 修改角色状态
  /// </summary>
  public async Task<LeanApiResult> SetStatusAsync(LeanChangeRoleStatusDto input)
  {
    try
    {
      var role = await GetRoleByIdAsync(input.Id);
      if (role == null)
      {
        return LeanApiResult.Error($"角色 {input.Id} 不存在");
      }

      role.RoleStatus = input.RoleStatus;
      role.UpdateTime = DateTime.Now;

      await _roleRepository.UpdateAsync(role);

      return LeanApiResult.Ok();
    }
    catch (Exception ex)
    {
      return LeanApiResult.Error($"修改角色状态失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 获取角色的菜单权限
  /// </summary>
  public async Task<LeanApiResult<List<long>>> GetRoleMenusAsync(long roleId)
  {
    try
    {
      var role = await GetRoleByIdAsync(roleId);
      if (role == null)
      {
        return LeanApiResult<List<long>>.Error($"角色 {roleId} 不存在");
      }

      var roleMenus = await _roleMenuRepository.GetListAsync(rm => rm.RoleId == roleId);
      var menuIds = roleMenus.Select(rm => rm.MenuId).ToList();

      return LeanApiResult<List<long>>.Ok(menuIds);
    }
    catch (Exception ex)
    {
      return LeanApiResult<List<long>>.Error($"获取角色菜单权限失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 获取角色的API权限
  /// </summary>
  public async Task<LeanApiResult<List<long>>> GetRoleApisAsync(long roleId)
  {
    try
    {
      var role = await GetRoleByIdAsync(roleId);
      if (role == null)
      {
        return LeanApiResult<List<long>>.Error($"角色 {roleId} 不存在");
      }

      var roleApis = await _roleApiRepository.GetListAsync(ra => ra.RoleId == roleId);
      var apiIds = roleApis.Select(ra => ra.ApiId).ToList();

      return LeanApiResult<List<long>>.Ok(apiIds);
    }
    catch (Exception ex)
    {
      return LeanApiResult<List<long>>.Error($"获取角色API权限失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 分配角色菜单权限
  /// </summary>
  public async Task<LeanApiResult> AssignMenusAsync(LeanAssignRoleMenuDto input)
  {
    try
    {
      var role = await GetRoleByIdAsync(input.RoleId);
      if (role == null)
      {
        return LeanApiResult.Error($"角色 {input.RoleId} 不存在");
      }

      await UpdateRoleMenusAsync(input.RoleId, input.MenuIds);

      return LeanApiResult.Ok();
    }
    catch (Exception ex)
    {
      return LeanApiResult.Error($"分配角色菜单权限失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 分配角色API权限
  /// </summary>
  public async Task<LeanApiResult> AssignApisAsync(LeanAssignRoleApiDto input)
  {
    try
    {
      var role = await GetRoleByIdAsync(input.RoleId);
      if (role == null)
      {
        return LeanApiResult.Error($"角色 {input.RoleId} 不存在");
      }

      await UpdateRoleApisAsync(input.RoleId, input.ApiIds);

      return LeanApiResult.Ok();
    }
    catch (Exception ex)
    {
      return LeanApiResult.Error($"分配角色API权限失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 获取角色的数据权限
  /// </summary>
  public async Task<LeanApiResult<List<long>>> GetRoleDataScopeAsync(long roleId)
  {
    try
    {
      var role = await GetRoleByIdAsync(roleId);
      if (role == null)
      {
        return LeanApiResult<List<long>>.Error($"角色 {roleId} 不存在");
      }

      // TODO: 从角色-部门关联表中获取数据权限范围
      var deptIds = new List<long>();

      return LeanApiResult<List<long>>.Ok(deptIds);
    }
    catch (Exception ex)
    {
      return LeanApiResult<List<long>>.Error($"获取角色数据权限失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 分配角色数据权限
  /// </summary>
  public async Task<LeanApiResult> AssignDataScopeAsync(LeanAssignRoleDataScopeDto input)
  {
    try
    {
      var role = await GetRoleByIdAsync(input.RoleId);
      if (role == null)
      {
        return LeanApiResult.Error($"角色 {input.RoleId} 不存在");
      }

      role.DataScope = input.DataScope;
      role.UpdateTime = DateTime.Now;

      await _roleRepository.UpdateAsync(role);

      // TODO: 更新角色-部门关联表

      return LeanApiResult.Ok();
    }
    catch (Exception ex)
    {
      return LeanApiResult.Error($"分配角色数据权限失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 获取角色的互斥角色列表
  /// </summary>
  public async Task<LeanApiResult<List<long>>> GetRoleMutexesAsync(long roleId)
  {
    try
    {
      var role = await GetRoleByIdAsync(roleId);
      if (role == null)
      {
        return LeanApiResult<List<long>>.Error($"角色 {roleId} 不存在");
      }

      var roleMutexes = await _roleMutexRepository.GetListAsync(rm => rm.RoleId == roleId);
      var mutexRoleIds = roleMutexes.Select(rm => rm.MutexRoleId).ToList();

      return LeanApiResult<List<long>>.Ok(mutexRoleIds);
    }
    catch (Exception ex)
    {
      return LeanApiResult<List<long>>.Error($"获取角色互斥关系失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 设置角色互斥关系
  /// </summary>
  public async Task<LeanApiResult> SetRoleMutexesAsync(LeanSetRoleMutexDto input)
  {
    try
    {
      var role = await GetRoleByIdAsync(input.RoleId);
      if (role == null)
      {
        return LeanApiResult.Error($"角色 {input.RoleId} 不存在");
      }

      await _roleMutexRepository.DeleteAsync(rm => rm.RoleId == input.RoleId);
      if (input.MutexRoleIds?.Any() == true)
      {
        var roleMutexes = input.MutexRoleIds.Select(mutexRoleId => new LeanRoleMutex
        {
          RoleId = input.RoleId,
          MutexRoleId = mutexRoleId,
          CreateTime = DateTime.Now
        }).ToList();
        await _roleMutexRepository.CreateRangeAsync(roleMutexes);
      }

      return LeanApiResult.Ok();
    }
    catch (Exception ex)
    {
      return LeanApiResult.Error($"设置角色互斥关系失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 获取角色的前置角色列表
  /// </summary>
  public async Task<LeanApiResult<List<long>>> GetRolePrerequisitesAsync(long roleId)
  {
    try
    {
      var role = await GetRoleByIdAsync(roleId);
      if (role == null)
      {
        return LeanApiResult<List<long>>.Error($"角色 {roleId} 不存在");
      }

      var prerequisites = await _rolePrerequisiteRepository.GetListAsync(rp => rp.RoleId == roleId);
      var prerequisiteRoleIds = prerequisites.Select(rp => rp.PrerequisiteRoleId).ToList();

      return LeanApiResult<List<long>>.Ok(prerequisiteRoleIds);
    }
    catch (Exception ex)
    {
      return LeanApiResult<List<long>>.Error($"获取角色前置条件失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 设置角色前置条件
  /// </summary>
  public async Task<LeanApiResult> SetRolePrerequisitesAsync(LeanSetRolePrerequisiteDto input)
  {
    try
    {
      var role = await GetRoleByIdAsync(input.RoleId);
      if (role == null)
      {
        return LeanApiResult.Error($"角色 {input.RoleId} 不存在");
      }

      await _rolePrerequisiteRepository.DeleteAsync(rp => rp.RoleId == input.RoleId);
      if (input.PrerequisiteRoleIds?.Any() == true)
      {
        var prerequisites = input.PrerequisiteRoleIds.Select(prerequisiteRoleId => new LeanRolePrerequisite
        {
          RoleId = input.RoleId,
          PrerequisiteRoleId = prerequisiteRoleId,
          CreateTime = DateTime.Now
        }).ToList();
        await _rolePrerequisiteRepository.CreateRangeAsync(prerequisites);
      }

      return LeanApiResult.Ok();
    }
    catch (Exception ex)
    {
      return LeanApiResult.Error($"设置角色前置条件失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 验证用户是否可以分配指定角色
  /// </summary>
  public async Task<LeanApiResult<bool>> ValidateUserRoleAssignmentAsync(long userId, long roleId)
  {
    try
    {
      var role = await GetRoleByIdAsync(roleId);
      if (role == null)
      {
        return LeanApiResult<bool>.Error($"角色 {roleId} 不存在");
      }

      // 检查角色状态
      if (role.RoleStatus != LeanRoleStatus.Normal)
      {
        return LeanApiResult<bool>.Ok(false);
      }

      // 获取用户当前的角色列表
      var userRoles = await _roleRepository.GetListAsync(r => r.UserRoles.Any(ur => ur.UserId == userId));
      var userRoleIds = userRoles.Select(r => r.Id).ToList();

      // 检查互斥角色
      var mutexRoles = await _roleMutexRepository.GetListAsync(rm => rm.RoleId == roleId);
      if (mutexRoles.Any(rm => userRoleIds.Contains(rm.MutexRoleId)))
      {
        return LeanApiResult<bool>.Ok(false);
      }

      // 检查前置角色
      var prerequisites = await _rolePrerequisiteRepository.GetListAsync(rp => rp.RoleId == roleId);
      if (prerequisites.Any() && !prerequisites.All(rp => userRoleIds.Contains(rp.PrerequisiteRoleId)))
      {
        return LeanApiResult<bool>.Ok(false);
      }

      return LeanApiResult<bool>.Ok(true);
    }
    catch (Exception ex)
    {
      return LeanApiResult<bool>.Error($"验证用户角色分配失败: {ex.Message}");
    }
  }

  #region 私有方法

  /// <summary>
  /// 获取角色
  /// </summary>
  private async Task<LeanRole?> GetRoleByIdAsync(long id)
  {
    return await _roleRepository.GetByIdAsync(id);
  }

  /// <summary>
  /// 验证角色输入
  /// </summary>
  private async Task<bool> ValidateRoleInputAsync(string roleName, string roleCode, long? id = null)
  {
    if (string.IsNullOrEmpty(roleName) || string.IsNullOrEmpty(roleCode))
    {
      return false;
    }

    if (!ValidateInput(new[] { roleName, roleCode }))
    {
      return false;
    }

    try
    {
      await _uniqueValidator.ValidateAsync(
          (r => r.RoleName, roleName, id, $"角色名称 {roleName} 已存在"),
          (r => r.RoleCode, roleCode, id, $"角色编码 {roleCode} 已存在")
      );
      return true;
    }
    catch
    {
      return false;
    }
  }

  /// <summary>
  /// 更新角色菜单关系
  /// </summary>
  private async Task UpdateRoleMenusAsync(long roleId, List<long> menuIds)
  {
    await _roleMenuRepository.DeleteAsync(rm => rm.RoleId == roleId);
    if (menuIds?.Any() == true)
    {
      var roleMenus = menuIds.Select(menuId => new LeanRoleMenu
      {
        RoleId = roleId,
        MenuId = menuId,
        CreateTime = DateTime.Now
      }).ToList();
      await _roleMenuRepository.CreateRangeAsync(roleMenus);
    }
  }

  /// <summary>
  /// 更新角色API关系
  /// </summary>
  private async Task UpdateRoleApisAsync(long roleId, List<long> apiIds)
  {
    await _roleApiRepository.DeleteAsync(ra => ra.RoleId == roleId);
    if (apiIds?.Any() == true)
    {
      var roleApis = apiIds.Select(apiId => new LeanRoleApi
      {
        RoleId = roleId,
        ApiId = apiId,
        CreateTime = DateTime.Now
      }).ToList();
      await _roleApiRepository.CreateRangeAsync(roleApis);
    }
  }

  /// <summary>
  /// 删除角色关联信息
  /// </summary>
  private async Task DeleteRoleRelationsAsync(long roleId)
  {
    await _roleMenuRepository.DeleteAsync(rm => rm.RoleId == roleId);
    await _roleApiRepository.DeleteAsync(ra => ra.RoleId == roleId);
    // TODO: 删除角色-部门关联
  }

  /// <summary>
  /// 构建角色查询条件
  /// </summary>
  private Expression<Func<LeanRole, bool>> BuildRoleQueryPredicate(LeanQueryRoleDto input)
  {
    Expression<Func<LeanRole, bool>> predicate = r => true;

    if (!string.IsNullOrEmpty(input.RoleName))
    {
      var roleName = CleanInput(input.RoleName);
      predicate = LeanExpressionExtensions.And(predicate, r => r.RoleName.Contains(roleName));
    }

    if (!string.IsNullOrEmpty(input.RoleCode))
    {
      var roleCode = CleanInput(input.RoleCode);
      predicate = LeanExpressionExtensions.And(predicate, r => r.RoleCode.Contains(roleCode));
    }

    if (input.RoleStatus.HasValue)
    {
      predicate = LeanExpressionExtensions.And(predicate, r => r.RoleStatus == input.RoleStatus.Value);
    }

    if (input.StartTime.HasValue)
    {
      predicate = LeanExpressionExtensions.And(predicate, r => r.CreateTime >= input.StartTime.Value);
    }

    if (input.EndTime.HasValue)
    {
      predicate = LeanExpressionExtensions.And(predicate, r => r.CreateTime <= input.EndTime.Value);
    }

    return predicate;
  }

  #endregion
}