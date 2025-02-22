namespace Lean.CodeGen.Common.Enums;

/// <summary>
/// 工作流节点类型
/// </summary>
public enum LeanWorkflowNodeType
{
  /// <summary>
  /// 开始节点
  /// </summary>
  Start = 0,

  /// <summary>
  /// 结束节点
  /// </summary>
  End = 1,

  /// <summary>
  /// 用户任务节点
  /// </summary>
  UserTask = 2,

  /// <summary>
  /// 系统任务节点
  /// </summary>
  ServiceTask = 3,

  /// <summary>
  /// 排他网关
  /// </summary>
  ExclusiveGateway = 4,

  /// <summary>
  /// 并行网关
  /// </summary>
  ParallelGateway = 5,

  /// <summary>
  /// 包容网关
  /// </summary>
  InclusiveGateway = 6
}