//===================================================
// 项目名: Lean.CodeGen.Domain
// 文件名: LeanPost.cs
// 功能描述: 岗位实体
// 创建时间: 2024-03-26
// 作者: Lean
// 版本: 1.0
//===================================================

using Lean.CodeGen.Common.Enums;
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
}