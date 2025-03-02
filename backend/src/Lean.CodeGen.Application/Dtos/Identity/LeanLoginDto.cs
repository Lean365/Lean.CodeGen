using System;
using System.ComponentModel.DataAnnotations;
using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Common.Models;

namespace Lean.CodeGen.Application.Dtos.Identity;

/// <summary>
/// 登录信息DTO
/// </summary>
public class LeanLoginDto : LeanBaseDto
{
  /// <summary>
  /// 用户ID
  /// </summary>
  public long UserId { get; set; }

  /// <summary>
  /// 用户名
  /// </summary>
  public string UserName { get; set; } = string.Empty;

  /// <summary>
  /// 设备ID
  /// </summary>
  public string DeviceId { get; set; } = string.Empty;

  /// <summary>
  /// 设备名称
  /// </summary>
  public string? DeviceName { get; set; }

  /// <summary>
  /// 设备类型
  /// </summary>
  public LeanDeviceType DeviceType { get; set; }

  /// <summary>
  /// 是否信任设备
  /// </summary>
  public bool IsTrusted { get; set; }

  /// <summary>
  /// 设备状态
  /// </summary>
  public LeanDeviceStatus DeviceStatus { get; set; }

  /// <summary>
  /// 登录IP
  /// </summary>
  public string? LoginIp { get; set; }

  /// <summary>
  /// 登录地点
  /// </summary>
  public string? LoginLocation { get; set; }

  /// <summary>
  /// 浏览器
  /// </summary>
  public string? Browser { get; set; }

  /// <summary>
  /// 操作系统
  /// </summary>
  public string? Os { get; set; }

  /// <summary>
  /// 登录方式
  /// </summary>
  public LeanLoginType LoginType { get; set; }

  /// <summary>
  /// 登录状态
  /// </summary>
  public LeanLoginStatus LoginStatus { get; set; }

  /// <summary>
  /// 密码错误次数
  /// </summary>
  public int PasswordErrorCount { get; set; }

  /// <summary>
  /// 最后密码错误时间
  /// </summary>
  public DateTime? LastPasswordErrorTime { get; set; }

  /// <summary>
  /// 密码修改时间
  /// </summary>
  public DateTime? PasswordUpdateTime { get; set; }

  /// <summary>
  /// 会话令牌
  /// </summary>
  public string Token { get; set; } = string.Empty;

  /// <summary>
  /// 刷新令牌
  /// </summary>
  public string? RefreshToken { get; set; }

  /// <summary>
  /// 过期时间
  /// </summary>
  public DateTime ExpireTime { get; set; }
}

/// <summary>
/// 登录查询参数
/// </summary>
public class LeanQueryLoginDto : LeanPage
{
  /// <summary>
  /// 用户ID
  /// </summary>
  public long? UserId { get; set; }

  /// <summary>
  /// 用户名
  /// </summary>
  public string? UserName { get; set; }

  /// <summary>
  /// 设备ID
  /// </summary>
  public string? DeviceId { get; set; }

  /// <summary>
  /// 设备类型
  /// </summary>
  public LeanDeviceType? DeviceType { get; set; }

  /// <summary>
  /// 设备状态
  /// </summary>
  public LeanDeviceStatus? DeviceStatus { get; set; }

  /// <summary>
  /// 登录IP
  /// </summary>
  public string? LoginIp { get; set; }

  /// <summary>
  /// 登录地点
  /// </summary>
  public string? LoginLocation { get; set; }

  /// <summary>
  /// 登录方式
  /// </summary>
  public LeanLoginType? LoginType { get; set; }

  /// <summary>
  /// 登录状态
  /// </summary>
  public LeanLoginStatus? LoginStatus { get; set; }

  /// <summary>
  /// 创建时间范围-开始
  /// </summary>
  public DateTime? StartTime { get; set; }

  /// <summary>
  /// 创建时间范围-结束
  /// </summary>
  public DateTime? EndTime { get; set; }
}

/// <summary>
/// 登录参数
/// </summary>
public class LeanLoginParamDto
{
  /// <summary>
  /// 用户名
  /// </summary>
  [Required(ErrorMessage = "用户名不能为空")]
  [StringLength(50, MinimumLength = 2, ErrorMessage = "用户名长度必须在2-50个字符之间")]
  public string UserName { get; set; } = string.Empty;

  /// <summary>
  /// 密码
  /// </summary>
  [Required(ErrorMessage = "密码不能为空")]
  [StringLength(100, MinimumLength = 6, ErrorMessage = "密码长度必须在6-100个字符之间")]
  public string Password { get; set; } = string.Empty;

  /// <summary>
  /// 验证码
  /// </summary>
  public string? VerifyCode { get; set; }

  /// <summary>
  /// 设备ID
  /// </summary>
  public string? DeviceId { get; set; }

