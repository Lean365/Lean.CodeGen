using Lean.CodeGen.Application.Dtos.Identity;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Common.Excel;

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
  /// 分页查询角色
  /// </summary>
  /// <param name="input">查询参数</param>
  /// <returns>分页查询结果</returns>
  Task<LeanApiResult<LeanPageResult<LeanRoleDto>>> GetPageAsync(LeanRoleQueryDto input);

  /// <summary>
  /// 获取角色信息
  /// </summary>
  /// <param name="id">角色ID</param>
  /// <returns>角色详细信息</returns>
  Task<LeanApiResult<LeanRoleDto>> GetAsync(long id);

  /// <summary>
  /// 创建角色
  /// </summary>
  /// <param name="input">角色创建参数</param>
  /// <returns>创建成功的角色信息</returns>
  Task<LeanApiResult<long>> CreateAsync(LeanRoleCreateDto input);

  /// <summary>
  /// 更新角色
  /// </summary>
  /// <param name="input">角色更新参数</param>
  /// <returns>更新后的角色信息</returns>
  Task<LeanApiResult> UpdateAsync(LeanRoleUpdateDto input);

  /// <summary>
  /// 删除角色
  /// </summary>
  /// <param name="id">角色ID</param>
  Task<LeanApiResult> DeleteAsync(long id);

  /// <summary>
  /// 批量删除角色
  /// </summary>
  Task<LeanApiResult> BatchDeleteAsync(List<long> ids);

  /// <summary>
  /// 导出角色数据
  /// </summary>
  /// <param name="input">查询参数</param>
  /// <returns>导出的角色数据</returns>
  Task<byte[]> ExportAsync(LeanRoleQueryDto input);

  /// <summary>
  /// 获取导入模板
  /// </summary>
  /// <returns>导入模板</returns>
  Task<byte[]> GetTemplateAsync();

  /// <summary>
  /// 导入角色数据
  /// </summary>
  /// <param name="file">导入的文件信息</param>
  /// <returns>导入结果</returns>
  Task<LeanRoleImportResultDto> ImportAsync(LeanFileInfo file);

  /// <summary>
  /// 获取角色的菜单权限
  /// </summary>
  /// <param name="roleId">角色ID</param>
  /// <returns>角色菜单ID列表</returns>
  Task<List<long>> GetRoleMenusAsync(long roleId);

  /// <summary>
  /// 设置角色的菜单权限
  /// </summary>
  /// <param name="input">菜单分配参数</param>
  Task<LeanApiResult> SetRoleMenusAsync(LeanRoleSetMenusDto input);
}