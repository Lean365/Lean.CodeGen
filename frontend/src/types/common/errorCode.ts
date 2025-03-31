/**
 * API错误码
 */
export enum LeanErrorCode {
  // HTTP标准状态码
  // 1xx 信息响应
  Status100Continue = 100, // 继续
  Status101SwitchingProtocols = 101, // 切换协议
  Status102Processing = 102, // 处理中

  // 2xx 成功
  Status200OK = 200, // 成功
  Status201Created = 201, // 已创建
  Status202Accepted = 202, // 已接受
  Status203NonAuthoritative = 203, // 非授权信息
  Status204NoContent = 204, // 无内容
  Status205ResetContent = 205, // 重置内容
  Status206PartialContent = 206, // 部分内容

  // 3xx 重定向
  Status300MultipleChoices = 300, // 多种选择
  Status301MovedPermanently = 301, // 永久移动
  Status302Found = 302, // 临时移动
  Status303SeeOther = 303, // 查看其他位置
  Status304NotModified = 304, // 未修改
  Status305UseProxy = 305, // 使用代理
  Status306SwitchProxy = 306, // 切换代理
  Status307TemporaryRedirect = 307, // 临时重定向
  Status308PermanentRedirect = 308, // 永久重定向

  // 4xx 客户端错误
  Status400BadRequest = 400, // 错误请求
  Status401Unauthorized = 401, // 未授权
  Status402PaymentRequired = 402, // 需要支付
  Status403Forbidden = 403, // 禁止访问
  Status404NotFound = 404, // 未找到
  Status405MethodNotAllowed = 405, // 方法不允许
  Status406NotAcceptable = 406, // 不接受
  Status407ProxyAuthenticationRequired = 407, // 需要代理认证
  Status408RequestTimeout = 408, // 请求超时
  Status409Conflict = 409, // 冲突
  Status410Gone = 410, // 已删除
  Status411LengthRequired = 411, // 需要有效长度
  Status412PreconditionFailed = 412, // 预处理失败
  Status413RequestEntityTooLarge = 413, // 请求实体太大
  Status414RequestUriTooLong = 414, // 请求URI太长
  Status415UnsupportedMediaType = 415, // 不支持的媒体类型
  Status416RangeNotSatisfiable = 416, // 请求范围不符合要求
  Status417ExpectationFailed = 417, // 预期失败
  Status423Locked = 423, // 已锁定
  Status424FailedDependency = 424, // 依赖关系失败
  Status426UpgradeRequired = 426, // 升级需要
  Status428PreconditionRequired = 428, // 前提条件需要
  Status429TooManyRequests = 429, // 请求过多
  Status431RequestHeaderFieldsTooLarge = 431, // 请求头字段太大
  Status451UnavailableForLegalReasons = 451, // 不可用于法律原因
  Status499ClientClosedRequest = 499, // 客户端已关闭请求

  // 5xx 服务器错误
  Status500InternalServerError = 500, // 服务器内部错误
  Status501NotImplemented = 501, // 未实现
  Status502BadGateway = 502, // 错误网关
  Status503ServiceUnavailable = 503, // 服务不可用
  Status504GatewayTimeout = 504, // 网关超时
  Status505HttpVersionNotSupported = 505, // HTTP版本不支持
  Status506VariantAlsoNegotiates = 506, // 变体也可以协商
  Status507InsufficientStorage = 507, // 存储空间不足
  Status508LoopDetected = 508, // 检测到无限循环
  Status509BandwidthLimitExceeded = 509, // 带宽限制超过
  Status510NotExtended = 510, // 未扩展
  Status511NetworkAuthenticationRequired = 511, // 需要网络认证

  // 业务错误码 (6xxxxx)
  UnknownError = 600000, // 未知错误
  SystemError = 600001, // 系统错误
  BusinessError = 600002, // 业务错误
  ValidationError = 600003, // 验证错误
  ValidationFailed = 600004, // 参数验证失败
  DataNotFound = 600005, // 数据不存在
  DataNotFoundError = 600006, // 数据不存在错误
  DataAlreadyExists = 600007, // 数据已存在
  InvalidDataStatus = 600008, // 数据状态错误
  OperationFailed = 600009, // 操作失败
  OperationForbidden = 600010, // 操作被禁止
  NotFound = 600011, // 未找到
  DuplicateError = 600012, // 重复错误
  ConcurrencyError = 600013, // 并发错误

  // 身份认证错误码 (61xxxx)
  InvalidCredentials = 610001, // 用户名或密码错误
  TokenExpired = 610002, // 令牌已过期
  InvalidToken = 610003, // 无效的令牌
  InvalidCaptcha = 610004, // 验证码错误

  // 权限错误码 (62xxxx)
  NoPermission = 620001, // 无权限访问
  NoRole = 620002, // 角色未分配

  // 业务操作错误码 (63xxxx)
  DataReferenced = 630001, // 数据被引用
  StatusNotAllowed = 630002, // 状态不允许操作
}
