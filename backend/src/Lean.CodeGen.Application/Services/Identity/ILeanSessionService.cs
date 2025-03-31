using Lean.CodeGen.Domain.Entities.Identity;

namespace Lean.CodeGen.Application.Services.Identity;

/// <summary>
/// 会话服务接口
/// </summary>
public interface ILeanSessionService
{
  /// <summary>
  /// 创建会话
  /// </summary>
  Task<LeanLoginExtend> CreateSessionAsync(LeanUser user, string deviceId, string ip, string userAgent);

  /// <summary>
  /// 验证会话
  /// </summary>
  Task<bool> ValidateSessionAsync(long userId, string deviceId);

  /// <summary>
  /// 使会话失效
  /// </summary>
  Task InvalidateSessionAsync(long userId, string deviceId);

  /// <summary>
  /// 使用户所有会话失效
  /// </summary>
  Task InvalidateAllSessionsAsync(long userId);

  /// <summary>
  /// 获取用户当前会话数
  /// </summary>
  /// <param name="userId">用户ID</param>
  /// <param name="deviceId">设备ID</param>
  Task<int> GetUserSessionCountAsync(long userId, string deviceId);
}