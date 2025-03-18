using Lean.CodeGen.Application.Dtos.Identity;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Common.Helpers;
using Lean.CodeGen.Common.Options;
using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Domain.Entities.Identity;
using Lean.CodeGen.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Mapster;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Lean.CodeGen.Common.Security;
using System.Collections.Generic;
using Newtonsoft.Json;
using Lean.CodeGen.Application.Dtos.Captcha;
using Lean.CodeGen.Application.Services.Audit;

namespace Lean.CodeGen.Application.Services.Identity;

/// <summary>
/// 认证服务实现类
/// </summary>
public class LeanAuthService : ILeanAuthService
{
  /// <summary>
  /// 内存缓存服务
  /// </summary>
  private readonly IMemoryCache _cache;

  /// <summary>
  /// 滑块验证码帮助类
  /// </summary>
  private readonly LeanSliderCaptchaHelper _sliderCaptchaHelper;

  /// <summary>
  /// 滑块验证码配置
  /// </summary>
  private readonly LeanSliderCaptchaOptions _options;

  /// <summary>
  /// 设备扩展仓储
  /// </summary>
  private readonly ILeanRepository<LeanDeviceExtend> _deviceExtendRepository;

  /// <summary>
  /// 登录扩展仓储
  /// </summary>
  private readonly ILeanRepository<LeanLoginExtend> _loginExtendRepository;

  /// <summary>
  /// 用户仓储
  /// </summary>
  private readonly ILeanRepository<LeanUser> _userRepository;

  /// <summary>
  /// Token服务
  /// </summary>
  private readonly ITokenService _tokenService;

  /// <summary>
  /// HTTP上下文访问器
  /// </summary>
  private readonly IHttpContextAccessor _httpContextAccessor;

  /// <summary>
  /// 服务器系统信息帮助类
  /// </summary>
  private readonly LeanServerInfoHelper _serverInfoHelper;
  /// <summary>
  /// 客户端系统信息帮助类
  /// </summary>
  private readonly LeanClientInfoHelper _clientInfoHelper;

  /// <summary>
  /// IP 帮助类
  /// </summary>
  private readonly LeanIpHelper _ipHelper;

  /// <summary>
  /// 登录日志服务
  /// </summary>
  private readonly ILeanLoginLogService _loginLogService;

  /// <summary>
  /// 安全设置
  /// </summary>
  private readonly LeanSecurityOptions _securitySettings;

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="cache">内存缓存服务</param>
  /// <param name="sliderCaptchaHelper">滑块验证码帮助类</param>
  /// <param name="options">滑块验证码配置</param>
  /// <param name="securitySettings">安全设置</param>
  /// <param name="deviceExtendRepository">设备扩展仓储</param>
  /// <param name="loginExtendRepository">登录扩展仓储</param>
  /// <param name="userRepository">用户仓储</param>
  /// <param name="tokenService">Token服务</param>
  /// <param name="httpContextAccessor">HTTP上下文访问器</param>
  /// <param name="serverInfoHelper">服务器系统信息帮助类</param>
  /// <param name="clientInfoHelper">客户端系统信息帮助类</param>
  /// <param name="ipHelper">IP 帮助类</param>
  /// <param name="loginLogService">登录日志服务</param>
  public LeanAuthService(
      IMemoryCache cache,
      LeanSliderCaptchaHelper sliderCaptchaHelper,
      IOptions<LeanSliderCaptchaOptions> options,
      IOptions<LeanSecurityOptions> securitySettings,
      ILeanRepository<LeanDeviceExtend> deviceExtendRepository,
      ILeanRepository<LeanLoginExtend> loginExtendRepository,
      ILeanRepository<LeanUser> userRepository,
      ITokenService tokenService,
      IHttpContextAccessor httpContextAccessor,
      LeanServerInfoHelper serverInfoHelper,
      LeanClientInfoHelper clientInfoHelper,
      LeanIpHelper ipHelper,
      ILeanLoginLogService loginLogService)
  {
    _cache = cache;
    _sliderCaptchaHelper = sliderCaptchaHelper;
    _options = options.Value;
    _securitySettings = securitySettings.Value;
    _deviceExtendRepository = deviceExtendRepository;
    _loginExtendRepository = loginExtendRepository;
    _userRepository = userRepository;
    _tokenService = tokenService;
    _httpContextAccessor = httpContextAccessor;
    _serverInfoHelper = serverInfoHelper;
    _clientInfoHelper = clientInfoHelper;
    _ipHelper = ipHelper;
    _loginLogService = loginLogService;
  }

