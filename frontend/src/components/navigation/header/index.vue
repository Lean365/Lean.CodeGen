<template>
  <a-layout-header class="header">
    <div class="header-content">
      <div class="left-section">
        <a-button type="text" class="action-button" @click="toggleCollapsed">
          <MenuFoldOutlined v-if="isCollapsed" />
          <MenuUnfoldOutlined v-else />
        </a-button>
        <a-button type="text" class="action-button" @click="handleRefresh">
          <ReloadOutlined />
        </a-button>
      </div>

      <div class="right-section">
        <a-space>
          <HbtSearch @search="handleSearch" />
          <HbtNotification />
          <HbtFont @change="handleFontChange" />
          <HbtFullscreen @change="handleFullscreen" />
          <HbtLocale />
          <HbtTheme @change="handleThemeChange" />
          <HbtUser @profile="handleProfile" @settings="handleSettings" @logout="handleLogout" />
          <HbtSkin />
        </a-space>
      </div>
    </div>
  </a-layout-header>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import {
  MenuFoldOutlined,
  MenuUnfoldOutlined,
  ReloadOutlined,
} from '@ant-design/icons-vue'
import { useRouter } from 'vue-router'
import { useAppStore } from '@/stores/app'

const router = useRouter()
const appStore = useAppStore()
const isCollapsed = ref(false)

const toggleCollapsed = () => {
  isCollapsed.value = !isCollapsed.value
  emit('collapse', isCollapsed.value)
}

const handleRefresh = () => {
  window.location.reload()
}

const handleSearch = (keyword: string) => {
  // 处理搜索
  console.log('Search keyword:', keyword)
}

const handleFullscreen = (isFullscreen: boolean) => {
  // 处理全屏切换
  console.log('Fullscreen changed:', isFullscreen)
}

const handleFontChange = (size: string) => {
  // 处理字体大小变化
  console.log('Font size changed:', size)
}

const handleThemeChange = () => {
  appStore.toggleTheme()
}

const handleProfile = () => {
  router.push('/user/profile')
}

const handleSettings = () => {
  router.push('/user/settings')
}

const handleLogout = () => {
  router.push('/login')
}

const emit = defineEmits(['collapse'])
</script>

<style lang="less" scoped>
.header {
  padding: 0;
  background: var(--color-bg-container);
  border-bottom: 1px solid var(--color-border);
  z-index: 100;

  .header-content {
    display: flex;
    justify-content: space-between;
    align-items: center;
    height: 100%;
    padding: 0 16px;

    .left-section {
      display: flex;
      align-items: center;
      gap: 8px;
    }

    .right-section {
      display: flex;
      align-items: center;

      :deep(.ant-space) {
        gap: 2px !important;
      }
    }

    .action-button {
      display: flex;
      align-items: center;
      justify-content: center;
      width: 40px;
      height: 40px;
      color: var(--color-text);
    }
  }
}
</style>