//===================================================
// 项目名: Lean.CodeGen.Application
// 文件名: LeanExceptionLogDto.cs
// 功能描述: 异常日志相关DTO
// 创建时间: 2024-03-26
// 作者: Lean
// 版本: 1.0
//===================================================

using System;
using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Common.Models;

namespace Lean.CodeGen.Application.Dtos.Audit
{
    /// <summary>
    /// 异常日志查询DTO
    /// </summary>
    public class LeanExceptionLogDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public long? UserId { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// 应用名称
        /// </summary>
        public string AppName { get; set; } = default!;

        /// <summary>
        /// 环境名称
        /// </summary>
        public string Environment { get; set; } = default!;

        /// <summary>
        /// 异常类型
        /// </summary>
        public string ExceptionType { get; set; } = default!;

        /// <summary>
        /// 异常消息
        /// </summary>
        public string ExceptionMessage { get; set; } = default!;

        /// <summary>
        /// 异常堆栈
        /// </summary>
        public string? StackTrace { get; set; }

        /// <summary>
        /// 异常源
        /// </summary>
        public string? ExceptionSource { get; set; }

        /// <summary>
        /// 请求URL
        /// </summary>
        public string? RequestUrl { get; set; }

        /// <summary>
        /// 请求方法
        /// </summary>
        public string? RequestMethod { get; set; }

        /// <summary>
        /// 请求参数
        /// </summary>
        public string? RequestParams { get; set; }

        /// <summary>
        /// 请求头
        /// </summary>
        public string? RequestHeaders { get; set; }

        /// <summary>
        /// 客户端IP
        /// </summary>
        public string? ClientIp { get; set; }

        /// <summary>
        /// 客户端浏览器
        /// </summary>
        public string? Browser { get; set; }

        /// <summary>
        /// 操作系统
        /// </summary>
        public string? Os { get; set; }

        /// <summary>
        /// 异常级别
        /// 0-调试
        /// 1-信息
        /// 2-警告
        /// 3-错误
        /// 4-致命
        /// </summary>
        public int LogLevel { get; set; }

        /// <summary>
        /// 处理状态
        /// 0-未处理
        /// 1-已处理
        /// </summary>
        public int HandleStatus { get; set; }

        /// <summary>
        /// 处理时间
        /// </summary>
        public DateTime? HandleTime { get; set; }

        /// <summary>
        /// 处理人ID
        /// </summary>
        public long? HandlerId { get; set; }

        /// <summary>
        /// 处理人名称
        /// </summary>
        public string? HandlerName { get; set; }

        /// <summary>
        /// 处理备注
        /// </summary>
        public string? HandleRemark { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }

    /// <summary>
    /// 异常日志查询条件DTO
    /// </summary>
    public class LeanExceptionLogQueryDto : LeanPage
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long? UserId { get; set; }

        /// <summary>
        /// 应用名称
        /// </summary>
        public string? AppName { get; set; }

        /// <summary>
        /// 环境名称
        /// </summary>
        public string? Environment { get; set; }

        /// <summary>
        /// 异常类型
        /// </summary>
        public string? ExceptionType { get; set; }

        /// <summary>
        /// 异常级别
        /// 0-调试
        /// 1-信息
        /// 2-警告
        /// 3-错误
        /// 4-致命
        /// </summary>
        public int? LogLevel { get; set; }

        /// <summary>
        /// 处理状态
        /// 0-未处理
        /// 1-已处理
        /// </summary>
        public int? HandleStatus { get; set; }

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
    /// 异常日志处理DTO
    /// </summary>
    public class LeanExceptionLogHandleDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 处理状态
        /// 0-未处理
        /// 1-已处理
        /// </summary>
        public int HandleStatus { get; set; }

        /// <summary>
        /// 处理人ID
        /// </summary>
        public long HandlerId { get; set; }

        /// <summary>
        /// 处理人名称
        /// </summary>
        public string HandlerName { get; set; } = default!;

        /// <summary>
        /// 处理备注
        /// </summary>
        public string HandleRemark { get; set; } = default!;
    }

    /// <summary>
    /// 异常日志导出DTO
    /// </summary>
    public class LeanExceptionLogExportDto
    {
        /// <summary>
        /// 用户名称
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// 应用名称
        /// </summary>
        public string AppName { get; set; } = default!;

        /// <summary>
        /// 环境名称
        /// </summary>
        public string Environment { get; set; } = default!;

        /// <summary>
        /// 异常类型
        /// </summary>
        public string ExceptionType { get; set; } = default!;

        /// <summary>
        /// 异常消息
        /// </summary>
        public string ExceptionMessage { get; set; } = default!;

        /// <summary>
        /// 异常源
        /// </summary>
        public string? ExceptionSource { get; set; }

        /// <summary>
        /// 请求URL
        /// </summary>
        public string? RequestUrl { get; set; }

        /// <summary>
        /// 客户端IP
        /// </summary>
        public string? ClientIp { get; set; }

        /// <summary>
        /// 异常级别
        /// 0-调试
        /// 1-信息
        /// 2-警告
        /// 3-错误
        /// 4-致命
        /// </summary>
        public string LogLevel { get; set; } = default!;

        /// <summary>
        /// 处理状态
        /// 0-未处理
        /// 1-已处理
        /// </summary>
        public string HandleStatus { get; set; } = default!;

        /// <summary>
        /// 处理时间
        /// </summary>
        public DateTime? HandleTime { get; set; }

        /// <summary>
        /// 处理人名称
        /// </summary>
        public string? HandlerName { get; set; }

        /// <summary>
        /// 处理备注
        /// </summary>
        public string? HandleRemark { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}