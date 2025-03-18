using System.ComponentModel;
using Microsoft.AspNetCore.Http;

namespace Lean.CodeGen.Common.Enums;

/// <summary>
/// API错误码
/// </summary>
public enum LeanErrorCode
{
  #region HTTP标准状态码

  #region 1xx 信息响应

  /// <summary>
  /// 继续 (100)
  /// </summary>
  [Description("继续")]
  Status100Continue = StatusCodes.Status100Continue,

  /// <summary>
  /// 切换协议 (101)
  /// </summary>
  [Description("切换协议")]
  Status101SwitchingProtocols = StatusCodes.Status101SwitchingProtocols,

  /// <summary>
  /// 处理中 (102)
  /// </summary>
  [Description("处理中")]
  Status102Processing = StatusCodes.Status102Processing,

  #endregion

  #region 2xx 成功

  /// <summary>
  /// 成功 (200)
  /// </summary>
  [Description("成功")]
  Status200OK = StatusCodes.Status200OK,

  /// <summary>
  /// 已创建 (201)
  /// </summary>
  [Description("已创建")]
  Status201Created = StatusCodes.Status201Created,

  /// <summary>
  /// 已接受 (202)
  /// </summary>
  [Description("已接受")]
  Status202Accepted = StatusCodes.Status202Accepted,

  /// <summary>
  /// 非授权信息 (203)
  /// </summary>
  [Description("非授权信息")]
  Status203NonAuthoritative = StatusCodes.Status203NonAuthoritative,

  /// <summary>
  /// 无内容 (204)
  /// </summary>
  [Description("无内容")]
  Status204NoContent = StatusCodes.Status204NoContent,

  /// <summary>
  /// 重置内容 (205)
  /// </summary>
  [Description("重置内容")]
  Status205ResetContent = StatusCodes.Status205ResetContent,

  /// <summary>
  /// 部分内容 (206)
  /// </summary>
  [Description("部分内容")]
  Status206PartialContent = StatusCodes.Status206PartialContent,

  #endregion

  #region 3xx 重定向

  /// <summary>
  /// 多种选择 (300)
  /// </summary>
  [Description("多种选择")]
  Status300MultipleChoices = StatusCodes.Status300MultipleChoices,

  /// <summary>
  /// 永久移动 (301)
  /// </summary>
  [Description("永久移动")]
  Status301MovedPermanently = StatusCodes.Status301MovedPermanently,

  /// <summary>
  /// 临时移动 (302)
  /// </summary>
  [Description("临时移动")]
  Status302Found = StatusCodes.Status302Found,

  /// <summary>
  /// 查看其他位置 (303)
  /// </summary>
  [Description("查看其他位置")]
  Status303SeeOther = StatusCodes.Status303SeeOther,

  /// <summary>
  /// 未修改 (304)
  /// </summary>
  [Description("未修改")]
  Status304NotModified = StatusCodes.Status304NotModified,

  /// <summary>
  /// 使用代理 (305)
  /// </summary>
  [Description("使用代理")]
  Status305UseProxy = StatusCodes.Status305UseProxy,

  /// <summary>
  /// 临时重定向 (307)
  /// </summary>
  [Description("临时重定向")]
  Status307TemporaryRedirect = StatusCodes.Status307TemporaryRedirect,

  /// <summary>
  /// 永久重定向 (308)
  /// </summary>
  [Description("永久重定向")]
  Status308PermanentRedirect = StatusCodes.Status308PermanentRedirect,

  /// <summary>
  /// 切换代理 (306)
  /// </summary>
  [Description("切换代理")]
  Status306SwitchProxy = StatusCodes.Status306SwitchProxy,

  #endregion

  #region 4xx 客户端错误

  /// <summary>
  /// 错误请求 (400)
  /// </summary>
  [Description("错误请求")]
  Status400BadRequest = StatusCodes.Status400BadRequest,

  /// <summary>
  /// 未授权 (401)
  /// </summary>
  [Description("未授权")]
  Status401Unauthorized = StatusCodes.Status401Unauthorized,

  /// <summary>
  /// 需要支付 (402)
  /// </summary>
  [Description("需要支付")]
  Status402PaymentRequired = StatusCodes.Status402PaymentRequired,

  /// <summary>
  /// 禁止访问 (403)
  /// </summary>
  [Description("禁止访问")]
  Status403Forbidden = StatusCodes.Status403Forbidden,

  /// <summary>
  /// 未找到 (404)
  /// </summary>
  [Description("未找到")]
  Status404NotFound = StatusCodes.Status404NotFound,

  /// <summary>
  /// 方法不允许 (405)
  /// </summary>
  [Description("方法不允许")]
  Status405MethodNotAllowed = StatusCodes.Status405MethodNotAllowed,

