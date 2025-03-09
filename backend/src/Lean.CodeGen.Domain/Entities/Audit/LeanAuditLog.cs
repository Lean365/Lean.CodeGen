//===================================================
// 项目名: Lean.CodeGen.Domain
// 文件名: LeanAuditLog.cs
// 功能描述: 审计日志实体
// 创建时间: 2024-03-26
// 作者: Lean
// 版本: 1.0
//===================================================

using SqlSugar;
using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Domain.Entities.Identity;

namespace Lean.CodeGen.Domain.Entities.Audit;

/// <summary>
/// 审计日志实体
/// </summary>
[SugarTable("lean_mon_audit_log", "审计日志表")]
[SugarIndex("idx_user", nameof(UserId), OrderByType.Asc)]
[SugarIndex("idx_entity", nameof(EntityType), OrderByType.Asc)]
public class LeanAuditLog : LeanBaseEntity
{
  /// <summary>
  /// 用户ID
  /// </summary>
  /// <remarks>
  /// 操作用户的ID
  /// </remarks>
  [SugarColumn(ColumnName = "user_id", ColumnDescription = "用户ID", IsNullable = false, ColumnDataType = "bigint")]
  public long UserId { get; set; }

  /// <summary>
  /// 实体类型
  /// </summary>
  /// <remarks>
  /// 被操作的实体类型
  /// </remarks>
  [SugarColumn(ColumnName = "entity_type", ColumnDescription = "实体类型", Length = 100, IsNullable = false, ColumnDataType = "nvarchar")]
  public string EntityType { get; set; } = string.Empty;

  /// <summary>
  /// 实体ID
  /// </summary>
  /// <remarks>
  /// 被操作的实体ID
  /// </remarks>
  [SugarColumn(ColumnName = "entity_id", ColumnDescription = "实体ID", Length = 100, IsNullable = false, ColumnDataType = "nvarchar")]
  public string EntityId { get; set; } = string.Empty;

  /// <summary>
  /// 操作类型
  /// </summary>
  /// <remarks>
  /// 审计操作类型：Create-创建，Update-更新，Delete-删除，Other-其他
  /// </remarks>
  [SugarColumn(ColumnName = "operation_type", ColumnDescription = "操作类型", IsNullable = false, DefaultValue = "0", ColumnDataType = "int")]
  public int OperationType { get; set; }

  /// <summary>
  /// 操作描述
  /// </summary>
  /// <remarks>
  /// 操作的详细描述
  /// </remarks>
  [SugarColumn(ColumnName = "description", ColumnDescription = "操作描述", Length = 500, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? Description { get; set; }

  /// <summary>
  /// 请求IP
  /// </summary>
  /// <remarks>
  /// 发起请求的IP地址
  /// </remarks>
  [SugarColumn(ColumnName = "request_ip", ColumnDescription = "请求IP", Length = 50, IsNullable = false, ColumnDataType = "nvarchar")]
  public string RequestIp { get; set; } = string.Empty;

  /// <summary>
  /// 请求地点
  /// </summary>
  /// <remarks>
  /// 根据IP解析的请求地点
  /// </remarks>
  [SugarColumn(ColumnName = "request_location", ColumnDescription = "请求地点", Length = 100, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? RequestLocation { get; set; }

  /// <summary>
  /// 浏览器
  /// </summary>
  /// <remarks>
  /// 操作使用的浏览器信息
  /// </remarks>
  [SugarColumn(ColumnName = "browser", ColumnDescription = "浏览器", Length = 50, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? Browser { get; set; }

  /// <summary>
  /// 操作系统
  /// </summary>
  /// <remarks>
  /// 操作使用的操作系统信息
  /// </remarks>
  [SugarColumn(ColumnName = "os", ColumnDescription = "操作系统", Length = 50, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? Os { get; set; }

  /// <summary>
  /// 用户
  /// </summary>
  /// <remarks>
  /// 关联的用户实体
  /// </remarks>
  [Navigate(NavigateType.OneToOne, nameof(UserId))]
  public virtual LeanUser User { get; set; } = default!;

  /// <summary>
  /// 数据差异
  /// </summary>
  /// <remarks>
  /// 关联的数据差异记录
  /// </remarks>
  [Navigate(NavigateType.OneToMany, nameof(LeanSqlDiffLog.AuditLogId))]
  public virtual List<LeanSqlDiffLog> DataDiffs { get; set; } = new();
}