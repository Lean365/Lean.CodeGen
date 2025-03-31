using System.Threading.Tasks;
using System.Collections.Generic;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Application.Dtos.Signalr;
using Lean.CodeGen.Domain.Entities.Signalr;

namespace Lean.CodeGen.Application.Services.Signalr;

/// <summary>
/// 在线用户服务接口
/// </summary>
public interface ILeanOnlineUserService
{
  /// <summary>
  /// 获取在线用户列表
  /// </summary>
  Task<List<LeanOnlineUser>> GetOnlineUsersAsync();

  /// <summary>
  /// 分页查询在线用户
  /// </summary>
  Task<LeanPageResult<LeanOnlineUserDto>> GetPageListAsync(LeanOnlineUserQueryDto input);

  /// <summary>
  /// 获取用户在线状态
  /// </summary>
  /// <param name="userId">用户ID</param>
  /// <param name="deviceId">设备指纹</param>
  /// <returns>用户是否在线</returns>
  Task<bool> IsUserOnlineAsync(long userId, string deviceId);

  /// <summary>
  /// 获取用户连接ID
  /// </summary>
  Task<string?> GetUserConnectionIdAsync(long userId);

  /// <summary>
  /// 更新用户在线状态
  /// </summary>
  /// <param name="connectionId">连接ID</param>
  /// <param name="isOnline">是否在线</param>
  /// <param name="userId">用户ID</param>
  /// <param name="deviceId">设备ID</param>
  Task UpdateUserStatusAsync(string connectionId, bool isOnline, long userId, string deviceId);

  /// <summary>
  /// 更新用户信息
  /// </summary>
  /// <param name="connectionId">连接ID</param>
  /// <param name="userId">用户ID</param>
  /// <param name="userName">用户名</param>
  /// <param name="avatar">头像</param>
  /// <param name="deviceFingerprint">设备指纹</param>
  Task UpdateUserInfoAsync(string connectionId, long userId, string? userName, string? avatar, string deviceFingerprint);

  /// <summary>
  /// 更新用户最后活动时间
  /// </summary>
  /// <param name="connectionId">连接ID</param>
  /// <param name="userId">用户ID</param>
  /// <param name="deviceId">设备ID</param>
  Task UpdateLastActiveTimeAsync(string connectionId, long userId, string deviceId);

  /// <summary>
  /// 清理离线用户
  /// </summary>
  Task CleanOfflineUsersAsync(int timeoutMinutes = 30);

  /// <summary>
  /// 强制退出用户
  /// </summary>
  Task ForceLogoutAsync(long userId);
}