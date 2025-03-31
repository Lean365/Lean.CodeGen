using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq.Expressions;
using NLog;
using Microsoft.Extensions.Options;
using Mapster;
using Lean.CodeGen.Common.Options;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Common.Extensions;
using Lean.CodeGen.Domain.Entities.Signalr;
using Lean.CodeGen.Domain.Interfaces.Repositories;
using Lean.CodeGen.Application.Services.Base;
using Lean.CodeGen.Application.Services.Security;
using Lean.CodeGen.Application.Dtos.Signalr;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Lean.CodeGen.Application.Services.Signalr;

/// <summary>
/// 在线用户服务实现
/// </summary>
/// <remarks>
/// 提供在线用户管理功能，包括：
/// 1. 用户在线状态管理
/// 2. 用户信息更新
/// 3. 用户查询和分页
/// 4. 用户强制登出
/// 5. 离线用户清理
/// </remarks>
public class LeanOnlineUserService : LeanBaseService, ILeanOnlineUserService
{
  private readonly ILeanRepository<LeanOnlineUser> _userRepository;
  private readonly ILogger<LeanOnlineUserService> _logger;
  private readonly IHttpContextAccessor _httpContextAccessor;

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="userRepository">在线用户仓储接口</param>
  /// <param name="context">基础服务上下文</param>
  /// <param name="logger">日志记录器</param>
  /// <param name="httpContextAccessor">HTTP上下文访问器</param>
  public LeanOnlineUserService(
      ILeanRepository<LeanOnlineUser> userRepository,
      LeanBaseServiceContext context,
      ILogger<LeanOnlineUserService> logger,
      IHttpContextAccessor httpContextAccessor)
      : base(context)
  {
    _userRepository = userRepository;
    _logger = logger;
    _httpContextAccessor = httpContextAccessor;
  }

  /// <summary>
  /// 获取在线用户列表
  /// </summary>
  public async Task<List<LeanOnlineUser>> GetOnlineUsersAsync()
  {
    var users = await _userRepository.GetListAsync(u => u.IsOnline == 1);
    return users;
  }

  /// <summary>
  /// 获取指定用户的在线状态
  /// </summary>
  /// <param name="userIds">用户ID列表</param>
  /// <returns>在线用户列表</returns>
  public async Task<List<LeanOnlineUserDto>> GetOnlineUsersAsync(long[] userIds)
  {
    var users = await _userRepository.GetListAsync(u => userIds.Contains(u.UserId));
    return users.Select(u => new LeanOnlineUserDto
    {
      UserId = u.UserId,
      UserName = u.UserName ?? string.Empty,
      Avatar = u.Avatar,
      ConnectionId = u.ConnectionId,
      DeviceId = u.DeviceId,
      IsOnline = u.IsOnline,
      LastActiveTime = u.LastActiveTime,
      CreateTime = u.CreateTime,
      UpdateTime = u.UpdateTime
    }).ToList();
  }

  /// <summary>
  /// 检查用户是否在线
  /// </summary>
  /// <param name="userId">用户ID</param>
  /// <param name="deviceId">设备指纹</param>
  /// <returns>用户是否在线</returns>
  public async Task<bool> IsUserOnlineAsync(long userId, string deviceId)
  {
    var user = await _userRepository.FirstOrDefaultAsync(u =>
        u.UserId == userId &&
        u.DeviceId == deviceId);
    return user?.IsOnline == 1;
  }

  /// <summary>
  /// 更新用户在线状态
  /// </summary>
  /// <param name="connectionId">连接ID</param>
  /// <param name="isOnline">是否在线</param>
  /// <param name="userId">用户ID</param>
  /// <param name="deviceId">设备ID</param>
  public async Task UpdateUserStatusAsync(string connectionId, bool isOnline, long userId, string deviceId)
  {
    var user = await _userRepository.FirstOrDefaultAsync(x =>
        x.UserId == userId &&
        x.DeviceId == deviceId);

    if (user != null)
    {
      user.ConnectionId = connectionId;
      user.LastActiveTime = DateTime.Now;
      user.IsOnline = isOnline ? 1 : 0;
      await _userRepository.UpdateAsync(user);
    }
  }

