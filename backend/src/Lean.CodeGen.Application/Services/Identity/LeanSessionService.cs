using Lean.CodeGen.Common.Options;
using Lean.CodeGen.Domain.Entities.Identity;
using Lean.CodeGen.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Options;
using Lean.CodeGen.Common.Helpers;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Lean.CodeGen.Domain.Interfaces.Hubs;
using Lean.CodeGen.Common.Exceptions;
using Lean.CodeGen.Application.Services.Routine;

namespace Lean.CodeGen.Application.Services.Identity;

/// <summary>
/// 会话服务实现
/// </summary>
public class LeanSessionService : ILeanSessionService
{
    private readonly ILeanRepository<LeanLoginExtend> _loginExtendRepository;
    private readonly ILeanNotificationService _notificationService;
    private readonly LeanClientInfoHelper _clientInfoHelper;
    private readonly LeanIpHelper _ipHelper;
    private readonly LeanSecurityOptions _securityOptions;
    private readonly ILeanSignalRHub _signalRHub;

    public LeanSessionService(
        ILeanRepository<LeanLoginExtend> loginExtendRepository,
        ILeanNotificationService notificationService,
        LeanClientInfoHelper clientInfoHelper,
        LeanIpHelper ipHelper,
        IOptions<LeanSecurityOptions> securityOptions,
        ILeanSignalRHub signalRHub)
    {
        _loginExtendRepository = loginExtendRepository;
        _notificationService = notificationService;
        _clientInfoHelper = clientInfoHelper;
        _ipHelper = ipHelper;
        _securityOptions = securityOptions.Value;
        _signalRHub = signalRHub;

        // 清理过期会话
        CleanupExpiredSessionsAsync().ConfigureAwait(false);
    }

    private async Task CleanupExpiredSessionsAsync()
    {
        try
        {
            // 清理超过24小时的会话
            var expiredTime = DateTime.Now.AddHours(-24);
            var expiredSessions = await _loginExtendRepository.GetListAsync(x =>
                x.LastLoginTime < expiredTime &&
                x.LoginStatus == 0);

            foreach (var session in expiredSessions)
            {
                session.LoginStatus = 1; // 设置为过期状态
            }

            if (expiredSessions.Any())
            {
                await _loginExtendRepository.UpdateRangeAsync(expiredSessions);
            }
        }
        catch (Exception ex)
        {
            // 记录错误但不影响服务启动
            Console.WriteLine($"清理过期会话失败：{ex.Message}");
        }
    }

    public async Task<LeanLoginExtend> CreateSessionAsync(LeanUser user, string deviceId, string ip, string userAgent)
    {
        try
        {
            // 获取客户端信息
            var clientInfo = _clientInfoHelper.GetSystemInfo();

            // 验证设备指纹
            var deviceFingerprint = await ValidateDeviceFingerprintAsync(deviceId, clientInfo);
            if (!deviceFingerprint.Success)
            {
                throw new LeanException("设备验证失败", LeanErrorCode.Status400BadRequest);
            }

            // 检查是否存在相同设备的活跃会话
            var existingSession = await _loginExtendRepository.FirstOrDefaultAsync(x =>
              x.UserId == user.Id &&
              x.LastDeviceId == deviceId &&
              x.LoginStatus == 0);

            if (existingSession != null)
            {
                // 更新现有会话
                existingSession.LastLoginTime = DateTime.Now;
                existingSession.LastLoginIp = ip;
                existingSession.LastLoginLocation = await _ipHelper.GetIpLocationAsync(ip);
                existingSession.LastBrowser = clientInfo.BrowserInfo.Browser;
                existingSession.LastOs = clientInfo.BrowserInfo.Platform;
                existingSession.SystemInfo = JsonConvert.SerializeObject(clientInfo);
                await _loginExtendRepository.UpdateAsync(existingSession);
                return existingSession;
            }

            // 检查是否启用单点登录
            if (_securityOptions.Login.EnableSingleSignOn)
            {
                // 检查当前会话数
                var sessionCount = await GetUserSessionCountAsync(user.Id, deviceId);
                if (sessionCount >= _securityOptions.Login.MaxConcurrentSessions)
                {
                    // 获取其他设备的活跃会话
                    var otherSession = await _loginExtendRepository.FirstOrDefaultAsync(x =>
                      x.UserId == user.Id &&
                      x.LastDeviceId != deviceId &&
                      x.LoginStatus == 0);

                    if (otherSession != null)
                    {
                        var location = await _ipHelper.GetIpLocationAsync(otherSession.LastLoginIp);
                        var message = $"该账号已在 {location} 登录，请注意账号安全";

                        // 通知其他客户端有新登录请求
                        await _notificationService.NotifyUserLoginAttemptAsync(
                          user.Id,
                          message,
                          DateTime.Now,
                          ip,
                          await _ipHelper.GetIpLocationAsync(ip)
                        );

                        // 如果启用了强制登出，则使旧会话失效
                        if (_securityOptions.Login.ForceLogoutOtherDevices)
                        {
                            otherSession.LoginStatus = 1; // 设置为失效状态
                            await _loginExtendRepository.UpdateAsync(otherSession);
                        }
                        else
                        {
                            throw new LeanException(message, LeanErrorCode.Status400BadRequest);
                        }
                    }
                }
            }

            // 创建新的会话
            var session = new LeanLoginExtend
            {
                UserId = user.Id,
                LastDeviceId = deviceId,
                LastLoginTime = DateTime.Now,
                LastLoginIp = ip,
                LastLoginLocation = await _ipHelper.GetIpLocationAsync(ip),
                LastBrowser = clientInfo.BrowserInfo.Browser,
                LastOs = clientInfo.BrowserInfo.Platform,
                LoginStatus = 0,
                SystemInfo = JsonConvert.SerializeObject(clientInfo)
            };

            await _loginExtendRepository.CreateAsync(session);
            return session;
        }
        catch (LeanException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new LeanException($"创建会话失败：{ex.Message}", LeanErrorCode.Status500InternalServerError);
        }
    }

