using SqlSugar;
using Lean.CodeGen.Common.Enums;

namespace Lean.CodeGen.Domain.Entities.Workflow;

/// <summary>
/// 工作流定义实体
/// </summary>
[SugarTable("lean_wk_definition", "工作流定义表")]
[SugarIndex("uk_code", nameof(WorkflowCode), OrderByType.Asc, true)]
public class LeanWorkflowDefinition : LeanBaseEntity
{
  /// <summary>
  /// 工作流名称
  /// </summary>
  [SugarColumn(ColumnName = "workflow_name", ColumnDescription = "工作流名称", Length = 100, IsNullable = false)]
  public string WorkflowName { get; set; } = string.Empty;

  /// <summary>
  /// 工作流编码(唯一标识)
  /// </summary>
  [SugarColumn(ColumnName = "workflow_code", ColumnDescription = "工作流编码", Length = 50, IsNullable = false)]
  public string WorkflowCode { get; set; } = string.Empty;

  /// <summary>
  /// 工作流显示名称
  /// </summary>
  [SugarColumn(ColumnName = "display_name", ColumnDescription = "显示名称", Length = 100, IsNullable = true)]
  public string? DisplayName { get; set; }

  /// <summary>
  /// 工作流描述
  /// </summary>
  [SugarColumn(ColumnName = "workflow_description", ColumnDescription = "工作流描述", Length = 500, IsNullable = true)]
  public string? WorkflowDescription { get; set; }

  /// <summary>
  /// 工作流版本号
  /// </summary>
  [SugarColumn(ColumnName = "version", ColumnDescription = "工作流版本号", IsNullable = false, DefaultValue = "1")]
  public int Version { get; set; } = 1;

  /// <summary>
  /// 是否为最新版本(0=否,1=是)
  /// </summary>
  [SugarColumn(ColumnName = "is_latest", ColumnDescription = "是否为最新版本", IsNullable = false, DefaultValue = "1")]
  public int IsLatest { get; set; } = 1;

  /// <summary>
  /// 是否已发布(0=否,1=是)
  /// </summary>
  [SugarColumn(ColumnName = "is_published", ColumnDescription = "是否已发布", IsNullable = false, DefaultValue = "0")]
  public int IsPublished { get; set; } = 0;

  /// <summary>
  /// 工作流状态
  /// </summary>
  [SugarColumn(ColumnName = "workflow_status", ColumnDescription = "工作流状态", IsNullable = false)]
  public int WorkflowStatus { get; set; }

  /// <summary>
  /// 是否启用(0=禁用,1=启用)
  /// </summary>
  [SugarColumn(ColumnName = "status", ColumnDescription = "是否启用", IsNullable = false)]
  public int Status { get; set; }

  /// <summary>
  /// 是否删除已完成实例(0=否,1=是)
  /// </summary>
  [SugarColumn(ColumnName = "delete_completed_instances", ColumnDescription = "是否删除已完成实例", IsNullable = false, DefaultValue = "0")]
  public int DeleteCompletedInstances { get; set; } = 0;

  /// <summary>
  /// 排序号
  /// </summary>
  [SugarColumn(ColumnName = "order_num", ColumnDescription = "排序号", IsNullable = false, DefaultValue = "0")]
  public int OrderNum { get; set; }

  /// <summary>
  /// 是否内置(0=否,1=是)
  /// </summary>
  [SugarColumn(ColumnName = "is_builtin", ColumnDescription = "是否内置", IsNullable = false, DefaultValue = "0")]
  public int IsBuiltin { get; set; }

  /// <summary>
  /// 表单定义
  /// </summary>
  [Navigate(NavigateType.OneToMany, nameof(LeanWorkflowFormDefinition.DefinitionId))]
  public virtual List<LeanWorkflowFormDefinition> Forms { get; set; } = new();

  /// <summary>
  /// 流程活动
  /// </summary>
  [Navigate(NavigateType.OneToMany, nameof(LeanWorkflowActivity.DefinitionId))]
  public virtual List<LeanWorkflowActivity> Activities { get; set; } = new();

  /// <summary>
  /// 流程变量
  /// </summary>
  [Navigate(NavigateType.OneToMany, nameof(LeanWorkflowVariable.DefinitionId))]
  public virtual List<LeanWorkflowVariable> Variables { get; set; } = new();

  /// <summary>
  /// 流程触发器
  /// </summary>
  [Navigate(NavigateType.OneToMany, nameof(LeanWorkflowTrigger.DefinitionId))]
  public virtual List<LeanWorkflowTrigger> Triggers { get; set; } = new();

  /// <summary>
  /// 连线列表
  /// </summary>
  [Navigate(NavigateType.OneToMany, nameof(LeanWorkflowFlow.DefinitionId))]
  public virtual List<LeanWorkflowFlow> Flows { get; set; } = new();
}