  /// <summary>
  /// 更新用户信息
  /// </summary>
  /// <param name="connectionId">连接ID</param>
  /// <param name="userId">用户ID</param>
  /// <param name="userName">用户名</param>
  /// <param name="avatar">头像</param>
  /// <param name="deviceFingerprint">设备指纹</param>
  public async Task UpdateUserInfoAsync(string connectionId, long userId, string? userName, string? avatar, string deviceFingerprint)
  {
    var user = await _userRepository.FirstOrDefaultAsync(x =>
        x.UserId == userId &&
        x.DeviceId == deviceFingerprint);

    if (user == null)
    {
      user = new LeanOnlineUser
      {
        ConnectionId = connectionId,
        UserId = userId,
        UserName = userName,
        Avatar = avatar,
        IsOnline = 1,
        LastActiveTime = DateTime.Now,
        DeviceId = deviceFingerprint,
        IpAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString(),
        Browser = _httpContextAccessor.HttpContext?.Request.Headers["User-Agent"].ToString(),
        Os = _httpContextAccessor.HttpContext?.Request.Headers["User-Agent"].ToString()
      };
      await _userRepository.CreateAsync(user);
    }
    else
    {
      user.ConnectionId = connectionId;
      user.UserName = userName;
      user.Avatar = avatar;
      user.LastActiveTime = DateTime.Now;
      user.IsOnline = 1;
      user.IpAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
      user.Browser = _httpContextAccessor.HttpContext?.Request.Headers["User-Agent"].ToString();
      user.Os = _httpContextAccessor.HttpContext?.Request.Headers["User-Agent"].ToString();
      await _userRepository.UpdateAsync(user);
    }

    _logger.LogInformation($"更新用户信息成功，ConnectionId: {connectionId}, UserId: {userId}, DeviceId: {deviceFingerprint}");
  }

  /// <summary>
  /// 强制用户登出
  /// </summary>
  /// <param name="userId">用户ID</param>
  public async Task ForceLogoutAsync(long userId)
  {
    var user = await _userRepository.FirstOrDefaultAsync(u => u.UserId == userId);
    if (user != null)
    {
      user.IsOnline = 0;
      user.LastActiveTime = DateTime.Now;
      user.UpdateTime = DateTime.Now;
      await _userRepository.UpdateAsync(user);
    }
  }

  /// <summary>
  /// 清理离线用户
  /// </summary>
  /// <param name="minutes">超时时间（分钟）</param>
  public async Task CleanOfflineUsersAsync(int minutes = 30)
  {
    var cutoffTime = DateTime.Now.AddMinutes(-minutes);
    await _userRepository.DeleteAsync(u => u.IsOnline == 0 && u.LastActiveTime < cutoffTime);
  }

  /// <summary>
  /// 分页查询在线用户
  /// </summary>
  public async Task<LeanPageResult<LeanOnlineUserDto>> GetPageListAsync(LeanOnlineUserQueryDto input)
  {
    Expression<Func<LeanOnlineUser, bool>> predicate = u => true;

    if (input.UserId.HasValue)
    {
      predicate = predicate.And(u => u.UserId == input.UserId.Value);
    }

    if (!string.IsNullOrEmpty(input.UserName))
    {
      predicate = predicate.And(u => u.UserName.Contains(input.UserName));
    }

    if (input.IsOnline.HasValue)
    {
      predicate = predicate.And(u => u.IsOnline == input.IsOnline.Value);
    }

    if (input.StartTime.HasValue)
    {
      predicate = predicate.And(u => u.LastActiveTime >= input.StartTime.Value);
    }

    if (input.EndTime.HasValue)
    {
      predicate = predicate.And(u => u.LastActiveTime <= input.EndTime.Value);
    }

    var result = await _userRepository.GetPageListAsync(
        predicate,
        input.PageSize,
        input.PageIndex,
        u => u.LastActiveTime,
        false);

    return new LeanPageResult<LeanOnlineUserDto>
    {
      Total = result.Total,
      Items = result.Items.Select(u => new LeanOnlineUserDto
      {
        UserId = u.UserId,
        UserName = u.UserName ?? string.Empty,
        Avatar = u.Avatar,
        ConnectionId = u.ConnectionId,
        DeviceId = u.DeviceId,
        IsOnline = u.IsOnline,
        LastActiveTime = u.LastActiveTime,
        CreateTime = u.CreateTime,
        UpdateTime = u.UpdateTime
      }).ToList()
    };
  }

  /// <summary>
  /// 获取用户连接ID
  /// </summary>
  public async Task<string?> GetUserConnectionIdAsync(long userId)
  {
    var user = await _userRepository.FirstOrDefaultAsync(x => x.UserId == userId && x.IsOnline == 1);
    return user?.ConnectionId;
  }

  /// <summary>
  /// 更新最后活动时间
  /// </summary>
  /// <param name="connectionId">连接ID</param>
  /// <param name="userId">用户ID</param>
  /// <param name="deviceId">设备ID</param>
  public async Task UpdateLastActiveTimeAsync(string connectionId, long userId, string deviceId)
  {
    var user = await _userRepository.FirstOrDefaultAsync(x =>
        x.UserId == userId &&
        x.DeviceId == deviceId);

    if (user != null)
    {
      user.ConnectionId = connectionId;
      user.LastActiveTime = DateTime.Now;
      await _userRepository.UpdateAsync(user);
    }
  }
}