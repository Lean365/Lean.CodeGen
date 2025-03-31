<template>
  <a-layout-sider v-model:collapsed="collapsed" :trigger="null" collapsible class="side-menu" :theme="theme">
    <div class="logo">
      <img src="@/assets/images/logo/logo.svg" alt="logo" />
      <h1 v-show="!collapsed">{{ appName }}</h1>
    </div>
    <a-menu v-model:selectedKeys="selectedKeys" mode="inline" :theme="theme" :inline-collapsed="collapsed"
      :openKeys="openKeys" :inlineOpenKeys="rootSubmenuKeys" @openChange="handleOpenChange">
      <template v-for="menu in menuList" :key="menu.id">
        <a-sub-menu v-if="menu.children && menu.children.length > 0" :key="`parent-${menu.path}`">
          <template #title>
            <span>
              <component :is="menu.icon" v-if="menu.icon" />
              <span>{{ getMenuTitle(menu) }}</span>
            </span>
          </template>
          <a-menu-item v-for="child in menu.children" :key="child.path" @click="handleMenuClick(child)">
            <component :is="child.icon" v-if="child.icon" />
            <span>{{ getMenuTitle(child) }}</span>
          </a-menu-item>
        </a-sub-menu>
        <a-menu-item v-else :key="`menu-${menu.path}`" @click="handleMenuClick(menu)">
          <component :is="menu.icon" v-if="menu.icon" />
          <span>{{ getMenuTitle(menu) }}</span>
        </a-menu-item>
      </template>
    </a-menu>
  </a-layout-sider>
</template>

<script setup lang="ts">
import { ref, computed, watch } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import { useI18n } from 'vue-i18n'
import { useAppStore } from '@/stores/app'
import { useMenuStore } from '@/stores/modules/menu'
import {
  DashboardOutlined,
  BarChartOutlined,
  MonitorOutlined,
  HomeOutlined,
  InfoCircleOutlined,
  FileOutlined,
  LockOutlined
} from '@ant-design/icons-vue'
import type { MenuTheme } from 'ant-design-vue'
import type { MenuProps } from 'ant-design-vue'
import type { Key } from 'ant-design-vue/lib/_util/type'
import type { LeanMenuDto } from '@/types/identity/menu'
import router from '@/router'

// 静态菜单接口
interface StaticMenuItem {
  id: number
  menuName: string
  path: string
  icon: any // 修改为any类型以支持Ant Design Vue的图标组件
  transKey: string
  parentId: number
  component: string
  isFrame: number
  isCached: number
  visible: number
  menuStatus: number
  menuType: number
  orderNum: number
  perms: string
  isBuiltin: number
  createTime: string
  children?: StaticMenuItem[]
}

const { t } = useI18n()
const route = useRoute()
const appStore = useAppStore()
const menuStore = useMenuStore()

const collapsed = ref(false)
const selectedKeys = ref<(string | number)[]>([])
const openKeys = ref<Key[]>([])

const theme = computed<MenuTheme>(() => appStore.isDark ? 'dark' : 'light')

// 获取静态菜单
const staticMenus = computed<StaticMenuItem[]>(() => {
  const routes = router.getRoutes()
  // 只获取根级路由
  const rootRoute = routes.find(route => route.path === '/')
  if (!rootRoute || !rootRoute.children) return []

  // 获取静态菜单路由（dashboard和about）
  const staticRoutes = rootRoute.children.filter(route =>
    route.path === 'dashboard' || route.path === 'about'
  )

  // 将路由转换为菜单项
  const menuItems: StaticMenuItem[] = staticRoutes.map(route => ({
    id: Number(route.name?.toString().replace(/[^0-9]/g, '') || '0'),
    menuName: route.meta?.title as string,
    path: `/${route.path}`,
    icon: route.path === 'dashboard' ? DashboardOutlined : InfoCircleOutlined,
    transKey: route.meta?.transKey as string,
    parentId: 0,
    component: route.path,
    isFrame: 0,
    isCached: 0,
    visible: 0,
    menuStatus: 0,
    menuType: 0,
    orderNum: (route.meta?.orderNum as number) || 0,
    perms: route.path,
    isBuiltin: 0,
    createTime: new Date().toISOString(),
    children: route.children?.map(child => ({
      id: Number(child.name?.toString().replace(/[^0-9]/g, '') || '0'),
      menuName: child.meta?.title as string,
      path: `/${route.path}/${child.path}`,
      icon: getChildIcon(route.path, child.path),
      transKey: child.meta?.transKey as string,
      parentId: Number(route.name?.toString().replace(/[^0-9]/g, '') || '0'),
      component: child.path,
      isFrame: 0,
      isCached: 0,
      visible: 0,
      menuStatus: 0,
      menuType: 0,
      orderNum: (child.meta?.orderNum as number) || 0,
      perms: child.path,
      isBuiltin: 0,
      createTime: new Date().toISOString()
    }))
  }))

  // 根据orderNum排序
  return menuItems.sort((a, b) => (a.orderNum || 0) - (b.orderNum || 0))
})

