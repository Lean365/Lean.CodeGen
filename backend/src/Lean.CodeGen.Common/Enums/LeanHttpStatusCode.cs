using System.ComponentModel;

namespace Lean.CodeGen.Common.Enums;

/// <summary>
/// HTTP状态码枚举
/// </summary>
public enum LeanHttpStatusCode
{
  /// <summary>
  /// 请求成功
  /// </summary>
  [Description("请求成功")]
  OK = 200,

  /// <summary>
  /// 已创建
  /// </summary>
  [Description("已创建")]
  Created = 201,

  /// <summary>
  /// 已接受
  /// </summary>
  [Description("已接受")]
  Accepted = 202,

  /// <summary>
  /// 无内容
  /// </summary>
  [Description("无内容")]
  NoContent = 204,

  /// <summary>
  /// 错误的请求
  /// </summary>
  [Description("错误的请求")]
  BadRequest = 400,

  /// <summary>
  /// 未授权
  /// </summary>
  [Description("未授权")]
  Unauthorized = 401,

  /// <summary>
  /// 禁止访问
  /// </summary>
  [Description("禁止访问")]
  Forbidden = 403,

  /// <summary>
  /// 未找到
  /// </summary>
  [Description("未找到")]
  NotFound = 404,

  /// <summary>
  /// 方法不允许
  /// </summary>
  [Description("方法不允许")]
  MethodNotAllowed = 405,

  /// <summary>
  /// 不可接受
  /// </summary>
  [Description("不可接受")]
  NotAcceptable = 406,

  /// <summary>
  /// 请求超时
  /// </summary>
  [Description("请求超时")]
  RequestTimeout = 408,

  /// <summary>
  /// 冲突
  /// </summary>
  [Description("冲突")]
  Conflict = 409,

  /// <summary>
  /// 已删除
  /// </summary>
  [Description("已删除")]
  Gone = 410,

  /// <summary>
  /// 不支持的媒体类型
  /// </summary>
  [Description("不支持的媒体类型")]
  UnsupportedMediaType = 415,

  /// <summary>
  /// 请求实体过大
  /// </summary>
  [Description("请求实体过大")]
  RequestEntityTooLarge = 413,

  /// <summary>
  /// 请求URI过长
  /// </summary>
  [Description("请求URI过长")]
  RequestURITooLong = 414,

  /// <summary>
  /// 过多请求
  /// </summary>
  [Description("过多请求")]
  TooManyRequests = 429,

  /// <summary>
  /// 服务器错误
  /// </summary>
  [Description("服务器错误")]
  InternalServerError = 500,

  /// <summary>
  /// 服务不可用
  /// </summary>
  [Description("服务不可用")]
  ServiceUnavailable = 503,

  /// <summary>
  /// 网关超时
  /// </summary>
  [Description("网关超时")]
  GatewayTimeout = 504
}