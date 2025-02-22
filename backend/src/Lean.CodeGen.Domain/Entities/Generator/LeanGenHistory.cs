//===================================================
// 项目名: Lean.CodeGen.Domain
// 文件名: LeanGenHistory.cs
// 功能描述: 代码生成历史记录实体
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
  /// 代码生成历史记录实体
  /// </summary>
  [SugarTable("lean_gen_history", "代码生成历史记录")]
  public class LeanGenHistory : LeanBaseEntity
  {
    /// <summary>
    /// 任务Id
    /// </summary>
    /// <remarks>
    /// 关联的代码生成任务Id
    /// </remarks>
    [SugarColumn(ColumnName = "task_id", ColumnDescription = "任务Id", IsNullable = false, ColumnDataType = "bigint")]
    public long TaskId { get; set; }

    /// <summary>
    /// 表Id
    /// </summary>
    /// <remarks>
    /// 关联的数据库表Id
    /// </remarks>
    [SugarColumn(ColumnName = "table_id", ColumnDescription = "表Id", IsNullable = false, ColumnDataType = "bigint")]
    public long TableId { get; set; }

    /// <summary>
    /// 生成状态
    /// </summary>
    /// <remarks>
    /// 生成状态：0-成功，1-失败
    /// </remarks>
    [SugarColumn(ColumnName = "status", ColumnDescription = "生成状态", IsNullable = false, DefaultValue = "0", ColumnDataType = "int")]
    public int Status { get; set; }

    /// <summary>
    /// 生成时间
    /// </summary>
    /// <remarks>
    /// 代码生成的时间
    /// </remarks>
    [SugarColumn(ColumnName = "generate_time", ColumnDescription = "生成时间", IsNullable = false, ColumnDataType = "datetime")]
    public DateTime GenerateTime { get; set; }

    /// <summary>
    /// 生成路径
    /// </summary>
    /// <remarks>
    /// 代码生成的输出路径
    /// </remarks>
    [SugarColumn(ColumnName = "output_path", ColumnDescription = "生成路径", Length = 500, IsNullable = false, ColumnDataType = "nvarchar")]
    public string OutputPath { get; set; } = default!;

    /// <summary>
    /// 错误信息
    /// </summary>
    /// <remarks>
    /// 生成失败时的错误信息
    /// </remarks>
    [SugarColumn(ColumnName = "error_message", ColumnDescription = "错误信息", Length = 2000, IsNullable = true, ColumnDataType = "nvarchar")]
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// 所属任务
    /// </summary>
    /// <remarks>
    /// 关联的代码生成任务
    /// </remarks>
    [Navigate(NavigateType.ManyToOne, nameof(TaskId))]
    public virtual LeanGenTask Task { get; set; }

    /// <summary>
    /// 所属表
    /// </summary>
    /// <remarks>
    /// 关联的数据库表
    /// </remarks>
    [Navigate(NavigateType.ManyToOne, nameof(TableId))]
    public virtual LeanDbTable Table { get; set; }
  }
}