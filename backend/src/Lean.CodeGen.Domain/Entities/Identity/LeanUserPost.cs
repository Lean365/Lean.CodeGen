//===================================================
// 项目名: Lean.CodeGen.Domain
// 文件名: LeanUserPost.cs
// 功能描述: 用户岗位关联实体
// 创建时间: 2024-03-26
// 作者: Lean
// 版本: 1.0
//===================================================

using SqlSugar;

namespace Lean.CodeGen.Domain.Entities.Identity;

/// <summary>
/// 用户岗位关联
/// </summary>
[SugarTable("lean_user_post")]
public class LeanUserPost : LeanBaseEntity
{
  /// <summary>
  /// 用户ID
  /// </summary>
  public long UserId { get; set; }

  /// <summary>
  /// 岗位ID
  /// </summary>
  public long PostId { get; set; }

  /// <summary>
  /// 用户
  /// </summary>
  [Navigate(NavigateType.OneToOne, nameof(UserId))]
  public virtual LeanUser? User { get; set; }

  /// <summary>
  /// 岗位
  /// </summary>
  [Navigate(NavigateType.OneToOne, nameof(PostId))]
  public virtual LeanPost? Post { get; set; }
}