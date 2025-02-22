namespace Lean.CodeGen.Application.Dtos.Workflow;

/// <summary>
/// 工作流故障DTO
/// </summary>
public class LeanWorkflowFaultDto
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
  /// 活动实例ID
  /// </summary>
  public long? ActivityInstanceId { get; set; }

  /// <summary>
  /// 活动ID
  /// </summary>
  public string? ActivityId { get; set; }

  /// <summary>
  /// 活动名称
  /// </summary>
  public string? ActivityName { get; set; }

  /// <summary>
  /// 故障类型
  /// </summary>
  public string FaultType { get; set; } = string.Empty;

  /// <summary>
  /// 故障消息
  /// </summary>
  public string Message { get; set; } = string.Empty;

  /// <summary>
  /// 故障详情
  /// </summary>
  public string? Details { get; set; }

  /// <summary>
  /// 异常信息
  /// </summary>
  public string? Exception { get; set; }

  /// <summary>
  /// 堆栈跟踪
  /// </summary>
  public string? StackTrace { get; set; }

  /// <summary>
  /// 故障数据JSON
  /// </summary>
  public string? FaultData { get; set; }

  /// <summary>
  /// 是否已处理
  /// </summary>
  public bool IsHandled { get; set; }

  /// <summary>
  /// 处理时间
  /// </summary>
  public DateTime? HandleTime { get; set; }

  /// <summary>
  /// 处理人ID
  /// </summary>
  public long? HandleUserId { get; set; }

  /// <summary>
  /// 处理人名称
  /// </summary>
  public string? HandleUserName { get; set; }

  /// <summary>
  /// 处理备注
  /// </summary>
  public string? HandleRemark { get; set; }

  /// <summary>
  /// 创建时间
  /// </summary>
  public DateTime CreateTime { get; set; }

  /// <summary>
  /// 更新时间
  /// </summary>
  public DateTime? UpdateTime { get; set; }
}