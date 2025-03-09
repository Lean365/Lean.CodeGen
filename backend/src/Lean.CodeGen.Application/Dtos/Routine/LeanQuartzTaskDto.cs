using System.Text.Json;
using System.ComponentModel.DataAnnotations;
using Lean.CodeGen.Common.Excel;

namespace Lean.CodeGen.Application.Dtos.Routine;

/// <summary>
/// 定时任务查询对象
/// </summary>
public class LeanQuartzTaskQueryDto : LeanPage
{
  /// <summary>
  /// 任务名称
  /// </summary>
  public string? TaskName { get; set; }

  /// <summary>
  /// 任务组名
  /// </summary>
  public string? GroupName { get; set; }

  /// <summary>
  /// 任务类型（1=程序集，2=网络请求，3=SQL语句）
  /// </summary>
  public int? TaskType { get; set; }

  /// <summary>
  /// 任务状态（0=停止，1=运行中）
  /// </summary>
  public int? TaskStatus { get; set; }

  /// <summary>
  /// 创建时间范围开始
  /// </summary>
  public DateTime? CreateTimeStart { get; set; }

  /// <summary>
  /// 创建时间范围结束
  /// </summary>
  public DateTime? CreateTimeEnd { get; set; }

  /// <summary>
  /// 上次执行时间范围开始
  /// </summary>
  public DateTime? LastRunTimeStart { get; set; }

  /// <summary>
  /// 上次执行时间范围结束
  /// </summary>
  public DateTime? LastRunTimeEnd { get; set; }
}

/// <summary>
/// 定时任务详情对象
/// </summary>
public class LeanQuartzTaskDto : LeanBaseDto
{
  /// <summary>
  /// 任务名称
  /// </summary>
  public string TaskName { get; set; } = string.Empty;

  /// <summary>
  /// 任务组名
  /// </summary>
  public string GroupName { get; set; } = string.Empty;

  /// <summary>
  /// 任务类型（1=程序集，2=网络请求，3=SQL语句）
  /// </summary>
  public int TaskType { get; set; }

  /// <summary>
  /// 触发器类型（1=Simple简单触发器，2=Cron表达式触发器）
  /// </summary>
  public int TriggerType { get; set; }

  /// <summary>
  /// Cron表达式
  /// </summary>
  public string? CronExpression { get; set; }

  /// <summary>
  /// 执行间隔时间（秒）
  /// </summary>
  public int? IntervalSecond { get; set; }

  /// <summary>
  /// 程序集名称
  /// </summary>
  public string? AssemblyName { get; set; }

  /// <summary>
  /// 任务所在类
  /// </summary>
  public string? ClassName { get; set; }

  /// <summary>
  /// API执行地址
  /// </summary>
  public string? ApiUrl { get; set; }

  /// <summary>
  /// 请求方式（GET、POST、PUT、DELETE等）
  /// </summary>
  public string? RequestMethod { get; set; }

  /// <summary>
  /// 请求头
  /// </summary>
  public string? RequestHeaders { get; set; }

  /// <summary>
  /// SQL语句
  /// </summary>
  public string? SqlScript { get; set; }

  /// <summary>
  /// 任务状态（0=停止，1=运行中）
  /// </summary>
  public int TaskStatus { get; set; }

  /// <summary>
  /// 上次执行时间
  /// </summary>
  public DateTime? LastRunTime { get; set; }

  /// <summary>
  /// 下次执行时间
  /// </summary>
  public DateTime? NextRunTime { get; set; }

  /// <summary>
  /// 备注
  /// </summary>
  public string? Remark { get; set; }
}

/// <summary>
/// 定时任务创建对象
/// </summary>
public class LeanQuartzTaskCreateDto
{
  /// <summary>
  /// 任务名称
  /// </summary>
  [Required(ErrorMessage = "任务名称不能为空")]
  [StringLength(100, MinimumLength = 2, ErrorMessage = "任务名称长度必须在2-100个字符之间")]
  public string TaskName { get; set; } = string.Empty;

