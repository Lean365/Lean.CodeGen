using SqlSugar;

namespace Lean.CodeGen.Domain.Entities.Workflow;

/// <summary>
/// 工作流表单定义实体
/// </summary>
[SugarTable("lean_workflow_form_definition", "工作流表单定义表")]
[SugarIndex("idx_definition", nameof(DefinitionId), OrderByType.Asc)]
public class LeanWorkflowFormDefinition : LeanBaseEntity
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
  /// 表单编码
  /// </summary>
  [SugarColumn(ColumnName = "form_code", ColumnDescription = "表单编码", Length = 50, IsNullable = false)]
  public string FormCode { get; set; } = string.Empty;

  /// <summary>
  /// 表单名称
  /// </summary>
  [SugarColumn(ColumnName = "form_name", ColumnDescription = "表单名称", Length = 100, IsNullable = false)]
  public string FormName { get; set; } = string.Empty;

  /// <summary>
  /// 表单类型(预设/自定义)
  /// </summary>
  [SugarColumn(ColumnName = "form_type", ColumnDescription = "表单类型", Length = 50, IsNullable = false)]
  public string FormType { get; set; } = string.Empty;

  /// <summary>
  /// 表单版本号
  /// </summary>
  [SugarColumn(ColumnName = "version", ColumnDescription = "表单版本号", IsNullable = false, DefaultValue = "1")]
  public int Version { get; set; } = 1;

  /// <summary>
  /// 是否为最新版本
  /// </summary>
  [SugarColumn(ColumnName = "is_latest", ColumnDescription = "是否为最新版本", IsNullable = false, DefaultValue = "1")]
  public int IsLatest { get; set; } = 1;

  /// <summary>
  /// 表单布局JSON
  /// </summary>
  [SugarColumn(ColumnName = "layout", ColumnDescription = "表单布局JSON", IsNullable = true)]
  public string? Layout { get; set; }

  /// <summary>
  /// 表单配置JSON
  /// </summary>
  [SugarColumn(ColumnName = "config", ColumnDescription = "表单配置JSON", IsNullable = true)]
  public string? Config { get; set; }

  /// <summary>
  /// 表单样式JSON
  /// </summary>
  [SugarColumn(ColumnName = "styles", ColumnDescription = "表单样式JSON", IsNullable = true)]
  public string? Styles { get; set; }

  /// <summary>
  /// 是否启用
  /// </summary>
  [SugarColumn(ColumnName = "status", ColumnDescription = "是否启用", IsNullable = false, DefaultValue = "1")]
  public int Status { get; set; } = 1;

  /// <summary>
  /// 备注
  /// </summary>
  [SugarColumn(ColumnName = "remark", ColumnDescription = "备注", Length = 500, IsNullable = true)]
  public string? Remark { get; set; }

  /// <summary>
  /// 排序号
  /// </summary>
  [SugarColumn(ColumnName = "order_num", ColumnDescription = "排序号", IsNullable = false, DefaultValue = "0")]
  public int OrderNum { get; set; }
}