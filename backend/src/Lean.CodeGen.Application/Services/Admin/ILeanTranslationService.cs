using Lean.CodeGen.Application.Dtos.Admin;
using Lean.CodeGen.Common.Excel;
using Lean.CodeGen.Common.Models;

namespace Lean.CodeGen.Application.Services.Admin;

/// <summary>
/// 翻译服务接口
/// </summary>
public interface ILeanTranslationService
{
  /// <summary>
  /// 创建翻译
  /// </summary>
  /// <param name="input">创建参数</param>
  /// <returns>翻译ID</returns>
  Task<LeanApiResult<long>> CreateAsync(LeanCreateTranslationDto input);

  /// <summary>
  /// 更新翻译
  /// </summary>
  /// <param name="input">更新参数</param>
  /// <returns>更新结果</returns>
  Task<LeanApiResult> UpdateAsync(LeanUpdateTranslationDto input);

  /// <summary>
  /// 删除翻译
  /// </summary>
  /// <param name="id">主键</param>
  /// <returns>删除结果</returns>
  Task<LeanApiResult> DeleteAsync(long id);

  /// <summary>
  /// 批量删除翻译
  /// </summary>
  /// <param name="ids">主键集合</param>
  /// <returns>删除结果</returns>
  Task<LeanApiResult> BatchDeleteAsync(List<long> ids);

  /// <summary>
  /// 获取翻译
  /// </summary>
  /// <param name="id">主键</param>
  /// <returns>翻译信息</returns>
  Task<LeanApiResult<LeanTranslationDto>> GetAsync(long id);

  /// <summary>
  /// 获取翻译分页列表
  /// </summary>
  /// <param name="input">查询参数</param>
  /// <returns>翻译分页列表</returns>
  Task<LeanApiResult<LeanPageResult<LeanTranslationDto>>> GetPageAsync(LeanQueryTranslationDto input);

  /// <summary>
  /// 设置翻译状态
  /// </summary>
  /// <param name="input">状态修改参数</param>
  /// <returns>设置结果</returns>
  Task<LeanApiResult> SetStatusAsync(LeanChangeTranslationStatusDto input);

  /// <summary>
  /// 导出翻译
  /// </summary>
  /// <param name="input">查询参数</param>
  /// <returns>导出结果</returns>
  Task<LeanApiResult<List<LeanTranslationExportDto>>> ExportAsync(LeanQueryTranslationDto input);

  /// <summary>
  /// 导入翻译
  /// </summary>
  /// <param name="langId">语言ID</param>
  /// <param name="translations">翻译数据</param>
  /// <returns>导入结果</returns>
  Task<LeanApiResult> ImportAsync(long langId, Dictionary<string, string> translations);

  /// <summary>
  /// 导入翻译（从Excel文件）
  /// </summary>
  /// <param name="file">导入文件</param>
  /// <returns>导入结果</returns>
  Task<LeanApiResult<LeanExcelImportResult<LeanTranslationImportDto>>> ImportFromExcelAsync(LeanFileInfo file);

  /// <summary>
  /// 获取导入模板
  /// </summary>
  /// <returns>导入模板</returns>
  Task<LeanApiResult<List<LeanTranslationImportTemplateDto>>> GetImportTemplateAsync();

  /// <summary>
  /// 获取指定语言的所有翻译
  /// </summary>
  /// <param name="langCode">语言代码</param>
  /// <returns>翻译字典</returns>
  Task<LeanApiResult<Dictionary<string, string>>> GetTranslationsByLangAsync(string langCode);

  /// <summary>
  /// 获取所有模块列表
  /// </summary>
  /// <returns>模块列表</returns>
  Task<LeanApiResult<List<string>>> GetModuleListAsync();
}