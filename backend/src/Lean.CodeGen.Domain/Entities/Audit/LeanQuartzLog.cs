using SqlSugar;

namespace Lean.CodeGen.Domain.Entities.Audit;

/// <summary>
/// 定时任务日志实体类
/// </summary>
[SugarTable("lean_quartz_log", "定时任务日志表")]
public class LeanQuartzLog : LeanBaseEntity
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
}