using Lean.CodeGen.Application.Dtos.Identity;
using Lean.CodeGen.Common.Models;

namespace Lean.CodeGen.Application.Services.Identity;

/// <summary>
/// 菜单服务接口
/// </summary>
/// <remarks>
/// 提供菜单管理相关的业务功能，包括：
/// 1. 菜单的增删改查
/// 2. 菜单状态管理
/// 3. 菜单树形结构管理
/// 4. 菜单权限管理
/// </remarks>
public interface ILeanMenuService
{
  /// <summary>
  /// 创建菜单
  /// </summary>
  /// <param name="input">菜单创建参数</param>
  /// <returns>创建成功的菜单信息</returns>
  Task<LeanMenuDto> CreateAsync(LeanCreateMenuDto input);

  /// <summary>
  /// 更新菜单
  /// </summary>
  /// <param name="input">菜单更新参数</param>
  /// <returns>更新后的菜单信息</returns>
  Task<LeanMenuDto> UpdateAsync(LeanUpdateMenuDto input);

  /// <summary>
  /// 删除菜单
  /// </summary>
  /// <param name="input">菜单删除参数</param>
  Task DeleteAsync(LeanDeleteMenuDto input);

  /// <summary>
  /// 获取菜单信息
  /// </summary>
  /// <param name="id">菜单ID</param>
  /// <returns>菜单详细信息</returns>
  Task<LeanMenuDto> GetAsync(long id);

  /// <summary>
  /// 查询菜单列表
  /// </summary>
  /// <param name="input">查询参数</param>
  /// <returns>菜单列表</returns>
  Task<List<LeanMenuDto>> QueryAsync(LeanQueryMenuDto input);

  /// <summary>
  /// 获取菜单树形结构
  /// </summary>
  /// <param name="input">查询参数</param>
  /// <returns>菜单树形结构</returns>
  Task<List<LeanMenuTreeDto>> GetTreeAsync(LeanQueryMenuDto input);

  /// <summary>
  /// 修改菜单状态
  /// </summary>
  /// <param name="input">状态修改参数</param>
  Task ChangeStatusAsync(LeanChangeMenuStatusDto input);

  /// <summary>
  /// 获取角色菜单树
  /// </summary>
  /// <param name="roleId">角色ID</param>
  /// <returns>角色菜单树</returns>
  Task<List<LeanMenuTreeDto>> GetRoleMenuTreeAsync(long roleId);

  /// <summary>
  /// 获取用户菜单树
  /// </summary>
  /// <param name="userId">用户ID</param>
  /// <returns>用户菜单树</returns>
  Task<List<LeanMenuTreeDto>> GetUserMenuTreeAsync(long userId);

  /// <summary>
  /// 获取用户权限列表
  /// </summary>
  /// <param name="userId">用户ID</param>
  /// <returns>用户权限列表</returns>
  Task<List<string>> GetUserPermissionsAsync(long userId);
}