namespace Lean.CodeGen.Application.Dtos.Workflow;

/// <summary>
/// 工作流指标DTO
/// </summary>
public class LeanWorkflowMetricDto
{
  /// <summary>
  /// ID
  /// </summary>
  public long Id { get; set; }

  /// <summary>
  /// 工作流定义ID
  /// </summary>
  public long DefinitionId { get; set; }

  /// <summary>
  /// 工作流实例ID
  /// </summary>
  public long? InstanceId { get; set; }

  /// <summary>
  /// 指标名称
  /// </summary>
  public string MetricName { get; set; } = string.Empty;

  /// <summary>
  /// 指标类型(Counter=计数器,Gauge=仪表盘,Histogram=直方图,Summary=摘要)
  /// </summary>
  public string MetricType { get; set; } = string.Empty;

  /// <summary>
  /// 指标值
  /// </summary>
  public decimal MetricValue { get; set; }

  /// <summary>
  /// 指标单位
  /// </summary>
  public string? MetricUnit { get; set; }

  /// <summary>
  /// 指标标签JSON
  /// </summary>
  public string? MetricLabels { get; set; }

  /// <summary>
  /// 指标描述
  /// </summary>
  public string? Description { get; set; }

  /// <summary>
  /// 指标时间
  /// </summary>
  public DateTime MetricTime { get; set; }

  /// <summary>
  /// 创建时间
  /// </summary>
  public DateTime CreateTime { get; set; }
}