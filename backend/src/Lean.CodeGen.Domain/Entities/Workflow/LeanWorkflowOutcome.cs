using SqlSugar;

namespace Lean.CodeGen.Domain.Entities.Workflow;

/// <summary>
/// 工作流结果实体
/// </summary>
[SugarTable("lean_wk_outcome", "工作流结果表")]
[SugarIndex("idx_activity_instance", nameof(ActivityInstanceId), OrderByType.Asc)]
public class LeanWorkflowOutcome : LeanBaseEntity
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
  /// 结果名称
  /// </summary>
  [SugarColumn(ColumnName = "outcome_name", ColumnDescription = "结果名称", Length = 50, IsNullable = false)]
  public string OutcomeName { get; set; } = string.Empty;

  /// <summary>
  /// 结果值
  /// </summary>
  [SugarColumn(ColumnName = "outcome_value", ColumnDescription = "结果值", IsNullable = true)]
  public string? OutcomeValue { get; set; }

  /// <summary>
  /// 结果类型
  /// </summary>
  [SugarColumn(ColumnName = "outcome_type", ColumnDescription = "结果类型", Length = 50, IsNullable = true)]
  public string? OutcomeType { get; set; }

  /// <summary>
  /// 结果描述
  /// </summary>
  [SugarColumn(ColumnName = "outcome_description", ColumnDescription = "结果描述", Length = 500, IsNullable = true)]
  public string? OutcomeDescription { get; set; }
}