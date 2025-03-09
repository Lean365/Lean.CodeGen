using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Lean.CodeGen.Application.Hubs;

/// <summary>
/// 通知SignalR Hub
/// </summary>
public class LeanNotificationHub : Hub
{
  /// <summary>
  /// 连接建立时
  /// </summary>
  public override async Task OnConnectedAsync()
  {
    var userId = Context.User?.FindFirst("sub")?.Value;
    if (!string.IsNullOrEmpty(userId))
    {
      await Groups.AddToGroupAsync(Context.ConnectionId, $"User_{userId}");
    }
    await base.OnConnectedAsync();
  }

  /// <summary>
  /// 连接断开时
  /// </summary>
  public override async Task OnDisconnectedAsync(Exception? exception)
  {
    var userId = Context.User?.FindFirst("sub")?.Value;
    if (!string.IsNullOrEmpty(userId))
    {
      await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"User_{userId}");
    }
    await base.OnDisconnectedAsync(exception);
  }
}