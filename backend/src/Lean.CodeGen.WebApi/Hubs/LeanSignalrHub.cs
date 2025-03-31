using Microsoft.AspNetCore.SignalR;
using Lean.CodeGen.Domain.Interfaces.Hubs;
using Lean.CodeGen.Application.Dtos.Routine;
using NLog;
using ILogger = NLog.ILogger;
using Lean.CodeGen.Application.Services.Signalr;
using Lean.CodeGen.Domain.Entities.Signalr;
using System.Security.Claims;
using Lean.CodeGen.Common.Exceptions;
using Lean.CodeGen.Common.Helpers;
using Lean.CodeGen.Application.Dtos.Signalr;
using Lean.CodeGen.Domain.Context;

namespace Lean.CodeGen.WebApi.Hubs;

/// <summary>
/// SignalR Hub实现
/// </summary>
public class LeanSignalRHub : Hub, ILeanSignalRHub
{
  private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
  private readonly IHubContext<LeanSignalRHub> _hubContext;
  private readonly ILeanOnlineUserService _onlineUserService;
  private readonly ILeanOnlineMessageService _onlineMessageService;
  private readonly ILeanUserContext _userContext;

  public LeanSignalRHub(
    IHubContext<LeanSignalRHub> hubContext,
    ILeanOnlineUserService onlineUserService,
    ILeanOnlineMessageService onlineMessageService,
    ILeanUserContext userContext)
  {
    _hubContext = hubContext;
    _onlineUserService = onlineUserService;
    _onlineMessageService = onlineMessageService;
    _userContext = userContext;
  }

  /// <summary>
  /// 客户端连接时触发
  /// </summary>
  public override async Task OnConnectedAsync()
  {
    try
    {
      var userId = _userContext.CurrentUserId;
      if (!userId.HasValue)
      {
        _logger.Warn($"用户连接失败 - 无效的用户ID - ConnectionId: {Context.ConnectionId}");
        return;
      }

      var userAgent = Context.User?.FindFirst("UserAgent")?.Value;
      var deviceType = Context.User?.FindFirst("DeviceType")?.Value;
      var deviceFingerprint = Context.User?.FindFirst("DeviceFingerprint")?.Value;

      if (string.IsNullOrEmpty(userAgent) || string.IsNullOrEmpty(deviceType) || string.IsNullOrEmpty(deviceFingerprint))
      {
        _logger.Warn($"用户连接失败 - 无效的设备信息 - ConnectionId: {Context.ConnectionId}");
        return;
      }

      await _onlineUserService.UpdateUserInfoAsync(Context.ConnectionId, userId.Value, null, null, deviceFingerprint);
      await _onlineUserService.UpdateUserStatusAsync(Context.ConnectionId, true, userId.Value, deviceFingerprint);
      _logger.Info($"用户连接成功 - ConnectionId: {Context.ConnectionId}, UserId: {userId}");
    }
    catch (Exception ex)
    {
      _logger.Error(ex, $"用户连接失败 - ConnectionId: {Context.ConnectionId}");
      throw;
    }
  }

  /// <summary>
  /// 客户端断开连接时触发
  /// </summary>
  public override async Task OnDisconnectedAsync(Exception? exception)
  {
    try
    {
      var userId = _userContext.CurrentUserId;
      if (!userId.HasValue)
      {
        _logger.Warn($"用户断开连接失败 - 无效的用户ID - ConnectionId: {Context.ConnectionId}");
        return;
      }

      var deviceFingerprint = Context.User?.FindFirst("DeviceFingerprint")?.Value;

      if (string.IsNullOrEmpty(deviceFingerprint))
      {
        _logger.Warn($"用户断开连接失败 - 无效的设备指纹 - ConnectionId: {Context.ConnectionId}");
        return;
      }

      await _onlineUserService.UpdateUserStatusAsync(Context.ConnectionId, false, userId.Value, deviceFingerprint);
      _logger.Info($"用户断开连接成功 - ConnectionId: {Context.ConnectionId}, UserId: {userId}");
    }
    catch (Exception ex)
    {
      _logger.Error(ex, $"用户断开连接失败 - ConnectionId: {Context.ConnectionId}");
      throw;
    }
  }

