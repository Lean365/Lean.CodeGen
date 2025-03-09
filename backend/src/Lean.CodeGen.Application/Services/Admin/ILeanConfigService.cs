using Lean.CodeGen.Application.Dtos.Admin;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Common.Excel;

namespace Lean.CodeGen.Application.Services.Admin;

/// <summary>
/// 系统配置服务接口
/// </summary>
public interface ILeanConfigService
{
    /// <summary>
    /// 创建系统配置
    /// </summary>
    Task<LeanApiResult<long>> CreateAsync(LeanConfigCreateDto input);

    /// <summary>
    /// 更新系统配置
    /// </summary>
    Task<LeanApiResult> UpdateAsync(LeanConfigUpdateDto input);

    /// <summary>
    /// 删除系统配置
    /// </summary>
    Task<LeanApiResult> DeleteAsync(List<long> ids);

    /// <summary>
    /// 获取系统配置详情
    /// </summary>
    Task<LeanApiResult<LeanConfigDto>> GetAsync(long id);

    /// <summary>
    /// 分页查询系统配置
    /// </summary>
    Task<LeanApiResult<LeanPageResult<LeanConfigDto>>> GetPagedListAsync(LeanConfigQueryDto input);

    /// <summary>
    /// 导出系统配置
    /// </summary>
    Task<byte[]> ExportAsync(LeanConfigQueryDto input);

    /// <summary>
    /// 导入系统配置
    /// </summary>
    Task<LeanExcelImportResult<LeanConfigImportDto>> ImportAsync(LeanFileInfo file);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    Task<byte[]> GetImportTemplateAsync();
}