namespace Lean.CodeGen.Application.Dtos.Workflow;

/// <summary>
/// 工作流结果DTO
/// </summary>
public class LeanWorkflowOutcomeDto
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
  /// 结果名称
  /// </summary>
  public string OutcomeName { get; set; } = string.Empty;

  /// <summary>
  /// 结果类型
  /// </summary>
  public string OutcomeType { get; set; } = string.Empty;

  /// <summary>
  /// 结果值JSON
  /// </summary>
  public string? OutcomeValue { get; set; }

  /// <summary>
  /// 结果状态
  /// </summary>
  public bool Status { get; set; }

  /// <summary>
  /// 优先级
  /// </summary>
  public int Priority { get; set; }

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
