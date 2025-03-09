//===================================================
// 项目名: Lean.CodeGen.Domain
// 文件名: LeanGenConfig.cs
// 功能描述: 代码生成配置实体
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
  /// 代码生成配置实体
  /// </summary>
  [SugarTable("lean_gen_config", "代码生成配置")]
  public class LeanGenConfig : LeanBaseEntity
  {
    /// <summary>
    /// 配置名称
    /// </summary>
    /// <remarks>
    /// 用于标识不同的代码生成配置
    /// </remarks>
    [SugarColumn(ColumnName = "name", ColumnDescription = "配置名称", Length = 100, IsNullable = false, ColumnDataType = "nvarchar")]
    public string Name { get; set; } = default!;

    /// <summary>
    /// 生成代码方式（0zip压缩包 1自定义路径）
    /// </summary>
    [SugarColumn(ColumnName = "gen_type", ColumnDescription = "生成代码方式", IsNullable = false, DefaultValue = "0", ColumnDataType = "int")]
    public int GenType { get; set; }

    /// <summary>
    /// 生成路径（不填默认项目路径）
    /// </summary>
    [SugarColumn(ColumnName = "gen_path", ColumnDescription = "生成路径", Length = 500, IsNullable = true, ColumnDataType = "nvarchar")]
    public string? GenPath { get; set; }

    /// <summary>
    /// 后端工具
    /// </summary>
    [SugarColumn(ColumnName = "backend_tool", ColumnDescription = "后端工具", Length = 100, IsNullable = false, ColumnDataType = "nvarchar")]
    public string BackendTool { get; set; } = default!;

    /// <summary>
    /// 前端工具
    /// </summary>
    [SugarColumn(ColumnName = "frontend_tool", ColumnDescription = "前端工具", Length = 100, IsNullable = false, ColumnDataType = "nvarchar")]
    public string FrontendTool { get; set; } = default!;

    /// <summary>
    /// 是否覆盖已有文件
    /// 0-否
    /// 1-是
    /// </summary>
    /// <remarks>
    /// 生成代码时是否覆盖已存在的文件
    /// </remarks>
    [SugarColumn(ColumnName = "is_override", ColumnDescription = "是否覆盖已有文件", IsNullable = false, ColumnDataType = "int")]
    public int IsOverride { get; set; }

    /// <summary>
    /// 是否移除表前缀
    /// 0-否
    /// 1-是
    /// </summary>
    [SugarColumn(ColumnName = "is_remove_prefix", ColumnDescription = "是否移除表前缀", IsNullable = false, ColumnDataType = "int")]
    public int IsRemovePrefix { get; set; }

    /// <summary>
    /// 表前缀
    /// </summary>
    /// <remarks>
    /// 需要去除的表前缀
    /// </remarks>
    [SugarColumn(ColumnName = "table_prefix", ColumnDescription = "表前缀", Length = 50, IsNullable = true, ColumnDataType = "nvarchar")]
    public string? TablePrefix { get; set; }

    /// <summary>
    /// 排序号
    /// </summary>
    /// <remarks>
    /// 显示顺序，数值越小越靠前
    /// </remarks>
    [SugarColumn(ColumnName = "order_num", ColumnDescription = "排序号", IsNullable = false, DefaultValue = "0", ColumnDataType = "int")]
    public int OrderNum { get; set; }

    /// <summary>
    /// 关联的模板列表
    /// </summary>
    /// <remarks>
    /// 与代码生成模板的一对多关系
    /// </remarks>
    [Navigate(NavigateType.OneToMany, nameof(LeanGenTemplate.ConfigId))]
    public virtual ICollection<LeanGenTemplate> Templates { get; set; } = new List<LeanGenTemplate>();
  }
}