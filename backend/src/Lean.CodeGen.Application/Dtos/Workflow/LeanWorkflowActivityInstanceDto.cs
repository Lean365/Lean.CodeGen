using Lean.CodeGen.Common.Enums;

namespace Lean.CodeGen.Application.Dtos.Workflow;

/// <summary>
/// 工作流活动实例DTO
/// </summary>
public class LeanWorkflowActivityInstanceDto
{
  /// <summary>
  /// ID
  /// </summary>
  public long Id { get; set; }

  /// <summary>
  /// 工作流实例ID
  /// </summary>
  public long WorkflowInstanceId { get; set; }

  /// <summary>
  /// 工作流实例
  /// </summary>
  public LeanWorkflowInstanceDto Instance { get; set; } = null!;

  /// <summary>
  /// 活动类型
  /// </summary>
  public string ActivityType { get; set; } = string.Empty;

  /// <summary>
  /// 活动名称
  /// </summary>
  public string ActivityName { get; set; } = string.Empty;

  /// <summary>
  /// 活动状态
  /// </summary>
  public int ActivityStatus { get; set; }

  /// <summary>
  /// 开始时间
  /// </summary>
  public DateTime? StartTime { get; set; }

  /// <summary>
  /// 结束时间
  /// </summary>
  public DateTime? EndTime { get; set; }

  /// <summary>
  /// 输入参数JSON
  /// </summary>
  public string? InputParameters { get; set; }

  /// <summary>
  /// 输出参数JSON
  /// </summary>
  public string? OutputParameters { get; set; }

  /// <summary>
  /// 属性值JSON
  /// </summary>
  public string? PropertyValues { get; set; }

  /// <summary>
  /// 结果值JSON
  /// </summary>
  public string? OutcomeValues { get; set; }

  /// <summary>
  /// 错误信息JSON
  /// </summary>
  public string? ErrorInfo { get; set; }

  /// <summary>
  /// 自定义属性JSON
  /// </summary>
  public string? CustomAttributes { get; set; }

  /// <summary>
  /// 补偿列表
  /// </summary>
  public List<LeanWorkflowCompensationDto> Compensations { get; set; } = new();

  /// <summary>
  /// 输出列表
  /// </summary>
  public List<LeanWorkflowOutputDto> Outputs { get; set; } = new();

  /// <summary>
  /// 结果列表
  /// </summary>
  public List<LeanWorkflowOutcomeDto> Outcomes { get; set; } = new();

  /// <summary>
  /// 创建时间
  /// </summary>
  public DateTime CreateTime { get; set; }

  /// <summary>
  /// 更新时间
  /// </summary>
  public DateTime? UpdateTime { get; set; }
}