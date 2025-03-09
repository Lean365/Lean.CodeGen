using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Domain.Entities.Workflow;
using Lean.CodeGen.Domain.Interfaces.Repositories;

namespace Lean.CodeGen.Application.Services.Workflow.Executors;

/// <summary>
/// 节点执行器
/// </summary>
public class LeanNodeExecutor
{
  private readonly ILeanRepository<LeanWorkflowActivityInstance> _activityRepository;

  public LeanNodeExecutor(ILeanRepository<LeanWorkflowActivityInstance> activityRepository)
  {
    _activityRepository = activityRepository;
  }

  /// <summary>
  /// 执行节点
  /// </summary>
  public async Task<bool> ExecuteAsync(LeanWorkflowActivity activity, Dictionary<string, object> context)
  {
    switch (activity.ActivityType)
    {
      case "StartEvent":
        return await ExecuteStartEventAsync(activity, context);

      case "EndEvent":
        return await ExecuteEndEventAsync(activity, context);

      case "UserTask":
        return await ExecuteUserTaskAsync(activity, context);

      case "ExclusiveGateway":
        return await ExecuteExclusiveGatewayAsync(activity, context);

      default:
        throw new Exception($"Unsupported activity type: {activity.ActivityType}");
    }
  }

  private async Task<bool> ExecuteStartEventAsync(LeanWorkflowActivity activity, Dictionary<string, object> context)
  {
    // 创建活动实例
    var activityInstance = new LeanWorkflowActivityInstance
    {
      WorkflowInstanceId = Convert.ToInt64(context["ProcessInstanceId"]),
      ActivityType = activity.ActivityType,
      ActivityName = activity.ActivityName,
      ActivityStatus = 2, // Completed
      StartTime = DateTime.Now,
      EndTime = DateTime.Now
    };

    await _activityRepository.CreateAsync(activityInstance);

    return true;
  }

  private async Task<bool> ExecuteEndEventAsync(LeanWorkflowActivity activity, Dictionary<string, object> context)
  {
    var instanceId = Convert.ToInt64(context["ProcessInstanceId"]);

    // 创建活动实例
    var activityInstance = new LeanWorkflowActivityInstance
    {
      WorkflowInstanceId = instanceId,
      ActivityType = activity.ActivityType,
      ActivityName = activity.ActivityName,
      ActivityStatus = 2, // Completed
      StartTime = DateTime.Now,
      EndTime = DateTime.Now
    };

    await _activityRepository.CreateAsync(activityInstance);

    return true;
  }

  private async Task<bool> ExecuteUserTaskAsync(LeanWorkflowActivity activity, Dictionary<string, object> context)
  {
    var instanceId = Convert.ToInt64(context["ProcessInstanceId"]);

    // 创建活动实例
    var activityInstance = new LeanWorkflowActivityInstance
    {
      WorkflowInstanceId = instanceId,
      ActivityType = activity.ActivityType,
      ActivityName = activity.ActivityName,
      ActivityStatus = 1, // Running
      StartTime = DateTime.Now
    };

    await _activityRepository.CreateAsync(activityInstance);

    return true;
  }

  private async Task<bool> ExecuteExclusiveGatewayAsync(LeanWorkflowActivity activity, Dictionary<string, object> context)
  {
    var instanceId = Convert.ToInt64(context["ProcessInstanceId"]);

    // 创建活动实例
    var activityInstance = new LeanWorkflowActivityInstance
    {
      WorkflowInstanceId = instanceId,
      ActivityType = activity.ActivityType,
      ActivityName = activity.ActivityName,
      ActivityStatus = 1, // Running
      StartTime = DateTime.Now
    };

    await _activityRepository.CreateAsync(activityInstance);

    return true;
  }

  public async Task<int> GetNodeStatusAsync(string nodeId)
  {
    var activity = await _activityRepository.FirstOrDefaultAsync(x => x.ActivityId == nodeId);
    if (activity == null)
    {
      return 0;
    }
    return activity.ActivityStatus;
  }

  public async Task<int> StartNodeAsync(string nodeId)
  {
    var activity = await _activityRepository.FirstOrDefaultAsync(x => x.ActivityId == nodeId);
    if (activity == null)
    {
      return 0;
    }
    activity.ActivityStatus = 1; // Running
    await _activityRepository.UpdateAsync(activity);
    return activity.ActivityStatus;
  }

  public async Task<int> CompleteNodeAsync(string nodeId)
  {
    var activity = await _activityRepository.FirstOrDefaultAsync(x => x.ActivityId == nodeId);
    if (activity == null)
    {
      return 0;
    }
    activity.ActivityStatus = 2; // Completed
    await _activityRepository.UpdateAsync(activity);
    return activity.ActivityStatus;
  }

  public async Task<int> CancelNodeAsync(string nodeId)
  {
    var activity = await _activityRepository.FirstOrDefaultAsync(x => x.ActivityId == nodeId);
    if (activity == null)
    {
      return 0;
    }
    activity.ActivityStatus = 3; // Cancelled
    await _activityRepository.UpdateAsync(activity);
    return activity.ActivityStatus;
  }
}