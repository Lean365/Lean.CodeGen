using SqlSugar;

namespace Lean.CodeGen.Domain.Entities.Workflow;

/// <summary>
/// 工作流表单数据实体
/// </summary>
[SugarTable("lean_wk_form_data", "工作流表单数据表")]
[SugarIndex("idx_instance", nameof(InstanceId), OrderByType.Asc)]
[SugarIndex("idx_task", nameof(TaskId), OrderByType.Asc)]
public class LeanWorkflowFormData : LeanBaseEntity
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
  /// 表单定义ID
  /// </summary>
  [SugarColumn(ColumnName = "form_id", ColumnDescription = "表单定义ID", IsNullable = false)]
  public long FormId { get; set; }

  /// <summary>
  /// 表单定义
  /// </summary>
  [Navigate(NavigateType.OneToOne, nameof(FormId))]
  public virtual LeanWorkflowFormDefinition Form { get; set; } = null!;

  /// <summary>
  /// 字段编码
  /// </summary>
  [SugarColumn(ColumnName = "field_code", ColumnDescription = "字段编码", Length = 50, IsNullable = false)]
  public string FieldCode { get; set; } = string.Empty;

  /// <summary>
  /// 字段名称
  /// </summary>
  [SugarColumn(ColumnName = "field_name", ColumnDescription = "字段名称", Length = 100, IsNullable = false)]
  public string FieldName { get; set; } = string.Empty;

  /// <summary>
  /// 字段类型
  /// </summary>
  [SugarColumn(ColumnName = "field_type", ColumnDescription = "字段类型", Length = 50, IsNullable = false)]
  public string FieldType { get; set; } = string.Empty;

  /// <summary>
  /// 字段值
  /// </summary>
  [SugarColumn(ColumnName = "field_value", ColumnDescription = "字段值", IsNullable = true)]
  public string? FieldValue { get; set; }

  /// <summary>
  /// 显示值
  /// </summary>
  [SugarColumn(ColumnName = "display_value", ColumnDescription = "显示值", IsNullable = true)]
  public string? DisplayValue { get; set; }

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