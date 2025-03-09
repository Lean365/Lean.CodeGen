//===================================================
// 项目名: Lean.CodeGen.Application
// 文件名: LeanSqlDiffLogDto.cs
// 功能描述: SQL差异日志相关DTO
// 创建时间: 2024-03-26
// 作者: Lean
// 版本: 1.0
//===================================================

using System;
using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Common.Excel;

namespace Lean.CodeGen.Application.Dtos.Audit
{
    /// <summary>
    /// SQL差异日志查询DTO
    /// </summary>
    public class LeanSqlDiffLogDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 审计日志ID
        /// </summary>
        public long AuditLogId { get; set; }

        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; } = default!;

        /// <summary>
        /// 表描述
        /// </summary>
        public string? TableDescription { get; set; }

        /// <summary>
        /// 主键名
        /// </summary>
        public string PrimaryKeyName { get; set; } = default!;

        /// <summary>
        /// 主键值
        /// </summary>
        public string PrimaryKeyValue { get; set; } = default!;

        /// <summary>
        /// 变更前数据
        /// </summary>
        public string? BeforeData { get; set; }

        /// <summary>
        /// 变更后数据
        /// </summary>
        public string? AfterData { get; set; }

        /// <summary>
        /// 差异类型
        /// 0-新增
        /// 1-修改
        /// 2-删除
        /// </summary>
        public int DiffType { get; set; }

        /// <summary>
        /// SQL语句
        /// </summary>
        public string? SqlStatement { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }

    /// <summary>
    /// SQL差异日志查询条件DTO
    /// </summary>
    public class LeanSqlDiffLogQueryDto : LeanPage
    {
        /// <summary>
        /// 审计日志ID
        /// </summary>
        public long? AuditLogId { get; set; }

        /// <summary>
        /// 表名
        /// </summary>
        public string? TableName { get; set; }

        /// <summary>
        /// 差异类型
        /// 0-新增
        /// 1-修改
        /// 2-删除
        /// </summary>
        public int? DiffType { get; set; }

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
    /// SQL差异日志导出DTO
    /// </summary>
    public class LeanSqlDiffLogExportDto
    {
        /// <summary>
        /// 审计日志ID
        /// </summary>
        [LeanExcelColumn("审计日志ID")]
        public long AuditLogId { get; set; }

        /// <summary>
        /// 表名
        /// </summary>
        [LeanExcelColumn("表名")]
        public string TableName { get; set; } = default!;

        /// <summary>
        /// 表描述
        /// </summary>
        [LeanExcelColumn("表描述")]
        public string? TableDescription { get; set; }

        /// <summary>
        /// 主键名称
        /// </summary>
        [LeanExcelColumn("主键名称")]
        public string PrimaryKeyName { get; set; } = default!;

        /// <summary>
        /// 主键值
        /// </summary>
        [LeanExcelColumn("主键值")]
        public string PrimaryKeyValue { get; set; } = default!;

        /// <summary>
        /// 变更前数据
        /// </summary>
        [LeanExcelColumn("变更前数据")]
        public string? BeforeData { get; set; }

        /// <summary>
        /// 变更后数据
        /// </summary>
        [LeanExcelColumn("变更后数据")]
        public string? AfterData { get; set; }

        /// <summary>
        /// 差异类型
        /// </summary>
        [LeanExcelColumn("差异类型")]
        public string DiffType { get; set; } = default!;

        /// <summary>
        /// SQL语句
        /// </summary>
        [LeanExcelColumn("SQL语句")]
        public string SqlStatement { get; set; } = default!;

        /// <summary>
        /// 创建时间
        /// </summary>
        [LeanExcelColumn("创建时间")]
        public DateTime CreateTime { get; set; }
    }
}