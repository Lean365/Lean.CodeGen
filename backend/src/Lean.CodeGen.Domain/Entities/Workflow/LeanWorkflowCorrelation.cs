using SqlSugar;

namespace Lean.CodeGen.Domain.Entities.Workflow;

/// <summary>
/// 工作流关联实体
/// </summary>
[SugarTable("lean_wk_correlation", "工作流关联表")]
[SugarIndex("idx_instance", nameof(InstanceId), OrderByType.Asc)]
[SugarIndex("uk_correlation_id", nameof(CorrelationId), OrderByType.Asc, true)]
public class LeanWorkflowCorrelation : LeanBaseEntity
{
  /// <summary>
  /// 工作流实例ID
  /// </summary>
  [SugarColumn(ColumnName = "instance_id", ColumnDescription = "工作流实例ID", IsNullable = false)]
  public long InstanceId { get; set; }

  /// <summary>
  /// 工作流实例
  /// </summary>
  [Navigate(NavigateType.OneToOne, nameof(InstanceId))]
  public virtual LeanWorkflowInstance Instance { get; set; } = null!;

  /// <summary>
  /// 关联键
  /// </summary>
  [SugarColumn(ColumnName = "correlation_id", ColumnDescription = "关联键", Length = 50, IsNullable = false)]
  public string CorrelationId { get; set; } = string.Empty;

  /// <summary>
  /// 关联类型
  /// </summary>
  [SugarColumn(ColumnName = "correlation_type", ColumnDescription = "关联类型", Length = 50, IsNullable = false)]
  public string CorrelationType { get; set; } = string.Empty;

  /// <summary>
  /// 关联值
  /// </summary>
  [SugarColumn(ColumnName = "correlation_value", ColumnDescription = "关联值", IsNullable = true)]
  public string? CorrelationValue { get; set; }

  /// <summary>
  /// 关联状态
  /// </summary>
  [SugarColumn(ColumnName = "status", ColumnDescription = "关联状态", IsNullable = false)]
  public bool Status { get; set; }

  /// <summary>
  /// 关联数据JSON
  /// </summary>
  [SugarColumn(ColumnName = "correlation_data", ColumnDescription = "关联数据JSON", IsNullable = true)]
  public string? CorrelationData { get; set; }

  /// <summary>
  /// 自定义属性JSON
  /// </summary>
  [SugarColumn(ColumnName = "custom_attributes", ColumnDescription = "自定义属性JSON", IsNullable = true)]
  public string? CustomAttributes { get; set; }
}