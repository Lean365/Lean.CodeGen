<template>
  <div class="login-container">
    <!-- 右上角工具栏 -->
    <div class="toolbar">
      <a-space>
        <HbtLocale @change="handleLocaleChange" />
        <HbtTheme @change="handleThemeChange" />
      </a-space>
    </div>

    <div class="login-content">
      <!-- 左侧信息卡片 -->
      <a-card class="info-card" :bordered="false">
        <img src="@/assets/images/logo/logo.svg" alt="logo" class="logo" />
        <h1>{{ t('app.name') }}</h1>
        <p>{{ t('app.slogan') }}</p>
        <p>{{ t('app.description') }}</p>
      </a-card>

      <!-- 右侧登录卡片 -->
      <a-card class="login-card" :bordered="false">
        <h2>{{ t('login.title.text') }}</h2>
        <a-form ref="loginFormRef" :model="loginForm" :rules="loginRules" class="login-form">
          <a-form-item name="username">
            <a-input v-model:value="loginForm.username" :placeholder="t('login.form.username.placeholder')"
              size="large">
              <template #prefix>
                <UserOutlined />
              </template>
            </a-input>
          </a-form-item>

          <a-form-item name="password">
            <a-input-password v-model:value="loginForm.password" :placeholder="t('login.form.password.placeholder')"
              size="large">
              <template #prefix>
                <LockOutlined />
              </template>
            </a-input-password>
          </a-form-item>

          <a-form-item name="captcha">
            <HbtSlider v-model:value="captcha.value" :loading="captcha.loading" :success="captcha.success"
              :captcha="captcha" @refresh="handleRefreshCaptcha" @validate="handleCaptchaValidate" />
          </a-form-item>

          <a-form-item>
            <a-button type="primary" :loading="loading" class="login-button" @click="handleLogin">
              {{ t('login.form.submit.text') }}
            </a-button>
          </a-form-item>
        </a-form>
      </a-card>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { useI18n } from 'vue-i18n'
import { message } from 'ant-design-vue'
import type { FormInstance } from 'ant-design-vue'
import type { Rule } from 'ant-design-vue/es/form'
import { useAppStore } from '@/stores/app'
import type { Locale } from '@/stores/app'
import { useUserStore } from '@/stores/user'
import { useMenuStore } from '@/stores/modules/menu'
import { getBrowserInfo } from '@/utils/browser'
import type { BrowserInfo } from '@/utils/browser'
import type { SliderCaptcha } from '@/types/identity/auth'

import {
  UserOutlined,
  LockOutlined
} from '@ant-design/icons-vue'

const { t } = useI18n()
const router = useRouter()
const appStore = useAppStore()
const userStore = useUserStore()
const menuStore = useMenuStore()
const loading = ref(false)
const loginFormRef = ref<FormInstance>()
const captcha = ref({
  value: 0,
  loading: false,
  success: false,
  captchaKey: '',
  backgroundImage: '',
  sliderImage: '',
  y: 0,
  width: 0,
  height: 0,
  hasCaptcha: false,
  hasBackgroundImage: false,
  hasSliderImage: false,
  hasShownSuccess: false
})

const loginForm = reactive({
  username: 'admin',
  password: '123456',
  captcha: ''
})

const loginRules: Record<string, Rule[]> = {
  username: [
    { required: true, message: t('login.form.username.required'), trigger: 'change' },
    { min: 2, max: 20, message: t('form.lengthError'), trigger: 'blur' }
  ],
  password: [
    { required: true, message: t('login.form.password.required'), trigger: 'change' },
    { min: 6, max: 20, message: t('form.lengthError'), trigger: 'blur' }
  ]
}

const handleRefreshCaptcha = async () => {
  try {
    captcha.value.loading = true
    console.log('开始获取验证码...')
    const response = await userStore.getSliderCaptcha()
    console.log('验证码响应数据:', response)

    // 检查响应是否成功
    if (!response.success) {
      throw new Error(response.message || '获取验证码失败')
    }

    // 检查数据是否完整
    if (!response.data) {
      throw new Error('验证码数据为空')
    }

    const { backgroundImage, sliderImage, y, width, height, captchaKey } = response.data

    // 更新验证码数据
    captcha.value = {
      ...captcha.value,
      backgroundImage,
      sliderImage,
      y,
      width: width || 0,
      height: height || 0,
      captchaKey,
      hasCaptcha: true,
      hasBackgroundImage: !!backgroundImage,
      hasSliderImage: !!sliderImage,
      value: 0,
      success: false,
      hasShownSuccess: false
    }
  } catch (error) {
    console.error('获取验证码失败:', error)
    message.error(error instanceof Error ? error.message : t('slider.refreshFailed'))
  } finally {
    captcha.value.loading = false
  }
}

