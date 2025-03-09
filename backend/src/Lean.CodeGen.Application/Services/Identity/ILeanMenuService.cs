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
    /// 创建菜单
    /// </summary>
    /// <param name="input">菜单创建参数</param>
    /// <returns>创建成功的菜单信息</returns>
    Task<LeanApiResult<long>> CreateAsync(LeanMenuCreateDto input);

    /// <summary>
    /// 更新菜单
    /// </summary>
    /// <param name="input">菜单更新参数</param>
    /// <returns>更新后的菜单信息</returns>
    Task<LeanApiResult> UpdateAsync(LeanMenuUpdateDto input);

    /// <summary>
    /// 删除菜单
    /// </summary>
    /// <param name="input">菜单删除参数</param>
    Task<LeanApiResult> DeleteAsync(long id);

    /// <summary>
    /// 批量删除菜单
    /// </summary>
    Task<LeanApiResult> BatchDeleteAsync(List<long> ids);

    /// <summary>
    /// 获取菜单信息
    /// </summary>
    /// <param name="id">菜单ID</param>
    /// <returns>菜单详细信息</returns>
    Task<LeanApiResult<LeanMenuDto>> GetAsync(long id);

    /// <summary>
    /// 分页查询菜单
    /// </summary>
    Task<LeanApiResult<LeanPageResult<LeanMenuDto>>> GetPageAsync(LeanMenuQueryDto input);

    /// <summary>
    /// 设置菜单状态
    /// </summary>
    Task<LeanApiResult> SetStatusAsync(LeanMenuChangeStatusDto input);

    /// <summary>
    /// 获取菜单树形结构
    /// </summary>
    /// <param name="input">查询参数</param>
    /// <returns>菜单树形结构</returns>
    Task<List<LeanMenuTreeDto>> GetTreeAsync(LeanMenuQueryDto input);

    /// <summary>
    /// 导出菜单数据
    /// </summary>
    Task<byte[]> ExportAsync(LeanMenuQueryDto input);

    /// <summary>
    /// 导入菜单数据
    /// </summary>
    Task<LeanMenuImportResultDto> ImportAsync(LeanFileInfo file);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    Task<byte[]> GetImportTemplateAsync();

    /// <summary>
    /// 查询菜单列表
    /// </summary>
    /// <param name="input">查询参数</param>
    /// <returns>菜单列表</returns>
    Task<List<LeanMenuDto>> QueryAsync(LeanMenuQueryDto input);

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