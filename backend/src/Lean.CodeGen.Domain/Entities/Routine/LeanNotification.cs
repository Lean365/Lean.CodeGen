// -----------------------------------------------------------------------
// <copyright file="LeanNotification.cs" company="Lean">
// Copyright (c) Lean. All rights reserved.
// </copyright>
// <author>Lean</author>
// <created>2024-03-09</created>
// <summary>通知实体类</summary>
// -----------------------------------------------------------------------

using SqlSugar;

namespace Lean.CodeGen.Domain.Entities.Routine;

/// <summary>
/// 通知实体类
/// </summary>
[SugarTable("lean_rou_notification", "通知信息表")]
public class LeanNotification : LeanBaseEntity
{
  /// <summary>
  /// 通知标题
  /// </summary>
  [SugarColumn(ColumnDescription = "通知标题", Length = 255, IsNullable = false, ColumnDataType = "nvarchar")]
  public string NotificationTitle { get; set; } = string.Empty;

  /// <summary>
  /// 通知内容
  /// </summary>
  [SugarColumn(ColumnDescription = "通知内容", IsNullable = false, ColumnDataType = "ntext")]
  public string NotificationContent { get; set; } = string.Empty;

  /// <summary>
  /// 通知类型（1=系统通知，2=待办通知，3=公告通知，4=预警通知）
  /// </summary>
  [SugarColumn(ColumnDescription = "通知类型", IsNullable = false, DefaultValue = "1")]
  public int NotificationType { get; set; }

  /// <summary>
  /// 通知级别（1=一般，2=重要，3=紧急）
  /// </summary>
  [SugarColumn(ColumnDescription = "通知级别", IsNullable = false, DefaultValue = "1")]
  public int NotificationLevel { get; set; } = 1;

  /// <summary>
  /// 接收者类型（1=指定用户，2=指定角色，3=指定部门，4=全部用户）
  /// </summary>
  [SugarColumn(ColumnDescription = "接收者类型", IsNullable = false, DefaultValue = "1")]
  public int ReceiverType { get; set; }

  /// <summary>
  /// 接收者ID列表（以分号分隔）
  /// </summary>
  [SugarColumn(ColumnDescription = "接收者ID列表", Length = 1000, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? ReceiverIds { get; set; }

  /// <summary>
  /// 是否需要确认（0=否，1=是）
  /// </summary>
  [SugarColumn(ColumnDescription = "是否需要确认（0=否，1=是）", IsNullable = false, DefaultValue = "0")]
  public int RequireConfirmation { get; set; }

  /// <summary>
  /// 确认截止时间
  /// </summary>
  [SugarColumn(ColumnDescription = "确认截止时间", IsNullable = true)]
  public DateTime? ConfirmationDeadline { get; set; }

  /// <summary>
  /// 相关链接
  /// </summary>
  [SugarColumn(ColumnDescription = "相关链接", Length = 500, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? RelatedUrl { get; set; }

  /// <summary>
  /// 附件列表（以分号分隔的文件ID）
  /// </summary>
  [SugarColumn(ColumnDescription = "附件列表", Length = 1000, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? Attachments { get; set; }

  /// <summary>
  /// 相关业务模块
  /// </summary>
  [SugarColumn(ColumnDescription = "相关业务模块", Length = 100, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? BusinessModule { get; set; }

  /// <summary>
  /// 相关业务ID
  /// </summary>
  [SugarColumn(ColumnDescription = "相关业务ID", IsNullable = true)]
  public long? BusinessId { get; set; }

  /// <summary>
  /// 发布状态（0=草稿，1=已发布，2=已撤回）
  /// </summary>
  [SugarColumn(ColumnDescription = "发布状态（0=草稿，1=已发布，2=已撤回）", IsNullable = false, DefaultValue = "0")]
  public int PublishStatus { get; set; }

  /// <summary>
  /// 发布时间
  /// </summary>
  [SugarColumn(ColumnDescription = "发布时间", IsNullable = true)]
  public DateTime? PublishTime { get; set; }

  /// <summary>
  /// 是否置顶（0=否，1=是）
  /// </summary>
  [SugarColumn(ColumnDescription = "是否置顶（0=否，1=是）", IsNullable = false, DefaultValue = "0")]
  public int IsTop { get; set; }

  /// <summary>
  /// 置顶截止时间
  /// </summary>
  [SugarColumn(ColumnDescription = "置顶截止时间", IsNullable = true)]
  public DateTime? TopEndTime { get; set; }

  /// <summary>
  /// 阅读次数
  /// </summary>
  [SugarColumn(ColumnDescription = "阅读次数", IsNullable = false, DefaultValue = "0")]
  public int ReadCount { get; set; }

  /// <summary>
  /// 确认次数
  /// </summary>
  [SugarColumn(ColumnDescription = "确认次数", IsNullable = false, DefaultValue = "0")]
  public int ConfirmationCount { get; set; }
}