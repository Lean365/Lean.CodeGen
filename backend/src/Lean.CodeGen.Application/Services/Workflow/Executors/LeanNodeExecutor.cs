using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Domain.Entities.Workflow;

namespace Lean.CodeGen.Application.Services.Workflow.Executors;

/// <summary>
/// 节点执行器
/// </summary>
public class LeanNodeExecutor
{
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
            ActivityStatus = LeanWorkflowActivityStatus.Completed,
            StartTime = DateTime.Now,
            EndTime = DateTime.Now
        };

        // TODO: 保存活动实例

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
            ActivityStatus = LeanWorkflowActivityStatus.Completed,
            StartTime = DateTime.Now,
            EndTime = DateTime.Now
        };

        // TODO: 保存活动实例
        // TODO: 更新流程实例状态为已完成

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
            ActivityStatus = LeanWorkflowActivityStatus.Running,
            StartTime = DateTime.Now
        };

        // TODO: 保存活动实例

        // 创建任务
        var task = new LeanWorkflowTask
        {
            InstanceId = instanceId,
            TaskName = activity.ActivityName,
            TaskType = LeanWorkflowTaskType.Approval,
            TaskNode = activity.ActivityId,
            TaskStatus = LeanWorkflowTaskStatus.Processing,
            StartTime = DateTime.Now
        };

        // TODO: 保存任务

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
            ActivityStatus = LeanWorkflowActivityStatus.Running,
            StartTime = DateTime.Now
        };

        // TODO: 保存活动实例
        // TODO: 评估网关条件，确定下一个节点

        return true;
    }
}