const handleCaptchaValidate = async (x: number, y: number) => {
  try {
    console.log('验证码验证:', { x, y })
    const response = await userStore.validateSliderCaptcha({
      captchaKey: captcha.value.captchaKey,
      x,
      y
    })

    if (response.success) {
      loginForm.captcha = `${x},${y}`
      captcha.value.success = true
      if (!captcha.value.hasShownSuccess) {
        message.success(t('slider.success'))
        captcha.value.hasShownSuccess = true
      }
    } else {
      message.error(response.message || t('slider.failed'))
      handleRefreshCaptcha()
    }
  } catch (error) {
    console.error('验证码验证失败:', error)
    message.error(error instanceof Error ? error.message : t('slider.error'))
    handleRefreshCaptcha()
  }
}

const handleLogin = async () => {
  try {
    loading.value = true
    await loginFormRef.value?.validate()

    // 刷新XSRF令牌
    refreshXsrfToken()

    // 解析验证码坐标
    const [x, y] = loginForm.captcha.split(',').map(Number)
    if (!x || !y) {
      message.error(t('login.form.captcha.required'))
      return
    }

    // 获取设备信息
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

    // 如果浏览器或操作系统不支持，显示错误信息并返回
    if (!browserInfo.isSupported) {
      message.error({
        content: browserInfo.unsupportedMessage,
        duration: 0,
        key: 'browser-compatibility'
      })
      return
    }

    const deviceInfo = {
      deviceName: browserInfo.deviceName,
      deviceType: browserInfo.deviceType,
      os: browserInfo.os,
      browser: browserInfo.browser,
      userAgent: browserInfo.userAgent,
      screenWidth: browserInfo.screen.width,
      screenHeight: browserInfo.screen.height,
      screenColorDepth: browserInfo.screen.colorDepth,
      screenPixelRatio: browserInfo.screen.pixelRatio,
      language: browserInfo.locale.language,
      timezone: browserInfo.locale.timezone,
      hardwareConcurrency: browserInfo.hardware.hardwareConcurrency,
      deviceMemory: browserInfo.hardware.deviceMemory,
      maxTouchPoints: browserInfo.hardware.maxTouchPoints
    }
    console.log('设备信息:', deviceInfo)

    console.log('开始登录...', {
      userName: loginForm.username,
      sliderX: x,
      sliderY: y,
      captchaKey: captcha.value.captchaKey,
      ...deviceInfo
    })

    const response = await userStore.login({
      userName: loginForm.username,
      password: loginForm.password,
      sliderX: x,
      sliderY: y,
      captchaKey: captcha.value.captchaKey,
      ...deviceInfo
    })

    if (response.success && response.code !== 400) {
      message.success(t('login.messages.success'))
      router.push('/')
    } else {
      handleRefreshCaptcha()
    }
  } catch (error) {
    console.error('登录失败:', error)
    handleRefreshCaptcha()
  } finally {
    loading.value = false
  }
}

// 处理语言切换
const handleLocaleChange = (locale: Locale) => {
  appStore.setLocale(locale)
}

// 处理主题切换
const handleThemeChange = (theme: 'light' | 'dark') => {
  appStore.setTheme(theme)
}

// 组件挂载时获取验证码
onMounted(() => {
  // 检查浏览器和操作系统兼容性
  const browserInfo = getBrowserInfo({
    browser: t('browser.unsupported.browser'),
    windows: t('browser.unsupported.windows'),
    macos: t('browser.unsupported.macos'),
    linux: t('browser.unsupported.linux'),
    android: t('browser.unsupported.android'),
    ios: t('browser.unsupported.ios'),
    os: t('browser.unsupported.os')
  })
  if (!browserInfo.isSupported) {
    message.error({
      content: browserInfo.unsupportedMessage,
      duration: 0,
      key: 'browser-compatibility'
    })
  }

  handleRefreshCaptcha()
})
</script>

<style lang="less" scoped>
.login-container {
  position: relative;
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 100vh;

  .toolbar {
    position: absolute;
    top: 24px;
    right: 64px;
    z-index: 1;
  }

  .login-content {
    display: flex;
    width: 1000px;

    .info-card {
      flex: 1;
      text-align: center;

      .logo {
        width: 120px;
        height: 120px;
        margin-bottom: 24px;
      }

      h1 {
        font-size: 28px;
        margin-bottom: 16px;
      }

      p {
        margin-bottom: 8px;
      }
    }

    .login-card {
      flex: 1;

      h2 {
        font-size: 24px;
        margin-bottom: 24px;
      }

      .login-form {
        max-width: 360px;
        margin: 0 auto;

        .login-button {
          width: 100%;
        }
      }
    }
  }
}
</style>