using SqlSugar;

namespace Lean.CodeGen.Domain.Entities.Workflow;

/// <summary>
/// 工作流活动实例实体
/// </summary>
[SugarTable("lean_workflow_activity_instance", "工作流活动实例表")]
[SugarIndex("idx_instance", nameof(InstanceId), OrderByType.Asc)]
public class LeanWorkflowActivityInstance : LeanBaseEntity
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
  /// 活动ID
  /// </summary>
  [SugarColumn(ColumnName = "activity_id", ColumnDescription = "活动ID", Length = 50, IsNullable = false)]
  public string ActivityId { get; set; } = string.Empty;

  /// <summary>
  /// 活动名称
  /// </summary>
  [SugarColumn(ColumnName = "activity_name", ColumnDescription = "活动名称", Length = 100, IsNullable = false)]
  public string ActivityName { get; set; } = string.Empty;

  /// <summary>
  /// 活动类型
  /// </summary>
  [SugarColumn(ColumnName = "activity_type", ColumnDescription = "活动类型", Length = 50, IsNullable = false)]
  public string ActivityType { get; set; } = string.Empty;

  /// <summary>
  /// 活动状态
  /// </summary>
  [SugarColumn(ColumnName = "activity_status", ColumnDescription = "活动状态", IsNullable = false)]
  public int ActivityStatus { get; set; }

  /// <summary>
  /// 开始时间
  /// </summary>
  [SugarColumn(ColumnName = "start_time", ColumnDescription = "开始时间", IsNullable = true)]
  public DateTime? StartTime { get; set; }

  /// <summary>
  /// 结束时间
  /// </summary>
  [SugarColumn(ColumnName = "end_time", ColumnDescription = "结束时间", IsNullable = true)]
  public DateTime? EndTime { get; set; }

  /// <summary>
  /// 输入数据JSON
  /// </summary>
  [SugarColumn(ColumnName = "input_data", ColumnDescription = "输入数据JSON", IsNullable = true)]
  public string? InputData { get; set; }

  /// <summary>
  /// 输出数据JSON
  /// </summary>
  [SugarColumn(ColumnName = "output_data", ColumnDescription = "输出数据JSON", IsNullable = true)]
  public string? OutputData { get; set; }

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
}