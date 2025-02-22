//===================================================
// 项目名: Lean.CodeGen.Domain
// 文件名: LeanPostPermission.cs
// 功能描述: 岗位权限实体
// 创建时间: 2024-03-26
// 作者: Lean
// 版本: 1.0
//===================================================

using Lean.CodeGen.Common.Enums;
using SqlSugar;

namespace Lean.CodeGen.Domain.Entities.Identity;

/// <summary>
/// 岗位权限实体
/// </summary>
[SugarTable("lean_post_permission", "岗位权限表")]
[SugarIndex("uk_post_permission", $"{nameof(PostId)},{nameof(ResourceType)},{nameof(ResourceId)}", OrderByType.Asc, true)]
public class LeanPostPermission : LeanBaseEntity
{
  /// <summary>
  /// 岗位ID
  /// </summary>
  /// <remarks>
  /// 关联的岗位ID
  /// </remarks>
  [SugarColumn(ColumnName = "post_id", ColumnDescription = "岗位ID", IsNullable = false, ColumnDataType = "bigint")]
  public long PostId { get; set; }

  /// <summary>
  /// 资源类型
  /// </summary>
  /// <remarks>
  /// 资源类型：1-菜单，2-API
  /// </remarks>
  [SugarColumn(ColumnName = "resource_type", ColumnDescription = "资源类型", IsNullable = false, ColumnDataType = "int")]
  public LeanResourceType ResourceType { get; set; }

  /// <summary>
  /// 资源ID
  /// </summary>
  /// <remarks>
  /// 关联的资源ID，根据资源类型关联到不同的表
  /// </remarks>
  [SugarColumn(ColumnName = "resource_id", ColumnDescription = "资源ID", IsNullable = false, ColumnDataType = "bigint")]
  public long ResourceId { get; set; }

  /// <summary>
  /// 权限名称
  /// </summary>
  /// <remarks>
  /// 权限的标识名称，如：system:user:list、system:role:create等
  /// </remarks>
  [SugarColumn(ColumnName = "permission_name", ColumnDescription = "权限名称", Length = 100, IsNullable = false, ColumnDataType = "nvarchar")]
  public string PermissionName { get; set; } = default!;

  /// <summary>
  /// 操作类型
  /// </summary>
  /// <remarks>
  /// 操作类型：1-查看，2-新增，3-修改，4-删除，5-导入，6-导出
  /// </remarks>
  [SugarColumn(ColumnName = "operation", ColumnDescription = "操作类型", IsNullable = false, ColumnDataType = "int")]
  public LeanOperationType Operation { get; set; }

  /// <summary>
  /// 岗位
  /// </summary>
  /// <remarks>
  /// 关联的岗位实体
  /// </remarks>
  [Navigate(NavigateType.OneToOne, nameof(PostId))]
  public virtual LeanPost Post { get; set; } = default!;
}