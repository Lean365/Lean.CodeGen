using System;

namespace Lean.CodeGen.Common.Options;

/// <summary>
/// 安全配置选项
/// </summary>
public class LeanSecurityOptions
{
  /// <summary>
  /// 配置节点
  /// </summary>
  public const string Position = "Security";

  /// <summary>
  /// 默认密码
  /// </summary>
  public string DefaultPassword { get; set; } = "123456";

  /// <summary>
  /// 最大密码错误次数
  /// </summary>
  public int MaxPasswordAttempts { get; set; } = 5;

  /// <summary>
  /// 密码锁定时间（分钟）
  /// </summary>
  public int PasswordLockMinutes { get; set; } = 30;

  /// <summary>
  /// 密码过期天数
  /// </summary>
  public int PasswordExpireDays { get; set; } = 90;

  /// <summary>
  /// JWT配置
  /// </summary>
  public LeanJwtOptions Jwt { get; set; } = new();

  /// <summary>
  /// 防伪配置
  /// </summary>
  public LeanAntiforgeryOptions Antiforgery { get; set; } = new();

  /// <summary>
  /// 是否启用防伪验证
  /// </summary>
  public bool EnableAntiforgery { get; set; } = true;

  /// <summary>
  /// 是否启用限流
  /// </summary>
  public bool EnableRateLimit { get; set; } = true;

  /// <summary>
  /// 是否启用SQL注入防护
  /// </summary>
  public bool EnableSqlInjection { get; set; } = true;

  /// <summary>
  /// 限流配置
  /// </summary>
  public RateLimitOptions RateLimit { get; set; } = new();

  /// <summary>
  /// SQL注入配置
  /// </summary>
  public SqlInjectionOptions SqlInjection { get; set; } = new();
}

/// <summary>
/// 防伪配置
/// </summary>
public class AntiforgeryOptions
{
  /// <summary>
  /// 请求头名称
  /// </summary>
  public string HeaderName { get; set; } = "X-XSRF-TOKEN";

  /// <summary>
  /// Cookie名称
  /// </summary>
  public string CookieName { get; set; } = "XSRF-TOKEN";
}

/// <summary>
/// 限流配置
/// </summary>
public class RateLimitOptions
{
  /// <summary>
  /// 默认限流
  /// </summary>
  public RateLimitRule DefaultRateLimit { get; set; } = new();

  /// <summary>
  /// IP限流
  /// </summary>
  public RateLimitRule IpRateLimit { get; set; } = new();

  /// <summary>
  /// 用户限流
  /// </summary>
  public RateLimitRule UserRateLimit { get; set; } = new();

  /// <summary>
  /// 接口限流
  /// </summary>
  public Dictionary<string, RateLimitRule> EndpointRateLimits { get; set; } = new();

  /// <summary>
  /// IP白名单
  /// </summary>
  public List<string> IpWhitelist { get; set; } = new();

  /// <summary>
  /// 用户白名单
  /// </summary>
  public List<string> UserWhitelist { get; set; } = new();
}

/// <summary>
/// 限流规则
/// </summary>
public class RateLimitRule
{
  /// <summary>
  /// 时间窗口（秒）
  /// </summary>
  public int Seconds { get; set; } = 60;

  /// <summary>
  /// 最大请求次数
  /// </summary>
  public int MaxRequests { get; set; } = 100;
}

/// <summary>
/// SQL注入配置
/// </summary>
public class SqlInjectionOptions
{
  /// <summary>
  /// 阻止的关键字
  /// </summary>
  public List<string> BlockedKeywords { get; set; } = new();
}