  /// <summary>
  /// 心跳检测
  /// </summary>
  public async Task HeartbeatAsync()
  {
    try
    {
      var userId = _userContext.CurrentUserId;
      if (!userId.HasValue)
      {
        _logger.Warn($"心跳检测失败 - 无效的用户ID - ConnectionId: {Context.ConnectionId}");
        return;
      }

      var userAgent = Context.User?.FindFirst("UserAgent")?.Value;
      var deviceType = Context.User?.FindFirst("DeviceType")?.Value;
      var deviceFingerprint = Context.User?.FindFirst("DeviceFingerprint")?.Value;

      if (!string.IsNullOrEmpty(userAgent) && !string.IsNullOrEmpty(deviceType) && !string.IsNullOrEmpty(deviceFingerprint))
      {
        // 使用 LeanDeviceHelper 生成设备ID
        var deviceId = LeanDeviceHelper.GenerateDeviceId(deviceFingerprint, userAgent, deviceType);

        // 更新最后活动时间
        await _onlineUserService.UpdateLastActiveTimeAsync(
          connectionId: Context.ConnectionId,
          userId: userId.Value,
          deviceId: deviceId
        );
      }
    }
    catch (Exception ex)
    {
      _logger.Error(ex, $"心跳检测失败 - ConnectionId: {Context.ConnectionId}");
      throw;
    }
  }

  /// <summary>
  /// 发送用户登录尝试通知
  /// </summary>
  public async Task SendUserLoginAttemptAsync(long userId, string message, DateTime loginTime, string loginIp, string loginLocation)
  {
    try
    {
      if (userId <= 0)
      {
        _logger.Warn($"无效的用户ID: {userId}");
        return;
      }

      if (string.IsNullOrEmpty(message))
      {
        _logger.Warn("消息内容不能为空");
        return;
      }

      if (string.IsNullOrEmpty(loginIp))
      {
        _logger.Warn("登录IP不能为空");
        return;
      }

      if (string.IsNullOrEmpty(loginLocation))
      {
        _logger.Warn("登录地点不能为空");
        return;
      }

      await _hubContext.Clients.User(userId.ToString()).SendAsync("ReceiveUserLoginAttempt", message, loginTime, loginIp, loginLocation);
      _logger.Info($"已发送用户登录尝试通知 - 用户ID: {userId}, 消息: {message}");
    }
    catch (Exception ex)
    {
      _logger.Error(ex, $"发送用户登录尝试通知失败 - 用户ID: {userId}");
      throw;
    }
  }

  /// <summary>
  /// 发送通知
  /// </summary>
  public async Task SendNotificationAsync(long userId, object notification)
  {
    try
    {
      if (userId <= 0)
      {
        _logger.Warn($"无效的用户ID: {userId}");
        return;
      }

      if (notification == null)
      {
        _logger.Warn("通知内容不能为空");
        return;
      }

      await _hubContext.Clients.User(userId.ToString()).SendAsync("ReceiveNotification", notification);
      _logger.Info($"已发送通知 - 用户ID: {userId}");
    }
    catch (Exception ex)
    {
      _logger.Error(ex, $"发送通知失败 - 用户ID: {userId}");
      throw;
    }
  }

