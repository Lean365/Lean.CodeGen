using System;
using System.ComponentModel;
using Lean.CodeGen.Common.Enums;

namespace Lean.CodeGen.Common.Models;

/// <summary>
/// API 统一返回结果
/// </summary>
/// <remarks>
/// 提供统一的 API 响应格式，包括：
/// 1. 操作是否成功
/// 2. 错误代码和消息
/// 3. 业务类型标识
/// 4. 跟踪信息
/// </remarks>
public class LeanApiResult
{
    /// <summary>
    /// 是否成功
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// 错误代码
    /// </summary>
    public LeanErrorCode Code { get; set; }

    /// <summary>
    /// 错误消息
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// 业务类型
    /// </summary>
    /// <remarks>
    /// 标识当前操作的业务类型，用于业务分类和统计
    /// 0-其他, 1-新增, 2-修改, 3-删除, 4-授权, 5-导出, 6-导入, 7-强退, 8-生成代码, 9-清空数据
    /// </remarks>
    public LeanBusinessType BusinessType { get; set; }

    /// <summary>
    /// 跟踪ID
    /// </summary>
    /// <remarks>
    /// 用于跟踪和关联请求，便于问题排查和日志分析
    /// </remarks>
    public string? TraceId { get; set; }

    /// <summary>
    /// 时间戳
    /// </summary>
    /// <remarks>
    /// 记录响应生成的 UTC 时间戳（毫秒）
    /// </remarks>
    public long Timestamp { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

    /// <summary>
    /// 创建成功结果
    /// </summary>
    /// <param name="businessType">业务类型，默认为其他类型</param>
    /// <returns>成功的 API 结果</returns>
    /// <remarks>
    /// 用于快速创建一个成功的响应结果
    /// </remarks>
    public static LeanApiResult Ok(LeanBusinessType businessType = LeanBusinessType.Other)
    {
        return new LeanApiResult { Success = true, BusinessType = businessType };
    }

    /// <summary>
    /// 创建错误结果
    /// </summary>
    /// <param name="message">错误消息</param>
    /// <param name="code">错误代码</param>
    /// <param name="businessType">业务类型，默认为其他类型</param>
    /// <returns>错误的 API 结果</returns>
    /// <remarks>
    /// 用于快速创建一个带有类型信息的错误响应结果
    /// </remarks>
    public static LeanApiResult Error(string message, LeanErrorCode code = LeanErrorCode.Status400BadRequest, LeanBusinessType businessType = LeanBusinessType.Other)
    {
        return new LeanApiResult
        {
            Success = false,
            Message = message,
            Code = code,
            BusinessType = businessType
        };
    }
}

/// <summary>
/// API 统一返回结果（泛型版本）
/// </summary>
/// <typeparam name="T">数据类型</typeparam>
/// <remarks>
/// 继承自基础返回结果，增加了具体的数据返回功能：
/// 1. 支持返回任意类型的数据
/// 2. 保持与基础返回结果相同的错误处理机制
/// </remarks>
public class LeanApiResult<T> : LeanApiResult
{
    /// <summary>
    /// 返回数据
    /// </summary>
    /// <remarks>
    /// 操作成功时返回的具体数据内容
    /// </remarks>
    public T? Data { get; set; }

    /// <summary>
    /// 创建成功结果
    /// </summary>
    /// <param name="data">要返回的数据</param>
    /// <param name="businessType">业务类型，默认为其他类型</param>
    /// <returns>包含数据的成功 API 结果</returns>
    /// <remarks>
    /// 用于快速创建一个包含数据的成功响应结果
    /// </remarks>
    public static LeanApiResult<T> Ok(T data, LeanBusinessType businessType = LeanBusinessType.Other)
    {
        return new LeanApiResult<T> { Success = true, Data = data, BusinessType = businessType };
    }

    /// <summary>
    /// 创建错误结果
    /// </summary>
    /// <param name="message">错误消息</param>
    /// <param name="code">错误代码</param>
    /// <param name="businessType">业务类型，默认为其他类型</param>
    /// <returns>错误的 API 结果</returns>
    /// <remarks>
    /// 用于快速创建一个带有类型信息的错误响应结果
    /// </remarks>
    public new static LeanApiResult<T> Error(string message, LeanErrorCode code = LeanErrorCode.Status400BadRequest, LeanBusinessType businessType = LeanBusinessType.Other)
    {
        return new LeanApiResult<T>
        {
            Success = false,
            Message = message,
            Code = code,
            BusinessType = businessType
        };
    }
}