// 获取子菜单图标
const getChildIcon = (parentPath: string, childPath: string) => {
  if (parentPath === 'dashboard') {
    switch (childPath) {
      case 'index': return HomeOutlined
      case 'analysis': return BarChartOutlined
      case 'monitor': return MonitorOutlined
      default: return ''
    }
  } else if (parentPath === 'about') {
    switch (childPath) {
      case 'index': return InfoCircleOutlined
      case 'terms': return FileOutlined
      case 'privacy': return LockOutlined
      default: return ''
    }
  }
  return ''
}

// 获取菜单列表（合并静态菜单和动态菜单）
const menuList = computed<(LeanMenuDto | StaticMenuItem)[]>(() => {
  // 按照orderNum排序所有菜单
  return [...staticMenus.value, ...menuStore.getMenuList].sort((a, b) => {
    const aOrder = (a as StaticMenuItem).orderNum || 0
    const bOrder = (b as StaticMenuItem).orderNum || 0
    return aOrder - bOrder
  })
})

// 获取应用名称
const appName = computed(() => {
  try {
    return t('app.name')
  } catch (error) {
    console.error('Translation error:', error)
    return 'Lean.CodeGen'
  }
})

// 获取菜单标题
const getMenuTitle = (menu: LeanMenuDto | StaticMenuItem) => {
  try {
    if (menu.transKey) {
      const title = t(menu.transKey);
      return title === menu.transKey ? menu.menuName : title;
    }
    return menu.menuName;
  } catch (error) {
    console.error('菜单翻译错误:', error);
    return menu.menuName;
  }
}

const rootSubmenuKeys = computed(() => menuList.value
  .filter(menu => menu.children && menu.children.length > 0)
  .map(menu => `parent-${menu.path}`)
)

// 初始化菜单状态
const initMenuState = () => {
  const path = route.path
  const findMatchingMenu = (menus: (LeanMenuDto | StaticMenuItem)[]): { menu: LeanMenuDto | StaticMenuItem; parent: (LeanMenuDto | StaticMenuItem) | null } | null => {
    for (const menu of menus) {
      if (path === menu.path) {
        return { menu, parent: null }
      }
      if (menu.children && menu.children.length > 0) {
        for (const child of menu.children) {
          if (child.path === path) {
            return { menu: child, parent: menu }
          }
        }
      }
    }
    return null
  }

  const result = findMatchingMenu(menuList.value)
  if (result) {
    // 如果是子菜单项，直接使用path作为key
    // 如果是根菜单项，加上menu-前缀
    selectedKeys.value = [result.parent ? result.menu.path : `menu-${result.menu.path}`]
  } else {
    selectedKeys.value = []
  }
}

// 处理菜单展开/收起
const handleOpenChange = (keys: Key[]) => {
  const latestOpenKey = keys.find(key => !openKeys.value.includes(key))
  if (latestOpenKey) {
    // 如果是顶级菜单，只保留最新打开的菜单
    if (rootSubmenuKeys.value.includes(latestOpenKey.toString())) {
      openKeys.value = [latestOpenKey]
    } else {
      // 如果是子菜单，保持当前状态
      openKeys.value = keys
    }
  } else {
    // 如果是关闭操作，直接更新状态
    openKeys.value = keys
  }
}

// 监听路由变化，更新菜单状态
watch(() => route.path, (path) => {
  initMenuState()
  // 找到当前路径对应的父菜单
  const parent = menuList.value.find(menu =>
    menu.children?.some(child => child.path === path)
  )
  if (parent) {
    openKeys.value = [`parent-${parent.path}`]
  }
}, { immediate: true })

// 处理菜单点击
const handleMenuClick = async (menu: LeanMenuDto | StaticMenuItem) => {
  if (menu.path) {
    if (menu.children && menu.children.length > 0) {
      await router.push(menu.children[0].path)
    } else {
      await router.push(menu.path)
    }
  }
}
</script>

<style lang="less" scoped>
.side-menu {
  height: 100vh;
  position: fixed;
  left: 0;
  top: 0;
  bottom: 0;
  z-index: 100;

  .logo {
    height: 32px;
    margin: 16px;
    display: flex;
    align-items: center;
    justify-content: center;

    img {
      height: 32px;
      margin-right: 8px;
    }

    h1 {
      margin: 0;
      font-size: 18px;
      font-weight: 600;
      white-space: nowrap;
    }
  }

  :deep(.ant-menu) {
    border-inline-end: none;
  }

  :deep(.ant-menu-item) {
    margin: 4px 8px !important;
    border-radius: 4px;

    .anticon {
      font-size: 16px;
      margin-right: 10px;
    }
  }

  :deep(.ant-menu-submenu) {
    .ant-menu-submenu-title {
      margin: 4px 8px !important;
      border-radius: 4px;

      .anticon {
        font-size: 16px;
        margin-right: 10px;
      }
    }

    .ant-menu-sub {
      .ant-menu-item {
        padding-left: 48px !important;
        margin: 4px 8px !important;
      }
    }
  }
}
</style>