//===================================================
// 项目名: Lean.CodeGen.Domain
// 文件名: LeanSqlDiffLog.cs
// 功能描述: SQL差异日志实体
// 创建时间: 2024-03-26
// 作者: Lean
// 版本: 1.0
//===================================================

using SqlSugar;
using Lean.CodeGen.Common.Enums;

namespace Lean.CodeGen.Domain.Entities.Audit;

/// <summary>
/// SQL差异日志实体
/// </summary>
[SugarTable("lean_mon_sqldiff_log", "SQL差异日志表")]
[SugarIndex("idx_audit", nameof(AuditLogId), OrderByType.Asc)]
[SugarIndex("idx_table", nameof(TableName), OrderByType.Asc)]
public class LeanSqlDiffLog : LeanBaseEntity
{
    /// <summary>
    /// 审计日志ID
    /// </summary>
    [SugarColumn(ColumnName = "audit_log_id", ColumnDescription = "审计日志ID", IsNullable = false, ColumnDataType = "bigint")]
    public long AuditLogId { get; set; }

    /// <summary>
    /// 表名
    /// </summary>
    [SugarColumn(ColumnName = "table_name", ColumnDescription = "表名", Length = 100, IsNullable = false, ColumnDataType = "nvarchar")]
    public string TableName { get; set; } = string.Empty;

    /// <summary>
    /// 表描述
    /// </summary>
    [SugarColumn(ColumnName = "table_description", ColumnDescription = "表描述", Length = 200, IsNullable = true, ColumnDataType = "nvarchar")]
    public string? TableDescription { get; set; }

    /// <summary>
    /// 主键名
    /// </summary>
    [SugarColumn(ColumnName = "primary_key_name", ColumnDescription = "主键名", Length = 100, IsNullable = false, ColumnDataType = "nvarchar")]
    public string PrimaryKeyName { get; set; } = string.Empty;

    /// <summary>
    /// 主键值
    /// </summary>
    [SugarColumn(ColumnName = "primary_key_value", ColumnDescription = "主键值", Length = 100, IsNullable = false, ColumnDataType = "nvarchar")]
    public string PrimaryKeyValue { get; set; } = string.Empty;

    /// <summary>
    /// 变更前数据
    /// </summary>
    [SugarColumn(ColumnName = "before_data", ColumnDescription = "变更前数据(JSON格式)", Length = -1, IsNullable = true, ColumnDataType = "nvarchar")]
    public string? BeforeData { get; set; }

    /// <summary>
    /// 变更后数据
    /// </summary>
    [SugarColumn(ColumnName = "after_data", ColumnDescription = "变更后数据(JSON格式)", Length = -1, IsNullable = true, ColumnDataType = "nvarchar")]
    public string? AfterData { get; set; }

    /// <summary>
    /// 差异类型
    /// </summary>
    [SugarColumn(ColumnName = "diff_type", ColumnDescription = "差异类型", IsNullable = false, DefaultValue = "0", ColumnDataType = "int")]
    public LeanDiffType DiffType { get; set; }

    /// <summary>
    /// SQL语句
    /// </summary>
    [SugarColumn(ColumnName = "sql_statement", ColumnDescription = "SQL语句", Length = 4000, IsNullable = true, ColumnDataType = "nvarchar")]
    public string? SqlStatement { get; set; }

    /// <summary>
    /// 审计日志
    /// </summary>
    [Navigate(NavigateType.OneToOne, nameof(AuditLogId))]
    public virtual LeanAuditLog AuditLog { get; set; } = default!;
}