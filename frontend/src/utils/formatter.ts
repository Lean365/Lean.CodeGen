import dayjs from 'dayjs'
import relativeTime from 'dayjs/plugin/relativeTime'
import 'dayjs/locale/zh-cn'

dayjs.extend(relativeTime)
dayjs.locale('zh-cn')

export const formatDate = (date: string | number | Date, format = 'YYYY-MM-DD') => {
  return date ? dayjs(date).format(format) : ''
}

export const formatDateTime = (date: string | number | Date, format = 'YYYY-MM-DD HH:mm:ss') => {
  return date ? dayjs(date).format(format) : ''
}

export const formatTime = (date: string | number | Date, format = 'HH:mm:ss') => {
  return date ? dayjs(date).format(format) : ''
}

export const formatRelativeTime = (date: string | number | Date) => {
  return date ? dayjs(date).fromNow() : ''
}

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

/**
 * 格式化数字（添加千分位）
 * @param value 要格式化的数值
 * @param precision 小数位数
 */
export const formatNumber = (value: number | string, precision = 0): string => {
  if (value === null || value === undefined || value === '') {
    return ''
  }
  const num = Number(value)
  if (isNaN(num)) {
    return ''
  }
  return num.toLocaleString('zh-CN', {
    minimumFractionDigits: precision,
    maximumFractionDigits: precision
  })
}

/**
 * 格式化百分比
 * @param value 要格式化的数值
 * @param precision 小数位数
 */
export const formatPercent = (value: number | string, precision = 2): string => {
  if (value === null || value === undefined || value === '') {
    return ''
  }
  const num = Number(value)
  if (isNaN(num)) {
    return ''
  }
  return `${(num * 100).toFixed(precision)}%`
}

/**
 * 格式化金额
 * @param value 要格式化的金额
 * @param currency 货币符号
 * @param precision 小数位数
 */
export const formatMoney = (value: number | string, currency = '¥', precision = 2): string => {
  if (value === null || value === undefined || value === '') {
    return ''
  }
  const num = Number(value)
  if (isNaN(num)) {
    return ''
  }
  return `${currency}${formatNumber(num, precision)}`
} 