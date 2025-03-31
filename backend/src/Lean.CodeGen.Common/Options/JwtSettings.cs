namespace Lean.CodeGen.Common.Options;

/// <summary>
/// JWT设置
/// </summary>
public class JwtSettings
{
  /// <summary>
  /// 密钥
  /// </summary>
  public string SecretKey { get; set; } = string.Empty;

  /// <summary>
  /// 发行者
  /// </summary>
  public string Issuer { get; set; } = string.Empty;

  /// <summary>
  /// 接收者
  /// </summary>
  public string Audience { get; set; } = string.Empty;

  /// <summary>
  /// 过期时间（分钟）
  /// </summary>
  public int ExpiresInMinutes { get; set; }
}