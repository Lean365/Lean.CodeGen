/**
 * 安全工具函数
 */
import i18n from '@/locales'

const { t } = i18n.global

/** SQL 注入防护配置 */
export const SQL_INJECTION_CONFIG = {
  /** 阻止的关键字（与后端配置匹配） */
  blockedKeywords: [
    'select',
    'insert',
    'update',
    'delete',
    'drop',
    'truncate',
    'exec',
    'execute'
  ],
  /** 高风险字符 */
  riskChars: [';', '\'', '--']
}

/**
 * 检查输入是否包含 SQL 注入风险
 * @param input 输入字符串
 * @returns 是否包含风险
 */
export const hasSqlInjectionRisk = (input: string): boolean => {
  if (!input) return false
  
  // 检查高风险字符
  if (SQL_INJECTION_CONFIG.riskChars.some(char => input.includes(char))) {
    return true
  }
  
  // 检查关键字
  const lowerInput = input.toLowerCase()
  return SQL_INJECTION_CONFIG.blockedKeywords.some(keyword => 
    new RegExp(`\\b${keyword}\\b`, 'i').test(lowerInput)
  )
}

/**
 * 清理可能存在 SQL 注入风险的输入
 * @param input 输入字符串
 * @returns 清理后的字符串，如果不安全返回空字符串
 */
export const cleanSqlInjectionRisk = (input: string): string => {
  if (!input) return ''
  
  // 检查高风险字符
  if (SQL_INJECTION_CONFIG.riskChars.some(char => input.includes(char))) {
    console.warn(t('security.sql.riskCharsDetected'), input)
    return ''
  }
  
  // 清理 SQL 关键字
  let cleaned = input
  SQL_INJECTION_CONFIG.blockedKeywords.forEach(keyword => {
    cleaned = cleaned.replace(new RegExp(`\\b${keyword}\\b`, 'gi'), '')
  })
  
  // 检查清理后的长度变化
  if (Math.abs(cleaned.length - input.length) > input.length * 0.3) {
    console.warn(t('security.sql.possibleRisk'), input)
    return ''
  }
  
  return cleaned
}

/**
 * 转义特殊字符
 * @param input 输入字符串
 * @returns 转义后的字符串
 */
export const escapeSpecialChars = (input: string): string => {
  if (!input) return ''
  return input
    .replace(/'/g, '\'\'')
    .replace(/\\/g, '\\\\')
    .replace(/"/g, '\\"')
}

/**
 * 验证并清理用户输入
 * @param input 输入字符串
 * @returns 清理后的字符串，如果输入不安全则返回空字符串
 */
export const validateAndCleanInput = (input: string): string => {
  if (!input) return ''
  
  // 检查是否包含高风险字符
  if (SQL_INJECTION_CONFIG.riskChars.some(char => input.includes(char))) {
    console.warn(t('security.sql.riskDetected'), input)
    return ''
  }

  // 清理 SQL 注入风险
  const cleaned = cleanSqlInjectionRisk(input)

  // 如果清理后的字符串与原字符串长度相差太大，可能存在攻击
  if (Math.abs(cleaned.length - input.length) > input.length * 0.3) {
    console.warn(t('security.sql.possibleRisk'), input)
    return ''
  }

  // 转义特殊字符
  return escapeSpecialChars(cleaned)
} 