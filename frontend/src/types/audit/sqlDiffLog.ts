// SQL差异日志基础接口
export interface LeanSqlDiffLog {
  id: number
  auditLogId: number
  tableName: string
  tableDescription?: string
  primaryKeyName: string
  primaryKeyValue: string
  beforeData?: string
  afterData?: string
  diffType: number
  sqlStatement?: string
  createTime: string
  updateTime?: string
}

// SQL差异日志查询参数
export interface LeanSqlDiffLogQueryDto {
  pageIndex: number
  pageSize: number
  auditLogId?: number
  tableName?: string
  diffType?: number
  startTime?: string
  endTime?: string
}

// SQL差异日志详情响应
export interface LeanSqlDiffLogDto extends LeanSqlDiffLog {
  auditDescription?: string
  auditOperationType?: number
  auditUserName?: string
}

// SQL差异日志导出参数
export interface LeanSqlDiffLogExportDto {
  auditLogId?: number
  tableName?: string
  diffType?: number
  startTime?: string
  endTime?: string
}

// SQL差异日志统计
export interface LeanSqlDiffLogStatsDto {
  totalCount: number
  createCount: number
  updateCount: number
  deleteCount: number
  tableStats: {
    tableName: string
    count: number
    createCount: number
    updateCount: number
    deleteCount: number
  }[]
  hourlyStats: {
    hour: number
    count: number
    createCount: number
    updateCount: number
    deleteCount: number
  }[]
} 