  /// <summary>
  /// 任务组名
  /// </summary>
  [Required(ErrorMessage = "任务组名不能为空")]
  [StringLength(100, MinimumLength = 2, ErrorMessage = "任务组名长度必须在2-100个字符之间")]
  public string GroupName { get; set; } = string.Empty;

  /// <summary>
  /// 任务类型（1=程序集，2=网络请求，3=SQL语句）
  /// </summary>
  [Required(ErrorMessage = "任务类型不能为空")]
  public int TaskType { get; set; }

  /// <summary>
  /// 触发器类型（1=Simple简单触发器，2=Cron表达式触发器）
  /// </summary>
  [Required(ErrorMessage = "触发器类型不能为空")]
  public int TriggerType { get; set; }

  /// <summary>
  /// Cron表达式
  /// </summary>
  [StringLength(100, ErrorMessage = "Cron表达式长度不能超过100个字符")]
  public string? CronExpression { get; set; }

  /// <summary>
  /// 执行间隔时间（秒）
  /// </summary>
  public int? IntervalSecond { get; set; }

  /// <summary>
  /// 程序集名称
  /// </summary>
  [StringLength(200, ErrorMessage = "程序集名称长度不能超过200个字符")]
  public string? AssemblyName { get; set; }

  /// <summary>
  /// 任务所在类
  /// </summary>
  [StringLength(200, ErrorMessage = "任务所在类长度不能超过200个字符")]
  public string? ClassName { get; set; }

  /// <summary>
  /// API执行地址
  /// </summary>
  [StringLength(500, ErrorMessage = "API执行地址长度不能超过500个字符")]
  public string? ApiUrl { get; set; }

  /// <summary>
  /// 请求方式（GET、POST、PUT、DELETE等）
  /// </summary>
  [StringLength(10, ErrorMessage = "请求方式长度不能超过10个字符")]
  public string? RequestMethod { get; set; }

  /// <summary>
  /// 请求头
  /// </summary>
  [StringLength(1000, ErrorMessage = "请求头长度不能超过1000个字符")]
  public string? RequestHeaders { get; set; }

  /// <summary>
  /// SQL语句
  /// </summary>
  [StringLength(4000, ErrorMessage = "SQL语句长度不能超过4000个字符")]
  public string? SqlScript { get; set; }

  /// <summary>
  /// 备注
  /// </summary>
  [StringLength(500, ErrorMessage = "备注长度不能超过500个字符")]
  public string? Remark { get; set; }
}

/// <summary>
/// 定时任务更新对象
/// </summary>
public class LeanQuartzTaskUpdateDto : LeanQuartzTaskCreateDto
{
  /// <summary>
  /// 主键
  /// </summary>
  [Required(ErrorMessage = "任务ID不能为空")]
  public long Id { get; set; }
}

/// <summary>
/// 定时任务导入对象
/// </summary>
public class LeanQuartzTaskImportDto
{
  /// <summary>
  /// 任务名称
  /// </summary>
  [LeanExcelColumn("任务名称")]
  public string TaskName { get; set; } = string.Empty;

  /// <summary>
  /// 任务组名
  /// </summary>
  [LeanExcelColumn("任务组名")]
  public string GroupName { get; set; } = string.Empty;

  /// <summary>
  /// 任务类型（1=程序集，2=网络请求，3=SQL语句）
  /// </summary>
  [LeanExcelColumn("任务类型")]
  public int TaskType { get; set; }

  /// <summary>
  /// 触发器类型（1=Simple简单触发器，2=Cron表达式触发器）
  /// </summary>
  [LeanExcelColumn("触发器类型")]
  public int TriggerType { get; set; }

  /// <summary>
  /// Cron表达式
  /// </summary>
  [LeanExcelColumn("Cron表达式")]
  public string? CronExpression { get; set; }

