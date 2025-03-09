//===================================================
// 项目名: Lean.CodeGen.Application
// 文件名: LeanQuartzLogDto.cs
// 功能描述: 定时任务日志相关DTO
// 创建时间: 2024-03-26
// 作者: Lean
// 版本: 1.0
//===================================================

using System;
using Lean.CodeGen.Common.Excel;
using Lean.CodeGen.Common.Models;

namespace Lean.CodeGen.Application.Dtos.Audit;

/// <summary>
/// 定时任务日志查询DTO
/// </summary>
public class LeanQuartzLogQueryDto : LeanPage
{
  /// <summary>
  /// 任务ID
  /// </summary>
  public long? TaskId { get; set; }

  /// <summary>
  /// 任务名称
  /// </summary>
  public string? TaskName { get; set; }

  /// <summary>
  /// 任务组名
  /// </summary>
  public string? GroupName { get; set; }

  /// <summary>
  /// 执行结果（0=失败，1=成功）
  /// </summary>
  public int? RunResult { get; set; }

  /// <summary>
  /// 开始时间范围-开始
  /// </summary>
  public DateTime? StartTimeBegin { get; set; }

  /// <summary>
  /// 开始时间范围-结束
  /// </summary>
  public DateTime? StartTimeEnd { get; set; }
}

/// <summary>
/// 定时任务日志详情DTO
/// </summary>
public class LeanQuartzLogDto
{
  /// <summary>
  /// 日志ID
  /// </summary>
  public long Id { get; set; }

  /// <summary>
  /// 任务ID
  /// </summary>
  public long TaskId { get; set; }

  /// <summary>
  /// 任务名称
  /// </summary>
  public string TaskName { get; set; } = string.Empty;

  /// <summary>
  /// 任务组名
  /// </summary>
  public string GroupName { get; set; } = string.Empty;

  /// <summary>
  /// 开始时间
  /// </summary>
  public DateTime StartTime { get; set; }

  /// <summary>
  /// 结束时间
  /// </summary>
  public DateTime? EndTime { get; set; }

  /// <summary>
  /// 执行耗时（毫秒）
  /// </summary>
  public long? ElapsedTime { get; set; }

  /// <summary>
  /// 执行结果（0=失败，1=成功）
  /// </summary>
  public int RunResult { get; set; }

  /// <summary>
  /// 错误信息
  /// </summary>
  public string? ErrorMessage { get; set; }

  /// <summary>
  /// 执行参数
  /// </summary>
  public string? TaskData { get; set; }

  /// <summary>
  /// 执行机器IP
  /// </summary>
  public string? ServerIp { get; set; }

  /// <summary>
  /// 执行机器名称
  /// </summary>
  public string? ServerName { get; set; }

  /// <summary>
  /// 重试次数
  /// </summary>
  public int RetryCount { get; set; }

  /// <summary>
  /// 创建时间
  /// </summary>
  public DateTime CreateTime { get; set; }

  /// <summary>
  /// 创建人
  /// </summary>
  public string? Creator { get; set; }
}

/// <summary>
/// 定时任务日志导出DTO
/// </summary>
public class LeanQuartzLogExportDto
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
  /// 开始时间
  /// </summary>
  [LeanExcelColumn("开始时间")]
  public DateTime StartTime { get; set; }

  /// <summary>
  /// 结束时间
  /// </summary>
  [LeanExcelColumn("结束时间")]
  public DateTime? EndTime { get; set; }

  /// <summary>
  /// 执行耗时（毫秒）
  /// </summary>
  [LeanExcelColumn("执行耗时")]
  public long? ElapsedTime { get; set; }

  /// <summary>
  /// 执行结果
  /// </summary>
  [LeanExcelColumn("执行结果")]
  public string RunResultName { get; set; } = string.Empty;

  /// <summary>
  /// 错误信息
  /// </summary>
  [LeanExcelColumn("错误信息")]
  public string? ErrorMessage { get; set; }

  /// <summary>
  /// 执行机器IP
  /// </summary>
  [LeanExcelColumn("执行机器IP")]
  public string? ServerIp { get; set; }

  /// <summary>
  /// 执行机器名称
  /// </summary>
  [LeanExcelColumn("执行机器名称")]
  public string? ServerName { get; set; }

  /// <summary>
  /// 重试次数
  /// </summary>
  [LeanExcelColumn("重试次数")]
  public int RetryCount { get; set; }

  /// <summary>
  /// 创建时间
  /// </summary>
  [LeanExcelColumn("创建时间")]
  public DateTime CreateTime { get; set; }
}