  /// <summary>
  /// 设备名称
  /// </summary>
  public string? DeviceName { get; set; }

  /// <summary>
  /// 设备类型
  /// </summary>
  public LeanDeviceType DeviceType { get; set; }

  /// <summary>
  /// 是否信任设备
  /// </summary>
  public bool IsTrusted { get; set; }

  /// <summary>
  /// 登录IP
  /// </summary>
  public string? LoginIp { get; set; }

  /// <summary>
  /// 浏览器
  /// </summary>
  public string? Browser { get; set; }

  /// <summary>
  /// 操作系统
  /// </summary>
  public string? Os { get; set; }
}

/// <summary>
/// 登录结果
/// </summary>
public class LeanLoginResultDto
{
  /// <summary>
  /// 用户ID
  /// </summary>
  public long UserId { get; set; }

  /// <summary>
  /// 用户名
  /// </summary>
  public string UserName { get; set; } = string.Empty;

  /// <summary>
  /// 真实姓名
  /// </summary>
  public string RealName { get; set; } = string.Empty;

  /// <summary>
  /// 头像
  /// </summary>
  public string? Avatar { get; set; }

  /// <summary>
  /// 访问令牌
  /// </summary>
  public string Token { get; set; } = string.Empty;

  /// <summary>
  /// 刷新令牌
  /// </summary>
  public string? RefreshToken { get; set; }

  /// <summary>
  /// 过期时间
  /// </summary>
  public DateTime ExpireTime { get; set; }
}

/// <summary>
/// 刷新令牌参数
/// </summary>
public class LeanRefreshTokenDto
{
  /// <summary>
  /// 刷新令牌
  /// </summary>
  [Required(ErrorMessage = "刷新令牌不能为空")]
  public string RefreshToken { get; set; } = string.Empty;
}

/// <summary>
/// 登出参数
/// </summary>
public class LeanLogoutDto
{
  /// <summary>
  /// 设备ID
  /// </summary>
  [Required(ErrorMessage = "设备ID不能为空")]
  public string DeviceId { get; set; } = string.Empty;
}

/// <summary>
/// 登录请求
/// </summary>
public class LeanLoginRequestDto
{
  /// <summary>
  /// 用户名
  /// </summary>
  [Required(ErrorMessage = "用户名不能为空")]
  [StringLength(50, ErrorMessage = "用户名长度不能超过50个字符")]
  public string UserName { get; set; } = default!;

  /// <summary>
  /// 密码
  /// </summary>
  [Required(ErrorMessage = "密码不能为空")]
  [StringLength(50, ErrorMessage = "密码长度不能超过50个字符")]
  public string Password { get; set; } = default!;

  /// <summary>
  /// 滑块验证码X坐标
  /// </summary>
  [Required(ErrorMessage = "滑块验证码位置不能为空")]
  public int SliderX { get; set; }

  /// <summary>
  /// 滑块验证码Y坐标
  /// </summary>
  [Required(ErrorMessage = "滑块验证码位置不能为空")]
  public int SliderY { get; set; }

  /// <summary>
  /// 验证码键
  /// </summary>
  [Required(ErrorMessage = "验证码键不能为空")]
  public string CaptchaKey { get; set; } = default!;
}

/// <summary>
/// 登录响应
/// </summary>
public class LeanLoginResponseDto
{
  /// <summary>
  /// 访问令牌
  /// </summary>
  public string AccessToken { get; set; } = default!;

  /// <summary>
  /// 过期时间（分钟）
  /// </summary>
  public int ExpiresIn { get; set; }

  /// <summary>
  /// 用户信息
  /// </summary>
  public LeanUserInfoDto UserInfo { get; set; } = default!;
}

/// <summary>
/// 用户信息
/// </summary>
public class LeanUserInfoDto
{
  /// <summary>
  /// 用户ID
  /// </summary>
  public long UserId { get; set; }

  /// <summary>
  /// 用户名
  /// </summary>
  public string UserName { get; set; } = default!;

  /// <summary>
  /// 昵称
  /// </summary>
  public string NickName { get; set; } = default!;

  /// <summary>
  /// 头像
  /// </summary>
  public string? Avatar { get; set; }

  /// <summary>
  /// 角色列表
  /// </summary>
  public List<string> Roles { get; set; } = new();

  /// <summary>
  /// 权限列表
  /// </summary>
  public List<string> Permissions { get; set; } = new();
}

/// <summary>
/// 验证码响应
/// </summary>
public class LeanCaptchaResponseDto
{
  /// <summary>
  /// 验证码键
  /// </summary>
  public string CaptchaKey { get; set; } = default!;

  /// <summary>
  /// 验证码图片（Base64）
  /// </summary>
  public string CaptchaImage { get; set; } = default!;
}