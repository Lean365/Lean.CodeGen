/**
 * 速率限制工具
 */
import i18n from '@/locales'
import { formatDateTime } from './formatter'

const { t } = i18n.global

/** 接口配置类型 */
interface EndpointConfig {
  /** 时间窗口（毫秒） */
  window: number
  /** 限制次数 */
  limit: number
}

/** 速率限制配置 */
export const RATE_LIMIT_CONFIG = {
  /** 默认时间窗口（毫秒） */
  defaultWindow: 60 * 1000,
  /** 默认请求限制次数 */
  defaultLimit: 60,
  /** 特定接口限制配置 */
  endpoints: {
    '/api/LeanAuth/login': {
      window: 60 * 1000,  // 1分钟
      limit: 5  // 最多5次登录尝试
    },
    '/api/LeanAuth/captcha/slider': {
      window: 30 * 1000,  // 30秒
      limit: 3  // 最多3次验证码请求
    }
  } as Record<string, EndpointConfig>
}

/** 请求记录类型 */
interface RequestRecord {
  /** 请求时间戳列表 */
  timestamps: number[]
  /** 时间窗口（毫秒） */
  window: number
  /** 限制次数 */
  limit: number
}

/** 请求记录存储 */
const requestRecords = new Map<string, RequestRecord>()

/**
 * 清理过期的请求记录
 * @param endpoint 接口路径
 * @param record 请求记录
 */
const cleanExpiredRecords = (endpoint: string, record: RequestRecord): void => {
  const now = Date.now()
  record.timestamps = record.timestamps.filter(
    timestamp => now - timestamp < record.window
  )
  
  if (record.timestamps.length === 0) {
    requestRecords.delete(endpoint)
  }
}

/**
 * 检查请求是否超出限制
 * @param endpoint 接口路径
 * @returns 是否允许请求
 */
export const checkRateLimit = (endpoint: string): boolean => {
  const now = Date.now()
  
  // 获取或创建请求记录
  let record = requestRecords.get(endpoint)
  if (!record) {
    const config = RATE_LIMIT_CONFIG.endpoints[endpoint] || {
      window: RATE_LIMIT_CONFIG.defaultWindow,
      limit: RATE_LIMIT_CONFIG.defaultLimit
    }
    record = {
      timestamps: [],
      window: config.window,
      limit: config.limit
    }
    requestRecords.set(endpoint, record)
  }
  
  // 清理过期记录
  cleanExpiredRecords(endpoint, record)
  
  // 检查是否超出限制
  if (record.timestamps.length >= record.limit) {
    console.warn(t('security.rateLimit.exceeded'))
    return false
  }
  
  // 记录新的请求
  record.timestamps.push(now)
  return true
}

/**
 * 获取剩余可用请求次数
 * @param endpoint 接口路径
 * @returns 剩余次数
 */
export const getRemainingRequests = (endpoint: string): number => {
  const record = requestRecords.get(endpoint)
  if (!record) {
    const config = RATE_LIMIT_CONFIG.endpoints[endpoint] || {
      limit: RATE_LIMIT_CONFIG.defaultLimit
    }
    return config.limit
  }
  
  cleanExpiredRecords(endpoint, record)
  const remaining = Math.max(0, record.limit - record.timestamps.length)
  console.info(t('security.rateLimit.remaining', { count: remaining }))
  return remaining
}

/**
 * 获取下次可用时间
 * @param endpoint 接口路径
 * @returns 下次可用时间戳，如果当前可用则返回 0
 */
export const getNextAvailableTime = (endpoint: string): number => {
  const record = requestRecords.get(endpoint)
  if (!record || record.timestamps.length === 0) {
    return 0
  }
  
  cleanExpiredRecords(endpoint, record)
  if (record.timestamps.length < record.limit) {
    return 0
  }
  
  const nextTime = record.timestamps[0] + record.window
  console.info(t('security.rateLimit.nextAvailable', { time: formatDateTime(nextTime) }))
  return nextTime
}

/**
 * 重置特定接口的请求记录
 * @param endpoint 接口路径
 */
export const resetRateLimit = (endpoint: string): void => {
  requestRecords.delete(endpoint)
  console.info(t('security.rateLimit.reset'))
} 