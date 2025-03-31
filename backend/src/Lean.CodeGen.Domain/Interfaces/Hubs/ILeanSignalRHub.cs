using Microsoft.AspNetCore.SignalR;

namespace Lean.CodeGen.Domain.Interfaces.Hubs;

/// <summary>
/// SignalR Hub接口
/// </summary>
public interface ILeanSignalRHub
{
  /// <summary>
  /// 发送用户登录尝试通知
  /// </summary>
  /// <param name="userId">用户ID</param>
  /// <param name="message">消息</param>
  /// <param name="loginTime">登录时间</param>
  /// <param name="loginIp">登录IP</param>
  /// <param name="loginLocation">登录地点</param>
  Task SendUserLoginAttemptAsync(long userId, string message, DateTime loginTime, string loginIp, string loginLocation);

  /// <summary>
  /// 发送通知
  /// </summary>
  /// <param name="userId">用户ID</param>
  /// <param name="notification">通知内容</param>
  Task SendNotificationAsync(long userId, object notification);
}