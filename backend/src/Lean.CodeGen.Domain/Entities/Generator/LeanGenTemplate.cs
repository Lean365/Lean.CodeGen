//===================================================
// 项目名: Lean.CodeGen.Domain
// 文件名: LeanGenTemplate.cs
// 功能描述: 代码生成模板实体
// 创建时间: 2024-03-26
// 作者: Lean
// 版本: 1.0
//===================================================

using System;
using SqlSugar;
using Lean.CodeGen.Domain.Entities;

namespace Lean.CodeGen.Domain.Entities.Generator
{
  /// <summary>
  /// 代码生成模板实体
  /// </summary>
  [SugarTable("lean_gen_template", "代码生成模板")]
  public class LeanGenTemplate : LeanBaseEntity
  {
    /// <summary>
    /// 模板名称
    /// </summary>
    /// <remarks>
    /// 用于标识不同的代码模板
    /// </remarks>
    [SugarColumn(ColumnName = "name", ColumnDescription = "模板名称", Length = 100, IsNullable = false, ColumnDataType = "nvarchar")]
    public string Name { get; set; } = default!;

    /// <summary>
    /// 模板组
    /// </summary>
    /// <remarks>
    /// 用于标识不同的代码模板组
    /// </remarks>
    [SugarColumn(ColumnName = "group_name", ColumnDescription = "模板组", Length = 100, IsNullable = false, ColumnDataType = "nvarchar")]
    public string GroupName { get; set; } = default!;

    /// <summary>
    /// 模板文件名
    /// </summary>
    /// <remarks>
    /// 生成文件的文件名
    /// </remarks>
    [SugarColumn(ColumnName = "file_name", ColumnDescription = "模板文件名", Length = 200, IsNullable = false, ColumnDataType = "nvarchar")]
    public string FileName { get; set; } = default!;

    /// <summary>
    /// 模板内容
    /// </summary>
    /// <remarks>
    /// 代码模板的具体内容
    /// </remarks>
    [SugarColumn(ColumnName = "content", ColumnDescription = "模板内容", IsNullable = false, ColumnDataType = "ntext")]
    public string Content { get; set; } = default!;

    /// <summary>
    /// 模板引擎
    /// </summary>
    /// <remarks>
    /// 模板引擎（如：Razor、Handlebars等）
    /// </remarks>
    [SugarColumn(ColumnName = "engine", ColumnDescription = "模板引擎", Length = 50, IsNullable = false, ColumnDataType = "nvarchar")]
    public string Engine { get; set; } = default!;

    /// <summary>
    /// 目标文件路径
    /// </summary>
    /// <remarks>
    /// 生成文件的目标路径
    /// </remarks>
    [SugarColumn(ColumnName = "target_path", ColumnDescription = "目标文件路径", Length = 500, IsNullable = false, ColumnDataType = "nvarchar")]
    public string TargetPath { get; set; } = default!;

    /// <summary>
    /// 语言类型
    /// </summary>
    /// <remarks>
    /// 生成文件的语言类型（如：C#、TypeScript等）
    /// </remarks>
    [SugarColumn(ColumnName = "language", ColumnDescription = "语言类型", Length = 50, IsNullable = false, ColumnDataType = "nvarchar")]
    public string Language { get; set; } = default!;

    /// <summary>
    /// 是否启用（0：禁用 1：启用）
    /// </summary>
    /// <remarks>
    /// 标识模板是否启用
    /// </remarks>
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
    /// 配置Id
    /// </summary>
    /// <remarks>
    /// 关联的代码生成配置Id
    /// </remarks>
    [SugarColumn(ColumnName = "config_id", ColumnDescription = "配置Id", IsNullable = false, ColumnDataType = "bigint")]
    public long ConfigId { get; set; }

    /// <summary>
    /// 所属配置
    /// </summary>
    /// <remarks>
    /// 关联的代码生成配置信息
    /// </remarks>
    [Navigate(NavigateType.ManyToOne, nameof(ConfigId))]
    public virtual LeanGenConfig Config { get; set; } = default!;
  }
}