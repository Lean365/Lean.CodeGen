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
  private readonly ILogger _logger;

  /// <summary>
  /// 构造函数
  /// </summary>
  public LeanOnlineUserService(
      ILeanRepository<LeanOnlineUser> userRepository,
      LeanBaseServiceContext context)
      : base(context)
  {
    _userRepository = userRepository;
    _logger = context.Logger;
  }

  /// <summary>
  /// 获取在线用户列表
  /// </summary>
  public async Task<List<LeanOnlineUserDto>> GetOnlineUsersAsync()
  {
    var users = await _userRepository.GetListAsync(u => u.IsOnline == 1);
    return users.Adapt<List<LeanOnlineUserDto>>();
  }

  /// <summary>
  /// 检查用户是否在线
  /// </summary>
  /// <param name="userId">用户ID</param>
  /// <returns>用户是否在线</returns>
  public async Task<bool> IsUserOnlineAsync(string userId)
  {
    var user = await _userRepository.FirstOrDefaultAsync(u => u.UserId == userId);
    return user?.IsOnline == 1;
  }

  /// <summary>
  /// 更新用户信息
  /// </summary>
  /// <param name="connectionId">连接ID</param>
  /// <param name="userId">用户ID</param>
  /// <param name="userName">用户名</param>
  /// <param name="avatar">头像</param>
  public async Task UpdateUserInfoAsync(string connectionId, string userId, string userName, string? avatar)
  {
    var user = await _userRepository.FirstOrDefaultAsync(u => u.ConnectionId == connectionId);
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
        CreateTime = DateTime.Now,
        UpdateTime = DateTime.Now
      };
      await _userRepository.CreateAsync(user);
    }
    else
    {
      user.UserId = userId;
      user.UserName = userName;
      user.Avatar = avatar;
      user.UpdateTime = DateTime.Now;
      await _userRepository.UpdateAsync(user);
    }
  }

  /// <summary>
  /// 强制用户登出
  /// </summary>
  /// <param name="userId">用户ID</param>
  public async Task ForceLogoutAsync(string userId)
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
    Expression<Func<LeanOnlineUser, bool>> predicate = BuildQueryPredicate(input);
    var result = await _userRepository.GetPageListAsync(
        predicate,
        input.PageSize,
        input.PageIndex,
        u => u.CreateTime,
        false);

    return new LeanPageResult<LeanOnlineUserDto>
    {
      Total = result.Total,
      Items = result.Items.Adapt<List<LeanOnlineUserDto>>()
    };
  }

  private Expression<Func<LeanOnlineUser, bool>> BuildQueryPredicate(LeanOnlineUserQueryDto input)
  {
    Expression<Func<LeanOnlineUser, bool>> predicate = u => true;

    if (!string.IsNullOrEmpty(input.UserId))
    {
      predicate = predicate.And(u => u.UserId == input.UserId);
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
      predicate = predicate.And(u => u.CreateTime >= input.StartTime.Value);
    }

    if (input.EndTime.HasValue)
    {
      predicate = predicate.And(u => u.CreateTime <= input.EndTime.Value);
    }

    return predicate;
  }

  public async Task<string?> GetUserConnectionIdAsync(string userId)
  {
    var user = await _userRepository.FirstOrDefaultAsync(u => u.UserId == userId && u.IsOnline == 1);
    return user?.ConnectionId;
  }

  public async Task UpdateUserStatusAsync(string connectionId, bool isOnline)
  {
    var user = await _userRepository.FirstOrDefaultAsync(u => u.ConnectionId == connectionId);
    if (user != null)
    {
      user.IsOnline = isOnline ? 1 : 0;
      user.UpdateTime = DateTime.Now;
      if (!isOnline)
      {
        user.LastActiveTime = DateTime.Now;
      }
      await _userRepository.UpdateAsync(user);
    }
  }

  public async Task UpdateLastActiveTimeAsync(string connectionId)
  {
    var user = await _userRepository.FirstOrDefaultAsync(u => u.ConnectionId == connectionId);
    if (user != null)
    {
      user.LastActiveTime = DateTime.Now;
      user.UpdateTime = DateTime.Now;
      await _userRepository.UpdateAsync(user);
    }
  }
}