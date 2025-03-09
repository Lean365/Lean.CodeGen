//===================================================
// 项目名: Lean.CodeGen.Domain
// 文件名: LeanLoginLog.cs
// 功能描述: 登录日志实体
// 创建时间: 2024-03-26
// 作者: Lean
// 版本: 1.0
//===================================================

using SqlSugar;
using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Domain.Entities.Identity;

namespace Lean.CodeGen.Domain.Entities.Audit;

/// <summary>
/// 登录日志实体
/// </summary>
[SugarTable("lean_mon_login_log", "登录日志表")]
[SugarIndex("idx_user", nameof(UserId), OrderByType.Asc)]
[SugarIndex("idx_device", nameof(DeviceId), OrderByType.Asc)]
public class LeanLoginLog : LeanBaseEntity
{
  /// <summary>
  /// 用户ID
  /// </summary>
  /// <remarks>
  /// 登录用户的ID
  /// </remarks>
  [SugarColumn(ColumnName = "user_id", ColumnDescription = "用户ID", IsNullable = false, ColumnDataType = "bigint")]
  public long UserId { get; set; }

  /// <summary>
  /// 用户名
  /// </summary>
  /// <remarks>
  /// 登录用户的用户名
  /// </remarks>
  [SugarColumn(ColumnName = "user_name", ColumnDescription = "用户名", Length = 50, IsNullable = false, ColumnDataType = "nvarchar")]
  public string UserName { get; set; } = string.Empty;

  /// <summary>
  /// 设备ID
  /// </summary>
  /// <remarks>
  /// 登录设备的ID
  /// </remarks>
  [SugarColumn(ColumnName = "device_id", ColumnDescription = "设备ID", Length = 100, IsNullable = false, ColumnDataType = "nvarchar")]
  public string DeviceId { get; set; } = default!;

  /// <summary>
  /// 登录IP
  /// </summary>
  /// <remarks>
  /// 登录时的IP地址
  /// </remarks>
  [SugarColumn(ColumnName = "login_ip", ColumnDescription = "登录IP", Length = 50, IsNullable = false, ColumnDataType = "nvarchar")]
  public string LoginIp { get; set; } = default!;

  /// <summary>
  /// 客户端IP
  /// </summary>
  /// <remarks>
  /// 客户端的真实IP地址，通过X-Forwarded-For等头部获取
  /// </remarks>
  [SugarColumn(ColumnName = "client_ip", ColumnDescription = "客户端IP", Length = 50, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? ClientIp { get; set; }

  /// <summary>
  /// 登录地点
  /// </summary>
  /// <remarks>
  /// 根据IP解析的登录地点
  /// </remarks>
  [SugarColumn(ColumnName = "login_location", ColumnDescription = "登录地点", Length = 100, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? LoginLocation { get; set; }

  /// <summary>
  /// 浏览器
  /// </summary>
  /// <remarks>
  /// 登录使用的浏览器信息
  /// </remarks>
  [SugarColumn(ColumnName = "browser", ColumnDescription = "浏览器", Length = 100, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? Browser { get; set; }

  /// <summary>
  /// 操作系统
  /// </summary>
  /// <remarks>
  /// 登录设备的操作系统信息
  /// </remarks>
  [SugarColumn(ColumnName = "os", ColumnDescription = "操作系统", Length = 100, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? Os { get; set; }

  /// <summary>
  /// 登录状态
  /// </summary>
  /// <remarks>
  /// 登录状态：0-成功，1-失败
  /// </remarks>
  [SugarColumn(ColumnName = "login_status", ColumnDescription = "登录状态", IsNullable = false, DefaultValue = "0", ColumnDataType = "int")]
  public int LoginStatus { get; set; }

  /// <summary>
  /// 登录方式
  /// </summary>
  /// <remarks>
  /// 0-密码
  /// 1-验证码
  /// 2-令牌
  /// 3-其他
  /// </remarks>
  [SugarColumn(ColumnName = "login_type", ColumnDescription = "登录方式", IsNullable = false, DefaultValue = "0", ColumnDataType = "int")]
  public int LoginType { get; set; }

  /// <summary>
  /// 错误消息
  /// </summary>
  /// <remarks>
  /// 登录失败时的错误消息
  /// </remarks>
  [SugarColumn(ColumnName = "error_msg", ColumnDescription = "错误消息", Length = 500, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? ErrorMsg { get; set; }

  /// <summary>
  /// 用户
  /// </summary>
  /// <remarks>
  /// 关联的用户实体
  /// </remarks>
  [Navigate(NavigateType.OneToOne, nameof(UserId))]
  public virtual LeanUser User { get; set; } = default!;

  /// <summary>
  /// 设备
  /// </summary>
  /// <remarks>
  /// 关联的设备实体
  /// </remarks>
  [Navigate(NavigateType.OneToOne, nameof(DeviceId))]
  public virtual LeanDeviceExtend Device { get; set; } = default!;

  /// <summary>
  /// 登录扩展
  /// </summary>
  /// <remarks>
  /// 关联的登录扩展实体
  /// </remarks>
  [Navigate(NavigateType.OneToOne, nameof(UserId))]
  public virtual LeanLoginExtend LoginExtend { get; set; } = default!;
}