//===================================================
// 项目名: Lean.CodeGen.Domain
// 文件名: LeanRole.cs
// 功能描述: 角色身份实体
// 创建时间: 2024-03-26
// 作者: Lean
// 版本: 1.0
//===================================================

using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Domain.Entities;
using SqlSugar;

namespace Lean.CodeGen.Domain.Entities.Identity;

/// <summary>
/// 角色身份实体
/// </summary>
[SugarTable("lean_role", "角色身份表")]
[SugarIndex("uk_code", nameof(RoleCode), OrderByType.Asc, true)]
public class LeanRole : LeanBaseEntity
{
  /// <summary>
  /// 父级ID
  /// </summary>
  /// <remarks>
  /// 上级角色的ID，用于构建角色继承关系
  /// 下级角色自动继承上级角色的所有权限
  /// </remarks>
  [SugarColumn(ColumnName = "parent_id", ColumnDescription = "父级ID", IsNullable = true, ColumnDataType = "bigint")]
  public long? ParentId { get; set; }

  /// <summary>
  /// 角色名称
  /// </summary>
  /// <remarks>
  /// 角色的显示名称，如：系统管理员、普通用户等
  /// </remarks>
  [SugarColumn(ColumnName = "role_name", ColumnDescription = "角色名称", Length = 50, IsNullable = false, ColumnDataType = "nvarchar")]
  public string RoleName { get; set; } = default!;

  /// <summary>
  /// 角色编码
  /// </summary>
  /// <remarks>
  /// 角色的唯一标识符，如：admin、user等
  /// </remarks>
  [SugarColumn(ColumnName = "role_code", ColumnDescription = "角色编码", Length = 50, IsNullable = false, UniqueGroupNameList = new[] { "uk_code" }, ColumnDataType = "nvarchar")]
  public string RoleCode { get; set; } = default!;

  /// <summary>
  /// 角色描述
  /// </summary>
  /// <remarks>
  /// 对角色的详细说明
  /// </remarks>
  [SugarColumn(ColumnName = "role_description", ColumnDescription = "角色描述", Length = 500, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? RoleDescription { get; set; }

  /// <summary>
  /// 排序号
  /// </summary>
  /// <remarks>
  /// 角色在列表中的显示顺序，数值越小越靠前
  /// </remarks>
  [SugarColumn(ColumnName = "order_num", ColumnDescription = "排序号", IsNullable = false, DefaultValue = "0", ColumnDataType = "int")]
  public int OrderNum { get; set; }

  /// <summary>
  /// 状态
  /// </summary>
  /// <remarks>
  /// 角色状态：0-正常，1-禁用
  /// </remarks>
  [SugarColumn(ColumnName = "role_status", ColumnDescription = "状态", IsNullable = false, DefaultValue = "0", ColumnDataType = "int")]
  public LeanRoleStatus RoleStatus { get; set; }

  /// <summary>
  /// 数据权限范围
  /// </summary>
  /// <remarks>
  /// 角色的数据权限范围类型
  /// </remarks>
  [SugarColumn(ColumnName = "data_scope", ColumnDescription = "数据权限范围", IsNullable = false, DefaultValue = "5", ColumnDataType = "int")]
  public LeanDataScopeType DataScope { get; set; }

  /// <summary>
  /// 是否内置
  /// </summary>
  /// <remarks>
  /// 是否为系统内置角色：No-否，Yes-是
  /// 内置角色不允许删除
  /// </remarks>
  [SugarColumn(ColumnName = "is_builtin", ColumnDescription = "是否内置", IsNullable = false, DefaultValue = "0", ColumnDataType = "int")]
  public LeanBuiltinStatus IsBuiltin { get; set; }

  /// <summary>
  /// 用户角色关联
  /// </summary>
  /// <remarks>
  /// 角色与用户的多对多关系
  /// </remarks>
  [Navigate(NavigateType.OneToMany, nameof(LeanUserRole.RoleId))]
  public virtual ICollection<LeanUserRole> UserRoles { get; set; } = new List<LeanUserRole>();

  /// <summary>
  /// 角色部门关联
  /// </summary>
  /// <remarks>
  /// 角色与部门的多对多关系
  /// </remarks>
  [Navigate(NavigateType.OneToMany, nameof(LeanRoleDept.RoleId))]
  public virtual ICollection<LeanRoleDept> RoleDepts { get; set; } = new List<LeanRoleDept>();

  /// <summary>
  /// 角色菜单关联
  /// </summary>
  /// <remarks>
  /// 角色与菜单的多对多关系
  /// </remarks>
  [Navigate(NavigateType.OneToMany, nameof(LeanRoleMenu.RoleId))]
  public virtual ICollection<LeanRoleMenu> RoleMenus { get; set; } = new List<LeanRoleMenu>();
}