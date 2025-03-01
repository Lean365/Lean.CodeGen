//===================================================
// 项目名: Lean.CodeGen.Application
// 文件名: LeanBaseDto.cs
// 功能描述: DTO基础类
// 创建时间: 2024-03-26
// 作者: Lean
// 版本: 1.0
//===================================================

using Lean.CodeGen.Common.Enums;

namespace Lean.CodeGen.Application.Dtos;

/// <summary>
/// DTO基础类
/// </summary>
public abstract class LeanBaseDto
{
  /// <summary>
  /// 主键
  /// </summary>
  public long Id { get; set; }

  /// <summary>
  /// 租户ID
  /// </summary>
  /// <remarks>
  /// 记录实体所属的租户ID，用于多租户隔离
  /// </remarks>
  public long? TenantId { get; set; }

  #region 创建信息

  /// <summary>
  /// 创建时间
  /// </summary>
  /// <remarks>
  /// 记录实体创建的时间
  /// </remarks>
  public DateTime CreateTime { get; set; }

  /// <summary>
  /// 创建者ID
  /// </summary>
  /// <remarks>
  /// 记录创建实体的用户ID
  /// </remarks>
  public long? CreateUserId { get; set; }

  /// <summary>
  /// 创建者
  /// </summary>
  /// <remarks>
  /// 记录创建实体的用户名称
  /// </remarks>
  public string? CreateUserName { get; set; }

  #endregion

  #region 更新信息

  /// <summary>
  /// 更新时间
  /// </summary>
  /// <remarks>
  /// 记录实体最后一次更新的时间
  /// </remarks>
  public DateTime? UpdateTime { get; set; }

  /// <summary>
  /// 更新者ID
  /// </summary>
  /// <remarks>
  /// 记录最后一次更新实体的用户ID
  /// </remarks>
  public long? UpdateUserId { get; set; }

  /// <summary>
  /// 更新者
  /// </summary>
  /// <remarks>
  /// 记录最后一次更新实体的用户名称
  /// </remarks>
  public string? UpdateUserName { get; set; }

  #endregion

  #region 审核信息

  /// <summary>
  /// 审核状态
  /// </summary>
  /// <remarks>
  /// 记录实体的审核状态：NoNeedAudit-无需审核，Pending-待审核，Approved-已审核，Rejected-已驳回等
  /// </remarks>
  public LeanAuditStatus AuditStatus { get; set; }

  /// <summary>
  /// 审核人员ID
  /// </summary>
  /// <remarks>
  /// 记录执行审核操作的用户ID
  /// </remarks>
  public long? AuditUserId { get; set; }

  /// <summary>
  /// 审核人员
  /// </summary>
  /// <remarks>
  /// 记录执行审核操作的用户名称
  /// </remarks>
  public string? AuditUserName { get; set; }

  /// <summary>
  /// 审核时间
  /// </summary>
  /// <remarks>
  /// 记录执行审核操作的时间
  /// </remarks>
  public DateTime? AuditTime { get; set; }

  /// <summary>
  /// 审核意见
  /// </summary>
  /// <remarks>
  /// 记录审核过程中的意见说明，如审核通过理由、驳回原因等
  /// </remarks>
  public string? AuditOpinion { get; set; }

  #endregion

  #region 撤销信息

  /// <summary>
  /// 撤销人员ID
  /// </summary>
  /// <remarks>
  /// 记录执行撤销操作的用户ID
  /// </remarks>
  public long? RevokeUserId { get; set; }

  /// <summary>
  /// 撤销人员
  /// </summary>
  /// <remarks>
  /// 记录执行撤销操作的用户名称
  /// </remarks>
  public string? RevokeUserName { get; set; }

  /// <summary>
  /// 撤销时间
  /// </summary>
  /// <remarks>
  /// 记录执行撤销操作的时间
  /// </remarks>
  public DateTime? RevokeTime { get; set; }

  /// <summary>
  /// 撤销意见
  /// </summary>
  /// <remarks>
  /// 记录撤销操作的原因说明
  /// </remarks>
  public string? RevokeOpinion { get; set; }

  #endregion

  #region 删除信息

  /// <summary>
  /// 是否删除
  /// </summary>
  /// <remarks>
  /// 软删除标记：0表示未删除，1表示已删除
  /// </remarks>
  public int IsDeleted { get; set; }

  /// <summary>
  /// 删除时间
  /// </summary>
  /// <remarks>
  /// 记录实体被软删除的时间
  /// </remarks>
  public DateTime? DeleteTime { get; set; }

  /// <summary>
  /// 删除者ID
  /// </summary>
  /// <remarks>
  /// 记录执行软删除操作的用户ID
  /// </remarks>
  public long? DeleteUserId { get; set; }

  /// <summary>
  /// 删除者
  /// </summary>
  /// <remarks>
  /// 记录执行软删除操作的用户名称
  /// </remarks>
  public string? DeleteUserName { get; set; }

  #endregion

  /// <summary>
  /// 备注
  /// </summary>
  /// <remarks>
  /// 实体的补充说明
  /// </remarks>
  public string? Remark { get; set; }
}

