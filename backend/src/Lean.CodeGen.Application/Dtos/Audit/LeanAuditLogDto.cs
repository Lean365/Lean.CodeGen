//===================================================
// 项目名: Lean.CodeGen.Application
// 文件名: LeanAuditLogDto.cs
// 功能描述: 审计日志相关DTO
// 创建时间: 2024-03-26
// 作者: Lean
// 版本: 1.0
//===================================================

using System;
using System.Collections.Generic;
using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Common.Excel;

namespace Lean.CodeGen.Application.Dtos.Audit
{
  /// <summary>
  /// 审计日志查询DTO
  /// </summary>
  public class LeanAuditLogDto
  {
    /// <summary>
    /// 主键
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 用户ID
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// 用户名称
    /// </summary>
    public string UserName { get; set; } = default!;

    /// <summary>
    /// 实体类型
    /// </summary>
    public string EntityType { get; set; } = default!;

    /// <summary>
    /// 实体ID
    /// </summary>
    public string EntityId { get; set; } = default!;

    /// <summary>
    /// 操作类型
    /// </summary>
    public LeanAuditOperationType OperationType { get; set; }

    /// <summary>
    /// 操作描述
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// 请求IP
    /// </summary>
    public string RequestIp { get; set; } = default!;

    /// <summary>
    /// 请求地点
    /// </summary>
    public string? RequestLocation { get; set; }

    /// <summary>
    /// 浏览器
    /// </summary>
    public string? Browser { get; set; }

    /// <summary>
    /// 操作系统
    /// </summary>
    public string? Os { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 数据差异
    /// </summary>
    public List<LeanSqlDiffLogDto> DataDiffs { get; set; } = new();
  }

  /// <summary>
  /// 审计日志查询条件DTO
  /// </summary>
  public class LeanAuditLogQueryDto : LeanPage
  {
    /// <summary>
    /// 用户ID
    /// </summary>
    public long? UserId { get; set; }

    /// <summary>
    /// 实体类型
    /// </summary>
    public string? EntityType { get; set; }

    /// <summary>
    /// 实体ID
    /// </summary>
    public string? EntityId { get; set; }

    /// <summary>
    /// 操作类型
    /// </summary>
    public LeanAuditOperationType? OperationType { get; set; }

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
  /// 审计日志导出DTO
  /// </summary>
  public class LeanAuditLogExportDto
  {
    /// <summary>
    /// 用户名称
    /// </summary>
    [LeanExcelColumn("用户名称", DataType = LeanExcelDataType.String)]
    public string UserName { get; set; } = default!;

    /// <summary>
    /// 实体类型
    /// </summary>
    [LeanExcelColumn("实体类型", DataType = LeanExcelDataType.String)]
    public string EntityType { get; set; } = default!;

    /// <summary>
    /// 实体ID
    /// </summary>
    [LeanExcelColumn("实体ID", DataType = LeanExcelDataType.String)]
    public string EntityId { get; set; } = default!;

    /// <summary>
    /// 操作类型
    /// </summary>
    [LeanExcelColumn("操作类型", DataType = LeanExcelDataType.String)]
    public string OperationType { get; set; } = default!;

    /// <summary>
    /// 操作描述
    /// </summary>
    [LeanExcelColumn("操作描述", DataType = LeanExcelDataType.String)]
    public string? Description { get; set; }

    /// <summary>
    /// 请求IP
    /// </summary>
    [LeanExcelColumn("请求IP", DataType = LeanExcelDataType.String)]
    public string RequestIp { get; set; } = default!;

    /// <summary>
    /// 请求地点
    /// </summary>
    [LeanExcelColumn("请求地点", DataType = LeanExcelDataType.String)]
    public string? RequestLocation { get; set; }

    /// <summary>
    /// 浏览器
    /// </summary>
    [LeanExcelColumn("浏览器", DataType = LeanExcelDataType.String)]
    public string? Browser { get; set; }

    /// <summary>
    /// 操作系统
    /// </summary>
    [LeanExcelColumn("操作系统", DataType = LeanExcelDataType.String)]
    public string? Os { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    [LeanExcelColumn("创建时间", DataType = LeanExcelDataType.DateTime, Format = "yyyy-MM-dd HH:mm:ss")]
    public DateTime CreateTime { get; set; }
  }

  /// <summary>
  /// 审计日志导入DTO
  /// </summary>
  public class LeanAuditLogImportDto
  {
    /// <summary>
    /// 用户名称
    /// </summary>
    [LeanExcelColumn("用户名称")]
    public string UserName { get; set; } = default!;

    /// <summary>
    /// 实体类型
    /// </summary>
    [LeanExcelColumn("实体类型")]
    public string EntityType { get; set; } = default!;

    /// <summary>
    /// 实体ID
    /// </summary>
    [LeanExcelColumn("实体ID")]
    public string EntityId { get; set; } = default!;

    /// <summary>
    /// 操作类型
    /// </summary>
    [LeanExcelColumn("操作类型")]
    public string OperationType { get; set; } = default!;

    /// <summary>
    /// 操作描述
    /// </summary>
    [LeanExcelColumn("操作描述")]
    public string? Description { get; set; }

    /// <summary>
    /// 请求IP
    /// </summary>
    [LeanExcelColumn("请求IP")]
    public string RequestIp { get; set; } = default!;

    /// <summary>
    /// 请求地点
    /// </summary>
    [LeanExcelColumn("请求地点")]
    public string? RequestLocation { get; set; }

    /// <summary>
    /// 浏览器
    /// </summary>
    [LeanExcelColumn("浏览器")]
    public string? Browser { get; set; }

    /// <summary>
    /// 操作系统
    /// </summary>
    [LeanExcelColumn("操作系统")]
    public string? Os { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    [LeanExcelColumn("创建时间")]
    public DateTime CreateTime { get; set; }
  }
}