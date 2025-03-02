namespace Lean.CodeGen.Common.Enums;

/// <summary>
/// 工作流活动状态
/// </summary>
public enum LeanWorkflowActivityStatus
{
  /// <summary>
  /// 运行中
  /// </summary>
  Running = 1,

  /// <summary>
  /// 已完成
  /// </summary>
  Completed = 2,

  /// <summary>
  /// 已终止
  /// </summary>
  Terminated = 3,

  /// <summary>
  /// 已驳回
  /// </summary>
  Rejected = 4,

  /// <summary>
  /// 已取消
  /// </summary>
  Cancelled = 5,

  /// <summary>
  /// 已暂停
  /// </summary>
  Suspended = 6,

  /// <summary>
  /// 已补偿
  /// </summary>
  Compensated = 7
}