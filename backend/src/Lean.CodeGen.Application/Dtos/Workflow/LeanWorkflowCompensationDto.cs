namespace Lean.CodeGen.Application.Dtos.Workflow;

/// <summary>
/// 工作流补偿DTO
/// </summary>
public class LeanWorkflowCompensationDto
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
  /// 补偿活动ID
  /// </summary>
  public string CompensationActivityId { get; set; } = string.Empty;

  /// <summary>
  /// 补偿活动名称
  /// </summary>
  public string CompensationActivityName { get; set; } = string.Empty;

  /// <summary>
  /// 补偿类型
  /// </summary>
  public string CompensationType { get; set; } = string.Empty;

  /// <summary>
  /// 补偿状态
  /// </summary>
  public int CompensationStatus { get; set; }

  /// <summary>
  /// 补偿数据JSON
  /// </summary>
  public string? CompensationData { get; set; }

  /// <summary>
  /// 开始时间
  /// </summary>
  public DateTime? StartTime { get; set; }

  /// <summary>
  /// 结束时间
  /// </summary>
  public DateTime? EndTime { get; set; }

  /// <summary>
  /// 错误信息JSON
  /// </summary>
  public string? ErrorInfo { get; set; }

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