  /// <summary>
  /// 检查账户锁定状态
  /// </summary>
  private (bool isLocked, string message) CheckAccountLockStatus(LeanLoginExtend loginExtend)
  {
    // 检查永久锁定
    if (loginExtend.LockStatus == 2) // 永久锁定
    {
      return (true, "账户已被永久锁定，请联系管理员");
    }

    // 检查临时锁定
    if (loginExtend.LockStatus == 1) // 临时锁定
    {
      var unlockTime = loginExtend.UnlockTime;
      if (unlockTime.HasValue && unlockTime.Value > DateTime.Now)
      {
        var remainingMinutes = (int)(unlockTime.Value - DateTime.Now).TotalMinutes;
        return (true, $"账户已被锁定，请{remainingMinutes}分钟后重试");
      }

      // 如果锁定时间已过，自动解锁
      loginExtend.LockStatus = 0; // 正常状态
      loginExtend.UnlockTime = null;
      loginExtend.PasswordErrorCount = 0;
    }

    return (false, string.Empty);
  }

  /// <summary>
  /// 更新账户锁定状态
  /// </summary>
  private void UpdateAccountLockStatus(LeanLoginExtend loginExtend)
  {
    loginExtend.PasswordErrorCount++;
    loginExtend.LastPasswordErrorTime = DateTime.Now;

    // 检查是否需要永久锁定
    if (_securitySettings.Account.EnablePermanentLock &&
        loginExtend.PasswordErrorCount >= _securitySettings.Account.PermanentLockThreshold)
    {
      loginExtend.LockStatus = 2; // 永久锁定
      loginExtend.LockTime = DateTime.Now;
      loginExtend.LockReason = "密码错误次数过多，账户已被永久锁定";
      return;
    }

    // 检查是否需要临时锁定
    if (loginExtend.PasswordErrorCount >= _securitySettings.Account.MaxPasswordErrorCount)
    {
      loginExtend.LockStatus = 1; // 临时锁定
      loginExtend.LockTime = DateTime.Now;
      loginExtend.UnlockTime = DateTime.Now.AddMinutes(_securitySettings.Account.LockDuration);
      loginExtend.LockReason = $"密码错误{loginExtend.PasswordErrorCount}次，账户已被临时锁定";
    }
  }

