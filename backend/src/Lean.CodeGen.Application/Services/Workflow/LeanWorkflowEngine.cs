using System.Text.Json;
using Lean.CodeGen.Application.Dtos.Workflow;
using Lean.CodeGen.Application.Services.Workflow.Executors;
using Lean.CodeGen.Application.Services.Workflow.Parsers;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Domain.Entities.Workflow;
using Lean.CodeGen.Domain.Extensions;
using Lean.CodeGen.Domain.Interfaces.Repositories;
using Mapster;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SqlSugar;

namespace Lean.CodeGen.Application.Services.Workflow;

/// <summary>
/// 工作流引擎实现
/// </summary>
public class LeanWorkflowEngine : ILeanWorkflowEngine
{
  private readonly ILogger<LeanWorkflowEngine> _logger;
  private readonly ILeanRepository<LeanWorkflowDefinition> _definitionRepository;
  private readonly ILeanRepository<LeanWorkflowInstance> _instanceRepository;
  private readonly ILeanRepository<LeanWorkflowActivityInstance> _activityInstanceRepository;
  private readonly ILeanRepository<LeanWorkflowTask> _taskRepository;
  private readonly ILeanRepository<LeanWorkflowFormData> _formDataRepository;
  private readonly ILeanRepository<LeanWorkflowVariableData> _variableDataRepository;
  private readonly ILeanRepository<LeanWorkflowHistory> _historyRepository;
  private readonly LeanBpmnParser _bpmnParser;
  private readonly LeanNodeExecutor _nodeExecutor;
  private readonly LeanConditionParser _conditionParser;

  /// <summary>
  /// 构造函数
  /// </summary>
  public LeanWorkflowEngine(
      ILogger<LeanWorkflowEngine> logger,
      ILeanRepository<LeanWorkflowDefinition> definitionRepository,
      ILeanRepository<LeanWorkflowInstance> instanceRepository,
      ILeanRepository<LeanWorkflowActivityInstance> activityInstanceRepository,
      ILeanRepository<LeanWorkflowTask> taskRepository,
      ILeanRepository<LeanWorkflowFormData> formDataRepository,
      ILeanRepository<LeanWorkflowVariableData> variableDataRepository,
      ILeanRepository<LeanWorkflowHistory> historyRepository,
      LeanBpmnParser bpmnParser,
      LeanNodeExecutor nodeExecutor,
      LeanConditionParser conditionParser)
  {
    _logger = logger;
    _definitionRepository = definitionRepository;
    _instanceRepository = instanceRepository;
    _activityInstanceRepository = activityInstanceRepository;
    _taskRepository = taskRepository;
    _formDataRepository = formDataRepository;
    _variableDataRepository = variableDataRepository;
    _historyRepository = historyRepository;
    _bpmnParser = bpmnParser;
    _nodeExecutor = nodeExecutor;
    _conditionParser = conditionParser;
  }

  #region 流程定义管理
  public async Task<bool> ValidateDefinitionAsync(string bpmnContent)
  {
    try
    {
      var definition = await ParseDefinitionAsync(bpmnContent);
      return definition != null;
    }
    catch
    {
      return false;
    }
  }

  public async Task<LeanWorkflowDefinition> ParseDefinitionAsync(string bpmnContent)
  {
    return _bpmnParser.Parse(bpmnContent);
  }
  #endregion

