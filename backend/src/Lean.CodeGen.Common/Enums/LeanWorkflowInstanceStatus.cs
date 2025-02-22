namespace Lean.CodeGen.Common.Enums;

/// <summary>
/// 工作流实例状态枚举
/// </summary>
public enum LeanWorkflowInstanceStatus
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
  /// 已暂停
  /// </summary>
  Suspended = 4,

  /// <summary>
  /// 出错
  /// </summary>
  Faulted = 5
}