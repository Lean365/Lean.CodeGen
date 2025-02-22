using Lean.CodeGen.Application.Dtos.Identity;
using Lean.CodeGen.Common.Models;

namespace Lean.CodeGen.Application.Services.Identity;

/// <summary>
/// 角色服务接口
/// </summary>
public interface ILeanRoleService
{
  /// <summary>
  /// 创建角色
  /// </summary>
  Task<LeanApiResult<long>> CreateAsync(LeanCreateRoleDto input);

  /// <summary>
  /// 更新角色
  /// </summary>
  Task<LeanApiResult> UpdateAsync(LeanUpdateRoleDto input);

  /// <summary>
  /// 删除角色
  /// </summary>
  Task<LeanApiResult> DeleteAsync(long id);

  /// <summary>
  /// 批量删除角色
  /// </summary>
  Task<LeanApiResult> BatchDeleteAsync(List<long> ids);

  /// <summary>
  /// 获取角色信息
  /// </summary>
  Task<LeanApiResult<LeanRoleDto>> GetAsync(long id);

  /// <summary>
  /// 分页查询角色
  /// </summary>
  Task<LeanApiResult<LeanPageResult<LeanRoleDto>>> GetPageAsync(LeanQueryRoleDto input);

  /// <summary>
  /// 修改角色状态
  /// </summary>
  Task<LeanApiResult> SetStatusAsync(LeanChangeRoleStatusDto input);

  /// <summary>
  /// 获取角色的菜单权限
  /// </summary>
  Task<LeanApiResult<List<long>>> GetRoleMenusAsync(long roleId);

  /// <summary>
  /// 获取角色的API权限
  /// </summary>
  Task<LeanApiResult<List<long>>> GetRoleApisAsync(long roleId);

  /// <summary>
  /// 分配角色菜单权限
  /// </summary>
  Task<LeanApiResult> AssignMenusAsync(LeanAssignRoleMenuDto input);

  /// <summary>
  /// 分配角色API权限
  /// </summary>
  Task<LeanApiResult> AssignApisAsync(LeanAssignRoleApiDto input);

  /// <summary>
  /// 获取角色的数据权限
  /// </summary>
  Task<LeanApiResult<List<long>>> GetRoleDataScopeAsync(long roleId);

  /// <summary>
  /// 分配角色数据权限
  /// </summary>
  Task<LeanApiResult> AssignDataScopeAsync(LeanAssignRoleDataScopeDto input);

  /// <summary>
  /// 获取角色的互斥角色列表
  /// </summary>
  Task<LeanApiResult<List<long>>> GetRoleMutexesAsync(long roleId);

  /// <summary>
  /// 设置角色互斥关系
  /// </summary>
  Task<LeanApiResult> SetRoleMutexesAsync(LeanSetRoleMutexDto input);

  /// <summary>
  /// 获取角色的前置角色列表
  /// </summary>
  Task<LeanApiResult<List<long>>> GetRolePrerequisitesAsync(long roleId);

  /// <summary>
  /// 设置角色前置条件
  /// </summary>
  Task<LeanApiResult> SetRolePrerequisitesAsync(LeanSetRolePrerequisiteDto input);

  /// <summary>
  /// 验证用户是否可以分配指定角色
  /// </summary>
  Task<LeanApiResult<bool>> ValidateUserRoleAssignmentAsync(long userId, long roleId);
}