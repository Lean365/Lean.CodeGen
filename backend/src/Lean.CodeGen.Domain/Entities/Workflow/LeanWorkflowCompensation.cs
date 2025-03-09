using SqlSugar;

namespace Lean.CodeGen.Domain.Entities.Workflow;

/// <summary>
/// 工作流补偿实体
/// </summary>
[SugarTable("lean_wk_compensation", "工作流补偿表")]
[SugarIndex("idx_activity_instance", nameof(ActivityInstanceId), OrderByType.Asc)]
public class LeanWorkflowCompensation : LeanBaseEntity
{
  /// <summary>
  /// 活动实例ID
  /// </summary>
  [SugarColumn(ColumnName = "activity_instance_id", ColumnDescription = "活动实例ID", IsNullable = false)]
  public long ActivityInstanceId { get; set; }

  /// <summary>
  /// 活动实例
  /// </summary>
  [Navigate(NavigateType.OneToOne, nameof(ActivityInstanceId))]
  public virtual LeanWorkflowActivityInstance ActivityInstance { get; set; } = null!;

  /// <summary>
  /// 补偿时间
  /// </summary>
  [SugarColumn(ColumnName = "compensation_time", ColumnDescription = "补偿时间", IsNullable = false)]
  public DateTime CompensationTime { get; set; }

  /// <summary>
  /// 补偿原因
  /// </summary>
  [SugarColumn(ColumnName = "compensation_reason", ColumnDescription = "补偿原因", Length = 500, IsNullable = true)]
  public string? CompensationReason { get; set; }

  /// <summary>
  /// 补偿数据JSON
  /// </summary>
  [SugarColumn(ColumnName = "compensation_data", ColumnDescription = "补偿数据JSON", IsNullable = true)]
  public string? CompensationData { get; set; }

  /// <summary>
  /// 补偿结果JSON
  /// </summary>
  [SugarColumn(ColumnName = "compensation_result", ColumnDescription = "补偿结果JSON", IsNullable = true)]
  public string? CompensationResult { get; set; }
}