  /// <summary>
  /// 执行间隔时间（秒）
  /// </summary>
  [LeanExcelColumn("执行间隔时间")]
  public int? IntervalSecond { get; set; }
}

/// <summary>
/// 定时任务导出对象
/// </summary>
public class LeanQuartzTaskExportDto
{
  /// <summary>
  /// 任务名称
  /// </summary>
  [LeanExcelColumn("任务名称")]
  public string TaskName { get; set; } = string.Empty;

  /// <summary>
  /// 任务组名
  /// </summary>
  [LeanExcelColumn("任务组名")]
  public string GroupName { get; set; } = string.Empty;

  /// <summary>
  /// 任务类型
  /// </summary>
  [LeanExcelColumn("任务类型")]
  public string TaskTypeName { get; set; } = string.Empty;

  /// <summary>
  /// 触发器类型
  /// </summary>
  [LeanExcelColumn("触发器类型")]
  public string TriggerTypeName { get; set; } = string.Empty;

  /// <summary>
  /// Cron表达式
  /// </summary>
  [LeanExcelColumn("Cron表达式")]
  public string? CronExpression { get; set; }

  /// <summary>
  /// 执行间隔时间
  /// </summary>
  [LeanExcelColumn("执行间隔时间")]
  public string? IntervalSecond { get; set; }

  /// <summary>
  /// 任务状态
  /// </summary>
  [LeanExcelColumn("任务状态")]
  public string TaskStatusName { get; set; } = string.Empty;

  /// <summary>
  /// 上次执行时间
  /// </summary>
  [LeanExcelColumn("上次执行时间")]
  public DateTime? LastRunTime { get; set; }

  /// <summary>
  /// 下次执行时间
  /// </summary>
  [LeanExcelColumn("下次执行时间")]
  public DateTime? NextRunTime { get; set; }

  /// <summary>
  /// 创建时间
  /// </summary>
  [LeanExcelColumn("创建时间")]
  public DateTime CreateTime { get; set; }

  /// <summary>
  /// 备注
  /// </summary>
  [LeanExcelColumn("备注")]
  public string? Remark { get; set; }
}

#region 状态更新
/// <summary>
/// 定时任务状态更新对象
/// </summary>
public class LeanQuartzTaskStatusDto
{
  /// <summary>
  /// 任务ID
  /// </summary>
  [Required(ErrorMessage = "任务ID不能为空")]
  public long Id { get; set; }

  /// <summary>
  /// 任务状态（0=停止，1=运行中）
  /// </summary>
  [Required(ErrorMessage = "任务状态不能为空")]
  public int TaskStatus { get; set; }
}
#endregion

/// <summary>
/// 定时任务导入模板对象
/// </summary>
public class LeanQuartzTaskImportTemplateDto
{
  /// <summary>
  /// 任务名称
  /// </summary>
  [LeanExcelColumn("任务名称")]
  public string TaskName { get; set; } = "示例任务";

  /// <summary>
  /// 任务组名
  /// </summary>
  [LeanExcelColumn("任务组名")]
  public string GroupName { get; set; } = "默认组";

  /// <summary>
  /// 任务类型（1=程序集，2=网络请求，3=SQL语句）
  /// </summary>
  [LeanExcelColumn("任务类型")]
  public int TaskType { get; set; } = 1;

  /// <summary>
  /// 触发器类型（1=Simple简单触发器，2=Cron表达式触发器）
  /// </summary>
  [LeanExcelColumn("触发器类型")]
  public int TriggerType { get; set; } = 2;

  /// <summary>
  /// Cron表达式
  /// </summary>
  [LeanExcelColumn("Cron表达式")]
  public string CronExpression { get; set; } = "0 0 * * * ?";

  /// <summary>
  /// 执行间隔时间（秒）
  /// </summary>
  [LeanExcelColumn("执行间隔时间")]
  public int? IntervalSecond { get; set; } = 3600;
}