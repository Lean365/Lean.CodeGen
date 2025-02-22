//===================================================
// 项目名: Lean.CodeGen.Domain
// 文件名: LeanConfig.cs
// 功能描述: 系统配置实体
// 创建时间: 2024-03-26
// 作者: Lean
// 版本: 1.0
//===================================================

using SqlSugar;
using Lean.CodeGen.Common.Enums;

namespace Lean.CodeGen.Domain.Entities.Admin;

/// <summary>
/// 系统配置实体
/// </summary>
[SugarTable("lean_config", "系统配置表")]
[SugarIndex("uk_key", nameof(ConfigKey), OrderByType.Asc, true)]
public class LeanConfig : LeanBaseEntity
{
    /// <summary>
    /// 配置名称
    /// </summary>
    /// <remarks>
    /// 配置项的显示名称
    /// </remarks>
    [SugarColumn(ColumnName = "config_name", ColumnDescription = "配置名称", Length = 100, IsNullable = false, ColumnDataType = "nvarchar")]
    public string ConfigName { get; set; } = default!;

    /// <summary>
    /// 配置键
    /// </summary>
    /// <remarks>
    /// 配置项的唯一标识键
    /// </remarks>
    [SugarColumn(ColumnName = "config_key", ColumnDescription = "配置键", Length = 100, IsNullable = false, UniqueGroupNameList = new[] { "uk_key" }, ColumnDataType = "nvarchar")]
    public string ConfigKey { get; set; } = default!;

    /// <summary>
    /// 配置值
    /// </summary>
    /// <remarks>
    /// 配置项的值
    /// </remarks>
    [SugarColumn(ColumnName = "config_value", ColumnDescription = "配置值", Length = 4000, IsNullable = false, ColumnDataType = "nvarchar")]
    public string ConfigValue { get; set; } = default!;

    /// <summary>
    /// 配置类型
    /// </summary>
    /// <remarks>
    /// 配置值的数据类型：String-字符串，Number-数值，Boolean-布尔值，Json-JSON对象，Other-其他
    /// </remarks>
    [SugarColumn(ColumnName = "config_type", ColumnDescription = "配置类型", IsNullable = false, DefaultValue = "0", ColumnDataType = "int")]
    public LeanConfigType ConfigType { get; set; }

    /// <summary>
    /// 系统内置
    /// </summary>
    /// <remarks>
    /// 是否为系统内置配置项：No-否，Yes-是
    /// 内置配置项不允许删除
    /// </remarks>
    [SugarColumn(ColumnName = "is_builtin", ColumnDescription = "系统内置", IsNullable = false, DefaultValue = "0", ColumnDataType = "int")]
    public LeanBuiltinStatus IsBuiltin { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    /// <remarks>
    /// 配置项状态：0-正常，1-停用
    /// </remarks>
    [SugarColumn(ColumnName = "status", ColumnDescription = "状态", IsNullable = false, DefaultValue = "0", ColumnDataType = "int")]
    public LeanStatus Status { get; set; }

    /// <summary>
    /// 配置分组
    /// </summary>
    /// <remarks>
    /// 配置项所属的分组
    /// </remarks>
    [SugarColumn(ColumnName = "config_group", ColumnDescription = "配置分组", Length = 50, IsNullable = true, ColumnDataType = "nvarchar")]
    public string? ConfigGroup { get; set; }

    /// <summary>
    /// 排序号
    /// </summary>
    /// <remarks>
    /// 显示顺序，数值越小越靠前
    /// </remarks>
    [SugarColumn(ColumnName = "order_num", ColumnDescription = "排序号", IsNullable = false, DefaultValue = "0", ColumnDataType = "int")]
    public int OrderNum { get; set; }
}