  /// <summary>
  /// 发送消息
  /// </summary>
  public async Task SendMessageAsync(LeanOnlineMessage message)
  {
    try
    {
      if (message == null)
      {
        _logger.Warn("消息内容不能为空");
        return;
      }

      if (message.ReceiverId <= 0)
      {
        _logger.Warn($"无效的接收者ID: {message.ReceiverId}");
        return;
      }

      await _hubContext.Clients.User(message.ReceiverId.ToString()).SendAsync("ReceiveMessage", message);
      _logger.Info($"已发送消息 - 接收者ID: {message.ReceiverId}");
    }
    catch (Exception ex)
    {
      _logger.Error(ex, $"发送消息失败 - 接收者ID: {message.ReceiverId}");
      throw;
    }
  }

  /// <summary>
  /// 获取未读消息
  /// </summary>
  public async Task GetUnreadMessagesAsync(long userId)
  {
    try
    {
      if (userId <= 0)
      {
        _logger.Warn($"无效的用户ID: {userId}");
        return;
      }

      var messages = await _onlineMessageService.GetUnreadMessagesAsync(userId);
      await _hubContext.Clients.User(userId.ToString()).SendAsync("ReceiveUnreadMessages", messages);
      _logger.Info($"已获取未读消息 - 用户ID: {userId}");
    }
    catch (Exception ex)
    {
      _logger.Error(ex, $"获取未读消息失败 - 用户ID: {userId}");
      throw;
    }
  }

  /// <summary>
  /// 获取消息历史
  /// </summary>
  public async Task GetMessageHistoryAsync(long userId, int pageSize, int pageIndex)
  {
    try
    {
      if (userId <= 0)
      {
        _logger.Warn($"无效的用户ID: {userId}");
        return;
      }

      if (pageSize <= 0)
      {
        _logger.Warn($"无效的页面大小: {pageSize}");
        return;
      }

      if (pageIndex <= 0)
      {
        _logger.Warn($"无效的页码: {pageIndex}");
        return;
      }

      var messages = await _onlineMessageService.GetMessageHistoryAsync(userId, pageSize, pageIndex);
      await _hubContext.Clients.User(userId.ToString()).SendAsync("ReceiveMessageHistory", messages);
      _logger.Info($"已获取消息历史 - 用户ID: {userId}, 页码: {pageIndex}, 页面大小: {pageSize}");
    }
    catch (Exception ex)
    {
      _logger.Error(ex, $"获取消息历史失败 - 用户ID: {userId}");
      throw;
    }
  }

  /// <summary>
  /// 标记消息已读
  /// </summary>
  public async Task MarkMessageAsReadAsync(long messageId)
  {
    try
    {
      if (messageId <= 0)
      {
        _logger.Warn($"无效的消息ID: {messageId}");
        return;
      }

      await _onlineMessageService.MarkMessageAsReadAsync(messageId);
      await _hubContext.Clients.All.SendAsync("MessageRead", messageId);
      _logger.Info($"已标记消息已读 - 消息ID: {messageId}");
    }
    catch (Exception ex)
    {
      _logger.Error(ex, $"标记消息已读失败 - 消息ID: {messageId}");
      throw;
    }
  }

  /// <summary>
  /// 批量标记消息已读
  /// </summary>
  public async Task MarkMessagesAsReadAsync(LeanOnlineMessageMarkAsReadDto input)
  {
    try
    {
      if (input == null)
      {
        _logger.Warn("输入参数不能为空");
        return;
      }

      if (input.UserId <= 0)
      {
        _logger.Warn($"无效的用户ID: {input.UserId}");
        return;
      }

      if (input.SenderId <= 0)
      {
        _logger.Warn($"无效的发送者ID: {input.SenderId}");
        return;
      }

      await _onlineMessageService.MarkMessagesAsReadAsync(input);
      await _hubContext.Clients.User(input.UserId.ToString()).SendAsync("MessagesRead", input);
      _logger.Info($"已批量标记消息已读 - 用户ID: {input.UserId}, 发送者ID: {input.SenderId}");
    }
    catch (Exception ex)
    {
      _logger.Error(ex, $"批量标记消息已读失败 - 用户ID: {input.UserId}");
      throw;
    }
  }

