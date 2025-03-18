import axios from 'axios'
import type {  AxiosResponse, InternalAxiosRequestConfig } from 'axios'
import { getToken, removeToken } from './auth'
import { checkRateLimit, getRemainingRequests } from './rateLimit'
import { getXsrfHeader, refreshXsrfToken } from './xsrf'
import { validateAndCleanInput } from './security'
import { message } from 'ant-design-vue'
import i18n from '@/locales'

const t = i18n.global.t as (key: string, values?: any) => string

/** 统一响应结构 */
export interface Response<T = any> {
  /** 是否成功 */
  success: boolean
  /** 响应数据 */
  data: T
  /** 错误信息 */
  message: string
  /** 错误码 */
  code: number
  /** 业务类型 */
  businessType?: string
  /** 跟踪ID */
  traceId?: string
  /** 时间戳 */
  timestamp?: number
}

/** 请求配置 */
const REQUEST_CONFIG = {
  /** 基础 URL */
  baseURL: import.meta.env.VITE_API_BASE_URL as string,
  /** 超时时间 */
  timeout: 10000,
  /** 请求头 */
  headers: {
    'Content-Type': 'application/json'
  },
  /** 允许发送Cookie */
  withCredentials: true
}

/** 创建 axios 实例 */
const request = axios.create(REQUEST_CONFIG)

/** 请求拦截器 */
request.interceptors.request.use(
  (config: InternalAxiosRequestConfig) => {
    // 检查速率限制
    if (!checkRateLimit(config.url || '')) {
      const remaining = getRemainingRequests(config.url || '')
      message.error(t('http.business.rateLimit', { remaining }))
      return Promise.reject(new Error(t('http.business.rateLimit', { remaining })))
    }

    // 添加认证令牌
    const token = getToken()
    if (token && config.headers) {
      config.headers.Authorization = `Bearer ${token}`
    }

    // 添加 XSRF 令牌
    const xsrfHeaders = getXsrfHeader()
    if (config.headers) {
      Object.assign(config.headers, xsrfHeaders)
    }

    // 验证并清理请求数据
    if (config.data && typeof config.data === 'object') {
      Object.keys(config.data).forEach(key => {
        if (typeof config.data[key] === 'string') {
          config.data[key] = validateAndCleanInput(config.data[key])
        }
      })
    }

    return config
  },
  error => {
    message.error(t('http.error.config'))
    return Promise.reject(error)
  }
)

/** 响应拦截器 */
request.interceptors.response.use(
  <T = any>(response: AxiosResponse<Response<T>>) => {
    // 刷新 XSRF 令牌
    refreshXsrfToken()

    console.log('响应拦截器 - 开始处理响应:', {
      status: response.status,
      data: response.data
    })

    // 先检查 HTTP 状态码
    if (response.status !== 200) {
      console.log('响应拦截器 - HTTP状态码错误:', response.status)
      message.error(t('http.error.default'))
      return Promise.reject(new Error(t('http.error.default')))
    }

    const res = response.data
    console.log('响应拦截器 - 检查业务状态:', {
      success: res.success,
      code: res.code,
      message: res.message
    })

    if (!res.success) {
      console.log('响应拦截器 - 业务处理失败')
      const errorMessage = res.message || t('http.error.default')
      message.error(errorMessage)
      return Promise.reject(new Error(errorMessage))
    }

    console.log('响应拦截器 - 处理成功，返回数据')
    return response.data as any
  },
  error => {
    console.error('响应拦截器 - 捕获错误:', {
      name: error.name,
      message: error.message,
      code: error.code,
      config: error.config,
      response: error.response,
      stack: error.stack
    })

    if (error.response) {
      const { status, data } = error.response
      
      // 优先使用后端返回的错误信息
      const errorMessage = data?.message || error.message || t('http.error.default')

      switch (status) {
        // 客户端错误 (4xx)
        case 400:
          message.error(errorMessage || t('http.status.400'))
          break
        case 401:
          message.error(t('http.status.401'))
          removeToken()
          // TODO: 重定向到登录页
          window.location.href = '/login'
          break
        case 402:
          message.error(t('http.status.402'))
          break
        case 403:
          message.error(t('http.status.403'))
          break
        case 404:
          message.error(t('http.status.404'))
          break
        case 405:
          message.error(t('http.status.405'))
          break
        case 406:
          message.error(t('http.status.406'))
          break
        case 408:
          message.error(t('http.status.408'))
          break
        case 413:
          message.error(t('http.status.413'))
          break
        case 415:
          message.error(t('http.status.415'))
          break
        case 422:
          message.error(t('http.status.422'))
          break
        case 423:
          message.error(t('http.status.423'))
          break
        case 428:
          message.error(t('http.status.428'))
          break
        case 429:
          message.error(t('http.status.429'))
          break

        // 服务器错误 (5xx)
        case 500:
          message.error(errorMessage || t('http.status.500'))
          break
        case 501:
          message.error(t('http.status.501'))
          break
        case 502:
          message.error(t('http.status.502'))
          break
        case 503:
          message.error(t('http.status.503'))
          break
        case 504:
          message.error(t('http.status.504'))
          break
        case 505:
          message.error(t('http.status.505'))
          break
        
        default:
          if (status >= 400 && status < 500) {
            message.error(`${t('http.error.default')}(${status}): ${errorMessage}`)
          } else if (status >= 500 && status < 600) {
            message.error(`${t('http.error.serverError')}(${status}): ${errorMessage}`)
          } else {
            message.error(`${t('http.error.unknown')}(${status}): ${errorMessage}`)
          }
      }
      return Promise.reject(new Error(errorMessage))
    } else if (error.request) {
      // 网络错误
      const errorMessage = error.message || t('http.error.network')
      if (error.code === 'ECONNABORTED') {
        message.error(t('http.error.timeout'))
      } else if (error.code === 'ERR_NETWORK') {
        message.error(t('http.business.networkError'))
      } else if (error.code === 'ERR_BAD_RESPONSE') {
        message.error(t('http.business.serverError'))
      } else {
        message.error(t('http.error.network'))
      }
      return Promise.reject(new Error(errorMessage))
    } else {
      // 请求配置错误
      const errorMessage = error.message || t('http.error.default')
      if (error.code === 'ERR_BAD_REQUEST') {
        message.error(t('http.business.requestError'))
      } else if (error.code === 'ERR_BAD_OPTION') {
        message.error(t('http.business.optionError'))
      } else {
        message.error(`${t('http.error.default')}: ${errorMessage}`)
      }
      return Promise.reject(new Error(errorMessage))
    }
  }
)

export default request 