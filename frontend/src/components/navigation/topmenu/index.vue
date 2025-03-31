<template>
  <div class="top-menu">
    <a-menu v-model:selectedKeys="selectedKeys" mode="horizontal" :theme="theme" class="main-menu">
      <template v-for="menu in menuList" :key="menu.id">
        <a-sub-menu v-if="menu.children && menu.children.length > 0" :key="`sub-${menu.id}`">
          <template #title>
            <span>
              <component :is="menu.icon" v-if="menu.icon" />
              <span>{{ getMenuTitle(menu) }}</span>
            </span>
          </template>
          <a-menu-item v-for="child in menu.children" :key="child.id" @click="handleMenuClick(child)">
            <component :is="child.icon" v-if="child.icon" />
            <span>{{ getMenuTitle(child) }}</span>
          </a-menu-item>
        </a-sub-menu>
        <a-menu-item v-else :key="`item-${menu.id}`" @click="handleMenuClick(menu)">
          <component :is="menu.icon" v-if="menu.icon" />
          <span>{{ getMenuTitle(menu) }}</span>
        </a-menu-item>
      </template>
    </a-menu>

    <!-- 移动端菜单按钮 -->
    <a-button v-if="isMobile" class="mobile-menu-btn" @click="showDrawer = true">
      <template #icon>
        <MenuOutlined />
      </template>
    </a-button>

    <!-- 移动端抽屉菜单 -->
    <a-drawer v-model:open="showDrawer" :title="t('app.name')" placement="left" class="mobile-drawer">
      <a-menu v-model:selectedKeys="selectedKeys" mode="inline" :theme="theme">
        <template v-for="menu in menuList" :key="menu.id">
          <a-sub-menu v-if="menu.children && menu.children.length > 0" :key="`sub-${menu.id}`">
            <template #title>
              <span>
                <component :is="menu.icon" v-if="menu.icon" />
                <span>{{ getMenuTitle(menu) }}</span>
              </span>
            </template>
            <a-menu-item v-for="child in menu.children" :key="child.id" @click="handleMobileMenuClick(child)">
              <component :is="child.icon" v-if="child.icon" />
              <span>{{ getMenuTitle(child) }}</span>
            </a-menu-item>
          </a-sub-menu>
          <a-menu-item v-else :key="`item-${menu.id}`" @click="handleMobileMenuClick(menu)">
            <component :is="menu.icon" v-if="menu.icon" />
            <span>{{ getMenuTitle(menu) }}</span>
          </a-menu-item>
        </template>
      </a-menu>
    </a-drawer>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted, onUnmounted, watch } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import { useI18n } from 'vue-i18n'
import { useAppStore } from '@/stores/app'
import { useMenuStore } from '@/stores/modules/menu'
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
import type { LeanMenuDto } from '@/types/identity/menu'

const { t } = useI18n()
const router = useRouter()
const route = useRoute()
const appStore = useAppStore()
const menuStore = useMenuStore()

const selectedKeys = ref<(string | number)[]>([])
const showDrawer = ref(false)
const isMobile = ref(false)

const theme = computed<MenuTheme>(() => appStore.isDark ? 'dark' : 'light')

// 获取菜单列表
const menuList = computed(() => menuStore.getMenuList)

// 获取菜单标题
const getMenuTitle = (menu: LeanMenuDto) => {
  try {
    console.log('处理菜单标题:', {
      id: menu.id,
      menuName: menu.menuName,
      transKey: menu.transKey,
      path: menu.path
    });

    if (menu.transKey) {
      const title = t(menu.transKey);
      console.log('翻译结果:', {
        transKey: menu.transKey,
        title: title,
        isDefault: title === menu.transKey
      });

      // 如果翻译结果与翻译键相同，说明没有找到翻译，使用 menuName
      if (title === menu.transKey) {
        console.log('未找到翻译，使用默认菜单名称:', menu.menuName);
        return menu.menuName;
      }

      return title;
    }

    console.log('使用默认菜单名称:', menu.menuName);
    return menu.menuName;
  } catch (error) {
    console.error('菜单翻译错误:', error);
    return menu.menuName;
  }
}

// 处理菜单点击
const handleMenuClick = async (menu: LeanMenuDto) => {
  if (menu.path) {
    await router.push(menu.path)
  }
}

const handleMobileMenuClick = (menu: LeanMenuDto) => {
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

// 监听路由变化，更新选中的菜单项
watch(() => route.path, (newPath) => {
  const currentMenu = menuList.value.find(menu => menu.path === newPath)
  if (currentMenu) {
    selectedKeys.value = [currentMenu.id]
  }
}, { immediate: true })
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

:deep(.ant-menu-item) {
  .anticon {
    font-size: 16px;
    margin-right: 6px;
  }
}

:deep(.ant-menu-submenu) {
  .ant-menu-submenu-title {
    .anticon {
      font-size: 16px;
      margin-right: 6px;
    }
  }
}
</style>