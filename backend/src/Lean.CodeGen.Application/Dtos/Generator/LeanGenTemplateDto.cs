//===================================================
// 项目名: Lean.CodeGen.Application
// 文件名: LeanGenTemplateDto.cs
// 功能描述: 代码生成模板相关DTO
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
  /// 代码生成模板查询DTO
  /// </summary>
  public class LeanGenTemplateQueryDto : LeanPage
  {
    /// <summary>
    /// 模板名称
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// 模板组
    /// </summary>
    public string? GroupName { get; set; }

    /// <summary>
    /// 模板引擎
    /// </summary>
    public string? Engine { get; set; }

    /// <summary>
    /// 语言类型
    /// </summary>
    public string? Language { get; set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    public bool? IsEnabled { get; set; }

    /// <summary>
    /// 配置Id
    /// </summary>
    public long? ConfigId { get; set; }

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
  /// 代码生成模板导出DTO
  /// </summary>
  public class LeanGenTemplateExportDto
  {
    /// <summary>
    /// 模板名称
    /// </summary>
    [LeanExcelColumn("模板名称", DataType = LeanExcelDataType.String)]
    public string Name { get; set; } = default!;

    /// <summary>
    /// 模板组
    /// </summary>
    [LeanExcelColumn("模板组", DataType = LeanExcelDataType.String)]
    public string GroupName { get; set; } = default!;

    /// <summary>
    /// 模板文件名
    /// </summary>
    [LeanExcelColumn("模板文件名", DataType = LeanExcelDataType.String)]
    public string FileName { get; set; } = default!;

    /// <summary>
    /// 模板内容
    /// </summary>
    [LeanExcelColumn("模板内容", DataType = LeanExcelDataType.String)]
    public string Content { get; set; } = default!;

    /// <summary>
    /// 模板引擎
    /// </summary>
    [LeanExcelColumn("模板引擎", DataType = LeanExcelDataType.String)]
    public string Engine { get; set; } = default!;

    /// <summary>
    /// 目标文件路径
    /// </summary>
    [LeanExcelColumn("目标文件路径", DataType = LeanExcelDataType.String)]
    public string TargetPath { get; set; } = default!;

    /// <summary>
    /// 语言类型
    /// </summary>
    [LeanExcelColumn("语言类型", DataType = LeanExcelDataType.String)]
    public string Language { get; set; } = default!;

    /// <summary>
    /// 是否启用
    /// </summary>
    [LeanExcelColumn("是否启用", DataType = LeanExcelDataType.Boolean)]
    public bool IsEnabled { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [LeanExcelColumn("备注", DataType = LeanExcelDataType.String)]
    public string? Remark { get; set; }

    /// <summary>
    /// 配置Id
    /// </summary>
    [LeanExcelColumn("配置Id", DataType = LeanExcelDataType.String)]
    public long ConfigId { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    [LeanExcelColumn("创建时间", DataType = LeanExcelDataType.DateTime, Format = "yyyy-MM-dd HH:mm:ss")]
    public DateTime CreateTime { get; set; }
  }

  /// <summary>
  /// 代码生成模板导入DTO
  /// </summary>
  public class LeanGenTemplateImportDto
  {
    /// <summary>
    /// 模板名称
    /// </summary>
    [LeanExcelColumn("模板名称", DataType = LeanExcelDataType.String)]
    public string Name { get; set; } = default!;

    /// <summary>
    /// 模板组
    /// </summary>
    [LeanExcelColumn("模板组", DataType = LeanExcelDataType.String)]
    public string GroupName { get; set; } = default!;

    /// <summary>
    /// 模板文件名
    /// </summary>
    [LeanExcelColumn("模板文件名", DataType = LeanExcelDataType.String)]
    public string FileName { get; set; } = default!;

    /// <summary>
    /// 模板内容
    /// </summary>
    [LeanExcelColumn("模板内容", DataType = LeanExcelDataType.String)]
    public string Content { get; set; } = default!;

    /// <summary>
    /// 模板引擎
    /// </summary>
    [LeanExcelColumn("模板引擎", DataType = LeanExcelDataType.String)]
    public string Engine { get; set; } = default!;

    /// <summary>
    /// 目标文件路径
    /// </summary>
    [LeanExcelColumn("目标文件路径", DataType = LeanExcelDataType.String)]
    public string TargetPath { get; set; } = default!;

    /// <summary>
    /// 语言类型
    /// </summary>
    [LeanExcelColumn("语言类型", DataType = LeanExcelDataType.String)]
    public string Language { get; set; } = default!;

    /// <summary>
    /// 是否启用
    /// </summary>
    [LeanExcelColumn("是否启用", DataType = LeanExcelDataType.Boolean)]
    public bool IsEnabled { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [LeanExcelColumn("备注", DataType = LeanExcelDataType.String)]
    public string? Remark { get; set; }

    /// <summary>
    /// 配置Id
    /// </summary>
    [LeanExcelColumn("配置Id", DataType = LeanExcelDataType.String)]
    public long ConfigId { get; set; }
  }

  /// <summary>
  /// 代码生成模板导入模板DTO
  /// </summary>
  public class LeanGenTemplateImportTemplateDto
  {
    /// <summary>
    /// 模板名称
    /// </summary>
    [LeanExcelColumn("模板名称", DataType = LeanExcelDataType.String)]
    public string Name { get; set; } = "实体类模板";

    /// <summary>
    /// 模板组
    /// </summary>
    [LeanExcelColumn("模板组", DataType = LeanExcelDataType.String)]
    public string GroupName { get; set; } = "后端";

    /// <summary>
    /// 模板文件名
    /// </summary>
    [LeanExcelColumn("模板文件名", DataType = LeanExcelDataType.String)]
    public string FileName { get; set; } = "Entity.cs.cshtml";

    /// <summary>
    /// 模板内容
    /// </summary>
    [LeanExcelColumn("模板内容", DataType = LeanExcelDataType.String)]
    public string Content { get; set; } = "@model Lean.CodeGen.Domain.Models.LeanGenTemplateModel\n@{\n  // 模板示例\n}";

    /// <summary>
    /// 模板引擎
    /// </summary>
    [LeanExcelColumn("模板引擎", DataType = LeanExcelDataType.String)]
    public string Engine { get; set; } = "Razor";

    /// <summary>
    /// 目标文件路径
    /// </summary>
    [LeanExcelColumn("目标文件路径", DataType = LeanExcelDataType.String)]
    public string TargetPath { get; set; } = "src/Domain/Entities";

    /// <summary>
    /// 语言类型
    /// </summary>
    [LeanExcelColumn("语言类型", DataType = LeanExcelDataType.String)]
    public string Language { get; set; } = "CSharp";

    /// <summary>
    /// 是否启用
    /// </summary>
    [LeanExcelColumn("是否启用", DataType = LeanExcelDataType.Boolean)]
    public bool IsEnabled { get; set; } = true;

    /// <summary>
    /// 备注
    /// </summary>
    [LeanExcelColumn("备注", DataType = LeanExcelDataType.String)]
    public string? Remark { get; set; } = "实体类代码生成模板";

    /// <summary>
    /// 配置Id
    /// </summary>
    [LeanExcelColumn("配置Id", DataType = LeanExcelDataType.String)]
    public long ConfigId { get; set; } = 0;
  }

  /// <summary>
  /// 代码生成模板更新DTO
  /// </summary>
  public class LeanUpdateGenTemplateDto : LeanCreateGenTemplateDto
  {
    /// <summary>
    /// 主键
    /// </summary>
    public long Id { get; set; }
  }
}