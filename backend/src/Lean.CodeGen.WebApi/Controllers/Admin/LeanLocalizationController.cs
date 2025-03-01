using Lean.CodeGen.Application.Services.Admin;
using Lean.CodeGen.Common.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Lean.CodeGen.WebApi.Controllers.Admin;

/// <summary>
/// 本地化控制器
/// </summary>
[Route("api/admin/localization")]
[ApiController]
public class LeanLocalizationController : LeanBaseController
{
  private readonly ILeanLocalizationService _localizationService;

  /// <summary>
  /// 构造函数
  /// </summary>
  public LeanLocalizationController(
      ILeanLocalizationService localizationService,
      IConfiguration configuration)
      : base(localizationService, configuration)
  {
    _localizationService = localizationService;
  }

  /// <summary>
  /// 获取系统语言
  /// </summary>
  [HttpGet("system-language")]
  public IActionResult GetSystemLanguageCode()
  {
    var langCode = GetSystemLanguage();
    return Success(langCode, LeanBusinessType.Query);
  }

  /// <summary>
  /// 获取当前用户界面语言
  /// </summary>
  [HttpGet("current-language")]
  public IActionResult GetCurrentLanguageCode()
  {
    var langCode = GetCurrentUiLanguage();
    return Success(langCode, LeanBusinessType.Query);
  }

  /// <summary>
  /// 设置当前用户界面语言
  /// </summary>
  [HttpPost("language/{langCode}")]
  public async Task<IActionResult> SetLanguage([FromRoute] string langCode)
  {
    // 验证语言代码是否支持
    var supportedLanguages = await _localizationService.GetSupportedLanguagesAsync();
    if (!supportedLanguages.Contains(langCode))
    {
      return await ErrorAsync("common.error.unsupported_language", LeanErrorCode.ValidationError);
    }

    // 设置语言
    SetCurrentUiLanguage(langCode);

    return Success(langCode, LeanBusinessType.Update);
  }

  /// <summary>
  /// 获取指定语言的所有翻译
  /// </summary>
  [HttpGet("translations/{langCode}")]
  public async Task<IActionResult> GetTranslationsAsync([FromRoute] string langCode)
  {
    var result = await _localizationService.GetTranslationsAsync(langCode);
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 获取指定语言的指定模块的翻译
  /// </summary>
  [HttpGet("translations/{langCode}/{moduleName}")]
  public async Task<IActionResult> GetModuleTranslationsAsync([FromRoute] string langCode, [FromRoute] string moduleName)
  {
    var result = await _localizationService.GetModuleTranslationsAsync(langCode, moduleName);
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 获取指定键的翻译
  /// </summary>
  [HttpGet("translation/{langCode}/{key}")]
  public new async Task<IActionResult> GetTranslationAsync([FromRoute] string langCode, [FromRoute] string key)
  {
    var result = await _localizationService.GetTranslationAsync(langCode, key);
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 获取所有支持的语言列表
  /// </summary>
  [HttpGet("languages")]
  public async Task<IActionResult> GetSupportedLanguagesAsync()
  {
    var result = await _localizationService.GetSupportedLanguagesAsync();
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 刷新翻译缓存
  /// </summary>
  [HttpPost("cache/refresh")]
  public async Task<IActionResult> RefreshCacheAsync()
  {
    await _localizationService.RefreshCacheAsync();
    return Success<object?>(null, LeanBusinessType.Other);
  }
}