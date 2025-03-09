using SqlSugar;

namespace Lean.CodeGen.Domain.Entities.Workflow;

/// <summary>
/// 工作流活动类型实体
/// </summary>
[SugarTable("lean_wk_activity_type", "工作流活动类型表")]
[SugarIndex("uk_type_name", nameof(TypeName), OrderByType.Asc, true)]
public class LeanWorkflowActivityType : LeanBaseEntity
{
  /// <summary>
  /// 活动类型名称
  /// </summary>
  [SugarColumn(ColumnName = "type_name", ColumnDescription = "活动类型名称", Length = 50, IsNullable = false)]
  public string TypeName { get; set; } = string.Empty;

  /// <summary>
  /// 显示名称
  /// </summary>
  [SugarColumn(ColumnName = "display_name", ColumnDescription = "显示名称", Length = 100, IsNullable = true)]
  public string? DisplayName { get; set; }

  /// <summary>
  /// 描述
  /// </summary>
  [SugarColumn(ColumnName = "description", ColumnDescription = "描述", Length = 500, IsNullable = true)]
  public string? Description { get; set; }

  /// <summary>
  /// 分类
  /// </summary>
  [SugarColumn(ColumnName = "category", ColumnDescription = "分类", Length = 50, IsNullable = false)]
  public string Category { get; set; } = string.Empty;

  /// <summary>
  /// 图标
  /// </summary>
  [SugarColumn(ColumnName = "icon", ColumnDescription = "图标", Length = 50, IsNullable = true)]
  public string? Icon { get; set; }

  /// <summary>
  /// 颜色
  /// </summary>
  [SugarColumn(ColumnName = "color", ColumnDescription = "颜色", Length = 20, IsNullable = true)]
  public string? Color { get; set; }

  /// <summary>
  /// 输入参数定义JSON
  /// </summary>
  [SugarColumn(ColumnName = "input_parameters", ColumnDescription = "输入参数定义JSON", IsNullable = true)]
  public string? InputParameters { get; set; }

  /// <summary>
  /// 输出参数定义JSON
  /// </summary>
  [SugarColumn(ColumnName = "output_parameters", ColumnDescription = "输出参数定义JSON", IsNullable = true)]
  public string? OutputParameters { get; set; }

  /// <summary>
  /// 属性定义JSON
  /// </summary>
  [SugarColumn(ColumnName = "properties", ColumnDescription = "属性定义JSON", IsNullable = true)]
  public string? Properties { get; set; }

  /// <summary>
  /// 结果定义JSON
  /// </summary>
  [SugarColumn(ColumnName = "outcomes", ColumnDescription = "结果定义JSON", IsNullable = true)]
  public string? Outcomes { get; set; }

  /// <summary>
  /// 是否支持补偿
  /// </summary>
  [SugarColumn(ColumnName = "supports_compensation", ColumnDescription = "是否支持补偿", IsNullable = false, DefaultValue = "0")]
  public bool SupportsCompensation { get; set; }

  /// <summary>
  /// 是否为阻塞活动
  /// </summary>
  [SugarColumn(ColumnName = "is_blocking", ColumnDescription = "是否为阻塞活动", IsNullable = false, DefaultValue = "0")]
  public bool IsBlocking { get; set; }

  /// <summary>
  /// 是否为触发器活动
  /// </summary>
  [SugarColumn(ColumnName = "is_trigger", ColumnDescription = "是否为触发器活动", IsNullable = false, DefaultValue = "0")]
  public bool IsTrigger { get; set; }

  /// <summary>
  /// 是否为容器活动
  /// </summary>
  [SugarColumn(ColumnName = "is_container", ColumnDescription = "是否为容器活动", IsNullable = false, DefaultValue = "0")]
  public bool IsContainer { get; set; }

  /// <summary>
  /// 是否为系统活动
  /// </summary>
  [SugarColumn(ColumnName = "is_system", ColumnDescription = "是否为系统活动", IsNullable = false, DefaultValue = "0")]
  public bool IsSystem { get; set; }

  /// <summary>
  /// 是否启用
  /// </summary>
  [SugarColumn(ColumnName = "status", ColumnDescription = "是否启用", IsNullable = false, DefaultValue = "1")]
  public bool Status { get; set; } = true;

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

  /// <summary>
  /// 活动属性列表
  /// </summary>
  [Navigate(NavigateType.OneToMany, nameof(LeanWorkflowActivityProperty.ActivityTypeId))]
  public virtual List<LeanWorkflowActivityProperty> ActivityProperties { get; set; } = new();
}