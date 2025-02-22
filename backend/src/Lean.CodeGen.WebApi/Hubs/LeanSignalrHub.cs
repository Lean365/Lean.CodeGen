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

namespace Lean.CodeGen.WebApi.Hubs;

/// <summary>
/// SignalR实时通信Hub
/// </summary>
public class LeanSignalrHub : Hub
{
  private static readonly ConcurrentDictionary<string, LeanOnlineUser> OnlineUsers = new();

  /// <summary>
  /// 客户端连接事件
  /// </summary>
  public override async Task OnConnectedAsync()
  {
    var user = new LeanOnlineUser
    {
      ConnectionId = Context.ConnectionId,
      LastActiveTime = DateTime.Now,
      IsOnline = true,
      CreateTime = DateTime.Now
    };

    OnlineUsers.TryAdd(Context.ConnectionId, user);
    await Clients.All.SendAsync("UserConnected", user);
    await base.OnConnectedAsync();
  }

  /// <summary>
  /// 客户端断开连接事件
  /// </summary>
  public override async Task OnDisconnectedAsync(Exception? exception)
  {
    if (OnlineUsers.TryRemove(Context.ConnectionId, out var user))
    {
      user.IsOnline = false;
      user.UpdateTime = DateTime.Now;
      await Clients.All.SendAsync("UserDisconnected", user);
    }
    await base.OnDisconnectedAsync(exception);
  }

  /// <summary>
  /// 更新用户信息
  /// </summary>
  public async Task UpdateUserInfo(string userId, string userName, string? avatar)
  {
    if (OnlineUsers.TryGetValue(Context.ConnectionId, out var user))
    {
      user.UserId = userId;
      user.UserName = userName;
      user.Avatar = avatar;
      user.UpdateTime = DateTime.Now;
      await Clients.All.SendAsync("UserUpdated", user);
    }
  }

  /// <summary>
  /// 发送消息
  /// </summary>
  public async Task SendMessage(LeanOnlineMessage message)
  {
    message.SendTime = DateTime.Now;
    message.CreateTime = DateTime.Now;

    if (string.IsNullOrEmpty(message.ReceiverId))
    {
      await Clients.All.SendAsync("ReceiveMessage", message);
    }
    else
    {
      await Clients.User(message.ReceiverId).SendAsync("ReceiveMessage", message);
      await Clients.Caller.SendAsync("ReceiveMessage", message);
    }
  }

  /// <summary>
  /// 标记消息已读
  /// </summary>
  public async Task MarkMessageAsRead(long messageId)
  {
    await Clients.All.SendAsync("MessageRead", messageId);
  }

  /// <summary>
  /// 获取在线用户列表
  /// </summary>
  public async Task GetOnlineUsers()
  {
    await Clients.Caller.SendAsync("OnlineUsers", OnlineUsers.Values);
  }

  /// <summary>
  /// 更新用户最后活动时间
  /// </summary>
  public async Task UpdateLastActiveTime()
  {
    if (OnlineUsers.TryGetValue(Context.ConnectionId, out var user))
    {
      user.LastActiveTime = DateTime.Now;
      user.UpdateTime = DateTime.Now;
      await Clients.All.SendAsync("UserUpdated", user);
    }
  }
}