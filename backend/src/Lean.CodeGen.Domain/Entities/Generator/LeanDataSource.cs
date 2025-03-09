//===================================================
// 项目名: Lean.CodeGen.Domain
// 文件名: LeanDataSource.cs
// 功能描述: 数据源管理实体
// 创建时间: 2024-03-26
// 作者: Lean
// 版本: 1.0
//===================================================

using System;
using System.Collections.Generic;
using SqlSugar;
using Lean.CodeGen.Domain.Entities;

namespace Lean.CodeGen.Domain.Entities.Generator
{
  /// <summary>
  /// 数据源管理实体
  /// </summary>
  [SugarTable("lean_gen_data_source", "数据源管理")]
  public class LeanDataSource : LeanBaseEntity
  {
    /// <summary>
    /// 数据源名称
    /// </summary>
    [SugarColumn(ColumnName = "name", ColumnDescription = "数据源名称", Length = 100, IsNullable = false, ColumnDataType = "nvarchar")]
    public string Name { get; set; } = default!;

    /// <summary>
    /// 数据库类型（MySQL、SQLServer、PostgreSQL等）
    /// </summary>
    [SugarColumn(ColumnName = "db_type", ColumnDescription = "数据库类型", Length = 50, IsNullable = false, ColumnDataType = "nvarchar")]
    public string DbType { get; set; } = default!;

    /// <summary>
    /// 连接字符串
    /// </summary>
    [SugarColumn(ColumnName = "connection_string", ColumnDescription = "连接字符串", Length = 1000, IsNullable = false, ColumnDataType = "nvarchar")]
    public string ConnectionString { get; set; } = default!;

    /// <summary>
    /// 主机地址
    /// </summary>
    [SugarColumn(ColumnName = "host", ColumnDescription = "主机地址", Length = 200, IsNullable = false, ColumnDataType = "nvarchar")]
    public string Host { get; set; } = default!;

    /// <summary>
    /// 端口号
    /// </summary>
    [SugarColumn(ColumnName = "port", ColumnDescription = "端口号", Length = 10, IsNullable = false, ColumnDataType = "nvarchar")]
    public string Port { get; set; } = default!;

    /// <summary>
    /// 数据库名称
    /// </summary>
    [SugarColumn(ColumnName = "database_name", ColumnDescription = "数据库名称", Length = 100, IsNullable = false, ColumnDataType = "nvarchar")]
    public string DatabaseName { get; set; } = default!;

    /// <summary>
    /// 用户名
    /// </summary>
    [SugarColumn(ColumnName = "username", ColumnDescription = "用户名", Length = 100, IsNullable = false, ColumnDataType = "nvarchar")]
    public string Username { get; set; } = default!;

    /// <summary>
    /// 密码
    /// </summary>
    [SugarColumn(ColumnName = "password", ColumnDescription = "密码", Length = 100, IsNullable = false, ColumnDataType = "nvarchar")]
    public string Password { get; set; } = default!;

    /// <summary>
    /// 是否启用
    /// 0-禁用
    /// 1-启用
    /// </summary>
    [SugarColumn(ColumnName = "is_enabled", ColumnDescription = "是否启用", IsNullable = false, ColumnDataType = "int")]
    public int IsEnabled { get; set; }

    /// <summary>
    /// 排序号
    /// </summary>
    /// <remarks>
    /// 显示顺序，数值越小越靠前
    /// </remarks>
    [SugarColumn(ColumnName = "order_num", ColumnDescription = "排序号", IsNullable = false, DefaultValue = "0", ColumnDataType = "int")]
    public int OrderNum { get; set; }

    /// <summary>
    /// 关联的数据库表
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(LeanDbTable.DataSourceId))]
    public virtual ICollection<LeanDbTable> Tables { get; set; } = new List<LeanDbTable>();
  }
}