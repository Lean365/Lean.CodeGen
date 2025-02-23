using SqlSugar;
using Lean.CodeGen.Common.Enums;

namespace Lean.CodeGen.Domain.Entities.Admin;

/// <summary>
/// 语言实体
/// </summary>
[SugarTable("lean_admin_language", "语言表")]
[SugarIndex("uk_lang_code", nameof(LangCode), OrderByType.Asc, true)]
public class LeanLanguage : LeanBaseEntity
{
    /// <summary>
    /// 语言名称
    /// </summary>
    /// <remarks>
    /// 语言的显示名称，如：简体中文、English等
    /// </remarks>
    [SugarColumn(ColumnName = "lang_name", ColumnDescription = "语言名称", Length = 50, IsNullable = false, ColumnDataType = "nvarchar")]
    public string LangName { get; set; } = string.Empty;

    /// <summary>
    /// 语言代码
    /// </summary>
    /// <remarks>
    /// 语言的唯一标识符，如：zh-CN、en-US等
    /// </remarks>
    [SugarColumn(ColumnName = "lang_code", ColumnDescription = "语言代码", Length = 20, IsNullable = false, UniqueGroupNameList = new[] { "uk_lang_code" }, ColumnDataType = "nvarchar")]
    public string LangCode { get; set; } = string.Empty;

    /// <summary>
    /// 语言图标
    /// </summary>
    /// <remarks>
    /// 语言的图标，如：国旗图标等
    /// </remarks>
    [SugarColumn(ColumnName = "icon", ColumnDescription = "图标", Length = 100, IsNullable = true, ColumnDataType = "nvarchar")]
    public string? LangIcon { get; set; }

    /// <summary>
    /// 是否默认语言
    /// </summary>
    /// <remarks>
    /// 是否为默认语言：No-否，Yes-是
    /// </remarks>
    [SugarColumn(ColumnName = "is_default", ColumnDescription = "是否默认", IsNullable = false, DefaultValue = "0", ColumnDataType = "int")]
    public LeanYesNo IsDefault { get; set; }

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
    /// 语言状态：0-正常，1-停用
    /// </remarks>
    [SugarColumn(ColumnName = "status", ColumnDescription = "状态", IsNullable = false, DefaultValue = "0", ColumnDataType = "int")]
    public LeanStatus Status { get; set; }

    /// <summary>
    /// 是否内置
    /// </summary>
    /// <remarks>
    /// 是否为系统内置语言：No-否，Yes-是
    /// </remarks>
    [SugarColumn(ColumnName = "is_builtin", ColumnDescription = "是否内置", IsNullable = false, DefaultValue = "0", ColumnDataType = "int")]
    public LeanBuiltinStatus IsBuiltin { get; set; }

    /// <summary>
    /// 翻译列表
    /// </summary>
    /// <remarks>
    /// 语言与翻译的一对多关系
    /// </remarks>
    [Navigate(NavigateType.OneToMany, nameof(LeanTranslation.LangId))]
    public virtual List<LeanTranslation> Translations { get; set; } = new();
}