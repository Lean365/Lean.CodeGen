/**
 * XSRF 保护工具
 */
import i18n from '@/locales'

const { t } = i18n.global as { t: (key: string) => string }

/** XSRF 配置 */
export const XSRF_CONFIG = {
  /** Cookie 名称 */
  cookieName: 'XSRF-TOKEN',
  /** 请求头名称 */
  headerName: 'X-XSRF-TOKEN',
  /** Cookie 配置 */
  cookieOptions: {
    /** 仅 HTTPS */
    secure: true,
    /** 严格同站点 */
    sameSite: 'strict' as const,
    /** 路径 */
    path: '/'
  }
}

/**
 * 生成 XSRF 令牌
 * @returns XSRF 令牌
 */
export const generateXsrfToken = (): string => {
  const array = new Uint8Array(32)
  window.crypto.getRandomValues(array)
  return Array.from(array, byte => byte.toString(16).padStart(2, '0')).join('')
}

/**
 * 获取 XSRF 令牌
 * @returns XSRF 令牌
 */
export const getXsrfToken = (): string => {
  const cookies = document.cookie.split(';')
  const xsrfCookie = cookies.find(cookie => 
    cookie.trim().startsWith(`${XSRF_CONFIG.cookieName}=`)
  )
  if (!xsrfCookie) {
    console.warn(t('security.xsrf.tokenMissing'))
    return ''
  }
  return xsrfCookie.split('=')[1].trim()
}

/**
 * 设置 XSRF 令牌
 * @param token XSRF 令牌
 */
export const setXsrfToken = (token: string): void => {
  const options = XSRF_CONFIG.cookieOptions
  const cookieString = [
    `${XSRF_CONFIG.cookieName}=${token}`,
    `path=${options.path}`,
    options.secure ? 'secure' : '',
    `samesite=${options.sameSite}`
  ].filter(Boolean).join('; ')
  
  document.cookie = cookieString
}

/**
 * 验证 XSRF 令牌
 * @param token 要验证的令牌
 * @returns 是否有效
 */
export const validateXsrfToken = (token: string): boolean => {
  const storedToken = getXsrfToken()
  const isValid = Boolean(storedToken && token && storedToken === token)
  if (!isValid) {
    console.warn(t('security.xsrf.tokenInvalid'))
  }
  return isValid
}

/**
 * 刷新 XSRF 令牌
 * @returns 新的 XSRF 令牌
 */
export const refreshXsrfToken = (): string => {
  const newToken = generateXsrfToken()
  setXsrfToken(newToken)
  console.info(t('security.xsrf.tokenRefreshed'))
  return newToken
}

/**
 * 获取 XSRF 请求头
 * @returns 请求头对象
 */
export const getXsrfHeader = (): Record<string, string> => {
  const token = getXsrfToken()
  return token ? { [XSRF_CONFIG.headerName]: token } : {}
}

/**
 * 移除 XSRF 令牌
 */
export const removeXsrfToken = (): void => {
  document.cookie = `${XSRF_CONFIG.cookieName}=; path=/; expires=Thu, 01 Jan 1970 00:00:00 GMT`
}