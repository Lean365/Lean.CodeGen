import { createApp } from 'vue'
import { createPinia } from 'pinia'

import Antd from 'ant-design-vue'

import 'ant-design-vue/dist/reset.css';
// 导入全局样式
import '@/assets/styles/index.less'

import App from './App.vue'
import router from './router'
import { setupIcons } from './plugins/icons'
import i18n from '@/locales'

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

// 注册 Pinia
app.use(createPinia())

// 注册路由
app.use(router)

// 注册 Ant Design Vue
app.use(Antd)

// 注册 i18n
app.use(i18n)

// 注册图标
setupIcons(app)

app.mount('#app')