  #region 流程实例控制
  public async Task<LeanWorkflowInstance> StartProcessAsync(
      long definitionId,
      string businessKey,
      string businessType,
      string title,
      long initiatorId,
      string initiatorName,
      long initiatorDeptId,
      string initiatorDeptName,
      Dictionary<string, object>? variables = null,
      Dictionary<string, object>? formData = null)
  {
    // 1. 获取流程定义
    var definition = await _definitionRepository.GetByIdAsync(definitionId);
    if (definition == null)
    {
      throw new Exception($"流程定义[{definitionId}]不存在");
    }

    // 2. 检查流程定义状态
    if (definition.Status != 1)
    {
      throw new Exception($"流程定义[{definition.WorkflowName}]已禁用");
    }

    if (definition.IsPublished != 1)
    {
      throw new Exception($"流程定义[{definition.WorkflowName}]未发布");
    }

    // 3. 创建流程实例
    var instance = new LeanWorkflowInstance
    {
      DefinitionId = definitionId,
      DefinitionVersion = definition.Version,
      BusinessKey = businessKey,
      BusinessType = businessType,
      Title = title,
      InitiatorId = initiatorId,
      InitiatorName = initiatorName,
      InitiatorDeptId = initiatorDeptId,
      InitiatorDeptName = initiatorDeptName,
      WorkflowStatus = 1, // Running
      StartTime = DateTime.Now
    };

    await _instanceRepository.CreateAsync(instance);

    // 4. 保存变量数据
    if (variables != null)
    {
      await SaveVariablesAsync(instance.Id, variables);
    }

    // 5. 保存表单数据
    if (formData != null)
    {
      await SaveFormDataAsync(instance.Id, null, formData);
    }

    // 6. 创建开始节点活动实例
    var startActivity = definition.Activities.FirstOrDefault(x => x.ActivityType == "StartEvent");
    if (startActivity != null)
    {
      var activityInstance = new LeanWorkflowActivityInstance
      {
        WorkflowInstanceId = instance.Id,
        ActivityType = startActivity.ActivityType,
        ActivityName = startActivity.ActivityName,
        ActivityStatus = 1, // Running
        StartTime = DateTime.Now,
        EndTime = DateTime.Now
      };

      await _activityInstanceRepository.CreateAsync(activityInstance);

      // 7. 获取下一个节点
      var context = new Dictionary<string, object>
        {
            { "ProcessInstanceId", instance.Id },
            { "BusinessKey", businessKey },
            { "BusinessType", businessType }
        };
      if (variables != null)
      {
        foreach (var variable in variables)
        {
          context[variable.Key] = variable.Value;
        }
      }

      var nextNodes = await GetNextNodesAsync(startActivity.ActivityId, context);

      // 8. 创建用户任务
      foreach (var nodeId in nextNodes)
      {
        var activity = await GetActivityByIdAsync(nodeId);
        if (activity?.ActivityType == "UserTask")
        {
          var task = new LeanWorkflowTask
          {
            InstanceId = instance.Id,
            TaskName = activity.ActivityName,
            TaskType = 1,
            TaskNode = activity.ActivityId,
            TaskStatus = 1,
            StartTime = DateTime.Now
          };

          await _taskRepository.CreateAsync(task);

          // 更新实例当前节点信息
          instance.CurrentNodeId = activity.ActivityId;
          instance.CurrentNodeName = activity.ActivityName;
          await _instanceRepository.UpdateAsync(instance);
        }
        else
        {
          // 执行其他类型节点
          await ExecuteNodeAsync(nodeId, context);
        }
      }
    }

    // 9. 记录历史
    await CreateHistoryAsync(instance.Id, null, 1, initiatorId, initiatorName);

    return instance;
  }

  public async Task<bool> SuspendProcessAsync(long processInstanceId)
  {
    // 1. 获取流程实例
    var instance = await _instanceRepository.GetByIdAsync(processInstanceId);
    if (instance == null)
    {
      throw new Exception($"流程实例[{processInstanceId}]不存在");
    }

    // 2. 检查实例状态
    if (instance.WorkflowStatus != 1)
    {
      throw new Exception($"流程实例[{processInstanceId}]不在运行状态");
    }

    if (instance.IsSuspended == 1)
    {
      throw new Exception($"流程实例[{processInstanceId}]已经处于暂停状态");
    }

    // 3. 更新实例状态
    instance.IsSuspended = 1;
    instance.SuspendTime = DateTime.Now;
    await _instanceRepository.UpdateAsync(instance);

    // 4. 暂停当前所有运行中的活动实例
    var runningActivities = await _activityInstanceRepository.GetListAsync(x =>
        x.WorkflowInstanceId == processInstanceId &&
        x.ActivityStatus == 1);

    foreach (var activity in runningActivities)
    {
      activity.ActivityStatus = 5;
      await _activityInstanceRepository.UpdateAsync(activity);
    }

    // 5. 暂停当前所有运行中的任务
    var runningTasks = await _taskRepository.GetListAsync(x =>
        x.InstanceId == processInstanceId &&
        x.TaskStatus == 1);

    foreach (var task in runningTasks)
    {
      task.TaskStatus = 5;
      await _taskRepository.UpdateAsync(task);
    }

    // 6. 记录历史
    await CreateHistoryAsync(
        processInstanceId,
        null,
        1,
        instance.InitiatorId,
        instance.InitiatorName,
        "流程已暂停");

    return true;
  }

