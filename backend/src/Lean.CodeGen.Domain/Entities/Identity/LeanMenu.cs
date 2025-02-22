//===================================================
// 项目名: Lean.CodeGen.Domain
// 文件名: LeanMenu.cs
// 功能描述: 菜单实体
// 创建时间: 2024-03-26
// 作者: Lean
// 版本: 1.0
//===================================================

using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Domain.Entities;
using SqlSugar;

namespace Lean.CodeGen.Domain.Entities.Identity;

/// <summary>
/// 菜单实体
/// </summary>
[SugarTable("lean_menu", "菜单表")]
[SugarIndex("uk_perms", nameof(Perms), OrderByType.Asc)]
public class LeanMenu : LeanBaseEntity
{
  #region 基础信息

  /// <summary>
  /// 父级ID
  /// </summary>
  /// <remarks>
  /// 上级菜单的ID，用于构建菜单树结构
  /// </remarks>
  [SugarColumn(ColumnName = "parent_id", ColumnDescription = "父级ID", IsNullable = true, ColumnDataType = "bigint")]
  public long? ParentId { get; set; }

  /// <summary>
  /// 菜单名称
  /// </summary>
  /// <remarks>
  /// 菜单的显示名称，如：用户管理、角色管理等
  /// </remarks>
  [SugarColumn(ColumnName = "menu_name", ColumnDescription = "菜单名称", Length = 50, IsNullable = false, ColumnDataType = "nvarchar")]
  public string MenuName { get; set; } = default!;

  /// <summary>
  /// 权限标识
  /// </summary>
  /// <remarks>
  /// 权限标识符，用于后端权限验证，如：system:user:list、system:role:create等
  /// </remarks>
  [SugarColumn(ColumnName = "perms", ColumnDescription = "权限标识", Length = 100, IsNullable = false, UniqueGroupNameList = new[] { "uk_perms" }, ColumnDataType = "nvarchar")]
  public string Perms { get; set; } = default!;

  /// <summary>
  /// 菜单类型
  /// </summary>
  /// <remarks>
  /// 菜单类型：0-目录，1-菜单，2-按钮，3-API接口
  /// </remarks>
  [SugarColumn(ColumnName = "menu_type", ColumnDescription = "菜单类型", IsNullable = false, ColumnDataType = "int")]
  public LeanMenuType MenuType { get; set; }

  #endregion

  #region 显示配置

  /// <summary>
  /// 排序号
  /// </summary>
  /// <remarks>
  /// 显示顺序，数值越小越靠前
  /// </remarks>
  [SugarColumn(ColumnName = "order_num", ColumnDescription = "排序号", IsNullable = false, DefaultValue = "0", ColumnDataType = "int")]
  public int OrderNum { get; set; }

  /// <summary>
  /// 图标
  /// </summary>
  /// <remarks>
  /// 菜单图标，使用Element Plus的图标
  /// </remarks>
  [SugarColumn(ColumnName = "icon", ColumnDescription = "图标", Length = 100, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? Icon { get; set; }

  /// <summary>
  /// 国际化翻译键
  /// </summary>
  /// <remarks>
  /// 用于前端国际化翻译，如：menu.system.user、menu.system.role等
  /// </remarks>
  [SugarColumn(ColumnName = "trans_key", ColumnDescription = "国际化翻译键", Length = 100, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? TransKey { get; set; }

  #endregion

  #region 路由配置

  /// <summary>
  /// 路由路径
  /// </summary>
  /// <remarks>
  /// 前端路由路径，如：/system/user、/system/role等
  /// 当IsFrame为true时，此处填写完整的外部URL地址
  /// </remarks>
  [SugarColumn(ColumnName = "path", ColumnDescription = "路由路径", Length = 200, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? Path { get; set; }

  /// <summary>
  /// 组件路径
  /// </summary>
  /// <remarks>
  /// 前端组件路径，如：system/user/index、system/role/index等
  /// 当IsFrame为true时，此处固定为"InnerLink"或"Layout"
  /// </remarks>
  [SugarColumn(ColumnName = "component", ColumnDescription = "组件路径", Length = 200, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? Component { get; set; }

  /// <summary>
  /// 重定向路径
  /// </summary>
  /// <remarks>
  /// 重定向路径，主要用于以下场景：
  /// 1. 目录类型：重定向到第一个子菜单
  /// 2. 外链类型(IsFrame=true)：不需要设置重定向
  /// 3. 普通菜单：特殊情况下重定向到其他页面
  /// </remarks>
  [SugarColumn(ColumnName = "redirect", ColumnDescription = "重定向路径", Length = 200, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? Redirect { get; set; }

  #endregion

  #region 状态标记

  /// <summary>
  /// 状态
  /// </summary>
  /// <remarks>
  /// 菜单状态：0-正常，1-禁用
  /// </remarks>
  [SugarColumn(ColumnName = "menu_status", ColumnDescription = "状态", IsNullable = false, DefaultValue = "0", ColumnDataType = "int")]
  public LeanMenuStatus MenuStatus { get; set; }

  /// <summary>
  /// 显示状态
  /// </summary>
  /// <remarks>
  /// 显示状态：0-显示，1-隐藏
  /// </remarks>
  [SugarColumn(ColumnName = "visible", ColumnDescription = "显示状态", IsNullable = false, DefaultValue = "0", ColumnDataType = "int")]
  public int Visible { get; set; }

  /// <summary>
  /// 是否外链
  /// </summary>
  /// <remarks>
  /// 是否外链：0-否，1-是
  /// 当设置为外链时：
  /// 1. path：填写完整的外部URL地址
  /// 2. component：固定为"InnerLink"或"Layout"
  /// 3. redirect：不需要设置
  /// </remarks>
  [SugarColumn(ColumnName = "is_frame", ColumnDescription = "是否外链", IsNullable = false, DefaultValue = "0", ColumnDataType = "int")]
  public int IsFrame { get; set; }

  /// <summary>
  /// 是否缓存
  /// </summary>
  /// <remarks>
  /// 是否缓存该页面：0-不缓存，1-缓存
  /// 用于标记页面是否需要保持状态
  /// </remarks>
  [SugarColumn(ColumnName = "is_cached", ColumnDescription = "是否缓存", IsNullable = false, DefaultValue = "1", ColumnDataType = "int")]
  public int IsCached { get; set; } = 1;

  /// <summary>
  /// 是否内置
  /// </summary>
  /// <remarks>
  /// 是否为系统内置菜单：No-否，Yes-是
  /// 内置菜单不允许删除
  /// </remarks>
  [SugarColumn(ColumnName = "is_builtin", ColumnDescription = "是否内置", IsNullable = false, DefaultValue = "0", ColumnDataType = "int")]
  public LeanBuiltinStatus IsBuiltin { get; set; }

  #endregion

  #region 关联关系

  /// <summary>
  /// 角色菜单关联
  /// </summary>
  /// <remarks>
  /// 菜单与角色的多对多关系
  /// </remarks>
  [Navigate(NavigateType.OneToMany, nameof(LeanRoleMenu.MenuId))]
  public virtual ICollection<LeanRoleMenu> RoleMenus { get; set; } = new List<LeanRoleMenu>();

  #endregion
}