  /// <summary>
  /// 不接受 (406)
  /// </summary>
  [Description("不接受")]
  Status406NotAcceptable = StatusCodes.Status406NotAcceptable,

  /// <summary>
  /// 需要代理认证 (407)
  /// </summary>
  [Description("需要代理认证")]
  Status407ProxyAuthenticationRequired = StatusCodes.Status407ProxyAuthenticationRequired,

  /// <summary>
  /// 请求超时 (408)
  /// </summary>
  [Description("请求超时")]
  Status408RequestTimeout = StatusCodes.Status408RequestTimeout,

  /// <summary>
  /// 冲突 (409)
  /// </summary>
  [Description("冲突")]
  Status409Conflict = StatusCodes.Status409Conflict,

  /// <summary>
  /// 已删除 (410)
  /// </summary>
  [Description("已删除")]
  Status410Gone = StatusCodes.Status410Gone,

  /// <summary>
  /// 需要有效长度 (411)
  /// </summary>
  [Description("需要有效长度")]
  Status411LengthRequired = StatusCodes.Status411LengthRequired,

  /// <summary>
  /// 预处理失败 (412)
  /// </summary>
  [Description("预处理失败")]
  Status412PreconditionFailed = StatusCodes.Status412PreconditionFailed,

  /// <summary>
  /// 请求实体太大 (413)
  /// </summary>
  [Description("请求实体太大")]
  Status413RequestEntityTooLarge = StatusCodes.Status413RequestEntityTooLarge,

  /// <summary>
  /// 请求URI太长 (414)
  /// </summary>
  [Description("请求URI太长")]
  Status414RequestUriTooLong = StatusCodes.Status414RequestUriTooLong,

  /// <summary>
  /// 不支持的媒体类型 (415)
  /// </summary>
  [Description("不支持的媒体类型")]
  Status415UnsupportedMediaType = StatusCodes.Status415UnsupportedMediaType,

  /// <summary>
  /// 请求范围不符合要求 (416)
  /// </summary>
  [Description("请求范围不符合要求")]
  Status416RangeNotSatisfiable = StatusCodes.Status416RangeNotSatisfiable,

  /// <summary>
  /// 预期失败 (417)
  /// </summary>
  [Description("预期失败")]
  Status417ExpectationFailed = StatusCodes.Status417ExpectationFailed,

  /// <summary>
  /// 升级需要 (426)
  /// </summary>
  [Description("升级需要")]
  Status426UpgradeRequired = StatusCodes.Status426UpgradeRequired,

  /// <summary>
  /// 前提条件需要 (428)
  /// </summary>
  [Description("前提条件需要")]
  Status428PreconditionRequired = StatusCodes.Status428PreconditionRequired,

  /// <summary>
  /// 请求过多 (429)
  /// </summary>
  [Description("请求过多")]
  Status429TooManyRequests = StatusCodes.Status429TooManyRequests,

  /// <summary>
  /// 请求头字段太大 (431)
  /// </summary>
  [Description("请求头字段太大")]
  Status431RequestHeaderFieldsTooLarge = StatusCodes.Status431RequestHeaderFieldsTooLarge,

  /// <summary>
  /// 不可用于法律原因 (451)
  /// </summary>
  [Description("不可用于法律原因")]
  Status451UnavailableForLegalReasons = StatusCodes.Status451UnavailableForLegalReasons,

  /// <summary>
  /// 已锁定 (423)
  /// </summary>
  [Description("已锁定")]
  Status423Locked = StatusCodes.Status423Locked,

  /// <summary>
  /// 依赖关系失败 (424)
  /// </summary>
  [Description("依赖关系失败")]
  Status424FailedDependency = StatusCodes.Status424FailedDependency,

  /// <summary>
  /// 客户端已关闭请求 (499)
  /// </summary>
  [Description("客户端已关闭请求")]
  Status499ClientClosedRequest = StatusCodes.Status499ClientClosedRequest,

  #endregion

  #region 5xx 服务器错误

  /// <summary>
  /// 服务器内部错误 (500)
  /// </summary>
  [Description("服务器内部错误")]
  Status500InternalServerError = StatusCodes.Status500InternalServerError,

  /// <summary>
  /// 未实现 (501)
  /// </summary>
  [Description("未实现")]
  Status501NotImplemented = StatusCodes.Status501NotImplemented,

  /// <summary>
  /// 错误网关 (502)
  /// </summary>
  [Description("错误网关")]
  Status502BadGateway = StatusCodes.Status502BadGateway,

  /// <summary>
  /// 服务不可用 (503)
  /// </summary>
  [Description("服务不可用")]
  Status503ServiceUnavailable = StatusCodes.Status503ServiceUnavailable,

  /// <summary>
  /// 网关超时 (504)
  /// </summary>
  [Description("网关超时")]
  Status504GatewayTimeout = StatusCodes.Status504GatewayTimeout,

