import { createI18n } from 'vue-i18n';
// 导入公共翻译
import commonEnUS from './common/en-US'
import commonZhCN from './common/zh-CN'

// 导入应用翻译
import appEnUS from './app/en-US'
import appZhCN from './app/zh-CN'

// 导入登录翻译
import loginEnUS from './identity/login/en-US'
import loginZhCN from './identity/login/zh-CN'

// 导入菜单翻译
import menuEnUS from './menu/en-US'
import menuZhCN from './menu/zh-CN'

// 导入用户翻译
import userEnUS from './identity/user/en-US'
import userZhCN from './identity/user/zh-CN'

// 导入验证码翻译
import captchaEnUS from './identity/captcha/en-US'
import captchaZhCN from './identity/captcha/zh-CN'

// 导入关于页面翻译
import aboutEnUS from './about/en-US'
import aboutZhCN from './about/zh-CN'

// 导入仪表盘翻译
import dashboardEnUS from './dashboard/en-US'
import dashboardZhCN from './dashboard/zh-CN'

// 导入代码生成翻译
import generateEnUS from './generator/en-US'
import generateZhCN from './generator/zh-CN'

// 导入工作流翻译
import workflowEnUS from './workflow/en-US'
import workflowZhCN from './workflow/zh-CN'

// 导入安全翻译
import securityEnUS from './security/en-US'
import securityZhCN from './security/zh-CN'

// 导入皮肤设置翻译
import skinEnUS from './skin/en-US'
import skinZhCN from './skin/zh-CN'

// 合并翻译
const messages = {
  'en-US': {
    ...commonEnUS,
    ...appEnUS,
    ...loginEnUS,
    ...menuEnUS,
    ...userEnUS,
    ...captchaEnUS,
    ...aboutEnUS,
    ...dashboardEnUS,
    ...generateEnUS,
    ...workflowEnUS,
    ...securityEnUS,
    ...skinEnUS
  },
  'zh-CN': {
    ...commonZhCN,
    ...appZhCN,
    ...loginZhCN,
    ...menuZhCN,
    ...userZhCN,
    ...captchaZhCN,
    ...aboutZhCN,
    ...dashboardZhCN,
    ...generateZhCN,
    ...workflowZhCN,
    ...securityZhCN,
    ...skinZhCN
  }
}

const i18n = createI18n({
  legacy: false,
  locale: localStorage.getItem('language') || 'zh-CN',
  fallbackLocale: 'zh-CN',
  messages
})

export default i18n; 