using System.ComponentModel;

namespace Lean.CodeGen.Common.Enums
{
  /// <summary>
  /// 审核状态枚举
  /// </summary>
  public enum LeanAuditStatus
  {
    /// <summary>
    /// 无需审核
    /// </summary>
    /// <remarks>
    /// 数据无需经过审核流程
    /// </remarks>
    NoNeedAudit = 0,

    /// <summary>
    /// 草稿
    /// </summary>
    /// <remarks>
    /// 数据处于编辑状态,尚未提交审核
    /// </remarks>
    Draft = 1,

    /// <summary>
    /// 待审核
    /// </summary>
    /// <remarks>
    /// 数据已提交等待审核
    /// </remarks>
    Pending = 2,

    /// <summary>
    /// 审核中
    /// </summary>
    /// <remarks>
    /// 数据正在审核流程中
    /// </remarks>
    Processing = 3,

    /// <summary>
    /// 已通过
    /// </summary>
    /// <remarks>
    /// 数据已通过审核
    /// </remarks>
    Approved = 4,

    /// <summary>
    /// 已驳回
    /// </summary>
    /// <remarks>
    /// 数据未通过审核,需要修改后重新提交
    /// </remarks>
    Rejected = 5,

    /// <summary>
    /// 已撤回
    /// </summary>
    /// <remarks>
    /// 提交人主动撤回审核申请
    /// </remarks>
    Withdrawn = 6,

    /// <summary>
    /// 已取消
    /// </summary>
    /// <remarks>
    /// 审核流程被取消
    /// </remarks>
    Cancelled = 7,

    /// <summary>
    /// 已过期
    /// </summary>
    /// <remarks>
    /// 审核流程超时未处理
    /// </remarks>
    Expired = 8,

    /// <summary>
    /// 已失效
    /// </summary>
    /// <remarks>
    /// 审核通过的数据被标记为失效
    /// </remarks>
    Invalid = 9,

    /// <summary>
    /// 待复审
    /// </summary>
    /// <remarks>
    /// 需要进行二次或多次审核
    /// </remarks>
    PendingReview = 10,

    /// <summary>
    /// 复审中
    /// </summary>
    /// <remarks>
    /// 正在进行二次或多次审核
    /// </remarks>
    Reviewing = 11,

    /// <summary>
    /// 已归档
    /// </summary>
    /// <remarks>
    /// 审核流程结束并归档
    /// </remarks>
    Archived = 12
  }
}
