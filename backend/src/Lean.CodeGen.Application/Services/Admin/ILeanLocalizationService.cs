using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lean.CodeGen.Application.Services.Admin;

/// <summary>
/// 本地化服务接口
/// </summary>
public interface ILeanLocalizationService
{
  /// <summary>
  /// 获取指定语言的所有翻译
  /// </summary>
  /// <param name="langCode">语言代码</param>
  /// <returns>翻译字典</returns>
  Task<Dictionary<string, string>> GetTranslationsAsync(string langCode);

  /// <summary>
  /// 获取指定语言的指定模块的翻译
  /// </summary>
  /// <param name="langCode">语言代码</param>
  /// <param name="moduleName">模块名称</param>
  /// <returns>翻译字典</returns>
  Task<Dictionary<string, string>> GetModuleTranslationsAsync(string langCode, string moduleName);

  /// <summary>
  /// 获取指定键的翻译
  /// </summary>
  /// <param name="langCode">语言代码</param>
  /// <param name="key">翻译键</param>
  /// <returns>翻译值</returns>
  Task<string> GetTranslationAsync(string langCode, string key);

  /// <summary>
  /// 获取所有支持的语言列表
  /// </summary>
  /// <returns>语言代码列表</returns>
  Task<List<string>> GetSupportedLanguagesAsync();

  /// <summary>
  /// 刷新翻译缓存
  /// </summary>
  Task RefreshCacheAsync();
}