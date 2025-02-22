//===================================================
// 项目名: Lean.CodeGen.Domain
// 文件名: LeanUserPermission.cs
// 功能描述: 用户直接权限实体
// 创建时间: 2024-03-26
// 作者: Lean
// 版本: 1.0
//===================================================

using Lean.CodeGen.Common.Enums;
using SqlSugar;

namespace Lean.CodeGen.Domain.Entities.Identity;

/// <summary>
/// 用户直接权限实体
/// </summary>
/// <remarks>
/// 用于定义用户的直接权限，不通过角色分配的权限
/// </remarks>
[SugarTable("lean_user_permission", "用户直接权限表")]
[SugarIndex("uk_user_permission", $"{nameof(UserId)},{nameof(ResourceType)},{nameof(ResourceId)}", OrderByType.Asc, true)]
public class LeanUserPermission : LeanBaseEntity
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
  /// 用户
  /// </summary>
  /// <remarks>
  /// 关联的用户实体
  /// </remarks>
  [Navigate(NavigateType.OneToOne, nameof(UserId))]
  public virtual LeanUser User { get; set; } = default!;
}