// 登录日志基础接口
export interface LeanLoginLog {
  id: number
  userId: number
  userName: string
  deviceId: string
  loginIp: string
  clientIp?: string
  loginLocation?: string
  browser?: string
  os?: string
  loginStatus: number
  loginType: number
  errorMsg?: string
  createTime: string
  updateTime?: string
}

// 登录日志查询参数
export interface LeanLoginLogQueryDto {
  pageIndex: number
  pageSize: number
  userId?: number
  userName?: string
  deviceId?: string
  loginStatus?: number
  loginType?: number
  startTime?: string
  endTime?: string
}

// 登录日志详情响应
export interface LeanLoginLogDto extends LeanLoginLog {
  nickName?: string
  deviceName?: string
  deviceType?: string
  deviceStatus?: number
  lastLoginTime?: string
  lastLoginIp?: string
  lastLoginLocation?: string
}

// 登录日志导出参数
export interface LeanLoginLogExportDto {
  userId?: number
  userName?: string
  deviceId?: string
  loginStatus?: number
  loginType?: number
  startTime?: string
  endTime?: string
}

// 登录日志统计
export interface LeanLoginLogStatsDto {
  totalCount: number
  successCount: number
  failureCount: number
  typeStats: {
    loginType: number
    count: number
  }[]
  deviceStats: {
    deviceType: string
    count: number
  }[]
  locationStats: {
    location: string
    count: number
  }[]
  hourlyStats: {
    hour: number
    count: number
  }[]
} 