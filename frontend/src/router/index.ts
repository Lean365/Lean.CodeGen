import { createRouter, createWebHistory } from 'vue-router'
import DefaultLayout from '@/layouts/default/index.vue'
import i18n from '@/locales'
import type { Composer } from 'vue-i18n'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/login',
      name: 'login',
      component: () => import('@/views/login/index.vue'),
      meta: {
        title: 'login.title',
        hidden: true
      }
    },
    {
      path: '/',
      component: DefaultLayout,
      meta: {
        icon: 'HomeOutlined',
        title: 'menu.home.title'
      },
      children: [
        {
          path: '',
          redirect: '/dashboard'
        },
        {
          path: 'dashboard',
          name: 'dashboard',
          component: () => import('@/views/dashboard/index.vue'),
          meta: {
            title: 'menu.dashboard.title',
            requiresAuth: true,
            icon: 'DashboardOutlined'
          }
        },
        {
          path: 'dashboard/analysis',
          name: 'dashboard.analysis',
          component: () => import('@/views/dashboard/analysis.vue'),
          meta: {
            title: 'menu.dashboard.analysis',
            requiresAuth: true,
            icon: 'BarChartOutlined'
          }
        },
        {
          path: 'dashboard/monitor',
          name: 'dashboard.monitor',
          component: () => import('@/views/dashboard/monitor.vue'),
          meta: {
            title: 'menu.dashboard.monitor',
            requiresAuth: true,
            icon: 'MonitorOutlined'
          }
        },
        {
          path: 'about',
          name: 'about',
          component: () => import('@/views/about/index.vue'),
          meta: {
            title: 'menu.about.title',
            icon: 'InfoCircleOutlined'
          }
        },
        {
          path: 'about/terms',
          name: 'about.terms',
          component: () => import('@/views/about/terms.vue'),
          meta: {
            title: 'menu.about.terms',
            icon: 'FileTextOutlined'
          }
        },
        {
          path: 'about/privacy',
          name: 'about.privacy',
          component: () => import('@/views/about/privacy.vue'),
          meta: {
            title: 'menu.about.privacy',
            icon: 'SafetyOutlined'
          }
        }
      ]
    }
  ]
})

// 路由守卫
router.beforeEach((to, from, next) => {
  // 设置页面标题
  const title = to.meta.title ? (i18n.global as Composer).t(to.meta.title as string) : 'Lean.CodeGen'
  document.title = `${title} - Lean.CodeGen`
  
  // 检查是否需要认证
  if (to.meta.requiresAuth) {
    const isAuthenticated = localStorage.getItem('token')
    if (!isAuthenticated) {
      next('/login')
      return
    }
  }
  
  next()
})

export default router
