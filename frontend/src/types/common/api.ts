// API响应基础接口
export interface ApiResponse<T = any> {
  /** 是否成功 */
  success: boolean

  /** 错误代码 */
  code: LeanErrorCode

  /** 错误消息 */
  message: string

  /** 业务类型 */
  businessType: LeanBusinessType

  /** 跟踪ID */
  traceId: string

  /** 时间戳（毫秒） */
  timestamp: number

  /** 返回数据 */
  data?: T
}

/**
 * 业务操作类型
 */
export enum LeanBusinessType {
  /** 其他 */
  Other = 0,
  /** 查询 */
  Query = 1,
  /** 新增 */
  Create = 2,
  /** 修改 */
  Update = 3,
  /** 删除 */
  Delete = 4,
  /** 导出 */
  Export = 5,
  /** 导入 */
  Import = 6,
  /** 上传 */
  Upload = 7,
  /** 下载 */
  Download = 8,
  /** 打印 */
  Print = 9,
  /** 预览 */
  Preview = 10,
  /** 复制 */
  Copy = 11,
  /** 移动 */
  Move = 12,
  /** 重命名 */
  Rename = 13,
  /** 合并 */
  Merge = 14,
  /** 分割 */
  Split = 15,
  /** 排序 */
  Sort = 16,
  /** 过滤 */
  Filter = 17,
  /** 分组 */
  Group = 18,
  /** 统计 */
  Statistics = 19,
  /** 分析 */
  Analysis = 20,
  /** 比较 */
  Compare = 21,
  /** 验证 */
  Validate = 22,
  /** 审核 */
  Audit = 23,
  /** 审批 */
  Approve = 24,
  /** 驳回 */
  Reject = 25,
  /** 提交 */
  Submit = 26,
  /** 撤回 */
  Revoke = 27,
  /** 发布 */
  Publish = 28,
  /** 撤销发布 */
  Unpublish = 29,
  /** 启用 */
  Enable = 30,
  /** 禁用 */
  Disable = 31,
  /** 锁定 */
  Lock = 32,
  /** 解锁 */
  Unlock = 33,
  /** 授权 */
  Grant = 34,
  /** 取消授权 */
  Ungrant = 35,
  /** 分配 */
  Assign = 36,
  /** 取消分配 */
  Unassign = 37,
  /** 转办 */
  Transfer = 38,
  /** 委托 */
  Delegate = 39,
  /** 签收 */
  Sign = 40,
  /** 退回 */
  Return = 41,
  /** 归档 */
  Archive = 42,
  /** 备份 */
  Backup = 43,
  /** 还原 */
  Restore = 44,
  /** 清理 */
  Clean = 45,
  /** 同步 */
  Sync = 46,
  /** 刷新 */
  Refresh = 47,
  /** 重置 */
  Reset = 48,
  /** 初始化 */
  Initialize = 49,
  /** 配置 */
  Configure = 50,
  /** 安装 */
  Install = 51,
  /** 卸载 */
  Uninstall = 52,
  /** 升级 */
  Upgrade = 53,
  /** 降级 */
  Downgrade = 54,
  /** 启动 */
  Start = 55,
  /** 停止 */
  Stop = 56,
  /** 重启 */
  Restart = 57,
  /** 暂停 */
  Pause = 58,
  /** 恢复 */
  Resume = 59,
  /** 发送 */
  Send = 60,
  /** 接收 */
  Receive = 61,
  /** 转发 */
  Forward = 62,
  /** 回复 */
  Reply = 63,
  /** 登录 */
  Login = 64,
  /** 注销 */
  Logout = 65,
  /** 注册 */
  Register = 66,
  /** 注销账号 */
  Unregister = 67,
  /** 生成 */
  Generate = 68,
  /** 执行 */
  Execute = 69,
  /** 测试 */
  Test = 70
}

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

// 分页请求参数
export interface PageRequest {
  /** 页码（从1开始） */
  pageIndex: number
  
  /** 每页大小（默认20条） */
  pageSize: number
  
  /** 排序字段 */
  orderBy?: string
  
  /** 是否升序（默认true） */
  isAsc?: boolean
  
  /** 其他查询参数 */
  [key: string]: any
}

// 分页响应数据
export interface PageResponse<T> {
  /** 数据列表 */
  items: T[]
  
  /** 总记录数 */
  total: number
  
  /** 是否有下一页 */
  hasNextPage: boolean
  
  /** 是否有上一页 */
  hasPreviousPage: boolean
  
  /** 当前页码 */
  pageIndex: number
  
  /** 每页大小 */
  pageSize: number
  
  /** 总页数 */
  totalPages: number
}

// API错误响应
export interface ApiError {
  /** 错误代码 */
  code: LeanErrorCode
  
  /** 错误消息 */
  message: string
  
  /** 详细信息 */
  details?: string
  
  /** 字段验证错误 */
  validationErrors?: Record<string, string[]>
} 