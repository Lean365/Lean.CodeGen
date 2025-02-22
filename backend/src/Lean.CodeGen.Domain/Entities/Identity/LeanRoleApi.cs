//===================================================
// 项目名: Lean.CodeGen.Domain
// 文件名: LeanRoleApi.cs
// 功能描述: 角色API关联实体
// 创建时间: 2024-03-26
// 作者: Lean
// 版本: 1.0
//===================================================

using SqlSugar;

namespace Lean.CodeGen.Domain.Entities.Identity;

/// <summary>
/// 角色API关联实体
/// </summary>
[SugarTable("lean_role_api", "角色API关联表")]
[SugarIndex("uk_role_api", $"{nameof(RoleId)},{nameof(ApiId)}", OrderByType.Asc, true)]
public class LeanRoleApi : LeanBaseEntity
{
  /// <summary>
  /// 角色ID
  /// </summary>
  /// <remarks>
  /// 关联的角色ID
  /// </remarks>
  [SugarColumn(ColumnName = "role_id", ColumnDescription = "角色ID", IsNullable = false, ColumnDataType = "bigint")]
  public long RoleId { get; set; }

  /// <summary>
  /// API ID
  /// </summary>
  /// <remarks>
  /// 关联的API ID
  /// </remarks>
  [SugarColumn(ColumnName = "api_id", ColumnDescription = "API ID", IsNullable = false, ColumnDataType = "bigint")]
  public long ApiId { get; set; }

  /// <summary>
  /// 角色
  /// </summary>
  /// <remarks>
  /// 关联的角色实体
  /// </remarks>
  [Navigate(NavigateType.OneToOne, nameof(RoleId))]
  public virtual LeanRole Role { get; set; } = default!;

  /// <summary>
  /// API
  /// </summary>
  /// <remarks>
  /// 关联的API实体
  /// </remarks>
  [Navigate(NavigateType.OneToOne, nameof(ApiId))]
  public virtual LeanApi Api { get; set; } = default!;
}