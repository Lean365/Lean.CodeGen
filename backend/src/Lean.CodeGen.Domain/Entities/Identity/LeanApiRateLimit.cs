namespace Lean.CodeGen.Domain.Entities.Identity;

/// <summary>
/// API访问频率限制
/// </summary>
public class LeanApiRateLimit : LeanBaseEntity
{
  /// <summary>
  /// API ID
  /// </summary>
  public long ApiId { get; set; }

  /// <summary>
  /// 时间窗口(秒)
  /// </summary>
  public int TimeWindow { get; set; }

  /// <summary>
  /// 最大请求次数
  /// </summary>
  public int MaxRequests { get; set; }
}