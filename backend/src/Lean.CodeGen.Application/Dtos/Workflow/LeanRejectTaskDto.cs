namespace Lean.CodeGen.Application.Dtos.Workflow;

/// <summary>
/// 驳回任务请求DTO
/// </summary>
public class LeanRejectTaskDto
{
    /// <summary>
    /// 操作人ID
    /// </summary>
    public long OperatorId { get; set; }

    /// <summary>
    /// 操作人姓名
    /// </summary>
    public string OperatorName { get; set; }

    /// <summary>
    /// 审批意见
    /// </summary>
    public string Comment { get; set; }

    /// <summary>
    /// 目标活动节点ID
    /// </summary>
    public string TargetActivityId { get; set; }
}