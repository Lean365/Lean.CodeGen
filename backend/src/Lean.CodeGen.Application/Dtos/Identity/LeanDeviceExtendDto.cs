using System;
using System.ComponentModel.DataAnnotations;
using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Common.Models;

namespace Lean.CodeGen.Application.Dtos.Identity;

/// <summary>
/// 设备扩展信息 DTO
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
  /// <remarks>
  /// 设备的唯一标识
  /// </remarks>
  [Required(ErrorMessage = "设备ID不能为空")]
  [StringLength(100, ErrorMessage = "设备ID长度不能超过100个字符")]
  public string DeviceId { get; set; } = default!;

  /// <summary>
  /// 设备名称
  /// </summary>
  public string? DeviceName { get; set; }

  /// <summary>
  /// 设备类型
  /// </summary>
  /// <remarks>
  /// 设备的类型（如：手机、平板、桌面等）
  /// </remarks>
  [StringLength(50, ErrorMessage = "设备类型长度不能超过50个字符")]
  public string? DeviceType { get; set; }

  /// <summary>
  /// 操作系统信息
  /// </summary>
  /// <remarks>
  /// 用户使用的操作系统信息
  /// </remarks>
  [StringLength(100, ErrorMessage = "操作系统信息长度不能超过100个字符")]
  public string? Os { get; set; }

  /// <summary>
  /// 浏览器信息
  /// </summary>
  /// <remarks>
  /// 用户使用的浏览器信息
  /// </remarks>
  [StringLength(100, ErrorMessage = "浏览器信息长度不能超过100个字符")]
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
  /// 屏幕宽度
  /// </summary>
  /// <remarks>
  /// 设备屏幕的宽度
  /// </remarks>
  public int? ScreenWidth { get; set; }

  /// <summary>
  /// 屏幕高度
  /// </summary>
  /// <remarks>
  /// 设备屏幕的高度
  /// </remarks>
  public int? ScreenHeight { get; set; }

  /// <summary>
  /// 屏幕颜色深度
  /// </summary>
  /// <remarks>
  /// 设备屏幕的颜色深度
  /// </remarks>
  public int? ScreenColorDepth { get; set; }

  /// <summary>
  /// 屏幕像素比
  /// </summary>
  /// <remarks>
  /// 设备屏幕的像素比
  /// </remarks>
  public double? ScreenPixelRatio { get; set; }

  /// <summary>
  /// 语言
  /// </summary>
  /// <remarks>
  /// 设备的语言设置
  /// </remarks>
  [StringLength(50, ErrorMessage = "语言长度不能超过50个字符")]
  public string? Language { get; set; }

  /// <summary>
  /// 时区
  /// </summary>
  /// <remarks>
  /// 设备的时区设置
  /// </remarks>
  [StringLength(50, ErrorMessage = "时区长度不能超过50个字符")]
  public string? Timezone { get; set; }

  /// <summary>
  /// 用户代理
  /// </summary>
  /// <remarks>
  /// 浏览器的用户代理字符串
  /// </remarks>
  [StringLength(500, ErrorMessage = "用户代理长度不能超过500个字符")]
  public string? UserAgent { get; set; }

  /// <summary>
  /// IP地址
  /// </summary>
  /// <remarks>
  /// 设备的IP地址
  /// </remarks>
  [StringLength(50, ErrorMessage = "IP地址长度不能超过50个字符")]
  public string? IpAddress { get; set; }

  /// <summary>
  /// 地理位置
  /// </summary>
  /// <remarks>
  /// 设备的地理位置信息
  /// </remarks>
  [StringLength(200, ErrorMessage = "地理位置长度不能超过200个字符")]
  public string? Location { get; set; }

  /// <summary>
  /// 设备型号
  /// </summary>
  /// <remarks>
  /// 设备的具体型号
  /// </remarks>
  [StringLength(100, ErrorMessage = "设备型号长度不能超过100个字符")]
  public string? DeviceModel { get; set; }

  /// <summary>
  /// 设备制造商
  /// </summary>
  /// <remarks>
  /// 设备的制造商信息
  /// </remarks>
  [StringLength(100, ErrorMessage = "设备制造商长度不能超过100个字符")]
  public string? DeviceManufacturer { get; set; }

  /// <summary>
  /// 关联的用户信息
  /// </summary>
  public LeanUserDto? User { get; set; }

  /// <summary>
  /// 设备信息 DTO
  /// </summary>
  public class DeviceInfoDto
  {
    /// <summary>
    /// 设备ID
    /// </summary>
    /// <remarks>
    /// 设备的唯一标识
    /// </remarks>
    [Required(ErrorMessage = "设备ID不能为空")]
    [StringLength(100, ErrorMessage = "设备ID长度不能超过100个字符")]
    public string DeviceId { get; set; } = default!;

    /// <summary>
    /// 浏览器信息
    /// </summary>
    /// <remarks>
    /// 用户使用的浏览器信息
    /// </remarks>
    [StringLength(100, ErrorMessage = "浏览器信息长度不能超过100个字符")]
    public string? Browser { get; set; }

    /// <summary>
    /// 操作系统信息
    /// </summary>
    /// <remarks>
    /// 用户使用的操作系统信息
    /// </remarks>
    [StringLength(100, ErrorMessage = "操作系统信息长度不能超过100个字符")]
    public string? Os { get; set; }

    /// <summary>
    /// 屏幕宽度
    /// </summary>
    /// <remarks>
    /// 设备屏幕的宽度
    /// </remarks>
    public int? ScreenWidth { get; set; }

    /// <summary>
    /// 屏幕高度
    /// </summary>
    /// <remarks>
    /// 设备屏幕的高度
    /// </remarks>
    public int? ScreenHeight { get; set; }

    /// <summary>
    /// 屏幕颜色深度
    /// </summary>
    /// <remarks>
    /// 设备屏幕的颜色深度
    /// </remarks>
    public int? ScreenColorDepth { get; set; }

    /// <summary>
    /// 屏幕像素比
    /// </summary>
    /// <remarks>
    /// 设备屏幕的像素比
    /// </remarks>
    public double? ScreenPixelRatio { get; set; }

    /// <summary>
    /// 语言
    /// </summary>
    /// <remarks>
    /// 设备的语言设置
    /// </remarks>
    [StringLength(50, ErrorMessage = "语言长度不能超过50个字符")]
    public string? Language { get; set; }

    /// <summary>
    /// 时区
    /// </summary>
    /// <remarks>
    /// 设备的时区设置
    /// </remarks>
    [StringLength(50, ErrorMessage = "时区长度不能超过50个字符")]
    public string? Timezone { get; set; }
  }

  /// <summary>
  /// 设备ID DTO
  /// </summary>
  public class DeviceIdDto
  {
    /// <summary>
    /// 设备ID
    /// </summary>
    /// <remarks>
    /// 设备的唯一标识
    /// </remarks>
    [Required(ErrorMessage = "设备ID不能为空")]
    [StringLength(100, ErrorMessage = "设备ID长度不能超过100个字符")]
    public string DeviceId { get; set; } = default!;
  }
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

