using Lean.CodeGen.Application.Dtos.Workflow;
using Lean.CodeGen.Application.Services.Workflow;
using Lean.CodeGen.Domain.Entities.Workflow;
using Microsoft.AspNetCore.Mvc;
using Lean.CodeGen.Common.Enums;
using Microsoft.Extensions.Configuration;

namespace Lean.CodeGen.WebApi.Controllers.Workflow;

/// <summary>
/// 工作流控制器
/// </summary>
[ApiController]
[Route("api/[controller]")]
[ApiExplorerSettings(GroupName = "workflow")]
public class LeanWorkflowController : LeanBaseController
{
  private readonly ILeanWorkflowEngine _workflowEngine;

  /// <summary>
  /// 构造函数
  /// </summary>
  public LeanWorkflowController(
      ILeanWorkflowEngine workflowEngine,
      ILeanLocalizationService localizationService,
      IConfiguration configuration,
      ILeanUserContext userContext)
      : base(localizationService, configuration, userContext)
  {
    _workflowEngine = workflowEngine;
  }

  /// <summary>
  /// 验证流程定义
  /// </summary>
  [HttpPost("validate")]
  public async Task<ActionResult<bool>> ValidateDefinition([FromBody] string bpmnContent)
  {
    return await _workflowEngine.ValidateDefinitionAsync(bpmnContent);
  }

  /// <summary>
  /// 启动流程实例
  /// </summary>
  [HttpPost("start")]
  public async Task<ActionResult<LeanWorkflowInstance>> StartProcess([FromBody] LeanStartProcessDto request)
  {
    var instance = await _workflowEngine.StartProcessAsync(
        request.DefinitionId,
        request.BusinessKey,
        request.BusinessType,
        request.Title,
        request.InitiatorId,
        request.InitiatorName,
        request.InitiatorDeptId,
        request.InitiatorDeptName,
        request.Variables,
        request.FormData);

    return instance;
  }

  /// <summary>
  /// 暂停流程实例
  /// </summary>
  [HttpPost("{processInstanceId}/suspend")]
  public async Task<ActionResult<bool>> SuspendProcess(long processInstanceId)
  {
    return await _workflowEngine.SuspendProcessAsync(processInstanceId);
  }

  /// <summary>
  /// 恢复流程实例
  /// </summary>
  [HttpPost("{processInstanceId}/resume")]
  public async Task<ActionResult<bool>> ResumeProcess(long processInstanceId)
  {
    return await _workflowEngine.ResumeProcessAsync(processInstanceId);
  }

  /// <summary>
  /// 终止流程实例
  /// </summary>
  [HttpPost("{processInstanceId}/terminate")]
  public async Task<ActionResult<bool>> TerminateProcess(long processInstanceId, [FromBody] string reason)
  {
    return await _workflowEngine.TerminateProcessAsync(processInstanceId, reason);
  }

  /// <summary>
  /// 完成任务
  /// </summary>
  [HttpPost("task/{taskId}/complete")]
  public async Task<ActionResult<bool>> CompleteTask(long taskId, [FromBody] LeanCompleteTaskDto request)
  {
    return await _workflowEngine.CompleteTaskAsync(
        taskId,
        request.OperatorId,
        request.OperatorName,
        request.Comment,
        request.Variables,
        request.FormData);
  }

  /// <summary>
  /// 驳回任务
  /// </summary>
  [HttpPost("task/{taskId}/reject")]
  public async Task<ActionResult<bool>> RejectTask(long taskId, [FromBody] LeanRejectTaskDto request)
  {
    return await _workflowEngine.RejectTaskAsync(
        taskId,
        request.OperatorId,
        request.OperatorName,
        request.Comment,
        request.TargetActivityId);
  }

  /// <summary>
  /// 转办任务
  /// </summary>
  [HttpPost("task/{taskId}/transfer")]
  public async Task<ActionResult<bool>> TransferTask(long taskId, [FromBody] LeanTransferTaskDto request)
  {
    return await _workflowEngine.TransferTaskAsync(
        taskId,
        request.OperatorId,
        request.OperatorName,
        request.TargetUserId,
        request.TargetUserName,
        request.Comment);
  }

  /// <summary>
  /// 委派任务
  /// </summary>
  [HttpPost("task/{taskId}/delegate")]
  public async Task<ActionResult<bool>> DelegateTask(long taskId, [FromBody] LeanDelegateTaskDto request)
  {
    return await _workflowEngine.DelegateTaskAsync(
        taskId,
        request.OperatorId,
        request.OperatorName,
        request.TargetUserId,
        request.TargetUserName,
        request.Comment);
  }

  /// <summary>
  /// 撤回任务
  /// </summary>
  [HttpPost("task/{taskId}/withdraw")]
  public async Task<ActionResult<bool>> WithdrawTask(long taskId, [FromBody] LeanWithdrawTaskDto request)
  {
    return await _workflowEngine.WithdrawTaskAsync(
        taskId,
        request.OperatorId,
        request.OperatorName,
        request.Comment);
  }

  /// <summary>
  /// 获取流程变量
  /// </summary>
  [HttpGet("{processInstanceId}/variables")]
  public async Task<ActionResult<Dictionary<string, object>>> GetProcessVariables(long processInstanceId)
  {
    return await _workflowEngine.GetProcessVariablesAsync(processInstanceId);
  }

  /// <summary>
  /// 设置流程变量
  /// </summary>
  [HttpPost("{processInstanceId}/variables")]
  public async Task<ActionResult<bool>> SetProcessVariables(long processInstanceId, [FromBody] Dictionary<string, object> variables)
  {
    return await _workflowEngine.SetProcessVariablesAsync(processInstanceId, variables);
  }

  /// <summary>
  /// 获取流程状态
  /// </summary>
  [HttpGet("{processInstanceId}/status")]
  public async Task<ActionResult<int>> GetProcessStatus(long processInstanceId)
  {
    return await _workflowEngine.GetProcessStatusAsync(processInstanceId);
  }

  /// <summary>
  /// 获取节点状态
  /// </summary>
  [HttpGet("node/{nodeId}/status")]
  public async Task<ActionResult<int>> GetNodeStatus(string nodeId)
  {
    return await _workflowEngine.GetNodeStatusAsync(nodeId);
  }

  /// <summary>
  /// 获取当前活动节点
  /// </summary>
  [HttpGet("{processInstanceId}/activities")]
  public async Task<ActionResult<List<LeanWorkflowActivityInstance>>> GetCurrentActivities(long processInstanceId)
  {
    return await _workflowEngine.GetCurrentActivitiesAsync(processInstanceId);
  }

  /// <summary>
  /// 获取当前任务
  /// </summary>
  [HttpGet("{processInstanceId}/tasks")]
  public async Task<ActionResult<List<LeanWorkflowTask>>> GetCurrentTasks(long processInstanceId)
  {
    return await _workflowEngine.GetCurrentTasksAsync(processInstanceId);
  }
}