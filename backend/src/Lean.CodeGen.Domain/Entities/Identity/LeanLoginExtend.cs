//===================================================
// 项目名: Lean.CodeGen.Domain
// 文件名: LeanLoginExtend.cs
// 功能描述: 登录扩展实体
// 创建时间: 2024-03-26
// 作者: Lean
// 版本: 1.0
//===================================================

using SqlSugar;
using Lean.CodeGen.Common.Enums;

namespace Lean.CodeGen.Domain.Entities.Identity;

/// <summary>
/// 登录扩展实体
/// </summary>
[SugarTable("lean_id_login_extend", "登录扩展表")]
[SugarIndex("idx_user", nameof(UserId), OrderByType.Asc)]
public class LeanLoginExtend : LeanBaseEntity
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
  /// 密码错误次数
  /// </summary>
  /// <remarks>
  /// 连续密码错误的次数
  /// </remarks>
  [SugarColumn(ColumnName = "password_error_count", ColumnDescription = "密码错误次数", IsNullable = false, DefaultValue = "0", ColumnDataType = "int")]
  public int PasswordErrorCount { get; set; }

  /// <summary>
  /// 最后密码错误时间
  /// </summary>
  /// <remarks>
  /// 最后一次密码错误的时间，用于计算锁定时间
  /// </remarks>
  [SugarColumn(ColumnName = "last_password_error_time", ColumnDescription = "最后密码错误时间", IsNullable = true, ColumnDataType = "datetime")]
  public DateTime? LastPasswordErrorTime { get; set; }

  /// <summary>
  /// 锁定状态
  /// </summary>
  /// <remarks>
  /// 当前锁定状态
  /// 0-正常
  /// 1-临时锁定
  /// 2-永久锁定
  /// </remarks>
  [SugarColumn(ColumnName = "lock_status", ColumnDescription = "锁定状态", IsNullable = false, ColumnDataType = "int")]
  public int LockStatus { get; set; }

  /// <summary>
  /// 锁定时间
  /// </summary>
  /// <remarks>
  /// 锁定的时间
  /// </remarks>
  [SugarColumn(ColumnName = "lock_time", ColumnDescription = "锁定时间", IsNullable = true, ColumnDataType = "datetime")]
  public DateTime? LockTime { get; set; }

  /// <summary>
  /// 解锁时间
  /// </summary>
  /// <remarks>
  /// 解锁的时间
  /// </remarks>
  [SugarColumn(ColumnName = "unlock_time", ColumnDescription = "解锁时间", IsNullable = true, ColumnDataType = "datetime")]
  public DateTime? UnlockTime { get; set; }

  /// <summary>
  /// 锁定原因
  /// </summary>
  /// <remarks>
  /// 锁定的原因
  /// </remarks>
  [SugarColumn(ColumnName = "lock_reason", ColumnDescription = "锁定原因", Length = 500, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? LockReason { get; set; }

  /// <summary>
  /// 解锁原因
  /// </summary>
  /// <remarks>
  /// 解锁的原因
  /// </remarks>
  [SugarColumn(ColumnName = "unlock_reason", ColumnDescription = "解锁原因", Length = 500, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? UnlockReason { get; set; }

  /// <summary>
  /// 首次登录IP
  /// </summary>
  /// <remarks>
  /// 第一次登录的IP地址
  /// </remarks>
  [SugarColumn(ColumnName = "first_login_ip", ColumnDescription = "首次登录IP", Length = 50, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? FirstLoginIp { get; set; }

  /// <summary>
  /// 首次登录地点
  /// </summary>
  /// <remarks>
  /// 第一次登录的地点
  /// </remarks>
  [SugarColumn(ColumnName = "first_login_location", ColumnDescription = "首次登录地点", Length = 100, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? FirstLoginLocation { get; set; }

  /// <summary>
  /// 首次登录时间
  /// </summary>
  /// <remarks>
  /// 第一次登录的时间
  /// </remarks>
  [SugarColumn(ColumnName = "first_login_time", ColumnDescription = "首次登录时间", IsNullable = true, ColumnDataType = "datetime")]
  public DateTime? FirstLoginTime { get; set; }

  /// <summary>
  /// 首次登录设备
  /// </summary>
  /// <remarks>
  /// 第一次登录的设备ID
  /// </remarks>
  [SugarColumn(ColumnName = "first_device_id", ColumnDescription = "首次登录设备", Length = 50, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? FirstDeviceId { get; set; }

  /// <summary>
  /// 首次登录浏览器
  /// </summary>
  /// <remarks>
  /// 第一次登录使用的浏览器
  /// </remarks>
  [SugarColumn(ColumnName = "first_browser", ColumnDescription = "首次登录浏览器", Length = 50, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? FirstBrowser { get; set; }

  /// <summary>
  /// 首次登录系统
  /// </summary>
  /// <remarks>
  /// 第一次登录的操作系统
  /// </remarks>
  [SugarColumn(ColumnName = "first_os", ColumnDescription = "首次登录系统", Length = 50, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? FirstOs { get; set; }

  /// <summary>
  /// 首次登录方式
  /// </summary>
  /// <remarks>
  /// 第一次登录使用的方式
  /// 0-密码
  /// 1-验证码
  /// 2-令牌
  /// 3-其他
  /// </remarks>
  [SugarColumn(ColumnName = "first_login_type", ColumnDescription = "首次登录方式", IsNullable = true, ColumnDataType = "int")]
  public int? FirstLoginType { get; set; }

  /// <summary>
  /// 末次登录IP
  /// </summary>
  /// <remarks>
  /// 最后一次登录的IP地址
  /// </remarks>
  [SugarColumn(ColumnName = "last_login_ip", ColumnDescription = "末次登录IP", Length = 50, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? LastLoginIp { get; set; }

  /// <summary>
  /// 末次登录地点
  /// </summary>
  /// <remarks>
  /// 最后一次登录的地点
  /// </remarks>
  [SugarColumn(ColumnName = "last_login_location", ColumnDescription = "末次登录地点", Length = 100, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? LastLoginLocation { get; set; }

  /// <summary>
  /// 末次登录时间
  /// </summary>
  /// <remarks>
  /// 最后一次登录的时间
  /// </remarks>
  [SugarColumn(ColumnName = "last_login_time", ColumnDescription = "末次登录时间", IsNullable = true, ColumnDataType = "datetime")]
  public DateTime? LastLoginTime { get; set; }

  /// <summary>
  /// 末次登录设备
  /// </summary>
  /// <remarks>
  /// 最后一次登录的设备ID
  /// </remarks>
  [SugarColumn(ColumnName = "last_device_id", ColumnDescription = "末次登录设备", Length = 50, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? LastDeviceId { get; set; }

  /// <summary>
  /// 末次登录浏览器
  /// </summary>
  /// <remarks>
  /// 最后一次登录使用的浏览器
  /// </remarks>
  [SugarColumn(ColumnName = "last_browser", ColumnDescription = "末次登录浏览器", Length = 50, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? LastBrowser { get; set; }

  /// <summary>
  /// 末次登录系统
  /// </summary>
  /// <remarks>
  /// 最后一次登录的操作系统
  /// </remarks>
  [SugarColumn(ColumnName = "last_os", ColumnDescription = "末次登录系统", Length = 50, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? LastOs { get; set; }

  /// <summary>
  /// 末次登录方式
  /// </summary>
  /// <remarks>
  /// 最后一次登录使用的方式
  /// 0-密码
  /// 1-验证码
  /// 2-令牌
  /// 3-其他
  /// </remarks>
  [SugarColumn(ColumnName = "last_login_type", ColumnDescription = "末次登录方式", IsNullable = true, ColumnDataType = "int")]
  public int? LastLoginType { get; set; }

  /// <summary>
  /// 登录状态
  /// </summary>
  /// <remarks>
  /// 当前登录状态：0-正常，1-锁定，2-禁用
  /// </remarks>
  [SugarColumn(ColumnName = "login_status", ColumnDescription = "登录状态", IsNullable = false, DefaultValue = "0", ColumnDataType = "int")]
  public int LoginStatus { get; set; }

  /// <summary>
  /// 系统信息
  /// </summary>
  [SugarColumn(ColumnName = "system_info", ColumnDescription = "系统信息", Length = -1, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? SystemInfo { get; set; }

  /// <summary>
  /// 末次登出IP
  /// </summary>
  /// <remarks>
  /// 最后一次登出的IP地址
  /// </remarks>
  [SugarColumn(ColumnName = "last_logout_ip", ColumnDescription = "末次登出IP", Length = 50, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? LastLogoutIp { get; set; }

  /// <summary>
  /// 末次登出地点
  /// </summary>
  /// <remarks>
  /// 最后一次登出的地点
  /// </remarks>
  [SugarColumn(ColumnName = "last_logout_location", ColumnDescription = "末次登出地点", Length = 100, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? LastLogoutLocation { get; set; }

  /// <summary>
  /// 末次登出时间
  /// </summary>
  /// <remarks>
  /// 最后一次登出的时间
  /// </remarks>
  [SugarColumn(ColumnName = "last_logout_time", ColumnDescription = "末次登出时间", IsNullable = true, ColumnDataType = "datetime")]
  public DateTime? LastLogoutTime { get; set; }

  /// <summary>
  /// 用户
  /// </summary>
  /// <remarks>
  /// 关联的用户实体
  /// </remarks>
  [Navigate(NavigateType.OneToOne, nameof(UserId))]
  public virtual LeanUser User { get; set; } = default!;

  /// <summary>
  /// 首次登录设备
  /// </summary>
  /// <remarks>
  /// 关联的首次登录设备实体
  /// </remarks>
  [Navigate(NavigateType.OneToOne, nameof(FirstDeviceId))]
  public virtual LeanDeviceExtend? FirstDevice { get; set; }

  /// <summary>
  /// 末次登录设备
  /// </summary>
  /// <remarks>
  /// 关联的末次登录设备实体
  /// </remarks>
  [Navigate(NavigateType.OneToOne, nameof(LastDeviceId))]
  public virtual LeanDeviceExtend? LastDevice { get; set; }
}