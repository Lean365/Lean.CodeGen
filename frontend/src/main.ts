import { createApp } from 'vue'
import { createPinia } from 'pinia'
import Antd from 'ant-design-vue'
import 'ant-design-vue/dist/reset.css'
import '@/assets/styles/index.less'

import App from './App.vue'
import router from './router'
import { setupIcons } from './plugins/icons'
import i18n from '@/locales'
import { signalRService } from '@/utils/signalr'

// 导入组件
import Locale from '@/components/common/locale/index.vue'
import Slider from '@/components/common/captcha/slider/index.vue'
import Icons from '@/components/common/icons/index.vue'
import Search from '@/components/common/search/index.vue'
import Fullscreen from '@/components/common/fullscreen/index.vue'
import Font from '@/components/common/font/index.vue'
import Theme from '@/components/common/theme/index.vue'
import Skin from '@/components/common/skin/index.vue'
import User from '@/components/common/user/index.vue'
import Notification from '@/components/common/notification/index.vue'
import Topmenu from '@/components/navigation/topmenu/index.vue'

// 导入业务组件
import LeanQuery from '@/components/business/Query/index.vue'
import LeanToolbar from '@/components/business/Toolbar/index.vue'
import LeanTable from '@/components/business/Table/index.vue'
import LeanPagination from '@/components/business/Pagination/index.vue'
import LeanModal from '@/components/business/Modal/index.vue'
import LeanSelect from '@/components/business/Select/index.vue'

// 创建应用实例
const app = createApp(App)

// 注册组件
app.component('HbtLocale', Locale)
app.component('HbtSlider', Slider)
app.component('HbtIcons', Icons)
app.component('HbtSearch', Search)
app.component('HbtFullscreen', Fullscreen)
app.component('HbtFont', Font)
app.component('HbtTheme', Theme)
app.component('HbtSkin', Skin)
app.component('HbtUser', User)
app.component('HbtNotification', Notification)
app.component('HbtTopmenu', Topmenu)

// 注册业务组件
app.component('LeanQuery', LeanQuery)
app.component('LeanToolbar', LeanToolbar)
app.component('LeanTable', LeanTable)
app.component('LeanPagination', LeanPagination)
app.component('LeanModal', LeanModal)
app.component('LeanSelect', LeanSelect)


// 注册图标
setupIcons(app)

// 使用插件
app.use(createPinia())
app.use(router)
app.use(i18n)

// 注册 Ant Design Vue
app.use(Antd)

// 挂载应用
app.mount('#app')

// 初始化 SignalR 连接
const token = localStorage.getItem('token')
if (token) {
  signalRService.initConnection()
}

// 监听 token 变化
window.addEventListener('storage', (e) => {
  if (e.key === 'token' && e.newValue) {
    signalRService.initConnection()
  }
})
