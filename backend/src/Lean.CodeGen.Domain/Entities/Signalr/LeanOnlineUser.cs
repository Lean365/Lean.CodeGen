//===================================================
// 项目名: Lean.CodeGen.Domain
// 文件名: HbtOnlineUser.cs
// 功能描述: 在线用户实体
// 创建时间: 2024-03-26
// 作者: Lean
// 版本: 1.0
//===================================================

using SqlSugar;

namespace Lean.CodeGen.Domain.Entities.Signalr;

/// <summary>
/// 在线用户实体
/// </summary>
[SugarTable("lean_online_user", "在线用户表")]
[SugarIndex("uk_connection", nameof(ConnectionId), OrderByType.Asc, true)]
public class LeanOnlineUser : LeanBaseEntity
{
  /// <summary>
  /// 连接ID
  /// </summary>
  /// <remarks>
  /// SignalR连接的唯一标识
  /// </remarks>
  [SugarColumn(ColumnName = "connection_id", ColumnDescription = "连接ID", Length = 100, IsNullable = false, UniqueGroupNameList = new[] { "uk_connection" }, ColumnDataType = "nvarchar")]
  public string ConnectionId { get; set; } = default!;

  /// <summary>
  /// 设备ID
  /// </summary>
  /// <remarks>
  /// 设备的唯一标识
  /// </remarks>
  [SugarColumn(ColumnName = "device_id", ColumnDescription = "设备ID", Length = 100, IsNullable = false, ColumnDataType = "nvarchar")]
  public string DeviceId { get; set; } = default!;

  /// <summary>
  /// 用户ID
  /// </summary>
  /// <remarks>
  /// 关联的用户ID
  /// </remarks>
  [SugarColumn(ColumnName = "user_id", ColumnDescription = "用户ID", IsNullable = false, ColumnDataType = "bigint")]
  public long UserId { get; set; }

  /// <summary>
  /// 用户名
  /// </summary>
  /// <remarks>
  /// 用户显示名称
  /// </remarks>
  [SugarColumn(ColumnName = "user_name", ColumnDescription = "用户名", Length = 50, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? UserName { get; set; }

  /// <summary>
  /// 头像
  /// </summary>
  /// <remarks>
  /// 用户头像URL
  /// </remarks>
  [SugarColumn(ColumnName = "avatar", ColumnDescription = "头像", Length = 500, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? Avatar { get; set; }

  /// <summary>
  /// 最后活动时间
  /// </summary>
  /// <remarks>
  /// 用户最后一次活动的时间
  /// </remarks>
  [SugarColumn(ColumnName = "last_active_time", ColumnDescription = "最后活动时间", IsNullable = false, ColumnDataType = "datetime")]
  public DateTime LastActiveTime { get; set; }

  /// <summary>
  /// IP地址
  /// </summary>
  /// <remarks>
  /// 用户连接的IP地址
  /// </remarks>
  [SugarColumn(ColumnName = "ip_address", ColumnDescription = "IP地址", Length = 50, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? IpAddress { get; set; }

  /// <summary>
  /// 浏览器
  /// </summary>
  /// <remarks>
  /// 用户使用的浏览器信息
  /// </remarks>
  [SugarColumn(ColumnName = "browser", ColumnDescription = "浏览器", Length = 500, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? Browser { get; set; }

  /// <summary>
  /// 操作系统
  /// </summary>
  /// <remarks>
  /// 用户使用的操作系统信息
  /// </remarks>
  [SugarColumn(ColumnName = "os", ColumnDescription = "操作系统", Length = 500, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? Os { get; set; }

  /// <summary>
  /// 是否在线
  /// </summary>
  /// <remarks>
  /// 用户是否处于在线状态
  /// 0-离线
  /// 1-在线
  /// </remarks>
  [SugarColumn(ColumnName = "is_online", ColumnDescription = "是否在线", IsNullable = false, DefaultValue = "1", ColumnDataType = "int")]
  public int IsOnline { get; set; }
}