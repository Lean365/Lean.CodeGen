//===================================================
// 项目名: Lean.CodeGen.Domain
// 文件名: LeanUserSession.cs
// 功能描述: 用户会话实体
// 创建时间: 2024-03-26
// 作者: Lean
// 版本: 1.0
//===================================================

using SqlSugar;

namespace Lean.CodeGen.Domain.Entities.Identity;

/// <summary>
/// 用户会话实体
/// </summary>
/// <remarks>
/// 用于记录用户的会话信息，支持会话级别的权限激活/停用
/// </remarks>
[SugarTable("lean_user_session", "用户会话表")]
[SugarIndex("idx_user", nameof(UserId), OrderByType.Asc)]
[SugarIndex("idx_token", nameof(Token), OrderByType.Asc)]
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
  public string Token { get; set; } = default!;

  /// <summary>
  /// 登录IP
  /// </summary>
  /// <remarks>
  /// 用户登录时的IP地址
  /// </remarks>
  [SugarColumn(ColumnName = "login_ip", ColumnDescription = "登录IP", Length = 50, IsNullable = false, ColumnDataType = "nvarchar")]
  public string LoginIp { get; set; } = default!;

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
  [SugarColumn(ColumnName = "browser", ColumnDescription = "浏览器", Length = 100, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? Browser { get; set; }

  /// <summary>
  /// 操作系统
  /// </summary>
  /// <remarks>
  /// 用户使用的操作系统信息
  /// </remarks>
  [SugarColumn(ColumnName = "os", ColumnDescription = "操作系统", Length = 100, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? Os { get; set; }

  /// <summary>
  /// 过期时间
  /// </summary>
  /// <remarks>
  /// 会话的过期时间
  /// </remarks>
  [SugarColumn(ColumnName = "expire_time", ColumnDescription = "过期时间", IsNullable = false, ColumnDataType = "datetime")]
  public DateTime ExpireTime { get; set; }

  /// <summary>
  /// 刷新令牌
  /// </summary>
  /// <remarks>
  /// 用于刷新会话的令牌
  /// </remarks>
  [SugarColumn(ColumnName = "refresh_token", ColumnDescription = "刷新令牌", Length = 100, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? RefreshToken { get; set; }

  /// <summary>
  /// 激活的角色
  /// </summary>
  /// <remarks>
  /// 当前会话中激活的角色ID列表，用逗号分隔
  /// </remarks>
  [SugarColumn(ColumnName = "active_roles", ColumnDescription = "激活的角色", Length = 500, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? ActiveRoles { get; set; }

  /// <summary>
  /// 用户
  /// </summary>
  /// <remarks>
  /// 关联的用户实体
  /// </remarks>
  [Navigate(NavigateType.OneToOne, nameof(UserId))]
  public virtual LeanUser User { get; set; } = default!;
}