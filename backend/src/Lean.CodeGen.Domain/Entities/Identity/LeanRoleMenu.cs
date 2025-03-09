//===================================================
// 项目名: Lean.CodeGen.Domain
// 文件名: LeanRoleMenu.cs
// 功能描述: 角色菜单关联实体
// 创建时间: 2024-03-26
// 作者: Lean
// 版本: 1.0
//===================================================

using SqlSugar;

namespace Lean.CodeGen.Domain.Entities.Identity;

/// <summary>
/// 角色菜单关联实体
/// </summary>
[SugarTable("lean_id_role_menu", "角色菜单关联表")]
[SugarIndex("pk_role_menu", nameof(RoleId), OrderByType.Asc, nameof(MenuId), OrderByType.Asc)]
public class LeanRoleMenu : LeanBaseEntity
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
    /// 菜单ID
    /// </summary>
    /// <remarks>
    /// 关联的菜单ID
    /// </remarks>
    [SugarColumn(ColumnName = "menu_id", ColumnDescription = "菜单ID", IsNullable = false, ColumnDataType = "bigint")]
    public long MenuId { get; set; }

    /// <summary>
    /// 角色
    /// </summary>
    /// <remarks>
    /// 关联的角色实体
    /// </remarks>
    [Navigate(NavigateType.OneToOne, nameof(RoleId))]
    public virtual LeanRole Role { get; set; } = default!;

    /// <summary>
    /// 菜单
    /// </summary>
    /// <remarks>
    /// 关联的菜单实体
    /// </remarks>
    [Navigate(NavigateType.OneToOne, nameof(MenuId))]
    public virtual LeanMenu Menu { get; set; } = default!;
}