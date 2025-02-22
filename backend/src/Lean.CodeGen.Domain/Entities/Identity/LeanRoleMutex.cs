//===================================================
// 项目名: Lean.CodeGen.Domain
// 文件名: LeanRoleMutex.cs
// 功能描述: 角色互斥关系实体
// 创建时间: 2024-03-26
// 作者: Lean
// 版本: 1.0
//===================================================

using SqlSugar;

namespace Lean.CodeGen.Domain.Entities.Identity;

/// <summary>
/// 角色互斥关系实体
/// </summary>
/// <remarks>
/// 用于定义角色之间的互斥关系，互斥的角色不能同时分配给同一个用户
/// </remarks>
[SugarTable("lean_role_mutex", "角色互斥关系表")]
[SugarIndex("pk_role_mutex", nameof(RoleId), OrderByType.Asc, nameof(MutexRoleId), OrderByType.Asc)]
public class LeanRoleMutex : LeanBaseEntity
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
    /// 互斥角色ID
    /// </summary>
    /// <remarks>
    /// 与之互斥的角色ID
    /// </remarks>
    [SugarColumn(ColumnName = "mutex_role_id", ColumnDescription = "互斥角色ID", IsNullable = false, ColumnDataType = "bigint")]
    public long MutexRoleId { get; set; }

    /// <summary>
    /// 角色
    /// </summary>
    /// <remarks>
    /// 关联的角色实体
    /// </remarks>
    [Navigate(NavigateType.OneToOne, nameof(RoleId))]
    public virtual LeanRole Role { get; set; } = default!;

    /// <summary>
    /// 互斥角色
    /// </summary>
    /// <remarks>
    /// 关联的互斥角色实体
    /// </remarks>
    [Navigate(NavigateType.OneToOne, nameof(MutexRoleId))]
    public virtual LeanRole MutexRole { get; set; } = default!;
}