using SqlSugar;

namespace Lean.CodeGen.Domain.Entities.Workflow;

/// <summary>
/// 工作流活动属性实体
/// </summary>
[SugarTable("lean_workflow_activity_property", "工作流活动属性表")]
[SugarIndex("uk_activity_type_property_name", nameof(ActivityTypeId), OrderByType.Asc, nameof(PropertyName), OrderByType.Asc)]
public class LeanWorkflowActivityProperty : LeanBaseEntity
{
    /// <summary>
    /// 活动类型ID
    /// </summary>
    [SugarColumn(ColumnName = "activity_type_id", ColumnDescription = "活动类型ID", IsNullable = false)]
    public long ActivityTypeId { get; set; }

    /// <summary>
    /// 属性名称
    /// </summary>
    [SugarColumn(ColumnName = "property_name", ColumnDescription = "属性名称", Length = 50, IsNullable = false)]
    public string PropertyName { get; set; } = string.Empty;

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
    /// 属性类型
    /// </summary>
    [SugarColumn(ColumnName = "property_type", ColumnDescription = "属性类型", Length = 50, IsNullable = false)]
    public string PropertyType { get; set; } = string.Empty;

    /// <summary>
    /// 默认值
    /// </summary>
    [SugarColumn(ColumnName = "default_value", ColumnDescription = "默认值", IsNullable = true)]
    public string? DefaultValue { get; set; }

    /// <summary>
    /// 是否必填
    /// </summary>
    [SugarColumn(ColumnName = "is_required", ColumnDescription = "是否必填", IsNullable = false, DefaultValue = "0")]
    public bool IsRequired { get; set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    [SugarColumn(ColumnName = "status", ColumnDescription = "是否启用", IsNullable = false, DefaultValue = "1")]
    public bool Status { get; set; } = true;

    /// <summary>
    /// 排序号
    /// </summary>
    [SugarColumn(ColumnName = "order_num", ColumnDescription = "排序号", IsNullable = false, DefaultValue = "0")]
    public int OrderNum { get; set; }

    /// <summary>
    /// 所属活动类型
    /// </summary>
    [Navigate(NavigateType.OneToOne, nameof(ActivityTypeId))]
    public virtual LeanWorkflowActivityType? ActivityType { get; set; }
}