    private async Task<LeanApiResult> ValidateDeviceFingerprintAsync(string deviceId, ClientSystemInfo clientInfo)
    {
        try
        {
            // 验证设备ID是否为空
            if (string.IsNullOrEmpty(deviceId))
            {
                return LeanApiResult.Error("设备ID不能为空", LeanErrorCode.Status400BadRequest);
            }

            // 生成后端设备ID
            var backendDeviceId = LeanDeviceHelper.GenerateDeviceId(
              deviceId,
              clientInfo.BrowserInfo.UserAgent,
              clientInfo.BrowserInfo.Platform
            );

            // 验证生成的设备ID是否为Base64格式
            if (!IsValidBase64String(backendDeviceId))
            {
                return LeanApiResult.Error("设备ID格式不正确", LeanErrorCode.Status400BadRequest);
            }

            return LeanApiResult.Ok();
        }
        catch (Exception ex)
        {
            return LeanApiResult.Error($"设备验证失败：{ex.Message}", LeanErrorCode.Status500InternalServerError);
        }
    }

    /// <summary>
    /// 验证字符串是否为有效的Base64格式
    /// </summary>
    private bool IsValidBase64String(string input)
    {
        if (string.IsNullOrEmpty(input)) return false;
        try
        {
            Convert.FromBase64String(input);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> ValidateSessionAsync(long userId, string deviceId)
    {
        var sessions = await _loginExtendRepository.GetListAsync(x =>
          x.UserId == userId &&
          x.LastDeviceId == deviceId &&
          x.LoginStatus == 0);
        return sessions.Any();
    }

    public async Task InvalidateSessionAsync(long userId, string deviceId)
    {
        var sessions = await _loginExtendRepository.GetListAsync(x =>
          x.UserId == userId &&
          x.LastDeviceId == deviceId);
        if (sessions.Any())
        {
            var session = sessions.First();
            session.LoginStatus = 1;
            await _loginExtendRepository.UpdateAsync(session);
        }
    }

    public async Task InvalidateAllSessionsAsync(long userId)
    {
        var sessions = await _loginExtendRepository.GetListAsync(x =>
          x.UserId == userId);
        foreach (var session in sessions)
        {
            session.LoginStatus = 1;
        }
        await _loginExtendRepository.UpdateRangeAsync(sessions);
    }

    public async Task<int> GetUserSessionCountAsync(long userId, string deviceId)
    {
        var count = await _loginExtendRepository.CountAsync(x =>
          x.UserId == userId &&
          x.LoginStatus == 0 &&
          x.LastDeviceId == deviceId);
        return (int)count;
    }
}