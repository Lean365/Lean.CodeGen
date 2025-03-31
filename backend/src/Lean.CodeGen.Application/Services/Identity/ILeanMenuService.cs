using System.Collections.Generic;
using System.Threading.Tasks;
using Lean.CodeGen.Application.Dtos.Identity;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Common.Excel;

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
  /// 分页查询菜单
  /// </summary>
  Task<LeanApiResult<LeanPageResult<LeanMenuDto>>> GetPageAsync(LeanMenuQueryDto input);

  /// <summary>
  /// 获取菜单信息
  /// </summary>
  Task<LeanApiResult<LeanMenuDto>> GetAsync(long id);

  /// <summary>
  /// 创建菜单
  /// </summary>
  Task<LeanApiResult<long>> CreateAsync(LeanMenuCreateDto input);

  /// <summary>
  /// 更新菜单
  /// </summary>
  Task<LeanApiResult> UpdateAsync(LeanMenuUpdateDto input);

  /// <summary>
  /// 排序菜单
  /// </summary>
  Task<LeanApiResult> SortAsync(List<LeanMenuSortDto> input);

  /// <summary>
  /// 删除菜单
  /// </summary>
  Task<LeanApiResult> DeleteAsync(long id);

  /// <summary>
  /// 批量删除菜单
  /// </summary>
  Task<LeanApiResult> BatchDeleteAsync(List<long> ids);

  /// <summary>
  /// 导出菜单数据
  /// </summary>
  Task<byte[]> ExportAsync(LeanMenuQueryDto input);

  /// <summary>
  /// 获取导入模板
  /// </summary>
  Task<byte[]> GetTemplateAsync();

  /// <summary>
  /// 导入菜单数据
  /// </summary>
  Task<LeanMenuImportResultDto> ImportAsync(LeanFileInfo file);

  /// <summary>
  /// 设置菜单状态
  /// </summary>
  Task<LeanApiResult> SetStatusAsync(LeanMenuChangeStatusDto input);

  /// <summary>
  /// 获取菜单树形结构
  /// </summary>
  Task<List<LeanMenuTreeDto>> GetTreeAsync(LeanMenuQueryDto input);

  /// <summary>
  /// 获取角色菜单树
  /// </summary>
  Task<List<LeanMenuTreeDto>> GetRoleMenuTreeAsync(long roleId);

  /// <summary>
  /// 获取用户菜单树
  /// </summary>
  /// <param name="userId">用户ID</param>
  /// <returns>菜单树形结构</returns>
  Task<List<LeanMenuTreeDto>> GetUserMenuTreeAsync(long userId);

  /// <summary>
  /// 获取用户权限清单
  /// </summary>
  /// <param name="userId">用户ID</param>
  /// <returns>权限清单</returns>
  Task<List<string>> GetUserPermissionsAsync(long userId);

  /// <summary>
  /// 获取用户角色列表
  /// </summary>
  /// <param name="userId">用户ID</param>
  /// <returns>角色列表</returns>
  Task<List<string>> GetUserRolesAsync(long userId);

  /// <summary>
  /// 获取菜单权限列表
  /// </summary>
  /// <param name="menuIds">菜单ID列表</param>
  /// <returns>权限列表</returns>
  Task<List<string>> GetMenuPermissionsAsync(List<long> menuIds);
}