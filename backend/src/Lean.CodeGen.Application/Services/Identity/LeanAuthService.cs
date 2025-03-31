using Lean.CodeGen.Application.Dtos.Captcha;
using Lean.CodeGen.Application.Dtos.Identity;
using Lean.CodeGen.Application.Services.Audit;
using Lean.CodeGen.Application.Services.Signalr;
using Lean.CodeGen.Common.Helpers;
using Lean.CodeGen.Common.Http;
using Lean.CodeGen.Common.Options;
using Lean.CodeGen.Common.Security;
using Lean.CodeGen.Domain.Context;
using Lean.CodeGen.Domain.Entities.Identity;
using Lean.CodeGen.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Security.Claims;

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
  private readonly ILeanHttpContextAccessor _httpContextAccessor;

  /// <summary>
  /// 用户上下文
  /// </summary>
  private readonly ILeanUserContext _userContext;

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
  /// 菜单服务
  /// </summary>
  private readonly ILeanMenuService _menuService;

  private readonly ILeanSessionService _sessionService;
  private readonly JwtSettings _jwtSettings;

  private readonly ILeanOnlineUserService _onlineUserService;

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
  /// <param name="menuService">菜单服务</param>
  /// <param name="sessionService">会话服务</param>
  /// <param name="jwtSettings">JWT设置</param>
  /// <param name="onlineUserService">在线用户服务</param>
  /// <param name="userContext">用户上下文</param>
  public LeanAuthService(
      IMemoryCache cache,
      LeanSliderCaptchaHelper sliderCaptchaHelper,
      IOptions<LeanSliderCaptchaOptions> options,
      IOptions<LeanSecurityOptions> securitySettings,
      ILeanRepository<LeanDeviceExtend> deviceExtendRepository,
      ILeanRepository<LeanLoginExtend> loginExtendRepository,
      ILeanRepository<LeanUser> userRepository,
      ITokenService tokenService,
      ILeanHttpContextAccessor httpContextAccessor,
      ILeanUserContext userContext,
      LeanServerInfoHelper serverInfoHelper,
      LeanClientInfoHelper clientInfoHelper,
      LeanIpHelper ipHelper,
      ILeanLoginLogService loginLogService,
      ILeanMenuService menuService,
      ILeanSessionService sessionService,
      IOptions<JwtSettings> jwtSettings,
      ILeanOnlineUserService onlineUserService)
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
    _userContext = userContext;
    _serverInfoHelper = serverInfoHelper;
    _clientInfoHelper = clientInfoHelper;
    _ipHelper = ipHelper;
    _loginLogService = loginLogService;
    _menuService = menuService;
    _sessionService = sessionService;
    _jwtSettings = jwtSettings.Value;
    _onlineUserService = onlineUserService;
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
      await LoginErrorLogAsync(request.UserName, "Unknown", request.LoginIp ?? "Unknown",
        await _ipHelper.GetIpLocationAsync(request.LoginIp ?? "Unknown"),
        request.Browser ?? "Unknown", request.Os ?? "Unknown",
        "验证码验证失败");
      return LeanApiResult<LeanLoginResponseDto>.Error(captchaResult.Message, LeanErrorCode.Status400BadRequest, LeanBusinessType.Grant);
    }

    // 验证用户名和密码
    var user = await _userRepository.FirstOrDefaultAsync(u => u.UserName == request.UserName);
    if (user == null)
    {
      await LoginErrorLogAsync(request.UserName, "Unknown", request.LoginIp ?? "Unknown",
        await _ipHelper.GetIpLocationAsync(request.LoginIp ?? "Unknown"),
        request.Browser ?? "Unknown", request.Os ?? "Unknown",
        "用户名不存在");
      return LeanApiResult<LeanLoginResponseDto>.Error("用户名或密码错误", LeanErrorCode.Status400BadRequest, LeanBusinessType.Grant);
    }

    // 获取系统信息
    var serverInfo = _serverInfoHelper.GetSystemInfo();
    var clientInfo = _clientInfoHelper.GetSystemInfo();

    // 记录 userAgent 信息
    Console.WriteLine($"前端传递的 UserAgent: {request.UserAgent}");
    Console.WriteLine($"客户端信息中的 UserAgent: {clientInfo.UserAgent}");
    Console.WriteLine($"设备类型: {request.DeviceType} -> {((LeanDeviceType)request.DeviceType).ToString()}");
    Console.WriteLine($"浏览器信息: {request.Browser}");
    Console.WriteLine($"操作系统: {request.Os}");
    Console.WriteLine($"屏幕信息: {request.ScreenWidth}x{request.ScreenHeight} ({request.ScreenPixelRatio})");
    Console.WriteLine($"设备名称: {request.DeviceName}");

    // 生成设备ID，使用 LeanDeviceHelper
    var deviceFingerprint = $"{request.Os}|{request.Browser}|{request.DeviceType}";
    Console.WriteLine($"生成的设备指纹: {deviceFingerprint}");
    var deviceId = LeanDeviceHelper.GenerateDeviceId(
      deviceFingerprint,
      request.Browser,
      request.DeviceType == (int)LeanDeviceType.Mobile ? LeanDeviceType.Mobile.ToString() : LeanDeviceType.Desktop.ToString()
    );

    // 检查用户是否已在线
    Console.WriteLine("========== 登录时检查用户在线状态 ==========");
    Console.WriteLine($"用户ID: {user.Id}");
    Console.WriteLine($"设备指纹: {deviceFingerprint}");
    Console.WriteLine($"生成的设备ID: {deviceId}");
    var isOnline = await _onlineUserService.IsUserOnlineAsync(user.Id, deviceId);
    Console.WriteLine($"用户在线状态: {isOnline}");
    Console.WriteLine("================================");

    if (isOnline)
    {
      return LeanApiResult<LeanLoginResponseDto>.Error("用户已在线，请先退出其他设备", LeanErrorCode.Status400BadRequest, LeanBusinessType.Grant);
    }

    // 更新登录扩展信息
    var loginExtend = await LoginExtendAsync(user, deviceId, clientInfo, serverInfo);

    // 检查账户锁定状态
    var (isLocked, lockMessage) = CheckAccountLockStatus(loginExtend);
    if (isLocked)
    {
      await LoginErrorLogAsync(request.UserName, deviceId, clientInfo.IpAddress,
        await _ipHelper.GetIpLocationAsync(clientInfo.IpAddress),
        clientInfo.BrowserInfo.Browser, clientInfo.BrowserInfo.Platform,
        lockMessage);
      await _loginExtendRepository.UpdateAsync(loginExtend);
      return LeanApiResult<LeanLoginResponseDto>.Error(lockMessage, LeanErrorCode.Status400BadRequest, LeanBusinessType.Grant);
    }

    // 验证密码
    if (!LeanPassword.VerifyPassword(request.Password, user.Password, user.Salt))
    {
      UpdateAccountLockStatus(loginExtend);
      await _loginExtendRepository.UpdateAsync(loginExtend);

      if (_securitySettings.Account.EnableNotification &&
          loginExtend.PasswordErrorCount >= _securitySettings.Account.NotificationThreshold)
      {
        // TODO: 发送通知
      }

      await LoginErrorLogAsync(request.UserName, deviceId, clientInfo.IpAddress,
        await _ipHelper.GetIpLocationAsync(clientInfo.IpAddress),
        clientInfo.BrowserInfo.Browser, clientInfo.BrowserInfo.Platform,
        $"密码错误，已错误{loginExtend.PasswordErrorCount}次");

      return LeanApiResult<LeanLoginResponseDto>.Error("用户名或密码错误", LeanErrorCode.Status400BadRequest, LeanBusinessType.Grant);
    }

    // 登录成功，重置错误次数和锁定状态
    loginExtend.PasswordErrorCount = 0;
    loginExtend.LockStatus = 0;
    loginExtend.UnlockTime = loginExtend.LockStatus != 0 ? DateTime.Now : null;

    // 删除验证码缓存
    _cache.Remove($"slider_captcha:{request.CaptchaKey}");

    // 新增或更新登录日志
    await LoginLogAsync(user, deviceId, clientInfo);

    // 更新设备扩展信息
    var device = await DeviceExtendAsync(user, deviceId, clientInfo);

    // 更新在线用户状态
    var connectionId = Guid.NewGuid().ToString();
    await _onlineUserService.UpdateUserInfoAsync(
        connectionId: connectionId,
        userId: user.Id,
        userName: user.UserName,
        avatar: user.Avatar,
        deviceFingerprint: deviceId
    );

    // 生成基本的 Claims
    var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim(ClaimTypes.GivenName, user.RealName),
        new Claim("EnglishName", user.EnglishName ?? string.Empty),
        new Claim("UserType", user.UserType.ToString()),
        new Claim("Avatar", user.Avatar ?? string.Empty),
        new Claim("ConnectionId", connectionId),
        new Claim("DeviceId", deviceId),
        new Claim("LastLoginTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
    };

    // 生成 Token
    var token = _tokenService.CreateToken(claims.ToArray());

    // 创建会话
    var session = await _sessionService.CreateSessionAsync(
      user,
      deviceId,
      clientInfo.IpAddress,
      clientInfo.BrowserInfo.UserAgent
    );

    // 返回登录结果
    return LeanApiResult<LeanLoginResponseDto>.Ok(new LeanLoginResponseDto
    {
      AccessToken = token,
      ExpiresIn = _options.ExpirySeconds,
      UserInfo = new LeanUserInfoDto
      {
        UserId = user.Id,
        UserName = user.UserName,
        NickName = user.RealName,
        Avatar = user.Avatar
      }
    }, LeanBusinessType.Grant);
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
  /// <param name="request">登出请求参数</param>
  /// <returns>登出结果</returns>
  public async Task<LeanApiResult> LogoutAsync(LeanLogoutDto request)
  {
    try
    {
      Console.WriteLine("========== 登出过程日志 ==========");
      Console.WriteLine($"登出请求参数: {JsonConvert.SerializeObject(request)}");

      // 从 Token 中获取用户 ID 和设备 ID
      var token = request.Token;
      if (string.IsNullOrEmpty(token))
      {
        Console.WriteLine("登出失败：Token 为空");
        return LeanApiResult.Error("用户未登录", LeanErrorCode.Status401Unauthorized, LeanBusinessType.Grant);
      }

      var tokenClaims = _tokenService.GetTokenClaims(token);
      var userIdClaim = tokenClaims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
      var deviceIdClaim = tokenClaims.FirstOrDefault(c => c.Type == "DeviceId");

      if (userIdClaim == null || !long.TryParse(userIdClaim.Value, out var userId))
      {
        Console.WriteLine("登出失败：无法从 Token 中获取用户 ID");
        return LeanApiResult.Error("用户未登录", LeanErrorCode.Status401Unauthorized, LeanBusinessType.Grant);
      }

      if (deviceIdClaim == null)
      {
        Console.WriteLine("登出失败：无法从 Token 中获取设备 ID");
        return LeanApiResult.Error("用户未登录", LeanErrorCode.Status401Unauthorized, LeanBusinessType.Grant);
      }

      var deviceId = deviceIdClaim.Value;

      // 获取系统信息
      var serverInfo = _serverInfoHelper.GetSystemInfo();
      var clientInfo = _clientInfoHelper.GetSystemInfo();
      Console.WriteLine($"客户端信息: {JsonConvert.SerializeObject(clientInfo)}");

      // 记录设备信息
      Console.WriteLine("========== 登出时设备信息 ==========");
      Console.WriteLine($"用户ID: {userId}");
      Console.WriteLine($"设备ID: {deviceId}");
      Console.WriteLine($"浏览器: {request.Browser}");
      Console.WriteLine($"操作系统: {request.Os}");
      Console.WriteLine($"设备类型: {request.DeviceType}");
      Console.WriteLine("================================");

      // 检查用户是否在线
      var isOnline = await _onlineUserService.IsUserOnlineAsync(userId, deviceId);
      if (!isOnline)
      {
        Console.WriteLine("登出失败：用户未登录");
        return LeanApiResult.Error("用户未登录", LeanErrorCode.Status401Unauthorized, LeanBusinessType.Grant);
      }
      Console.WriteLine($"用户在线状态: {isOnline}");

      // 查询当前在线用户记录
      Console.WriteLine("========== 查询在线用户状态 ==========");
      var onlineUsers = await _onlineUserService.GetOnlineUsersAsync();
      Console.WriteLine($"当前在线用户总数: {onlineUsers.Count}");
      Console.WriteLine("所有在线用户列表:");
      foreach (var user in onlineUsers)
      {
        Console.WriteLine($"用户ID: {user.UserId}, 设备ID: {user.DeviceId}, 连接ID: {user.ConnectionId}, 在线状态: {user.IsOnline}");
      }

      var currentUser = onlineUsers.FirstOrDefault(u => u.DeviceId == deviceId);
      Console.WriteLine($"\n当前用户在线记录:");
      Console.WriteLine($"是否找到用户: {currentUser != null}");
      if (currentUser != null)
      {
        Console.WriteLine($"用户ID: {currentUser.UserId}");
        Console.WriteLine($"设备ID: {currentUser.DeviceId}");
        Console.WriteLine($"连接ID: {currentUser.ConnectionId}");
        Console.WriteLine($"当前在线状态: {currentUser.IsOnline}");

        // 更新用户在线状态
        Console.WriteLine("\n开始更新用户在线状态...");
        await _onlineUserService.UpdateUserStatusAsync(
          connectionId: currentUser.ConnectionId,
          isOnline: false,
          userId: userId,
          deviceId: deviceId
        );
        Console.WriteLine("在线状态更新完成");

        // 再次查询确认状态是否更新
        Console.WriteLine("\n验证状态更新结果:");
        var updatedUser = (await _onlineUserService.GetOnlineUsersAsync()).FirstOrDefault(u => u.DeviceId == deviceId);
        if (updatedUser != null)
        {
          Console.WriteLine($"更新后的在线状态: {updatedUser.IsOnline}");
          Console.WriteLine($"更新后的连接ID: {updatedUser.ConnectionId}");
        }
        else
        {
          Console.WriteLine("用户记录已被移除");
        }
      }
      else
      {
        Console.WriteLine("未找到当前用户的在线记录");
        return LeanApiResult.Error("用户未登录", LeanErrorCode.Status401Unauthorized, LeanBusinessType.Grant);
      }
      Console.WriteLine("================================");

      // 获取登录扩展信息
      var loginExtend = await _loginExtendRepository.FirstOrDefaultAsync(l => l.UserId == userId);
      if (loginExtend != null)
      {
        // 更新登录日志
        await _loginLogService.AddLogoutLogAsync(
          userId,
          request.UserName,
          deviceId,
          request.LoginIp,
          await _ipHelper.GetIpLocationAsync(request.LoginIp),
          request.Browser,
          request.Os
        );

        // 更新设备扩展信息
        var device = await _deviceExtendRepository.FirstOrDefaultAsync(d =>
          d.UserId == userId &&
          d.DeviceId == deviceId
        );
        if (device != null)
        {
          device.LastLoginTime = DateTime.Now;
          device.DeviceStatus = 0; // 离线状态
          await _deviceExtendRepository.UpdateAsync(device);
        }

        // 更新登录扩展信息
        loginExtend.LastLogoutTime = DateTime.Now;
        loginExtend.LastLogoutIp = request.LoginIp;
        loginExtend.LastLogoutLocation = await _ipHelper.GetIpLocationAsync(request.LoginIp);
        loginExtend.SystemInfo = JsonConvert.SerializeObject(new { Server = serverInfo, Client = clientInfo });
        await _loginExtendRepository.UpdateAsync(loginExtend);

        // 使会话失效
        await _sessionService.InvalidateSessionAsync(userId, deviceId);
      }

      // 将当前令牌加入黑名单
      var cacheKey = $"token_blacklist:{token}";
      var expirationTime = tokenClaims.FirstOrDefault(c => c.Type == "exp")?.Value;
      if (expirationTime != null && long.TryParse(expirationTime, out var exp))
      {
        var expiration = DateTimeOffset.FromUnixTimeSeconds(exp).DateTime - DateTime.UtcNow;
        if (expiration > TimeSpan.Zero)
        {
          _cache.Set(cacheKey, true, expiration);
        }
      }

      return LeanApiResult.Ok(LeanBusinessType.Grant);
    }
    catch (Exception ex)
    {
      return LeanApiResult.Error($"登出失败：{ex.Message}", LeanErrorCode.Status500InternalServerError, LeanBusinessType.Grant);
    }
  }

  /// <summary>
  /// 获取当前登录用户信息
  /// </summary>
  /// <returns>用户信息</returns>
  public async Task<LeanApiResult<LeanUserDto>> GetCurrentUserAsync()
  {
    try
    {
      // 获取当前用户ID
      var userId = _userContext.GetCurrentUserId();
      if (!userId.HasValue)
      {
        return LeanApiResult<LeanUserDto>.Error("未登录", LeanErrorCode.Status401Unauthorized, LeanBusinessType.Grant);
      }

      // 获取用户信息
      var user = await _userRepository.FirstOrDefaultAsync(u => u.Id == userId.Value);
      if (user == null)
      {
        return LeanApiResult<LeanUserDto>.Error("用户不存在", LeanErrorCode.Status404NotFound, LeanBusinessType.Grant);
      }

      // 转换为DTO
      var userDto = user.Adapt<LeanUserDto>();

      return LeanApiResult<LeanUserDto>.Ok(userDto, LeanBusinessType.Grant);
    }
    catch (Exception ex)
    {
      return LeanApiResult<LeanUserDto>.Error($"获取用户信息失败：{ex.Message}", LeanErrorCode.Status500InternalServerError, LeanBusinessType.Grant);
    }
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
  /// 记录登录日志
  /// </summary>
  private async Task LoginLogAsync(LeanUser user, string deviceId, ClientSystemInfo clientInfo)
  {
    await _loginLogService.AddLoginLogAsync(
      user.Id,
      user.UserName,
      deviceId,
      clientInfo.IpAddress,
      await _ipHelper.GetIpLocationAsync(clientInfo.IpAddress),
      clientInfo.BrowserInfo.Browser,
      clientInfo.BrowserInfo.Platform
    );
  }

  /// <summary>
  /// 更新登录扩展信息
  /// </summary>
  private async Task<LeanLoginExtend> LoginExtendAsync(LeanUser user, string deviceId, ClientSystemInfo clientInfo, ServerSystemInfo serverInfo)
  {
    var loginExtend = await _loginExtendRepository.FirstOrDefaultAsync(l => l.UserId == user.Id);
    var loginLocation = await _ipHelper.GetIpLocationAsync(clientInfo.IpAddress);

    if (loginExtend == null)
    {
      loginExtend = new LeanLoginExtend
      {
        UserId = user.Id,
        PasswordErrorCount = 0,
        LockStatus = 0, // 正常状态
        FirstLoginIp = clientInfo.IpAddress,
        FirstLoginLocation = loginLocation,
        FirstLoginTime = DateTime.Now,
        FirstDeviceId = deviceId,
        FirstBrowser = clientInfo.BrowserInfo?.Browser ?? "未知",
        FirstOs = clientInfo.BrowserInfo?.Platform ?? "未知",
        FirstLoginType = (int)LeanLoginType.Password,
        LastLoginIp = clientInfo.IpAddress,
        LastLoginLocation = loginLocation,
        LastLoginTime = DateTime.Now,
        LastDeviceId = deviceId,
        LastBrowser = clientInfo.BrowserInfo?.Browser ?? "未知",
        LastOs = clientInfo.BrowserInfo?.Platform ?? "未知",
        LastLoginType = (int)LeanLoginType.Password,
        SystemInfo = JsonConvert.SerializeObject(new { Server = serverInfo, Client = clientInfo })
      };
      await _loginExtendRepository.CreateAsync(loginExtend);
    }
    else
    {
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
    }
    return loginExtend;
  }

  /// <summary>
  /// 更新设备扩展信息
  /// </summary>
  private async Task<LeanDeviceExtend> DeviceExtendAsync(LeanUser user, string deviceId, ClientSystemInfo clientInfo)
  {
    // 检查设备是否已存在
    var device = await _deviceExtendRepository.FirstOrDefaultAsync(d =>
      d.UserId == user.Id &&
      d.DeviceId == deviceId);

    if (device != null)
    {
      // 更新设备信息
      device.LastLoginIp = clientInfo.IpAddress;
      device.LastLoginTime = DateTime.Now;
      device.DeviceStatus = (int)LeanDeviceStatus.Normal;
      device.Browser = clientInfo.BrowserInfo.Browser;
      device.Os = clientInfo.BrowserInfo.Platform;
      await _deviceExtendRepository.UpdateAsync(device);
      return device;
    }

    // 创建新设备
    device = new LeanDeviceExtend
    {
      UserId = user.Id,
      DeviceId = deviceId,
      DeviceName = clientInfo.BrowserInfo.Platform,
      DeviceType = clientInfo.BrowserInfo.IsMobile ? (int)LeanDeviceType.Mobile : (int)LeanDeviceType.Desktop,
      Os = clientInfo.BrowserInfo.Platform,
      Browser = clientInfo.BrowserInfo.Browser,
      LastLoginIp = clientInfo.IpAddress,
      LastLoginTime = DateTime.Now,
      IsTrusted = 0,
      DeviceStatus = (int)LeanDeviceStatus.Normal
    };

    await _deviceExtendRepository.CreateAsync(device);
    return device;
  }

  /// <summary>
  /// 记录登录错误日志
  /// </summary>
  private async Task LoginErrorLogAsync(string userName, string deviceId, string ip, string location, string browser, string os, string errorMessage)
  {
    await _loginLogService.AddLoginErrorLogAsync(
      userName,
      deviceId,
      ip,
      location,
      browser,
      os,
      errorMessage
    );
  }
}