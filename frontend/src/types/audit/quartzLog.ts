// 定时任务日志基础接口
export interface LeanQuartzLog {
  id: number
  taskId: number
  taskName: string
  groupName: string
  startTime: string
  endTime?: string
  elapsedTime?: number
  runResult: number
  errorMessage?: string
  taskData?: string
  serverIp?: string
  serverName?: string
  retryCount: number
  createTime: string
  updateTime?: string
}

// 定时任务日志查询参数
export interface LeanQuartzLogQueryDto {
  pageIndex: number
  pageSize: number
  taskId?: number
  taskName?: string
  groupName?: string
  runResult?: number
  startTime?: string
  endTime?: string
}

// 定时任务日志详情响应
export interface LeanQuartzLogDto extends LeanQuartzLog {
  taskCron?: string
  taskDesc?: string
  taskStatus?: number
  lastRunTime?: string
  nextRunTime?: string
}

// 定时任务日志导出参数
export interface LeanQuartzLogExportDto {
  taskId?: number
  taskName?: string
  groupName?: string
  runResult?: number
  startTime?: string
  endTime?: string
}

// 定时任务日志统计
export interface LeanQuartzLogStatsDto {
  totalCount: number
  successCount: number
  failureCount: number
  taskStats: {
    taskId: number
    taskName: string
    count: number
    successCount: number
    failureCount: number
    avgElapsedTime: number
  }[]
  groupStats: {
    groupName: string
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
} 