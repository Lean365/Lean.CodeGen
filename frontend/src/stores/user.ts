import { defineStore } from 'pinia'
import { ref } from 'vue'
import type { LoginRequest, LoginResponse, UserInfo, SliderCaptcha } from '@/types/identity/auth'
import type { Response } from '@/utils/request'
import { loginAsync, logoutAsync, validateSliderCaptchaAsync, getSliderCaptchaAsync } from '@/api/identity/auth'
import { getUserInfoAsync, updateUserInfoAsync, changePasswordAsync, type ChangePasswordRequest } from '@/api/identity/user'
import { getCurrentUserPermissions } from '@/api/identity/menu'
import { JWT_CONFIG } from '@/utils/auth'
import { HubConnectionBuilder } from '@microsoft/signalr'
import { message } from 'ant-design-vue'
import { useI18n } from 'vue-i18n'
import { getBrowserInfo } from '@/utils/browser'

export const useUserStore = defineStore('user', () => {
  const token = ref(localStorage.getItem(JWT_CONFIG.tokenKey) || '')
  const userInfo = ref<UserInfo | null>(null)
  const loading = ref(false)
  const permissions = ref<string[]>([])
  let hubConnection: any = null
  const { t } = useI18n()

  // 初始化 SignalR 连接
  const initSignalRConnection = async () => {
    if (hubConnection) return

    try {
      hubConnection = new HubConnectionBuilder()
        .withUrl('http://localhost:5152/signalr/hubs')
        .withAutomaticReconnect()
        .build()

      // 监听登录尝试通知
      hubConnection.on('UserLoginAttempt', (data: { 
        userId: number;
        message: string;
        loginTime: string;
        loginIp: string;
        loginLocation: string;
      }) => {
        if (userInfo.value && data.userId === userInfo.value.id) {
          // 显示通知
          message.warning({
            content: `${data.message}\n登录时间：${new Date(data.loginTime).toLocaleString()}\n登录地点：${data.loginLocation}`,
            duration: 5
          })
        }
      })

      await hubConnection.start()
      console.log('SignalR 连接成功')
    } catch (err) {
      console.error('SignalR 连接失败:', err)
    }
  }

  // 从 localStorage 恢复用户信息
  const restoreUserInfo = () => {
    const storedUserInfo = localStorage.getItem('userInfo')
    const storedToken = localStorage.getItem(JWT_CONFIG.tokenKey)
    console.log('从 localStorage 恢复的数据:', {
      hasStoredUserInfo: !!storedUserInfo,
      hasStoredToken: !!storedToken,
      storedUserInfo,
      storedToken
    })

    if (storedUserInfo && storedToken) {
      try {
        const parsedUserInfo = JSON.parse(storedUserInfo)
        console.log('解析后的用户信息:', parsedUserInfo)
        
        // 设置状态
        userInfo.value = parsedUserInfo
        token.value = storedToken
        
        console.log('恢复后的状态:', {
          hasUserInfo: !!userInfo.value,
          hasToken: !!token.value,
          userInfoDetails: userInfo.value
        })

        // 如果有用户信息，初始化 SignalR 连接
        initSignalRConnection()
      } catch (error) {
        console.error('恢复用户信息失败:', error)
        // 如果解析失败，清除存储的信息
        localStorage.removeItem('userInfo')
        localStorage.removeItem(JWT_CONFIG.tokenKey)
        // 重置状态
        userInfo.value = null
        token.value = ''
      }
    } else {
      console.log('没有找到存储的用户信息或 token')
      // 确保状态被重置
      userInfo.value = null
      token.value = ''
    }
  }

  // 初始化时恢复用户信息
  restoreUserInfo()

  // 设置用户信息
  const setUserInfo = (loginData: LoginResponse) => {
    console.log('开始设置用户信息:', loginData)
    
    if (!loginData.accessToken || !loginData.userInfo) {
      console.error('登录数据不完整:', loginData)
      return
    }

    // 更新状态
    token.value = loginData.accessToken
    userInfo.value = loginData.userInfo

    // 存储到 localStorage
    localStorage.setItem(JWT_CONFIG.tokenKey, loginData.accessToken)
    localStorage.setItem('userInfo', JSON.stringify(loginData.userInfo))

    console.log('用户信息设置完成:', {
      hasToken: !!token.value,
      hasUserInfo: !!userInfo.value,
      userInfoDetails: userInfo.value
    })

    // 初始化 SignalR 连接
    initSignalRConnection()
  }

  // 设置用户权限
  const setPermissions = (perms: string[]) => {
    permissions.value = perms
    localStorage.setItem('permissions', JSON.stringify(perms))
  }

  // 获取用户权限
  const fetchUserPermissions = async () => {
    try {
      loading.value = true
      console.log('开始获取用户权限清单...')
      const response = await getCurrentUserPermissions()
      if (response.data) {
        setPermissions(response.data)
        // console.log('用户权限清单已更新:', response.data)
      }
      return response
    } catch (error) {
      console.error('获取用户权限清单失败:', error)
      throw error
    } finally {
      loading.value = false
    }
  }

  // 解析 JWT token
  const parseToken = (token: string) => {
    try {
      const base64Url = token.split('.')[1]
      const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/')
      const jsonPayload = decodeURIComponent(atob(base64).split('').map(function(c) {
        return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2)
      }).join(''))
      return JSON.parse(jsonPayload)
    } catch (error) {
      console.error('解析 token 失败:', error)
      return null
    }
  }

  // 登录
  const login = async (loginForm: LoginRequest) => {
    try {
      loading.value = true
      const response = await loginAsync(loginForm)
      if (response.success && response.data) {
        setUserInfo(response.data)
        // 登录成功后获取用户权限
        await fetchUserPermissions()
      } else {
        // 如果是用户在线错误，显示更友好的提示
        if (response.code === 400 && response.message === '用户已在线，请先退出其他设备') {
          message.warning(response.message || '该账号已在其他设备登录，请先退出其他设备后再登录')
        } else {
          message.error(response.message || t('login.messages.failed'))
        }
      }
      return response
    } catch (error) {
      console.error('登录失败:', error)
      message.error(t('login.messages.failed'))
      throw error
    } finally {
      loading.value = false
    }
  }

  // 登出
  const logout = async () => {
    try {
      // 如果已经在登出过程中，直接返回
      if (loading.value) {
        return
      }

      loading.value = true
      
      console.log('开始登出，当前状态:', {
        hasToken: !!token.value,
        hasUserInfo: !!userInfo.value,
        userInfoDetails: userInfo.value
      })
      
      // 检查 token 和 userInfo
      if (!token.value && !userInfo.value) {
        message.info('您已退出登录')
        return
      }

      // 获取浏览器信息
      const browserInfo = getBrowserInfo({
        browser: t('browser.unsupported.browser'),
        windows: t('browser.unsupported.windows'),
        macos: t('browser.unsupported.macos'),
        linux: t('browser.unsupported.linux'),
        android: t('browser.unsupported.android'),
        ios: t('browser.unsupported.ios'),
        os: t('browser.unsupported.os')
      })
      console.log('浏览器信息:', browserInfo)

      // 使用正则表达式检测是否为移动设备
      const isMobile = /Mobile|Android|iPhone/i.test(browserInfo.userAgent)

      // 生成设备名称
      const deviceName = `${browserInfo.os} ${browserInfo.browser}`

      // 优先使用 userInfo 中的信息
      let userId = userInfo.value?.id || 0  // 使用 id 而不是 userId
      let userName = userInfo.value?.userName || ''
      
      console.log('从 userInfo 获取的用户信息:', { userId, userName })

      // 如果 userInfo 中没有，尝试从 token 中获取
      if (!userId && token.value) {
        console.log('尝试从 token 解析用户信息')
        const tokenPayload = parseToken(token.value)
        console.log('token 解析结果:', tokenPayload)
        
        if (tokenPayload) {
          // 使用正确的 claim 名称获取用户ID
          userId = parseInt(tokenPayload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'] || '0')
          userName = tokenPayload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'] || ''
          console.log('从 token 获取的用户信息:', { userId, userName })
        }
      }

      if (!userId) {
        message.error('无法获取用户信息，请刷新页面后重试')
        return
      }

      // 打印登出请求参数
      const logoutParams = {
        userId,
        userName,
        loginIp: '',
        browser: browserInfo.browser || '',
        os: browserInfo.os || '',
        userAgent: browserInfo.userAgent || '',
        isMobile: isMobile,
        deviceName: deviceName,
        deviceType: isMobile ? 1 : 0,
        token: token.value
      }
      console.log('登出请求参数:', logoutParams)

      try {
        // 调用后端登出 API
        await logoutAsync(logoutParams)

        // 停止 SignalR 连接
        if (hubConnection) {
          await hubConnection.stop()
          hubConnection = null
        }

        // 清理前端状态
        token.value = ''
        userInfo.value = null
        permissions.value = []
        localStorage.removeItem(JWT_CONFIG.tokenKey)
        localStorage.removeItem('userInfo')
        localStorage.removeItem('permissions')

        message.success('已成功退出登录')
      } catch (error) {
        console.error('登出请求失败:', error)
        message.error('登出失败，请重试')
        throw error
      }
    } catch (error) {
      console.error('登出过程出错:', error)
      // 不抛出错误，避免界面显示错误信息
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
      const response = await validateSliderCaptchaAsync(data)
      if (!response.success) {
        throw new Error(response.message || '验证码验证失败')
      }
      return response
    } finally {
      loading.value = false
    }
  }

  // 获取滑块验证码
  const getSliderCaptcha = async (): Promise<Response<SliderCaptcha>> => {
    try {
      loading.value = true
      return await getSliderCaptchaAsync()
    } finally {
      loading.value = false
    }
  }

  // 检查是否有权限
  const hasPermission = (permission: string) => {
    return permissions.value.includes(permission)
  }

  return {
    token,
    userInfo,
    permissions,
    loading,
    login,
    logout,
    getUserInfo,
    updateUserInfo,
    changePassword,
    validateSliderCaptcha,
    getSliderCaptcha,
    fetchUserPermissions,
    hasPermission
  }
}) 