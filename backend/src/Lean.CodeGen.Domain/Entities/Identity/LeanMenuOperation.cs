//===================================================
// 项目名: Lean.CodeGen.Domain
// 文件名: LeanMenuOperation.cs
// 功能描述: 菜单操作权限实体
// 创建时间: 2024-03-26
// 作者: Lean
// 版本: 1.0
//===================================================

using SqlSugar;

namespace Lean.CodeGen.Domain.Entities.Identity;

/// <summary>
/// 菜单操作权限实体
/// </summary>
[SugarTable("lean_menu_operation", "菜单操作权限表")]
[SugarIndex("uk_menu_operation", $"{nameof(MenuId)},{nameof(Code)}", OrderByType.Asc, true)]
public class LeanMenuOperation : LeanBaseEntity
{
  /// <summary>
  /// 菜单ID
  /// </summary>
  /// <remarks>
  /// 关联的菜单ID
  /// </remarks>
  [SugarColumn(ColumnName = "menu_id", ColumnDescription = "菜单ID", IsNullable = false, ColumnDataType = "bigint")]
  public long MenuId { get; set; }

  /// <summary>
  /// 操作编码
  /// </summary>
  /// <remarks>
  /// 操作的唯一标识符，如：create、update、delete等
  /// </remarks>
  [SugarColumn(ColumnName = "code", ColumnDescription = "操作编码", Length = 50, IsNullable = false, ColumnDataType = "nvarchar")]
  public string Code { get; set; } = default!;

  /// <summary>
  /// 操作名称
  /// </summary>
  /// <remarks>
  /// 操作的显示名称，如：创建、更新、删除等
  /// </remarks>
  [SugarColumn(ColumnName = "name", ColumnDescription = "操作名称", Length = 50, IsNullable = false, ColumnDataType = "nvarchar")]
  public string Name { get; set; } = default!;

  /// <summary>
  /// 菜单
  /// </summary>
  /// <remarks>
  /// 关联的菜单实体
  /// </remarks>
  [Navigate(NavigateType.OneToOne, nameof(MenuId))]
  public virtual LeanMenu Menu { get; set; } = default!;
}