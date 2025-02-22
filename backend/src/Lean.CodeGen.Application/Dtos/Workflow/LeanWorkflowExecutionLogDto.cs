namespace Lean.CodeGen.Application.Dtos.Workflow;

/// <summary>
/// 工作流执行日志DTO
/// </summary>
public class LeanWorkflowExecutionLogDto
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
  /// 活动实例ID
  /// </summary>
  public long? ActivityInstanceId { get; set; }

  /// <summary>
  /// 活动ID
  /// </summary>
  public string? ActivityId { get; set; }

  /// <summary>
  /// 活动名称
  /// </summary>
  public string? ActivityName { get; set; }

  /// <summary>
  /// 活动类型
  /// </summary>
  public string? ActivityType { get; set; }

  /// <summary>
  /// 日志级别(Debug=0,Info=1,Warning=2,Error=3)
  /// </summary>
  public int LogLevel { get; set; }

  /// <summary>
  /// 日志消息
  /// </summary>
  public string Message { get; set; } = string.Empty;

  /// <summary>
  /// 日志数据JSON
  /// </summary>
  public string? LogData { get; set; }

  /// <summary>
  /// 异常信息
  /// </summary>
  public string? Exception { get; set; }

  /// <summary>
  /// 堆栈跟踪
  /// </summary>
  public string? StackTrace { get; set; }

  /// <summary>
  /// 创建时间
  /// </summary>
  public DateTime CreateTime { get; set; }
}