  /// <summary>
  /// 用户登录
  /// </summary>
  /// <param name="request">登录请求参数</param>
  /// <returns>登录结果</returns>
  public async Task<LeanApiResult<LeanLoginResponseDto>> LoginAsync(LeanLoginRequestDto request)
  {
    // 验证滑块验证码
    var captchaResult = await ValidateSliderCaptchaAsync(new LeanSliderCaptchaRequestDto
    {
      CaptchaKey = request.CaptchaKey,
      X = request.SliderX,
      Y = request.SliderY
    });

    if (!captchaResult.Success)
    {
      return LeanApiResult<LeanLoginResponseDto>.Error(captchaResult.Message, LeanErrorCode.Status400BadRequest, LeanBusinessType.Grant);
    }

    // 验证用户名和密码
    var user = await _userRepository.FirstOrDefaultAsync(u => u.UserName == request.UserName);
    if (user == null)
    {
      return LeanApiResult<LeanLoginResponseDto>.Error("用户名或密码错误", LeanErrorCode.Status400BadRequest, LeanBusinessType.Grant);
    }

    // 生成设备ID
    var deviceId = $"{request.UserName}_{DateTime.Now.Ticks}";

    // 获取登录扩展信息
    var loginExtend = await _loginExtendRepository.FirstOrDefaultAsync(l => l.UserId == user.Id);
    if (loginExtend == null)
    {
      loginExtend = new LeanLoginExtend
      {
        UserId = user.Id,
        PasswordErrorCount = 0,
        LockStatus = 0, // 正常状态
        FirstLoginIp = request.LoginIp ?? "未知",
        FirstLoginTime = DateTime.Now,
        FirstDeviceId = deviceId,
        FirstBrowser = request.Browser ?? "未知",
        FirstOs = request.Os ?? "未知",
        FirstLoginType = (int)LeanLoginType.Password,
        LastLoginIp = request.LoginIp ?? "未知",
        LastLoginTime = DateTime.Now,
        LastDeviceId = deviceId,
        LastBrowser = request.Browser ?? "未知",
        LastOs = request.Os ?? "未知",
        LastLoginType = (int)LeanLoginType.Password
      };
      await _loginExtendRepository.CreateAsync(loginExtend);
    }

    // 检查账户锁定状态
    var (isLocked, lockMessage) = CheckAccountLockStatus(loginExtend);
    if (isLocked)
    {
      await _loginExtendRepository.UpdateAsync(loginExtend);
      return LeanApiResult<LeanLoginResponseDto>.Error(lockMessage, LeanErrorCode.Status400BadRequest, LeanBusinessType.Grant);
    }

    // 验证密码
    if (!LeanPassword.VerifyPassword(request.Password, user.Password, user.Salt))
    {
      // 更新账户锁定状态
      UpdateAccountLockStatus(loginExtend);
      await _loginExtendRepository.UpdateAsync(loginExtend);

      // 如果启用了通知，并且错误次数达到阈值，可以在这里发送通知
      if (_securitySettings.Account.EnableNotification &&
          loginExtend.PasswordErrorCount >= _securitySettings.Account.NotificationThreshold)
      {
        // TODO: 发送通知
      }

      return LeanApiResult<LeanLoginResponseDto>.Error("用户名或密码错误", LeanErrorCode.Status400BadRequest, LeanBusinessType.Grant);
    }

    // 登录成功，重置错误次数和锁定状态
    loginExtend.PasswordErrorCount = 0;
    loginExtend.LockStatus = 0; // 正常状态
    loginExtend.UnlockTime = loginExtend.LockStatus != 0 ? DateTime.Now : null;

    // 删除验证码缓存
    _cache.Remove($"slider_captcha:{request.CaptchaKey}");

    // 获取系统信息
    var serverInfo = _serverInfoHelper.GetSystemInfo();
    var clientInfo = _clientInfoHelper.GetSystemInfo();

    // 更新设备信息
    var device = await _deviceExtendRepository.FirstOrDefaultAsync(d => d.DeviceId == deviceId);
    if (device == null)
    {
      device = new LeanDeviceExtend
      {
        DeviceId = deviceId,
        DeviceName = clientInfo.BrowserInfo.Browser,
        DeviceType = clientInfo.BrowserInfo.IsMobile ? 0 : 1, // 0: Mobile, 1: Desktop
        IsTrusted = 0, // 0: 不信任, 1: 信任
        DeviceStatus = 0, // 0: Normal
        LastLoginIp = clientInfo.IpAddress,
        Browser = clientInfo.BrowserInfo.Browser,
        Os = clientInfo.BrowserInfo.Platform
      };
      await _deviceExtendRepository.CreateAsync(device);
    }
    else
    {
      device.LastLoginIp = clientInfo.IpAddress;
      device.Browser = clientInfo.BrowserInfo.Browser;
      device.Os = clientInfo.BrowserInfo.Platform;
      await _deviceExtendRepository.UpdateAsync(device);
    }

    // 更新登录扩展信息
    var loginLocation = await _ipHelper.GetIpLocationAsync(clientInfo.IpAddress);
    if (loginExtend.FirstLoginTime == null)
    {
      loginExtend.FirstLoginIp = clientInfo.IpAddress;
      loginExtend.FirstLoginLocation = loginLocation;
      loginExtend.FirstLoginTime = DateTime.Now;
      loginExtend.FirstDeviceId = deviceId;
      loginExtend.FirstBrowser = clientInfo.BrowserInfo?.Browser ?? "未知";
      loginExtend.FirstOs = clientInfo.BrowserInfo?.Platform ?? "未知";
      loginExtend.FirstLoginType = (int)LeanLoginType.Password;
    }

    loginExtend.LastLoginIp = clientInfo.IpAddress;
    loginExtend.LastLoginLocation = loginLocation;
    loginExtend.LastLoginTime = DateTime.Now;
    loginExtend.LastDeviceId = deviceId;
    loginExtend.LastBrowser = clientInfo.BrowserInfo?.Browser ?? "未知";
    loginExtend.LastOs = clientInfo.BrowserInfo?.Platform ?? "未知";
    loginExtend.LoginStatus = 0;
    loginExtend.SystemInfo = JsonConvert.SerializeObject(new { Server = serverInfo, Client = clientInfo });
    loginExtend.LastLoginType = (int)LeanLoginType.Password;

    await _loginExtendRepository.UpdateAsync(loginExtend);

    // 生成访问令牌
    var claims = new[]
    {
      new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
      new Claim(ClaimTypes.Name, user.UserName),
      new Claim(ClaimTypes.GivenName, user.RealName ?? user.UserName)
    };
    var token = _tokenService.CreateToken(claims);

    // 返回登录结果
    return LeanApiResult<LeanLoginResponseDto>.Ok(new LeanLoginResponseDto
    {
      AccessToken = token,
      ExpiresIn = _options.ExpirySeconds,
      UserInfo = new LeanUserInfoDto
      {
        UserId = user.Id,
        UserName = user.UserName,
        NickName = user.Nickname ?? user.RealName,
        Avatar = user.Avatar,
        Roles = [], // TODO: 获取用户角色
        Permissions = [] // TODO: 获取用户权限
      }
    }, LeanBusinessType.Grant);
  }

