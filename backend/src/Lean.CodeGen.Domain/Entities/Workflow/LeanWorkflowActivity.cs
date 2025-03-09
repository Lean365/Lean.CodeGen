using SqlSugar;

namespace Lean.CodeGen.Domain.Entities.Workflow;

/// <summary>
/// 工作流活动定义实体
/// </summary>
[SugarTable("lean_wk_activity", "工作流活动定义表")]
[SugarIndex("idx_definition", nameof(DefinitionId), OrderByType.Asc)]
public class LeanWorkflowActivity : LeanBaseEntity
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
    /// 活动ID
    /// </summary>
    [SugarColumn(ColumnName = "activity_id", ColumnDescription = "活动ID", Length = 50, IsNullable = false)]
    public string ActivityId { get; set; } = string.Empty;

    /// <summary>
    /// 活动名称
    /// </summary>
    [SugarColumn(ColumnName = "activity_name", ColumnDescription = "活动名称", Length = 100, IsNullable = false)]
    public string ActivityName { get; set; } = string.Empty;

    /// <summary>
    /// 活动类型
    /// </summary>
    [SugarColumn(ColumnName = "activity_type", ColumnDescription = "活动类型", Length = 50, IsNullable = false)]
    public string ActivityType { get; set; } = string.Empty;

    /// <summary>
    /// 活动显示名称
    /// </summary>
    [SugarColumn(ColumnName = "display_name", ColumnDescription = "活动显示名称", Length = 100, IsNullable = true)]
    public string? DisplayName { get; set; }

    /// <summary>
    /// 活动描述
    /// </summary>
    [SugarColumn(ColumnName = "description", ColumnDescription = "活动描述", Length = 500, IsNullable = true)]
    public string? Description { get; set; }

    /// <summary>
    /// 是否启用(0=禁用,1=启用)
    /// </summary>
    [SugarColumn(ColumnName = "status", ColumnDescription = "是否启用", IsNullable = false, DefaultValue = "1")]
    public int Status { get; set; } = 1;

    /// <summary>
    /// 活动属性JSON
    /// </summary>
    [SugarColumn(ColumnName = "properties", ColumnDescription = "活动属性JSON", IsNullable = true)]
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