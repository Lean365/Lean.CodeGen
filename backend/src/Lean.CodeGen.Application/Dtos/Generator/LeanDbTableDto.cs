//===================================================
// 项目名: Lean.CodeGen.Application
// 文件名: LeanDbTableDto.cs
// 功能描述: 数据库表相关DTO
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
  /// 数据库表查询DTO
  /// </summary>
  public class LeanDbTableDto
  {
    /// <summary>
    /// 主键
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 表名称
    /// </summary>
    public string TableName { get; set; } = default!;

    /// <summary>
    /// 表描述
    /// </summary>
    public string? TableComment { get; set; }

    /// <summary>
    /// 实体类名称
    /// </summary>
    public string ClassName { get; set; } = default!;

    /// <summary>
    /// 使用的模板（crud单表操作、tree树表操作、sub主子表操作）
    /// </summary>
    public string TemplateCategory { get; set; } = default!;

    /// <summary>
    /// 生成包路径
    /// </summary>
    public string PackageName { get; set; } = default!;

    /// <summary>
    /// 生成模块名
    /// </summary>
    public string ModuleName { get; set; } = default!;

    /// <summary>
    /// 生成业务名
    /// </summary>
    public string BusinessName { get; set; } = default!;

    /// <summary>
    /// 生成功能名
    /// </summary>
    public string FunctionName { get; set; } = default!;

    /// <summary>
    /// 生成功能作者
    /// </summary>
    public string FunctionAuthor { get; set; } = default!;

    /// <summary>
    /// 父表名称
    /// </summary>
    public string? ParentTableName { get; set; }

    /// <summary>
    /// 父表主键
    /// </summary>
    public string? ParentTableFk { get; set; }

    /// <summary>
    /// 主键信息
    /// </summary>
    public string PkColumn { get; set; } = default!;

    /// <summary>
    /// 其它生成选项
    /// </summary>
    public string? Options { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 表来源（0手动创建 1数据库导入）
    /// </summary>
    public int SourceType { get; set; }

    /// <summary>
    /// 数据源Id
    /// </summary>
    public long DataSourceId { get; set; }

    /// <summary>
    /// 数据源名称
    /// </summary>
    public string DataSourceName { get; set; } = default!;

    /// <summary>
    /// 列信息
    /// </summary>
    public List<LeanDbColumnDto> Columns { get; set; } = new();
  }

  /// <summary>
  /// 数据库列查询DTO
  /// </summary>
  public class LeanDbColumnDto
  {
    /// <summary>
    /// 主键
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 列名称
    /// </summary>
    public string ColumnName { get; set; } = default!;

    /// <summary>
    /// 列描述
    /// </summary>
    public string? ColumnComment { get; set; }

    /// <summary>
    /// 列类型
    /// </summary>
    public string ColumnType { get; set; } = default!;

    /// <summary>
    /// .NET类型
    /// </summary>
    public string NetType { get; set; } = default!;

    /// <summary>
    /// Java类型
    /// </summary>
    public string JavaType { get; set; } = default!;

    /// <summary>
    /// TypeScript类型
    /// </summary>
    public string TsType { get; set; } = default!;

    /// <summary>
    /// 是否主键
    /// </summary>
    public bool IsPk { get; set; }

    /// <summary>
    /// 是否自增
    /// </summary>
    public bool IsIncrement { get; set; }

    /// <summary>
    /// 是否必填
    /// </summary>
    public bool IsRequired { get; set; }

    /// <summary>
    /// 是否为空
    /// </summary>
    public bool IsNullable { get; set; }

    /// <summary>
    /// 列长度
    /// </summary>
    public int? Length { get; set; }

    /// <summary>
    /// 精度
    /// </summary>
    public int? Precision { get; set; }

    /// <summary>
    /// 小数位数
    /// </summary>
    public int? Scale { get; set; }

    /// <summary>
    /// 默认值
    /// </summary>
    public string? DefaultValue { get; set; }

    /// <summary>
    /// 排序号
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// 所属表Id
    /// </summary>
    public long TableId { get; set; }

    /// <summary>
    /// 是否为插入字段
    /// </summary>
    public bool IsInsert { get; set; }

    /// <summary>
    /// 是否编辑字段
    /// </summary>
    public bool IsEdit { get; set; }

    /// <summary>
    /// 显示类型
    /// </summary>
    public string? HtmlType { get; set; }

    /// <summary>
    /// 字典类型
    /// </summary>
    public string? DictType { get; set; }

    /// <summary>
    /// 验证规则
    /// </summary>
    public string? ValidateRule { get; set; }

    /// <summary>
    /// 是否列表显示
    /// </summary>
    public bool IsList { get; set; }

    /// <summary>
    /// 是否查询字段
    /// </summary>
    public bool IsQuery { get; set; }

    /// <summary>
    /// 查询方式
    /// </summary>
    public string? QueryType { get; set; }

    /// <summary>
    /// 是否导入字段
    /// </summary>
    public bool IsImport { get; set; }

    /// <summary>
    /// 是否导出字段
    /// </summary>
    public bool IsExport { get; set; }

    /// <summary>
    /// 是否业务键
    /// </summary>
    public bool IsBusinessKey { get; set; }

    /// <summary>
    /// 是否版本字段
    /// </summary>
    public bool IsVersion { get; set; }
  }

  /// <summary>
  /// 数据库表创建DTO
  /// </summary>
  public class LeanCreateDbTableDto
  {
    /// <summary>
    /// 表名称
    /// </summary>
    public string TableName { get; set; } = default!;

    /// <summary>
    /// 表描述
    /// </summary>
    public string? TableComment { get; set; }

    /// <summary>
    /// 实体类名称
    /// </summary>
    public string ClassName { get; set; } = default!;

    /// <summary>
    /// 使用的模板
    /// </summary>
    public string TemplateCategory { get; set; } = default!;

    /// <summary>
    /// 生成包路径
    /// </summary>
    public string PackageName { get; set; } = default!;

    /// <summary>
    /// 生成模块名
    /// </summary>
    public string ModuleName { get; set; } = default!;

    /// <summary>
    /// 生成业务名
    /// </summary>
    public string BusinessName { get; set; } = default!;

    /// <summary>
    /// 生成功能名
    /// </summary>
    public string FunctionName { get; set; } = default!;

    /// <summary>
    /// 生成功能作者
    /// </summary>
    public string FunctionAuthor { get; set; } = default!;

    /// <summary>
    /// 父表名称
    /// </summary>
    public string? ParentTableName { get; set; }

    /// <summary>
    /// 父表主键
    /// </summary>
    public string? ParentTableFk { get; set; }

    /// <summary>
    /// 主键信息
    /// </summary>
    public string PkColumn { get; set; } = default!;

    /// <summary>
    /// 其它生成选项
    /// </summary>
    public string? Options { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 数据源Id
    /// </summary>
    public long DataSourceId { get; set; }

    /// <summary>
    /// 列信息
    /// </summary>
    public List<LeanCreateDbColumnDto> Columns { get; set; } = new();
  }

  /// <summary>
  /// 数据库列创建DTO
  /// </summary>
  public class LeanCreateDbColumnDto
  {
    /// <summary>
    /// 列名称
    /// </summary>
    public string ColumnName { get; set; } = default!;

    /// <summary>
    /// 列描述
    /// </summary>
    public string? ColumnComment { get; set; }

    /// <summary>
    /// 列类型
    /// </summary>
    public string ColumnType { get; set; } = default!;

    /// <summary>
    /// .NET类型
    /// </summary>
    public string NetType { get; set; } = default!;

    /// <summary>
    /// Java类型
    /// </summary>
    public string JavaType { get; set; } = default!;

    /// <summary>
    /// TypeScript类型
    /// </summary>
    public string TsType { get; set; } = default!;

    /// <summary>
    /// 是否主键
    /// </summary>
    public bool IsPk { get; set; }

    /// <summary>
    /// 是否自增
    /// </summary>
    public bool IsIncrement { get; set; }

    /// <summary>
    /// 是否必填
    /// </summary>
    public bool IsRequired { get; set; }

    /// <summary>
    /// 是否为空
    /// </summary>
    public bool IsNullable { get; set; }

    /// <summary>
    /// 列长度
    /// </summary>
    public int? Length { get; set; }

    /// <summary>
    /// 精度
    /// </summary>
    public int? Precision { get; set; }

    /// <summary>
    /// 小数位数
    /// </summary>
    public int? Scale { get; set; }

    /// <summary>
    /// 默认值
    /// </summary>
    public string? DefaultValue { get; set; }

    /// <summary>
    /// 排序号
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// 是否为插入字段
    /// </summary>
    public bool IsInsert { get; set; }

    /// <summary>
    /// 是否编辑字段
    /// </summary>
    public bool IsEdit { get; set; }

    /// <summary>
    /// 显示类型
    /// </summary>
    public string? HtmlType { get; set; }

    /// <summary>
    /// 字典类型
    /// </summary>
    public string? DictType { get; set; }

    /// <summary>
    /// 验证规则
    /// </summary>
    public string? ValidateRule { get; set; }

    /// <summary>
    /// 是否列表显示
    /// </summary>
    public bool IsList { get; set; }

    /// <summary>
    /// 是否查询字段
    /// </summary>
    public bool IsQuery { get; set; }

    /// <summary>
    /// 查询方式
    /// </summary>
    public string? QueryType { get; set; }

    /// <summary>
    /// 是否导入字段
    /// </summary>
    public bool IsImport { get; set; }

    /// <summary>
    /// 是否导出字段
    /// </summary>
    public bool IsExport { get; set; }

    /// <summary>
    /// 是否业务键
    /// </summary>
    public bool IsBusinessKey { get; set; }

    /// <summary>
    /// 是否版本字段
    /// </summary>
    public bool IsVersion { get; set; }
  }

  /// <summary>
  /// 数据库表更新DTO
  /// </summary>
  public class LeanUpdateDbTableDto : LeanCreateDbTableDto
  {
    /// <summary>
    /// 主键
    /// </summary>
    public long Id { get; set; }
  }

  /// <summary>
  /// 数据库表查询条件DTO
  /// </summary>
  public class LeanDbTableQueryDto : LeanPage
  {
    /// <summary>
    /// 表名称
    /// </summary>
    public string? TableName { get; set; }

    /// <summary>
    /// 表描述
    /// </summary>
    public string? TableComment { get; set; }

    /// <summary>
    /// 数据源Id
    /// </summary>
    public long? DataSourceId { get; set; }

    /// <summary>
    /// 表来源
    /// </summary>
    public int? SourceType { get; set; }

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
  /// 数据库表导出DTO
  /// </summary>
  public class LeanDbTableExportDto
  {
    /// <summary>
    /// 表名称
    /// </summary>
    [LeanExcelColumn("表名称", DataType = LeanExcelDataType.String)]
    public string TableName { get; set; } = default!;

    /// <summary>
    /// 表描述
    /// </summary>
    [LeanExcelColumn("表描述", DataType = LeanExcelDataType.String)]
    public string? TableComment { get; set; }

    /// <summary>
    /// 实体类名称
    /// </summary>
    [LeanExcelColumn("实体类名称", DataType = LeanExcelDataType.String)]
    public string ClassName { get; set; } = default!;

    /// <summary>
    /// 使用的模板
    /// </summary>
    [LeanExcelColumn("使用的模板", DataType = LeanExcelDataType.String)]
    public string TemplateCategory { get; set; } = default!;

    /// <summary>
    /// 生成包路径
    /// </summary>
    [LeanExcelColumn("生成包路径", DataType = LeanExcelDataType.String)]
    public string PackageName { get; set; } = default!;

    /// <summary>
    /// 生成模块名
    /// </summary>
    [LeanExcelColumn("生成模块名", DataType = LeanExcelDataType.String)]
    public string ModuleName { get; set; } = default!;

    /// <summary>
    /// 生成业务名
    /// </summary>
    [LeanExcelColumn("生成业务名", DataType = LeanExcelDataType.String)]
    public string BusinessName { get; set; } = default!;

    /// <summary>
    /// 生成功能名
    /// </summary>
    [LeanExcelColumn("生成功能名", DataType = LeanExcelDataType.String)]
    public string FunctionName { get; set; } = default!;

    /// <summary>
    /// 生成功能作者
    /// </summary>
    [LeanExcelColumn("生成功能作者", DataType = LeanExcelDataType.String)]
    public string FunctionAuthor { get; set; } = default!;

    /// <summary>
    /// 父表名称
    /// </summary>
    [LeanExcelColumn("父表名称", DataType = LeanExcelDataType.String)]
    public string? ParentTableName { get; set; }

    /// <summary>
    /// 父表主键
    /// </summary>
    [LeanExcelColumn("父表主键", DataType = LeanExcelDataType.String)]
    public string? ParentTableFk { get; set; }

    /// <summary>
    /// 主键信息
    /// </summary>
    [LeanExcelColumn("主键信息", DataType = LeanExcelDataType.String)]
    public string PkColumn { get; set; } = default!;

    /// <summary>
    /// 表来源
    /// </summary>
    [LeanExcelColumn("表来源", DataType = LeanExcelDataType.Int)]
    public int SourceType { get; set; }

    /// <summary>
    /// 数据源名称
    /// </summary>
    [LeanExcelColumn("数据源名称", DataType = LeanExcelDataType.String)]
    public string DataSourceName { get; set; } = default!;

    /// <summary>
    /// 创建时间
    /// </summary>
    [LeanExcelColumn("创建时间", DataType = LeanExcelDataType.DateTime, Format = "yyyy-MM-dd HH:mm:ss")]
    public DateTime CreateTime { get; set; }
  }

  /// <summary>
  /// 数据库表导入DTO
  /// </summary>
  public class LeanDbTableImportDto
  {
    /// <summary>
    /// 表名称
    /// </summary>
    [LeanExcelColumn("表名称", DataType = LeanExcelDataType.String)]
    public string TableName { get; set; } = default!;

    /// <summary>
    /// 表描述
    /// </summary>
    [LeanExcelColumn("表描述", DataType = LeanExcelDataType.String)]
    public string? TableComment { get; set; }

    /// <summary>
    /// 实体类名称
    /// </summary>
    [LeanExcelColumn("实体类名称", DataType = LeanExcelDataType.String)]
    public string ClassName { get; set; } = default!;

    /// <summary>
    /// 使用的模板
    /// </summary>
    [LeanExcelColumn("使用的模板", DataType = LeanExcelDataType.String)]
    public string TemplateCategory { get; set; } = default!;

    /// <summary>
    /// 生成包路径
    /// </summary>
    [LeanExcelColumn("生成包路径", DataType = LeanExcelDataType.String)]
    public string PackageName { get; set; } = default!;

    /// <summary>
    /// 生成模块名
    /// </summary>
    [LeanExcelColumn("生成模块名", DataType = LeanExcelDataType.String)]
    public string ModuleName { get; set; } = default!;

    /// <summary>
    /// 生成业务名
    /// </summary>
    [LeanExcelColumn("生成业务名", DataType = LeanExcelDataType.String)]
    public string BusinessName { get; set; } = default!;

    /// <summary>
    /// 生成功能名
    /// </summary>
    [LeanExcelColumn("生成功能名", DataType = LeanExcelDataType.String)]
    public string FunctionName { get; set; } = default!;

    /// <summary>
    /// 生成功能作者
    /// </summary>
    [LeanExcelColumn("生成功能作者", DataType = LeanExcelDataType.String)]
    public string FunctionAuthor { get; set; } = default!;

    /// <summary>
    /// 父表名称
    /// </summary>
    [LeanExcelColumn("父表名称", DataType = LeanExcelDataType.String)]
    public string? ParentTableName { get; set; }

    /// <summary>
    /// 父表主键
    /// </summary>
    [LeanExcelColumn("父表主键", DataType = LeanExcelDataType.String)]
    public string? ParentTableFk { get; set; }

    /// <summary>
    /// 主键信息
    /// </summary>
    [LeanExcelColumn("主键信息", DataType = LeanExcelDataType.String)]
    public string PkColumn { get; set; } = default!;

    /// <summary>
    /// 数据源Id
    /// </summary>
    [LeanExcelColumn("数据源Id", DataType = LeanExcelDataType.Long)]
    public long DataSourceId { get; set; }
  }

  /// <summary>
  /// 数据库表导入模板DTO
  /// </summary>
  public class LeanDbTableImportTemplateDto
  {
    /// <summary>
    /// 表名称
    /// </summary>
    [LeanExcelColumn("表名称", DataType = LeanExcelDataType.String)]
    public string TableName { get; set; } = "lean_example";

    /// <summary>
    /// 表描述
    /// </summary>
    [LeanExcelColumn("表描述", DataType = LeanExcelDataType.String)]
    public string? TableComment { get; set; } = "示例表";

    /// <summary>
    /// 实体类名称
    /// </summary>
    [LeanExcelColumn("实体类名称", DataType = LeanExcelDataType.String)]
    public string ClassName { get; set; } = "LeanExample";

    /// <summary>
    /// 使用的模板
    /// </summary>
    [LeanExcelColumn("使用的模板", DataType = LeanExcelDataType.String)]
    public string TemplateCategory { get; set; } = "crud";

    /// <summary>
    /// 生成包路径
    /// </summary>
    [LeanExcelColumn("生成包路径", DataType = LeanExcelDataType.String)]
    public string PackageName { get; set; } = "Lean.Example";

    /// <summary>
    /// 生成模块名
    /// </summary>
    [LeanExcelColumn("生成模块名", DataType = LeanExcelDataType.String)]
    public string ModuleName { get; set; } = "Example";

    /// <summary>
    /// 生成业务名
    /// </summary>
    [LeanExcelColumn("生成业务名", DataType = LeanExcelDataType.String)]
    public string BusinessName { get; set; } = "example";

    /// <summary>
    /// 生成功能名
    /// </summary>
    [LeanExcelColumn("生成功能名", DataType = LeanExcelDataType.String)]
    public string FunctionName { get; set; } = "示例";

    /// <summary>
    /// 生成功能作者
    /// </summary>
    [LeanExcelColumn("生成功能作者", DataType = LeanExcelDataType.String)]
    public string FunctionAuthor { get; set; } = "Lean";

    /// <summary>
    /// 父表名称
    /// </summary>
    [LeanExcelColumn("父表名称", DataType = LeanExcelDataType.String)]
    public string? ParentTableName { get; set; }

    /// <summary>
    /// 父表主键
    /// </summary>
    [LeanExcelColumn("父表主键", DataType = LeanExcelDataType.String)]
    public string? ParentTableFk { get; set; }

    /// <summary>
    /// 主键信息
    /// </summary>
    [LeanExcelColumn("主键信息", DataType = LeanExcelDataType.String)]
    public string PkColumn { get; set; } = "id";

    /// <summary>
    /// 数据源Id
    /// </summary>
    [LeanExcelColumn("数据源Id", DataType = LeanExcelDataType.Long)]
    public long DataSourceId { get; set; } = 1;
  }
}