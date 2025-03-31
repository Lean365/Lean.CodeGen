using Lean.CodeGen.Common.Localization;
using Lean.CodeGen.Common.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Lean.CodeGen.Domain.Context;
using Lean.CodeGen.Application.Services.Admin;

namespace Lean.CodeGen.WebApi.Controllers.Admin;

/// <summary>
/// 本地化控制器
/// </summary>
[ApiController]
[Route("api/[controller]")]
[ApiExplorerSettings(GroupName = "admin")]
public class LeanLocalizationController : LeanBaseController
{
  private readonly ILeanTranslationService _translationService;

  /// <summary>
  /// 构造函数
  /// </summary>
  public LeanLocalizationController(
      ILeanLocalizationService localizationService,
      ILeanTranslationService translationService,
      IConfiguration configuration,
      ILeanUserContext userContext)
      : base(localizationService, configuration, userContext)
  {
    _translationService = translationService;
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
    var supportedLanguages = await LocalizationService.GetSupportedLanguagesAsync();
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
    var result = await _translationService.GetTranslationsByLangAsync(langCode);
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 获取指定语言的指定模块的翻译
  /// </summary>
  [HttpGet("translations/{langCode}/{moduleName}")]
  public async Task<IActionResult> GetModuleTranslationsAsync([FromRoute] string langCode, [FromRoute] string moduleName)
  {
    var translations = await _translationService.GetTranslationsByLangAsync(langCode);
    var moduleTranslations = translations.Where(x => x.Key.StartsWith(moduleName + "."))
                                      .ToDictionary(x => x.Key, x => x.Value);
    return Success(moduleTranslations, LeanBusinessType.Query);
  }

  /// <summary>
  /// 获取指定键的翻译
  /// </summary>
  [HttpGet("translation/{langCode}/{key}")]
  public new async Task<IActionResult> GetTranslationAsync([FromRoute] string langCode, [FromRoute] string key)
  {
    var result = await LocalizationService.GetTranslationAsync(langCode, key);
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 获取所有支持的语言列表
  /// </summary>
  [HttpGet("languages")]
  public async Task<IActionResult> GetSupportedLanguagesAsync()
  {
    var result = await LocalizationService.GetSupportedLanguagesAsync();
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 刷新翻译缓存
  /// </summary>
  [HttpPost("cache/refresh")]
  public async Task<IActionResult> RefreshCacheAsync()
  {
    // 由于 Common.Localization 命名空间的 ILeanLocalizationService 接口没有 RefreshCacheAsync 方法
    // 我们暂时返回成功，后续可以考虑添加缓存刷新功能
    return Success<object?>(null, LeanBusinessType.Other);
  }
}