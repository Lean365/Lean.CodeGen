using Newtonsoft.Json;
using Lean.CodeGen.Common.Enums;

namespace Lean.CodeGen.Common.Models;

/// <summary>
/// API 结果
/// </summary>
public class LeanApiResult
{
  /// <summary>
  /// 是否成功
  /// </summary>
  [JsonProperty("success")]
  public bool Success { get; protected set; }

  /// <summary>
  /// 错误代码
  /// </summary>
  [JsonProperty("code")]
  public LeanErrorCode Code { get; protected set; }

  /// <summary>
  /// 错误消息
  /// </summary>
  [JsonProperty("message")]
  public string? Message { get; protected set; }

  /// <summary>
  /// 业务类型
  /// </summary>
  [JsonProperty("businessType")]
  public LeanBusinessType BusinessType { get; protected set; }

  /// <summary>
  /// 跟踪ID
  /// </summary>
  [JsonProperty("traceId")]
  public string? TraceId { get; set; }

  /// <summary>
  /// 时间戳
  /// </summary>
  [JsonProperty("timestamp")]
  public long Timestamp { get; protected set; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

  /// <summary>
  /// 设置成功结果
  /// </summary>
  protected void SetSuccess(LeanBusinessType businessType)
  {
    Success = true;
    Code = LeanErrorCode.Success;
    BusinessType = businessType;
  }

  /// <summary>
  /// 设置错误结果
  /// </summary>
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
  public static LeanApiResult Ok(LeanBusinessType businessType = LeanBusinessType.Other)
  {
    var result = new LeanApiResult();
    result.SetSuccess(businessType);
    return result;
  }

  /// <summary>
  /// 创建错误结果
  /// </summary>
  public static LeanApiResult Error(string message, LeanErrorCode code = LeanErrorCode.SystemError, LeanBusinessType businessType = LeanBusinessType.Other)
  {
    var result = new LeanApiResult();
    result.SetError(message, code, businessType);
    return result;
  }
}

/// <summary>
/// API 结果（泛型版本）
/// </summary>
public class LeanApiResult<T> : LeanApiResult
{
  /// <summary>
  /// 数据
  /// </summary>
  [JsonProperty("data")]
  public T? Data { get; private set; }

  /// <summary>
  /// 创建成功结果
  /// </summary>
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
  public static new LeanApiResult<T> Error(string message, LeanErrorCode code = LeanErrorCode.SystemError, LeanBusinessType businessType = LeanBusinessType.Other)
  {
    var result = new LeanApiResult<T>();
    result.SetError(message, code, businessType);
    return result;
  }
}