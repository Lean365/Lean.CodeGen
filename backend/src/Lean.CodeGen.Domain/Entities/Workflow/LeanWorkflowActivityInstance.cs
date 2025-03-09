using SqlSugar;
using Lean.CodeGen.Common.Enums;

namespace Lean.CodeGen.Domain.Entities.Workflow;

/// <summary>
/// 工作流活动实例实体
/// </summary>
[SugarTable("lean_wk_activity_instance", "工作流活动实例表")]
[SugarIndex("idx_workflow_instance", nameof(WorkflowInstanceId), OrderByType.Asc)]
public class LeanWorkflowActivityInstance : LeanBaseEntity
{
  /// <summary>
  /// 工作流实例ID
  /// </summary>
  [SugarColumn(ColumnName = "workflow_instance_id", ColumnDescription = "工作流实例ID", IsNullable = false)]
  public long WorkflowInstanceId { get; set; }

  /// <summary>
  /// 活动ID
  /// </summary>
  [SugarColumn(ColumnName = "activity_id", ColumnDescription = "活动ID", Length = 50, IsNullable = false)]
  public string ActivityId { get; set; } = string.Empty;

  /// <summary>
  /// 工作流实例
  /// </summary>
  [Navigate(NavigateType.OneToOne, nameof(WorkflowInstanceId))]
  public virtual LeanWorkflowInstance Instance { get; set; } = null!;

  /// <summary>
  /// 活动类型
  /// </summary>
  [SugarColumn(ColumnName = "activity_type", ColumnDescription = "活动类型", Length = 50, IsNullable = false)]
  public string ActivityType { get; set; } = string.Empty;

  /// <summary>
  /// 活动名称
  /// </summary>
  [SugarColumn(ColumnName = "activity_name", ColumnDescription = "活动名称", Length = 100, IsNullable = false)]
  public string ActivityName { get; set; } = string.Empty;

  /// <summary>
  /// 活动状态
  /// </summary>
  [SugarColumn(ColumnName = "activity_status", ColumnDescription = "活动状态", IsNullable = false)]
  public int ActivityStatus { get; set; }

  /// <summary>
  /// 开始时间
  /// </summary>
  [SugarColumn(ColumnName = "start_time", ColumnDescription = "开始时间", IsNullable = false)]
  public DateTime StartTime { get; set; }

  /// <summary>
  /// 结束时间
  /// </summary>
  [SugarColumn(ColumnName = "end_time", ColumnDescription = "结束时间", IsNullable = true)]
  public DateTime? EndTime { get; set; }

  /// <summary>
  /// 输入参数JSON
  /// </summary>
  [SugarColumn(ColumnName = "input_parameters", ColumnDescription = "输入参数JSON", IsNullable = true)]
  public string? InputParameters { get; set; }

  /// <summary>
  /// 输出参数JSON
  /// </summary>
  [SugarColumn(ColumnName = "output_parameters", ColumnDescription = "输出参数JSON", IsNullable = true)]
  public string? OutputParameters { get; set; }

  /// <summary>
  /// 属性值JSON
  /// </summary>
  [SugarColumn(ColumnName = "property_values", ColumnDescription = "属性值JSON", IsNullable = true)]
  public string? PropertyValues { get; set; }

  /// <summary>
  /// 结果值JSON
  /// </summary>
  [SugarColumn(ColumnName = "outcome_values", ColumnDescription = "结果值JSON", IsNullable = true)]
  public string? OutcomeValues { get; set; }

  /// <summary>
  /// 错误信息JSON
  /// </summary>
  [SugarColumn(ColumnName = "error_info", ColumnDescription = "错误信息JSON", IsNullable = true)]
  public string? ErrorInfo { get; set; }

  /// <summary>
  /// 自定义属性JSON
  /// </summary>
  [SugarColumn(ColumnName = "custom_attributes", ColumnDescription = "自定义属性JSON", IsNullable = true)]
  public string? CustomAttributes { get; set; }

  /// <summary>
  /// 补偿列表
  /// </summary>
  [Navigate(NavigateType.OneToMany, nameof(LeanWorkflowCompensation.ActivityInstanceId))]
  public virtual List<LeanWorkflowCompensation> Compensations { get; set; } = new();

  /// <summary>
  /// 输出列表
  /// </summary>
  [Navigate(NavigateType.OneToMany, nameof(LeanWorkflowOutput.ActivityInstanceId))]
  public virtual List<LeanWorkflowOutput> Outputs { get; set; } = new();

  /// <summary>
  /// 结果列表
  /// </summary>
  [Navigate(NavigateType.OneToMany, nameof(LeanWorkflowOutcome.ActivityInstanceId))]
  public virtual List<LeanWorkflowOutcome> Outcomes { get; set; } = new();
}