  /// <summary>
  /// 获取浏览器信息
  /// </summary>
  private string GetBrowserInfo(string userAgent)
  {
    if (string.IsNullOrEmpty(userAgent)) return "Unknown";

    if (userAgent.Contains("Chrome"))
      return "Chrome";
    if (userAgent.Contains("Firefox"))
      return "Firefox";
    if (userAgent.Contains("Safari"))
      return "Safari";
    if (userAgent.Contains("Edge"))
      return "Edge";
    if (userAgent.Contains("MSIE") || userAgent.Contains("Trident"))
      return "IE";

    return "Other";
  }

  /// <summary>
  /// 获取操作系统信息
  /// </summary>
  private string GetOsInfo(string userAgent)
  {
    if (string.IsNullOrEmpty(userAgent)) return "Unknown";

    if (userAgent.Contains("Windows"))
      return "Windows";
    if (userAgent.Contains("Mac"))
      return "MacOS";
    if (userAgent.Contains("Linux"))
      return "Linux";
    if (userAgent.Contains("Android"))
      return "Android";
    if (userAgent.Contains("iPhone") || userAgent.Contains("iPad"))
      return "iOS";

    return "Other";
  }

  /// <summary>
  /// 获取滑块验证码
  /// </summary>
  /// <returns>滑块验证码信息，包含背景图、滑块图和位置信息</returns>
  public async Task<LeanApiResult<LeanSliderCaptchaResponseDto>> GetSliderCaptchaAsync()
  {
    // 生成滑块验证码
    var (bgImage, sliderImage, x, y) = _sliderCaptchaHelper.Generate();
    var key = _sliderCaptchaHelper.GenerateKey();

    // 缓存正确的滑块位置
    _cache.Set($"slider_captcha:{key}", (x, y), TimeSpan.FromSeconds(_options.ExpirySeconds));

    // 返回验证码信息
    var response = new LeanSliderCaptchaResponseDto
    {
      CaptchaKey = key,
      BackgroundImage = Convert.ToBase64String(bgImage),
      SliderImage = Convert.ToBase64String(sliderImage),
      Y = y,
      Width = _options.Width,
      Height = _options.Height
    };
    return await Task.FromResult(LeanApiResult<LeanSliderCaptchaResponseDto>.Ok(response, LeanBusinessType.Grant));
  }