  /// <summary>
  /// HTTP版本不支持 (505)
  /// </summary>
  [Description("HTTP版本不支持")]
  Status505HttpVersionNotSupported = 505,

  /// <summary>
  /// 变体也可以协商 (506)
  /// </summary>
  [Description("变体也可以协商")]
  Status506VariantAlsoNegotiates = StatusCodes.Status506VariantAlsoNegotiates,

  /// <summary>
  /// 存储空间不足 (507)
  /// </summary>
  [Description("存储空间不足")]
  Status507InsufficientStorage = StatusCodes.Status507InsufficientStorage,

  /// <summary>
  /// 检测到无限循环 (508)
  /// </summary>
  [Description("检测到无限循环")]
  Status508LoopDetected = StatusCodes.Status508LoopDetected,

  /// <summary>
  /// 带宽限制超过 (509)
  /// </summary>
  [Description("带宽限制超过")]
  Status509BandwidthLimitExceeded = 509,

  /// <summary>
  /// 未扩展 (510)
  /// </summary>
  [Description("未扩展")]
  Status510NotExtended = StatusCodes.Status510NotExtended,

  /// <summary>
  /// 需要网络认证 (511)
  /// </summary>
  [Description("需要网络认证")]
  Status511NetworkAuthenticationRequired = StatusCodes.Status511NetworkAuthenticationRequired,

  #endregion

  #endregion

  #region 业务错误码 (6xxxxx)

  /// <summary>
  /// 未知错误 (600000)
  /// </summary>
  [Description("未知错误")]
  UnknownError = 600000,

  /// <summary>
  /// 系统错误 (600001)
  /// </summary>
  [Description("系统错误")]
  SystemError = 600001,

  /// <summary>
  /// 业务错误 (600002)
  /// </summary>
  [Description("业务错误")]
  BusinessError = 600002,

  /// <summary>
  /// 验证错误 (600003)
  /// </summary>
  [Description("验证错误")]
  ValidationError = 600003,

  /// <summary>
  /// 参数验证失败 (600004)
  /// </summary>
  [Description("参数验证失败")]
  ValidationFailed = 600004,

  /// <summary>
  /// 数据不存在 (600005)
  /// </summary>
  [Description("数据不存在")]
  DataNotFound = 600005,

  /// <summary>
  /// 数据不存在错误 (600006)
  /// </summary>
  [Description("数据不存在错误")]
  DataNotFoundError = 600006,

  /// <summary>
  /// 数据已存在 (600007)
  /// </summary>
  [Description("数据已存在")]
  DataAlreadyExists = 600007,

  /// <summary>
  /// 数据状态错误 (600008)
  /// </summary>
  [Description("数据状态错误")]
  InvalidDataStatus = 600008,

  /// <summary>
  /// 操作失败 (600009)
  /// </summary>
  [Description("操作失败")]
  OperationFailed = 600009,

  /// <summary>
  /// 操作被禁止 (600010)
  /// </summary>
  [Description("操作被禁止")]
  OperationForbidden = 600010,

  /// <summary>
  /// 未找到 (600011)
  /// </summary>
  [Description("未找到")]
  NotFound = 600011,

  /// <summary>
  /// 重复错误 (600012)
  /// </summary>
  [Description("重复错误")]
  DuplicateError = 600012,

  /// <summary>
  /// 并发错误 (600013)
  /// </summary>
  [Description("并发错误")]
  ConcurrencyError = 600013,

  #endregion

  #region 身份认证错误码 (61xxxx)

  /// <summary>
  /// 用户名或密码错误 (610001)
  /// </summary>
  [Description("用户名或密码错误")]
  InvalidCredentials = 610001,

  /// <summary>
  /// 令牌已过期 (610002)
  /// </summary>
  [Description("令牌已过期")]
  TokenExpired = 610002,

  /// <summary>
  /// 无效的令牌 (610003)
  /// </summary>
  [Description("无效的令牌")]
  InvalidToken = 610003,

  /// <summary>
  /// 验证码错误 (610004)
  /// </summary>
  [Description("验证码错误")]
  InvalidCaptcha = 610004,

  #endregion

  #region 权限错误码 (62xxxx)

  /// <summary>
  /// 无权限访问 (620001)
  /// </summary>
  [Description("无权限访问")]
  NoPermission = 620001,

  /// <summary>
  /// 角色未分配 (620002)
  /// </summary>
  [Description("角色未分配")]
  NoRole = 620002,

  #endregion

  #region 业务操作错误码 (63xxxx)

  /// <summary>
  /// 数据被引用 (630001)
  /// </summary>
  [Description("数据被引用")]
  DataReferenced = 630001,

  /// <summary>
  /// 状态不允许操作 (630002)
  /// </summary>
  [Description("状态不允许操作")]
  StatusNotAllowed = 630002

  #endregion
}