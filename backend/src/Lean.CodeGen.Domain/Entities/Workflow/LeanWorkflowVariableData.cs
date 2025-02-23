using SqlSugar;

namespace Lean.CodeGen.Domain.Entities.Workflow;

/// <summary>
/// 工作流变量数据实体
/// </summary>
[SugarTable("lean_workflow_variable_data", "工作流变量数据表")]
[SugarIndex("idx_instance", nameof(InstanceId), OrderByType.Asc)]
[SugarIndex("idx_task", nameof(TaskId), OrderByType.Asc)]
public class LeanWorkflowVariableData : LeanBaseEntity
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
  /// 任务ID
  /// </summary>
  [SugarColumn(ColumnName = "task_id", ColumnDescription = "任务ID", IsNullable = true)]
  public long? TaskId { get; set; }

  /// <summary>
  /// 工作流任务
  /// </summary>
  [Navigate(NavigateType.OneToOne, nameof(TaskId))]
  public virtual LeanWorkflowTask? Task { get; set; }

  /// <summary>
  /// 变量定义ID
  /// </summary>
  [SugarColumn(ColumnName = "variable_id", ColumnDescription = "变量定义ID", IsNullable = false)]
  public long VariableId { get; set; }

  /// <summary>
  /// 变量定义
  /// </summary>
  [Navigate(NavigateType.OneToOne, nameof(VariableId))]
  public virtual LeanWorkflowVariable Variable { get; set; } = null!;

  /// <summary>
  /// 变量名称
  /// </summary>
  [SugarColumn(ColumnName = "variable_name", ColumnDescription = "变量名称", Length = 100, IsNullable = false)]
  public string VariableName { get; set; } = string.Empty;

  /// <summary>
  /// 变量类型
  /// </summary>
  [SugarColumn(ColumnName = "variable_type", ColumnDescription = "变量类型", Length = 50, IsNullable = false)]
  public string VariableType { get; set; } = string.Empty;

  /// <summary>
  /// 变量值
  /// </summary>
  [SugarColumn(ColumnName = "variable_value", ColumnDescription = "变量值", IsNullable = true)]
  public string? VariableValue { get; set; }

  /// <summary>
  /// 操作人ID
  /// </summary>
  [SugarColumn(ColumnName = "operator_id", ColumnDescription = "操作人ID", IsNullable = false)]
  public long OperatorId { get; set; }

  /// <summary>
  /// 操作人名称
  /// </summary>
  [SugarColumn(ColumnName = "operator_name", ColumnDescription = "操作人名称", Length = 50, IsNullable = false)]
  public string OperatorName { get; set; } = string.Empty;

  /// <summary>
  /// 操作时间
  /// </summary>
  [SugarColumn(ColumnName = "operation_time", ColumnDescription = "操作时间", IsNullable = false)]
  public DateTime OperationTime { get; set; }

  /// <summary>
  /// 版本号
  /// </summary>
  [SugarColumn(ColumnName = "version", ColumnDescription = "版本号", IsNullable = false, DefaultValue = "1")]
  public int Version { get; set; } = 1;


}