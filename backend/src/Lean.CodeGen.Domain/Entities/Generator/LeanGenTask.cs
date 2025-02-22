//===================================================
// 项目名: Lean.CodeGen.Domain
// 文件名: LeanGenTask.cs
// 功能描述: 代码生成任务实体
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
  /// 代码生成任务实体
  /// </summary>
  [SugarTable("lean_gen_task", "代码生成任务")]
  public class LeanGenTask : LeanBaseEntity
  {
    /// <summary>
    /// 任务名称
    /// </summary>
    /// <remarks>
    /// 代码生成任务的名称
    /// </remarks>
    [SugarColumn(ColumnName = "name", ColumnDescription = "任务名称", Length = 100, IsNullable = false, ColumnDataType = "nvarchar")]
    public string Name { get; set; } = default!;

    /// <summary>
    /// 任务状态
    /// </summary>
    /// <remarks>
    /// 任务执行状态：0-等待执行，1-执行中，2-执行成功，3-执行失败
    /// </remarks>
    [SugarColumn(ColumnName = "status", ColumnDescription = "任务状态", IsNullable = false, DefaultValue = "0", ColumnDataType = "int")]
    public int Status { get; set; }

    /// <summary>
    /// 开始时间
    /// </summary>
    /// <remarks>
    /// 任务开始执行的时间
    /// </remarks>
    [SugarColumn(ColumnName = "start_time", ColumnDescription = "开始时间", IsNullable = true, ColumnDataType = "datetime")]
    public DateTime? StartTime { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    /// <remarks>
    /// 任务执行完成的时间
    /// </remarks>
    [SugarColumn(ColumnName = "end_time", ColumnDescription = "结束时间", IsNullable = true, ColumnDataType = "datetime")]
    public DateTime? EndTime { get; set; }

    /// <summary>
    /// 错误信息
    /// </summary>
    /// <remarks>
    /// 任务执行失败时的错误信息
    /// </remarks>
    [SugarColumn(ColumnName = "error_message", ColumnDescription = "错误信息", Length = 2000, IsNullable = true, ColumnDataType = "nvarchar")]
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// 配置Id
    /// </summary>
    /// <remarks>
    /// 关联的代码生成配置Id
    /// </remarks>
    [SugarColumn(ColumnName = "config_id", ColumnDescription = "配置Id", IsNullable = false, ColumnDataType = "bigint")]
    public long ConfigId { get; set; }

    /// <summary>
    /// 排序号
    /// </summary>
    /// <remarks>
    /// 显示顺序，数值越小越靠前
    /// </remarks>
    [SugarColumn(ColumnName = "order_num", ColumnDescription = "排序号", IsNullable = false, DefaultValue = "0", ColumnDataType = "int")]
    public int OrderNum { get; set; }

    /// <summary>
    /// 所属配置
    /// </summary>
    /// <remarks>
    /// 关联的代码生成配置信息
    /// </remarks>
    [Navigate(NavigateType.ManyToOne, nameof(ConfigId))]
    public virtual LeanGenConfig Config { get; set; }
  }
}