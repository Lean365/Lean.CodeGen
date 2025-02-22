//===================================================
// 项目名: Lean.CodeGen.Application
// 文件名: LeanTableConfigDto.cs
// 功能描述: 表和配置关联相关DTO
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
  /// 表和配置关联查询DTO
  /// </summary>
  public class LeanTableConfigDto
  {
    /// <summary>
    /// 主键
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 表Id
    /// </summary>
    public long TableId { get; set; }

    /// <summary>
    /// 表名称
    /// </summary>
    public string TableName { get; set; } = default!;

    /// <summary>
    /// 配置Id
    /// </summary>
    public long ConfigId { get; set; }

    /// <summary>
    /// 配置名称
    /// </summary>
    public string ConfigName { get; set; } = default!;

    /// <summary>
    /// 实体名称
    /// </summary>
    public string EntityName { get; set; } = default!;

    /// <summary>
    /// 业务名称
    /// </summary>
    public string BusinessName { get; set; } = default!;

    /// <summary>
    /// 功能名称
    /// </summary>
    public string FunctionName { get; set; } = default!;

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }
  }

  /// <summary>
  /// 表和配置关联创建DTO
  /// </summary>
  public class LeanCreateTableConfigDto
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
    /// 实体名称
    /// </summary>
    public string EntityName { get; set; } = default!;

    /// <summary>
    /// 业务名称
    /// </summary>
    public string BusinessName { get; set; } = default!;

    /// <summary>
    /// 功能名称
    /// </summary>
    public string FunctionName { get; set; } = default!;
  }

  /// <summary>
  /// 表和配置关联更新DTO
  /// </summary>
  public class LeanUpdateTableConfigDto : LeanCreateTableConfigDto
  {
    /// <summary>
    /// 主键
    /// </summary>
    public long Id { get; set; }
  }

  /// <summary>
  /// 表和配置关联查询条件DTO
  /// </summary>
  public class LeanTableConfigQueryDto : LeanPage
  {
    /// <summary>
    /// 表Id
    /// </summary>
    public long? TableId { get; set; }

    /// <summary>
    /// 配置Id
    /// </summary>
    public long? ConfigId { get; set; }

    /// <summary>
    /// 实体名称
    /// </summary>
    public string? EntityName { get; set; }

    /// <summary>
    /// 业务名称
    /// </summary>
    public string? BusinessName { get; set; }

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
  /// 表和配置关联导出DTO
  /// </summary>
  public class LeanTableConfigExportDto
  {
    /// <summary>
    /// 表名称
    /// </summary>
    [LeanExcelColumn("表名称", DataType = LeanExcelDataType.String)]
    public string TableName { get; set; } = default!;

    /// <summary>
    /// 配置名称
    /// </summary>
    [LeanExcelColumn("配置名称", DataType = LeanExcelDataType.String)]
    public string ConfigName { get; set; } = default!;

    /// <summary>
    /// 实体名称
    /// </summary>
    [LeanExcelColumn("实体名称", DataType = LeanExcelDataType.String)]
    public string EntityName { get; set; } = default!;

    /// <summary>
    /// 业务名称
    /// </summary>
    [LeanExcelColumn("业务名称", DataType = LeanExcelDataType.String)]
    public string BusinessName { get; set; } = default!;

    /// <summary>
    /// 功能名称
    /// </summary>
    [LeanExcelColumn("功能名称", DataType = LeanExcelDataType.String)]
    public string FunctionName { get; set; } = default!;

    /// <summary>
    /// 创建时间
    /// </summary>
    [LeanExcelColumn("创建时间", DataType = LeanExcelDataType.DateTime, Format = "yyyy-MM-dd HH:mm:ss")]
    public DateTime CreateTime { get; set; }
  }

  /// <summary>
  /// 表和配置关联导入DTO
  /// </summary>
  public class LeanTableConfigImportDto
  {
    /// <summary>
    /// 表Id
    /// </summary>
    [LeanExcelColumn("表Id", DataType = LeanExcelDataType.Long)]
    public long TableId { get; set; }

    /// <summary>
    /// 配置Id
    /// </summary>
    [LeanExcelColumn("配置Id", DataType = LeanExcelDataType.Long)]
    public long ConfigId { get; set; }

    /// <summary>
    /// 实体名称
    /// </summary>
    [LeanExcelColumn("实体名称", DataType = LeanExcelDataType.String)]
    public string EntityName { get; set; } = default!;

    /// <summary>
    /// 业务名称
    /// </summary>
    [LeanExcelColumn("业务名称", DataType = LeanExcelDataType.String)]
    public string BusinessName { get; set; } = default!;

    /// <summary>
    /// 功能名称
    /// </summary>
    [LeanExcelColumn("功能名称", DataType = LeanExcelDataType.String)]
    public string FunctionName { get; set; } = default!;
  }

  /// <summary>
  /// 表和配置关联导入模板DTO
  /// </summary>
  public class LeanTableConfigImportTemplateDto
  {
    /// <summary>
    /// 表Id
    /// </summary>
    [LeanExcelColumn("表Id", DataType = LeanExcelDataType.Long)]
    public long TableId { get; set; } = 1;

    /// <summary>
    /// 配置Id
    /// </summary>
    [LeanExcelColumn("配置Id", DataType = LeanExcelDataType.Long)]
    public long ConfigId { get; set; } = 1;

    /// <summary>
    /// 实体名称
    /// </summary>
    [LeanExcelColumn("实体名称", DataType = LeanExcelDataType.String)]
    public string EntityName { get; set; } = "LeanExample";

    /// <summary>
    /// 业务名称
    /// </summary>
    [LeanExcelColumn("业务名称", DataType = LeanExcelDataType.String)]
    public string BusinessName { get; set; } = "example";

    /// <summary>
    /// 功能名称
    /// </summary>
    [LeanExcelColumn("功能名称", DataType = LeanExcelDataType.String)]
    public string FunctionName { get; set; } = "示例";
  }
}