namespace Lean.CodeGen.Common.Enums;

/// <summary>
/// 工作流任务类型
/// </summary>
public enum LeanWorkflowTaskType
{
  /// <summary>
  /// 用户任务
  /// </summary>
  UserTask = 1,

  /// <summary>
  /// 系统任务
  /// </summary>
  SystemTask = 2,

  /// <summary>
  /// 脚本任务
  /// </summary>
  ScriptTask = 3,

  /// <summary>
  /// 服务任务
  /// </summary>
  ServiceTask = 4,

  /// <summary>
  /// 子流程任务
  /// </summary>
  SubProcessTask = 5
}