import { createRouter, createWebHistory } from 'vue-router'
import type { RouteRecordRaw } from 'vue-router'
import { useMenuStore } from '@/stores/modules/menu'
import { JWT_CONFIG } from '@/utils/auth'
import {
  DashboardOutlined,
  HomeOutlined,
  BarChartOutlined,
  MonitorOutlined,
  InfoCircleOutlined,
  FileOutlined,
  LockOutlined
} from '@ant-design/icons-vue'

// 基础路由
const baseRoutes: RouteRecordRaw[] = [
  {
    path: '/login',
    name: 'Login',
    component: () => import('@/views/login/index.vue'),
    meta: {
      title: '登录',
      isCache: false
    }
  },
  {
    path: '/',
    name: 'Layout',
    component: () => import('@/layouts/default/index.vue'),
    redirect: '/dashboard',
    children: [
      // 1. 仪表盘菜单（固定在最前）
      {
        path: 'dashboard',
        name: 'Dashboard',
        meta: {
          title: 'dashboard',
          icon: DashboardOutlined,
          isCache: true,
          transKey: 'menu.dashboard._self',
          orderNum: -1 // 确保在最前
        },
        redirect: '/dashboard/index',
        children: [
          {
            path: 'index',
            name: 'DashboardIndex',
            component: () => import('@/views/dashboard/index.vue'),
            meta: {
              title: 'dashboard',
              icon: HomeOutlined,
              isCache: true,
              transKey: 'menu.dashboard.index'
            }
          },
          {
            path: 'analysis',
            name: 'Analysis',
            component: () => import('@/views/dashboard/analysis.vue'),
            meta: {
              title: 'analysis',
              icon: BarChartOutlined,
              isCache: true,
              transKey: 'menu.dashboard.analysis'
            }
          },
          {
            path: 'monitor',
            name: 'Monitor',
            component: () => import('@/views/dashboard/monitor.vue'),
            meta: {
              title: 'monitor',
              icon: MonitorOutlined,
              isCache: true,
              transKey: 'menu.dashboard.monitor'
            }
          }
        ]
      },
      // 2. 关于菜单（固定在最后）
      {
        path: 'about',
        name: 'About',
        meta: {
          title: 'about',
          icon: InfoCircleOutlined,
          isCache: true,
          transKey: 'menu.about._self',
          orderNum: 999 // 确保在最后
        },
        redirect: '/about/index',
        children: [
          {
            path: 'index',
            name: 'AboutIndex',
            component: () => import('@/views/about/index.vue'),
            meta: {
              title: 'about',
              icon: InfoCircleOutlined,
              isCache: true,
              transKey: 'menu.about.index'
            }
          },
          {
            path: 'terms',
            name: 'Terms',
            component: () => import('@/views/about/terms.vue'),
            meta: {
              title: 'terms',
              icon: FileOutlined,
              isCache: true,
              transKey: 'menu.about.terms'
            }
          },
          {
            path: 'privacy',
            name: 'Privacy',
            component: () => import('@/views/about/privacy.vue'),
            meta: {
              title: 'privacy',
              icon: LockOutlined,
              isCache: true,
              transKey: 'menu.about.privacy'
            }
          }
        ]
      }
    ]
  }
]

const router = createRouter({
  history: createWebHistory(),
  routes: baseRoutes
})

// 路由守卫
router.beforeEach(async (to, from, next) => {
  // 设置页面标题
  document.title = `${to.meta.title} - Lean.CodeGen`

  // 获取token
  const token = localStorage.getItem(JWT_CONFIG.tokenKey)
  
  // 如果访问登录页，直接放行
  if (to.path === '/login') {
    next()
    return
  }

  // 如果没有token，重定向到登录页
  if (!token) {
    next('/login')
    return
  }

  // 获取用户菜单
  const menuStore = useMenuStore()
  if (menuStore.getRoutes.length === 0) {
    try {
      await menuStore.fetchUserMenu()
      // 重新进入当前路由
      next({ ...to, replace: true })
    } catch (error) {
      console.error('获取用户菜单失败:', error)
      // 如果获取菜单失败，跳转到登录页
      localStorage.removeItem(JWT_CONFIG.tokenKey)
      next('/login')
    }
  } else {
    next()
  }
})

export default router
