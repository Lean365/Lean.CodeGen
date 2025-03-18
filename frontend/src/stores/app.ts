import { defineStore } from 'pinia'
import { nextTick } from 'vue'
import i18n from '@/locales'

// 支持的语言类型
export type Locale = 'zh-CN' | 'zh-TW' | 'en-US' | 'ja-JP' | 'ko-KR' | 'fr-FR' | 'es-ES' | 'ar-EG' | 'ru-RU'

// 应用状态接口定义
interface AppState {
  locale: Locale      // 当前语言
  theme: 'light' | 'dark'  // 当前主题
}

// 定义应用状态管理
export const useAppStore = defineStore('app', {
  // 初始状态
  state: (): AppState => {
    const theme = localStorage.getItem('theme') as 'light' | 'dark' || 'light'
    // 初始化时设置主题
    document.body.setAttribute('data-theme', theme)
    if (theme === 'dark') {
      document.body.classList.add('dark')
    }
    
    return {
      locale: (localStorage.getItem('language') || 'zh-CN') as Locale,
      theme
    }
  },
  
  // 计算属性
  getters: {
    currentLocale: (state) => state.locale,  // 获取当前语言
    isDark: (state) => state.theme === 'dark'  // 判断是否为暗色主题
  },

  // 操作方法
  actions: {
    // 设置语言
    setLocale(locale: Locale) {
      this.locale = locale  // 更新状态
      localStorage.setItem('language', locale)  // 保存到本地存储
      // 刷新页面以应用新语言
      window.location.reload()
    },
    
    // 设置主题
    setTheme(theme: 'light' | 'dark') {
      this.theme = theme
      document.body.setAttribute('data-theme', theme)
      // 设置暗黑主题类名
      if (theme === 'dark') {
        document.body.classList.add('dark')
      } else {
        document.body.classList.remove('dark')
      }
    },
    
    // 切换主题
    toggleTheme() {
      this.theme = this.theme === 'light' ? 'dark' : 'light'
      document.body.setAttribute('data-theme', this.theme)
      // 设置暗黑主题类名
      if (this.theme === 'dark') {
        document.body.classList.add('dark')
      } else {
        document.body.classList.remove('dark')
      }
    }
  }
}) 