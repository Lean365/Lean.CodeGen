//===================================================
// 项目名: Lean.CodeGen.Domain
// 文件名: LeanUserDept.cs
// 功能描述: 用户部门关联实体
// 创建时间: 2024-03-26
// 作者: Lean
// 版本: 1.0
//===================================================

using Lean.CodeGen.Common.Enums;
using SqlSugar;

namespace Lean.CodeGen.Domain.Entities.Identity;

/// <summary>
/// 用户部门关联实体
/// </summary>
[SugarTable("lean_user_dept", "用户部门关联表")]
[SugarIndex("pk_user_dept", nameof(UserId), OrderByType.Asc)]
public class LeanUserDept : LeanBaseEntity
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
  /// 部门ID
  /// </summary>
  /// <remarks>
  /// 关联的部门ID
  /// </remarks>
  [SugarColumn(ColumnName = "dept_id", ColumnDescription = "部门ID", IsNullable = false, ColumnDataType = "bigint")]
  public long DeptId { get; set; }

  /// <summary>
  /// 是否主部门
  /// </summary>
  /// <remarks>
  /// 是否为用户的主部门：No-否，Yes-是
  /// 一个用户只能有一个主部门
  /// </remarks>
  [SugarColumn(ColumnName = "is_primary", ColumnDescription = "是否主部门", IsNullable = false, DefaultValue = "0", ColumnDataType = "int")]
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
  /// 部门
  /// </summary>
  /// <remarks>
  /// 关联的部门实体
  /// </remarks>
  [Navigate(NavigateType.OneToOne, nameof(DeptId))]
  public virtual LeanDept Dept { get; set; } = default!;
}