using SqlSugar;

namespace Lean.CodeGen.Domain.Entities.Workflow;

/// <summary>
/// 工作流触发器实体
/// </summary>
[SugarTable("lean_wk_trigger", "工作流触发器表")]
[SugarIndex("idx_definition", nameof(DefinitionId), OrderByType.Asc)]
public class LeanWorkflowTrigger : LeanBaseEntity
{
  /// <summary>
  /// 工作流定义ID
  /// </summary>
  [SugarColumn(ColumnName = "definition_id", ColumnDescription = "工作流定义ID", IsNullable = false)]
  public long DefinitionId { get; set; }

  /// <summary>
  /// 工作流定义
  /// </summary>
  [Navigate(NavigateType.OneToOne, nameof(DefinitionId))]
  public virtual LeanWorkflowDefinition Definition { get; set; } = null!;

  /// <summary>
  /// 触发器名称
  /// </summary>
  [SugarColumn(ColumnName = "trigger_name", ColumnDescription = "触发器名称", Length = 100, IsNullable = false)]
  public string TriggerName { get; set; } = string.Empty;

  /// <summary>
  /// 触发器类型(Timer=定时触发,Signal=信号触发,Event=事件触发)
  /// </summary>
  [SugarColumn(ColumnName = "trigger_type", ColumnDescription = "触发器类型", Length = 50, IsNullable = false)]
  public string TriggerType { get; set; } = string.Empty;

  /// <summary>
  /// 触发器配置JSON
  /// </summary>
  [SugarColumn(ColumnName = "trigger_config", ColumnDescription = "触发器配置JSON", IsNullable = true)]
  public string? TriggerConfig { get; set; }

  /// <summary>
  /// 触发器状态(0=禁用,1=启用)
  /// </summary>
  [SugarColumn(ColumnName = "status", ColumnDescription = "触发器状态", IsNullable = false, DefaultValue = "1")]
  public int Status { get; set; } = 1;

  /// <summary>
  /// 上次触发时间
  /// </summary>
  [SugarColumn(ColumnName = "last_trigger_time", ColumnDescription = "上次触发时间", IsNullable = true)]
  public DateTime? LastTriggerTime { get; set; }

  /// <summary>
  /// 下次触发时间
  /// </summary>
  [SugarColumn(ColumnName = "next_trigger_time", ColumnDescription = "下次触发时间", IsNullable = true)]
  public DateTime? NextTriggerTime { get; set; }

  /// <summary>
  /// 触发次数
  /// </summary>
  [SugarColumn(ColumnName = "trigger_count", ColumnDescription = "触发次数", IsNullable = false, DefaultValue = "0")]
  public int TriggerCount { get; set; } = 0;

  /// <summary>
  /// 触发条件JSON
  /// </summary>
  [SugarColumn(ColumnName = "trigger_condition", ColumnDescription = "触发条件JSON", IsNullable = true)]
  public string? TriggerCondition { get; set; }

  /// <summary>
  /// 输入参数JSON
  /// </summary>
  [SugarColumn(ColumnName = "input_parameters", ColumnDescription = "输入参数JSON", IsNullable = true)]
  public string? InputParameters { get; set; }

  /// <summary>
  /// 自定义属性JSON
  /// </summary>
  [SugarColumn(ColumnName = "custom_attributes", ColumnDescription = "自定义属性JSON", IsNullable = true)]
  public string? CustomAttributes { get; set; }

  /// <summary>
  /// 排序号
  /// </summary>
  [SugarColumn(ColumnName = "order_num", ColumnDescription = "排序号", IsNullable = false, DefaultValue = "0")]
  public int OrderNum { get; set; }
}