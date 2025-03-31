import type { RouteRecordRaw } from 'vue-router'
import type { LeanMenuDto } from '@/types/admin/menu'

// 生成路由配置
export function generateRoutes(menus: LeanMenuDto[]): RouteRecordRaw[] {
  return menus.map(menu => generateRoute(menu))
}

// 生成单个路由配置
function generateRoute(menu: LeanMenuDto): RouteRecordRaw {
  const route: RouteRecordRaw = {
    path: menu.path,
    name: menu.name,
    meta: {
      title: menu.name,
      icon: menu.icon,
      isCache: menu.isCache,
      isLink: false,
      link: menu.path,
      isHide: !menu.isVisible,
      isFull: false,
      isAffix: false,
      isKeepAlive: menu.isCache,
      roles: [],
      permissions: menu.perms
    },
    component: menu.component ? () => import(`@/views/${menu.component}.vue`) : undefined,
    children: menu.children?.map(child => generateRoute(child)) || []
  }

  return route
}