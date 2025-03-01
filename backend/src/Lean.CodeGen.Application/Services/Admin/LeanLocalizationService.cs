using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Common.Options;
using Lean.CodeGen.Domain.Entities.Admin;
using Lean.CodeGen.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Caching.Memory;
using NLog;
using Microsoft.Extensions.Options;

namespace Lean.CodeGen.Application.Services.Admin;

/// <summary>
/// 本地化服务实现
/// </summary>
public class LeanLocalizationService : ILeanLocalizationService
{
  private readonly ILogger _logger;
  private readonly ILeanRepository<LeanTranslation> _translationRepository;
  private readonly ILeanRepository<LeanLanguage> _languageRepository;
  private readonly IMemoryCache _cache;
  private readonly LeanLocalizationOptions _options;
  private readonly ConcurrentDictionary<string, DateTime> _cacheTimestamps;
  private const string CacheKeyPrefix = "Translation_";
  private const string LanguageListCacheKey = "LanguageList";
  private const int CacheExpirationMinutes = 30;

  /// <summary>
  /// 构造函数
  /// </summary>
  public LeanLocalizationService(
      ILogger logger,
      ILeanRepository<LeanTranslation> translationRepository,
      ILeanRepository<LeanLanguage> languageRepository,
      IMemoryCache cache,
      IOptions<LeanLocalizationOptions> options)
  {
    _logger = logger;
    _translationRepository = translationRepository;
    _languageRepository = languageRepository;
    _cache = cache;
    _options = options.Value;
    _cacheTimestamps = new ConcurrentDictionary<string, DateTime>();
  }

  /// <summary>
  /// 获取指定语言的所有翻译
  /// </summary>
  public async Task<Dictionary<string, string>> GetTranslationsAsync(string langCode)
  {
    try
    {
      var cacheKey = $"{CacheKeyPrefix}{langCode}";
      if (_cache.TryGetValue(cacheKey, out Dictionary<string, string> cachedTranslations))
      {
        return cachedTranslations;
      }

      var language = await _languageRepository.FirstOrDefaultAsync(x => x.LangCode == langCode && x.Status == LeanStatus.Normal);
      if (language == null)
      {
        _logger.Warn($"Language not found: {langCode}, falling back to default language: {_options.DefaultLanguage}");
        return await GetTranslationsAsync(_options.DefaultLanguage);
      }

      var translations = await _translationRepository.GetListAsync(x =>
          x.LangId == language.Id &&
          x.Status == LeanStatus.Normal);

      var result = translations.ToDictionary(x => x.TransKey, x => x.TransValue);

      var cacheOptions = new MemoryCacheEntryOptions()
          .SetAbsoluteExpiration(TimeSpan.FromMinutes(CacheExpirationMinutes));

      _cache.Set(cacheKey, result, cacheOptions);
      _cacheTimestamps.TryAdd(cacheKey, DateTime.UtcNow);

      return result;
    }
    catch (Exception ex)
    {
      _logger.Error(ex, $"Error getting translations for language: {langCode}");
      return new Dictionary<string, string>();
    }
  }

  /// <summary>
  /// 获取指定语言的指定模块的翻译
  /// </summary>
  public async Task<Dictionary<string, string>> GetModuleTranslationsAsync(string langCode, string moduleName)
  {
    try
    {
      var allTranslations = await GetTranslationsAsync(langCode);
      return allTranslations
          .Where(x => x.Key.StartsWith(moduleName + ".", StringComparison.OrdinalIgnoreCase))
          .ToDictionary(x => x.Key, x => x.Value);
    }
    catch (Exception ex)
    {
      _logger.Error(ex, $"Error getting module translations for language: {langCode}, module: {moduleName}");
      return new Dictionary<string, string>();
    }
  }

  /// <summary>
  /// 获取指定键的翻译
  /// </summary>
  public async Task<string> GetTranslationAsync(string langCode, string key)
  {
    try
    {
      var translations = await GetTranslationsAsync(langCode);
      return translations.TryGetValue(key, out var value) ? value : key;
    }
    catch (Exception ex)
    {
      _logger.Error(ex, $"Error getting translation for language: {langCode}, key: {key}");
      return key;
    }
  }

  /// <summary>
  /// 获取所有支持的语言列表
  /// </summary>
  public async Task<List<string>> GetSupportedLanguagesAsync()
  {
    try
    {
      if (_cache.TryGetValue(LanguageListCacheKey, out List<string> cachedLanguages))
      {
        return cachedLanguages;
      }

      var languages = await _languageRepository.GetListAsync(x => x.Status == LeanStatus.Normal);
      var result = languages.Select(x => x.LangCode).ToList();

      var cacheOptions = new MemoryCacheEntryOptions()
          .SetAbsoluteExpiration(TimeSpan.FromMinutes(CacheExpirationMinutes));

      _cache.Set(LanguageListCacheKey, result, cacheOptions);

      return result;
    }
    catch (Exception ex)
    {
      _logger.Error(ex, $"Error getting supported languages");
      return new List<string>();
    }
  }

  /// <summary>
  /// 刷新翻译缓存
  /// </summary>
  public async Task RefreshCacheAsync()
  {
    try
    {
      var languages = await GetSupportedLanguagesAsync();
      foreach (var langCode in languages)
      {
        var cacheKey = $"{CacheKeyPrefix}{langCode}";
        _cache.Remove(cacheKey);
        _cacheTimestamps.TryRemove(cacheKey, out _);
      }
      _cache.Remove(LanguageListCacheKey);
      _logger.Info("Translation cache refreshed successfully");
    }
    catch (Exception ex)
    {
      _logger.Error(ex, "Error refreshing translation cache");
    }
  }
}