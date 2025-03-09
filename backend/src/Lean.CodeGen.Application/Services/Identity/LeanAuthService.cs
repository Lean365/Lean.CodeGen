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
  /// 默认验证码宽度
  /// </summary>
  private const int DefaultWidth = 280;

  /// <summary>
  /// 默认验证码高度
  /// </summary>
  private const int DefaultHeight = 155;

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="cache">内存缓存服务</param>
  /// <param name="sliderCaptchaHelper">滑块验证码帮助类</param>
  /// <param name="options">滑块验证码配置</param>
  /// <param name="deviceExtendRepository">设备扩展仓储</param>
  /// <param name="loginExtendRepository">登录扩展仓储</param>
  /// <param name="userRepository">用户仓储</param>
  /// <param name="tokenService">Token服务</param>
  /// <param name="httpContextAccessor">HTTP上下文访问器</param>
  /// <param name="serverInfoHelper">服务器系统信息帮助类</param>
  /// <param name="clientInfoHelper">客户端系统信息帮助类</param>
  /// <param name="ipHelper">IP 帮助类</param>
  public LeanAuthService(
      IMemoryCache cache,
      LeanSliderCaptchaHelper sliderCaptchaHelper,
      IOptions<LeanSliderCaptchaOptions> options,
      ILeanRepository<LeanDeviceExtend> deviceExtendRepository,
      ILeanRepository<LeanLoginExtend> loginExtendRepository,
      ILeanRepository<LeanUser> userRepository,
      ITokenService tokenService,
      IHttpContextAccessor httpContextAccessor,
      LeanServerInfoHelper serverInfoHelper,
      LeanClientInfoHelper clientInfoHelper,
      LeanIpHelper ipHelper)
  {
    _cache = cache;
    _sliderCaptchaHelper = sliderCaptchaHelper;
    _options = options.Value;
    _deviceExtendRepository = deviceExtendRepository;
    _loginExtendRepository = loginExtendRepository;
    _userRepository = userRepository;
    _tokenService = tokenService;
    _httpContextAccessor = httpContextAccessor;
    _serverInfoHelper = serverInfoHelper;
    _clientInfoHelper = clientInfoHelper;
    _ipHelper = ipHelper;
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
      return LeanApiResult<LeanLoginResponseDto>.Error(captchaResult.Message);
    }

    // 验证用户名和密码
    var user = await _userRepository.FirstOrDefaultAsync(u => u.UserName == request.UserName);
    if (user == null)
    {
      return LeanApiResult<LeanLoginResponseDto>.Error("用户名或密码错误");
    }

    // 验证密码
    if (!LeanPassword.VerifyPassword(request.Password, user.Password, user.Salt))
    {
      // 更新密码错误次数
      var loginExtend = await _loginExtendRepository.FirstOrDefaultAsync(l => l.UserId == user.Id);
      if (loginExtend != null)
      {
        loginExtend.PasswordErrorCount++;
        loginExtend.LastPasswordErrorTime = DateTime.Now;
        await _loginExtendRepository.UpdateAsync(loginExtend);
      }
      return LeanApiResult<LeanLoginResponseDto>.Error("用户名或密码错误");
    }

    // 获取系统信息
    var serverInfo = _serverInfoHelper.GetSystemInfo();
    var clientInfo = _clientInfoHelper.GetSystemInfo();

    // 生成设备ID
    var deviceId = Guid.NewGuid().ToString("N");

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
    var loginExtendInfo = await _loginExtendRepository.FirstOrDefaultAsync(l => l.UserId == user.Id);
    if (loginExtendInfo == null)
    {
      var loginLocation = await _ipHelper.GetIpLocationAsync(clientInfo.IpAddress);
      loginExtendInfo = new LeanLoginExtend
      {
        UserId = user.Id,
        PasswordErrorCount = 0,
        FirstLoginIp = clientInfo.IpAddress,
        FirstLoginLocation = loginLocation,
        FirstLoginTime = DateTime.Now,
        FirstDeviceId = deviceId,
        FirstBrowser = clientInfo.BrowserInfo.Browser,
        FirstOs = clientInfo.BrowserInfo.Platform,
        FirstLoginType = 0, // 0: Password
        LastLoginIp = clientInfo.IpAddress,
        LastLoginLocation = loginLocation,
        LastLoginTime = DateTime.Now,
        LastDeviceId = deviceId,
        LastBrowser = clientInfo.BrowserInfo.Browser,
        LastOs = clientInfo.BrowserInfo.Platform,
        LastLoginType = 0, // 0: Password
        LoginStatus = 0, // 0: Normal
        SystemInfo = JsonConvert.SerializeObject(new { Server = serverInfo, Client = clientInfo })
      };
      await _loginExtendRepository.CreateAsync(loginExtendInfo);
    }
    else
    {
      var loginLocation = await _ipHelper.GetIpLocationAsync(clientInfo.IpAddress);
      loginExtendInfo.PasswordErrorCount = 0;
      loginExtendInfo.LastLoginIp = clientInfo.IpAddress;
      loginExtendInfo.LastLoginLocation = loginLocation;
      loginExtendInfo.LastLoginTime = DateTime.Now;
      loginExtendInfo.LastDeviceId = deviceId;
      loginExtendInfo.LastBrowser = clientInfo.BrowserInfo.Browser;
      loginExtendInfo.LastOs = clientInfo.BrowserInfo.Platform;
      loginExtendInfo.LoginStatus = 0; // 0: Normal
      loginExtendInfo.SystemInfo = JsonConvert.SerializeObject(new { Server = serverInfo, Client = clientInfo });
      loginExtendInfo.LastLoginType = 0; // 0: Password
      await _loginExtendRepository.UpdateAsync(loginExtendInfo);
    }

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
    });
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
      Width = DefaultWidth,
      Height = DefaultHeight
    };
    return await Task.FromResult(LeanApiResult<LeanSliderCaptchaResponseDto>.Ok(response));
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
      return await Task.FromResult(LeanApiResult.Error("验证码已过期"));
    }

    // 验证滑块位置
    var isValid = _sliderCaptchaHelper.Validate(request.X, request.Y, position.x, position.y);
    _cache.Remove(cacheKey);

    // 返回验证结果
    return await Task.FromResult(isValid
      ? LeanApiResult.Ok()
      : LeanApiResult.Error("验证失败"));
  }

  /// <summary>
  /// 用户登出
  /// </summary>
  /// <returns>登出结果</returns>
  public Task<LeanApiResult> LogoutAsync()
  {
    throw new NotImplementedException();
  }
}