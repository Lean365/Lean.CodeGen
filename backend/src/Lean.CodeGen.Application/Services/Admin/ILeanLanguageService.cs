using Lean.CodeGen.Application.Dtos.Admin;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Common.Excel;

namespace Lean.CodeGen.Application.Services.Admin;

/// <summary>
/// 语言服务接口
/// </summary>
public interface ILeanLanguageService
{
  /// <summary>
  /// 创建语言
  /// </summary>
  Task<LeanApiResult<long>> CreateAsync(LeanCreateLanguageDto input);

  /// <summary>
  /// 更新语言
  /// </summary>
  Task<LeanApiResult> UpdateAsync(LeanUpdateLanguageDto input);

  /// <summary>
  /// 删除语言
  /// </summary>
  Task<LeanApiResult> DeleteAsync(long id);

  /// <summary>
  /// 批量删除语言
  /// </summary>
  Task<LeanApiResult> BatchDeleteAsync(List<long> ids);

  /// <summary>
  /// 获取语言信息
  /// </summary>
  Task<LeanApiResult<LeanLanguageDto>> GetAsync(long id);

  /// <summary>
  /// 分页查询语言
  /// </summary>
  Task<LeanApiResult<LeanPageResult<LeanLanguageDto>>> GetPageAsync(LeanQueryLanguageDto input);

  /// <summary>
  /// 修改语言状态
  /// </summary>
  Task<LeanApiResult> SetStatusAsync(LeanChangeLanguageStatusDto input);

  /// <summary>
  /// 获取所有正常状态的语言列表
  /// </summary>
  Task<LeanApiResult<List<LeanLanguageDto>>> GetListAsync();

  /// <summary>
  /// 设置默认语言
  /// </summary>
  Task<LeanApiResult> SetDefaultAsync(long id);

  /// <summary>
  /// 导出语言
  /// </summary>
  /// <param name="input">查询参数</param>
  /// <returns>导出的Excel文件字节数组</returns>
  Task<byte[]> ExportAsync(LeanQueryLanguageDto input);

  /// <summary>
  /// 导入语言
  /// </summary>
  /// <param name="file">导入文件信息</param>
  /// <returns>导入结果</returns>
  Task<LeanExcelImportResult<LeanLanguageImportDto>> ImportAsync(LeanFileInfo file);

  /// <summary>
  /// 获取导入模板
  /// </summary>
  /// <returns>导入模板Excel文件字节数组</returns>
  Task<byte[]> GetImportTemplateAsync();
}