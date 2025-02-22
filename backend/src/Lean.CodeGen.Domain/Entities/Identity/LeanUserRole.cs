//===================================================
// 项目名: Lean.CodeGen.Domain
// 文件名: LeanUserRole.cs
// 功能描述: 用户角色关联实体
// 创建时间: 2024-03-26
// 作者: Lean
// 版本: 1.0
//===================================================

using SqlSugar;

namespace Lean.CodeGen.Domain.Entities.Identity;

/// <summary>
/// 用户角色关联实体
/// </summary>
[SugarTable("lean_user_role", "用户角色关联表")]
[SugarIndex("pk_user_role", nameof(UserId), OrderByType.Asc, nameof(RoleId), OrderByType.Asc)]
public class LeanUserRole : LeanBaseEntity
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
    /// 角色ID
    /// </summary>
    /// <remarks>
    /// 关联的角色ID
    /// </remarks>
    [SugarColumn(ColumnName = "role_id", ColumnDescription = "角色ID", IsNullable = false, ColumnDataType = "bigint")]
    public long RoleId { get; set; }

    /// <summary>
    /// 用户
    /// </summary>
    /// <remarks>
    /// 关联的用户实体
    /// </remarks>
    [Navigate(NavigateType.OneToOne, nameof(UserId))]
    public virtual LeanUser User { get; set; } = default!;

    /// <summary>
    /// 角色
    /// </summary>
    /// <remarks>
    /// 关联的角色实体
    /// </remarks>
    [Navigate(NavigateType.OneToOne, nameof(RoleId))]
    public virtual LeanRole Role { get; set; } = default!;
}