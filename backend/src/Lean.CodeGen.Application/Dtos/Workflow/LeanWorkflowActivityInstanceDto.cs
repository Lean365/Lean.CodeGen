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
  public long InstanceId { get; set; }

  /// <summary>
  /// 活动ID
  /// </summary>
  public string ActivityId { get; set; } = string.Empty;

  /// <summary>
  /// 活动名称
  /// </summary>
  public string ActivityName { get; set; } = string.Empty;

  /// <summary>
  /// 活动类型
  /// </summary>
  public string ActivityType { get; set; } = string.Empty;

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
  /// 输入数据JSON
  /// </summary>
  public string? InputData { get; set; }

  /// <summary>
  /// 输出数据JSON
  /// </summary>
  public string? OutputData { get; set; }

  /// <summary>
  /// 错误信息JSON
  /// </summary>
  public string? ErrorInfo { get; set; }

  /// <summary>
  /// 自定义属性JSON
  /// </summary>
  public string? CustomAttributes { get; set; }

  /// <summary>
  /// 创建时间
  /// </summary>
  public DateTime CreateTime { get; set; }

  /// <summary>
  /// 更新时间
  /// </summary>
  public DateTime? UpdateTime { get; set; }
}