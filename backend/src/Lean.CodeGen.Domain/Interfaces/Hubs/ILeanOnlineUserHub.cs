using Microsoft.AspNetCore.SignalR;
using Lean.CodeGen.Domain.Entities.Signalr;

namespace Lean.CodeGen.Domain.Interfaces.Hubs;

/// <summary>
/// 在线用户Hub接口
/// </summary>
public interface ILeanOnlineUserHub
{
  /// <summary>
  /// 用户上线
  /// </summary>
  Task UserOnlineAsync(LeanOnlineUser user);

  /// <summary>
  /// 用户下线
  /// </summary>
  Task UserOfflineAsync(long userId);

  /// <summary>
  /// 更新在线用户列表
  /// </summary>
  Task UpdateOnlineUsersAsync(List<LeanOnlineUser> users);

  /// <summary>
  /// 更新用户状态
  /// </summary>
  Task UpdateUserStatusAsync(long userId, bool isOnline);
}