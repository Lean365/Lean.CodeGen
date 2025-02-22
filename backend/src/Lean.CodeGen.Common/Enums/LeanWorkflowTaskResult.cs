namespace Lean.CodeGen.Common.Enums;

/// <summary>
/// 工作流任务处理结果
/// </summary>
public enum LeanWorkflowTaskResult
{
  /// <summary>
  /// 同意
  /// </summary>
  Approve = 0,

  /// <summary>
  /// 拒绝
  /// </summary>
  Reject = 1,

  /// <summary>
  /// 退回
  /// </summary>
  Return = 2,

  /// <summary>
  /// 转办
  /// </summary>
  Transfer = 3,

  /// <summary>
  /// 加签
  /// </summary>
  AddSign = 4,

  /// <summary>
  /// 减签
  /// </summary>
  RemoveSign = 5
}