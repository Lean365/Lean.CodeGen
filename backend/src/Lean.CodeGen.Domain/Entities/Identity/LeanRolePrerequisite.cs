//===================================================
// 项目名: Lean.CodeGen.Domain
// 文件名: LeanRolePrerequisite.cs
// 功能描述: 角色前置条件实体
// 创建时间: 2024-03-26
// 作者: Lean
// 版本: 1.0
//===================================================

using SqlSugar;

namespace Lean.CodeGen.Domain.Entities.Identity;

/// <summary>
/// 角色前置条件实体
/// </summary>
/// <remarks>
/// 用于定义角色的前置条件，只有拥有前置角色才能被分配目标角色
/// </remarks>
[SugarTable("lean_role_prerequisite", "角色前置条件表")]
[SugarIndex("pk_role_prerequisite", nameof(RoleId), OrderByType.Asc)]
public class LeanRolePrerequisite : LeanBaseEntity
{
  /// <summary>
  /// 角色ID
  /// </summary>
  /// <remarks>
  /// 目标角色ID
  /// </remarks>
  [SugarColumn(ColumnName = "role_id", ColumnDescription = "角色ID", IsNullable = false, ColumnDataType = "bigint")]
  public long RoleId { get; set; }

  /// <summary>
  /// 前置角色ID
  /// </summary>
  /// <remarks>
  /// 前置条件角色ID
  /// </remarks>
  [SugarColumn(ColumnName = "prerequisite_role_id", ColumnDescription = "前置角色ID", IsNullable = false, ColumnDataType = "bigint")]
  public long PrerequisiteRoleId { get; set; }

  /// <summary>
  /// 角色
  /// </summary>
  /// <remarks>
  /// 关联的目标角色实体
  /// </remarks>
  [Navigate(NavigateType.OneToOne, nameof(RoleId))]
  public virtual LeanRole Role { get; set; } = default!;

  /// <summary>
  /// 前置角色
  /// </summary>
  /// <remarks>
  /// 关联的前置角色实体
  /// </remarks>
  [Navigate(NavigateType.OneToOne, nameof(PrerequisiteRoleId))]
  public virtual LeanRole PrerequisiteRole { get; set; } = default!;
}