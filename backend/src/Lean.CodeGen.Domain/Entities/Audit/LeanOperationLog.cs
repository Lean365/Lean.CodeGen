//===================================================
// 项目名: Lean.CodeGen.Domain
// 文件名: LeanOperationLog.cs
// 功能描述: 操作日志实体
// 创建时间: 2024-03-26
// 作者: Lean
// 版本: 1.0
//===================================================

using SqlSugar;
using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Domain.Entities.Identity;

namespace Lean.CodeGen.Domain.Entities.Audit;

/// <summary>
/// 操作日志实体
/// </summary>
[SugarTable("lean_mon_operation_log", "操作日志表")]
[SugarIndex("idx_user", nameof(UserId), OrderByType.Asc)]
[SugarIndex("idx_module", nameof(Module), OrderByType.Asc)]
public class LeanOperationLog : LeanBaseEntity
{
    /// <summary>
    /// 用户ID
    /// </summary>
    /// <remarks>
    /// 操作用户的ID
    /// </remarks>
    [SugarColumn(ColumnName = "user_id", ColumnDescription = "用户ID", IsNullable = false, ColumnDataType = "bigint")]
    public long UserId { get; set; }

    /// <summary>
    /// 模块
    /// </summary>
    /// <remarks>
    /// 操作所属的功能模块
    /// </remarks>
    [SugarColumn(ColumnName = "module", ColumnDescription = "模块", Length = 50, IsNullable = false, ColumnDataType = "nvarchar")]
    public string Module { get; set; } = string.Empty;

    /// <summary>
    /// 操作名称
    /// </summary>
    /// <remarks>
    /// 具体的操作名称
    /// </remarks>
    [SugarColumn(ColumnName = "operation", ColumnDescription = "操作名称", Length = 100, IsNullable = false, ColumnDataType = "nvarchar")]
    public string Operation { get; set; } = string.Empty;

    /// <summary>
    /// 请求方法
    /// </summary>
    /// <remarks>
    /// HTTP请求方法：GET、POST等
    /// </remarks>
    [SugarColumn(ColumnName = "request_method", ColumnDescription = "请求方法", Length = 10, IsNullable = false, ColumnDataType = "nvarchar")]
    public string RequestMethod { get; set; } = string.Empty;

    /// <summary>
    /// 请求URL
    /// </summary>
    /// <remarks>
    /// 请求的完整URL地址
    /// </remarks>
    [SugarColumn(ColumnName = "request_url", ColumnDescription = "请求URL", Length = 500, IsNullable = false, ColumnDataType = "nvarchar")]
    public string RequestUrl { get; set; } = string.Empty;

    /// <summary>
    /// 请求参数
    /// </summary>
    /// <remarks>
    /// 请求的参数信息，JSON格式
    /// </remarks>
    [SugarColumn(ColumnName = "request_param", ColumnDescription = "请求参数", Length = 4000, IsNullable = true, ColumnDataType = "nvarchar")]
    public string? RequestParam { get; set; }

    /// <summary>
    /// 请求IP
    /// </summary>
    /// <remarks>
    /// 发起请求的IP地址
    /// </remarks>
    [SugarColumn(ColumnName = "request_ip", ColumnDescription = "请求IP", Length = 50, IsNullable = false, ColumnDataType = "nvarchar")]
    public string RequestIp { get; set; } = string.Empty;

    /// <summary>
    /// 客户端IP
    /// </summary>
    /// <remarks>
    /// 客户端的真实IP地址，通过X-Forwarded-For等头部获取
    /// </remarks>
    [SugarColumn(ColumnName = "client_ip", ColumnDescription = "客户端IP", Length = 50, IsNullable = true, ColumnDataType = "nvarchar")]
    public string? ClientIp { get; set; }

    /// <summary>
    /// 请求地点
    /// </summary>
    /// <remarks>
    /// 根据IP解析的请求地点
    /// </remarks>
    [SugarColumn(ColumnName = "request_location", ColumnDescription = "请求地点", Length = 100, IsNullable = true, ColumnDataType = "nvarchar")]
    public string? RequestLocation { get; set; }

    /// <summary>
    /// 浏览器
    /// </summary>
    /// <remarks>
    /// 操作使用的浏览器信息
    /// </remarks>
    [SugarColumn(ColumnName = "browser", ColumnDescription = "浏览器", Length = 50, IsNullable = true, ColumnDataType = "nvarchar")]
    public string? Browser { get; set; }

    /// <summary>
    /// 操作系统
    /// </summary>
    /// <remarks>
    /// 操作使用的操作系统信息
    /// </remarks>
    [SugarColumn(ColumnName = "os", ColumnDescription = "操作系统", Length = 50, IsNullable = true, ColumnDataType = "nvarchar")]
    public string? Os { get; set; }

    /// <summary>
    /// 响应结果
    /// </summary>
    /// <remarks>
    /// 操作的响应结果，JSON格式
    /// </remarks>
    [SugarColumn(ColumnName = "response_result", ColumnDescription = "响应结果", Length = 4000, IsNullable = true, ColumnDataType = "nvarchar")]
    public string? ResponseResult { get; set; }

    /// <summary>
    /// 执行时长
    /// </summary>
    /// <remarks>
    /// 操作执行时长（毫秒）
    /// </remarks>
    [SugarColumn(ColumnName = "execution_time", ColumnDescription = "执行时长", IsNullable = false, DefaultValue = "0", ColumnDataType = "int")]
    public int ExecutionTime { get; set; }

    /// <summary>
    /// 操作状态
    /// </summary>
    /// <remarks>
    /// 操作执行状态：Success-成功，Failed-失败
    /// </remarks>
    [SugarColumn(ColumnName = "operation_status", ColumnDescription = "操作状态", IsNullable = false, DefaultValue = "0", ColumnDataType = "int")]
    public LeanOperationStatus OperationStatus { get; set; }

    /// <summary>
    /// 错误消息
    /// </summary>
    /// <remarks>
    /// 操作失败时的错误消息
    /// </remarks>
    [SugarColumn(ColumnName = "error_msg", ColumnDescription = "错误消息", Length = 2000, IsNullable = true, ColumnDataType = "nvarchar")]
    public string? ErrorMsg { get; set; }

    /// <summary>
    /// 用户
    /// </summary>
    /// <remarks>
    /// 关联的用户实体
    /// </remarks>
    [Navigate(NavigateType.OneToOne, nameof(UserId))]
    public virtual LeanUser User { get; set; } = default!;
}