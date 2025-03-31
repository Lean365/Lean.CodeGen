import axios from 'axios'
import type {  AxiosResponse, InternalAxiosRequestConfig } from 'axios'
import { getToken, removeToken } from './auth'
import { checkRateLimit, getRemainingRequests } from './rateLimit'
import { getXsrfHeader, refreshXsrfToken } from './xsrf'
import { validateAndCleanInput } from './security'
import { message } from 'ant-design-vue'
import i18n from '@/locales'
import type { LeanApiResult } from '@/types/common/api'
import { LeanBusinessType } from '@/types/common/businessType'
import { LeanErrorCode } from '@/types/common/errorCode'

const t = i18n.global.t as (key: string, values?: any) => string

/** 统一响应结构 */
export type Response<T = any> = LeanApiResult<T>

/** 请求配置 */
const REQUEST_CONFIG = {
  /** 基础 URL */
  baseURL: import.meta.env.VITE_API_BASE_URL as string,
  /** 超时时间 */
  timeout: 10000,
  /** 请求头 */
  headers: {
    'Content-Type': 'application/json;charset=UTF-8'
  },
  /** 允许发送Cookie */
  withCredentials: true
}

/** 创建 axios 实例 */
const request = axios.create(REQUEST_CONFIG) as unknown as {
  <T = any>(config: Partial<InternalAxiosRequestConfig>): Promise<Response<T>>
  get<T = any>(url: string, config?: Partial<InternalAxiosRequestConfig>): Promise<Response<T>>
  post<T = any>(url: string, data?: any, config?: Partial<InternalAxiosRequestConfig>): Promise<Response<T>>
  put<T = any>(url: string, data?: any, config?: Partial<InternalAxiosRequestConfig>): Promise<Response<T>>
  delete<T = any>(url: string, config?: Partial<InternalAxiosRequestConfig>): Promise<Response<T>>
  patch<T = any>(url: string, data?: any, config?: Partial<InternalAxiosRequestConfig>): Promise<Response<T>>
  interceptors: {
    request: {
      use: (onFulfilled?: (config: InternalAxiosRequestConfig) => InternalAxiosRequestConfig | Promise<InternalAxiosRequestConfig>, onRejected?: (error: any) => any) => number
      eject: (id: number) => void
    }
    response: {
      use: <T = any>(onFulfilled?: (response: AxiosResponse<Response<T>>) => Response<T> | Promise<Response<T>>, onRejected?: (error: any) => any) => number
      eject: (id: number) => void
    }
  }
}

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

    // 确保 Content-Type 设置正确
    if (config.method === 'post' || config.method === 'put' || config.method === 'patch') {
      if (!config.headers) {
        config.headers = new axios.AxiosHeaders()
      }
      if (!config.headers['Content-Type']) {
        config.headers['Content-Type'] = 'application/json;charset=UTF-8'
      }
    }

    // 验证并清理请求数据
    if (config.data && typeof config.data === 'object') {
      Object.keys(config.data).forEach(key => {
        if (typeof config.data[key] === 'string' && key !== 'userAgent') {
          config.data[key] = validateAndCleanInput(config.data[key])
        }
      })
    }

    return config
  },
  (error: any) => {
    message.error(t('http.error.config'))
    return Promise.reject(error)
  }
)

/** 响应拦截器 */
request.interceptors.response.use(
  <T = any>(response: AxiosResponse<Response<T>>): Promise<Response<T>> => {
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
      return Promise.reject({
        success: false,
        code: LeanErrorCode.Status400BadRequest,
        message: t('http.error.default'),
        businessType: LeanBusinessType.Other,
        traceId: undefined,
        timestamp: Date.now(),
        data: null
      })
    }

    // 如果 success 为 false，则视为业务处理失败
    if (!response.data.success) {
      console.log('响应拦截器 - 业务处理失败:', response.data)
      message.error(response.data.message || t('http.error.default'))
      return Promise.reject(response.data)
    }

    console.log('响应拦截器 - 处理成功，返回数据:', response.data)

    return Promise.resolve(response.data)
  },
  (error: any) => {
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
      const errorMessage = ((data as any)?.Message ?? (data as any)?.message) || error.message || t('http.error.default')

      // 构建标准错误响应
      const errorResponse: Response = {
        success: false,
        code: LeanErrorCode.Status400BadRequest,
        message: errorMessage,
        businessType: LeanBusinessType.Other,
        traceId: undefined,
        timestamp: Date.now(),
        data: null
      }

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
        case 407:
          message.error(t('http.status.407'))
          break
        case 408:
          message.error(t('http.status.408'))
          break
        case 409:
          message.error(t('http.status.409'))
          break
        case 410:
          message.error(t('http.status.410'))
          break
        case 411:
          message.error(t('http.status.411'))
          break
        case 412:
          message.error(t('http.status.412'))
          break
        case 413:
          message.error(t('http.status.413'))
          break
        case 414:
          message.error(t('http.status.414'))
          break
        case 415:
          message.error(t('http.status.415'))
          break
        case 416:
          message.error(t('http.status.416'))
          break
        case 417:
          message.error(t('http.status.417'))
          break
        case 418:
          message.error(t('http.status.418'))
          break
        case 421:
          message.error(t('http.status.421'))
          break
        case 422:
          message.error(t('http.status.422'))
          break
        case 423:
          message.error(t('http.status.423'))
          break
        case 424:
          message.error(t('http.status.424'))
          break
        case 425:
          message.error(t('http.status.425'))
          break
        case 426:
          message.error(t('http.status.426'))
          break
        case 428:
          message.error(t('http.status.428'))
          break
        case 429:
          message.error(t('http.status.429'))
          break
        case 431:
          message.error(t('http.status.431'))
          break
        case 451:
          message.error(t('http.status.451'))
          break
        // 服务器错误 (5xx)
        case 500:
          message.error(t('http.status.500'))
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
        case 506:
          message.error(t('http.status.506'))
          break
        case 507:
          message.error(t('http.status.507'))
          break
        case 508:
          message.error(t('http.status.508'))
          break
        case 510:
          message.error(t('http.status.510'))
          break
        case 511:
          message.error(t('http.status.511'))
          break
        default:
          message.error(errorMessage)
      }

      return Promise.reject(errorResponse)
    } else {
      message.error(error.message || t('http.error.network'))
      return Promise.reject({
        success: false,
        code: LeanErrorCode.Status500InternalServerError,
        message: error.message || t('http.error.network'),
        businessType: LeanBusinessType.Other,
        traceId: undefined,
        timestamp: Date.now(),
        data: null
      })
    }
  }
)

export default request 