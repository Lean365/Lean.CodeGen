//===================================================
// 项目名: Lean.CodeGen.Domain
// 文件名: LeanRoleDept.cs
// 功能描述: 角色部门关联实体
// 创建时间: 2024-03-26
// 作者: Lean
// 版本: 1.0
//===================================================

using SqlSugar;

namespace Lean.CodeGen.Domain.Entities.Identity;

/// <summary>
/// 角色部门关联实体
/// </summary>
/// <remarks>
/// 用于数据权限控制，定义角色可以访问哪些部门的数据
/// </remarks>
[SugarTable("lean_id_role_dept", "角色部门关联表")]
[SugarIndex("pk_role_dept", nameof(RoleId), OrderByType.Asc, nameof(DeptId), OrderByType.Asc)]
public class LeanRoleDept : LeanBaseEntity
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
    /// 部门ID
    /// </summary>
    /// <remarks>
    /// 关联的部门ID
    /// </remarks>
    [SugarColumn(ColumnName = "dept_id", ColumnDescription = "部门ID", IsNullable = false, ColumnDataType = "bigint")]
    public long DeptId { get; set; }

    /// <summary>
    /// 角色
    /// </summary>
    /// <remarks>
    /// 关联的角色实体
    /// </remarks>
    [Navigate(NavigateType.OneToOne, nameof(RoleId))]
    public virtual LeanRole Role { get; set; } = default!;

    /// <summary>
    /// 部门
    /// </summary>
    /// <remarks>
    /// 关联的部门实体
    /// </remarks>
    [Navigate(NavigateType.OneToOne, nameof(DeptId))]
    public virtual LeanDept Dept { get; set; } = default!;
}