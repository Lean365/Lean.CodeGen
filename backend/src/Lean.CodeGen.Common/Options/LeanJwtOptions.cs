using System;

namespace Lean.CodeGen.Common.Options;

/// <summary>
/// JWT配置选项
/// </summary>
public class LeanJwtOptions
{
  /// <summary>
  /// 密钥
  /// </summary>
  public string SecretKey { get; set; }

  /// <summary>
  /// 发行者
  /// </summary>
  public string Issuer { get; set; }

  /// <summary>
  /// 接收者
  /// </summary>
  public string Audience { get; set; } = string.Empty;

  /// <summary>
  /// 过期时间（分钟）
  /// </summary>
  public int ExpireMinutes { get; set; } = 120;

  /// <summary>
  /// 刷新令牌过期时间（分钟）
  /// </summary>
  public int RefreshExpireMinutes { get; set; } = 10080; // 7天
}