//===================================================
// 项目名: Lean.CodeGen.Application
// 文件名: LeanLoginLogDto.cs
// 功能描述: 登录日志相关DTO
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
    /// 登录日志查询DTO
    /// </summary>
    public class LeanLoginLogDto
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
        /// 设备ID
        /// </summary>
        public string DeviceId { get; set; } = default!;

        /// <summary>
        /// 设备名称
        /// </summary>
        public string DeviceName { get; set; } = default!;

        /// <summary>
        /// 登录IP
        /// </summary>
        public string LoginIp { get; set; } = default!;

        /// <summary>
        /// 登录地点
        /// </summary>
        public string? LoginLocation { get; set; }

        /// <summary>
        /// 浏览器
        /// </summary>
        public string? Browser { get; set; }

        /// <summary>
        /// 操作系统
        /// </summary>
        public string? Os { get; set; }

        /// <summary>
        /// 登录状态
        /// 0-成功
        /// 1-失败
        /// </summary>
        public int LoginStatus { get; set; }

        /// <summary>
        /// 登录方式
        /// 0-密码
        /// 1-验证码
        /// 2-令牌
        /// 3-其他
        /// </summary>
        public int LoginType { get; set; }

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
    /// 登录日志查询条件DTO
    /// </summary>
    public class LeanLoginLogQueryDto : LeanPage
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long? UserId { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// 设备ID
        /// </summary>
        public string? DeviceId { get; set; }

        /// <summary>
        /// 客户端IP
        /// </summary>
        public string? ClientIp { get; set; }

        /// <summary>
        /// 登录状态
        /// 0-成功
        /// 1-失败
        /// </summary>
        public int? LoginStatus { get; set; }

        /// <summary>
        /// 登录类型
        /// 0-密码
        /// 1-验证码
        /// 2-令牌
        /// 3-其他
        /// </summary>
        public int? LoginType { get; set; }

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
    /// 登录日志导出DTO
    /// </summary>
    public class LeanLoginLogExportDto
    {
        /// <summary>
        /// 用户名称
        /// </summary>
        [LeanExcelColumn("用户名称")]
        public string UserName { get; set; } = default!;

        /// <summary>
        /// 设备名称
        /// </summary>
        [LeanExcelColumn("设备名称")]
        public string? DeviceName { get; set; }

        /// <summary>
        /// 登录IP
        /// </summary>
        [LeanExcelColumn("登录IP")]
        public string LoginIp { get; set; } = default!;

        /// <summary>
        /// 登录地点
        /// </summary>
        [LeanExcelColumn("登录地点")]
        public string? LoginLocation { get; set; }

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
        /// 登录状态
        /// </summary>
        [LeanExcelColumn("登录状态")]
        public string LoginStatus { get; set; } = default!;

        /// <summary>
        /// 登录类型
        /// </summary>
        [LeanExcelColumn("登录类型")]
        public string LoginType { get; set; } = default!;

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