using System.ComponentModel.DataAnnotations;
using Lean.CodeGen.Common.Models;

namespace Lean.CodeGen.Application.Dtos.Identity;

/// <summary>
/// 用户会话查询参数
/// </summary>
public class LeanQueryUserSessionDto : LeanPage
{
  /// <summary>
  /// 用户ID
  /// </summary>
  public long? UserId { get; set; }

  /// <summary>
  /// 会话令牌
  /// </summary>
  public string? Token { get; set; }

  /// <summary>
  /// 登录IP
  /// </summary>
  public string? LoginIp { get; set; }

  /// <summary>
  /// 开始时间
  /// </summary>
  public DateTime? StartTime { get; set; }

  /// <summary>
  /// 结束时间
  /// </summary>
  public DateTime? EndTime { get; set; }
}

/// <summary>
/// 用户会话创建参数
/// </summary>
public class LeanCreateUserSessionDto
{
  /// <summary>
  /// 用户ID
  /// </summary>
  [Required(ErrorMessage = "用户ID不能为空")]
  public long UserId { get; set; }

  /// <summary>
  /// 会话令牌
  /// </summary>
  [Required(ErrorMessage = "会话令牌不能为空")]
  [StringLength(100, ErrorMessage = "会话令牌长度不能超过100个字符")]
  public string Token { get; set; } = default!;

  /// <summary>
  /// 登录IP
  /// </summary>
  [Required(ErrorMessage = "登录IP不能为空")]
  [StringLength(50, ErrorMessage = "登录IP长度不能超过50个字符")]
  public string LoginIp { get; set; } = default!;

  /// <summary>
  /// 登录地点
  /// </summary>
  [StringLength(100, ErrorMessage = "登录地点长度不能超过100个字符")]
  public string? LoginLocation { get; set; }

  /// <summary>
  /// 浏览器
  /// </summary>
  [StringLength(100, ErrorMessage = "浏览器长度不能超过100个字符")]
  public string? Browser { get; set; }

  /// <summary>
  /// 操作系统
  /// </summary>
  [StringLength(100, ErrorMessage = "操作系统长度不能超过100个字符")]
  public string? Os { get; set; }

  /// <summary>
  /// 过期时间
  /// </summary>
  [Required(ErrorMessage = "过期时间不能为空")]
  public DateTime ExpireTime { get; set; }

  /// <summary>
  /// 刷新令牌
  /// </summary>
  [StringLength(100, ErrorMessage = "刷新令牌长度不能超过100个字符")]
  public string? RefreshToken { get; set; }

  /// <summary>
  /// 激活的角色
  /// </summary>
  [StringLength(500, ErrorMessage = "激活的角色长度不能超过500个字符")]
  public string? ActiveRoles { get; set; }
}

/// <summary>
/// 用户会话更新参数
/// </summary>
public class LeanUpdateUserSessionDto : LeanCreateUserSessionDto
{
  /// <summary>
  /// 会话ID
  /// </summary>
  [Required(ErrorMessage = "会话ID不能为空")]
  public long Id { get; set; }
}

/// <summary>
/// 用户会话详情
/// </summary>
public class LeanUserSessionDetailDto
{
  /// <summary>
  /// 会话ID
  /// </summary>
  public long Id { get; set; }

  /// <summary>
  /// 用户ID
  /// </summary>
  public long UserId { get; set; }

  /// <summary>
  /// 用户名称
  /// </summary>
  public string UserName { get; set; } = default!;

  /// <summary>
  /// 会话令牌
  /// </summary>
  public string Token { get; set; } = default!;

  /// <summary>
  /// 登录IP
  /// </summary>
  public string LoginIp { get; set; } = default!;

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
  /// 过期时间
  /// </summary>
  public DateTime ExpireTime { get; set; }

  /// <summary>
  /// 刷新令牌
  /// </summary>
  public string? RefreshToken { get; set; }

  /// <summary>
  /// 激活的角色
  /// </summary>
  public string? ActiveRoles { get; set; }

  /// <summary>
  /// 创建时间
  /// </summary>
  public DateTime CreateTime { get; set; }

  /// <summary>
  /// 创建者
  /// </summary>
  public string? CreateUserName { get; set; }

  /// <summary>
  /// 更新时间
  /// </summary>
  public DateTime? UpdateTime { get; set; }

  /// <summary>
  /// 更新者
  /// </summary>
  public string? UpdateUserName { get; set; }
}

/// <summary>
/// 用户会话 DTO
/// </summary>
public class LeanUserSessionDto
{
  /// <summary>
  /// 会话ID
  /// </summary>
  public long Id { get; set; }

  /// <summary>
  /// 用户ID
  /// </summary>
  public long UserId { get; set; }

  /// <summary>
  /// 用户名称
  /// </summary>
  public string UserName { get; set; } = default!;

  /// <summary>
  /// 会话令牌
  /// </summary>
  public string Token { get; set; } = default!;

  /// <summary>
  /// 登录IP
  /// </summary>
  public string LoginIp { get; set; } = default!;

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
  /// 过期时间
  /// </summary>
  public DateTime ExpireTime { get; set; }

  /// <summary>
  /// 刷新令牌
  /// </summary>
  public string? RefreshToken { get; set; }

  /// <summary>
  /// 激活的角色
  /// </summary>
  public string? ActiveRoles { get; set; }
}