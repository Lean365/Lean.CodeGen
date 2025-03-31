using Lean.CodeGen.Application.Dtos.Admin;
using Lean.CodeGen.Application.Services.Admin;
using Lean.CodeGen.Common.Models;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Common.Attributes;
using Microsoft.Extensions.Configuration;

namespace Lean.CodeGen.WebApi.Controllers.Admin;

/// <summary>
/// 翻译控制器
/// </summary>
[ApiController]
[Route("api/[controller]")]
[ApiExplorerSettings(GroupName = "admin")]
public class LeanTranslationController : LeanBaseController
{
  private readonly ILeanTranslationService _translationService;

  /// <summary>
  /// 构造函数
  /// </summary>
  public LeanTranslationController(
      ILeanTranslationService translationService,
      ILeanLocalizationService localizationService,
      IConfiguration configuration,
      ILeanUserContext userContext)
      : base(localizationService, configuration, userContext)
  {
    _translationService = translationService;
  }

  /// <summary>
  /// 创建翻译
  /// </summary>
  [HttpPost]
  public async Task<IActionResult> CreateAsync([FromBody] LeanTranslationCreateDto input)
  {
    var result = await _translationService.CreateAsync(input);
    return Success(result, LeanBusinessType.Create);
  }

  /// <summary>
  /// 更新翻译
  /// </summary>
  [HttpPut]
  public async Task<IActionResult> UpdateAsync([FromBody] LeanTranslationUpdateDto input)
  {
    var result = await _translationService.UpdateAsync(input);
    return Success(result, LeanBusinessType.Update);
  }

  /// <summary>
  /// 删除翻译
  /// </summary>
  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteAsync([FromRoute] long id)
  {
    var result = await _translationService.DeleteAsync(id);
    return Success(result, LeanBusinessType.Delete);
  }

  /// <summary>
  /// 批量删除翻译
  /// </summary>
  [HttpDelete("batch")]
  public async Task<IActionResult> BatchDeleteAsync([FromBody] List<long> ids)
  {
    var result = await _translationService.BatchDeleteAsync(ids);
    return Success(result, LeanBusinessType.Delete);
  }

  /// <summary>
  /// 获取翻译信息
  /// </summary>
  [HttpGet("{id}")]
  public async Task<IActionResult> GetAsync([FromRoute] long id)
  {
    var result = await _translationService.GetAsync(id);
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 分页查询翻译
  /// </summary>
  [HttpGet("page")]
  public async Task<IActionResult> GetPageAsync([FromQuery] LeanTranslationQueryDto input)
  {
    var result = await _translationService.GetPageAsync(input);
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 修改翻译状态
  /// </summary>
  [HttpPut("status")]
  public async Task<IActionResult> SetStatusAsync([FromBody] LeanTranslationChangeStatusDto input)
  {
    var result = await _translationService.SetStatusAsync(input);
    return Success(result, LeanBusinessType.Update);
  }

  /// <summary>
  /// 获取指定语言的所有翻译
  /// </summary>
  [HttpGet("lang/{langCode}")]
  public async Task<IActionResult> GetTranslationsByLangAsync([FromRoute] string langCode)
  {
    var result = await _translationService.GetTranslationsByLangAsync(langCode);
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 获取所有模块列表
  /// </summary>
  [HttpGet("modules")]
  public async Task<IActionResult> GetModuleListAsync()
  {
    var result = await _translationService.GetModuleListAsync();
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 导入翻译数据（从字典）
  /// </summary>
  [HttpPost("import/dict/{langId}")]
  public async Task<IActionResult> ImportFromDictionaryAsync([FromRoute] long langId, [FromBody] Dictionary<string, string> translations)
  {
    var result = await _translationService.ImportFromDictionaryAsync(langId, translations);
    return Success(result, LeanBusinessType.Import);
  }

  /// <summary>
  /// 导出翻译数据
  /// </summary>
  [HttpGet("export")]
  public async Task<IActionResult> ExportAsync([FromQuery] LeanTranslationQueryDto input)
  {
    var bytes = await _translationService.ExportAsync(input);
    return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "translations.xlsx");
  }

  /// <summary>
  /// 导出转置的翻译数据（按翻译键分组）
  /// </summary>
  [HttpGet("export/transpose")]
  public async Task<IActionResult> ExportTransposeAsync([FromQuery] LeanTranslationQueryDto input)
  {
    var bytes = await _translationService.ExportTransposeAsync(input);
    return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "translations-transpose.xlsx");
  }

  /// <summary>
  /// 导入翻译
  /// </summary>
  /// <param name="file">Excel文件</param>
  /// <returns>导入结果</returns>
  [HttpPost("import")]
  [LeanPermission("system:translation:import", "导入翻译")]
  public async Task<IActionResult> ImportAsync([FromForm] LeanFileInfo file)
  {
    var result = await _translationService.ImportAsync(file);
    return Success(result, LeanBusinessType.Import);
  }

  /// <summary>
  /// 导入转置的翻译数据（按翻译键分组）
  /// </summary>
  /// <param name="file">Excel文件</param>
  /// <returns>导入结果</returns>
  [HttpPost("import/transpose")]
  [LeanPermission("system:translation:import", "导入翻译")]
  public async Task<IActionResult> ImportTransposeAsync([FromForm] LeanFileInfo file)
  {
    var result = await _translationService.ImportTransposeAsync(file);
    return Success(result, LeanBusinessType.Import);
  }

  /// <summary>
  /// 获取导入模板
  /// </summary>
  [HttpGet("template")]
  public async Task<IActionResult> GetTemplateAsync()
  {
    var bytes = await _translationService.GetTemplateAsync();
    return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "translation-template.xlsx");
  }

  /// <summary>
  /// 获取转置导入模板
  /// </summary>
  [HttpGet("template/transpose")]
  public async Task<IActionResult> GetTransposeImportTemplateAsync()
  {
    var bytes = await _translationService.GetTransposeImportTemplateAsync();
    return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "translation-transpose-template.xlsx");
  }

  /// <summary>
  /// 获取转置的翻译列表（分页）
  /// </summary>
  [HttpGet("page/transpose")]
  public async Task<IActionResult> GetTransposePageAsync([FromQuery] LeanTranslationQueryDto input)
  {
    var result = await _translationService.GetTransposePageAsync(input);
    return Success(result, LeanBusinessType.Query);
  }
}