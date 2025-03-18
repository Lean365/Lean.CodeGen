import type { App } from 'vue'
import * as Icons from '@ant-design/icons-vue'
import { createFromIconfontCN } from '@ant-design/icons-vue'

// 创建 iconfont 组件
export const IconFont = createFromIconfontCN({
  // 使用单个图标库
  scriptUrl: '//at.alicdn.com/t/c/font_4444444_abcdef.js'
})

// 注册所有图标组件
export function setupIcons(app: App) {
  // 注册 Ant Design Vue 图标
  Object.entries(Icons).forEach(([key, component]) => {
    // 只注册组件类型的图标
    if (key.endsWith('Outlined') || key.endsWith('Filled') || key.endsWith('TwoTone')) {
      app.component(key, component)
    }
  })
  
  // 注册 iconfont 组件
  app.component('IconFont', IconFont)
}

// 导出所有图标
export { Icons }

// 导出类型
export type IconType = keyof typeof Icons 