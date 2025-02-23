//===================================================
// 项目名: Lean.CodeGen.Domain
// 文件名: LeanExceptionLog.cs
// 功能描述: 异常日志实体
// 创建时间: 2024-03-26
// 作者: Lean
// 版本: 1.0
//===================================================

using SqlSugar;
using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Domain.Entities.Identity;

namespace Lean.CodeGen.Domain.Entities.Audit;

/// <summary>
/// 异常日志实体
/// </summary>
[SugarTable("lean_mon_exception_log", "异常日志表")]
[SugarIndex("idx_user", nameof(UserId), OrderByType.Asc)]
[SugarIndex("idx_app", nameof(AppName), OrderByType.Asc)]
public class LeanExceptionLog : LeanBaseEntity
{
    /// <summary>
    /// 用户ID
    /// </summary>
    [SugarColumn(ColumnName = "user_id", ColumnDescription = "用户ID", IsNullable = true, ColumnDataType = "bigint")]
    public long? UserId { get; set; }

    /// <summary>
    /// 应用名称
    /// </summary>
    [SugarColumn(ColumnName = "app_name", ColumnDescription = "应用名称", Length = 50, IsNullable = false, ColumnDataType = "nvarchar")]
    public string AppName { get; set; } = string.Empty;

    /// <summary>
    /// 环境名称
    /// </summary>
    [SugarColumn(ColumnName = "environment", ColumnDescription = "环境名称", Length = 50, IsNullable = false, ColumnDataType = "nvarchar")]
    public string Environment { get; set; } = string.Empty;

    /// <summary>
    /// 异常类型
    /// </summary>
    [SugarColumn(ColumnName = "exception_type", ColumnDescription = "异常类型", Length = 200, IsNullable = false, ColumnDataType = "nvarchar")]
    public string ExceptionType { get; set; } = string.Empty;

    /// <summary>
    /// 异常消息
    /// </summary>
    [SugarColumn(ColumnName = "exception_message", ColumnDescription = "异常消息", Length = 2000, IsNullable = false, ColumnDataType = "nvarchar")]
    public string ExceptionMessage { get; set; } = string.Empty;

    /// <summary>
    /// 异常堆栈
    /// </summary>
    [SugarColumn(ColumnName = "stack_trace", ColumnDescription = "异常堆栈", Length = -1, IsNullable = true, ColumnDataType = "nvarchar")]
    public string? StackTrace { get; set; }

    /// <summary>
    /// 异常源
    /// </summary>
    [SugarColumn(ColumnName = "exception_source", ColumnDescription = "异常源", Length = 200, IsNullable = true, ColumnDataType = "nvarchar")]
    public string? ExceptionSource { get; set; }

    /// <summary>
    /// 请求URL
    /// </summary>
    [SugarColumn(ColumnName = "request_url", ColumnDescription = "请求URL", Length = 500, IsNullable = true, ColumnDataType = "nvarchar")]
    public string? RequestUrl { get; set; }

    /// <summary>
    /// 请求方法
    /// </summary>
    [SugarColumn(ColumnName = "request_method", ColumnDescription = "请求方法", Length = 10, IsNullable = true, ColumnDataType = "nvarchar")]
    public string? RequestMethod { get; set; }

    /// <summary>
    /// 请求参数
    /// </summary>
    [SugarColumn(ColumnName = "request_params", ColumnDescription = "请求参数(JSON格式)", Length = -1, IsNullable = true, ColumnDataType = "nvarchar")]
    public string? RequestParams { get; set; }

    /// <summary>
    /// 请求头
    /// </summary>
    [SugarColumn(ColumnName = "request_headers", ColumnDescription = "请求头(JSON格式)", Length = -1, IsNullable = true, ColumnDataType = "nvarchar")]
    public string? RequestHeaders { get; set; }

    /// <summary>
    /// 客户端IP
    /// </summary>
    [SugarColumn(ColumnName = "client_ip", ColumnDescription = "客户端IP", Length = 50, IsNullable = true, ColumnDataType = "nvarchar")]
    public string? ClientIp { get; set; }

    /// <summary>
    /// 客户端浏览器
    /// </summary>
    [SugarColumn(ColumnName = "browser", ColumnDescription = "客户端浏览器", Length = 100, IsNullable = true, ColumnDataType = "nvarchar")]
    public string? Browser { get; set; }

    /// <summary>
    /// 操作系统
    /// </summary>
    [SugarColumn(ColumnName = "os", ColumnDescription = "操作系统", Length = 100, IsNullable = true, ColumnDataType = "nvarchar")]
    public string? Os { get; set; }

    /// <summary>
    /// 异常级别
    /// </summary>
    [SugarColumn(ColumnName = "log_level", ColumnDescription = "异常级别", IsNullable = false, DefaultValue = "0", ColumnDataType = "int")]
    public LeanLogLevel LogLevel { get; set; }

    /// <summary>
    /// 处理状态
    /// </summary>
    [SugarColumn(ColumnName = "handle_status", ColumnDescription = "处理状态", IsNullable = false, DefaultValue = "0", ColumnDataType = "int")]
    public LeanHandleStatus HandleStatus { get; set; }

    /// <summary>
    /// 处理时间
    /// </summary>
    [SugarColumn(ColumnName = "handle_time", ColumnDescription = "处理时间", IsNullable = true, ColumnDataType = "datetime")]
    public DateTime? HandleTime { get; set; }

    /// <summary>
    /// 处理人ID
    /// </summary>
    [SugarColumn(ColumnName = "handler_id", ColumnDescription = "处理人ID", IsNullable = true, ColumnDataType = "bigint")]
    public long? HandlerId { get; set; }

    /// <summary>
    /// 处理人名称
    /// </summary>
    [SugarColumn(ColumnName = "handler_name", ColumnDescription = "处理人名称", Length = 50, IsNullable = true, ColumnDataType = "nvarchar")]
    public string? HandlerName { get; set; }

    /// <summary>
    /// 处理备注
    /// </summary>
    [SugarColumn(ColumnName = "handle_remark", ColumnDescription = "处理备注", Length = 500, IsNullable = true, ColumnDataType = "nvarchar")]
    public string? HandleRemark { get; set; }

    /// <summary>
    /// 用户
    /// </summary>
    [Navigate(NavigateType.OneToOne, nameof(UserId))]
    public virtual LeanUser? User { get; set; }

    /// <summary>
    /// 处理人
    /// </summary>
    [Navigate(NavigateType.OneToOne, nameof(HandlerId))]
    public virtual LeanUser? Handler { get; set; }
}