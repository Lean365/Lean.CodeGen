//===================================================
// 项目名: Lean.CodeGen.Application
// 文件名: LeanGenTaskDto.cs
// 功能描述: 代码生成任务相关DTO
// 创建时间: 2024-03-26
// 作者: Lean
// 版本: 1.0
//===================================================

using System;
using System.Collections.Generic;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Common.Excel;

namespace Lean.CodeGen.Application.Dtos.Generator
{
  /// <summary>
  /// 代码生成任务查询DTO
  /// </summary>
  public class LeanGenTaskDto
  {
    /// <summary>
    /// 主键
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 任务名称
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// 任务状态（0-等待执行，1-执行中，2-执行成功，3-执行失败）
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
    /// 错误信息
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// 配置Id
    /// </summary>
    public long ConfigId { get; set; }

    /// <summary>
    /// 配置名称
    /// </summary>
    public string ConfigName { get; set; } = default!;

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 生成历史记录
    /// </summary>
    public List<LeanGenHistoryDto> Histories { get; set; } = new();
  }

  /// <summary>
  /// 代码生成历史记录查询DTO
  /// </summary>
  public class LeanGenHistoryDto
  {
    /// <summary>
    /// 主键
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 任务Id
    /// </summary>
    public long TaskId { get; set; }

    /// <summary>
    /// 任务名称
    /// </summary>
    public string TaskName { get; set; } = default!;

    /// <summary>
    /// 表Id
    /// </summary>
    public long TableId { get; set; }

    /// <summary>
    /// 表名称
    /// </summary>
    public string TableName { get; set; } = default!;

    /// <summary>
    /// 生成状态（0-成功，1-失败）
    /// </summary>
    public int Status { get; set; }

    /// <summary>
    /// 生成时间
    /// </summary>
    public DateTime GenerateTime { get; set; }

    /// <summary>
    /// 生成路径
    /// </summary>
    public string OutputPath { get; set; } = default!;

    /// <summary>
    /// 错误信息
    /// </summary>
    public string? ErrorMessage { get; set; }
  }

  /// <summary>
  /// 代码生成任务创建DTO
  /// </summary>
  public class LeanCreateGenTaskDto
  {
    /// <summary>
    /// 任务名称
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// 配置Id
    /// </summary>
    public long ConfigId { get; set; }

    /// <summary>
    /// 需要生成的表Id列表
    /// </summary>
    public List<long> TableIds { get; set; } = new();
  }

  /// <summary>
  /// 代码生成任务更新DTO
  /// </summary>
  public class LeanUpdateGenTaskDto : LeanCreateGenTaskDto
  {
    /// <summary>
    /// 主键
    /// </summary>
    public long Id { get; set; }
  }

  /// <summary>
  /// 代码生成任务查询条件DTO
  /// </summary>
  public class LeanGenTaskQueryDto : LeanPage
  {
    /// <summary>
    /// 任务名称
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// 任务状态
    /// </summary>
    public int? Status { get; set; }

    /// <summary>
    /// 配置Id
    /// </summary>
    public long? ConfigId { get; set; }

    /// <summary>
    /// 开始时间范围-开始
    /// </summary>
    public DateTime? StartTimeBegin { get; set; }

    /// <summary>
    /// 开始时间范围-结束
    /// </summary>
    public DateTime? StartTimeEnd { get; set; }

    /// <summary>
    /// 创建时间范围-开始
    /// </summary>
    public DateTime? CreateTimeBegin { get; set; }

    /// <summary>
    /// 创建时间范围-结束
    /// </summary>
    public DateTime? CreateTimeEnd { get; set; }
  }

  /// <summary>
  /// 代码生成任务导出DTO
  /// </summary>
  public class LeanGenTaskExportDto
  {
    /// <summary>
    /// 任务名称
    /// </summary>
    [LeanExcelColumn("任务名称", DataType = LeanExcelDataType.String)]
    public string Name { get; set; } = default!;

    /// <summary>
    /// 任务状态
    /// </summary>
    [LeanExcelColumn("任务状态", DataType = LeanExcelDataType.Int)]
    public int Status { get; set; }

    /// <summary>
    /// 开始时间
    /// </summary>
    [LeanExcelColumn("开始时间", DataType = LeanExcelDataType.DateTime, Format = "yyyy-MM-dd HH:mm:ss")]
    public DateTime? StartTime { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    [LeanExcelColumn("结束时间", DataType = LeanExcelDataType.DateTime, Format = "yyyy-MM-dd HH:mm:ss")]
    public DateTime? EndTime { get; set; }

    /// <summary>
    /// 错误信息
    /// </summary>
    [LeanExcelColumn("错误信息", DataType = LeanExcelDataType.String)]
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// 配置名称
    /// </summary>
    [LeanExcelColumn("配置名称", DataType = LeanExcelDataType.String)]
    public string ConfigName { get; set; } = default!;

    /// <summary>
    /// 创建时间
    /// </summary>
    [LeanExcelColumn("创建时间", DataType = LeanExcelDataType.DateTime, Format = "yyyy-MM-dd HH:mm:ss")]
    public DateTime CreateTime { get; set; }
  }

  /// <summary>
  /// 代码生成任务导入DTO
  /// </summary>
  public class LeanGenTaskImportDto
  {
    /// <summary>
    /// 任务名称
    /// </summary>
    [LeanExcelColumn("任务名称", DataType = LeanExcelDataType.String)]
    public string Name { get; set; } = default!;

    /// <summary>
    /// 配置Id
    /// </summary>
    [LeanExcelColumn("配置Id", DataType = LeanExcelDataType.Long)]
    public long ConfigId { get; set; }

    /// <summary>
    /// 需要生成的表Id列表（逗号分隔）
    /// </summary>
    [LeanExcelColumn("需要生成的表Id列表", DataType = LeanExcelDataType.String)]
    public string TableIds { get; set; } = default!;
  }

  /// <summary>
  /// 代码生成任务导入模板DTO
  /// </summary>
  public class LeanGenTaskImportTemplateDto
  {
    /// <summary>
    /// 任务名称
    /// </summary>
    [LeanExcelColumn("任务名称", DataType = LeanExcelDataType.String)]
    public string Name { get; set; } = "示例任务";

    /// <summary>
    /// 配置Id
    /// </summary>
    [LeanExcelColumn("配置Id", DataType = LeanExcelDataType.Long)]
    public long ConfigId { get; set; } = 1;

    /// <summary>
    /// 需要生成的表Id列表（逗号分隔）
    /// </summary>
    [LeanExcelColumn("需要生成的表Id列表", DataType = LeanExcelDataType.String)]
    public string TableIds { get; set; } = "1,2,3";
  }
}