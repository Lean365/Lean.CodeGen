using Microsoft.AspNetCore.SignalR;
using Lean.CodeGen.Domain.Entities.Signalr;

namespace Lean.CodeGen.Domain.Interfaces.Hubs;

/// <summary>
/// 在线消息Hub接口
/// </summary>
public interface ILeanOnlineMessageHub
{
  /// <summary>
  /// 发送消息
  /// </summary>
  Task SendMessageAsync(LeanOnlineMessage message);

  /// <summary>
  /// 标记消息已读
  /// </summary>
  Task MarkMessageAsReadAsync(long messageId);

  /// <summary>
  /// 获取未读消息
  /// </summary>
  Task GetUnreadMessagesAsync(long userId);

  /// <summary>
  /// 获取消息历史
  /// </summary>
  Task GetMessageHistoryAsync(long userId, int pageSize, int pageIndex);
}