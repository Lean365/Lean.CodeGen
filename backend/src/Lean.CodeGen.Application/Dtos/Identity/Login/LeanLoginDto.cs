using System.ComponentModel.DataAnnotations;

namespace Lean.CodeGen.Application.Dtos.Identity.Login;

/// <summary>
/// 登录请求
/// </summary>
public class LeanLoginDto
{
  /// <summary>
  /// 用户名
  /// </summary>
  [Required(ErrorMessage = "用户名不能为空")]
  [StringLength(50, ErrorMessage = "用户名长度不能超过50个字符")]
  public string UserName { get; set; }

  /// <summary>
  /// 密码
  /// </summary>
  [Required(ErrorMessage = "密码不能为空")]
  [StringLength(50, ErrorMessage = "密码长度不能超过50个字符")]
  public string Password { get; set; }

  /// <summary>
  /// 验证码
  /// </summary>
  public string? VerifyCode { get; set; }

  /// <summary>
  /// 设备ID
  /// </summary>
  public string? DeviceId { get; set; }

  /// <summary>
  /// 浏览器
  /// </summary>
  public string? Browser { get; set; }

  /// <summary>
  /// 操作系统
  /// </summary>
  public string? Os { get; set; }

  /// <summary>
  /// 登录IP
  /// </summary>
  public string? LoginIp { get; set; }

  /// <summary>
  /// 登录地点
  /// </summary>
  public string? LoginLocation { get; set; }
}

/// <summary>
/// 登录结果
/// </summary>
public class LeanLoginResultDto
{
  /// <summary>
  /// 访问令牌
  /// </summary>
  public string AccessToken { get; set; } = string.Empty;

  /// <summary>
  /// 刷新令牌
  /// </summary>
  public string RefreshToken { get; set; } = string.Empty;

  /// <summary>
  /// 过期时间（秒）
  /// </summary>
  public int ExpiresIn { get; set; }

  /// <summary>
  /// 用户信息
  /// </summary>
  public LeanUserDto User { get; set; } = null!;
}