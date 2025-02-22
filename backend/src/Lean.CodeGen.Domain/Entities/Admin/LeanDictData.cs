//===================================================
// 项目名: Lean.CodeGen.Domain
// 文件名: LeanDictData.cs
// 功能描述: 字典数据实体
// 创建时间: 2024-03-26
// 作者: Lean
// 版本: 1.0
//===================================================

using SqlSugar;
using Lean.CodeGen.Common.Enums;

namespace Lean.CodeGen.Domain.Entities.Admin;

/// <summary>
/// 字典数据实体
/// </summary>
[SugarTable("lean_dict_data", "字典数据表")]
[SugarIndex("idx_type_value", nameof(TypeId), OrderByType.Asc, true)]
public class LeanDictData : LeanBaseEntity
{
    /// <summary>
    /// 字典类型Id
    /// </summary>
    /// <remarks>
    /// 关联的字典类型ID
    /// </remarks>
    [SugarColumn(ColumnName = "type_id", ColumnDescription = "字典类型Id", IsNullable = false, ColumnDataType = "bigint")]
    public long TypeId { get; set; }

    /// <summary>
    /// 翻译键
    /// </summary>
    /// <remarks>
    /// 用于多语言翻译的键名
    /// </remarks>
    [SugarColumn(ColumnName = "trans_key", ColumnDescription = "翻译键", Length = 100, IsNullable = true, ColumnDataType = "nvarchar")]
    public string? TransKey { get; set; }

    /// <summary>
    /// 字典标签
    /// </summary>
    /// <remarks>
    /// 字典数据的显示名称，如：男、女、正常、停用等
    /// </remarks>
    [SugarColumn(ColumnName = "dict_label", ColumnDescription = "字典标签", Length = 100, IsNullable = false, ColumnDataType = "nvarchar")]
    public string DictLabel { get; set; } = string.Empty;

    /// <summary>
    /// 字典键值
    /// </summary>
    /// <remarks>
    /// 字典数据的实际值，如：0、1等
    /// </remarks>
    [SugarColumn(ColumnName = "dict_value", ColumnDescription = "字典键值", Length = 100, IsNullable = false, ColumnDataType = "nvarchar")]
    public string DictValue { get; set; } = string.Empty;

    /// <summary>
    /// 扩展标签
    /// </summary>
    /// <remarks>
    /// 字典数据的扩展显示名称
    /// </remarks>
    [SugarColumn(ColumnName = "ext_label", ColumnDescription = "扩展标签", Length = 100, IsNullable = true, ColumnDataType = "nvarchar")]
    public string? ExtLabel { get; set; }

    /// <summary>
    /// 扩展键值
    /// </summary>
    /// <remarks>
    /// 字典数据的扩展值
    /// </remarks>
    [SugarColumn(ColumnName = "ext_value", ColumnDescription = "扩展键值", Length = 100, IsNullable = true, ColumnDataType = "nvarchar")]
    public string? ExtValue { get; set; }

    /// <summary>
    /// 样式属性
    /// </summary>
    /// <remarks>
    /// 用于前端显示的样式，如：primary、success、warning、danger等
    /// </remarks>
    [SugarColumn(ColumnName = "css_class", ColumnDescription = "样式属性", Length = 100, IsNullable = true, ColumnDataType = "nvarchar")]
    public string? CssClass { get; set; }

    /// <summary>
    /// 排序号
    /// </summary>
    /// <remarks>
    /// 显示顺序，数值越小越靠前
    /// </remarks>
    [SugarColumn(ColumnName = "order_num", ColumnDescription = "排序号", IsNullable = false, DefaultValue = "0", ColumnDataType = "int")]
    public int OrderNum { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    /// <remarks>
    /// 字典数据状态：0-正常，1-停用
    /// </remarks>
    [SugarColumn(ColumnName = "status", ColumnDescription = "状态", IsNullable = false, DefaultValue = "0", ColumnDataType = "int")]
    public LeanStatus Status { get; set; }

    /// <summary>
    /// 是否内置
    /// </summary>
    /// <remarks>
    /// 是否为系统内置数据：No-否，Yes-是
    /// </remarks>
    [SugarColumn(ColumnName = "is_builtin", ColumnDescription = "是否内置", IsNullable = false, DefaultValue = "0", ColumnDataType = "int")]
    public LeanBuiltinStatus IsBuiltin { get; set; }

    /// <summary>
    /// 所属字典类型
    /// </summary>
    /// <remarks>
    /// 字典数据与字典类型的多对一关系
    /// </remarks>
    [Navigate(NavigateType.OneToOne, nameof(TypeId))]
    public virtual LeanDictType DictType { get; set; } = null!;
}