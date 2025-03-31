// 异常日志基础接口
export interface LeanExceptionLog {
  id: number
  userId?: number
  appName: string
  environment: string
  exceptionType: string
  exceptionMessage: string
  stackTrace?: string
  exceptionSource?: string
  requestUrl?: string
  requestMethod?: string
  requestParams?: string
  requestHeaders?: string
  clientIp?: string
  browser?: string
  os?: string
  logLevel: number
  handleStatus: number
  handleTime?: string
  handlerId?: number
  handlerName?: string
  handleRemark?: string
  createTime: string
  updateTime?: string
}

// 异常日志查询参数
export interface LeanExceptionLogQueryDto {
  pageIndex: number
  pageSize: number
  userId?: number
  appName?: string
  environment?: string
  logLevel?: number
  handleStatus?: number
  startTime?: string
  endTime?: string
}

// 异常日志详情响应
export interface LeanExceptionLogDto extends LeanExceptionLog {
  userName?: string
  nickName?: string
}

// 异常日志处理参数
export interface LeanExceptionLogHandleDto {
  ids: number[]
  handleRemark: string
}

// 异常日志导出参数
export interface LeanExceptionLogExportDto {
  userId?: number
  appName?: string
  environment?: string
  logLevel?: number
  handleStatus?: number
  startTime?: string
  endTime?: string
}

// 异常日志统计
export interface LeanExceptionLogStatsDto {
  totalCount: number
  handledCount: number
  unhandledCount: number
  levelStats: {
    logLevel: number
    count: number
  }[]
  appStats: {
    appName: string
    count: number
  }[]
  envStats: {
    environment: string
    count: number
  }[]
  hourlyStats: {
    hour: number
    count: number
  }[]
} 