  /// <summary>
  /// 验证滑块验证码
  /// </summary>
  /// <param name="request">验证请求参数</param>
  /// <returns>验证结果</returns>
  public async Task<LeanApiResult> ValidateSliderCaptchaAsync(LeanSliderCaptchaRequestDto request)
  {
    // 获取缓存的正确位置
    var cacheKey = $"slider_captcha:{request.CaptchaKey}";
    if (!_cache.TryGetValue<(int x, int y)>(cacheKey, out var position))
    {
      return await Task.FromResult(LeanApiResult.Error("验证码已过期", LeanErrorCode.Status400BadRequest, LeanBusinessType.Grant));
    }

    // 验证滑块位置
    var (isValid, message) = _sliderCaptchaHelper.Validate(request.X, request.Y, position.x, position.y);

    // 返回验证结果
    return await Task.FromResult(isValid
      ? LeanApiResult.Ok(LeanBusinessType.Grant)
      : LeanApiResult.Error(message, LeanErrorCode.Status400BadRequest, LeanBusinessType.Grant));
  }

  /// <summary>
  /// 用户登出
  /// </summary>
  /// <returns>登出结果</returns>
  public async Task<LeanApiResult> LogoutAsync()
  {
    try
    {
      // 获取当前用户ID
      var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
      var userNameClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name);
      if (userIdClaim == null || !long.TryParse(userIdClaim.Value, out var userId) || userNameClaim == null)
      {
        return LeanApiResult.Error("未登录", LeanErrorCode.Status401Unauthorized, LeanBusinessType.Grant);
      }

      // 获取系统信息
      var serverInfo = _serverInfoHelper.GetSystemInfo();
      var clientInfo = _clientInfoHelper.GetSystemInfo();

      // 更新登录扩展信息
      var loginExtend = await _loginExtendRepository.FirstOrDefaultAsync(l => l.UserId == userId);
      if (loginExtend != null)
      {
        loginExtend.LastLogoutTime = DateTime.Now;
        loginExtend.LastLogoutIp = clientInfo.IpAddress;
        loginExtend.LastLogoutLocation = await _ipHelper.GetIpLocationAsync(clientInfo.IpAddress);
        loginExtend.SystemInfo = JsonConvert.SerializeObject(new { Server = serverInfo, Client = clientInfo });
        await _loginExtendRepository.UpdateAsync(loginExtend);
      }

      // 将当前令牌加入黑名单（可选：如果使用Redis可以实现令牌黑名单）
      var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
      if (!string.IsNullOrEmpty(token))
      {
        var cacheKey = $"token_blacklist:{token}";
        var claims = _tokenService.GetTokenClaims(token);
        var expirationTime = claims.FirstOrDefault(c => c.Type == "exp")?.Value;
        if (expirationTime != null && long.TryParse(expirationTime, out var exp))
        {
          var expiration = DateTimeOffset.FromUnixTimeSeconds(exp).DateTime - DateTime.UtcNow;
          if (expiration > TimeSpan.Zero)
          {
            _cache.Set(cacheKey, true, expiration);
          }
        }
      }

      // 记录登出日志
      await _loginLogService.AddLogoutLogAsync(
        userId,
        userNameClaim.Value,
        loginExtend?.LastDeviceId ?? "Unknown",
        clientInfo.IpAddress,
        await _ipHelper.GetIpLocationAsync(clientInfo.IpAddress),
        clientInfo.BrowserInfo.Browser,
        clientInfo.BrowserInfo.Platform
      );

      return LeanApiResult.Ok(LeanBusinessType.Grant);
    }
    catch (Exception ex)
    {
      return LeanApiResult.Error($"登出失败：{ex.Message}", LeanErrorCode.Status500InternalServerError, LeanBusinessType.Grant);
    }
  }
}