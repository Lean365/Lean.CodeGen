//===================================================
// 项目名: Lean.CodeGen.Domain
// 文件名: LeanUserSession.cs
// 功能描述: 用户会话实体
// 创建时间: 2024-03-26
// 作者: Lean
// 版本: 1.0
//===================================================

using SqlSugar;
using Lean.CodeGen.Common.Enums;

namespace Lean.CodeGen.Domain.Entities.Identity;

/// <summary>
/// 用户会话实体
/// </summary>
/// <remarks>
/// 用于记录用户的会话信息，支持会话级别的权限激活/停用
/// </remarks>
[SugarTable("lean_id_user_session", "用户会话表")]
[SugarIndex("idx_user", nameof(UserId), OrderByType.Asc)]
[SugarIndex("idx_token", nameof(Token), OrderByType.Asc)]
[SugarIndex("idx_device", nameof(DeviceId), OrderByType.Asc)]
public class LeanUserSession : LeanBaseEntity
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
    /// 会话令牌
    /// </summary>
    /// <remarks>
    /// 用户会话的唯一标识符
    /// </remarks>
    [SugarColumn(ColumnName = "token", ColumnDescription = "会话令牌", Length = 100, IsNullable = false, ColumnDataType = "nvarchar")]
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// 刷新令牌
    /// </summary>
    /// <remarks>
    /// 用于刷新会话的令牌
    /// </remarks>
    [SugarColumn(ColumnName = "refresh_token", ColumnDescription = "刷新令牌", Length = 100, IsNullable = true, ColumnDataType = "nvarchar")]
    public string? RefreshToken { get; set; }

    /// <summary>
    /// 过期时间
    /// </summary>
    /// <remarks>
    /// 会话的过期时间
    /// </remarks>
    [SugarColumn(ColumnName = "expire_time", ColumnDescription = "过期时间", IsNullable = false, ColumnDataType = "datetime")]
    public DateTime ExpireTime { get; set; }

    /// <summary>
    /// 设备ID
    /// </summary>
    /// <remarks>
    /// 设备的唯一标识符
    /// </remarks>
    [SugarColumn(ColumnName = "device_id", ColumnDescription = "设备ID", Length = 50, IsNullable = false, ColumnDataType = "nvarchar")]
    public string DeviceId { get; set; } = string.Empty;

    /// <summary>
    /// 设备名称
    /// </summary>
    /// <remarks>
    /// 设备的名称
    /// </remarks>
    [SugarColumn(ColumnName = "device_name", ColumnDescription = "设备名称", Length = 100, IsNullable = true, ColumnDataType = "nvarchar")]
    public string? DeviceName { get; set; }

    /// <summary>
    /// 设备类型
    /// </summary>
    /// <remarks>
    /// 设备的类型：0-PC，1-Mobile，2-Tablet，3-Other
    /// </remarks>
    [SugarColumn(ColumnName = "device_type", ColumnDescription = "设备类型", IsNullable = false, DefaultValue = "0", ColumnDataType = "int")]
    public int DeviceType { get; set; }

    /// <summary>
    /// 是否信任设备
    /// 0-否
    /// 1-是
    /// </summary>
    [SugarColumn(ColumnName = "is_trusted", ColumnDescription = "是否信任设备", IsNullable = false, DefaultValue = "0", ColumnDataType = "int")]
    public int IsTrusted { get; set; }

    /// <summary>
    /// 设备状态
    /// </summary>
    /// <remarks>
    /// 设备的当前状态：0-正常，1-禁用，2-锁定
    /// </remarks>
    [SugarColumn(ColumnName = "device_status", ColumnDescription = "设备状态", IsNullable = false, DefaultValue = "0", ColumnDataType = "int")]
    public int DeviceStatus { get; set; }

    /// <summary>
    /// 登录IP
    /// </summary>
    /// <remarks>
    /// 用户登录时的IP地址
    /// </remarks>
    [SugarColumn(ColumnName = "login_ip", ColumnDescription = "登录IP", Length = 50, IsNullable = true, ColumnDataType = "nvarchar")]
    public string? LoginIp { get; set; }

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
    /// 用户使用的浏览器信息
    /// </remarks>
    [SugarColumn(ColumnName = "browser", ColumnDescription = "浏览器", Length = 50, IsNullable = true, ColumnDataType = "nvarchar")]
    public string? Browser { get; set; }

    /// <summary>
    /// 操作系统
    /// </summary>
    /// <remarks>
    /// 用户使用的操作系统信息
    /// </remarks>
    [SugarColumn(ColumnName = "os", ColumnDescription = "操作系统", Length = 50, IsNullable = true, ColumnDataType = "nvarchar")]
    public string? Os { get; set; }

    /// <summary>
    /// 登录方式
    /// </summary>
    /// <remarks>
    /// 用户登录的方式：0-账号密码，1-手机验证码，2-邮箱验证码，3-第三方登录
    /// </remarks>
    [SugarColumn(ColumnName = "login_type", ColumnDescription = "登录方式", IsNullable = true, ColumnDataType = "int")]
    public int? LoginType { get; set; }

    /// <summary>
    /// 登录状态
    /// </summary>
    /// <remarks>
    /// 用户登录的状态：0-正常，1-锁定，2-禁用
    /// </remarks>
    [SugarColumn(ColumnName = "login_status", ColumnDescription = "登录状态", IsNullable = false, DefaultValue = "0", ColumnDataType = "int")]
    public int LoginStatus { get; set; }

    /// <summary>
    /// 密码错误次数
    /// </summary>
    /// <remarks>
    /// 用户密码错误的次数
    /// </remarks>
    [SugarColumn(ColumnName = "password_error_count", ColumnDescription = "密码错误次数", IsNullable = false, DefaultValue = "0", ColumnDataType = "int")]
    public int PasswordErrorCount { get; set; }

    /// <summary>
    /// 最后密码错误时间
    /// </summary>
    /// <remarks>
    /// 用户最后一次密码错误的时间
    /// </remarks>
    [SugarColumn(ColumnName = "last_password_error_time", ColumnDescription = "最后密码错误时间", IsNullable = true, ColumnDataType = "datetime")]
    public DateTime? LastPasswordErrorTime { get; set; }

    /// <summary>
    /// 密码修改时间
    /// </summary>
    /// <remarks>
    /// 用户最后一次修改密码的时间
    /// </remarks>
    [SugarColumn(ColumnName = "password_update_time", ColumnDescription = "密码修改时间", IsNullable = true, ColumnDataType = "datetime")]
    public DateTime? PasswordUpdateTime { get; set; }

    /// <summary>
    /// 活动角色列表
    /// </summary>
    /// <remarks>
    /// 当前会话中激活的角色ID列表，用逗号分隔
    /// </remarks>
    [SugarColumn(ColumnName = "active_roles", ColumnDescription = "活动角色列表", IsJson = true, ColumnDataType = "nvarchar(max)")]
    public List<long> ActiveRoles { get; set; } = [];

    /// <summary>
    /// 用户
    /// </summary>
    /// <remarks>
    /// 关联的用户实体
    /// </remarks>
    [Navigate(NavigateType.OneToOne, nameof(UserId))]
    public virtual LeanUser User { get; set; } = default!;
}