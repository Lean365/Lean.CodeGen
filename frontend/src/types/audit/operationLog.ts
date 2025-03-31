// 操作日志基础接口
export interface LeanOperationLog {
  id: number
  userId: number
  module: string
  operation: string
  requestMethod: string
  requestUrl: string
  requestParam?: string
  requestIp: string
  clientIp?: string
  requestLocation?: string
  browser?: string
  os?: string
  responseResult?: string
  executionTime: number
  operationStatus: number
  errorMsg?: string
  createTime: string
  updateTime?: string
}

// 操作日志查询参数
export interface LeanOperationLogQueryDto {
  pageIndex: number
  pageSize: number
  userId?: number
  module?: string
  operation?: string
  operationStatus?: number
  startTime?: string
  endTime?: string
}

// 操作日志详情响应
export interface LeanOperationLogDto extends LeanOperationLog {
  userName?: string
  nickName?: string
}

// 操作日志导出参数
export interface LeanOperationLogExportDto {
  userId?: number
  module?: string
  operation?: string
  operationStatus?: number
  startTime?: string
  endTime?: string
}

// 操作日志统计
export interface LeanOperationLogStatsDto {
  totalCount: number
  successCount: number
  failureCount: number
  moduleStats: {
    module: string
    count: number
    successCount: number
    failureCount: number
  }[]
  userStats: {
    userId: number
    userName: string
    count: number
    successCount: number
    failureCount: number
  }[]
  hourlyStats: {
    hour: number
    count: number
    successCount: number
    failureCount: number
  }[]
  avgExecutionTime: number
  maxExecutionTime: number
  minExecutionTime: number
} 