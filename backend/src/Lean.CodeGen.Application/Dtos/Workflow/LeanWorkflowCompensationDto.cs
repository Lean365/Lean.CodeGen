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
  /// 活动实例ID
  /// </summary>
  public long ActivityInstanceId { get; set; }

  /// <summary>
  /// 活动实例
  /// </summary>
  public LeanWorkflowActivityInstanceDto ActivityInstance { get; set; } = null!;

  /// <summary>
  /// 补偿时间
  /// </summary>
  public DateTime CompensationTime { get; set; }

  /// <summary>
  /// 补偿原因
  /// </summary>
  public string? CompensationReason { get; set; }

  /// <summary>
  /// 补偿数据JSON
  /// </summary>
  public string? CompensationData { get; set; }

  /// <summary>
  /// 补偿结果JSON
  /// </summary>
  public string? CompensationResult { get; set; }

  /// <summary>
  /// 创建时间
  /// </summary>
  public DateTime CreateTime { get; set; }

  /// <summary>
  /// 更新时间
  /// </summary>
  public DateTime? UpdateTime { get; set; }
}
