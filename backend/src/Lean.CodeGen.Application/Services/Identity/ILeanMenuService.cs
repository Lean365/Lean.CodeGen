using Lean.CodeGen.Application.Dtos.Identity;
using Lean.CodeGen.Common.Models;

namespace Lean.CodeGen.Application.Services.Identity;

/// <summary>
/// 菜单服务接口
/// </summary>
public interface ILeanMenuService
{
  /// <summary>
  /// 创建菜单
  /// </summary>
  Task<LeanApiResult<long>> CreateAsync(LeanCreateMenuDto input);

  /// <summary>
  /// 更新菜单
  /// </summary>
  Task<LeanApiResult> UpdateAsync(LeanUpdateMenuDto input);

  /// <summary>
  /// 删除菜单
  /// </summary>
  Task<LeanApiResult> DeleteAsync(long id);

  /// <summary>
  /// 批量删除菜单
  /// </summary>
  Task<LeanApiResult> BatchDeleteAsync(List<long> ids);

  /// <summary>
  /// 获取菜单信息
  /// </summary>
  Task<LeanApiResult<LeanMenuDto>> GetAsync(long id);

  /// <summary>
  /// 分页查询菜单
  /// </summary>
  Task<LeanApiResult<LeanPageResult<LeanMenuDto>>> GetPageAsync(LeanQueryMenuDto input);

  /// <summary>
  /// 修改菜单状态
  /// </summary>
  Task<LeanApiResult> SetStatusAsync(LeanChangeMenuStatusDto input);

  /// <summary>
  /// 获取菜单树形结构
  /// </summary>
  Task<LeanApiResult<List<LeanMenuTreeDto>>> GetTreeAsync();

  /// <summary>
  /// 获取用户菜单树
  /// </summary>
  Task<LeanApiResult<List<LeanMenuTreeDto>>> GetUserMenuTreeAsync(long userId);

  /// <summary>
  /// 获取用户权限列表
  /// </summary>
  Task<LeanApiResult<List<string>>> GetUserPermissionsAsync(long userId);

  /// <summary>
  /// 获取角色菜单树
  /// </summary>
  Task<LeanApiResult<List<LeanMenuTreeDto>>> GetRoleMenuTreeAsync(long roleId);

  /// <summary>
  /// 获取菜单的操作权限列表
  /// </summary>
  Task<LeanApiResult<List<LeanMenuOperationDto>>> GetMenuOperationsAsync(long menuId);

  /// <summary>
  /// 设置菜单的操作权限
  /// </summary>
  Task<LeanApiResult> SetMenuOperationsAsync(LeanSetMenuOperationDto input);

  /// <summary>
  /// 获取用户在指定菜单的操作权限
  /// </summary>
  Task<LeanApiResult<List<string>>> GetUserMenuOperationsAsync(long userId, long menuId);

  /// <summary>
  /// 验证用户是否有指定菜单的操作权限
  /// </summary>
  Task<LeanApiResult<bool>> ValidateUserMenuOperationAsync(long userId, long menuId, string operation);
}