/// <summary>
/// 设备扩展导出参数
/// </summary>
public class LeanDeviceExtendExportDto : LeanDeviceExtendQueryDto
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
/// 设备ID DTO
/// </summary>
public class LeanDeviceIdDto
{
  /// <summary>
  /// 设备ID
  /// </summary>
  /// <remarks>
  /// 设备的唯一标识
  /// </remarks>
  [Required(ErrorMessage = "设备ID不能为空")]
  [StringLength(100, ErrorMessage = "设备ID长度不能超过100个字符")]
  public string DeviceId { get; set; } = default!;
}

/// <summary>
/// 设备信息 DTO
/// </summary>
public class LeanDeviceInfoDto
{
  /// <summary>
  /// 设备ID
  /// </summary>
  /// <remarks>
  /// 设备的唯一标识
  /// </remarks>
  [Required(ErrorMessage = "设备ID不能为空")]
  [StringLength(100, ErrorMessage = "设备ID长度不能超过100个字符")]
  public string DeviceId { get; set; } = default!;

  /// <summary>
  /// 设备名称
  /// </summary>
  [StringLength(100, ErrorMessage = "设备名称长度不能超过100个字符")]
  public string? DeviceName { get; set; }

  /// <summary>
  /// 设备类型
  /// </summary>
  /// <remarks>
  /// 设备的类型（如：手机、平板、桌面等）
  /// </remarks>
  [StringLength(50, ErrorMessage = "设备类型长度不能超过50个字符")]
  public string? DeviceType { get; set; }

  /// <summary>
  /// 设备状态
  /// </summary>
  public int DeviceStatus { get; set; }

  /// <summary>
  /// 创建时间
  /// </summary>
  public DateTime CreateTime { get; set; }

  /// <summary>
  /// 更新时间
  /// </summary>
  public DateTime? UpdateTime { get; set; }

  /// <summary>
  /// 操作系统信息
  /// </summary>
  [StringLength(100, ErrorMessage = "操作系统信息长度不能超过100个字符")]
  public string? Os { get; set; }

  /// <summary>
  /// 浏览器信息
  /// </summary>
  [StringLength(100, ErrorMessage = "浏览器信息长度不能超过100个字符")]
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
  /// 用户代理
  /// </summary>
  [StringLength(500, ErrorMessage = "用户代理长度不能超过500个字符")]
  public string? UserAgent { get; set; }

  /// <summary>
  /// 平台信息
  /// </summary>
  [StringLength(100, ErrorMessage = "平台信息长度不能超过100个字符")]
  public string? Platform { get; set; }

  /// <summary>
  /// 屏幕宽度
  /// </summary>
  public int? ScreenWidth { get; set; }

  /// <summary>
  /// 屏幕高度
  /// </summary>
  public int? ScreenHeight { get; set; }

  /// <summary>
  /// 设备像素比
  /// </summary>
  public double? DevicePixelRatio { get; set; }

  /// <summary>
  /// CPU核心数
  /// </summary>
  public int? HardwareConcurrency { get; set; }

  /// <summary>
  /// 设备内存
  /// </summary>
  public double? DeviceMemory { get; set; }
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
public class LeanDeviceExtendUpdateDto : LeanDeviceExtendCreateDto
{
  /// <summary>
  /// 设备扩展ID
  /// </summary>
  [Required(ErrorMessage = "设备扩展ID不能为空")]
  public long Id { get; set; }

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