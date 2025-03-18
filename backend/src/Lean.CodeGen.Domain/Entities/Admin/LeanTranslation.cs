using SqlSugar;
using Lean.CodeGen.Common.Enums;

namespace Lean.CodeGen.Domain.Entities.Admin;

/// <summary>
/// 翻译实体
/// </summary>
[SugarTable("lean_adm_translation", "翻译表")]
[SugarIndex("uk_lang_key", nameof(LangId), OrderByType.Asc, nameof(TransKey), OrderByType.Asc, true)]
public class LeanTranslation : LeanBaseEntity
{
  /// <summary>
  /// 语言ID
  /// </summary>
  /// <remarks>
  /// 关联的语言ID
  /// </remarks>
  [SugarColumn(ColumnName = "lang_id", ColumnDescription = "语言ID", IsNullable = false)]
  public long LangId { get; set; }

  /// <summary>
  /// 翻译键名
  /// </summary>
  /// <remarks>
  /// 翻译项的唯一标识符，如：common.ok、common.cancel等
  /// </remarks>
  [SugarColumn(ColumnName = "trans_key", ColumnDescription = "翻译键名", Length = 100, IsNullable = false, ColumnDataType = "nvarchar")]
  public string TransKey { get; set; } = string.Empty;

  /// <summary>
  /// 翻译值
  /// </summary>
  /// <remarks>
  /// 翻译项的值，如：确定、取消等
  /// </remarks>
  [SugarColumn(ColumnName = "trans_value", ColumnDescription = "翻译值", Length = -1, IsNullable = false, ColumnDataType = "nvarchar(max)")]
  public string TransValue { get; set; } = string.Empty;

  /// <summary>
  /// 模块名称
  /// </summary>
  /// <remarks>
  /// 所属模块，如：common、system等
  /// </remarks>
  [SugarColumn(ColumnName = "module_name", ColumnDescription = "模块名称", Length = 50, IsNullable = false, ColumnDataType = "nvarchar")]
  public string? ModuleName { get; set; }

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
  /// 翻译状态：0-正常，1-停用
  /// </remarks>
  [SugarColumn(ColumnName = "trans_status", ColumnDescription = "状态", IsNullable = false, DefaultValue = "0", ColumnDataType = "int")]
  public int TransStatus { get; set; }

  /// <summary>
  /// 是否内置
  /// </summary>
  /// <remarks>
  /// 是否为系统内置翻译：0-否，1-是
  /// </remarks>
  [SugarColumn(ColumnName = "is_builtin", ColumnDescription = "是否内置", IsNullable = false, DefaultValue = "0", ColumnDataType = "int")]
  public int IsBuiltin { get; set; }

  /// <summary>
  /// 所属语言
  /// </summary>
  /// <remarks>
  /// 翻译与语言的多对一关系
  /// </remarks>
  [Navigate(NavigateType.OneToOne, nameof(LangId))]
  public virtual LeanLanguage Language { get; set; } = null!;
}