  public async Task<bool> ResumeProcessAsync(long processInstanceId)
  {
    // 1. 获取流程实例
    var instance = await _instanceRepository.GetByIdAsync(processInstanceId);
    if (instance == null)
    {
      throw new Exception($"流程实例[{processInstanceId}]不存在");
    }

    // 2. 检查实例状态
    if (instance.WorkflowStatus != 1)
    {
      throw new Exception($"流程实例[{processInstanceId}]不在运行状态");
    }

    if (instance.IsSuspended != 1)
    {
      throw new Exception($"流程实例[{processInstanceId}]不在暂停状态");
    }

    // 3. 更新实例状态
    instance.IsSuspended = 0;
    instance.SuspendTime = null;
    await _instanceRepository.UpdateAsync(instance);

    // 4. 恢复当前所有暂停的活动实例
    var suspendedActivities = await _activityInstanceRepository.GetListAsync(x =>
        x.WorkflowInstanceId == processInstanceId &&
        x.ActivityStatus == 5);

    foreach (var activity in suspendedActivities)
    {
      activity.ActivityStatus = 1;
      await _activityInstanceRepository.UpdateAsync(activity);
    }

    // 5. 恢复当前所有暂停的任务
    var suspendedTasks = await _taskRepository.GetListAsync(x =>
        x.InstanceId == processInstanceId &&
        x.TaskStatus == 5);

    foreach (var task in suspendedTasks)
    {
      task.TaskStatus = 1;
      await _taskRepository.UpdateAsync(task);
    }

    // 6. 记录历史
    await CreateHistoryAsync(
        processInstanceId,
        null,
        1,
        instance.InitiatorId,
        instance.InitiatorName,
        "流程已恢复");

    return true;
  }

  public async Task<bool> TerminateProcessAsync(long processInstanceId, string reason)
  {
    // 1. 获取流程实例
    var instance = await _instanceRepository.GetByIdAsync(processInstanceId);
    if (instance == null)
    {
      throw new Exception($"流程实例[{processInstanceId}]不存在");
    }

    // 2. 检查实例状态
    if (instance.WorkflowStatus != 1)
    {
      throw new Exception($"流程实例[{processInstanceId}]不在运行状态");
    }

    // 3. 更新实例状态
    instance.WorkflowStatus = 3;
    instance.EndTime = DateTime.Now;
    await _instanceRepository.UpdateAsync(instance);

    // 4. 终止当前所有运行中的活动实例
    var runningActivities = await _activityInstanceRepository.GetListAsync(x =>
        x.WorkflowInstanceId == processInstanceId &&
        (x.ActivityStatus == 1 ||
         x.ActivityStatus == 5));

    foreach (var activity in runningActivities)
    {
      activity.ActivityStatus = 3;
      activity.EndTime = DateTime.Now;
      await _activityInstanceRepository.UpdateAsync(activity);
    }

    // 5. 终止当前所有运行中的任务
    var runningTasks = await _taskRepository.GetListAsync(x =>
        x.InstanceId == processInstanceId &&
        (x.TaskStatus == 1 ||
         x.TaskStatus == 5));

    foreach (var task in runningTasks)
    {
      task.TaskStatus = 3;
      task.EndTime = DateTime.Now;
      await _taskRepository.UpdateAsync(task);
    }

    // 6. 记录历史
    await CreateHistoryAsync(
        processInstanceId,
        null,
        1,
        instance.InitiatorId,
        instance.InitiatorName,
        $"流程已终止，原因：{reason}");

    return true;
  }
  #endregion

  #region 节点执行控制
  public async Task<bool> ExecuteNodeAsync(string nodeId, Dictionary<string, object> context)
  {
    var activity = await GetActivityByIdAsync(nodeId);
    if (activity == null)
    {
      throw new Exception($"Activity not found: {nodeId}");
    }

    return await _nodeExecutor.ExecuteAsync(activity, context);
  }

  public async Task<bool> CompleteNodeAsync(string nodeId, Dictionary<string, object> output)
  {
    // TODO: 实现节点完成逻辑
    return true;
  }
  #endregion

  #region 流程路由
  public async Task<List<string>> GetNextNodesAsync(string currentNodeId, Dictionary<string, object> context)
  {
    var nextNodes = new List<string>();

    // 1. 获取当前节点
    var currentActivity = await GetActivityByIdAsync(currentNodeId);
    if (currentActivity == null)
    {
      throw new Exception($"活动节点[{currentNodeId}]不存在");
    }

    // 2. 获取出向连线
    var flows = await GetOutgoingFlowsAsync(currentNodeId);
    if (!flows.Any())
    {
      return nextNodes;
    }

    // 3. 如果是排他网关，只取第一个满足条件的连线
    if (currentActivity.ActivityType == "ExclusiveGateway")
    {
      foreach (var flow in flows)
      {
        if (string.IsNullOrEmpty(flow.Condition) || await EvaluateConditionAsync(flow.Condition, context))
        {
          nextNodes.Add(flow.TargetNodeId);
          break;
        }
      }
    }
    // 4. 如果是并行网关，取所有连线
    else if (currentActivity.ActivityType == "ParallelGateway")
    {
      nextNodes.AddRange(flows.Select(x => x.TargetNodeId));
    }
    // 5. 如果是普通节点，取所有满足条件的连线
    else
    {
      foreach (var flow in flows)
      {
        if (string.IsNullOrEmpty(flow.Condition) || await EvaluateConditionAsync(flow.Condition, context))
        {
          nextNodes.Add(flow.TargetNodeId);
        }
      }
    }

    return nextNodes;
  }

