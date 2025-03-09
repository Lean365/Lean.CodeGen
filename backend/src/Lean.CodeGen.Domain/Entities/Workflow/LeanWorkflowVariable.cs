using SqlSugar;

namespace Lean.CodeGen.Domain.Entities.Workflow;

/// <summary>
/// 工作流变量定义实体
/// </summary>
[SugarTable("lean_wk_variable", "工作流变量定义表")]
[SugarIndex("idx_definition", nameof(DefinitionId), OrderByType.Asc)]
public class LeanWorkflowVariable : LeanBaseEntity
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
  /// 变量显示名称
  /// </summary>
  [SugarColumn(ColumnName = "display_name", ColumnDescription = "变量显示名称", Length = 100, IsNullable = true)]
  public string? DisplayName { get; set; }

  /// <summary>
  /// 变量描述
  /// </summary>
  [SugarColumn(ColumnName = "description", ColumnDescription = "变量描述", Length = 500, IsNullable = true)]
  public string? Description { get; set; }

  /// <summary>
  /// 默认值
  /// </summary>
  [SugarColumn(ColumnName = "default_value", ColumnDescription = "默认值", IsNullable = true)]
  public string? DefaultValue { get; set; }

  /// <summary>
  /// 是否必填(0=否,1=是)
  /// </summary>
  [SugarColumn(ColumnName = "is_required", ColumnDescription = "是否必填", IsNullable = false, DefaultValue = "0")]
  public int IsRequired { get; set; } = 0;

  /// <summary>
  /// 是否只读(0=否,1=是)
  /// </summary>
  [SugarColumn(ColumnName = "is_readonly", ColumnDescription = "是否只读", IsNullable = false, DefaultValue = "0")]
  public int IsReadonly { get; set; } = 0;

  /// <summary>
  /// 是否启用(0=禁用,1=启用)
  /// </summary>
  [SugarColumn(ColumnName = "status", ColumnDescription = "是否启用", IsNullable = false, DefaultValue = "1")]
  public int Status { get; set; } = 1;

  /// <summary>
  /// 变量属性JSON
  /// </summary>
  [SugarColumn(ColumnName = "properties", ColumnDescription = "变量属性JSON", IsNullable = true)]
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