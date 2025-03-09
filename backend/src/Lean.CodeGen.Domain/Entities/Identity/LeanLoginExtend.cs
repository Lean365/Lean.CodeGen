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
    /// 最后密码错误时间
    /// </summary>
    /// <remarks>
    /// 最后一次密码错误的时间，用于计算锁定时间
    /// </remarks>
    [SugarColumn(ColumnName = "last_password_error_time", ColumnDescription = "最后密码错误时间", IsNullable = true, ColumnDataType = "datetime")]
    public DateTime? LastPasswordErrorTime { get; set; }

    /// <summary>
    /// 密码修改时间
    /// </summary>
    /// <remarks>
    /// 最后一次修改密码的时间
    /// </remarks>
    [SugarColumn(ColumnName = "password_update_time", ColumnDescription = "密码修改时间", IsNullable = true, ColumnDataType = "datetime")]
    public DateTime? PasswordUpdateTime { get; set; }

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