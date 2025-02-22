//===================================================
// 项目名: Lean.CodeGen.Domain
// 文件名: LeanDeviceExtend.cs
// 功能描述: 设备扩展实体
// 创建时间: 2024-03-26
// 作者: Lean
// 版本: 1.0
//===================================================

using SqlSugar;
using Lean.CodeGen.Common.Enums;

namespace Lean.CodeGen.Domain.Entities.Identity;

/// <summary>
/// 设备扩展实体
/// </summary>
[SugarTable("lean_device_extend", "设备扩展表")]
[SugarIndex("idx_user_device", nameof(UserId), OrderByType.Asc)]
public class LeanDeviceExtend : LeanBaseEntity
{
  /// <summary>
  /// 用户ID
  /// </summary>
  /// <remarks>
  /// 关联的用户ID
  /// </remarks>
  [SugarColumn(ColumnName = "user_id", ColumnDescription = "用户ID", IsNullable = false, ColumnDataType = "bigint")]
  public long UserId { get; set; }

  /// <summary>
  /// 设备ID
  /// </summary>
  /// <remarks>
  /// 设备唯一标识
  /// </remarks>
  [SugarColumn(ColumnName = "device_id", ColumnDescription = "设备ID", Length = 50, IsNullable = false, ColumnDataType = "nvarchar")]
  public string DeviceId { get; set; } = string.Empty;

  /// <summary>
  /// 设备名称
  /// </summary>
  /// <remarks>
  /// 设备的显示名称
  /// </remarks>
  [SugarColumn(ColumnName = "device_name", ColumnDescription = "设备名称", Length = 100, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? DeviceName { get; set; }

  /// <summary>
  /// 设备类型
  /// </summary>
  /// <remarks>
  /// 设备类型：Mobile-移动设备，Desktop-桌面设备，Other-其他
  /// </remarks>
  [SugarColumn(ColumnName = "device_type", ColumnDescription = "设备类型", IsNullable = false, DefaultValue = "0", ColumnDataType = "int")]
  public LeanDeviceType DeviceType { get; set; }

  /// <summary>
  /// 操作系统
  /// </summary>
  /// <remarks>
  /// 设备的操作系统信息
  /// </remarks>
  [SugarColumn(ColumnName = "os", ColumnDescription = "操作系统", Length = 50, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? Os { get; set; }

  /// <summary>
  /// 浏览器
  /// </summary>
  /// <remarks>
  /// 设备使用的浏览器信息
  /// </remarks>
  [SugarColumn(ColumnName = "browser", ColumnDescription = "浏览器", Length = 50, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? Browser { get; set; }

  /// <summary>
  /// 最后登录IP
  /// </summary>
  /// <remarks>
  /// 最后一次登录的IP地址
  /// </remarks>
  [SugarColumn(ColumnName = "last_login_ip", ColumnDescription = "最后登录IP", Length = 50, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? LastLoginIp { get; set; }

  /// <summary>
  /// 最后登录时间
  /// </summary>
  /// <remarks>
  /// 最后一次登录的时间
  /// </remarks>
  [SugarColumn(ColumnName = "last_login_time", ColumnDescription = "最后登录时间", IsNullable = true, ColumnDataType = "datetime")]
  public DateTime? LastLoginTime { get; set; }

  /// <summary>
  /// 是否信任设备
  /// </summary>
  /// <remarks>
  /// 标记设备是否为受信任设备
  /// </remarks>
  [SugarColumn(ColumnName = "is_trusted", ColumnDescription = "是否信任设备", IsNullable = false, DefaultValue = "0", ColumnDataType = "bit")]
  public bool IsTrusted { get; set; }

  /// <summary>
  /// 设备状态
  /// </summary>
  /// <remarks>
  /// 设备状态：Normal-正常，Disabled-禁用，Locked-锁定
  /// </remarks>
  [SugarColumn(ColumnName = "device_status", ColumnDescription = "设备状态", IsNullable = false, DefaultValue = "0", ColumnDataType = "int")]
  public LeanDeviceStatus DeviceStatus { get; set; }

  /// <summary>
  /// 用户
  /// </summary>
  /// <remarks>
  /// 关联的用户实体
  /// </remarks>
  [Navigate(NavigateType.OneToOne, nameof(UserId))]
  public virtual LeanUser User { get; set; } = default!;
}