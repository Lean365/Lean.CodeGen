using System.ComponentModel;

namespace Lean.CodeGen.Common.Enums;

/// <summary>
/// 错误代码
/// </summary>
public enum LeanErrorCode
{
  /// <summary>
  /// 成功
  /// </summary>
  [Description("成功")]
  Success = 0,

  /// <summary>
  /// 参数错误
  /// </summary>
  [Description("参数错误")]
  InvalidParameter = 400,

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
  /// 资源不存在
  /// </summary>
  [Description("资源不存在")]
  NotFound = 404,

  /// <summary>
  /// 系统错误
  /// </summary>
  [Description("系统错误")]
  SystemError = 500,

  /// <summary>
  /// 服务不可用
  /// </summary>
  [Description("服务不可用")]
  ServiceUnavailable = 503,

  /// <summary>
  /// 业务错误
  /// </summary>
  [Description("业务错误")]
  BusinessError = 600,

  /// <summary>
  /// 数据验证错误
  /// </summary>
  [Description("数据验证错误")]
  ValidationError = 601,

  /// <summary>
  /// 数据重复错误
  /// </summary>
  [Description("数据重复错误")]
  DuplicateError = 602,

  /// <summary>
  /// 数据不存在错误
  /// </summary>
  [Description("数据不存在错误")]
  DataNotFoundError = 603,

  /// <summary>
  /// 数据关联错误
  /// </summary>
  [Description("数据关联错误")]
  DataRelationError = 604,

  /// <summary>
  /// 数据状态错误
  /// </summary>
  [Description("数据状态错误")]
  DataStatusError = 605,

  /// <summary>
  /// 操作被禁止
  /// </summary>
  [Description("操作被禁止")]
  OperationForbidden = 606,

  /// <summary>
  /// 操作失败
  /// </summary>
  [Description("操作失败")]
  OperationFailed = 607
}