namespace Lean.CodeGen.Common.Options;

/// <summary>
/// 本地化选项
/// </summary>
public class LeanLocalizationOptions
{
  /// <summary>
  /// 配置节点名称
  /// </summary>
  public const string Position = "LocalizationSettings";

  /// <summary>
  /// 默认语言
  /// </summary>
  public string DefaultLanguage { get; set; } = "zh-CN";

  /// <summary>
  /// 系统语言
  /// </summary>
  public string SystemLanguage { get; set; } = "zh-CN";

  /// <summary>
  /// 支持的语言列表
  /// </summary>
  public List<LeanLanguageInfo> SupportedLanguages { get; set; } = new();

  /// <summary>
  /// 是否启用自动检测语言
  /// </summary>
  public bool EnableAutoDetectLanguage { get; set; } = true;

  /// <summary>
  /// 是否启用语言Cookie
  /// </summary>
  public bool EnableLanguageCookie { get; set; } = true;

  /// <summary>
  /// 语言Cookie名称
  /// </summary>
  public string LanguageCookieName { get; set; } = "lang";

  /// <summary>
  /// 语言Cookie过期天数
  /// </summary>
  public int LanguageCookieExpireDays { get; set; } = 30;

  /// <summary>
  /// 缓存过期时间（分钟）
  /// </summary>
  public int CacheExpirationMinutes { get; set; } = 30;

  /// <summary>
  /// 是否启用缓存
  /// </summary>
  public bool EnableCache { get; set; } = true;
}

/// <summary>
/// 语言信息
/// </summary>
public class LeanLanguageInfo
{
  /// <summary>
  /// 语言代码
  /// </summary>
  public string Code { get; set; } = string.Empty;

  /// <summary>
  /// 语言名称
  /// </summary>
  public string Name { get; set; } = string.Empty;

  /// <summary>
  /// 语言图标
  /// </summary>
  public string Icon { get; set; } = string.Empty;
}