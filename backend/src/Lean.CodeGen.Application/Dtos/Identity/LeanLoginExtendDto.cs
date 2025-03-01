using System;
using System.ComponentModel.DataAnnotations;
using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Common.Models;

namespace Lean.CodeGen.Application.Dtos.Identity;

/// <summary>
/// 登录扩展基础信息
/// </summary>
public class LeanLoginExtendDto : LeanBaseDto
{
  /// <summary>
  /// 用户ID
  /// </summary>
  public long UserId { get; set; }

  /// <summary>
  /// 密码错误次数
  /// </summary>
  public int PasswordErrorCount { get; set; }

  /// <summary>
  /// 首次登录IP
  /// </summary>
  public string? FirstLoginIp { get; set; }

  /// <summary>
  /// 首次登录地点
  /// </summary>
  public string? FirstLoginLocation { get; set; }

  /// <summary>
  /// 首次登录时间
  /// </summary>
  public DateTime? FirstLoginTime { get; set; }

  /// <summary>
  /// 首次登录设备
  /// </summary>
  public string? FirstDeviceId { get; set; }

  /// <summary>
  /// 首次登录浏览器
  /// </summary>
  public string? FirstBrowser { get; set; }

  /// <summary>
  /// 首次登录系统
  /// </summary>
  public string? FirstOs { get; set; }

  /// <summary>
  /// 首次登录方式
  /// </summary>
  public LeanLoginType? FirstLoginType { get; set; }

  /// <summary>
  /// 末次登录IP
  /// </summary>
  public string? LastLoginIp { get; set; }

  /// <summary>
  /// 末次登录地点
  /// </summary>
  public string? LastLoginLocation { get; set; }

  /// <summary>
  /// 末次登录时间
  /// </summary>
  public DateTime? LastLoginTime { get; set; }

  /// <summary>
  /// 末次登录设备
  /// </summary>
  public string? LastDeviceId { get; set; }

  /// <summary>
  /// 末次登录浏览器
  /// </summary>
  public string? LastBrowser { get; set; }

  /// <summary>
  /// 末次登录系统
  /// </summary>
  public string? LastOs { get; set; }

  /// <summary>
  /// 末次登录方式
  /// </summary>
  public LeanLoginType? LastLoginType { get; set; }

  /// <summary>
  /// 最后密码错误时间
  /// </summary>
  public DateTime? LastPasswordErrorTime { get; set; }

  /// <summary>
  /// 密码修改时间
  /// </summary>
  public DateTime? PasswordUpdateTime { get; set; }

  /// <summary>
  /// 登录状态
  /// </summary>
  public LeanLoginStatus LoginStatus { get; set; }

  /// <summary>
  /// 关联的用户信息
  /// </summary>
  public LeanUserDto? User { get; set; }

  /// <summary>
  /// 首次登录设备信息
  /// </summary>
  public LeanDeviceExtendDto? FirstDevice { get; set; }

  /// <summary>
  /// 末次登录设备信息
  /// </summary>
  public LeanDeviceExtendDto? LastDevice { get; set; }
}

/// <summary>
/// 登录扩展查询参数
/// </summary>
public class LeanQueryLoginExtendDto : LeanPage
{
  /// <summary>
  /// 用户ID
  /// </summary>
  public long? UserId { get; set; }

  /// <summary>
  /// 登录状态
  /// </summary>
  public LeanLoginStatus? LoginStatus { get; set; }

  /// <summary>
  /// 首次登录时间范围-开始
  /// </summary>
  public DateTime? FirstLoginStartTime { get; set; }

  /// <summary>
  /// 首次登录时间范围-结束
  /// </summary>
  public DateTime? FirstLoginEndTime { get; set; }

  /// <summary>
  /// 末次登录时间范围-开始
  /// </summary>
  public DateTime? LastLoginStartTime { get; set; }

