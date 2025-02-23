using SqlSugar;

namespace Lean.CodeGen.Domain.Entities.Workflow;

/// <summary>
/// 工作流输出实体
/// </summary>
[SugarTable("lean_workflow_output", "工作流输出表")]
[SugarIndex("idx_activity_instance", nameof(ActivityInstanceId), OrderByType.Asc)]
public class LeanWorkflowOutput : LeanBaseEntity
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
  /// 输出名称
  /// </summary>
  [SugarColumn(ColumnName = "output_name", ColumnDescription = "输出名称", Length = 50, IsNullable = false)]
  public string OutputName { get; set; } = string.Empty;

  /// <summary>
  /// 输出值
  /// </summary>
  [SugarColumn(ColumnName = "output_value", ColumnDescription = "输出值", IsNullable = true)]
  public string? OutputValue { get; set; }

  /// <summary>
  /// 输出类型
  /// </summary>
  [SugarColumn(ColumnName = "output_type", ColumnDescription = "输出类型", Length = 50, IsNullable = true)]
  public string? OutputType { get; set; }

  /// <summary>
  /// 输出描述
  /// </summary>
  [SugarColumn(ColumnName = "output_description", ColumnDescription = "输出描述", Length = 500, IsNullable = true)]
  public string? OutputDescription { get; set; }
}