<template>
  <div class="top-menu">
    <a-menu v-model:selectedKeys="selectedKeys" mode="horizontal" :theme="theme" class="main-menu">
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

    <!-- 移动端菜单按钮 -->
    <a-button v-if="isMobile" class="mobile-menu-btn" type="text" @click="showDrawer = true">
      <template #icon>
        <MenuOutlined />
      </template>
    </a-button>

    <!-- 移动端抽屉菜单 -->
    <a-drawer v-model:visible="showDrawer" :title="t('app.name')" placement="left" class="mobile-drawer">
      <a-menu v-model:selectedKeys="selectedKeys" mode="inline" :theme="theme">
        <template v-for="menu in menus" :key="menu.key">
          <template v-if="menu.children && menu.children.length > 0">
            <a-sub-menu :key="menu.key">
              <template #icon>
                <component :is="menu.icon" />
              </template>
              <template #title>{{ t(menu.title) }}</template>
              <a-menu-item v-for="child in menu.children" :key="child.key" @click="handleMobileMenuClick(child)">
                <template #icon>
                  <component :is="child.icon" />
                </template>
                {{ t(child.title) }}
              </a-menu-item>
            </a-sub-menu>
          </template>
          <template v-else>
            <a-menu-item :key="menu.key" @click="handleMobileMenuClick(menu)">
              <template #icon>
                <component :is="menu.icon" />
              </template>
              {{ t(menu.title) }}
            </a-menu-item>
          </template>
        </template>
      </a-menu>
    </a-drawer>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted, onUnmounted } from 'vue'
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
  InfoCircleOutlined,
  FileTextOutlined,
  SafetyOutlined
} from '@ant-design/icons-vue'

const { t } = useI18n()
const router = useRouter()
const route = useRoute()
const appStore = useAppStore()

const selectedKeys = ref<string[]>([route.name as string])
const showDrawer = ref(false)
const isMobile = ref(false)

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
    title: 'menu.dashboard',
    icon: DashboardOutlined,
    path: '/dashboard'
  },
  {
    key: 'system',
    title: 'menu.system',
    icon: SettingOutlined,
    children: [
      {
        key: 'user',
        title: 'menu.user',
        icon: UserOutlined,
        path: '/system/user'
      }
    ]
  },
  {
    key: 'generator',
    title: 'menu.generator',
    icon: CodeOutlined,
    path: '/generator'
  },
  {
    key: 'security',
    title: 'menu.security',
    icon: SafetyCertificateOutlined,
    path: '/security'
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

const handleMobileMenuClick = (menu: MenuItem) => {
  handleMenuClick(menu)
  showDrawer.value = false
}

// 响应式处理
const checkMobile = () => {
  isMobile.value = window.innerWidth < 768
}

onMounted(() => {
  checkMobile()
  window.addEventListener('resize', checkMobile)
})

onUnmounted(() => {
  window.removeEventListener('resize', checkMobile)
})
</script>

<style lang="less" scoped>
.top-menu {
  display: flex;
  align-items: center;
  justify-content: space-between;

  .main-menu {
    flex: 1;

    @media (max-width: 767px) {
      display: none;
    }
  }

  .mobile-menu-btn {
    display: none;
    margin-right: 16px;

    @media (max-width: 767px) {
      display: block;
    }
  }
}

.mobile-drawer {
  :deep(.ant-drawer-body) {
    padding: 0;
  }
}
</style>