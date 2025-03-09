// -----------------------------------------------------------------------
// <copyright file="LeanMailTmpl.cs" company="Lean">
// Copyright (c) Lean. All rights reserved.
// </copyright>
// <author>Lean</author>
// <created>2024-03-09</created>
// <summary>邮件模板实体</summary>
// -----------------------------------------------------------------------
using SqlSugar;

namespace Lean.CodeGen.Domain.Entities.Routine;

/// <summary>
/// 邮件模板实体
/// </summary>
[SugarTable("lean_rou_mail_tmpl", "邮件模板表")]
[SugarIndex("uk_code", nameof(TmplCode), OrderByType.Asc, true)]
public class LeanMailTmpl : LeanBaseEntity
{
  /// <summary>
  /// 模板编码
  /// </summary>
  [SugarColumn(ColumnName = "tmpl_code", ColumnDescription = "模板编码", Length = 100, IsNullable = false, UniqueGroupNameList = new[] { "uk_code" }, ColumnDataType = "nvarchar")]
  public string TmplCode { get; set; } = string.Empty;

  /// <summary>
  /// 模板名称
  /// </summary>
  [SugarColumn(ColumnName = "tmpl_name", ColumnDescription = "模板名称", Length = 200, IsNullable = false, ColumnDataType = "nvarchar")]
  public string TmplName { get; set; } = string.Empty;

  /// <summary>
  /// 模板主题
  /// </summary>
  [SugarColumn(ColumnName = "tmpl_subject", ColumnDescription = "模板主题", Length = 500, IsNullable = false, ColumnDataType = "nvarchar")]
  public string TmplSubject { get; set; } = string.Empty;

  /// <summary>
  /// 模板内容
  /// </summary>
  [SugarColumn(ColumnName = "tmpl_content", ColumnDescription = "模板内容", Length = 4000, IsNullable = false, ColumnDataType = "nvarchar")]
  public string TmplContent { get; set; } = string.Empty;

  /// <summary>
  /// 模板签名
  /// </summary>
  /// <remarks>
  /// 邮件模板的签名部分，可以包含发件人姓名、职位、联系方式等信息
  /// </remarks>
  [SugarColumn(ColumnName = "tmpl_signature", ColumnDescription = "模板签名", Length = 1000, IsNullable = false, ColumnDataType = "nvarchar")]
  public string TmplSignature { get; set; } = string.Empty;

  /// <summary>
  /// 是否HTML格式
  /// </summary>
  [SugarColumn(ColumnName = "tmpl_is_html", ColumnDescription = "是否HTML格式", IsNullable = false, DefaultValue = "1", ColumnDataType = "int")]
  public int TmplIsHtml { get; set; } = 1;

  /// <summary>
  /// 优先级
  /// </summary>
  [SugarColumn(ColumnName = "tmpl_priority", ColumnDescription = "优先级", IsNullable = false, DefaultValue = "2", ColumnDataType = "int")]
  public int TmplPriority { get; set; } = 2;

  /// <summary>
  /// 备注
  /// </summary>
  [SugarColumn(ColumnName = "tmpl_remark", ColumnDescription = "备注", Length = 500, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? TmplRemark { get; set; }

  /// <summary>
  /// 状态
  /// </summary>
  [SugarColumn(ColumnName = "tmpl_status", ColumnDescription = "状态", IsNullable = false, DefaultValue = "1", ColumnDataType = "int")]
  public int TmplStatus { get; set; } = 1;
}