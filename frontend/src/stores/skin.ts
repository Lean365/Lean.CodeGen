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
  borderRadius: number
  colorSaturation: number
  fontSize: number
  enableHover: boolean
  shadowStyle: string
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
            borderRadius: state.customColors.borderRadius,
            fontSize: state.customColors.fontSize,
            colorBgElevated: state.customColors.background,
            boxShadow: state.customColors.shadowStyle === 'none' ? 'none' :
              state.customColors.shadowStyle === 'light' ? '0 2px 8px rgba(0, 0, 0, 0.15)' :
                state.customColors.shadowStyle === 'medium' ? '0 4px 12px rgba(0, 0, 0, 0.15)' :
                  '0 8px 24px rgba(0, 0, 0, 0.15)'
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