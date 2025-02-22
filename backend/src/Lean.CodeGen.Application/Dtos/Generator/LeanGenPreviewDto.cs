//===================================================
// 项目名: Lean.CodeGen.Application
// 文件名: LeanGenPreviewDto.cs
// 功能描述: 代码生成预览相关DTO
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
  /// 代码生成预览请求DTO
  /// </summary>
  public class LeanGenPreviewRequestDto
  {
    /// <summary>
    /// 表Id
    /// </summary>
    public long TableId { get; set; }

    /// <summary>
    /// 配置Id
    /// </summary>
    public long ConfigId { get; set; }
  }

  /// <summary>
  /// 代码生成预览结果DTO
  /// </summary>
  public class LeanGenPreviewResultDto
  {
    /// <summary>
    /// 预览文件列表
    /// </summary>
    public List<LeanGenPreviewFileDto> Files { get; set; } = new();
  }

  /// <summary>
  /// 代码生成预览文件DTO
  /// </summary>
  public class LeanGenPreviewFileDto
  {
    /// <summary>
    /// 文件名
    /// </summary>
    public string FileName { get; set; } = default!;

    /// <summary>
    /// 文件路径
    /// </summary>
    public string FilePath { get; set; } = default!;

    /// <summary>
    /// 文件内容
    /// </summary>
    public string Content { get; set; } = default!;

    /// <summary>
    /// 语言类型
    /// </summary>
    public string Language { get; set; } = default!;

    /// <summary>
    /// 模板Id
    /// </summary>
    public long TemplateId { get; set; }

    /// <summary>
    /// 模板名称
    /// </summary>
    public string TemplateName { get; set; } = default!;
  }

  /// <summary>
  /// 代码生成下载请求DTO
  /// </summary>
  public class LeanGenDownloadRequestDto
  {
    /// <summary>
    /// 表Id
    /// </summary>
    public long TableId { get; set; }

    /// <summary>
    /// 配置Id
    /// </summary>
    public long ConfigId { get; set; }

    /// <summary>
    /// 是否覆盖已有文件
    /// </summary>
    public bool IsOverride { get; set; }
  }

  /// <summary>
  /// 代码生成下载结果DTO
  /// </summary>
  public class LeanGenDownloadResultDto
  {
    /// <summary>
    /// 文件路径
    /// </summary>
    public string FilePath { get; set; } = default!;

    /// <summary>
    /// 文件名
    /// </summary>
    public string FileName { get; set; } = default!;

    /// <summary>
    /// 文件大小（字节）
    /// </summary>
    public long FileSize { get; set; }
  }

  /// <summary>
  /// 代码生成预览导出DTO
  /// </summary>
  public class LeanGenPreviewExportDto
  {
    /// <summary>
    /// 文件名
    /// </summary>
    [LeanExcelColumn("文件名", DataType = LeanExcelDataType.String)]
    public string FileName { get; set; } = default!;

    /// <summary>
    /// 文件路径
    /// </summary>
    [LeanExcelColumn("文件路径", DataType = LeanExcelDataType.String)]
    public string FilePath { get; set; } = default!;

    /// <summary>
    /// 语言类型
    /// </summary>
    [LeanExcelColumn("语言类型", DataType = LeanExcelDataType.String)]
    public string Language { get; set; } = default!;

    /// <summary>
    /// 模板名称
    /// </summary>
    [LeanExcelColumn("模板名称", DataType = LeanExcelDataType.String)]
    public string TemplateName { get; set; } = default!;

    /// <summary>
    /// 创建时间
    /// </summary>
    [LeanExcelColumn("创建时间", DataType = LeanExcelDataType.DateTime, Format = "yyyy-MM-dd HH:mm:ss")]
    public DateTime CreateTime { get; set; }
  }

  /// <summary>
  /// 代码生成预览导入DTO
  /// </summary>
  public class LeanGenPreviewImportDto
  {
    /// <summary>
    /// 文件名
    /// </summary>
    [LeanExcelColumn("文件名", DataType = LeanExcelDataType.String)]
    public string FileName { get; set; } = default!;

    /// <summary>
    /// 文件路径
    /// </summary>
    [LeanExcelColumn("文件路径", DataType = LeanExcelDataType.String)]
    public string FilePath { get; set; } = default!;

    /// <summary>
    /// 语言类型
    /// </summary>
    [LeanExcelColumn("语言类型", DataType = LeanExcelDataType.String)]
    public string Language { get; set; } = default!;

    /// <summary>
    /// 模板Id
    /// </summary>
    [LeanExcelColumn("模板Id", DataType = LeanExcelDataType.Long)]
    public long TemplateId { get; set; }
  }

  /// <summary>
  /// 代码生成预览导入模板DTO
  /// </summary>
  public class LeanGenPreviewImportTemplateDto
  {
    /// <summary>
    /// 文件名
    /// </summary>
    [LeanExcelColumn("文件名", DataType = LeanExcelDataType.String)]
    public string FileName { get; set; } = "Example.cs";

    /// <summary>
    /// 文件路径
    /// </summary>
    [LeanExcelColumn("文件路径", DataType = LeanExcelDataType.String)]
    public string FilePath { get; set; } = "src/Example/Example.cs";

    /// <summary>
    /// 语言类型
    /// </summary>
    [LeanExcelColumn("语言类型", DataType = LeanExcelDataType.String)]
    public string Language { get; set; } = "CSharp";

    /// <summary>
    /// 模板Id
    /// </summary>
    [LeanExcelColumn("模板Id", DataType = LeanExcelDataType.Long)]
    public long TemplateId { get; set; } = 1;
  }
}