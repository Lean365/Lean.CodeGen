<template>
  <a-layout-sider v-model:collapsed="collapsed" :trigger="null" collapsible class="side-menu">
    <div class="logo">
      <img src="@/assets/images/logo/logo.svg" alt="logo" />
      <h1 v-show="!collapsed">{{ t('app.name') }}</h1>
    </div>
    <a-menu v-model:selectedKeys="selectedKeys" v-model:openKeys="openKeys" mode="inline" :theme="theme">
      <template v-for="menu in menus" :key="menu.key">
        <template v-if="menu.children && menu.children.length > 0">
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
  </a-layout-sider>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import { useI18n } from 'vue-i18n'
import { useAppStore } from '@/stores/app'
import type { MenuTheme } from 'ant-design-vue'
import {
  DashboardOutlined,
  UserOutlined,
  SettingOutlined,
  CodeOutlined,
  SafetyCertificateOutlined,
  MenuOutlined,
  BookOutlined,
  ToolOutlined,
  ApiOutlined,
  InfoCircleOutlined,
  FileTextOutlined,
  SafetyOutlined
} from '@ant-design/icons-vue'

const { t } = useI18n()
const router = useRouter()
const route = useRoute()
const appStore = useAppStore()

const collapsed = ref(false)
const selectedKeys = ref<string[]>([route.name as string])
const openKeys = ref<string[]>([])

const theme = computed<MenuTheme>(() => appStore.isDark ? 'dark' : 'light')

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
    icon: DashboardOutlined,
    children: [
      {
        key: 'dashboard.analysis',
        title: 'menu.dashboard.analysis',
        icon: DashboardOutlined,
        path: '/dashboard/analysis'
      },
      {
        key: 'dashboard.monitor',
        title: 'menu.dashboard.monitor',
        icon: DashboardOutlined,
        path: '/dashboard/monitor'
      }
    ]
  },
  {
    key: 'identity',
    title: 'menu.identity.title',
    icon: SafetyCertificateOutlined,
    children: [
      {
        key: 'identity.user',
        title: 'menu.identity.user.title',
        icon: UserOutlined,
        path: '/identity/user'
      }
    ]
  },
  {
    key: 'system',
    title: 'menu.system.title',
    icon: SettingOutlined,
    children: [
      {
        key: 'system.menu',
        title: 'menu.system.menu.title',
        icon: MenuOutlined,
        path: '/system/menu'
      },
      {
        key: 'system.dict',
        title: 'menu.system.dict.title',
        icon: BookOutlined,
        path: '/system/dict'
      }
    ]
  },
  {
    key: 'tools',
    title: 'menu.tools.title',
    icon: ToolOutlined,
    children: [
      {
        key: 'tools.codegen',
        title: 'menu.tools.codegen.title',
        icon: CodeOutlined,
        path: '/tools/codegen'
      },
      {
        key: 'tools.swagger',
        title: 'menu.tools.swagger',
        icon: ApiOutlined,
        path: '/tools/swagger'
      }
    ]
  },
  {
    key: 'about',
    title: 'menu.about.title',
    icon: InfoCircleOutlined,
    children: [
      {
        key: 'about',
        title: 'menu.about.index',
        icon: InfoCircleOutlined,
        path: '/about'
      },
      {
        key: 'about.terms',
        title: 'menu.about.terms',
        icon: FileTextOutlined,
        path: '/about/terms'
      },
      {
        key: 'about.privacy',
        title: 'menu.about.privacy',
        icon: SafetyOutlined,
        path: '/about/privacy'
      }
    ]
  }
]

const handleMenuClick = (menu: MenuItem) => {
  if (menu.path) {
    router.push(menu.path)
  }
}
</script>

<style lang="less" scoped>
.side-menu {
  .logo {
    height: 64px;
    padding: 16px;
    display: flex;
    align-items: center;

    img {
      width: 32px;
      height: 32px;
    }

    h1 {
      margin: 0 0 0 12px;
      font-weight: 600;
      font-size: 18px;
      line-height: 32px;
      white-space: nowrap;
    }
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