//===================================================
// 项目名: Lean.CodeGen.Application
// 文件名: LeanGenHistoryDto.cs
// 功能描述: 代码生成历史相关DTO
// 创建时间: 2024-03-26
// 作者: Lean
// 版本: 1.0
//===================================================

using System;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Common.Excel;

namespace Lean.CodeGen.Application.Dtos.Generator
{
  /// <summary>
  /// 代码生成历史查询DTO
  /// </summary>
  public class LeanGenHistoryQueryDto : LeanPage
  {
    /// <summary>
    /// 任务Id
    /// </summary>
    public long? TaskId { get; set; }

    /// <summary>
    /// 表Id
    /// </summary>
    public long? TableId { get; set; }

    /// <summary>
    /// 生成状态
    /// </summary>
    public int? Status { get; set; }

    /// <summary>
    /// 生成时间范围-开始
    /// </summary>
    public DateTime? GenerateTimeBegin { get; set; }

    /// <summary>
    /// 生成时间范围-结束
    /// </summary>
    public DateTime? GenerateTimeEnd { get; set; }

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
  /// 代码生成历史导出DTO
  /// </summary>
  public class LeanGenHistoryExportDto
  {
    /// <summary>
    /// 任务名称
    /// </summary>
    [LeanExcelColumn("任务名称", DataType = LeanExcelDataType.String)]
    public string TaskName { get; set; } = default!;

    /// <summary>
    /// 表名称
    /// </summary>
    [LeanExcelColumn("表名称", DataType = LeanExcelDataType.String)]
    public string TableName { get; set; } = default!;

    /// <summary>
    /// 生成状态
    /// </summary>
    [LeanExcelColumn("生成状态", DataType = LeanExcelDataType.Int)]
    public int Status { get; set; }

    /// <summary>
    /// 生成时间
    /// </summary>
    [LeanExcelColumn("生成时间", DataType = LeanExcelDataType.DateTime, Format = "yyyy-MM-dd HH:mm:ss")]
    public DateTime GenerateTime { get; set; }

    /// <summary>
    /// 生成路径
    /// </summary>
    [LeanExcelColumn("生成路径", DataType = LeanExcelDataType.String)]
    public string OutputPath { get; set; } = default!;

    /// <summary>
    /// 错误信息
    /// </summary>
    [LeanExcelColumn("错误信息", DataType = LeanExcelDataType.String)]
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    [LeanExcelColumn("创建时间", DataType = LeanExcelDataType.DateTime, Format = "yyyy-MM-dd HH:mm:ss")]
    public DateTime CreateTime { get; set; }
  }

  /// <summary>
  /// 代码生成历史导入DTO
  /// </summary>
  public class LeanGenHistoryImportDto
  {
    /// <summary>
    /// 任务Id
    /// </summary>
    [LeanExcelColumn("任务Id", DataType = LeanExcelDataType.Long)]
    public long TaskId { get; set; }

    /// <summary>
    /// 表Id
    /// </summary>
    [LeanExcelColumn("表Id", DataType = LeanExcelDataType.Long)]
    public long TableId { get; set; }

    /// <summary>
    /// 生成状态
    /// </summary>
    [LeanExcelColumn("生成状态", DataType = LeanExcelDataType.Int)]
    public int Status { get; set; }

    /// <summary>
    /// 生成时间
    /// </summary>
    [LeanExcelColumn("生成时间", DataType = LeanExcelDataType.DateTime, Format = "yyyy-MM-dd HH:mm:ss")]
    public DateTime GenerateTime { get; set; }

    /// <summary>
    /// 生成路径
    /// </summary>
    [LeanExcelColumn("生成路径", DataType = LeanExcelDataType.String)]
    public string OutputPath { get; set; } = default!;

    /// <summary>
    /// 错误信息
    /// </summary>
    [LeanExcelColumn("错误信息", DataType = LeanExcelDataType.String)]
    public string? ErrorMessage { get; set; }
  }

  /// <summary>
  /// 代码生成历史导入模板DTO
  /// </summary>
  public class LeanGenHistoryImportTemplateDto
  {
    /// <summary>
    /// 任务Id
    /// </summary>
    [LeanExcelColumn("任务Id", DataType = LeanExcelDataType.Long)]
    public long TaskId { get; set; } = 1;

    /// <summary>
    /// 表Id
    /// </summary>
    [LeanExcelColumn("表Id", DataType = LeanExcelDataType.Long)]
    public long TableId { get; set; } = 1;

    /// <summary>
    /// 生成状态
    /// </summary>
    [LeanExcelColumn("生成状态", DataType = LeanExcelDataType.Int)]
    public int Status { get; set; } = 0;

    /// <summary>
    /// 生成时间
    /// </summary>
    [LeanExcelColumn("生成时间", DataType = LeanExcelDataType.DateTime, Format = "yyyy-MM-dd HH:mm:ss")]
    public DateTime GenerateTime { get; set; } = DateTime.Now;

    /// <summary>
    /// 生成路径
    /// </summary>
    [LeanExcelColumn("生成路径", DataType = LeanExcelDataType.String)]
    public string OutputPath { get; set; } = "D:\\Code\\Generated";

    /// <summary>
    /// 错误信息
    /// </summary>
    [LeanExcelColumn("错误信息", DataType = LeanExcelDataType.String)]
    public string? ErrorMessage { get; set; }
  }
}