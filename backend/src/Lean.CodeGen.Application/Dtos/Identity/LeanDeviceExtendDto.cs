using System;
using System.ComponentModel.DataAnnotations;
using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Common.Models;

namespace Lean.CodeGen.Application.Dtos.Identity;

/// <summary>
/// 设备扩展基础信息
/// </summary>
public class LeanDeviceExtendDto : LeanBaseDto
{
  /// <summary>
  /// 用户ID
  /// </summary>
  public long UserId { get; set; }

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
  public int DeviceType { get; set; }

  /// <summary>
  /// 操作系统
  /// </summary>
  public string? Os { get; set; }

  /// <summary>
  /// 浏览器
  /// </summary>
  public string? Browser { get; set; }

  /// <summary>
  /// 最后登录IP
  /// </summary>
  public string? LastLoginIp { get; set; }

  /// <summary>
  /// 最后登录时间
  /// </summary>
  public DateTime? LastLoginTime { get; set; }

  /// <summary>
  /// 是否信任设备
  /// 0-否
  /// 1-是
  /// </summary>
  public int? IsTrusted { get; set; }

  /// <summary>
  /// 设备状态
  /// </summary>
  public int DeviceStatus { get; set; }

  /// <summary>
  /// 关联的用户信息
  /// </summary>
  public LeanUserDto? User { get; set; }
}

/// <summary>
/// 设备扩展查询参数
/// </summary>
public class LeanDeviceExtendQueryDto : LeanPage
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
  /// 设备名称
  /// </summary>
  public string? DeviceName { get; set; }

  /// <summary>
  /// 设备类型
  /// </summary>
  public int? DeviceType { get; set; }

  /// <summary>
  /// 设备状态
  /// </summary>
  public int? DeviceStatus { get; set; }

  /// <summary>
  /// 是否信任设备
  /// </summary>
  public bool? IsTrusted { get; set; }

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
/// 设备扩展创建参数
/// </summary>
public class LeanDeviceExtendCreateDto
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
  /// 操作系统
  /// </summary>
  [StringLength(50, ErrorMessage = "操作系统信息长度不能超过50个字符")]
  public string? Os { get; set; }

  /// <summary>
  /// 浏览器
  /// </summary>
  [StringLength(50, ErrorMessage = "浏览器信息长度不能超过50个字符")]
  public string? Browser { get; set; }

  /// <summary>
  /// 最后登录IP
  /// </summary>
  [StringLength(50, ErrorMessage = "登录IP长度不能超过50个字符")]
  public string? LastLoginIp { get; set; }

  /// <summary>
  /// 是否信任设备
  /// 0-否
  /// 1-是
  /// </summary>
  public int IsTrusted { get; set; }
}

/// <summary>
/// 设备扩展更新参数
/// </summary>
public class LeanDeviceExtendUpdateDto
{
  /// <summary>
  /// 设备扩展ID
  /// </summary>
  [Required(ErrorMessage = "设备扩展ID不能为空")]
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
}

/// <summary>
/// 设备扩展状态变更参数
/// </summary>
public class LeanDeviceExtendChangeStatusDto
{
  /// <summary>
  /// 设备扩展ID
  /// </summary>
  [Required(ErrorMessage = "设备扩展ID不能为空")]
  public long Id { get; set; }

  /// <summary>
  /// 设备状态
  /// </summary>
  [Required(ErrorMessage = "设备状态不能为空")]
  public int DeviceStatus { get; set; }
}

/// <summary>
/// 设备扩展导出查询参数
/// </summary>
public class LeanDeviceExtendExportQueryDto : LeanDeviceExtendQueryDto
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
/// 设备扩展删除参数
/// </summary>
public class LeanDeviceExtendDeleteDto
{
  /// <summary>
  /// 设备扩展ID
  /// </summary>
  [Required(ErrorMessage = "设备扩展ID不能为空")]
  public long Id { get; set; }
}