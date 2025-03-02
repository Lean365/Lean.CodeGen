namespace Lean.CodeGen.Common.Enums;

/// <summary>
/// 工作流任务状态
/// </summary>
public enum LeanWorkflowTaskStatus
{
  /// <summary>
  /// 处理中
  /// </summary>
  Processing = 1,

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
  /// 已暂停
  /// </summary>
  Suspended = 5,

  /// <summary>
  /// 已转办
  /// </summary>
  Transferred = 6
}