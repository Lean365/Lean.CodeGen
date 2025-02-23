namespace Lean.CodeGen.Common.Enums;

/// <summary>
/// 工作流活动状态
/// </summary>
public enum LeanWorkflowActivityStatus
{
  /// <summary>
  /// 未开始
  /// </summary>
  NotStarted = 0,

  /// <summary>
  /// 运行中
  /// </summary>
  Running = 1,

  /// <summary>
  /// 已完成
  /// </summary>
  Completed = 2,

  /// <summary>
  /// 已取消
  /// </summary>
  Cancelled = 3,

  /// <summary>
  /// 已补偿
  /// </summary>
  Compensated = 4,

  /// <summary>
  /// 失败
  /// </summary>
  Failed = 5
}