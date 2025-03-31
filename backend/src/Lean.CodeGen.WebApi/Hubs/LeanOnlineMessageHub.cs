using Microsoft.AspNetCore.SignalR;
using Lean.CodeGen.Domain.Interfaces.Hubs;
using Lean.CodeGen.Domain.Entities.Signalr;

namespace Lean.CodeGen.WebApi.Hubs;

/// <summary>
/// 在线消息Hub实现
/// </summary>
public class LeanOnlineMessageHub : Hub, ILeanOnlineMessageHub
{
  /// <summary>
  /// 发送消息
  /// </summary>
  public async Task SendMessageAsync(LeanOnlineMessage message)
  {
    // 私聊消息
    await Clients.User(message.ReceiverId.ToString()).SendAsync("ReceiveMessage", message);
  }

  /// <summary>
  /// 标记消息已读
  /// </summary>
  public async Task MarkMessageAsReadAsync(long messageId)
  {
    await Clients.All.SendAsync("MessageRead", messageId);
  }

  /// <summary>
  /// 获取未读消息
  /// </summary>
  public async Task GetUnreadMessagesAsync(long userId)
  {
    await Clients.User(userId.ToString()).SendAsync("UnreadMessages", new List<LeanOnlineMessage>());
  }

  /// <summary>
  /// 获取消息历史
  /// </summary>
  public async Task GetMessageHistoryAsync(long userId, int pageSize, int pageIndex)
  {
    await Clients.User(userId.ToString()).SendAsync("MessageHistory", new List<LeanOnlineMessage>());
  }
}