//===================================================
// 项目名: Lean.CodeGen.Application
// 文件名: LeanDataSourceDto.cs
// 功能描述: 数据源相关DTO
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
  /// 数据源查询DTO
  /// </summary>
  public class LeanDataSourceDto
  {
    /// <summary>
    /// 主键
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 数据源名称
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// 数据库类型
    /// </summary>
    public string DbType { get; set; } = default!;

    /// <summary>
    /// 主机地址
    /// </summary>
    public string Host { get; set; } = default!;

    /// <summary>
    /// 端口号
    /// </summary>
    public string Port { get; set; } = default!;

    /// <summary>
    /// 数据库名称
    /// </summary>
    public string DatabaseName { get; set; } = default!;

    /// <summary>
    /// 连接字符串
    /// </summary>
    public string ConnectionString { get; set; } = default!;

    /// <summary>
    /// 用户名
    /// </summary>
    public string Username { get; set; } = default!;

    /// <summary>
    /// 密码
    /// </summary>
    public string Password { get; set; } = default!;

    /// <summary>
    /// 是否启用
    /// </summary>
    public bool IsEnabled { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }
  }

  /// <summary>
  /// 数据源创建DTO
  /// </summary>
  public class LeanCreateDataSourceDto
  {
    /// <summary>
    /// 数据源名称
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// 数据库类型
    /// </summary>
    public string DbType { get; set; } = default!;

    /// <summary>
    /// 主机地址
    /// </summary>
    public string Host { get; set; } = default!;

    /// <summary>
    /// 端口号
    /// </summary>
    public string Port { get; set; } = default!;

    /// <summary>
    /// 数据库名称
    /// </summary>
    public string DatabaseName { get; set; } = default!;

    /// <summary>
    /// 连接字符串
    /// </summary>
    public string ConnectionString { get; set; } = default!;

    /// <summary>
    /// 用户名
    /// </summary>
    public string Username { get; set; } = default!;

    /// <summary>
    /// 密码
    /// </summary>
    public string Password { get; set; } = default!;

    /// <summary>
    /// 是否启用
    /// </summary>
    public bool IsEnabled { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }
  }

  /// <summary>
  /// 数据源更新DTO
  /// </summary>
  public class LeanUpdateDataSourceDto : LeanCreateDataSourceDto
  {
    /// <summary>
    /// 主键
    /// </summary>
    public long Id { get; set; }
  }

  /// <summary>
  /// 数据源查询条件DTO
  /// </summary>
  public class LeanDataSourceQueryDto : LeanPage
  {
    /// <summary>
    /// 数据源名称
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// 数据库类型
    /// </summary>
    public string? DbType { get; set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    public bool? IsEnabled { get; set; }

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
  /// 数据源导出DTO
  /// </summary>
  public class LeanDataSourceExportDto
  {
    /// <summary>
    /// 数据源名称
    /// </summary>
    [LeanExcelColumn("数据源名称", DataType = LeanExcelDataType.String)]
    public string Name { get; set; } = default!;

    /// <summary>
    /// 数据库类型
    /// </summary>
    [LeanExcelColumn("数据库类型", DataType = LeanExcelDataType.String)]
    public string DbType { get; set; } = default!;

    /// <summary>
    /// 主机地址
    /// </summary>
    [LeanExcelColumn("主机地址", DataType = LeanExcelDataType.String)]
    public string Host { get; set; } = default!;

    /// <summary>
    /// 端口号
    /// </summary>
    [LeanExcelColumn("端口号", DataType = LeanExcelDataType.String)]
    public string Port { get; set; } = default!;

    /// <summary>
    /// 数据库名称
    /// </summary>
    [LeanExcelColumn("数据库名称", DataType = LeanExcelDataType.String)]
    public string DatabaseName { get; set; } = default!;

    /// <summary>
    /// 用户名
    /// </summary>
    [LeanExcelColumn("用户名", DataType = LeanExcelDataType.String)]
    public string Username { get; set; } = default!;

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
    /// 创建时间
    /// </summary>
    [LeanExcelColumn("创建时间", DataType = LeanExcelDataType.DateTime, Format = "yyyy-MM-dd HH:mm:ss")]
    public DateTime CreateTime { get; set; }
  }

  /// <summary>
  /// 数据源导入DTO
  /// </summary>
  public class LeanDataSourceImportDto
  {
    /// <summary>
    /// 数据源名称
    /// </summary>
    [LeanExcelColumn("数据源名称", DataType = LeanExcelDataType.String)]
    public string Name { get; set; } = default!;

    /// <summary>
    /// 数据库类型
    /// </summary>
    [LeanExcelColumn("数据库类型", DataType = LeanExcelDataType.String)]
    public string DbType { get; set; } = default!;

    /// <summary>
    /// 主机地址
    /// </summary>
    [LeanExcelColumn("主机地址", DataType = LeanExcelDataType.String)]
    public string Host { get; set; } = default!;

    /// <summary>
    /// 端口号
    /// </summary>
    [LeanExcelColumn("端口号", DataType = LeanExcelDataType.String)]
    public string Port { get; set; } = default!;

    /// <summary>
    /// 数据库名称
    /// </summary>
    [LeanExcelColumn("数据库名称", DataType = LeanExcelDataType.String)]
    public string DatabaseName { get; set; } = default!;

    /// <summary>
    /// 用户名
    /// </summary>
    [LeanExcelColumn("用户名", DataType = LeanExcelDataType.String)]
    public string Username { get; set; } = default!;

    /// <summary>
    /// 密码
    /// </summary>
    [LeanExcelColumn("密码", DataType = LeanExcelDataType.String)]
    public string Password { get; set; } = default!;

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
  }

  /// <summary>
  /// 数据源导入模板DTO
  /// </summary>
  public class LeanDataSourceImportTemplateDto
  {
    [LeanExcelColumn("数据源名称", DataType = LeanExcelDataType.String)]
    public string Name { get; set; } = default!;

    [LeanExcelColumn("数据库类型", DataType = LeanExcelDataType.String)]
    public string DbType { get; set; } = default!;

    [LeanExcelColumn("主机地址", DataType = LeanExcelDataType.String)]
    public string Host { get; set; } = default!;

    [LeanExcelColumn("端口号", DataType = LeanExcelDataType.String)]
    public string Port { get; set; } = default!;

    [LeanExcelColumn("数据库名称", DataType = LeanExcelDataType.String)]
    public string DatabaseName { get; set; } = default!;

    [LeanExcelColumn("用户名", DataType = LeanExcelDataType.String)]
    public string Username { get; set; } = default!;

    [LeanExcelColumn("密码", DataType = LeanExcelDataType.String)]
    public string Password { get; set; } = default!;

    [LeanExcelColumn("是否启用", DataType = LeanExcelDataType.Boolean)]
    public bool IsEnabled { get; set; }

    [LeanExcelColumn("备注", DataType = LeanExcelDataType.String)]
    public string? Remark { get; set; }
  }
}