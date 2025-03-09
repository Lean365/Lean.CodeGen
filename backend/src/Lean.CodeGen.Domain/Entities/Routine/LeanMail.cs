// -----------------------------------------------------------------------
// <copyright file="LeanMail.cs" company="Lean">
// Copyright (c) Lean. All rights reserved.
// </copyright>
// <author>Lean</author>
// <created>2024-03-09</created>
// <summary>邮件实体类</summary>
// -----------------------------------------------------------------------

using SqlSugar;

namespace Lean.CodeGen.Domain.Entities.Routine;

/// <summary>
/// 邮件实体类
/// </summary>
[SugarTable("lean_rou_mail", "邮件信息表")]
public class LeanMail : LeanBaseEntity
{
  /// <summary>
  /// 邮件主题
  /// </summary>
  [SugarColumn(ColumnDescription = "邮件主题", Length = 255, IsNullable = false, ColumnDataType = "nvarchar")]
  public string Subject { get; set; } = string.Empty;

  /// <summary>
  /// 发件人邮箱
  /// </summary>
  [SugarColumn(ColumnDescription = "发件人邮箱", Length = 100, IsNullable = false, ColumnDataType = "nvarchar")]
  public string FromAddress { get; set; } = string.Empty;

  /// <summary>
  /// 发件人显示名称
  /// </summary>
  [SugarColumn(ColumnDescription = "发件人显示名称", Length = 100, IsNullable = true, ColumnDataType = "nvarchar")]
  public string FromName { get; set; } = string.Empty;

  /// <summary>
  /// 收件人邮箱列表（以分号分隔）
  /// </summary>
  [SugarColumn(ColumnDescription = "收件人邮箱列表", Length = 1000, IsNullable = false, ColumnDataType = "nvarchar")]
  public string ToAddresses { get; set; } = string.Empty;

  /// <summary>
  /// 抄送邮箱列表（以分号分隔）
  /// </summary>
  [SugarColumn(ColumnDescription = "抄送邮箱列表", Length = 1000, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? CcAddresses { get; set; }

  /// <summary>
  /// 密送邮箱列表（以分号分隔）
  /// </summary>
  [SugarColumn(ColumnDescription = "密送邮箱列表", Length = 1000, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? BccAddresses { get; set; }

  /// <summary>
  /// 邮件正文
  /// </summary>
  [SugarColumn(ColumnDescription = "邮件正文", IsNullable = false, ColumnDataType = "ntext")]
  public string Body { get; set; } = string.Empty;

  /// <summary>
  /// 是否为HTML格式（0=否，1=是）
  /// </summary>
  [SugarColumn(ColumnDescription = "是否为HTML格式（0=否，1=是）", IsNullable = false, DefaultValue = "1")]
  public int IsBodyHtml { get; set; } = 1;

  /// <summary>
  /// 优先级（1=低，2=普通，3=高）
  /// </summary>
  [SugarColumn(ColumnDescription = "优先级（1=低，2=普通，3=高）", IsNullable = false, DefaultValue = "2")]
  public int Priority { get; set; } = 2;

  /// <summary>
  /// 附件列表（以分号分隔的文件ID）
  /// </summary>
  [SugarColumn(ColumnDescription = "附件列表", Length = 1000, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? Attachments { get; set; }

  /// <summary>
  /// 附件ID列表，以逗号分隔
  /// </summary>
  [SugarColumn(ColumnName = "attachment_ids", ColumnDescription = "附件ID列表", Length = 1000, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? AttachmentIds { get; set; }

  /// <summary>
  /// 发送状态（0=待发送，1=发送成功，2=发送失败）
  /// </summary>
  [SugarColumn(ColumnDescription = "发送状态（0=待发送，1=发送成功，2=发送失败）", IsNullable = false, DefaultValue = "0")]
  public int SendStatus { get; set; }

  /// <summary>
  /// 发送时间
  /// </summary>
  [SugarColumn(ColumnDescription = "发送时间", IsNullable = true)]
  public DateTime? SendTime { get; set; }

  /// <summary>
  /// 失败原因
  /// </summary>
  [SugarColumn(ColumnDescription = "失败原因", Length = 500, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? FailureReason { get; set; }

  /// <summary>
  /// 重试次数
  /// </summary>
  [SugarColumn(ColumnDescription = "重试次数", IsNullable = false, DefaultValue = "0")]
  public int RetryCount { get; set; }
}