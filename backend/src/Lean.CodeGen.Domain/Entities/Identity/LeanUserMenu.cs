//===================================================
// 项目名: Lean.CodeGen.Domain
// 文件名: LeanUserMenu.cs
// 功能描述: 用户菜单关联实体
// 创建时间: 2024-03-26
// 作者: Lean
// 版本: 1.0
//===================================================

using SqlSugar;

namespace Lean.CodeGen.Domain.Entities.Identity;

/// <summary>
/// 用户菜单关联实体
/// </summary>
[SugarTable("lean_user_menu", "用户菜单关联表")]
[SugarIndex("uk_user_menu", nameof(UserId), OrderByType.Asc, nameof(MenuId), OrderByType.Asc)]
public class LeanUserMenu : LeanBaseEntity
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
    /// 菜单ID
    /// </summary>
    /// <remarks>
    /// 关联的菜单ID
    /// </remarks>
    [SugarColumn(ColumnName = "menu_id", ColumnDescription = "菜单ID", IsNullable = false, ColumnDataType = "bigint")]
    public long MenuId { get; set; }

    /// <summary>
    /// 用户
    /// </summary>
    /// <remarks>
    /// 关联的用户实体
    /// </remarks>
    [Navigate(NavigateType.OneToOne, nameof(UserId))]
    public virtual LeanUser User { get; set; } = default!;

    /// <summary>
    /// 菜单
    /// </summary>
    /// <remarks>
    /// 关联的菜单实体
    /// </remarks>
    [Navigate(NavigateType.OneToOne, nameof(MenuId))]
    public virtual LeanMenu Menu { get; set; } = default!;
}