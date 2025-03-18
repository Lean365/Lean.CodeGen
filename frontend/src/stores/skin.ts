import { defineStore } from 'pinia'
import { theme } from 'ant-design-vue'

// 预设的皮肤主题
export const PRESET_THEMES = {
  default: {
    token: {
      colorPrimary: '#1890ff',
      borderRadius: 2,
    }
  },
  blue: {
    token: {
      colorPrimary: '#1677ff',
      borderRadius: 2,
    }
  },
  green: {
    token: {
      colorPrimary: '#52c41a',
      borderRadius: 2,
    }
  },
  purple: {
    token: {
      colorPrimary: '#722ed1',
      borderRadius: 2,
    }
  }
}

export type SkinType = keyof typeof PRESET_THEMES | 'custom'

interface CustomColors {
  primary: string
  background: string
  text: string
  link: string
}

interface SkinState {
  currentSkin: SkinType
  customColors: CustomColors | null
}

export const useSkinStore = defineStore('skin', {
  state: (): SkinState => {
    const savedSkin = localStorage.getItem('skin') as SkinType || 'default'
    const savedCustomColors = localStorage.getItem('customColors')
    return {
      currentSkin: savedSkin,
      customColors: savedCustomColors ? JSON.parse(savedCustomColors) : null
    }
  },

  getters: {
    currentTheme: (state) => {
      if (state.currentSkin === 'custom' && state.customColors) {
        return {
          token: {
            colorPrimary: state.customColors.primary,
            colorBgContainer: state.customColors.background,
            colorText: state.customColors.text,
            colorLink: state.customColors.link,
            borderRadius: 2,
          }
        }
      }
      return PRESET_THEMES[state.currentSkin as keyof typeof PRESET_THEMES] || PRESET_THEMES.default
    }
  },

  actions: {
    setSkin(skin: SkinType) {
      this.currentSkin = skin
      localStorage.setItem('skin', skin)
    },

    setCustomColors(colors: CustomColors) {
      this.customColors = colors
      this.currentSkin = 'custom'
      localStorage.setItem('skin', 'custom')
      localStorage.setItem('customColors', JSON.stringify(colors))
    }
  }
}) 