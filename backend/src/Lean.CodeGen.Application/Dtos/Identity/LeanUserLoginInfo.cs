using System;

namespace Lean.CodeGen.Application.Dtos.Identity;

/// <summary>
/// 用户登录信息
/// </summary>
public class LeanUserLoginInfo
{
  /// <summary>
  /// 最后登录时间
  /// </summary>
  public DateTime? LastLoginTime { get; set; }

  /// <summary>
  /// 最后登录IP
  /// </summary>
  public string? LastLoginIp { get; set; }

  /// <summary>
  /// 最后登录地点
  /// </summary>
  public string? LastLoginLocation { get; set; }

  /// <summary>
  /// 最后登录浏览器
  /// </summary>
  public string? LastLoginBrowser { get; set; }

  /// <summary>
  /// 最后登录操作系统
  /// </summary>
  public string? LastLoginOs { get; set; }
}