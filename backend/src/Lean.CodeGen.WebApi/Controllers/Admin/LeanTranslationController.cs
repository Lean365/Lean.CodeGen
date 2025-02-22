using Lean.CodeGen.Application.Dtos.Admin;
using Lean.CodeGen.Application.Services.Admin;
using Lean.CodeGen.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lean.CodeGen.WebApi.Controllers.Admin;

/// <summary>
/// 翻译控制器
/// </summary>
[Route("api/admin/translation")]
[ApiController]
public class LeanTranslationController : LeanBaseController
{
  private readonly ILeanTranslationService _translationService;

  /// <summary>
  /// 构造函数
  /// </summary>
  public LeanTranslationController(ILeanTranslationService translationService)
  {
    _translationService = translationService;
  }

  /// <summary>
  /// 创建翻译
  /// </summary>
  [HttpPost]
  public Task<LeanApiResult<long>> CreateAsync([FromBody] LeanCreateTranslationDto input)
  {
    return _translationService.CreateAsync(input);
  }

  /// <summary>
  /// 更新翻译
  /// </summary>
  [HttpPut]
  public Task<LeanApiResult> UpdateAsync([FromBody] LeanUpdateTranslationDto input)
  {
    return _translationService.UpdateAsync(input);
  }

  /// <summary>
  /// 删除翻译
  /// </summary>
  [HttpDelete("{id}")]
  public Task<LeanApiResult> DeleteAsync([FromRoute] long id)
  {
    return _translationService.DeleteAsync(id);
  }

  /// <summary>
  /// 批量删除翻译
  /// </summary>
  [HttpDelete("batch")]
  public Task<LeanApiResult> BatchDeleteAsync([FromBody] List<long> ids)
  {
    return _translationService.BatchDeleteAsync(ids);
  }

  /// <summary>
  /// 获取翻译信息
  /// </summary>
  [HttpGet("{id}")]
  public Task<LeanApiResult<LeanTranslationDto>> GetAsync([FromRoute] long id)
  {
    return _translationService.GetAsync(id);
  }

  /// <summary>
  /// 分页查询翻译
  /// </summary>
  [HttpGet("page")]
  public Task<LeanApiResult<LeanPageResult<LeanTranslationDto>>> GetPageAsync([FromQuery] LeanQueryTranslationDto input)
  {
    return _translationService.GetPageAsync(input);
  }

  /// <summary>
  /// 修改翻译状态
  /// </summary>
  [HttpPut("status")]
  public Task<LeanApiResult> SetStatusAsync([FromBody] LeanChangeTranslationStatusDto input)
  {
    return _translationService.SetStatusAsync(input);
  }

  /// <summary>
  /// 获取指定语言的所有翻译
  /// </summary>
  [HttpGet("lang/{langCode}")]
  public Task<LeanApiResult<Dictionary<string, string>>> GetTranslationsByLangAsync([FromRoute] string langCode)
  {
    return _translationService.GetTranslationsByLangAsync(langCode);
  }

  /// <summary>
  /// 获取所有模块列表
  /// </summary>
  [HttpGet("modules")]
  public Task<LeanApiResult<List<string>>> GetModuleListAsync()
  {
    return _translationService.GetModuleListAsync();
  }

  /// <summary>
  /// 导入翻译数据
  /// </summary>
  [HttpPost("import/{langId}")]
  public Task<LeanApiResult> ImportAsync([FromRoute] long langId, [FromBody] Dictionary<string, string> translations)
  {
    return _translationService.ImportAsync(langId, translations);
  }

  /// <summary>
  /// 导出翻译数据
  /// </summary>
  [HttpGet("export/{langId}")]
  public Task<LeanApiResult<List<LeanTranslationExportDto>>> ExportAsync([FromRoute] long langId)
  {
    var input = new LeanQueryTranslationDto { LangId = langId };
    return _translationService.ExportAsync(input);
  }
}