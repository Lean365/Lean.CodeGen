using System;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using Lean.CodeGen.Common.Options;
using Lean.CodeGen.Common.Security;
using Lean.CodeGen.Common.Exceptions;
using Lean.CodeGen.Domain.Entities.Identity;
using Lean.CodeGen.Domain.Interfaces.Repositories;
using Lean.CodeGen.Application.Dtos.Identity.Login;
using Lean.CodeGen.Common.Enums;
using System.Collections.Generic;
using Lean.CodeGen.Application.Dtos.Identity;
using Mapster;

namespace Lean.CodeGen.Application.Services.Identity;

/// <summary>
/// 认证服务实现
/// </summary>
public class LeanAuthService : ILeanAuthService
{
  private readonly ILeanRepository<LeanUser> _userRepository;
  private readonly ILeanRepository<LeanLoginExtend> _loginExtendRepository;
  private readonly ILeanRepository<LeanDeviceExtend> _deviceExtendRepository;
  private readonly IHttpContextAccessor _httpContextAccessor;
  private readonly IOptions<LeanSecurityOptions> _securityOptions;
  private readonly ITokenService _tokenService;

  public LeanAuthService(
      ILeanRepository<LeanUser> userRepository,
      ILeanRepository<LeanLoginExtend> loginExtendRepository,
      ILeanRepository<LeanDeviceExtend> deviceExtendRepository,
      IHttpContextAccessor httpContextAccessor,
      IOptions<LeanSecurityOptions> securityOptions,
      ITokenService tokenService)
  {
    _userRepository = userRepository;
    _loginExtendRepository = loginExtendRepository;
    _deviceExtendRepository = deviceExtendRepository;
    _httpContextAccessor = httpContextAccessor;
    _securityOptions = securityOptions;
    _tokenService = tokenService;
  }

