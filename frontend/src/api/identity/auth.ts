import type { LoginRequest, LoginResponse, SliderCaptcha } from '@/types/identity/auth'
import type { Response } from '@/utils/request'
import type { ApiResponse } from '@/types/common/api'
import request from '@/utils/request'

export type { LoginRequest }

// 用户登录
export function loginAsync(data: LoginRequest): Promise<Response<LoginResponse>> {
  return request({
    url: 'api/LeanAuth/login',
    method: 'post',
    data: {
      userName: data.userName,
      password: data.password,
      captchaKey: data.captchaKey,
      sliderX: data.sliderX,
      sliderY: data.sliderY
    },
    withCredentials: true
  })
}

// 获取滑块验证码
export function getSliderCaptchaAsync(): Promise<Response<SliderCaptcha>> {
  return request({
    url: 'api/LeanAuth/captcha/slider',
    method: 'get'
  })
}

// 验证滑块验证码
export function validateSliderCaptchaAsync(data: { captchaKey: string; x: number; y: number }): Promise<Response<void>> {
  return request({
    url: 'api/LeanAuth/captcha/slider/validate',
    method: 'post',
    data
  })
}

// 用户登出
export function logoutAsync(): Promise<Response<void>> {
  return request({
    url: 'api/LeanAuth/logout',
    method: 'post'
  })
} 