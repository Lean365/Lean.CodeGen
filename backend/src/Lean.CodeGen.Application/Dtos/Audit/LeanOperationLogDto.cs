//===================================================
// 项目名: Lean.CodeGen.Application
// 文件名: LeanOperationLogDto.cs
// 功能描述: 操作日志相关DTO
// 创建时间: 2024-03-26
// 作者: Lean
// 版本: 1.0
//===================================================

using System;
using Lean.CodeGen.Common.Excel;
using Lean.CodeGen.Common.Models;

namespace Lean.CodeGen.Application.Dtos.Audit
{
    /// <summary>
    /// 操作日志查询DTO
    /// </summary>
    public class LeanOperationLogDto
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
        /// 模块
        /// </summary>
        public string Module { get; set; } = default!;

        /// <summary>
        /// 操作名称
        /// </summary>
        public string Operation { get; set; } = default!;

        /// <summary>
        /// 请求方法
        /// </summary>
        public string RequestMethod { get; set; } = default!;

        /// <summary>
        /// 请求URL
        /// </summary>
        public string RequestUrl { get; set; } = default!;

        /// <summary>
        /// 请求参数
        /// </summary>
        public string? RequestParam { get; set; }

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
        /// 响应结果
        /// </summary>
        public string? ResponseResult { get; set; }

        /// <summary>
        /// 执行时长（毫秒）
        /// </summary>
        public int ExecutionTime { get; set; }

        /// <summary>
        /// 操作状态
        /// 0-成功
        /// 1-失败
        /// </summary>
        public int OperationStatus { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string? ErrorMsg { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }

    /// <summary>
    /// 操作日志查询条件DTO
    /// </summary>
    public class LeanOperationLogQueryDto : LeanPage
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long? UserId { get; set; }

        /// <summary>
        /// 模块
        /// </summary>
        public string? Module { get; set; }

        /// <summary>
        /// 操作名称
        /// </summary>
        public string? Operation { get; set; }

        /// <summary>
        /// 操作状态
        /// 0-成功
        /// 1-失败
        /// </summary>
        public int? OperationStatus { get; set; }

        /// <summary>
        /// 客户端IP
        /// </summary>
        public string? ClientIp { get; set; }

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
    /// 操作日志导出DTO
    /// </summary>
    public class LeanOperationLogExportDto
    {
        /// <summary>
        /// 用户名称
        /// </summary>
        [LeanExcelColumn("用户名称")]
        public string UserName { get; set; } = default!;

        /// <summary>
        /// 模块
        /// </summary>
        [LeanExcelColumn("模块")]
        public string Module { get; set; } = default!;

        /// <summary>
        /// 操作名称
        /// </summary>
        [LeanExcelColumn("操作名称")]
        public string Operation { get; set; } = default!;

        /// <summary>
        /// 请求方法
        /// </summary>
        [LeanExcelColumn("请求方法")]
        public string RequestMethod { get; set; } = default!;

        /// <summary>
        /// 请求URL
        /// </summary>
        [LeanExcelColumn("请求URL")]
        public string RequestUrl { get; set; } = default!;

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
        /// 执行时长（毫秒）
        /// </summary>
        [LeanExcelColumn("执行时长（毫秒）")]
        public int ExecutionTime { get; set; }

        /// <summary>
        /// 操作状态
        /// </summary>
        [LeanExcelColumn("操作状态")]
        public string OperationStatus { get; set; } = default!;

        /// <summary>
        /// 错误消息
        /// </summary>
        [LeanExcelColumn("错误消息")]
        public string? ErrorMsg { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [LeanExcelColumn("创建时间")]
        public DateTime CreateTime { get; set; }
    }
}