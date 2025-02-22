using SqlSugar;
using Lean.CodeGen.Common.Enums;

namespace Lean.CodeGen.Domain.Entities.Workflow;

/// <summary>
/// 工作流历史实体
/// </summary>
[SugarTable("lean_workflow_history", "工作流历史表")]
[SugarIndex("idx_instance", nameof(InstanceId), OrderByType.Asc)]
[SugarIndex("idx_task", nameof(TaskId), OrderByType.Asc)]
public class LeanWorkflowHistory : LeanBaseEntity
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
  /// 操作类型
  /// </summary>
  [SugarColumn(ColumnName = "operation_type", ColumnDescription = "操作类型", IsNullable = false)]
  public LeanWorkflowOperationType OperationType { get; set; }

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
  /// 操作人部门ID
  /// </summary>
  [SugarColumn(ColumnName = "operator_dept_id", ColumnDescription = "操作人部门ID", IsNullable = false)]
  public long OperatorDeptId { get; set; }

  /// <summary>
  /// 操作人部门名称
  /// </summary>
  [SugarColumn(ColumnName = "operator_dept_name", ColumnDescription = "操作人部门名称", Length = 50, IsNullable = false)]
  public string OperatorDeptName { get; set; } = string.Empty;

  /// <summary>
  /// 操作时间
  /// </summary>
  [SugarColumn(ColumnName = "operation_time", ColumnDescription = "操作时间", IsNullable = false)]
  public DateTime OperationTime { get; set; }

  /// <summary>
  /// 操作说明
  /// </summary>
  [SugarColumn(ColumnName = "operation_desc", ColumnDescription = "操作说明", Length = 500, IsNullable = true)]
  public string? OperationDesc { get; set; }

  /// <summary>
  /// IP地址
  /// </summary>
  [SugarColumn(ColumnName = "ip_address", ColumnDescription = "IP地址", Length = 50, IsNullable = true)]
  public string? IpAddress { get; set; }

  /// <summary>
  /// 设备信息
  /// </summary>
  [SugarColumn(ColumnName = "device_info", ColumnDescription = "设备信息", Length = 200, IsNullable = true)]
  public string? DeviceInfo { get; set; }

  /// <summary>
  /// 表单数据
  /// </summary>
  [Navigate(NavigateType.OneToMany, nameof(LeanWorkflowFormData.Id))]
  public virtual List<LeanWorkflowFormData> FormDataList { get; set; } = new();

  /// <summary>
  /// 变量数据
  /// </summary>
  [Navigate(NavigateType.OneToMany, nameof(LeanWorkflowVariableData.Id))]
  public virtual List<LeanWorkflowVariableData> VariableDataList { get; set; } = new();
}