  public async Task<List<string>> HandleGatewayAsync(string gatewayId, Dictionary<string, object> context)
  {
    // 1. 获取网关节点
    var gateway = await GetActivityByIdAsync(gatewayId);
    if (gateway == null)
    {
      throw new Exception($"网关节点[{gatewayId}]不存在");
    }

    // 2. 根据网关类型处理
    switch (gateway.ActivityType)
    {
      case "ExclusiveGateway":
        return await HandleExclusiveGatewayAsync(gatewayId, context);
      case "ParallelGateway":
        return await HandleParallelGatewayAsync(gatewayId, context);
      default:
        throw new Exception($"不支持的网关类型: {gateway.ActivityType}");
    }
  }

  public async Task<bool> EvaluateConditionAsync(string condition, Dictionary<string, object> context)
  {
    try
    {
      return _conditionParser.Evaluate(condition, context);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, $"评估条件[{condition}]失败");
      return false;
    }
  }

  private async Task<List<string>> HandleExclusiveGatewayAsync(string gatewayId, Dictionary<string, object> context)
  {
    var nextNodes = new List<string>();
    var flows = await GetOutgoingFlowsAsync(gatewayId);

    // 排他网关只取第一个满足条件的连线
    foreach (var flow in flows)
    {
      if (string.IsNullOrEmpty(flow.Condition) || await EvaluateConditionAsync(flow.Condition, context))
      {
        nextNodes.Add(flow.TargetNodeId);
        break;
      }
    }

    if (!nextNodes.Any())
    {
      throw new Exception($"网关节点[{gatewayId}]没有满足条件的出向连线");
    }

    return nextNodes;
  }

  private async Task<List<string>> HandleParallelGatewayAsync(string gatewayId, Dictionary<string, object> context)
  {
    var flows = await GetOutgoingFlowsAsync(gatewayId);
    return flows.Select(x => x.TargetNodeId).ToList();
  }
  #endregion

  #region 流程数据管理
  public async Task<Dictionary<string, object>> GetProcessVariablesAsync(long processInstanceId)
  {
    // 1. 获取流程实例
    var instance = await _instanceRepository.GetByIdAsync(processInstanceId);
    if (instance == null)
    {
      throw new Exception($"流程实例[{processInstanceId}]不存在");
    }

    // 2. 获取变量列表
    var variables = await _variableDataRepository.GetListAsync(x => x.InstanceId == processInstanceId);
    var result = new Dictionary<string, object>();

    // 3. 反序列化变量值
    foreach (var variable in variables)
    {
      try
      {
        var value = JsonConvert.DeserializeObject(variable.VariableValue);
        if (value != null)
        {
          result[variable.VariableName] = value;
        }
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, $"反序列化变量[{variable.VariableName}]失败");
      }
    }

    return result;
  }

  public async Task<bool> SetProcessVariablesAsync(long processInstanceId, Dictionary<string, object> variables)
  {
    // 1. 获取流程实例
    var instance = await _instanceRepository.GetByIdAsync(processInstanceId);
    if (instance == null)
    {
      throw new Exception($"流程实例[{processInstanceId}]不存在");
    }

    // 2. 检查实例状态
    if (instance.WorkflowStatus != 1)
    {
      throw new Exception($"流程实例[{processInstanceId}]不在运行状态");
    }

    // 3. 保存变量
    await SaveVariablesAsync(processInstanceId, variables);

    return true;
  }
  #endregion

  #region 流程状态管理
  public async Task<int> GetProcessStatusAsync(long processInstanceId)
  {
    var instance = await _instanceRepository.GetByIdAsync(processInstanceId);
    if (instance == null)
    {
      return 0;
    }
    return instance.WorkflowStatus;
  }

  public async Task<int> GetNodeStatusAsync(string nodeId)
  {
    var activity = await _activityInstanceRepository.FirstOrDefaultAsync(x => x.ActivityId == nodeId);
    if (activity == null)
    {
      return 0;
    }
    return activity.ActivityStatus;
  }
  #endregion

  public async Task<bool> DeleteAsync(long instanceId)
  {
    // TODO: 实现删除流程实例逻辑
    return true;
  }

  public async Task<bool> CompleteTaskAsync(
      long taskId,
      long operatorId,
      string operatorName,
      string comment,
      Dictionary<string, object>? variables = null,
      Dictionary<string, object>? formData = null)
  {
    // 1. 获取任务
    var task = await _taskRepository.GetByIdAsync(taskId);
    if (task == null)
    {
      throw new Exception($"任务[{taskId}]不存在");
    }

    // 2. 获取工作流实例
    var instance = await _instanceRepository.GetByIdAsync(task.InstanceId);
    if (instance == null)
    {
      throw new Exception($"工作流实例[{task.InstanceId}]不存在");
    }

    if (instance.WorkflowStatus != 1)
    {
      throw new Exception($"工作流实例[{task.InstanceId}]已结束");
    }

    if (instance.IsSuspended == 1)
    {
      throw new Exception($"工作流实例[{task.InstanceId}]已暂停");
    }

    if (task.TaskStatus != 1)
    {
      throw new Exception($"任务[{taskId}]已结束");
    }

    // 3. 获取当前活动实例
    var activityInstance = await _activityInstanceRepository.FirstOrDefaultAsync(x =>
        x.WorkflowInstanceId == instance.Id &&
        x.ActivityId == task.TaskNode &&
        x.ActivityStatus == 1);

    if (activityInstance == null)
    {
      throw new Exception($"活动实例[{task.TaskNode}]不存在");
    }

    // 4. 保存变量数据
    if (variables != null)
    {
      await SaveVariablesAsync(instance.Id, variables);
    }

    // 5. 保存表单数据
    if (formData != null)
    {
      await SaveFormDataAsync(instance.Id, string.IsNullOrEmpty(task.TaskNode) ? null : Convert.ToInt64(task.TaskNode), formData);
    }

    // 6. 完成当前活动实例
    activityInstance.ActivityStatus = 2; // Completed
    activityInstance.EndTime = DateTime.Now;
    await _activityInstanceRepository.UpdateAsync(activityInstance);

    // 7. 完成当前任务
    task.TaskStatus = 2; // Completed
    task.EndTime = DateTime.Now;
    task.OperatorId = operatorId;
    task.OperatorName = operatorName;
    await _taskRepository.UpdateAsync(task);

    // 8. 获取下一个节点
    var context = await GetProcessContextAsync(instance.Id);
    var nextNodes = await GetNextNodesAsync(task.TaskNode, context);

    // 9. 创建下一个节点的活动实例和任务
    foreach (var nodeId in nextNodes)
    {
      var activity = await GetActivityByIdAsync(nodeId);
      if (activity == null)
      {
        continue;
      }

      // 创建活动实例
      var nextActivityInstance = new LeanWorkflowActivityInstance
      {
        WorkflowInstanceId = instance.Id,
        ActivityId = activity.ActivityId,
        ActivityType = activity.ActivityType,
        ActivityName = activity.ActivityName,
        ActivityStatus = 1, // Running
        StartTime = DateTime.Now
      };

      await _activityInstanceRepository.CreateAsync(nextActivityInstance);

      // 如果是用户任务，创建任务
      if (activity.ActivityType == "UserTask")
      {
        var nextTask = new LeanWorkflowTask
        {
          InstanceId = instance.Id,
          TaskName = activity.ActivityName,
          TaskType = 1,
          TaskNode = activity.ActivityId,
          TaskStatus = 1,
          StartTime = DateTime.Now
        };

        await _taskRepository.CreateAsync(nextTask);

        // 更新实例当前节点信息
        instance.CurrentNodeId = activity.ActivityId;
        instance.CurrentNodeName = activity.ActivityName;
        await _instanceRepository.UpdateAsync(instance);
      }
      else
      {
        // 执行其他类型节点
        await ExecuteNodeAsync(nodeId, context);
      }
    }

    // 10. 记录历史
    await CreateHistoryAsync(instance.Id, taskId, 1, operatorId, operatorName, comment);

    return true;
  }

  public async Task<bool> RejectTaskAsync(
      long taskId,
      long operatorId,
      string operatorName,
      string comment,
      string targetActivityId)
  {
    // 1. 获取任务
    var task = await _taskRepository.GetByIdAsync(taskId);
    if (task == null)
    {
      throw new Exception($"任务[{taskId}]不存在");
    }

    // 2. 获取工作流实例
    var instance = await _instanceRepository.GetByIdAsync(task.InstanceId);
    if (instance == null)
    {
      throw new Exception($"工作流实例[{task.InstanceId}]不存在");
    }

    if (instance.WorkflowStatus != 1)
    {
      throw new Exception($"工作流实例[{task.InstanceId}]已结束");
    }

    if (instance.IsSuspended == 1)
    {
      throw new Exception($"工作流实例[{task.InstanceId}]已暂停");
    }

    if (task.TaskStatus != 1)
    {
      throw new Exception($"任务[{taskId}]已结束");
    }

    // 3. 获取当前活动实例
    var activityInstance = await _activityInstanceRepository.FirstOrDefaultAsync(x =>
        x.WorkflowInstanceId == instance.Id &&
        x.ActivityId == task.TaskNode &&
        x.ActivityStatus == 1);

    if (activityInstance == null)
    {
      throw new Exception($"活动实例[{task.TaskNode}]不存在");
    }

    // 4. 完成当前活动实例
    activityInstance.ActivityStatus = 3; // Rejected
    activityInstance.EndTime = DateTime.Now;
    await _activityInstanceRepository.UpdateAsync(activityInstance);

    // 5. 完成当前任务
    task.TaskStatus = 3; // Rejected
    task.EndTime = DateTime.Now;
    task.OperatorId = operatorId;
    task.OperatorName = operatorName;
    await _taskRepository.UpdateAsync(task);

    // 6. 获取目标活动
    var previousActivity = await GetActivityByIdAsync(targetActivityId);
    if (previousActivity == null)
    {
      throw new Exception($"目标活动[{targetActivityId}]不存在");
    }

    // 7. 创建新的活动实例
    var newActivityInstance = new LeanWorkflowActivityInstance
    {
      WorkflowInstanceId = instance.Id,
      ActivityId = previousActivity.ActivityId,
      ActivityType = previousActivity.ActivityType,
      ActivityName = previousActivity.ActivityName,
      ActivityStatus = 1, // Running
      StartTime = DateTime.Now
    };

    await _activityInstanceRepository.CreateAsync(newActivityInstance);

    // 8. 获取上一个任务
    var previousTask = await _taskRepository.FirstOrDefaultAsync(x =>
        x.InstanceId == instance.Id &&
        x.TaskNode == targetActivityId &&
        x.TaskStatus == 2);

    if (previousTask == null)
    {
      throw new Exception($"上一个任务[{targetActivityId}]不存在");
    }

    // 9. 创建新的任务
    var newTask = new LeanWorkflowTask
    {
      InstanceId = instance.Id,
      TaskName = previousActivity.ActivityName,
      TaskType = 1,
      TaskNode = previousActivity.ActivityId,
      TaskStatus = 1,
      StartTime = DateTime.Now,
      AssigneeId = previousTask.AssigneeId,
      AssigneeName = previousTask.AssigneeName
    };

    await _taskRepository.CreateAsync(newTask);

    // 10. 更新实例当前节点信息
    instance.CurrentNodeId = previousActivity.ActivityId;
    instance.CurrentNodeName = previousActivity.ActivityName;
    await _instanceRepository.UpdateAsync(instance);

    // 11. 记录历史
    await CreateHistoryAsync(instance.Id, taskId, 2, operatorId, operatorName, comment);

    return true;
  }

  public async Task<bool> TransferTaskAsync(
      long taskId,
      long operatorId,
      string operatorName,
      long targetUserId,
      string targetUserName,
      string? comment = null)
  {
    // 1. 获取任务
    var task = await _taskRepository.GetByIdAsync(taskId);
    if (task == null)
    {
      throw new Exception($"任务[{taskId}]不存在");
    }

    // 2. 获取工作流实例
    var instance = await _instanceRepository.GetByIdAsync(task.InstanceId);
    if (instance == null)
    {
      throw new Exception($"工作流实例[{task.InstanceId}]不存在");
    }

    if (instance.WorkflowStatus != 1)
    {
      throw new Exception($"工作流实例[{task.InstanceId}]已结束");
    }

    if (instance.IsSuspended == 1)
    {
      throw new Exception($"工作流实例[{task.InstanceId}]已暂停");
    }

    if (task.TaskStatus != 1)
    {
      throw new Exception($"任务[{taskId}]已结束");
    }

    // 3. 更新任务处理人信息
    task.AssigneeId = targetUserId;
    task.AssigneeName = targetUserName;
    task.TransferTime = DateTime.Now;
    await _taskRepository.UpdateAsync(task);

    // 4. 记录历史
    await CreateHistoryAsync(instance.Id, taskId, 1, operatorId, operatorName, comment);

    return true;
  }

  public async Task<bool> DelegateTaskAsync(
      long taskId,
      long operatorId,
      string operatorName,
      long targetUserId,
      string targetUserName,
      string? comment = null)
  {
    // 1. 获取任务
    var task = await _taskRepository.GetByIdAsync(taskId);
    if (task == null)
    {
      throw new Exception($"任务[{taskId}]不存在");
    }

    // 2. 获取工作流实例
    var instance = await _instanceRepository.GetByIdAsync(task.InstanceId);
    if (instance == null)
    {
      throw new Exception($"工作流实例[{task.InstanceId}]不存在");
    }

    if (instance.WorkflowStatus != 1)
    {
      throw new Exception($"工作流实例[{task.InstanceId}]已结束");
    }

    if (instance.IsSuspended == 1)
    {
      throw new Exception($"工作流实例[{task.InstanceId}]已暂停");
    }

    if (task.TaskStatus != 1)
    {
      throw new Exception($"任务[{taskId}]已结束");
    }

    // 3. 更新任务委派信息
    task.DelegateUserId = targetUserId;
    task.DelegateUserName = targetUserName;
    task.DelegateTime = DateTime.Now;
    await _taskRepository.UpdateAsync(task);

    // 4. 记录历史
    await CreateHistoryAsync(instance.Id, taskId, 1, operatorId, operatorName, comment);

    return true;
  }

  public async Task<bool> WithdrawTaskAsync(
      long taskId,
      long operatorId,
      string operatorName,
      string? comment = null)
  {
    // 1. 获取任务
    var task = await _taskRepository.GetByIdAsync(taskId);
    if (task == null)
    {
      throw new Exception($"任务[{taskId}]不存在");
    }

    // 2. 获取工作流实例
    var instance = await _instanceRepository.GetByIdAsync(task.InstanceId);
    if (instance == null)
    {
      throw new Exception($"工作流实例[{task.InstanceId}]不存在");
    }

    if (instance.WorkflowStatus != 1)
    {
      throw new Exception($"工作流实例[{task.InstanceId}]已结束");
    }

    if (instance.IsSuspended == 1)
    {
      throw new Exception($"工作流实例[{task.InstanceId}]已暂停");
    }

    // 3. 获取上一个任务
    var previousTask = await _taskRepository.GetFirstAsync(x =>
        x.InstanceId == instance.Id &&
        x.TaskStatus == 2 &&
        x.EndTime < task.StartTime);

    if (previousTask == null)
    {
      throw new Exception($"找不到可撤回的任务");
    }

    // 4. 结束当前任务
    task.TaskStatus = 3;
    task.EndTime = DateTime.Now;
    await _taskRepository.UpdateAsync(task);

    // 5. 结束当前活动实例
    var activityInstance = await _activityInstanceRepository.GetFirstAsync(x =>
        x.WorkflowInstanceId == instance.Id &&
        x.ActivityId == task.TaskNode &&
        x.ActivityStatus == 1);

    if (activityInstance != null)
    {
      activityInstance.ActivityStatus = 3;
      activityInstance.EndTime = DateTime.Now;
      await _activityInstanceRepository.UpdateAsync(activityInstance);
    }

    // 6. 创建新的活动实例
    var previousActivity = await GetActivityByIdAsync(previousTask.TaskNode);
    if (previousActivity == null)
    {
      throw new Exception($"找不到上一个活动节点");
    }

    var newActivityInstance = new LeanWorkflowActivityInstance
    {
      WorkflowInstanceId = instance.Id,
      ActivityId = previousActivity.ActivityId,
      ActivityType = previousActivity.ActivityType,
      ActivityName = previousActivity.ActivityName,
      ActivityStatus = 1,
      StartTime = DateTime.Now
    };

    await _activityInstanceRepository.CreateAsync(newActivityInstance);

    // 7. 创建新的任务
    var newTask = new LeanWorkflowTask
    {
      InstanceId = instance.Id,
      TaskName = previousActivity.ActivityName,
      TaskType = 1,
      TaskNode = previousActivity.ActivityId,
      TaskStatus = 1,
      StartTime = DateTime.Now,
      AssigneeId = previousTask.AssigneeId,
      AssigneeName = previousTask.AssigneeName
    };

    await _taskRepository.CreateAsync(newTask);

    // 8. 更新实例当前节点信息
    instance.CurrentNodeId = previousActivity.ActivityId;
    instance.CurrentNodeName = previousActivity.ActivityName;
    await _instanceRepository.UpdateAsync(instance);

    // 9. 记录历史
    await CreateHistoryAsync(instance.Id, taskId, 1, operatorId, operatorName, comment);

    return true;
  }

  public async Task<List<LeanWorkflowActivityInstance>> GetCurrentActivitiesAsync(long instanceId)
  {
    // 1. 获取流程实例
    var instance = await _instanceRepository.GetByIdAsync(instanceId);
    if (instance == null)
    {
      throw new Exception($"流程实例[{instanceId}]不存在");
    }

    // 2. 获取当前运行中的活动实例
    return await _activityInstanceRepository.GetListAsync(x =>
        x.WorkflowInstanceId == instanceId &&
        x.ActivityStatus == 1);
  }

  public async Task<List<LeanWorkflowTask>> GetCurrentTasksAsync(long instanceId)
  {
    // 1. 获取流程实例
    var instance = await _instanceRepository.GetByIdAsync(instanceId);
    if (instance == null)
    {
      throw new Exception($"流程实例[{instanceId}]不存在");
    }

    // 2. 获取当前处理中的任务
    return await _taskRepository.GetListAsync(x =>
        x.InstanceId == instanceId &&
        x.TaskStatus == 1);
  }

  public async Task<Dictionary<string, object>> GetVariablesAsync(long instanceId)
  {
    return await GetProcessVariablesAsync(instanceId);
  }

  public async Task<bool> SetVariablesAsync(long instanceId, Dictionary<string, object> variables)
  {
    return await SetProcessVariablesAsync(instanceId, variables);
  }

  #region 私有方法
  private async Task<LeanWorkflowActivity?> GetStartActivityAsync(long definitionId)
  {
    var definition = await _definitionRepository.GetByIdAsync(definitionId);
    if (definition == null)
    {
      throw new Exception($"流程定义[{definitionId}]不存在");
    }

    return definition.Activities.FirstOrDefault(x => x.ActivityType == "StartEvent");
  }

  private async Task<LeanWorkflowActivity?> GetActivityByIdAsync(string activityId)
  {
    // 1. 从活动实例获取定义ID
    var activityInstance = await _activityInstanceRepository.GetFirstAsync(x => x.ActivityId == activityId);
    if (activityInstance == null)
    {
      return null;
    }

    // 2. 获取流程实例
    var instance = await _instanceRepository.GetByIdAsync(activityInstance.WorkflowInstanceId);
    if (instance == null)
    {
      return null;
    }

    // 3. 获取流程定义
    var definition = await _definitionRepository.GetByIdAsync(instance.DefinitionId);
    if (definition == null)
    {
      return null;
    }

    // 4. 获取活动节点
    return definition.Activities.FirstOrDefault(x => x.ActivityId == activityId);
  }

  private async Task<List<LeanWorkflowFlow>> GetOutgoingFlowsAsync(string nodeId)
  {
    // 1. 从活动实例获取定义ID
    var activityInstance = await _activityInstanceRepository.GetFirstAsync(x => x.ActivityId == nodeId);
    if (activityInstance == null)
    {
      return new List<LeanWorkflowFlow>();
    }

    // 2. 获取流程实例
    var instance = await _instanceRepository.GetByIdAsync(activityInstance.WorkflowInstanceId);
    if (instance == null)
    {
      return new List<LeanWorkflowFlow>();
    }

    // 3. 获取流程定义
    var definition = await _definitionRepository.GetByIdAsync(instance.DefinitionId);
    if (definition == null)
    {
      return new List<LeanWorkflowFlow>();
    }

    // 4. 获取出向连线
    return definition.Flows.Where(x => x.SourceNodeId == nodeId).ToList();
  }

  private async Task SaveVariablesAsync(long instanceId, Dictionary<string, object> variables)
  {
    foreach (var variable in variables)
    {
      await SaveVariableAsync(instanceId, variable.Key, variable.Value);
    }
  }

  private async Task SaveVariableAsync(long instanceId, string name, object value)
  {
    var variable = await _variableDataRepository.GetFirstAsync(x =>
        x.InstanceId == instanceId &&
        x.VariableName == name);

    if (variable == null)
    {
      variable = new LeanWorkflowVariableData
      {
        InstanceId = instanceId,
        VariableName = name,
        VariableType = value.GetType().Name,
        VariableValue = JsonConvert.SerializeObject(value)
      };

      await _variableDataRepository.CreateAsync(variable);
    }
    else
    {
      variable.VariableValue = JsonConvert.SerializeObject(value);
      await _variableDataRepository.UpdateAsync(variable);
    }
  }

  private async Task SaveFormDataAsync(long instanceId, long? taskId, Dictionary<string, object> formData)
  {
    foreach (var data in formData)
    {
      var formDataEntity = new LeanWorkflowFormData
      {
        InstanceId = instanceId,
        TaskId = taskId,
        FieldCode = data.Key,
        FieldValue = JsonConvert.SerializeObject(data.Value)
      };

      await _formDataRepository.CreateAsync(formDataEntity);
    }
  }

  private async Task CreateHistoryAsync(
    long instanceId,
    long? taskId,
    int operationType,
    long operatorId,
    string operatorName,
    string? comment = null)
  {
    var history = new LeanWorkflowHistory
    {
      InstanceId = instanceId,
      TaskId = taskId,
      OperationType = operationType,
      OperatorId = operatorId,
      OperatorName = operatorName,
      OperationDesc = comment,
      OperationTime = DateTime.Now
    };

    await _historyRepository.CreateAsync(history);
  }

  private async Task<Dictionary<string, object>> GetProcessContextAsync(long instanceId)
  {
    var context = new Dictionary<string, object>();

    // 1. 获取流程实例
    var instance = await _instanceRepository.GetByIdAsync(instanceId);
    if (instance == null)
    {
      return context;
    }

    // 2. 添加基本信息
    context["ProcessInstanceId"] = instance.Id;
    context["BusinessKey"] = instance.BusinessKey;
    context["BusinessType"] = instance.BusinessType;

    // 3. 获取变量数据
    var variables = await GetProcessVariablesAsync(instanceId);
    foreach (var variable in variables)
    {
      context[variable.Key] = variable.Value;
    }

    return context;
  }
  #endregion
}