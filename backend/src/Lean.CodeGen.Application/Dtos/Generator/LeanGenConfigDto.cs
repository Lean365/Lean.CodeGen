//===================================================
// 项目名: Lean.CodeGen.Application
// 文件名: LeanGenConfigDto.cs
// 功能描述: 代码生成配置相关DTO
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
  /// 代码生成配置查询DTO
  /// </summary>
  public class LeanGenConfigDto
  {
    /// <summary>
    /// 主键
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 配置名称
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// 生成代码方式（0zip压缩包 1自定义路径）
    /// </summary>
    public int GenType { get; set; }

    /// <summary>
    /// 生成路径（不填默认项目路径）
    /// </summary>
    public string? GenPath { get; set; }

    /// <summary>
    /// 后端工具
    /// </summary>
    public string BackendTool { get; set; } = default!;

    /// <summary>
    /// 前端工具
    /// </summary>
    public string FrontendTool { get; set; } = default!;

    /// <summary>
    /// 是否覆盖已有文件（0：否 1：是）
    /// </summary>
    [LeanExcelColumn("是否覆盖已有文件", DataType = LeanExcelDataType.Int)]
    public int IsOverride { get; set; }

    /// <summary>
    /// 是否移除表前缀（0：否 1：是）
    /// </summary>
    [LeanExcelColumn("是否移除表前缀", DataType = LeanExcelDataType.Int)]
    public int IsRemovePrefix { get; set; }

    /// <summary>
    /// 表前缀
    /// </summary>
    public string? TablePrefix { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 模板列表
    /// </summary>
    public List<LeanGenTemplateDto> Templates { get; set; } = new();
  }

  /// <summary>
  /// 代码生成模板查询DTO
  /// </summary>
  public class LeanGenTemplateDto
  {
    /// <summary>
    /// 主键
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 模板名称
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// 模板组
    /// </summary>
    public string GroupName { get; set; } = default!;

    /// <summary>
    /// 模板文件名
    /// </summary>
    public string FileName { get; set; } = default!;

    /// <summary>
    /// 模板内容
    /// </summary>
    public string Content { get; set; } = default!;

    /// <summary>
    /// 模板引擎
    /// </summary>
    public string Engine { get; set; } = default!;

    /// <summary>
    /// 目标文件路径
    /// </summary>
    public string TargetPath { get; set; } = default!;

    /// <summary>
    /// 语言类型
    /// </summary>
    public string Language { get; set; } = default!;

    /// <summary>
    /// 是否启用（0：禁用 1：启用）
    /// </summary>
    public int IsEnabled { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 配置Id
    /// </summary>
    public long ConfigId { get; set; }
  }

  /// <summary>
  /// 代码生成配置创建DTO
  /// </summary>
  public class LeanCreateGenConfigDto
  {
    /// <summary>
    /// 配置名称
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// 生成代码方式（0zip压缩包 1自定义路径）
    /// </summary>
    public int GenType { get; set; }

    /// <summary>
    /// 生成路径（不填默认项目路径）
    /// </summary>
    public string? GenPath { get; set; }

    /// <summary>
    /// 后端工具
    /// </summary>
    public string BackendTool { get; set; } = default!;

    /// <summary>
    /// 前端工具
    /// </summary>
    public string FrontendTool { get; set; } = default!;

    /// <summary>
    /// 是否覆盖已有文件（0：否 1：是）
    /// </summary>
    [LeanExcelColumn("是否覆盖已有文件", DataType = LeanExcelDataType.Int)]
    public int IsOverride { get; set; }

    /// <summary>
    /// 是否移除表前缀（0：否 1：是）
    /// </summary>
    [LeanExcelColumn("是否移除表前缀", DataType = LeanExcelDataType.Int)]
    public int IsRemovePrefix { get; set; }

    /// <summary>
    /// 表前缀
    /// </summary>
    public string? TablePrefix { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 模板列表
    /// </summary>
    public List<LeanCreateGenTemplateDto> Templates { get; set; } = new();
  }

  /// <summary>
  /// 代码生成模板创建DTO
  /// </summary>
  public class LeanCreateGenTemplateDto
  {
    /// <summary>
    /// 模板名称
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// 模板组
    /// </summary>
    public string GroupName { get; set; } = default!;

    /// <summary>
    /// 模板文件名
    /// </summary>
    public string FileName { get; set; } = default!;

    /// <summary>
    /// 模板内容
    /// </summary>
    public string Content { get; set; } = default!;

    /// <summary>
    /// 模板引擎
    /// </summary>
    public string Engine { get; set; } = default!;

    /// <summary>
    /// 目标文件路径
    /// </summary>
    public string TargetPath { get; set; } = default!;

    /// <summary>
    /// 语言类型
    /// </summary>
    public string Language { get; set; } = default!;

    /// <summary>
    /// 是否启用（0：禁用 1：启用）
    /// </summary>
    public int IsEnabled { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }
  }

  /// <summary>
  /// 代码生成配置更新DTO
  /// </summary>
  public class LeanUpdateGenConfigDto : LeanCreateGenConfigDto
  {
    /// <summary>
    /// 主键
    /// </summary>
    public long Id { get; set; }
  }

  /// <summary>
  /// 代码生成配置查询条件DTO
  /// </summary>
  public class LeanGenConfigQueryDto : LeanPage
  {
    /// <summary>
    /// 配置名称
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// 生成代码方式
    /// </summary>
    public int? GenType { get; set; }

    /// <summary>
    /// 后端工具
    /// </summary>
    public string? BackendTool { get; set; }

    /// <summary>
    /// 前端工具
    /// </summary>
    public string? FrontendTool { get; set; }

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
  /// 代码生成配置导出DTO
  /// </summary>
  public class LeanGenConfigExportDto
  {
    /// <summary>
    /// 配置名称
    /// </summary>
    [LeanExcelColumn("配置名称", DataType = LeanExcelDataType.String)]
    public string Name { get; set; } = default!;

    /// <summary>
    /// 生成代码方式
    /// </summary>
    [LeanExcelColumn("生成代码方式", DataType = LeanExcelDataType.Int)]
    public int GenType { get; set; }

    /// <summary>
    /// 生成路径
    /// </summary>
    [LeanExcelColumn("生成路径", DataType = LeanExcelDataType.String)]
    public string? GenPath { get; set; }

    /// <summary>
    /// 后端工具
    /// </summary>
    [LeanExcelColumn("后端工具", DataType = LeanExcelDataType.String)]
    public string BackendTool { get; set; } = default!;

    /// <summary>
    /// 前端工具
    /// </summary>
    [LeanExcelColumn("前端工具", DataType = LeanExcelDataType.String)]
    public string FrontendTool { get; set; } = default!;

    /// <summary>
    /// 是否覆盖已有文件（0：否 1：是）
    /// </summary>
    [LeanExcelColumn("是否覆盖已有文件", DataType = LeanExcelDataType.Int)]
    public int IsOverride { get; set; }

    /// <summary>
    /// 是否移除表前缀（0：否 1：是）
    /// </summary>
    [LeanExcelColumn("是否移除表前缀", DataType = LeanExcelDataType.Int)]
    public int IsRemovePrefix { get; set; }

    /// <summary>
    /// 表前缀
    /// </summary>
    [LeanExcelColumn("表前缀", DataType = LeanExcelDataType.String)]
    public string? TablePrefix { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [LeanExcelColumn("备注", DataType = LeanExcelDataType.String)]
    public string? Remark { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    [LeanExcelColumn("创建时间", DataType = LeanExcelDataType.DateTime, Format = "yyyy-MM-dd HH:mm:ss")]
    public DateTime CreateTime { get; set; }
  }

  /// <summary>
  /// 代码生成配置导入DTO
  /// </summary>
  public class LeanGenConfigImportDto
  {
    /// <summary>
    /// 配置名称
    /// </summary>
    [LeanExcelColumn("配置名称", DataType = LeanExcelDataType.String)]
    public string Name { get; set; } = default!;

    /// <summary>
    /// 生成代码方式
    /// </summary>
    [LeanExcelColumn("生成代码方式", DataType = LeanExcelDataType.Int)]
    public int GenType { get; set; }

    /// <summary>
    /// 生成路径
    /// </summary>
    [LeanExcelColumn("生成路径", DataType = LeanExcelDataType.String)]
    public string? GenPath { get; set; }

    /// <summary>
    /// 后端工具
    /// </summary>
    [LeanExcelColumn("后端工具", DataType = LeanExcelDataType.String)]
    public string BackendTool { get; set; } = default!;

    /// <summary>
    /// 前端工具
    /// </summary>
    [LeanExcelColumn("前端工具", DataType = LeanExcelDataType.String)]
    public string FrontendTool { get; set; } = default!;

    /// <summary>
    /// 是否覆盖已有文件（0：否 1：是）
    /// </summary>
    [LeanExcelColumn("是否覆盖已有文件", DataType = LeanExcelDataType.Int)]
    public int IsOverride { get; set; }

    /// <summary>
    /// 是否移除表前缀（0：否 1：是）
    /// </summary>
    [LeanExcelColumn("是否移除表前缀", DataType = LeanExcelDataType.Int)]
    public int IsRemovePrefix { get; set; }

    /// <summary>
    /// 表前缀
    /// </summary>
    [LeanExcelColumn("表前缀", DataType = LeanExcelDataType.String)]
    public string? TablePrefix { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [LeanExcelColumn("备注", DataType = LeanExcelDataType.String)]
    public string? Remark { get; set; }
  }

  /// <summary>
  /// 代码生成配置导入模板DTO
  /// </summary>
  public class LeanGenConfigImportTemplateDto
  {
    /// <summary>
    /// 配置名称
    /// </summary>
    [LeanExcelColumn("配置名称", DataType = LeanExcelDataType.String)]
    public string Name { get; set; } = "示例配置";

    /// <summary>
    /// 生成代码方式
    /// </summary>
    [LeanExcelColumn("生成代码方式", DataType = LeanExcelDataType.Int)]
    public int GenType { get; set; } = 0;

    /// <summary>
    /// 生成路径
    /// </summary>
    [LeanExcelColumn("生成路径", DataType = LeanExcelDataType.String)]
    public string? GenPath { get; set; } = "D:\\Code";

    /// <summary>
    /// 后端工具
    /// </summary>
    [LeanExcelColumn("后端工具", DataType = LeanExcelDataType.String)]
    public string BackendTool { get; set; } = "SqlSugar";

    /// <summary>
    /// 前端工具
    /// </summary>
    [LeanExcelColumn("前端工具", DataType = LeanExcelDataType.String)]
    public string FrontendTool { get; set; } = "Vue3";

    /// <summary>
    /// 是否覆盖已有文件（0：否 1：是）
    /// </summary>
    [LeanExcelColumn("是否覆盖已有文件", DataType = LeanExcelDataType.Int)]
    public int IsOverride { get; set; } = 0;

    /// <summary>
    /// 是否移除表前缀（0：否 1：是）
    /// </summary>
    [LeanExcelColumn("是否移除表前缀", DataType = LeanExcelDataType.Int)]
    public int IsRemovePrefix { get; set; } = 1;

    /// <summary>
    /// 表前缀
    /// </summary>
    [LeanExcelColumn("表前缀", DataType = LeanExcelDataType.String)]
    public string? TablePrefix { get; set; } = "lean_";

    /// <summary>
    /// 备注
    /// </summary>
    [LeanExcelColumn("备注", DataType = LeanExcelDataType.String)]
    public string? Remark { get; set; } = "示例配置";
  }
}