  /// <summary>
  /// 末次登录时间范围-结束
  /// </summary>
  public DateTime? LastLoginEndTime { get; set; }

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
/// 登录扩展创建参数
/// </summary>
public class LeanCreateLoginExtendDto
{
  /// <summary>
  /// 用户ID
  /// </summary>
  [Required(ErrorMessage = "用户ID不能为空")]
  public long UserId { get; set; }

  /// <summary>
  /// 首次登录IP
  /// </summary>
  [StringLength(50, ErrorMessage = "登录IP长度不能超过50个字符")]
  public string? FirstLoginIp { get; set; }

  /// <summary>
  /// 首次登录地点
  /// </summary>
  [StringLength(100, ErrorMessage = "登录地点长度不能超过100个字符")]
  public string? FirstLoginLocation { get; set; }

  /// <summary>
  /// 首次登录设备
  /// </summary>
  [StringLength(50, ErrorMessage = "设备ID长度不能超过50个字符")]
  public string? FirstDeviceId { get; set; }

  /// <summary>
  /// 首次登录浏览器
  /// </summary>
  [StringLength(50, ErrorMessage = "浏览器信息长度不能超过50个字符")]
  public string? FirstBrowser { get; set; }

  /// <summary>
  /// 首次登录系统
  /// </summary>
  [StringLength(50, ErrorMessage = "操作系统信息长度不能超过50个字符")]
  public string? FirstOs { get; set; }

  /// <summary>
  /// 首次登录方式
  /// </summary>
  public LeanLoginType? FirstLoginType { get; set; }
}

/// <summary>
/// 登录扩展更新参数
/// </summary>
public class LeanUpdateLoginExtendDto
{
  /// <summary>
  /// 登录扩展ID
  /// </summary>
  [Required(ErrorMessage = "登录扩展ID不能为空")]
  public long Id { get; set; }

  /// <summary>
  /// 末次登录IP
  /// </summary>
  [StringLength(50, ErrorMessage = "登录IP长度不能超过50个字符")]
  public string? LastLoginIp { get; set; }

  /// <summary>
  /// 末次登录地点
  /// </summary>
  [StringLength(100, ErrorMessage = "登录地点长度不能超过100个字符")]
  public string? LastLoginLocation { get; set; }

  /// <summary>
  /// 末次登录设备
  /// </summary>
  [StringLength(50, ErrorMessage = "设备ID长度不能超过50个字符")]
  public string? LastDeviceId { get; set; }

  /// <summary>
  /// 末次登录浏览器
  /// </summary>
  [StringLength(50, ErrorMessage = "浏览器信息长度不能超过50个字符")]
  public string? LastBrowser { get; set; }

  /// <summary>
  /// 末次登录系统
  /// </summary>
  [StringLength(50, ErrorMessage = "操作系统信息长度不能超过50个字符")]
  public string? LastOs { get; set; }

  /// <summary>
  /// 末次登录方式
  /// </summary>
  public LeanLoginType? LastLoginType { get; set; }
}

/// <summary>
/// 登录扩展状态变更参数
/// </summary>
public class LeanChangeLoginExtendStatusDto
{
  /// <summary>
  /// 登录扩展ID
  /// </summary>
  [Required(ErrorMessage = "登录扩展ID不能为空")]
  public long Id { get; set; }

  /// <summary>
  /// 登录状态
  /// </summary>
  [Required(ErrorMessage = "登录状态不能为空")]
  public LeanLoginStatus LoginStatus { get; set; }
}

/// <summary>
/// 登录扩展导出参数
/// </summary>
public class LeanExportLoginExtendDto : LeanQueryLoginExtendDto
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
  /// </summary>
  public bool IsExportAll { get; set; }

  /// <summary>
  /// 选中的ID列表
  /// </summary>
  public List<long> SelectedIds { get; set; } = new();
}

/// <summary>
/// 登录扩展删除参数
/// </summary>
public class LeanDeleteLoginExtendDto
{
  /// <summary>
  /// 登录扩展ID
  /// </summary>
  [Required(ErrorMessage = "登录扩展ID不能为空")]
  public long Id { get; set; }
}