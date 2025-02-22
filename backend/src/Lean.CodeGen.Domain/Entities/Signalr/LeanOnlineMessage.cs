//===================================================
// 项目名: Lean.CodeGen.Domain
// 文件名: HbtOnlineMessage.cs
// 功能描述: 在线消息实体
// 创建时间: 2024-03-26
// 作者: Lean
// 版本: 1.0
//===================================================

using SqlSugar;

namespace Lean.CodeGen.Domain.Entities.Signalr;

/// <summary>
/// 在线消息实体
/// </summary>
[SugarTable("hbt_online_message", "在线消息表")]
[SugarIndex("idx_sender", nameof(SenderId), OrderByType.Asc)]
[SugarIndex("idx_receiver", nameof(ReceiverId), OrderByType.Asc)]
public class LeanOnlineMessage : LeanBaseEntity
{
  /// <summary>
  /// 发送者ID
  /// </summary>
  /// <remarks>
  /// 消息发送者的用户ID
  /// </remarks>
  [SugarColumn(ColumnName = "sender_id", ColumnDescription = "发送者ID", Length = 50, IsNullable = false, ColumnDataType = "nvarchar")]
  public string SenderId { get; set; } = default!;

  /// <summary>
  /// 发送者名称
  /// </summary>
  /// <remarks>
  /// 消息发送者的显示名称
  /// </remarks>
  [SugarColumn(ColumnName = "sender_name", ColumnDescription = "发送者名称", Length = 50, IsNullable = false, ColumnDataType = "nvarchar")]
  public string SenderName { get; set; } = default!;

  /// <summary>
  /// 发送者头像
  /// </summary>
  /// <remarks>
  /// 消息发送者的头像URL
  /// </remarks>
  [SugarColumn(ColumnName = "sender_avatar", ColumnDescription = "发送者头像", Length = 500, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? SenderAvatar { get; set; }

  /// <summary>
  /// 接收者ID
  /// </summary>
  /// <remarks>
  /// 消息接收者的用户ID，为空表示群发消息
  /// </remarks>
  [SugarColumn(ColumnName = "receiver_id", ColumnDescription = "接收者ID", Length = 50, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? ReceiverId { get; set; }

  /// <summary>
  /// 接收者名称
  /// </summary>
  /// <remarks>
  /// 消息接收者的显示名称
  /// </remarks>
  [SugarColumn(ColumnName = "receiver_name", ColumnDescription = "接收者名称", Length = 50, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? ReceiverName { get; set; }

  /// <summary>
  /// 消息内容
  /// </summary>
  /// <remarks>
  /// 消息的具体内容
  /// </remarks>
  [SugarColumn(ColumnName = "content", ColumnDescription = "消息内容", Length = 4000, IsNullable = false, ColumnDataType = "nvarchar")]
  public string Content { get; set; } = default!;

  /// <summary>
  /// 发送时间
  /// </summary>
  /// <remarks>
  /// 消息发送的时间
  /// </remarks>
  [SugarColumn(ColumnName = "send_time", ColumnDescription = "发送时间", IsNullable = false, ColumnDataType = "datetime")]
  public DateTime SendTime { get; set; }

  /// <summary>
  /// 是否已读
  /// </summary>
  /// <remarks>
  /// 消息是否已被接收者阅读
  /// </remarks>
  [SugarColumn(ColumnName = "is_read", ColumnDescription = "是否已读", IsNullable = false, DefaultValue = "0", ColumnDataType = "bit")]
  public bool IsRead { get; set; }

  /// <summary>
  /// 消息类型
  /// </summary>
  /// <remarks>
  /// 消息的类型，如：文本、图片、文件等
  /// </remarks>
  [SugarColumn(ColumnName = "message_type", ColumnDescription = "消息类型", Length = 50, IsNullable = false, ColumnDataType = "nvarchar")]
  public string MessageType { get; set; } = default!;
}