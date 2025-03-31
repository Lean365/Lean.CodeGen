namespace Lean.CodeGen.Common.Options;

/// <summary>
/// 登录配置选项
/// </summary>
public class LeanLoginOptions
{
  /// <summary>
  /// 是否启用单点登录
  /// </summary>
  public bool EnableSingleSignOn { get; set; }

  /// <summary>
  /// 是否强制登出其他设备
  /// </summary>
  public bool ForceLogoutOtherDevices { get; set; }

  /// <summary>
  /// 最大并发会话数
  /// </summary>
  public int MaxConcurrentSessions { get; set; } = 1;
}