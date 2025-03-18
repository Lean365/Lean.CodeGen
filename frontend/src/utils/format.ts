/**
 * 格式化工具函数
 */
import dayjs from 'dayjs'
import relativeTime from 'dayjs/plugin/relativeTime'
import 'dayjs/locale/zh-cn'

dayjs.extend(relativeTime)
dayjs.locale('zh-cn')

/**
 * 格式化日期时间
 * @param timestamp 时间戳
 * @param format 格式化模板，默认为 YYYY-MM-DD HH:mm:ss
 * @returns 格式化后的日期时间字符串
 */
export const formatDateTime = (timestamp: number, format = 'YYYY-MM-DD HH:mm:ss'): string => {
  return dayjs(timestamp).format(format)
}

/**
 * 格式化日期
 * @param timestamp 时间戳
 * @param format 格式化模板，默认为 YYYY-MM-DD
 * @returns 格式化后的日期字符串
 */
export const formatDate = (timestamp: number, format = 'YYYY-MM-DD'): string => {
  return dayjs(timestamp).format(format)
}

/**
 * 格式化时间
 * @param timestamp 时间戳
 * @param format 格式化模板，默认为 HH:mm:ss
 * @returns 格式化后的时间字符串
 */
export const formatTime = (timestamp: number, format = 'HH:mm:ss'): string => {
  return dayjs(timestamp).format(format)
}

/**
 * 格式化相对时间
 * @param timestamp 时间戳
 * @returns 相对时间字符串
 */
export const formatRelativeTime = (timestamp: number): string => {
  return dayjs(timestamp).fromNow()
}

/**
 * 格式化持续时间（毫秒）
 * @param duration 持续时间（毫秒）
 * @returns 格式化后的持续时间字符串
 */
export const formatDuration = (duration: number): string => {
  const seconds = Math.floor(duration / 1000)
  const minutes = Math.floor(seconds / 60)
  const hours = Math.floor(minutes / 60)
  
  if (hours > 0) {
    return `${hours}小时${minutes % 60}分钟`
  }
  if (minutes > 0) {
    return `${minutes}分钟${seconds % 60}秒`
  }
  return `${seconds}秒`
} 