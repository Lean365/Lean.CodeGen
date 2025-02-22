namespace Lean.CodeGen.Common.Enums;

/// <summary>
/// 工作流操作类型
/// </summary>
public enum LeanWorkflowOperationType
{
  /// <summary>
  /// 创建实例
  /// </summary>
  CreateInstance = 1,

  /// <summary>
  /// 取消实例
  /// </summary>
  CancelInstance = 2,

  /// <summary>
  /// 暂停实例
  /// </summary>
  SuspendInstance = 3,

  /// <summary>
  /// 恢复实例
  /// </summary>
  ResumeInstance = 4,

  /// <summary>
  /// 处理任务
  /// </summary>
  HandleTask = 5,

  /// <summary>
  /// 转交任务
  /// </summary>
  TransferTask = 6,

  /// <summary>
  /// 撤回任务
  /// </summary>
  WithdrawTask = 7,

  /// <summary>
  /// 加签任务
  /// </summary>
  AddSign = 8,

  /// <summary>
  /// 减签任务
  /// </summary>
  RemoveSign = 9,

  /// <summary>
  /// 退回任务
  /// </summary>
  ReturnTask = 10,

  /// <summary>
  /// 终止流程
  /// </summary>
  TerminateWorkflow = 11,

  /// <summary>
  /// 修改变量
  /// </summary>
  ModifyVariable = 12,

  /// <summary>
  /// 修改表单
  /// </summary>
  ModifyForm = 13,

  /// <summary>
  /// 其他操作
  /// </summary>
  Other = 99
}