namespace Lean.CodeGen.Common.Enums;

/// <summary>
/// 工作流操作类型
/// </summary>
public enum LeanWorkflowOperationType
{
  /// <summary>
  /// 启动
  /// </summary>
  Start = 1,

  /// <summary>
  /// 完成
  /// </summary>
  Complete = 2,

  /// <summary>
  /// 驳回
  /// </summary>
  Reject = 3,

  /// <summary>
  /// 转办
  /// </summary>
  Transfer = 4,

  /// <summary>
  /// 委派
  /// </summary>
  Delegate = 5,

  /// <summary>
  /// 撤回
  /// </summary>
  Withdraw = 6,

  /// <summary>
  /// 创建实例
  /// </summary>
  CreateInstance = 7,

  /// <summary>
  /// 取消实例
  /// </summary>
  CancelInstance = 8,

  /// <summary>
  /// 暂停实例
  /// </summary>
  SuspendInstance = 9,

  /// <summary>
  /// 恢复实例
  /// </summary>
  ResumeInstance = 10,

  /// <summary>
  /// 处理任务
  /// </summary>
  HandleTask = 11,

  /// <summary>
  /// 转交任务
  /// </summary>
  TransferTask = 12,

  /// <summary>
  /// 加签任务
  /// </summary>
  AddSign = 13,

  /// <summary>
  /// 减签任务
  /// </summary>
  RemoveSign = 14,

  /// <summary>
  /// 退回任务
  /// </summary>
  ReturnTask = 15,

  /// <summary>
  /// 终止流程
  /// </summary>
  TerminateWorkflow = 16,

  /// <summary>
  /// 修改变量
  /// </summary>
  ModifyVariable = 17,

  /// <summary>
  /// 修改表单
  /// </summary>
  ModifyForm = 18,

  /// <summary>
  /// 暂停
  /// </summary>
  Suspend = 19,

  /// <summary>
  /// 恢复
  /// </summary>
  Resume = 20,

  /// <summary>
  /// 终止
  /// </summary>
  Terminate = 21,

  /// <summary>
  /// 其他操作
  /// </summary>
  Other = 99
}