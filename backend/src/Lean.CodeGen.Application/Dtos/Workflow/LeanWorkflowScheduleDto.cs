namespace Lean.CodeGen.Application.Dtos.Workflow;

/// <summary>
/// 工作流调度DTO
/// </summary>
public class LeanWorkflowScheduleDto
{
  /// <summary>
  /// ID
  /// </summary>
  public long Id { get; set; }

  /// <summary>
  /// 工作流定义ID
  /// </summary>
  public long DefinitionId { get; set; }

  /// <summary>
  /// 调度名称
  /// </summary>
  public string ScheduleName { get; set; } = string.Empty;

  /// <summary>
  /// 调度类型(Cron=Cron表达式,Simple=简单调度,Calendar=日历调度)
  /// </summary>
  public string ScheduleType { get; set; } = string.Empty;

  /// <summary>
  /// 调度表达式
  /// </summary>
  public string ScheduleExpression { get; set; } = string.Empty;

  /// <summary>
  /// 调度配置JSON
  /// </summary>
  public string? ScheduleConfig { get; set; }

  /// <summary>
  /// 是否启用
  /// 0-停用
  /// 1-启用
  /// </summary>
  public int Status { get; set; }

  /// <summary>
  /// 开始时间
  /// </summary>
  public DateTime? StartTime { get; set; }

  /// <summary>
  /// 结束时间
  /// </summary>
  public DateTime? EndTime { get; set; }

  /// <summary>
  /// 上次执行时间
  /// </summary>
  public DateTime? LastExecuteTime { get; set; }

  /// <summary>
  /// 下次执行时间
  /// </summary>
  public DateTime? NextExecuteTime { get; set; }

  /// <summary>
  /// 执行次数
  /// </summary>
  public int ExecuteCount { get; set; }

  /// <summary>
  /// 最大执行次数(0=无限制)
  /// </summary>
  public int MaxExecuteCount { get; set; }

  /// <summary>
  /// 输入参数JSON
  /// </summary>
  public string? InputParameters { get; set; }

  /// <summary>
  /// 自定义属性JSON
  /// </summary>
  public string? CustomAttributes { get; set; }

  /// <summary>
  /// 描述
  /// </summary>
  public string? Description { get; set; }

  /// <summary>
  /// 创建时间
  /// </summary>
  public DateTime CreateTime { get; set; }

  /// <summary>
  /// 更新时间
  /// </summary>
  public DateTime? UpdateTime { get; set; }
}