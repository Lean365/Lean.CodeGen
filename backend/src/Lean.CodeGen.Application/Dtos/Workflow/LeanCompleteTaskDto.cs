namespace Lean.CodeGen.Application.Dtos.Workflow;

/// <summary>
/// 完成任务请求DTO
/// </summary>
public class LeanCompleteTaskDto
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
    /// 流程变量
    /// </summary>
    public Dictionary<string, object> Variables { get; set; }

    /// <summary>
    /// 表单数据
    /// </summary>
    public Dictionary<string, object> FormData { get; set; }
}