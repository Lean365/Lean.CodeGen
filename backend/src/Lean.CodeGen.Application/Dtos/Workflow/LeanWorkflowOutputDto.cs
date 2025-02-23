namespace Lean.CodeGen.Application.Dtos.Workflow;

/// <summary>
/// 工作流输出DTO
/// </summary>
public class LeanWorkflowOutputDto
{
  /// <summary>
  /// ID
  /// </summary>
  public long Id { get; set; }

  /// <summary>
  /// 活动实例ID
  /// </summary>
  public long ActivityInstanceId { get; set; }

  /// <summary>
  /// 活动实例
  /// </summary>
  public LeanWorkflowActivityInstanceDto ActivityInstance { get; set; } = null!;

  /// <summary>
  /// 输出名称
  /// </summary>
  public string OutputName { get; set; } = string.Empty;

  /// <summary>
  /// 输出值
  /// </summary>
  public string? OutputValue { get; set; }

  /// <summary>
  /// 输出类型
  /// </summary>
  public string? OutputType { get; set; }

  /// <summary>
  /// 输出描述
  /// </summary>
  public string? OutputDescription { get; set; }

  /// <summary>
  /// 创建时间
  /// </summary>
  public DateTime CreateTime { get; set; }

  /// <summary>
  /// 更新时间
  /// </summary>
  public DateTime? UpdateTime { get; set; }
}