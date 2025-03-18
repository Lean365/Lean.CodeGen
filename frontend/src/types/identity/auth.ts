import type { ApiResponse } from '../common/api'

// 登录请求参数
export interface LoginRequest {
  userName: string
  password: string
  sliderX: number
  sliderY: number
  captchaKey: string
}

// 登录响应数据
export interface LoginResponse {
  accessToken: string
  userInfo: UserInfo
}

// 用户信息
export interface UserInfo {
  id: string
  userName: string
  displayName: string
  email: string
  roles: string[]
  permissions: string[]
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
export type LoginApiResponse = ApiResponse<LoginResponse>
export type SliderCaptchaResponse = ApiResponse<SliderCaptcha> 