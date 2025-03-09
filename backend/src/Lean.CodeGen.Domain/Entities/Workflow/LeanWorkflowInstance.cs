using SqlSugar;
using Lean.CodeGen.Common.Enums;

namespace Lean.CodeGen.Domain.Entities.Workflow;

/// <summary>
/// 工作流实例实体
/// </summary>
[SugarTable("lean_wk_instance", "工作流实例表")]
[SugarIndex("uk_business", nameof(BusinessKey), OrderByType.Asc, true)]
public class LeanWorkflowInstance : LeanBaseEntity
{
    /// <summary>
    /// 流程定义ID
    /// </summary>
    [SugarColumn(ColumnName = "definition_id", ColumnDescription = "流程定义ID", IsNullable = false)]
    public long DefinitionId { get; set; }

    /// <summary>
    /// 流程定义版本
    /// </summary>
    [SugarColumn(ColumnName = "definition_version", ColumnDescription = "流程定义版本", IsNullable = false)]
    public int DefinitionVersion { get; set; }

    /// <summary>
    /// 业务主键
    /// </summary>
    [SugarColumn(ColumnName = "business_key", ColumnDescription = "业务主键", Length = 50, IsNullable = false)]
    public string BusinessKey { get; set; } = string.Empty;

    /// <summary>
    /// 业务类型
    /// </summary>
    [SugarColumn(ColumnName = "business_type", ColumnDescription = "业务类型", Length = 50, IsNullable = false)]
    public string BusinessType { get; set; } = string.Empty;

    /// <summary>
    /// 实例标题
    /// </summary>
    [SugarColumn(ColumnName = "title", ColumnDescription = "实例标题", Length = 200, IsNullable = false)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 优先级
    /// </summary>
    [SugarColumn(ColumnName = "priority", ColumnDescription = "优先级", IsNullable = false)]
    public int Priority { get; set; }

    /// <summary>
    /// 发起人ID
    /// </summary>
    [SugarColumn(ColumnName = "initiator_id", ColumnDescription = "发起人ID", IsNullable = false)]
    public long InitiatorId { get; set; }

    /// <summary>
    /// 发起人名称
    /// </summary>
    [SugarColumn(ColumnName = "initiator_name", ColumnDescription = "发起人名称", Length = 50, IsNullable = false)]
    public string InitiatorName { get; set; } = string.Empty;

    /// <summary>
    /// 发起人部门ID
    /// </summary>
    [SugarColumn(ColumnName = "initiator_dept_id", ColumnDescription = "发起人部门ID", IsNullable = false)]
    public long InitiatorDeptId { get; set; }

    /// <summary>
    /// 发起人部门名称
    /// </summary>
    [SugarColumn(ColumnName = "initiator_dept_name", ColumnDescription = "发起人部门名称", Length = 50, IsNullable = false)]
    public string InitiatorDeptName { get; set; } = string.Empty;

    /// <summary>
    /// 当前节点ID
    /// </summary>
    [SugarColumn(ColumnName = "current_node_id", ColumnDescription = "当前节点ID", Length = 50, IsNullable = true)]
    public string? CurrentNodeId { get; set; }

    /// <summary>
    /// 当前节点名称
    /// </summary>
    [SugarColumn(ColumnName = "current_node_name", ColumnDescription = "当前节点名称", Length = 50, IsNullable = true)]
    public string? CurrentNodeName { get; set; }

    /// <summary>
    /// 工作流状态
    /// </summary>
    [SugarColumn(ColumnName = "workflow_status", ColumnDescription = "工作流状态", IsNullable = false)]
    public int WorkflowStatus { get; set; }

    /// <summary>
    /// 是否暂停(0=否,1=是)
    /// </summary>
    [SugarColumn(ColumnName = "is_suspended", ColumnDescription = "是否暂停", IsNullable = false, DefaultValue = "0")]
    public int IsSuspended { get; set; }

    /// <summary>
    /// 是否已归档(0=否,1=是)
    /// </summary>
    [SugarColumn(ColumnName = "is_archived", ColumnDescription = "是否已归档", IsNullable = false, DefaultValue = "0")]
    public int IsArchived { get; set; } = 0;

    /// <summary>
    /// 开始时间
    /// </summary>
    [SugarColumn(ColumnName = "start_time", ColumnDescription = "开始时间", IsNullable = false)]
    public DateTime StartTime { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    [SugarColumn(ColumnName = "end_time", ColumnDescription = "结束时间", IsNullable = true)]
    public DateTime? EndTime { get; set; }

    /// <summary>
    /// 暂停时间
    /// </summary>
    [SugarColumn(ColumnName = "suspend_time", ColumnDescription = "暂停时间", IsNullable = true)]
    public DateTime? SuspendTime { get; set; }

    /// <summary>
    /// 活动实例列表
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(LeanWorkflowActivityInstance.WorkflowInstanceId))]
    public virtual List<LeanWorkflowActivityInstance> Activities { get; set; } = new();

    /// <summary>
    /// 任务列表
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(LeanWorkflowTask.InstanceId))]
    public virtual List<LeanWorkflowTask> Tasks { get; set; } = new();

    /// <summary>
    /// 表单数据列表
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(LeanWorkflowFormData.InstanceId))]
    public virtual List<LeanWorkflowFormData> FormData { get; set; } = new();

    /// <summary>
    /// 变量数据列表
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(LeanWorkflowVariableData.InstanceId))]
    public virtual List<LeanWorkflowVariableData> Variables { get; set; } = new();

    /// <summary>
    /// 历史记录列表
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(LeanWorkflowHistory.InstanceId))]
    public virtual List<LeanWorkflowHistory> History { get; set; } = new();
}