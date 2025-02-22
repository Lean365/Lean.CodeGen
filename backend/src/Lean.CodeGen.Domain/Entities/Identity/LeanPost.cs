//===================================================
// 项目名: Lean.CodeGen.Domain
// 文件名: LeanPost.cs
// 功能描述: 岗位实体
// 创建时间: 2024-03-26
// 作者: Lean
// 版本: 1.0
//===================================================

using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Domain.Entities;
using SqlSugar;

namespace Lean.CodeGen.Domain.Entities.Identity;

/// <summary>
/// 岗位实体
/// </summary>
[SugarTable("lean_post", "岗位表")]
[SugarIndex("uk_code", nameof(PostCode), OrderByType.Asc, true)]
public class LeanPost : LeanBaseEntity
{
  /// <summary>
  /// 岗位名称
  /// </summary>
  /// <remarks>
  /// 岗位的显示名称，如：技术总监、项目经理等
  /// </remarks>
  [SugarColumn(ColumnName = "post_name", ColumnDescription = "岗位名称", Length = 50, IsNullable = false, ColumnDataType = "nvarchar")]
  public string PostName { get; set; } = default!;

  /// <summary>
  /// 岗位编码
  /// </summary>
  /// <remarks>
  /// 岗位的唯一标识符，如：tech_director、project_manager等
  /// </remarks>
  [SugarColumn(ColumnName = "post_code", ColumnDescription = "岗位编码", Length = 50, IsNullable = false, UniqueGroupNameList = new[] { "uk_code" }, ColumnDataType = "nvarchar")]
  public string PostCode { get; set; } = default!;

  /// <summary>
  /// 岗位描述
  /// </summary>
  /// <remarks>
  /// 对岗位的详细说明
  /// </remarks>
  [SugarColumn(ColumnName = "post_description", ColumnDescription = "岗位描述", Length = 500, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? PostDescription { get; set; }

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
  /// 岗位状态：0-正常，1-禁用
  /// </remarks>
  [SugarColumn(ColumnName = "post_status", ColumnDescription = "状态", IsNullable = false, DefaultValue = "0", ColumnDataType = "int")]
  public LeanPostStatus PostStatus { get; set; }

  /// <summary>
  /// 是否内置
  /// </summary>
  /// <remarks>
  /// 是否为系统内置岗位：No-否，Yes-是
  /// 内置岗位不允许删除
  /// </remarks>
  [SugarColumn(ColumnName = "is_builtin", ColumnDescription = "是否内置", IsNullable = false, DefaultValue = "0", ColumnDataType = "int")]
  public LeanBuiltinStatus IsBuiltin { get; set; }

  /// <summary>
  /// 用户岗位关联
  /// </summary>
  /// <remarks>
  /// 岗位与用户的多对多关系
  /// </remarks>
  [Navigate(NavigateType.OneToMany, nameof(LeanUserPost.PostId))]
  public virtual List<LeanUserPost> UserPosts { get; set; } = new();

  /// <summary>
  /// 岗位继承关系（作为继承者）
  /// </summary>
  /// <remarks>
  /// 当前岗位继承的其他岗位
  /// </remarks>
  [Navigate(NavigateType.OneToMany, nameof(LeanPostInheritance.PostId))]
  public virtual List<LeanPostInheritance> InheritedPosts { get; set; } = new();

  /// <summary>
  /// 岗位继承关系（作为被继承者）
  /// </summary>
  /// <remarks>
  /// 继承当前岗位的其他岗位
  /// </remarks>
  [Navigate(NavigateType.OneToMany, nameof(LeanPostInheritance.InheritedPostId))]
  public virtual List<LeanPostInheritance> InheritingPosts { get; set; } = new();

  /// <summary>
  /// 岗位权限
  /// </summary>
  /// <remarks>
  /// 岗位的权限列表
  /// </remarks>
  [Navigate(NavigateType.OneToMany, nameof(LeanPostPermission.PostId))]
  public virtual List<LeanPostPermission> Permissions { get; set; } = new();
}