using SqlSugar;

namespace Lean.CodeGen.Domain.Entities.Workflow;

/// <summary>
/// 工作流连线实体
/// </summary>
[SugarTable("lean_workflow_flow", "工作流连线表")]
public class LeanWorkflowFlow : LeanBaseEntity
{
  /// <summary>
  /// 流程定义ID
  /// </summary>
  [SugarColumn(ColumnName = "definition_id", ColumnDescription = "流程定义ID", IsNullable = false)]
  public long DefinitionId { get; set; }

  /// <summary>
  /// 连线ID
  /// </summary>
  [SugarColumn(ColumnName = "flow_id", ColumnDescription = "连线ID", Length = 50, IsNullable = false)]
  public string FlowId { get; set; } = string.Empty;

  /// <summary>
  /// 源节点ID
  /// </summary>
  [SugarColumn(ColumnName = "source_node_id", ColumnDescription = "源节点ID", Length = 50, IsNullable = false)]
  public string SourceNodeId { get; set; } = string.Empty;

  /// <summary>
  /// 目标节点ID
  /// </summary>
  [SugarColumn(ColumnName = "target_node_id", ColumnDescription = "目标节点ID", Length = 50, IsNullable = false)]
  public string TargetNodeId { get; set; } = string.Empty;

  /// <summary>
  /// 条件表达式
  /// </summary>
  [SugarColumn(ColumnName = "condition", ColumnDescription = "条件表达式", Length = 500, IsNullable = true)]
  public string? Condition { get; set; }

  /// <summary>
  /// 连线名称
  /// </summary>
  [SugarColumn(ColumnName = "flow_name", ColumnDescription = "连线名称", Length = 50, IsNullable = true)]
  public string? FlowName { get; set; }

  /// <summary>
  /// 连线类型
  /// </summary>
  [SugarColumn(ColumnName = "flow_type", ColumnDescription = "连线类型", Length = 20, IsNullable = true)]
  public string? FlowType { get; set; }

  /// <summary>
  /// 排序号
  /// </summary>
  [SugarColumn(ColumnName = "sort", ColumnDescription = "排序号", IsNullable = false)]
  public int Sort { get; set; }
}