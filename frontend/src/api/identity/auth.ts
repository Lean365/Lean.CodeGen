import type { LoginRequest, LoginResponse, SliderCaptcha } from '@/types/identity/auth'
import type { Response } from '@/utils/request'
import type { LeanApiResult } from '@/types/common/api'
import request from '@/utils/request'
import { getBrowserInfo } from '@/utils/browser'
import axios from 'axios'

export type { LoginRequest }

// 用户登录
export async function loginAsync(data: LoginRequest): Promise<Response<LoginResponse>> {
  console.log('发送登录请求数据:', {
    ...data,
    password: '******' // 隐藏密码
  })
  return request({
    url: 'api/LeanAuth/login',
    method: 'post',
    data
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
export function logoutAsync(data: {
  userId: number;
  userName: string;
  loginIp: string;
  browser: string;
  os: string;
  userAgent: string;
  isMobile: boolean;
  token: string;
}): Promise<Response<void>> {
  return request({
    url: 'api/LeanAuth/logout',
    method: 'post',
    data
  })
} 