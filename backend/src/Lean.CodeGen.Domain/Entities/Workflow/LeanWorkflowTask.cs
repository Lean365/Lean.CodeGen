using SqlSugar;
using Lean.CodeGen.Common.Enums;

namespace Lean.CodeGen.Domain.Entities.Workflow;

/// <summary>
/// 工作流任务实体
/// </summary>
[SugarTable("lean_workflow_task", "工作流任务表")]
[SugarIndex("idx_instance", nameof(InstanceId), OrderByType.Asc)]
[SugarIndex("idx_assignee", nameof(AssigneeId), OrderByType.Asc)]
public class LeanWorkflowTask : LeanBaseEntity
{
    /// <summary>
    /// 工作流实例ID
    /// </summary>
    [SugarColumn(ColumnName = "instance_id", ColumnDescription = "工作流实例ID", IsNullable = false)]
    public long InstanceId { get; set; }

    /// <summary>
    /// 工作流实例
    /// </summary>
    [Navigate(NavigateType.OneToOne, nameof(InstanceId))]
    public virtual LeanWorkflowInstance Instance { get; set; } = null!;

    /// <summary>
    /// 任务名称
    /// </summary>
    [SugarColumn(ColumnName = "task_name", ColumnDescription = "任务名称", Length = 100, IsNullable = false)]
    public string TaskName { get; set; } = string.Empty;

    /// <summary>
    /// 任务类型
    /// </summary>
    [SugarColumn(ColumnName = "task_type", ColumnDescription = "任务类型", IsNullable = false)]
    public LeanWorkflowTaskType TaskType { get; set; }

    /// <summary>
    /// 任务节点编码
    /// </summary>
    [SugarColumn(ColumnName = "task_node", ColumnDescription = "任务节点编码", Length = 50, IsNullable = false)]
    public string TaskNode { get; set; } = string.Empty;

    /// <summary>
    /// 任务优先级(0=普通,1=重要,2=紧急)
    /// </summary>
    [SugarColumn(ColumnName = "priority", ColumnDescription = "任务优先级", IsNullable = false, DefaultValue = "0")]
    public int Priority { get; set; }

    /// <summary>
    /// 处理人ID
    /// </summary>
    [SugarColumn(ColumnName = "assignee_id", ColumnDescription = "处理人ID", IsNullable = true)]
    public long? AssigneeId { get; set; }

    /// <summary>
    /// 处理人名称
    /// </summary>
    [SugarColumn(ColumnName = "assignee_name", ColumnDescription = "处理人名称", Length = 50, IsNullable = true)]
    public string? AssigneeName { get; set; }

    /// <summary>
    /// 处理人部门ID
    /// </summary>
    [SugarColumn(ColumnName = "assignee_dept_id", ColumnDescription = "处理人部门ID", IsNullable = true)]
    public long? AssigneeDeptId { get; set; }

    /// <summary>
    /// 处理人部门名称
    /// </summary>
    [SugarColumn(ColumnName = "assignee_dept_name", ColumnDescription = "处理人部门名称", Length = 50, IsNullable = true)]
    public string? AssigneeDeptName { get; set; }

    /// <summary>
    /// 原处理人ID(任务转交时使用)
    /// </summary>
    [SugarColumn(ColumnName = "original_assignee_id", ColumnDescription = "原处理人ID", IsNullable = true)]
    public long? OriginalAssigneeId { get; set; }

    /// <summary>
    /// 原处理人名称
    /// </summary>
    [SugarColumn(ColumnName = "original_assignee_name", ColumnDescription = "原处理人名称", Length = 50, IsNullable = true)]
    public string? OriginalAssigneeName { get; set; }

    /// <summary>
    /// 转办时间
    /// </summary>
    [SugarColumn(ColumnName = "transfer_time", ColumnDescription = "转办时间", IsNullable = true)]
    public DateTime? TransferTime { get; set; }

    /// <summary>
    /// 委派用户ID
    /// </summary>
    [SugarColumn(ColumnName = "delegate_user_id", ColumnDescription = "委派用户ID", IsNullable = true)]
    public long? DelegateUserId { get; set; }

    /// <summary>
    /// 委派用户名称
    /// </summary>
    [SugarColumn(ColumnName = "delegate_user_name", ColumnDescription = "委派用户名称", Length = 50, IsNullable = true)]
    public string? DelegateUserName { get; set; }

    /// <summary>
    /// 委派时间
    /// </summary>
    [SugarColumn(ColumnName = "delegate_time", ColumnDescription = "委派时间", IsNullable = true)]
    public DateTime? DelegateTime { get; set; }

    /// <summary>
    /// 任务状态
    /// </summary>
    [SugarColumn(ColumnName = "task_status", ColumnDescription = "任务状态", IsNullable = false)]
    public LeanWorkflowTaskStatus TaskStatus { get; set; }

    /// <summary>
    /// 开始时间
    /// </summary>
    [SugarColumn(ColumnName = "start_time", ColumnDescription = "开始时间", IsNullable = true)]
    public DateTime? StartTime { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    [SugarColumn(ColumnName = "end_time", ColumnDescription = "结束时间", IsNullable = true)]
    public DateTime? EndTime { get; set; }

    /// <summary>
    /// 到期时间
    /// </summary>
    [SugarColumn(ColumnName = "due_time", ColumnDescription = "到期时间", IsNullable = true)]
    public DateTime? DueTime { get; set; }

    /// <summary>
    /// 提醒时间
    /// </summary>
    [SugarColumn(ColumnName = "remind_time", ColumnDescription = "提醒时间", IsNullable = true)]
    public DateTime? RemindTime { get; set; }

    /// <summary>
    /// 处理意见
    /// </summary>
    [SugarColumn(ColumnName = "comment", ColumnDescription = "处理意见", Length = 500, IsNullable = true)]
    public string? Comment { get; set; }

    /// <summary>
    /// 是否超时(0=否,1=是)
    /// </summary>
    [SugarColumn(ColumnName = "is_timeout", ColumnDescription = "是否超时", IsNullable = false, DefaultValue = "0")]
    public int IsTimeout { get; set; } = 0;

    /// <summary>
    /// 是否已读(0=否,1=是)
    /// </summary>
    [SugarColumn(ColumnName = "is_read", ColumnDescription = "是否已读", IsNullable = false, DefaultValue = "0")]
    public int IsRead { get; set; } = 0;

    /// <summary>
    /// 是否已催办(0=否,1=是)
    /// </summary>
    [SugarColumn(ColumnName = "is_urged", ColumnDescription = "是否已催办", IsNullable = false, DefaultValue = "0")]
    public int IsUrged { get; set; } = 0;

    /// <summary>
    /// 表单数据
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(LeanWorkflowFormData.TaskId))]
    public virtual List<LeanWorkflowFormData> FormDataList { get; set; } = new();

    /// <summary>
    /// 变量数据
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(LeanWorkflowVariableData.TaskId))]
    public virtual List<LeanWorkflowVariableData> VariableDataList { get; set; } = new();

    /// <summary>
    /// 历史记录
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(LeanWorkflowHistory.TaskId))]
    public virtual List<LeanWorkflowHistory> Histories { get; set; } = new();

    /// <summary>
    /// 父任务ID(用于委派任务)
    /// </summary>
    [SugarColumn(ColumnName = "parent_task_id", ColumnDescription = "父任务ID", IsNullable = true)]
    public long? ParentTaskId { get; set; }
}