//===================================================
// 项目名: Lean.CodeGen.Domain
// 文件名: LeanEntityBase.cs
// 功能描述: 实体基类
// 创建时间: 2024-03-26
// 作者: Lean
// 版本: 1.0
//===================================================

using SqlSugar;
using Lean.CodeGen.Common.Enums;

namespace Lean.CodeGen.Domain.Entities;

/// <summary>
/// 实体基类
/// </summary>
public abstract class LeanBaseEntity
{
  /// <summary>
  /// 主键
  /// </summary>
  [SugarColumn(ColumnName = "id", ColumnDescription = "主键", IsPrimaryKey = true, IsIdentity = true, IsNullable = false, ColumnDataType = "bigint")]
  public long Id { get; set; }

  /// <summary>
  /// 租户ID
  /// </summary>
  /// <remarks>
  /// 记录实体所属的租户ID，用于多租户隔离
  /// </remarks>
  [SugarColumn(ColumnName = "tenant_id", ColumnDescription = "租户ID", IsNullable = true, ColumnDataType = "bigint")]
  public long? TenantId { get; set; } = 0;

  #region 创建信息

  /// <summary>
  /// 创建者
  /// </summary>
  /// <remarks>
  /// 记录创建实体的用户信息
  /// </remarks>
  [SugarColumn(ColumnName = "create_by", ColumnDescription = "创建者", Length = 50, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? CreateBy { get; set; }

  /// <summary>
  /// 创建时间
  /// </summary>
  /// <remarks>
  /// 记录实体创建的时间
  /// </remarks>
  [SugarColumn(ColumnName = "create_time", ColumnDescription = "创建时间", IsNullable = false, ColumnDataType = "datetime")]
  public DateTime CreateTime { get; set; }

  #endregion

  #region 更新信息

  /// <summary>
  /// 更新者
  /// </summary>
  /// <remarks>
  /// 记录最后一次更新实体的用户信息
  /// </remarks>
  [SugarColumn(ColumnName = "update_by", ColumnDescription = "更新者", Length = 50, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? UpdateBy { get; set; }

  /// <summary>
  /// 更新时间
  /// </summary>
  /// <remarks>
  /// 记录实体最后一次更新的时间
  /// </remarks>
  [SugarColumn(ColumnName = "update_time", ColumnDescription = "更新时间", IsNullable = true, ColumnDataType = "datetime")]
  public DateTime? UpdateTime { get; set; }

  #endregion

  #region 审核信息

  /// <summary>
  /// 审核状态
  /// </summary>
  /// <remarks>
  /// 记录实体的审核状态：0-无需审核，1-待审核，2-已审核，3-已驳回
  /// </remarks>
  [SugarColumn(ColumnName = "audit_status", ColumnDescription = "审核状态", IsNullable = false, DefaultValue = "0", ColumnDataType = "int")]
  public int AuditStatus { get; set; }

  /// <summary>
  /// 审核人员
  /// </summary>
  /// <remarks>
  /// 记录执行审核操作的用户信息
  /// </remarks>
  [SugarColumn(ColumnName = "audit_by", ColumnDescription = "审核人员", Length = 50, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? AuditBy { get; set; }

  /// <summary>
  /// 审核时间
  /// </summary>
  /// <remarks>
  /// 记录执行审核操作的时间
  /// </remarks>
  [SugarColumn(ColumnName = "audit_time", ColumnDescription = "审核时间", IsNullable = true, ColumnDataType = "datetime")]
  public DateTime? AuditTime { get; set; }

  /// <summary>
  /// 审核意见
  /// </summary>
  /// <remarks>
  /// 记录审核过程中的意见说明，如审核通过理由、驳回原因等
  /// </remarks>
  [SugarColumn(ColumnName = "audit_opinion", ColumnDescription = "审核意见", Length = 2000, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? AuditOpinion { get; set; }

  #endregion

  #region 撤销信息

  /// <summary>
  /// 撤销人员
  /// </summary>
  /// <remarks>
  /// 记录执行撤销操作的用户信息
  /// </remarks>
  [SugarColumn(ColumnName = "revoke_by", ColumnDescription = "撤销人员", Length = 50, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? RevokeBy { get; set; }

  /// <summary>
  /// 撤销时间
  /// </summary>
  /// <remarks>
  /// 记录执行撤销操作的时间
  /// </remarks>
  [SugarColumn(ColumnName = "revoke_time", ColumnDescription = "撤销时间", IsNullable = true, ColumnDataType = "datetime")]
  public DateTime? RevokeTime { get; set; }

  /// <summary>
  /// 撤销意见
  /// </summary>
  /// <remarks>
  /// 记录撤销操作的原因说明
  /// </remarks>
  [SugarColumn(ColumnName = "revoke_opinion", ColumnDescription = "撤销意见", Length = 2000, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? RevokeOpinion { get; set; }

  #endregion

  #region 删除信息

  /// <summary>
  /// 是否删除
  /// </summary>
  /// <remarks>
  /// 软删除标记：0表示未删除，1表示已删除
  /// </remarks>
  [SugarColumn(ColumnName = "is_deleted", ColumnDescription = "是否删除", IsNullable = false, DefaultValue = "0", ColumnDataType = "int")]
  public int IsDeleted { get; set; }

  /// <summary>
  /// 删除者
  /// </summary>
  /// <remarks>
  /// 记录执行软删除操作的用户信息
  /// </remarks>
  [SugarColumn(ColumnName = "delete_by", ColumnDescription = "删除者", Length = 50, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? DeleteBy { get; set; }

  /// <summary>
  /// 删除时间
  /// </summary>
  /// <remarks>
  /// 记录实体被软删除的时间
  /// </remarks>
  [SugarColumn(ColumnName = "delete_time", ColumnDescription = "删除时间", IsNullable = true, ColumnDataType = "datetime")]
  public DateTime? DeleteTime { get; set; }

  #endregion

  /// <summary>
  /// 备注
  /// </summary>
  /// <remarks>
  /// 实体的补充说明
  /// </remarks>
  [SugarColumn(ColumnName = "remark", ColumnDescription = "备注", Length = 500, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? Remark { get; set; }
}