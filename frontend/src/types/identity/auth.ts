import type { LeanApiResult } from '../common/api'

// 设备指纹信息
export interface DeviceFingerprint {
  platform: string
  userAgent: string
  screen: {
    width: number
    height: number
    colorDepth: number
    pixelRatio: number
    availWidth: number
    availHeight: number
  }
  language: string
  timezone: string
  hardwareConcurrency: number
  deviceMemory?: number
  maxTouchPoints: number
}

// 登录请求参数
export interface LoginRequest {
  userName: string
  password: string
  sliderX: number
  sliderY: number
  captchaKey: string
  deviceName?: string
  deviceType?: number
  isTrusted?: number
  loginIp?: string
  browser?: string
  os?: string
  userAgent?: string
  screenWidth?: number
  screenHeight?: number
  screenColorDepth?: number
  screenPixelRatio?: number
  language?: string
  timezone?: string
  hardwareConcurrency?: number
  deviceMemory?: number
  maxTouchPoints?: number
}

// 登录响应数据
export interface LoginResponse {
  accessToken: string
  userInfo: UserInfo
}

// 用户信息
export interface UserInfo {
  id: number
  userName: string
  displayName: string
  englishName: string
  email: string
  roles: string[]
  permissions: string[]
  avatar?: string
  depts: {
    deptId: number
    deptName: string
  }[]
  posts: {
    postId: number
    postName: string
  }[]
  userRoles: {
    roleId: number
    roleName: string
  }[]
  loginIp?: string
  browser?: string
  os?: string
  userAgent?: string
  isMobile?: boolean
}

// 滑块验证码数据
export interface SliderCaptcha {
  value: number
  loading: boolean
  success: boolean
  captchaKey: string
  backgroundImage: string
  sliderImage: string
  y: number
  width: number
  height: number
}

// API响应类型
export type LoginApiResponse = LeanApiResult<LoginResponse>
export type SliderCaptchaResponse = LeanApiResult<SliderCaptcha> 