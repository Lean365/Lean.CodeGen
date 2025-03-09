//===================================================
// 项目名: Lean.CodeGen.Domain
// 文件名: LeanDept.cs
// 功能描述: 部门实体
// 创建时间: 2024-03-26
// 作者: Lean
// 版本: 1.0
//===================================================

using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Domain.Entities;
using SqlSugar;

namespace Lean.CodeGen.Domain.Entities.Identity;

/// <summary>
/// 部门实体
/// </summary>
[SugarTable("lean_id_dept", "部门表")]
[SugarIndex("uk_code", nameof(DeptCode), OrderByType.Asc, true)]
public class LeanDept : LeanBaseEntity
{
    /// <summary>
    /// 父级ID
    /// </summary>
    /// <remarks>
    /// 上级部门的ID，用于构建部门树结构
    /// </remarks>
    [SugarColumn(ColumnName = "parent_id", ColumnDescription = "父级ID", IsNullable = true, ColumnDataType = "bigint")]
    public long? ParentId { get; set; }

    /// <summary>
    /// 部门名称
    /// </summary>
    /// <remarks>
    /// 部门的显示名称，如：技术部、人事部等
    /// </remarks>
    [SugarColumn(ColumnName = "dept_name", ColumnDescription = "部门名称", Length = 50, IsNullable = false, ColumnDataType = "nvarchar")]
    public string DeptName { get; set; } = default!;

    /// <summary>
    /// 部门编码
    /// </summary>
    /// <remarks>
    /// 部门的唯一标识符，如：tech、hr等
    /// </remarks>
    [SugarColumn(ColumnName = "dept_code", ColumnDescription = "部门编码", Length = 50, IsNullable = false, UniqueGroupNameList = new[] { "uk_code" }, ColumnDataType = "nvarchar")]
    public string DeptCode { get; set; } = default!;

    /// <summary>
    /// 部门描述
    /// </summary>
    /// <remarks>
    /// 对部门的详细说明
    /// </remarks>
    [SugarColumn(ColumnName = "dept_description", ColumnDescription = "部门描述", Length = 500, IsNullable = true, ColumnDataType = "nvarchar")]
    public string? DeptDescription { get; set; }

    /// <summary>
    /// 负责人
    /// </summary>
    /// <remarks>
    /// 部门负责人姓名
    /// </remarks>
    [SugarColumn(ColumnName = "leader", ColumnDescription = "负责人", Length = 50, IsNullable = true, ColumnDataType = "nvarchar")]
    public string? Leader { get; set; }

    /// <summary>
    /// 联系电话
    /// </summary>
    /// <remarks>
    /// 部门联系电话
    /// </remarks>
    [SugarColumn(ColumnName = "phone", ColumnDescription = "联系电话", Length = 20, IsNullable = true, ColumnDataType = "nvarchar")]
    public string? Phone { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    /// <remarks>
    /// 部门联系邮箱
    /// </remarks>
    [SugarColumn(ColumnName = "email", ColumnDescription = "邮箱", Length = 100, IsNullable = true, ColumnDataType = "nvarchar")]
    public string? Email { get; set; }

    /// <summary>
    /// 排序号
    /// </summary>
    /// <remarks>
    /// 显示顺序，数值越小越靠前
    /// </remarks>
    [SugarColumn(ColumnName = "order_num", ColumnDescription = "排序号", IsNullable = false, DefaultValue = "0", ColumnDataType = "int")]
    public int OrderNum { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    /// <remarks>
    /// 部门状态：0-正常，1-禁用
    /// </remarks>
    [SugarColumn(ColumnName = "dept_status", ColumnDescription = "状态", IsNullable = false, DefaultValue = "0", ColumnDataType = "int")]
    public int DeptStatus { get; set; }

    /// <summary>
    /// 是否内置
    /// </summary>
    /// <remarks>
    /// 是否为系统内置部门：0-否，1-是
    /// 内置部门不允许删除
    /// </remarks>
    [SugarColumn(ColumnName = "is_builtin", ColumnDescription = "是否内置", IsNullable = false, DefaultValue = "0", ColumnDataType = "int")]
    public int IsBuiltin { get; set; }

    /// <summary>
    /// 数据权限范围
    /// </summary>
    /// <remarks>
    /// 部门的数据权限范围类型：0-全部数据权限，1-本部门数据权限，2-本部门及以下数据权限，3-本人数据权限，4-自定义数据权限，5-仅本人数据权限
    /// </remarks>
    [SugarColumn(ColumnName = "data_scope", ColumnDescription = "数据权限范围", IsNullable = false, DefaultValue = "5", ColumnDataType = "int")]
    public int DataScope { get; set; } = 5;

    /// <summary>
    /// 角色部门关联
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(LeanRoleDept.DeptId))]
    public virtual ICollection<LeanRoleDept> RoleDepts { get; set; } = new List<LeanRoleDept>();
}