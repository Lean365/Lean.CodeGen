using Lean.CodeGen.Application.Dtos.Identity;
using Lean.CodeGen.Common.Models;

namespace Lean.CodeGen.Application.Services.Identity;

/// <summary>
/// 角色服务接口
/// </summary>
/// <remarks>
/// 提供角色管理相关的业务功能，包括：
/// 1. 角色的增删改查
/// 2. 角色状态管理
/// 3. 角色菜单管理
/// 4. 角色数据权限管理
/// </remarks>
public interface ILeanRoleService
{
  /// <summary>
  /// 创建角色
  /// </summary>
  /// <param name="input">角色创建参数</param>
  /// <returns>创建成功的角色信息</returns>
  Task<LeanRoleDto> CreateAsync(LeanCreateRoleDto input);

  /// <summary>
  /// 更新角色
  /// </summary>
  /// <param name="input">角色更新参数</param>
  /// <returns>更新后的角色信息</returns>
  Task<LeanRoleDto> UpdateAsync(LeanUpdateRoleDto input);

  /// <summary>
  /// 删除角色
  /// </summary>
  /// <param name="input">角色删除参数</param>
  Task DeleteAsync(LeanDeleteRoleDto input);

  /// <summary>
  /// 获取角色信息
  /// </summary>
  /// <param name="id">角色ID</param>
  /// <returns>角色详细信息</returns>
  Task<LeanRoleDto> GetAsync(long id);

  /// <summary>
  /// 分页查询角色
  /// </summary>
  /// <param name="input">查询参数</param>
  /// <returns>分页查询结果</returns>
  Task<LeanPageResult<LeanRoleDto>> QueryAsync(LeanQueryRoleDto input);

  /// <summary>
  /// 修改角色状态
  /// </summary>
  /// <param name="input">状态修改参数</param>
  Task ChangeStatusAsync(LeanChangeRoleStatusDto input);

  /// <summary>
  /// 分配角色菜单
  /// </summary>
  /// <param name="input">菜单分配参数</param>
  Task AssignMenusAsync(LeanAssignRoleMenuDto input);

  /// <summary>
  /// 分配角色数据权限
  /// </summary>
  /// <param name="input">数据权限分配参数</param>
  Task AssignDataScopeAsync(LeanAssignRoleDataScopeDto input);

  /// <summary>
  /// 获取角色菜单
  /// </summary>
  /// <param name="id">角色ID</param>
  /// <returns>角色菜单ID列表</returns>
  Task<List<long>> GetMenusAsync(long id);

  /// <summary>
  /// 获取角色数据权限
  /// </summary>
  /// <param name="id">角色ID</param>
  /// <returns>角色数据权限信息</returns>
  Task<LeanRoleDataScopeDto> GetDataScopeAsync(long id);
}