namespace Lean.CodeGen.Application.Dtos.Workflow;

/// <summary>
/// 工作流关联DTO
/// </summary>
public class LeanWorkflowCorrelationDto
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
  /// 关联键
  /// </summary>
  public string CorrelationId { get; set; } = string.Empty;

  /// <summary>
  /// 关联类型
  /// </summary>
  public string CorrelationType { get; set; } = string.Empty;

  /// <summary>
  /// 关联值
  /// </summary>
  public string? CorrelationValue { get; set; }

  /// <summary>
  /// 关联状态
  /// </summary>
  public bool Status { get; set; }

  /// <summary>
  /// 关联数据JSON
  /// </summary>
  public string? CorrelationData { get; set; }

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
