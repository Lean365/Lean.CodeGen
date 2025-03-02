using Lean.CodeGen.Application.Dtos.Workflow;
using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Domain.Entities.Workflow;

namespace Lean.CodeGen.Application.Services.Workflow;

/// <summary>
/// 工作流引擎接口
/// 提供工作流实例的生命周期管理、任务处理、数据存取等核心功能
/// </summary>
public interface ILeanWorkflowEngine
{
  #region 流程定义管理
  /// <summary>
  /// 验证流程定义
  /// </summary>
  Task<bool> ValidateDefinitionAsync(string bpmnContent);

  /// <summary>
  /// 解析流程定义
  /// </summary>
  Task<LeanWorkflowDefinition> ParseDefinitionAsync(string bpmnContent);
  #endregion

  #region 流程实例控制
  /// <summary>
  /// 启动流程实例
  /// </summary>
  /// <param name="definitionId">工作流定义ID - 指定要启动的工作流定义</param>
  /// <param name="businessKey">业务主键 - 关联的业务数据唯一标识</param>
  /// <param name="businessType">业务类型 - 关联的业务数据类型</param>
  /// <param name="title">实例标题 - 工作流实例的显示名称</param>
  /// <param name="initiatorId">发起人ID - 工作流发起人的用户ID</param>
  /// <param name="initiatorName">发起人名称 - 工作流发起人的用户名称</param>
  /// <param name="initiatorDeptId">发起人部门ID - 工作流发起人的部门ID</param>
  /// <param name="initiatorDeptName">发起人部门名称 - 工作流发起人的部门名称</param>
  /// <param name="variables">变量数据 - 工作流实例的初始变量数据</param>
  /// <param name="formData">表单数据 - 工作流实例的初始表单数据</param>
  /// <returns>返回创建的工作流实例对象</returns>
  Task<LeanWorkflowInstance> StartProcessAsync(
      long definitionId,
      string businessKey,
      string businessType,
      string title,
      long initiatorId,
      string initiatorName,
      long initiatorDeptId,
      string initiatorDeptName,
      Dictionary<string, object>? variables = null,
      Dictionary<string, object>? formData = null);

  /// <summary>
  /// 暂停流程实例
  /// 暂停后的实例将不能继续执行任务,直到被恢复
  /// </summary>
  /// <param name="processInstanceId">实例ID - 要暂停的工作流实例ID</param>
  /// <returns>操作是否成功</returns>
  Task<bool> SuspendProcessAsync(long processInstanceId);

  /// <summary>
  /// 恢复流程实例
  /// 恢复被暂停的工作流实例,使其可以继续执行
  /// </summary>
  /// <param name="processInstanceId">实例ID - 要恢复的工作流实例ID</param>
  /// <returns>操作是否成功</returns>
  Task<bool> ResumeProcessAsync(long processInstanceId);

  /// <summary>
  /// 终止流程实例
  /// 强制终止工作流实例,终止后的实例不能继续执行
  /// </summary>
  /// <param name="processInstanceId">实例ID - 要终止的工作流实例ID</param>
  /// <param name="reason">终止原因 - 记录终止工作流的原因</param>
  /// <returns>操作是否成功</returns>
  Task<bool> TerminateProcessAsync(long processInstanceId, string reason);
  #endregion

  #region 节点执行控制
  /// <summary>
  /// 执行节点
  /// </summary>
  Task<bool> ExecuteNodeAsync(string nodeId, Dictionary<string, object> context);

  /// <summary>
  /// 完成节点
  /// </summary>
  Task<bool> CompleteNodeAsync(string nodeId, Dictionary<string, object> output);
  #endregion

  #region 流程路由
  /// <summary>
  /// 获取下一个节点
  /// </summary>
  Task<List<string>> GetNextNodesAsync(string currentNodeId, Dictionary<string, object> context);

  /// <summary>
  /// 处理网关
  /// </summary>
  Task<List<string>> HandleGatewayAsync(string gatewayId, Dictionary<string, object> context);

  /// <summary>
  /// 评估条件
  /// </summary>
  Task<bool> EvaluateConditionAsync(string condition, Dictionary<string, object> context);
  #endregion

  #region 流程数据管理
  /// <summary>
  /// 获取流程变量
  /// </summary>
  Task<Dictionary<string, object>> GetProcessVariablesAsync(long processInstanceId);

  /// <summary>
  /// 设置流程变量
  /// </summary>
  Task<bool> SetProcessVariablesAsync(long processInstanceId, Dictionary<string, object> variables);
  #endregion

  #region 流程状态管理
  /// <summary>
  /// 获取流程状态
  /// </summary>
  Task<LeanWorkflowInstanceStatus> GetProcessStatusAsync(long processInstanceId);

  /// <summary>
  /// 获取节点状态
  /// </summary>
  Task<LeanWorkflowActivityStatus> GetNodeStatusAsync(string nodeId);
  #endregion

  /// <summary>
  /// 删除工作流实例
  /// 删除工作流实例及其相关的所有数据(活动、任务、表单、变量、历史等)
  /// </summary>
  /// <param name="instanceId">实例ID - 要删除的工作流实例ID</param>
  /// <returns>操作是否成功</returns>
  Task<bool> DeleteAsync(long instanceId);

