namespace Lean.CodeGen.Common.Options;

/// <summary>
/// 邮件配置选项
/// </summary>
public class LeanMailOptions
{
  /// <summary>
  /// 配置节点名称
  /// </summary>
  public const string Position = "Mail";

  /// <summary>
  /// SMTP服务器地址
  /// </summary>
  public string SmtpServer { get; set; } = string.Empty;

  /// <summary>
  /// SMTP服务器端口
  /// </summary>
  public int SmtpPort { get; set; } = 587;

  /// <summary>
  /// 是否启用SSL
  /// </summary>
  public bool EnableSsl { get; set; } = true;

  /// <summary>
  /// 用户名
  /// </summary>
  public string UserName { get; set; } = string.Empty;

  /// <summary>
  /// 密码
  /// </summary>
  public string Password { get; set; } = string.Empty;

  /// <summary>
  /// 默认发件人地址
  /// </summary>
  public string DefaultFromAddress { get; set; } = string.Empty;

  /// <summary>
  /// 默认发件人显示名称
  /// </summary>
  public string DefaultFromName { get; set; } = string.Empty;

  /// <summary>
  /// 重试次数
  /// </summary>
  public int RetryCount { get; set; } = 3;

  /// <summary>
  /// 重试间隔（秒）
  /// </summary>
  public int RetryInterval { get; set; } = 5;

  /// <summary>
  /// 超时时间（秒）
  /// </summary>
  public int Timeout { get; set; } = 30;
}