import { defineStore } from 'pinia'
import { ref } from 'vue'
import type { LoginRequest, LoginResponse, UserInfo } from '@/types/identity/auth'
import { loginAsync, logoutAsync, validateSliderCaptchaAsync, getSliderCaptchaAsync } from '@/api/identity/auth'
import { getUserInfoAsync, updateUserInfoAsync, changePasswordAsync, type ChangePasswordRequest } from '@/api/identity/user'

export const useUserStore = defineStore('user', () => {
  const token = ref(localStorage.getItem('token') || '')
  const userInfo = ref<UserInfo | null>(null)
  const loading = ref(false)

  // 设置用户信息
  const setUserInfo = (loginData: LoginResponse) => {
    token.value = loginData.accessToken
    userInfo.value = loginData.userInfo
    localStorage.setItem('token', loginData.accessToken)
    localStorage.setItem('userInfo', JSON.stringify(loginData.userInfo))
  }

  // 登录
  const login = async (loginForm: LoginRequest) => {
    try {
      loading.value = true
      const response = await loginAsync(loginForm)
      if (response.success) {
        setUserInfo(response.data)
      }
      return response
    } finally {
      loading.value = false
    }
  }

  // 登出
  const logout = async () => {
    try {
      loading.value = true
      await logoutAsync()
      token.value = ''
      userInfo.value = null
      localStorage.removeItem('token')
      localStorage.removeItem('userInfo')
    } finally {
      loading.value = false
    }
  }

  // 获取用户信息
  const getUserInfo = async () => {
    try {
      loading.value = true
      const response = await getUserInfoAsync()
      if (response.success && response.data) {
        userInfo.value = response.data
        localStorage.setItem('userInfo', JSON.stringify(response.data))
      }
      return response
    } finally {
      loading.value = false
    }
  }

  // 更新用户信息
  const updateUserInfo = async (data: Partial<UserInfo>) => {
    try {
      loading.value = true
      const response = await updateUserInfoAsync(data)
      if (response.success) {
        // 更新成功后重新获取用户信息
        await getUserInfo()
      }
      return response
    } finally {
      loading.value = false
    }
  }

  // 修改密码
  const changePassword = async (data: ChangePasswordRequest) => {
    try {
      loading.value = true
      return await changePasswordAsync(data)
    } finally {
      loading.value = false
    }
  }

  // 验证滑块验证码
  const validateSliderCaptcha = async (data: { captchaKey: string; x: number; y: number }) => {
    try {
      loading.value = true
      return await validateSliderCaptchaAsync(data)
    } finally {
      loading.value = false
    }
  }

  // 获取滑块验证码
  const getSliderCaptcha = async () => {
    try {
      loading.value = true
      return await getSliderCaptchaAsync()
    } finally {
      loading.value = false
    }
  }

  return {
    token,
    userInfo,
    loading,
    login,
    logout,
    getUserInfo,
    updateUserInfo,
    changePassword,
    validateSliderCaptcha,
    getSliderCaptcha
  }
}) 