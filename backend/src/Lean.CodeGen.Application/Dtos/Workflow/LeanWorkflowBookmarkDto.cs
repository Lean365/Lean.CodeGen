namespace Lean.CodeGen.Application.Dtos.Workflow;

/// <summary>
/// 工作流书签DTO
/// </summary>
public class LeanWorkflowBookmarkDto
{
  /// <summary>
  /// ID
  /// </summary>
  public long Id { get; set; }

  /// <summary>
  /// 工作流实例ID
  /// </summary>
  public long InstanceId { get; set; }

  /// <summary>
  /// 活动ID
  /// </summary>
  public string ActivityId { get; set; } = string.Empty;

  /// <summary>
  /// 活动名称
  /// </summary>
  public string ActivityName { get; set; } = string.Empty;

  /// <summary>
  /// 书签名称
  /// </summary>
  public string BookmarkName { get; set; } = string.Empty;

  /// <summary>
  /// 书签数据JSON
  /// </summary>
  public string? BookmarkData { get; set; }

  /// <summary>
  /// 关联键
  /// </summary>
  public string? CorrelationId { get; set; }

  /// <summary>
  /// 书签状态
  /// </summary>
  public bool Status { get; set; }

  /// <summary>
  /// 过期时间
  /// </summary>
  public DateTime? ExpireTime { get; set; }

  /// <summary>
  /// 自定义属性JSON
  /// </summary>
  public string? CustomAttributes { get; set; }

  /// <summary>
  /// 创建时间
  /// </summary>
  public DateTime CreateTime { get; set; }

  /// <summary>
  /// 更新时间
  /// </summary>
  public DateTime? UpdateTime { get; set; }
}