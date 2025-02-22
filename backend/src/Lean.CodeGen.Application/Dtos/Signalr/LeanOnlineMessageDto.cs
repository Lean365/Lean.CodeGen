using System.ComponentModel.DataAnnotations;
using Lean.CodeGen.Common.Models;

namespace Lean.CodeGen.Application.Dtos.Signalr;

/// <summary>
/// 在线消息查询参数
/// </summary>
public class LeanQueryOnlineMessageDto : LeanPage
{
  /// <summary>
  /// 发送者ID
  /// </summary>
  public string? SenderId { get; set; }

  /// <summary>
  /// 接收者ID
  /// </summary>
  public string? ReceiverId { get; set; }

  /// <summary>
  /// 是否已读
  /// </summary>
  public bool? IsRead { get; set; }

  /// <summary>
  /// 消息类型
  /// </summary>
  public string? MessageType { get; set; }

  /// <summary>
  /// 开始时间
  /// </summary>
  public DateTime? StartTime { get; set; }

  /// <summary>
  /// 结束时间
  /// </summary>
  public DateTime? EndTime { get; set; }
}

/// <summary>
/// 在线消息DTO
/// </summary>
public class LeanOnlineMessageDto
{
  /// <summary>
  /// 消息ID
  /// </summary>
  public long Id { get; set; }

  /// <summary>
  /// 发送者ID
  /// </summary>
  public string SenderId { get; set; } = null!;

  /// <summary>
  /// 发送者名称
  /// </summary>
  public string SenderName { get; set; } = null!;

  /// <summary>
  /// 发送者头像
  /// </summary>
  public string? SenderAvatar { get; set; }

  /// <summary>
  /// 接收者ID
  /// </summary>
  public string? ReceiverId { get; set; }

  /// <summary>
  /// 接收者名称
  /// </summary>
  public string? ReceiverName { get; set; }

  /// <summary>
  /// 接收者头像
  /// </summary>
  public string? ReceiverAvatar { get; set; }

  /// <summary>
  /// 消息内容
  /// </summary>
  public string Content { get; set; } = null!;

  /// <summary>
  /// 消息类型
  /// </summary>
  public string MessageType { get; set; } = "text";

  /// <summary>
  /// 是否已读
  /// </summary>
  public bool IsRead { get; set; }

  /// <summary>
  /// 发送时间
  /// </summary>
  public DateTime SendTime { get; set; }

  /// <summary>
  /// 创建时间
  /// </summary>
  public DateTime CreateTime { get; set; }

  /// <summary>
  /// 更新时间
  /// </summary>
  public DateTime? UpdateTime { get; set; }
}

/// <summary>
/// 发送消息DTO
/// </summary>
public class LeanSendMessageDto
{
  /// <summary>
  /// 发送者ID
  /// </summary>
  [Required(ErrorMessage = "发送者ID不能为空")]
  public string SenderId { get; set; } = null!;

  /// <summary>
  /// 接收者ID
  /// </summary>
  public string? ReceiverId { get; set; }

  /// <summary>
  /// 消息内容
  /// </summary>
  [Required(ErrorMessage = "消息内容不能为空")]
  public string Content { get; set; } = null!;

  /// <summary>
  /// 消息类型
  /// </summary>
  public string MessageType { get; set; } = "text";
}

/// <summary>
/// 批量标记消息已读DTO
/// </summary>
public class LeanMarkMessagesAsReadDto
{
  /// <summary>
  /// 用户ID
  /// </summary>
  [Required(ErrorMessage = "用户ID不能为空")]
  public string UserId { get; set; } = null!;

  /// <summary>
  /// 发送者ID
  /// </summary>
  [Required(ErrorMessage = "发送者ID不能为空")]
  public string SenderId { get; set; } = null!;
}