/**
 * JWT 认证工具
 */

/** JWT 配置 */
export const JWT_CONFIG = {
  /** 令牌存储键名 */
  tokenKey: 'access_token',
  /** 刷新令牌存储键名 */
  refreshTokenKey: 'refresh_token',
  /** 令牌过期时间（分钟） */
  expiresInMinutes: 60
}

/**
 * 获取令牌
 */
export const getToken = (): string | null => {
  return localStorage.getItem(JWT_CONFIG.tokenKey)
}

/**
 * 设置令牌
 */
export const setToken = (token: string) => {
  localStorage.setItem(JWT_CONFIG.tokenKey, token)
}

/**
 * 获取刷新令牌
 */
export const getRefreshToken = (): string | null => {
  return localStorage.getItem(JWT_CONFIG.refreshTokenKey)
}

/**
 * 设置刷新令牌
 */
export const setRefreshToken = (token: string) => {
  localStorage.setItem(JWT_CONFIG.refreshTokenKey, token)
}

/**
 * 移除令牌
 */
export const removeToken = () => {
  localStorage.removeItem(JWT_CONFIG.tokenKey)
  localStorage.removeItem(JWT_CONFIG.refreshTokenKey)
}

/**
 * 检查令牌是否过期
 */
export const isTokenExpired = (token: string): boolean => {
  try {
    const payload = JSON.parse(atob(token.split('.')[1]))
    return payload.exp * 1000 < Date.now()
  } catch {
    return true
  }
}

/**
 * 解析令牌
 */
export const parseToken = (token: string): any => {
  try {
    return JSON.parse(atob(token.split('.')[1]))
  } catch {
    return null
  }
}

/**
 * 获取令牌中的用户ID
 */
export const getUserIdFromToken = (token: string): number | null => {
  const payload = parseToken(token)
  return payload?.nameid ? parseInt(payload.nameid) : null
}

/**
 * 获取令牌中的用户名
 */
export const getUserNameFromToken = (token: string): string | null => {
  const payload = parseToken(token)
  return payload?.unique_name || null
} 