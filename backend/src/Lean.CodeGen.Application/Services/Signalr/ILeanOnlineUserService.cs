using System.Threading.Tasks;
using System.Collections.Generic;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Application.Dtos.Signalr;

namespace Lean.CodeGen.Application.Services.Signalr;

/// <summary>
/// 在线用户服务接口
/// </summary>
public interface ILeanOnlineUserService
{
    /// <summary>
    /// 获取在线用户列表
    /// </summary>
    Task<List<LeanOnlineUserDto>> GetOnlineUsersAsync();

    /// <summary>
    /// 分页查询在线用户
    /// </summary>
    Task<LeanPageResult<LeanOnlineUserDto>> GetPageListAsync(LeanOnlineUserQueryDto input);

    /// <summary>
    /// 获取用户在线状态
    /// </summary>
    Task<bool> IsUserOnlineAsync(string userId);

    /// <summary>
    /// 获取用户连接ID
    /// </summary>
    Task<string?> GetUserConnectionIdAsync(string userId);

    /// <summary>
    /// 更新用户在线状态
    /// </summary>
    Task UpdateUserStatusAsync(string connectionId, bool isOnline);

    /// <summary>
    /// 更新用户信息
    /// </summary>
    Task UpdateUserInfoAsync(string connectionId, string userId, string userName, string? avatar);

    /// <summary>
    /// 更新用户最后活动时间
    /// </summary>
    Task UpdateLastActiveTimeAsync(string connectionId);

    /// <summary>
    /// 清理离线用户
    /// </summary>
    Task CleanOfflineUsersAsync(int timeoutMinutes = 30);

    /// <summary>
    /// 强制退出用户
    /// </summary>
    Task ForceLogoutAsync(string userId);
}