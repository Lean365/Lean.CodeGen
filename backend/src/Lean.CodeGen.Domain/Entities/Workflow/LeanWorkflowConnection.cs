using SqlSugar;

namespace Lean.CodeGen.Domain.Entities.Workflow;

/// <summary>
/// 工作流连接定义实体
/// </summary>
[SugarTable("lean_workflow_connection", "工作流连接定义表")]
[SugarIndex("idx_definition", nameof(DefinitionId), OrderByType.Asc)]
public class LeanWorkflowConnection : LeanBaseEntity
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
  /// 连接ID
  /// </summary>
  [SugarColumn(ColumnName = "connection_id", ColumnDescription = "连接ID", Length = 50, IsNullable = false)]
  public string ConnectionId { get; set; } = string.Empty;

  /// <summary>
  /// 源活动ID
  /// </summary>
  [SugarColumn(ColumnName = "source_activity_id", ColumnDescription = "源活动ID", Length = 50, IsNullable = false)]
  public string SourceActivityId { get; set; } = string.Empty;

  /// <summary>
  /// 目标活动ID
  /// </summary>
  [SugarColumn(ColumnName = "target_activity_id", ColumnDescription = "目标活动ID", Length = 50, IsNullable = false)]
  public string TargetActivityId { get; set; } = string.Empty;

  /// <summary>
  /// 连接类型
  /// </summary>
  [SugarColumn(ColumnName = "connection_type", ColumnDescription = "连接类型", Length = 50, IsNullable = false)]
  public string ConnectionType { get; set; } = string.Empty;

  /// <summary>
  /// 连接名称
  /// </summary>
  [SugarColumn(ColumnName = "connection_name", ColumnDescription = "连接名称", Length = 100, IsNullable = true)]
  public string? ConnectionName { get; set; }

  /// <summary>
  /// 连接描述
  /// </summary>
  [SugarColumn(ColumnName = "description", ColumnDescription = "连接描述", Length = 500, IsNullable = true)]
  public string? Description { get; set; }

  /// <summary>
  /// 连接条件JSON
  /// </summary>
  [SugarColumn(ColumnName = "condition", ColumnDescription = "连接条件JSON", IsNullable = true)]
  public string? Condition { get; set; }

  /// <summary>
  /// 连接属性JSON
  /// </summary>
  [SugarColumn(ColumnName = "properties", ColumnDescription = "连接属性JSON", IsNullable = true)]
  public string? Properties { get; set; }

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