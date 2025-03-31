// 审计日志基础接口
export interface LeanAuditLog {
  id: number
  userId: number
  entityType: string
  entityId: string
  operationType: number
  description?: string
  requestIp: string
  requestLocation?: string
  browser?: string
  os?: string
  createTime: string
  updateTime?: string
}

// 审计日志查询参数
export interface LeanAuditLogQueryDto {
  pageIndex: number
  pageSize: number
  userId?: number
  entityType?: string
  operationType?: number
  startTime?: string
  endTime?: string
}

// 审计日志详情响应
export interface LeanAuditLogDto extends LeanAuditLog {
  userName?: string
  nickName?: string
}

// 审计日志导出参数
export interface LeanAuditLogExportDto {
  userId?: number
  entityType?: string
  operationType?: number
  startTime?: string
  endTime?: string
}

// 审计日志统计
export interface LeanAuditLogStatsDto {
  totalCount: number
  createCount: number
  updateCount: number
  deleteCount: number
  otherCount: number
  entityStats: {
    entityType: string
    count: number
  }[]
  userStats: {
    userId: number
    userName: string
    count: number
  }[]
  hourlyStats: {
    hour: number
    count: number
  }[]
}