  /// <summary>
  /// 完成任务
  /// 完成指定的工作流任务,并推进流程到下一个节点
  /// </summary>
  /// <param name="taskId">任务ID - 要完成的任务ID</param>
  /// <param name="operatorId">操作人ID - 执行任务完成操作的用户ID</param>
  /// <param name="operatorName">操作人名称 - 执行任务完成操作的用户名称</param>
  /// <param name="comment">审批意见 - 任务完成时的审批意见</param>
  /// <param name="variables">变量数据 - 任务完成时要更新的变量数据</param>
  /// <param name="formData">表单数据 - 任务完成时要保存的表单数据</param>
  /// <returns>操作是否成功</returns>
  Task<bool> CompleteTaskAsync(
      long taskId,
      long operatorId,
      string operatorName,
      string? comment = null,
      Dictionary<string, object>? variables = null,
      Dictionary<string, object>? formData = null);

  /// <summary>
  /// 驳回任务
  /// 驳回当前任务到指定的历史节点
  /// </summary>
  /// <param name="taskId">任务ID - 要驳回的任务ID</param>
  /// <param name="operatorId">操作人ID - 执行驳回操作的用户ID</param>
  /// <param name="operatorName">操作人名称 - 执行驳回操作的用户名称</param>
  /// <param name="comment">驳回意见 - 任务驳回时的意见说明</param>
  /// <param name="targetActivityId">目标活动ID - 要驳回到的目标节点ID</param>
  /// <returns>操作是否成功</returns>
  Task<bool> RejectTaskAsync(
      long taskId,
      long operatorId,
      string operatorName,
      string comment,
      string targetActivityId);

  /// <summary>
  /// 转办任务
  /// 将当前任务转交给其他人处理
  /// </summary>
  /// <param name="taskId">任务ID - 要转办的任务ID</param>
  /// <param name="operatorId">操作人ID - 执行转办操作的用户ID</param>
  /// <param name="operatorName">操作人名称 - 执行转办操作的用户名称</param>
  /// <param name="targetUserId">目标用户ID - 接收转办任务的用户ID</param>
  /// <param name="targetUserName">目标用户名称 - 接收转办任务的用户名称</param>
  /// <param name="comment">转办说明 - 任务转办时的说明</param>
  /// <returns>操作是否成功</returns>
  Task<bool> TransferTaskAsync(
      long taskId,
      long operatorId,
      string operatorName,
      long targetUserId,
      string targetUserName,
      string? comment = null);

  /// <summary>
  /// 委派任务
  /// 委派他人协助处理任务,处理完后需要返回委派人
  /// </summary>
  /// <param name="taskId">任务ID - 要委派的任务ID</param>
  /// <param name="operatorId">操作人ID - 执行委派操作的用户ID</param>
  /// <param name="operatorName">操作人名称 - 执行委派操作的用户名称</param>
  /// <param name="targetUserId">目标用户ID - 接收委派任务的用户ID</param>
  /// <param name="targetUserName">目标用户名称 - 接收委派任务的用户名称</param>
  /// <param name="comment">委派说明 - 任务委派时的说明</param>
  /// <returns>操作是否成功</returns>
  Task<bool> DelegateTaskAsync(
      long taskId,
      long operatorId,
      string operatorName,
      long targetUserId,
      string targetUserName,
      string? comment = null);

  /// <summary>
  /// 撤回任务
  /// 撤回已经完成的任务,恢复到当前用户处理
  /// </summary>
  /// <param name="taskId">任务ID - 要撤回的任务ID</param>
  /// <param name="operatorId">操作人ID - 执行撤回操作的用户ID</param>
  /// <param name="operatorName">操作人名称 - 执行撤回操作的用户名称</param>
  /// <param name="comment">撤回说明 - 任务撤回时的说明</param>
  /// <returns>操作是否成功</returns>
  Task<bool> WithdrawTaskAsync(
      long taskId,
      long operatorId,
      string operatorName,
      string? comment = null);

  /// <summary>
  /// 获取工作流实例当前活动节点
  /// 查询指定工作流实例当前正在执行的活动节点列表
  /// </summary>
  /// <param name="instanceId">实例ID - 要查询的工作流实例ID</param>
  /// <returns>当前活动的节点实例列表</returns>
  Task<List<LeanWorkflowActivityInstance>> GetCurrentActivitiesAsync(long instanceId);

  /// <summary>
  /// 获取工作流实例当前任务
  /// 查询指定工作流实例当前待处理的任务列表
  /// </summary>
  /// <param name="instanceId">实例ID - 要查询的工作流实例ID</param>
  /// <returns>当前待处理的任务列表</returns>
  Task<List<LeanWorkflowTask>> GetCurrentTasksAsync(long instanceId);

  /// <summary>
  /// 获取工作流实例变量
  /// 获取指定工作流实例的所有变量数据
  /// </summary>
  /// <param name="instanceId">实例ID - 要查询的工作流实例ID</param>
  /// <returns>变量名称和值的字典集合</returns>
  Task<Dictionary<string, object>> GetVariablesAsync(long instanceId);

  /// <summary>
  /// 设置工作流实例变量
  /// 更新指定工作流实例的变量数据
  /// </summary>
  /// <param name="instanceId">实例ID - 要更新的工作流实例ID</param>
  /// <param name="variables">变量数据 - 要更新的变量名称和值的字典集合</param>
  /// <returns>操作是否成功</returns>
  Task<bool> SetVariablesAsync(long instanceId, Dictionary<string, object> variables);
}