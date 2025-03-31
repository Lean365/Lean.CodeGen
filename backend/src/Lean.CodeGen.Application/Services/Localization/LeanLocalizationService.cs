using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Common.Localization;
using Lean.CodeGen.Common.Options;
using Lean.CodeGen.Domain.Entities.Admin;
using Lean.CodeGen.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Caching.Memory;
using NLog;
using Microsoft.Extensions.Options;

namespace Lean.CodeGen.Application.Services.Localization;

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
  /// 获取本地化文本
  /// </summary>
  public string GetLocalizedText(string key)
  {
    if (string.IsNullOrEmpty(key))
    {
      return string.Empty;
    }

    try
    {
      var cacheKey = $"{CacheKeyPrefix}{key}";
      if (_cache.TryGetValue(cacheKey, out string? value))
      {
        return value ?? key;
      }

      var language = GetCurrentLanguage();
      if (string.IsNullOrEmpty(language))
      {
        return key;
      }

      var languageEntity = _languageRepository.GetListAsync(x => x.LangCode == language).Result.FirstOrDefault();
      if (languageEntity == null)
      {
        return key;
      }

      var translation = _translationRepository.GetListAsync(x =>
        x.LangId == languageEntity.Id && x.TransKey == key && x.TransStatus == 0).Result.FirstOrDefault();

      if (translation == null && language != _options.DefaultLanguage)
      {
        // 如果当前语言不是默认语言，尝试获取默认语言的翻译
        var defaultLanguageEntity = _languageRepository.GetListAsync(x => x.LangCode == _options.DefaultLanguage).Result.FirstOrDefault();
        if (defaultLanguageEntity != null)
        {
          translation = _translationRepository.GetListAsync(x =>
            x.LangId == defaultLanguageEntity.Id && x.TransKey == key && x.TransStatus == 0).Result.FirstOrDefault();
        }
      }

      if (translation == null)
      {
        return key;
      }

      value = translation.TransValue;
      _cache.Set(cacheKey, value, TimeSpan.FromMinutes(CacheExpirationMinutes));
      return value;
    }
    catch (Exception ex)
    {
      _logger.Error(ex, "获取本地化文本失败: {Key}", key);
      return key;
    }
  }

  /// <summary>
  /// 异步获取本地化文本
  /// </summary>
  public async Task<string> GetLocalizedTextAsync(string key)
  {
    if (string.IsNullOrEmpty(key))
    {
      return string.Empty;
    }

    try
    {
      var cacheKey = $"{CacheKeyPrefix}{key}";
      if (_cache.TryGetValue(cacheKey, out string? value))
      {
        return value ?? key;
      }

      var language = await GetCurrentLanguageAsync();
      if (string.IsNullOrEmpty(language))
      {
        return key;
      }

      var languageEntity = (await _languageRepository.GetListAsync(x => x.LangCode == language)).FirstOrDefault();
      if (languageEntity == null)
      {
        return key;
      }

      var translations = await _translationRepository.GetListAsync(x =>
        x.LangId == languageEntity.Id && x.TransKey == key && x.TransStatus == 0);
      var translation = translations.FirstOrDefault();

      if (translation == null && language != _options.DefaultLanguage)
      {
        // 如果当前语言不是默认语言，尝试获取默认语言的翻译
        var defaultLanguageEntity = (await _languageRepository.GetListAsync(x => x.LangCode == _options.DefaultLanguage)).FirstOrDefault();
        if (defaultLanguageEntity != null)
        {
          translations = await _translationRepository.GetListAsync(x =>
            x.LangId == defaultLanguageEntity.Id && x.TransKey == key && x.TransStatus == 0);
          translation = translations.FirstOrDefault();
        }
      }

      if (translation == null)
      {
        return key;
      }

      value = translation.TransValue;
      _cache.Set(cacheKey, value, TimeSpan.FromMinutes(CacheExpirationMinutes));
      return value;
    }
    catch (Exception ex)
    {
      _logger.Error(ex, "获取本地化文本失败: {Key}", key);
      return key;
    }
  }

  /// <summary>
  /// 获取当前语言
  /// </summary>
  public string GetCurrentLanguage()
  {
    try
    {
      // 从缓存获取当前语言
      if (_cache.TryGetValue(LanguageListCacheKey, out List<LeanLanguage>? languages))
      {
        return languages?.FirstOrDefault(x => x.LangCode == _options.DefaultLanguage)?.LangCode ?? _options.DefaultLanguage;
      }

      // 从数据库获取语言列表
      languages = _languageRepository.GetListAsync(x => x.LangStatus == 0).Result;
      if (languages == null || !languages.Any())
      {
        return _options.DefaultLanguage;
      }

      // 更新缓存
      _cache.Set(LanguageListCacheKey, languages, TimeSpan.FromMinutes(CacheExpirationMinutes));

      return languages.FirstOrDefault(x => x.LangCode == _options.DefaultLanguage)?.LangCode ?? _options.DefaultLanguage;
    }
    catch (Exception ex)
    {
      _logger.Error(ex, "获取当前语言失败");
      return _options.DefaultLanguage;
    }
  }

  /// <summary>
  /// 异步获取当前语言
  /// </summary>
  public async Task<string> GetCurrentLanguageAsync()
  {
    try
    {
      // 从缓存获取当前语言
      if (_cache.TryGetValue(LanguageListCacheKey, out List<LeanLanguage>? languages))
      {
        return languages?.FirstOrDefault(x => x.LangCode == _options.DefaultLanguage)?.LangCode ?? _options.DefaultLanguage;
      }

      // 从数据库获取语言列表
      languages = await _languageRepository.GetListAsync(x => x.LangStatus == 0);
      if (languages == null || !languages.Any())
      {
        return _options.DefaultLanguage;
      }

      // 更新缓存
      _cache.Set(LanguageListCacheKey, languages, TimeSpan.FromMinutes(CacheExpirationMinutes));

      return languages.FirstOrDefault(x => x.LangCode == _options.DefaultLanguage)?.LangCode ?? _options.DefaultLanguage;
    }
    catch (Exception ex)
    {
      _logger.Error(ex, "获取当前语言失败");
      return _options.DefaultLanguage;
    }
  }

  /// <summary>
  /// 获取支持的语言列表
  /// </summary>
  public async Task<List<string>> GetSupportedLanguagesAsync()
  {
    try
    {
      var languages = await _languageRepository.GetListAsync(x => x.LangStatus == 0);
      return languages.Select(x => x.LangCode).ToList();
    }
    catch (Exception ex)
    {
      _logger.Error(ex, "获取支持的语言列表失败");
      return new List<string> { _options.DefaultLanguage };
    }
  }

  /// <summary>
  /// 获取指定语言的翻译
  /// </summary>
  public async Task<string> GetTranslationAsync(string language, string key)
  {
    if (string.IsNullOrEmpty(key))
    {
      return string.Empty;
    }

    try
    {
      var languageEntity = (await _languageRepository.GetListAsync(x => x.LangCode == language)).FirstOrDefault();
      if (languageEntity == null)
      {
        return key;
      }

      var translations = await _translationRepository.GetListAsync(x =>
        x.LangId == languageEntity.Id && x.TransKey == key && x.TransStatus == 0);
      var translation = translations.FirstOrDefault();

      if (translation == null && language != _options.DefaultLanguage)
      {
        var defaultLanguageEntity = (await _languageRepository.GetListAsync(x => x.LangCode == _options.DefaultLanguage)).FirstOrDefault();
        if (defaultLanguageEntity != null)
        {
          translations = await _translationRepository.GetListAsync(x =>
            x.LangId == defaultLanguageEntity.Id && x.TransKey == key && x.TransStatus == 0);
          translation = translations.FirstOrDefault();
        }
      }

      return translation?.TransValue ?? key;
    }
    catch (Exception ex)
    {
      _logger.Error(ex, "获取翻译失败: {Key}", key);
      return key;
    }
  }
}