  /// <summary>
  /// 用户登录
  /// </summary>
  public async Task<LeanLoginResultDto> LoginAsync(LeanLoginDto input)
  {
    try
    {
      // 获取用户
      var user = await _userRepository.FirstOrDefaultAsync(u => u.UserName == input.UserName);
      if (user == null)
      {
        throw new LeanException("用户名或密码错误");
      }

      // 获取登录扩展信息
      var loginExtend = await _loginExtendRepository.FirstOrDefaultAsync(le => le.UserId == user.Id);
      if (loginExtend == null)
      {
        loginExtend = new LeanLoginExtend
        {
          UserId = user.Id,
          LoginStatus = LeanLoginStatus.Normal
        };
        await _loginExtendRepository.CreateAsync(loginExtend);
      }

      // 检查用户状态
      if (loginExtend.LoginStatus != LeanLoginStatus.Normal)
      {
        throw new LeanException("账户已被锁定或禁用");
      }

      // 检查密码错误次数
      if (loginExtend.PasswordErrorCount >= _securityOptions.Value.MaxPasswordAttempts)
      {
        var lockEndTime = loginExtend.LastPasswordErrorTime?.AddMinutes(_securityOptions.Value.PasswordLockMinutes);
        if (lockEndTime.HasValue && DateTime.Now < lockEndTime)
        {
          var remainingMinutes = (lockEndTime.Value - DateTime.Now).TotalMinutes;
          throw new LeanException($"密码错误次数过多，账号已被锁定，请{remainingMinutes:F0}分钟后重试");
        }
      }

      // 验证密码
      var password = LeanPassword.HashPassword(input.Password, user.Salt);
      if (password != user.Password)
      {
        loginExtend.PasswordErrorCount++;
        if (loginExtend.PasswordErrorCount >= _securityOptions.Value.MaxPasswordAttempts)
        {
          loginExtend.LastPasswordErrorTime = DateTime.Now;
          loginExtend.LoginStatus = LeanLoginStatus.Locked;
        }
        await _loginExtendRepository.UpdateAsync(loginExtend);
        throw new LeanException("用户名或密码错误");
      }

      // 更新登录信息
      loginExtend.PasswordErrorCount = 0;
      loginExtend.LastLoginTime = DateTime.Now;
      loginExtend.LastLoginIp = input.LoginIp;
      loginExtend.LastLoginLocation = input.LoginLocation;
      loginExtend.LastLoginType = LeanLoginType.Password;
      loginExtend.LastBrowser = input.Browser;
      loginExtend.LastOs = input.Os;

      // 如果是首次登录，记录首次登录信息
      if (!loginExtend.FirstLoginTime.HasValue)
      {
        loginExtend.FirstLoginTime = DateTime.Now;
        loginExtend.FirstLoginIp = input.LoginIp;
        loginExtend.FirstLoginLocation = input.LoginLocation;
        loginExtend.FirstLoginType = LeanLoginType.Password;
        loginExtend.FirstBrowser = input.Browser;
        loginExtend.FirstOs = input.Os;
      }

      await _loginExtendRepository.UpdateAsync(loginExtend);

      // 更新设备信息
      if (!string.IsNullOrEmpty(input.DeviceId))
      {
        var device = await _deviceExtendRepository.FirstOrDefaultAsync(d => d.DeviceId == input.DeviceId);
        if (device == null)
        {
          device = new LeanDeviceExtend
          {
            UserId = user.Id,
            DeviceId = input.DeviceId,
            DeviceType = LeanDeviceType.Other,
            DeviceStatus = LeanDeviceStatus.Normal,
            LastLoginTime = DateTime.Now,
            LastLoginIp = input.LoginIp,
            Browser = input.Browser,
            Os = input.Os
          };
          await _deviceExtendRepository.CreateAsync(device);
        }
        else
        {
          device.LastLoginTime = DateTime.Now;
          device.LastLoginIp = input.LoginIp;
          device.Browser = input.Browser;
          device.Os = input.Os;
          await _deviceExtendRepository.UpdateAsync(device);
        }
      }

      // 生成令牌
      var claims = new[]
      {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim(ClaimTypes.GivenName, user.RealName),
        new Claim("UserType", user.UserType.ToString())
      };

      return new LeanLoginResultDto
      {
        AccessToken = _tokenService.CreateToken(claims),
        RefreshToken = _tokenService.CreateRefreshToken(),
        ExpiresIn = _securityOptions.Value.Jwt.ExpireMinutes * 60,
        User = user.Adapt<LeanUserDto>()
      };
    }
    catch (Exception ex)
    {
      throw new LeanException($"登录失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 刷新令牌
  /// </summary>
  public async Task<LeanLoginResultDto> RefreshTokenAsync(string refreshToken)
  {
    try
    {
      // 验证刷新令牌
      var principal = _tokenService.ValidateToken(refreshToken);
      var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      if (string.IsNullOrEmpty(userId) || !long.TryParse(userId, out var id))
      {
        throw new LeanException("无效的刷新令牌");
      }

      // 获取用户信息
      var user = await _userRepository.GetByIdAsync(id);
      if (user == null)
      {
        throw new LeanException("用户不存在");
      }

      // 生成新的令牌
      var claims = new[]
      {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim(ClaimTypes.GivenName, user.RealName),
        new Claim("UserType", user.UserType.ToString())
      };

      // 更新登录扩展信息
      var loginExtend = await _loginExtendRepository.FirstOrDefaultAsync(le => le.UserId == id);
      if (loginExtend != null)
      {
        loginExtend.LastLoginTime = DateTime.Now;
        loginExtend.LastLoginType = LeanLoginType.Token;
        await _loginExtendRepository.UpdateAsync(loginExtend);
      }

      return new LeanLoginResultDto
      {
        AccessToken = _tokenService.CreateToken(claims),
        RefreshToken = _tokenService.CreateRefreshToken(),
        ExpiresIn = _securityOptions.Value.Jwt.ExpireMinutes * 60,
        User = user.Adapt<LeanUserDto>()
      };
    }
    catch (Exception ex)
    {
      throw new LeanException($"刷新令牌失败: {ex.Message}");
    }
  }

  /// <summary>
  /// 退出登录
  /// </summary>
  public async Task LogoutAsync()
  {
    // 获取当前用户ID
    var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    if (string.IsNullOrEmpty(userId) || !long.TryParse(userId, out var id))
    {
      return;
    }

    // 更新登录扩展信息
    var loginExtend = await _loginExtendRepository.FirstOrDefaultAsync(le => le.UserId == id);
    if (loginExtend != null)
    {
      loginExtend.LoginStatus = LeanLoginStatus.Normal;
      await _loginExtendRepository.UpdateAsync(loginExtend);
    }
  }

  /// <summary>
  /// 验证密码
  /// </summary>
  private bool VerifyPassword(string password, string passwordHash)
  {
    // TODO: 实现密码验证逻辑
    return password == passwordHash;
  }
}