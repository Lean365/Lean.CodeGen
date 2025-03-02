namespace Lean.CodeGen.Application.Dtos.Workflow;

/// <summary>
/// 撤回任务请求DTO
/// </summary>
public class LeanWithdrawTaskDto
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
  /// 撤回说明
  /// </summary>
  public string Comment { get; set; }
}