using SqlSugar;

namespace Lean.CodeGen.Domain.Entities.Workflow;

/// <summary>
/// 工作流表单字段实体
/// </summary>
[SugarTable("lean_wk_form_field", "工作流表单字段表")]
[SugarIndex("idx_form", nameof(FormId), OrderByType.Asc)]
public class LeanWorkflowFormField : LeanBaseEntity
{
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
  /// 控件类型
  /// </summary>
  [SugarColumn(ColumnName = "control_type", ColumnDescription = "控件类型", Length = 50, IsNullable = false)]
  public string ControlType { get; set; } = string.Empty;

  /// <summary>
  /// 默认值
  /// </summary>
  [SugarColumn(ColumnName = "default_value", ColumnDescription = "默认值", IsNullable = true)]
  public string? DefaultValue { get; set; }

  /// <summary>
  /// 占位提示
  /// </summary>
  [SugarColumn(ColumnName = "placeholder", ColumnDescription = "占位提示", Length = 200, IsNullable = true)]
  public string? Placeholder { get; set; }

  /// <summary>
  /// 是否必填
  /// </summary>
  [SugarColumn(ColumnName = "is_required", ColumnDescription = "是否必填", IsNullable = false, DefaultValue = "0")]
  public int IsRequired { get; set; }

  /// <summary>
  /// 是否只读
  /// </summary>
  [SugarColumn(ColumnName = "is_readonly", ColumnDescription = "是否只读", IsNullable = false, DefaultValue = "0")]
  public int IsReadonly { get; set; }

  /// <summary>
  /// 是否隐藏
  /// </summary>
  [SugarColumn(ColumnName = "is_hidden", ColumnDescription = "是否隐藏", IsNullable = false, DefaultValue = "0")]
  public int IsHidden { get; set; }

  /// <summary>
  /// 验证规则JSON
  /// </summary>
  [SugarColumn(ColumnName = "validation_rules", ColumnDescription = "验证规则JSON", IsNullable = true)]
  public string? ValidationRules { get; set; }

  /// <summary>
  /// 数据源配置JSON
  /// </summary>
  [SugarColumn(ColumnName = "datasource_config", ColumnDescription = "数据源配置JSON", IsNullable = true)]
  public string? DatasourceConfig { get; set; }

  /// <summary>
  /// 联动规则JSON
  /// </summary>
  [SugarColumn(ColumnName = "linkage_rules", ColumnDescription = "联动规则JSON", IsNullable = true)]
  public string? LinkageRules { get; set; }

  /// <summary>
  /// 权限配置JSON
  /// </summary>
  [SugarColumn(ColumnName = "permission_config", ColumnDescription = "权限配置JSON", IsNullable = true)]
  public string? PermissionConfig { get; set; }

  /// <summary>
  /// UI配置JSON
  /// </summary>
  [SugarColumn(ColumnName = "ui_config", ColumnDescription = "UI配置JSON", IsNullable = true)]
  public string? UIConfig { get; set; }

  /// <summary>
  /// 是否启用
  /// </summary>
  [SugarColumn(ColumnName = "status", ColumnDescription = "是否启用", IsNullable = false, DefaultValue = "1")]
  public int Status { get; set; } = 1;

  /// <summary>
  /// 排序号
  /// </summary>
  [SugarColumn(ColumnName = "order_num", ColumnDescription = "排序号", IsNullable = false, DefaultValue = "0")]
  public int OrderNum { get; set; }
}