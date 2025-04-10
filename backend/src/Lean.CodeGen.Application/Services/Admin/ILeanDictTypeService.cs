using Lean.CodeGen.Application.Dtos.Admin;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Common.Excel;

namespace Lean.CodeGen.Application.Services.Admin;

/// <summary>
/// 字典类型服务接口
/// </summary>
public interface ILeanDictTypeService
{
    /// <summary>
    /// 创建字典类型
    /// </summary>
    Task<LeanApiResult<long>> CreateAsync(LeanDictTypeCreateDto input);

    /// <summary>
    /// 更新字典类型
    /// </summary>
    Task<LeanApiResult> UpdateAsync(LeanDictTypeUpdateDto input);

    /// <summary>
    /// 删除字典类型
    /// </summary>
    Task<LeanApiResult> DeleteAsync(long id);

    /// <summary>
    /// 批量删除字典类型
    /// </summary>
    Task<LeanApiResult> BatchDeleteAsync(List<long> ids);

    /// <summary>
    /// 获取字典类型信息
    /// </summary>
    Task<LeanApiResult<LeanDictTypeDto>> GetAsync(long id);

    /// <summary>
    /// 分页查询字典类型
    /// </summary>
    Task<LeanApiResult<LeanPageResult<LeanDictTypeDto>>> GetPageAsync(LeanDictTypeQueryDto input);

    /// <summary>
    /// 设置字典类型状态
    /// </summary>
    Task<LeanApiResult> SetStatusAsync(LeanDictTypeChangeStatusDto input);

    /// <summary>
    /// 导出字典类型
    /// </summary>
    Task<byte[]> ExportAsync(LeanDictTypeQueryDto input);

    /// <summary>
    /// 导入字典类型
    /// </summary>
    Task<LeanExcelImportResult<LeanDictTypeImportDto>> ImportAsync(LeanFileInfo file);

    /// <summary>
    /// 获取导入模板
    /// </summary>
    Task<byte[]> GetTemplateAsync();
}