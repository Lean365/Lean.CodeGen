//===================================================
// 项目名: Lean.CodeGen.Domain
// 文件名: LeanUserPost.cs
// 功能描述: 用户岗位关联实体
// 创建时间: 2024-03-26
// 作者: Lean
// 版本: 1.0
//===================================================

using Lean.CodeGen.Common.Enums;
using SqlSugar;

namespace Lean.CodeGen.Domain.Entities.Identity;

/// <summary>
/// 用户岗位关联实体
/// </summary>
[SugarTable("lean_user_post", "用户岗位关联表")]
[SugarIndex("uk_user_post", $"{nameof(UserId)},{nameof(PostId)}", OrderByType.Asc, true)]
public class LeanUserPost : LeanBaseEntity
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
  /// 岗位ID
  /// </summary>
  /// <remarks>
  /// 关联的岗位ID
  /// </remarks>
  [SugarColumn(ColumnName = "post_id", ColumnDescription = "岗位ID", IsNullable = false, ColumnDataType = "bigint")]
  public long PostId { get; set; }

  /// <summary>
  /// 是否主岗位
  /// </summary>
  /// <remarks>
  /// 是否为用户的主岗位：No-否，Yes-是
  /// </remarks>
  [SugarColumn(ColumnName = "is_primary", ColumnDescription = "是否主岗位", IsNullable = false, DefaultValue = "0", ColumnDataType = "int")]
  public LeanPrimaryStatus IsPrimary { get; set; }

  /// <summary>
  /// 用户
  /// </summary>
  /// <remarks>
  /// 关联的用户实体
  /// </remarks>
  [Navigate(NavigateType.OneToOne, nameof(UserId))]
  public virtual LeanUser User { get; set; } = default!;

  /// <summary>
  /// 岗位
  /// </summary>
  /// <remarks>
  /// 关联的岗位实体
  /// </remarks>
  [Navigate(NavigateType.OneToOne, nameof(PostId))]
  public virtual LeanPost Post { get; set; } = default!;
}