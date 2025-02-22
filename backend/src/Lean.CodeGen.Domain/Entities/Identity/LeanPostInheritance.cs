//===================================================
// 项目名: Lean.CodeGen.Domain
// 文件名: LeanPostInheritance.cs
// 功能描述: 岗位继承关系实体
// 创建时间: 2024-03-26
// 作者: Lean
// 版本: 1.0
//===================================================

using SqlSugar;

namespace Lean.CodeGen.Domain.Entities.Identity;

/// <summary>
/// 岗位继承关系实体
/// </summary>
[SugarTable("lean_post_inheritance", "岗位继承关系表")]
[SugarIndex("uk_post_inheritance", $"{nameof(PostId)},{nameof(InheritedPostId)}", OrderByType.Asc, true)]
public class LeanPostInheritance : LeanBaseEntity
{
  /// <summary>
  /// 岗位ID
  /// </summary>
  /// <remarks>
  /// 继承其他岗位权限的岗位ID
  /// </remarks>
  [SugarColumn(ColumnName = "post_id", ColumnDescription = "岗位ID", IsNullable = false, ColumnDataType = "bigint")]
  public long PostId { get; set; }

  /// <summary>
  /// 被继承的岗位ID
  /// </summary>
  /// <remarks>
  /// 被继承权限的岗位ID
  /// </remarks>
  [SugarColumn(ColumnName = "inherited_post_id", ColumnDescription = "被继承的岗位ID", IsNullable = false, ColumnDataType = "bigint")]
  public long InheritedPostId { get; set; }

  /// <summary>
  /// 岗位
  /// </summary>
  /// <remarks>
  /// 关联的岗位实体
  /// </remarks>
  [Navigate(NavigateType.OneToOne, nameof(PostId))]
  public virtual LeanPost Post { get; set; } = default!;

  /// <summary>
  /// 被继承的岗位
  /// </summary>
  /// <remarks>
  /// 关联的被继承岗位实体
  /// </remarks>
  [Navigate(NavigateType.OneToOne, nameof(InheritedPostId))]
  public virtual LeanPost InheritedPost { get; set; } = default!;
}