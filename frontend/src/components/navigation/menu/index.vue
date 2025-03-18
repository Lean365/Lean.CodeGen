<template>
  <a-menu v-model:selectedKeys="selectedKeys" v-model:openKeys="openKeys" mode="inline" theme="dark">
    <template v-for="menu in menus" :key="menu.key">
      <!-- 有子菜单的情况 -->
      <template v-if="menu.children && menu.children.length">
        <a-sub-menu :key="menu.key">
          <template #icon>
            <component :is="menu.icon" />
          </template>
          <template #title>{{ t(menu.title) }}</template>
          <a-menu-item v-for="child in menu.children" :key="child.key" @click="handleMenuClick(child)">
            <template #icon>
              <component :is="child.icon" />
            </template>
            {{ t(child.title) }}
          </a-menu-item>
        </a-sub-menu>
      </template>

      <!-- 没有子菜单的情况 -->
      <template v-else>
        <a-menu-item :key="menu.key" @click="handleMenuClick(menu)">
          <template #icon>
            <component :is="menu.icon" />
          </template>
          {{ t(menu.title) }}
        </a-menu-item>
      </template>
    </template>
  </a-menu>
</template>

<script setup lang="ts">
import { ref, watch } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import { useI18n } from 'vue-i18n'

const { t } = useI18n()
const router = useRouter()
const route = useRoute()

interface MenuItem {
  key: string
  title: string
  icon?: any
  path?: string
  children?: MenuItem[]
}

const menus: MenuItem[] = [
  {
    key: 'dashboard',
    title: 'menu.dashboard.title',
    icon: 'DashboardOutlined',
    children: [
      {
        key: 'dashboard',
        title: 'menu.dashboard.index',
        path: '/dashboard',
        icon: 'HomeOutlined'
      },
      {
        key: 'dashboard.analysis',
        title: 'menu.dashboard.analysis',
        path: '/dashboard/analysis',
        icon: 'BarChartOutlined'
      },
      {
        key: 'dashboard.monitor',
        title: 'menu.dashboard.monitor',
        path: '/dashboard/monitor',
        icon: 'MonitorOutlined'
      }
    ]
  },
  {
    key: 'about',
    title: 'menu.about.title',
    icon: 'InfoCircleOutlined',
    children: [
      {
        key: 'about',
        title: 'menu.about.index',
        path: '/about',
        icon: 'InfoCircleOutlined'
      },
      {
        key: 'about.terms',
        title: 'menu.about.terms',
        path: '/about/terms',
        icon: 'FileTextOutlined'
      },
      {
        key: 'about.privacy',
        title: 'menu.about.privacy',
        path: '/about/privacy',
        icon: 'SafetyOutlined'
      }
    ]
  }
]

const selectedKeys = ref<string[]>([])
const openKeys = ref<string[]>([])

// 根据当前路由更新选中的菜单项
const updateSelectedKeys = () => {
  const currentRoute = route.name as string
  if (currentRoute) {
    selectedKeys.value = [currentRoute]
    // 更新展开的子菜单
    const parentKey = currentRoute.split('.')[0]
    if (parentKey && parentKey !== currentRoute) {
      openKeys.value = [parentKey]
    }
  }
}

// 监听路由变化
watch(() => route.name, updateSelectedKeys, { immediate: true })

const handleMenuClick = (menu: MenuItem) => {
  if (menu.path) {
    router.push(menu.path)
  }
}
</script>

<style lang="less" scoped>
:deep(.ant-menu-item) {
  margin-top: 4px !important;
}

:deep(.ant-menu-submenu) {
  .ant-menu-submenu-title {
    margin-top: 4px !important;
  }
}
</style>