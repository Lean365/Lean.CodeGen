using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Common.Models;

namespace Lean.CodeGen.Application.Dtos.Identity;

/// <summary>
/// 用户会话基础信息
/// </summary>
public class LeanUserSessionDto : LeanBaseDto
{
  /// <summary>
  /// 用户ID
  /// </summary>
  public long UserId { get; set; }

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
  /// 0-PC
  /// 1-Mobile
  /// 2-Tablet
  /// 3-Other
  /// </summary>
  public int DeviceType { get; set; }

  /// <summary>
  /// 是否信任设备
  /// 0-否
  /// 1-是
  /// </summary>
  public int IsTrusted { get; set; }

  /// <summary>
  /// 设备状态
  /// 0-正常
  /// 1-禁用
  /// 2-锁定
  /// </summary>
  public int DeviceStatus { get; set; }

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
  /// 0-账号密码
  /// 1-手机验证码
  /// 2-邮箱验证码
  /// 3-第三方登录
  /// </summary>
  public int? LoginType { get; set; }

  /// <summary>
  /// 登录状态
  /// 0-正常
  /// 1-锁定
  /// 2-禁用
  /// </summary>
  public int LoginStatus { get; set; }

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
  /// 活动角色列表
  /// </summary>
  public List<long> ActiveRoles { get; set; } = [];

  /// <summary>
  /// 关联的用户信息
  /// </summary>
  public LeanUserDto? User { get; set; }
}

/// <summary>
/// 用户会话查询参数
/// </summary>
public class LeanUserSessionQueryDto : LeanPage
{
  /// <summary>
  /// 用户ID
  /// </summary>
  public long? UserId { get; set; }

  /// <summary>
  /// 设备ID
  /// </summary>
  public string? DeviceId { get; set; }

  /// <summary>
  /// 设备类型
  /// </summary>
  public int? DeviceType { get; set; }

  /// <summary>
  /// 设备状态
  /// </summary>
  public int? DeviceStatus { get; set; }

  /// <summary>
  /// 登录IP
  /// </summary>
  public string? LoginIp { get; set; }

  /// <summary>
  /// 登录状态
  /// </summary>
  public int? LoginStatus { get; set; }

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
/// 用户会话创建参数
/// </summary>
public class LeanUserSessionCreateDto
{
  /// <summary>
  /// 用户ID
  /// </summary>
  [Required(ErrorMessage = "用户ID不能为空")]
  public long UserId { get; set; }

  /// <summary>
  /// 设备ID
  /// </summary>
  [Required(ErrorMessage = "设备ID不能为空")]
  [StringLength(50, MinimumLength = 2, ErrorMessage = "设备ID长度必须在2-50个字符之间")]
  public string DeviceId { get; set; } = string.Empty;

  /// <summary>
  /// 设备名称
  /// </summary>
  [StringLength(100, ErrorMessage = "设备名称长度不能超过100个字符")]
  public string? DeviceName { get; set; }

  /// <summary>
  /// 设备类型
  /// </summary>
  public int DeviceType { get; set; }

  /// <summary>
  /// 是否信任设备
  /// 0-否
  /// 1-是
  /// </summary>
  public int IsTrusted { get; set; }

  /// <summary>
  /// 登录IP
  /// </summary>
  [StringLength(50, ErrorMessage = "登录IP长度不能超过50个字符")]
  public string? LoginIp { get; set; }

  /// <summary>
  /// 登录地点
  /// </summary>
  [StringLength(100, ErrorMessage = "登录地点长度不能超过100个字符")]
  public string? LoginLocation { get; set; }

  /// <summary>
  /// 浏览器
  /// </summary>
  [StringLength(50, ErrorMessage = "浏览器信息长度不能超过50个字符")]
  public string? Browser { get; set; }

  /// <summary>
  /// 操作系统
  /// </summary>
  [StringLength(50, ErrorMessage = "操作系统信息长度不能超过50个字符")]
  public string? Os { get; set; }

  /// <summary>
  /// 登录方式
  /// </summary>
  public int? LoginType { get; set; }

  /// <summary>
  /// 活动角色列表
  /// </summary>
  public List<long> ActiveRoles { get; set; } = [];
}

/// <summary>
/// 用户会话更新参数
/// </summary>
public class LeanUserSessionUpdateDto
{
  /// <summary>
  /// 会话ID
  /// </summary>
  [Required(ErrorMessage = "会话ID不能为空")]
  public long Id { get; set; }

  /// <summary>
  /// 设备名称
  /// </summary>
  [StringLength(100, ErrorMessage = "设备名称长度不能超过100个字符")]
  public string? DeviceName { get; set; }

  /// <summary>
  /// 是否信任设备
  /// 0-否
  /// 1-是
  /// </summary>
  public int IsTrusted { get; set; }

  /// <summary>
  /// 设备状态
  /// </summary>
  public int DeviceStatus { get; set; }

  /// <summary>
  /// 活动角色列表
  /// </summary>
  public List<long> ActiveRoles { get; set; } = [];
}

/// <summary>
/// 用户会话状态变更参数
/// </summary>
public class LeanUserSessionChangeStatusDto
{
  /// <summary>
  /// 会话ID
  /// </summary>
  [Required(ErrorMessage = "会话ID不能为空")]
  public long Id { get; set; }

  /// <summary>
  /// 设备状态
  /// </summary>
  [Required(ErrorMessage = "设备状态不能为空")]
  public int DeviceStatus { get; set; }
}

/// <summary>
/// 用户会话导出查询参数
/// </summary>
public class LeanUserSessionExportQueryDto : LeanUserSessionQueryDto
{
  /// <summary>
  /// 导出字段列表
  /// </summary>
  public List<string> ExportFields { get; set; } = new();

  /// <summary>
  /// 文件格式
  /// </summary>
  [Required(ErrorMessage = "文件格式不能为空")]
  public string FileFormat { get; set; } = "xlsx";

  /// <summary>
  /// 是否导出全部
  /// 0-否
  /// 1-是
  /// </summary>
  public int IsExportAll { get; set; }

  /// <summary>
  /// 选中的ID列表
  /// </summary>
  public List<long> SelectedIds { get; set; } = new();
}

/// <summary>
/// 用户会话删除参数
/// </summary>
public class LeanUserSessionDeleteDto
{
  /// <summary>
  /// 会话ID
  /// </summary>
  [Required(ErrorMessage = "会话ID不能为空")]
  public long Id { get; set; }
}