  /// <summary>
  /// 删除消息
  /// </summary>
  public async Task DeleteMessageAsync(long messageId)
  {
    try
    {
      if (messageId <= 0)
      {
        _logger.Warn($"无效的消息ID: {messageId}");
        return;
      }

      await _onlineMessageService.DeleteMessageAsync(messageId);
      await _hubContext.Clients.All.SendAsync("MessageDeleted", messageId);
      _logger.Info($"已删除消息 - 消息ID: {messageId}");
    }
    catch (Exception ex)
    {
      _logger.Error(ex, $"删除消息失败 - 消息ID: {messageId}");
      throw;
    }
  }

  /// <summary>
  /// 清理过期消息
  /// </summary>
  public async Task CleanExpiredMessagesAsync(int days = 30)
  {
    try
    {
      if (days <= 0)
      {
        _logger.Warn($"无效的天数: {days}");
        return;
      }

      await _onlineMessageService.CleanExpiredMessagesAsync(days);
      await _hubContext.Clients.All.SendAsync("MessagesCleaned", days);
      _logger.Info($"已清理过期消息 - 天数: {days}");
    }
    catch (Exception ex)
    {
      _logger.Error(ex, $"清理过期消息失败 - 天数: {days}");
      throw;
    }
  }

  /// <summary>
  /// 创建消息
  /// </summary>
  public async Task CreateMessageAsync(LeanOnlineMessage message)
  {
    try
    {
      if (message == null)
      {
        _logger.Warn("消息内容不能为空");
        return;
      }

      if (message.SenderId <= 0)
      {
        _logger.Warn($"无效的发送者ID: {message.SenderId}");
        return;
      }

      if (message.ReceiverId <= 0)
      {
        _logger.Warn($"无效的接收者ID: {message.ReceiverId}");
        return;
      }

      await _onlineMessageService.CreateMessageAsync(message);
      await _hubContext.Clients.User(message.ReceiverId.ToString()).SendAsync("MessageCreated", message);
      _logger.Info($"已创建消息 - 发送者ID: {message.SenderId}, 接收者ID: {message.ReceiverId}");
    }
    catch (Exception ex)
    {
      _logger.Error(ex, $"创建消息失败 - 发送者ID: {message.SenderId}, 接收者ID: {message.ReceiverId}");
      throw;
    }
  }

  /// <summary>
  /// 获取消息列表
  /// </summary>
  public async Task GetPageListAsync(LeanOnlineMessageQueryDto input)
  {
    try
    {
      if (input == null)
      {
        _logger.Warn("查询参数不能为空");
        return;
      }

      if (input.PageSize <= 0)
      {
        _logger.Warn($"无效的页面大小: {input.PageSize}");
        return;
      }

      if (input.PageIndex <= 0)
      {
        _logger.Warn($"无效的页码: {input.PageIndex}");
        return;
      }

      var messages = await _onlineMessageService.GetPageListAsync(input);
      await _hubContext.Clients.All.SendAsync("MessageListUpdated", messages);
      _logger.Info($"已获取消息列表 - 页码: {input.PageIndex}, 页面大小: {input.PageSize}");
    }
    catch (Exception ex)
    {
      _logger.Error(ex, $"获取消息列表失败 - 页码: {input.PageIndex}");
      throw;
    }
  }

  /// <summary>
  /// 获取消息详情
  /// </summary>
  public async Task GetAsync(long id)
  {
    try
    {
      if (id <= 0)
      {
        _logger.Warn($"无效的消息ID: {id}");
        return;
      }

      var message = await _onlineMessageService.GetAsync(id);
      await _hubContext.Clients.All.SendAsync("MessageDetail", message);
      _logger.Info($"已获取消息详情 - 消息ID: {id}");
    }
    catch (Exception ex)
    {
      _logger.Error(ex, $"获取消息详情失败 - 消息ID: {id}");
      throw;
    }
  }
}