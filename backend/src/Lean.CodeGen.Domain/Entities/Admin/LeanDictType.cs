//===================================================
// 项目名: Lean.CodeGen.Domain
// 文件名: LeanDictType.cs
// 功能描述: 字典类型实体
// 创建时间: 2024-03-26
// 作者: Lean
// 版本: 1.0
//===================================================

using SqlSugar;
using Lean.CodeGen.Common.Enums;

namespace Lean.CodeGen.Domain.Entities.Admin;

/// <summary>
/// 字典类型实体
/// </summary>
[SugarTable("lean_dict_type", "字典类型表")]
[SugarIndex("idx_type_code", nameof(DictCode), OrderByType.Asc, true)]
public class LeanDictType : LeanBaseEntity
{
    /// <summary>
    /// 字典名称
    /// </summary>
    /// <remarks>
    /// 字典类型的显示名称，如：用户性别、系统状态等
    /// </remarks>
    [SugarColumn(ColumnName = "dict_name", ColumnDescription = "字典名称", Length = 100, IsNullable = false, ColumnDataType = "nvarchar")]
    public string DictName { get; set; } = default!;

    /// <summary>
    /// 字典编码
    /// </summary>
    /// <remarks>
    /// 字典类型的唯一标识符，如：sys_user_sex、sys_normal_disable等
    /// </remarks>
    [SugarColumn(ColumnName = "dict_code", ColumnDescription = "字典编码", Length = 100, IsNullable = false, ColumnDataType = "nvarchar")]
    public string DictCode { get; set; } = default!;

    /// <summary>
    /// 字典分类
    /// </summary>
    /// <remarks>
    /// 字典类型分类：0-传统类型，1-SQL类型
    /// </remarks>
    [SugarColumn(ColumnName = "dict_category", ColumnDescription = "字典分类", IsNullable = false, DefaultValue = "0", ColumnDataType = "int")]
    public LeanDictTypeCategory DictCategory { get; set; }

    /// <summary>
    /// SQL语句
    /// </summary>
    /// <remarks>
    /// 当分类为SQL类型时的查询语句
    /// </remarks>
    [SugarColumn(ColumnName = "sql_statement", ColumnDescription = "SQL语句", Length = -1, IsNullable = true, ColumnDataType = "nvarchar(max)")]
    public string? SqlStatement { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    /// <remarks>
    /// 字典类型状态：0-正常，1-停用
    /// </remarks>
    [SugarColumn(ColumnName = "status", ColumnDescription = "状态", IsNullable = false, DefaultValue = "0", ColumnDataType = "int")]
    public LeanStatus Status { get; set; }

    /// <summary>
    /// 排序号
    /// </summary>
    /// <remarks>
    /// 显示顺序，数值越小越靠前
    /// </remarks>
    [SugarColumn(ColumnName = "order_num", ColumnDescription = "排序号", IsNullable = false, DefaultValue = "0", ColumnDataType = "int")]
    public int OrderNum { get; set; }

    /// <summary>
    /// 是否内置
    /// </summary>
    /// <remarks>
    /// 是否为系统内置字典类型：No-否，Yes-是
    /// 内置字典类型不允许删除
    /// </remarks>
    [SugarColumn(ColumnName = "is_builtin", ColumnDescription = "是否内置", IsNullable = false, DefaultValue = "0", ColumnDataType = "int")]
    public LeanBuiltinStatus IsBuiltin { get; set; }

    /// <summary>
    /// 字典数据列表
    /// </summary>
    /// <remarks>
    /// 字典类型与字典数据的一对多关系
    /// </remarks>
    [Navigate(NavigateType.OneToMany, nameof(LeanDictData.TypeId))]
    public virtual List<LeanDictData> DictDataList { get; set; } = new();
}