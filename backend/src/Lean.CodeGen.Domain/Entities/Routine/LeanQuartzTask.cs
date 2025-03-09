using SqlSugar;

namespace Lean.CodeGen.Domain.Entities.Routine;

/// <summary>
/// 定时任务实体类
/// </summary>
[SugarTable("lean_rou_quartz_task", "定时任务表")]
public class LeanQuartzTask : LeanBaseEntity
{
  /// <summary>
  /// 任务ID
  /// </summary>
  [SugarColumn(ColumnDescription = "任务ID", IsNullable = false)]
  public long TaskId { get; set; }

  /// <summary>
  /// 任务名称
  /// </summary>
  [SugarColumn(ColumnDescription = "任务名称", Length = 100, IsNullable = false, ColumnDataType = "nvarchar")]
  public string TaskName { get; set; } = string.Empty;

  /// <summary>
  /// 任务组名
  /// </summary>
  [SugarColumn(ColumnDescription = "任务组名", Length = 100, IsNullable = false, ColumnDataType = "nvarchar")]
  public string GroupName { get; set; } = string.Empty;

  /// <summary>
  /// 程序集名称
  /// </summary>
  [SugarColumn(ColumnDescription = "程序集名称", Length = 255, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? AssemblyName { get; set; }

  /// <summary>
  /// 任务所在类
  /// </summary>
  [SugarColumn(ColumnDescription = "任务所在类", Length = 255, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? ClassName { get; set; }

  /// <summary>
  /// 触发器类型（1=Simple简单触发器，2=Cron表达式触发器）
  /// </summary>
  [SugarColumn(ColumnDescription = "触发器类型", IsNullable = false)]
  public int TriggerType { get; set; }

  /// <summary>
  /// Cron表达式
  /// </summary>
  [SugarColumn(ColumnDescription = "Cron表达式", Length = 100, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? CronExpression { get; set; }

  /// <summary>
  /// 执行间隔时间（秒）
  /// </summary>
  [SugarColumn(ColumnDescription = "执行间隔时间（秒）", IsNullable = true)]
  public int? IntervalSecond { get; set; }

  /// <summary>
  /// API执行地址
  /// </summary>
  [SugarColumn(ColumnDescription = "API执行地址", Length = 500, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? ApiUrl { get; set; }

  /// <summary>
  /// 请求方式（GET、POST、PUT、DELETE等）
  /// </summary>
  [SugarColumn(ColumnDescription = "请求方式", Length = 10, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? RequestMethod { get; set; }

  /// <summary>
  /// 请求头
  /// </summary>
  [SugarColumn(ColumnDescription = "请求头", Length = -1, IsNullable = true, ColumnDataType = "ntext")]
  public string? RequestHeaders { get; set; }

  /// <summary>
  /// 任务类型（1=程序集，2=网络请求，3=SQL语句）
  /// </summary>
  [SugarColumn(ColumnDescription = "任务类型", IsNullable = false)]
  public int TaskType { get; set; }

  /// <summary>
  /// SQL语句
  /// </summary>
  [SugarColumn(ColumnDescription = "SQL语句", Length = -1, IsNullable = true, ColumnDataType = "ntext")]
  public string? SqlScript { get; set; }

  /// <summary>
  /// 开始时间
  /// </summary>
  [SugarColumn(ColumnDescription = "开始时间", IsNullable = false)]
  public DateTime StartTime { get; set; }

  /// <summary>
  /// 结束时间
  /// </summary>
  [SugarColumn(ColumnDescription = "结束时间", IsNullable = true)]
  public DateTime? EndTime { get; set; }

  /// <summary>
  /// 执行耗时（毫秒）
  /// </summary>
  [SugarColumn(ColumnDescription = "执行耗时（毫秒）", IsNullable = true)]
  public long? ElapsedTime { get; set; }

  /// <summary>
  /// 执行结果（0=失败，1=成功）
  /// </summary>
  [SugarColumn(ColumnDescription = "执行结果", IsNullable = false)]
  public int RunResult { get; set; }

  /// <summary>
  /// 错误信息
  /// </summary>
  [SugarColumn(ColumnDescription = "错误信息", Length = -1, IsNullable = true, ColumnDataType = "ntext")]
  public string? ErrorMessage { get; set; }

  /// <summary>
  /// 执行参数
  /// </summary>
  [SugarColumn(ColumnDescription = "执行参数", Length = -1, IsNullable = true, ColumnDataType = "ntext")]
  public string? TaskData { get; set; }

  /// <summary>
  /// 执行机器IP
  /// </summary>
  [SugarColumn(ColumnDescription = "执行机器IP", Length = 50, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? ServerIp { get; set; }

  /// <summary>
  /// 执行机器名称
  /// </summary>
  [SugarColumn(ColumnDescription = "执行机器名称", Length = 100, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? ServerName { get; set; }

  /// <summary>
  /// 重试次数
  /// </summary>
  [SugarColumn(ColumnDescription = "重试次数", IsNullable = false, DefaultValue = "0")]
  public int RetryCount { get; set; }

  /// <summary>
  /// 任务状态（0=停止，1=运行中）
  /// </summary>
  [SugarColumn(ColumnDescription = "任务状态", IsNullable = false, DefaultValue = "0")]
  public int TaskStatus { get; set; }

}