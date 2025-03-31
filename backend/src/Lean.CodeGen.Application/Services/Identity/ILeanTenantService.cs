using Lean.CodeGen.Application.Dtos.Identity;
using Lean.CodeGen.Common.Models;

namespace Lean.CodeGen.Application.Services.Identity;

/// <summary>
/// 租户服务接口
/// </summary>
public interface ILeanTenantService
{
  /// <summary>
  /// 创建租户
  /// </summary>
  /// <param name="input">租户创建参数</param>
  /// <returns>租户信息</returns>
  Task<LeanTenantDto> CreateAsync(LeanTenantCreateDto input);

  /// <summary>
  /// 更新租户
  /// </summary>
  /// <param name="input">租户更新参数</param>
  /// <returns>租户信息</returns>
  Task<LeanTenantDto> UpdateAsync(LeanTenantUpdateDto input);

  /// <summary>
  /// 删除租户
  /// </summary>
  /// <param name="input">租户删除参数</param>
  Task DeleteAsync(LeanTenantDeleteDto input);

  /// <summary>
  /// 获取租户信息
  /// </summary>
  /// <param name="id">租户ID</param>
  /// <returns>租户信息</returns>
  Task<LeanTenantDto> GetAsync(long id);

  /// <summary>
  /// 分页查询租户
  /// </summary>
  /// <param name="input">查询参数</param>
  /// <returns>租户列表</returns>
  Task<LeanPageResult<LeanTenantDto>> GetPageAsync(LeanTenantQueryDto input);

  /// <summary>
  /// 修改租户状态
  /// </summary>
  /// <param name="input">状态修改参数</param>
  Task ChangeStatusAsync(LeanTenantChangeStatusDto input);

  /// <summary>
  /// 导出租户
  /// </summary>
  /// <param name="input">导出查询参数</param>
  /// <returns>导出文件字节数组</returns>
  Task<byte[]> ExportAsync(LeanTenantExportQueryDto input);

  /// <summary>
  /// 导入租户数据
  /// </summary>
  /// <param name="file">导入文件</param>
  /// <returns>导入结果</returns>
  Task<LeanTenantImportResultDto> ImportAsync(LeanFileInfo file);

  /// <summary>
  /// 获取导入模板
  /// </summary>
  /// <returns>模板文件字节数组</returns>
  Task<byte[]> GetTemplateAsync();
}