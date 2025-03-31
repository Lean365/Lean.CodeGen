using Microsoft.AspNetCore.SignalR;
using Lean.CodeGen.Domain.Interfaces.Hubs;
using Lean.CodeGen.Domain.Entities.Signalr;

namespace Lean.CodeGen.WebApi.Hubs;

/// <summary>
/// 在线用户Hub实现
/// </summary>
public class LeanOnlineUserHub : Hub, ILeanOnlineUserHub
{
  /// <summary>
  /// 用户上线
  /// </summary>
  public async Task UserOnlineAsync(LeanOnlineUser user)
  {
    await Clients.All.SendAsync("UserOnline", user);
  }

  /// <summary>
  /// 用户下线
  /// </summary>
  public async Task UserOfflineAsync(long userId)
  {
    await Clients.All.SendAsync("UserOffline", userId);
  }

  /// <summary>
  /// 更新在线用户列表
  /// </summary>
  public async Task UpdateOnlineUsersAsync(List<LeanOnlineUser> users)
  {
    await Clients.All.SendAsync("UserListUpdated", users);
  }

  /// <summary>
  /// 更新用户状态
  /// </summary>
  public async Task UpdateUserStatusAsync(long userId, bool isOnline)
  {
    await Clients.All.SendAsync("UserStatusUpdated", userId, isOnline);
  }
}