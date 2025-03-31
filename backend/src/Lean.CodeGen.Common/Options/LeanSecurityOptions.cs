using System;
using System.Collections.Generic;

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
  /// 账户配置
  /// </summary>
  public LeanAccountOptions Account { get; set; } = new();

  /// <summary>
  /// 密码配置
  /// </summary>
  public LeanPasswordOptions Password { get; set; } = new();

  /// <summary>
  /// 默认密码
  /// </summary>
  public string DefaultPassword { get; set; } = "123456";

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

  /// <summary>
  /// 登录配置
  /// </summary>
  public LeanLoginOptions Login { get; set; } = new();
}

/// <summary>
/// 账户配置选项
/// </summary>
public class LeanAccountOptions
{
  /// <summary>
  /// 最大密码错误次数
  /// </summary>
  public int MaxPasswordErrorCount { get; set; } = 5;

  /// <summary>
  /// 锁定时长（分钟）
  /// </summary>
  public int LockDuration { get; set; } = 30;

  /// <summary>
  /// 错误次数重置时间（小时）
  /// </summary>
  public int ErrorCountResetHours { get; set; } = 24;

  /// <summary>
  /// 是否启用永久锁定
  /// </summary>
  public bool EnablePermanentLock { get; set; } = true;

  /// <summary>
  /// 触发永久锁定的错误次数
  /// </summary>
  public int PermanentLockThreshold { get; set; } = 10;

  /// <summary>
  /// 是否启用通知
  /// </summary>
  public bool EnableNotification { get; set; } = true;

  /// <summary>
  /// 开始发送警告通知的错误次数阈值
  /// </summary>
  public int NotificationThreshold { get; set; } = 3;
}

/// <summary>
/// 密码配置选项
/// </summary>
public class LeanPasswordOptions
{
  /// <summary>
  /// 最小长度
  /// </summary>
  public int MinLength { get; set; } = 8;

  /// <summary>
  /// 必须包含数字
  /// </summary>
  public bool RequireDigit { get; set; } = true;

  /// <summary>
  /// 必须包含小写字母
  /// </summary>
  public bool RequireLowercase { get; set; } = true;

  /// <summary>
  /// 必须包含大写字母
  /// </summary>
  public bool RequireUppercase { get; set; } = true;

  /// <summary>
  /// 必须包含特殊字符
  /// </summary>
  public bool RequireSpecialChar { get; set; } = true;

  /// <summary>
  /// 密码过期天数
  /// </summary>
  public int ExpirationDays { get; set; } = 90;

  /// <summary>
  /// 密码历史记录限制
  /// </summary>
  public int HistoryLimit { get; set; } = 3;
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