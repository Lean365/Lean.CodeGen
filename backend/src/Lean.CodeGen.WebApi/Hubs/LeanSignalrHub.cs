//===================================================
// 项目名: Lean.CodeGen.WebApi
// 文件名: HbtSignalrHub.cs
// 功能描述: SignalR实时通信Hub
// 创建时间: 2024-03-26
// 作者: Lean
// 版本: 1.0
//===================================================

using System;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;
using Lean.CodeGen.Domain.Entities.Signalr;
using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Application.Services.Signalr;
using Lean.CodeGen.Application.Dtos.Signalr;
using Microsoft.Extensions.Logging;

namespace Lean.CodeGen.WebApi.Hubs;

/// <summary>
/// SignalR实时通信Hub
/// </summary>
public class LeanSignalrHub : Hub
{
  private readonly ILeanOnlineUserService _userService;
  private readonly ILeanOnlineMessageService _messageService;
  private readonly ILogger<LeanSignalrHub> _logger;

  public LeanSignalrHub(
      ILeanOnlineUserService userService,
      ILeanOnlineMessageService messageService,
      ILogger<LeanSignalrHub> logger)
  {
    _userService = userService;
    _messageService = messageService;
    _logger = logger;
  }

  /// <summary>
  /// 客户端连接事件
  /// </summary>
  public override async Task OnConnectedAsync()
  {
    await _userService.UpdateUserStatusAsync(Context.ConnectionId, true);
    var users = await _userService.GetOnlineUsersAsync();
    await Clients.All.SendAsync("UserConnected", users);
    await base.OnConnectedAsync();
  }

  /// <summary>
  /// 客户端断开连接事件
  /// </summary>
  public override async Task OnDisconnectedAsync(Exception? exception)
  {
    await _userService.UpdateUserStatusAsync(Context.ConnectionId, false);
    var users = await _userService.GetOnlineUsersAsync();
    await Clients.All.SendAsync("UserDisconnected", users);
    await base.OnDisconnectedAsync(exception);
  }

  /// <summary>
  /// 更新用户信息
  /// </summary>
  public async Task UpdateUserInfo(string userId, string userName, string? avatar)
  {
    await _userService.UpdateUserInfoAsync(Context.ConnectionId, userId, userName, avatar);
    var users = await _userService.GetOnlineUsersAsync();
    await Clients.All.SendAsync("UserListUpdated", users);
  }

  /// <summary>
  /// 发送消息
  /// </summary>
  public async Task SendMessage(LeanSendMessageDto input)
  {
    var message = await _messageService.SendMessageAsync(input);

    if (string.IsNullOrEmpty(message.ReceiverId))
    {
      await Clients.All.SendAsync("ReceiveMessage", message);
    }
    else
    {
      var receiverConnectionId = await _userService.GetUserConnectionIdAsync(message.ReceiverId);
      if (!string.IsNullOrEmpty(receiverConnectionId))
      {
        await Clients.Client(receiverConnectionId).SendAsync("ReceiveMessage", message);
      }
      await Clients.Caller.SendAsync("ReceiveMessage", message);
    }
  }

  /// <summary>
  /// 标记消息已读
  /// </summary>
  public async Task MarkMessageAsRead(long messageId)
  {
    await _messageService.MarkMessageAsReadAsync(messageId);
  }

  /// <summary>
  /// 获取在线用户列表
  /// </summary>
  public async Task GetOnlineUsers()
  {
    var users = await _userService.GetOnlineUsersAsync();
    await Clients.Caller.SendAsync("UserListUpdated", users);
  }

  /// <summary>
  /// 获取未读消息
  /// </summary>
  public async Task GetUnreadMessages(string userId)
  {
    var messages = await _messageService.GetUnreadMessagesAsync(userId);
    await Clients.Caller.SendAsync("UnreadMessages", messages);
  }

  /// <summary>
  /// 获取消息历史
  /// </summary>
  public async Task GetMessageHistory(string userId, int pageSize, int pageIndex)
  {
    var history = await _messageService.GetMessageHistoryAsync(userId, pageSize, pageIndex);
    await Clients.Caller.SendAsync("MessageHistory", history);
  }

  /// <summary>
  /// 更新用户最后活动时间
  /// </summary>
  public async Task UpdateLastActiveTime()
  {
    await _userService.UpdateLastActiveTimeAsync(Context.ConnectionId);
  }
}