using Newtonsoft.Json;
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
  /// <remarks>
  /// true 表示操作成功，false 表示操作失败
  /// </remarks>
  [JsonProperty("success")]
  public bool Success { get; protected set; }

  /// <summary>
  /// 错误代码
  /// </summary>
  /// <remarks>
  /// 用于标识具体的错误类型，便于前端进行相应处理
  /// </remarks>
  [JsonProperty("code")]
  public LeanErrorCode Code { get; protected set; }

  /// <summary>
  /// 错误消息
  /// </summary>
  /// <remarks>
  /// 当操作失败时，提供具体的错误描述信息
  /// </remarks>
  [JsonProperty("message")]
  public string? Message { get; protected set; }

  /// <summary>
  /// 业务类型
  /// </summary>
  /// <remarks>
  /// 标识当前操作的业务类型，用于业务分类和统计
  /// 0-其他, 1-新增, 2-修改, 3-删除, 4-授权, 5-导出, 6-导入, 7-强退, 8-生成代码, 9-清空数据
  /// </remarks>
  [JsonProperty("businessType")]
  public LeanBusinessType BusinessType { get; protected set; }

  /// <summary>
  /// 跟踪ID
  /// </summary>
  /// <remarks>
  /// 用于跟踪和关联请求，便于问题排查和日志分析
  /// </remarks>
  [JsonProperty("traceId")]
  public string? TraceId { get; set; }

  /// <summary>
  /// 时间戳
  /// </summary>
  /// <remarks>
  /// 记录响应生成的 UTC 时间戳（毫秒）
  /// </remarks>
  [JsonProperty("timestamp")]
  public long Timestamp { get; protected set; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

  /// <summary>
  /// 设置成功结果
  /// </summary>
  /// <param name="businessType">业务类型</param>
  /// <remarks>
  /// 用于内部设置操作成功的结果状态
  /// </remarks>
  protected void SetSuccess(LeanBusinessType businessType)
  {
    Success = true;
    Code = LeanErrorCode.Success;
    BusinessType = businessType;
  }

  /// <summary>
  /// 设置错误结果
  /// </summary>
  /// <param name="message">错误消息</param>
  /// <param name="code">错误代码</param>
  /// <param name="businessType">业务类型</param>
  /// <remarks>
  /// 用于内部设置操作失败的结果状态
  /// </remarks>
  protected void SetError(string message, LeanErrorCode code, LeanBusinessType businessType)
  {
    Success = false;
    Code = code;
    Message = message;
    BusinessType = businessType;
  }

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
    var result = new LeanApiResult();
    result.SetSuccess(businessType);
    return result;
  }

  /// <summary>
  /// 创建错误结果
  /// </summary>
  /// <param name="message">错误消息</param>
  /// <param name="code">错误代码，默认为系统错误</param>
  /// <param name="businessType">业务类型，默认为其他类型</param>
  /// <returns>错误的 API 结果</returns>
  /// <remarks>
  /// 用于快速创建一个错误的响应结果
  /// </remarks>
  public static LeanApiResult Error(string message, LeanErrorCode code = LeanErrorCode.SystemError, LeanBusinessType businessType = LeanBusinessType.Other)
  {
    var result = new LeanApiResult();
    result.SetError(message, code, businessType);
    return result;
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
  [JsonProperty("data")]
  public T? Data { get; private set; }

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
    var result = new LeanApiResult<T>();
    result.SetSuccess(businessType);
    result.Data = data;
    return result;
  }

  /// <summary>
  /// 创建错误结果
  /// </summary>
  /// <param name="message">错误消息</param>
  /// <param name="code">错误代码，默认为系统错误</param>
  /// <param name="businessType">业务类型，默认为其他类型</param>
  /// <returns>错误的 API 结果</returns>
  /// <remarks>
  /// 用于快速创建一个带有类型信息的错误响应结果
  /// </remarks>
  public static new LeanApiResult<T> Error(string message, LeanErrorCode code = LeanErrorCode.SystemError, LeanBusinessType businessType = LeanBusinessType.Other)
  {
    var result = new LeanApiResult<T>();
    result.SetError(message, code, businessType);
    return result;
  }
}