using Lean.CodeGen.Domain.Entities;

namespace Lean.CodeGen.Domain.Entities.Identity;

/// <summary>
/// API访问日志
/// </summary>
public class LeanApiAccessLog : LeanBaseEntity
{
  /// <summary>
  /// API ID
  /// </summary>
  public long ApiId { get; set; }

  /// <summary>
  /// 用户ID
  /// </summary>
  public long UserId { get; set; }

  /// <summary>
  /// 请求路径
  /// </summary>
  public string Path { get; set; } = string.Empty;

  /// <summary>
  /// 请求方法
  /// </summary>
  public string Method { get; set; } = string.Empty;

  /// <summary>
  /// 访问时间
  /// </summary>
  public DateTime AccessTime { get; set; }
}