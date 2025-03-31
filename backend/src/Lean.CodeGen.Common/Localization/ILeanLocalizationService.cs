using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lean.CodeGen.Common.Localization;

/// <summary>
/// 本地化服务接口
/// </summary>
public interface ILeanLocalizationService
{
  /// <summary>
  /// 获取本地化文本
  /// </summary>
  /// <param name="key">翻译键</param>
  /// <returns>本地化文本</returns>
  string GetLocalizedText(string key);

  /// <summary>
  /// 异步获取本地化文本
  /// </summary>
  /// <param name="key">翻译键</param>
  /// <returns>本地化文本</returns>
  Task<string> GetLocalizedTextAsync(string key);

  /// <summary>
  /// 获取当前语言
  /// </summary>
  /// <returns>当前语言代码</returns>
  string GetCurrentLanguage();

  /// <summary>
  /// 异步获取当前语言
  /// </summary>
  /// <returns>当前语言代码</returns>
  Task<string> GetCurrentLanguageAsync();

  /// <summary>
  /// 获取支持的语言列表
  /// </summary>
  Task<List<string>> GetSupportedLanguagesAsync();

  /// <summary>
  /// 获取指定语言的翻译
  /// </summary>
  Task<string> GetTranslationAsync(string language, string key);
}