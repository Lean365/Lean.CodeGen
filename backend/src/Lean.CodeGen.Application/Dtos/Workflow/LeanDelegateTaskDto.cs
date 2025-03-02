namespace Lean.CodeGen.Application.Dtos.Workflow;

/// <summary>
/// 委派任务请求DTO
/// </summary>
public class LeanDelegateTaskDto
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
    /// 目标用户ID
    /// </summary>
    public long TargetUserId { get; set; }

    /// <summary>
    /// 目标用户姓名
    /// </summary>
    public string TargetUserName { get; set; }

    /// <summary>
    /// 委派说明
    /// </summary>
    public string Comment { get; set; }
}