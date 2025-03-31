<template>
  <a-layout-header class="header" :style="{ background: 'var(--color-bg-container)' }">
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
import { useUserStore } from '@/stores/user'
import { useOnlineUserStore } from '@/stores/signalr/onlineUser'
import { useOnlineMessageStore } from '@/stores/signalr/onlineMessage'
import { useMessageStore } from '@/stores/message'
import { useI18n } from 'vue-i18n'
import { message } from 'ant-design-vue'

const router = useRouter()
const appStore = useAppStore()
const userStore = useUserStore()
const onlineUserStore = useOnlineUserStore()
const onlineMessageStore = useOnlineMessageStore()
const messageStore = useMessageStore()
const isCollapsed = ref(false)
const { t } = useI18n()

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

const handleLogout = async () => {
  try {
    // 调用登出方法
    await userStore.logout()

    // 登出成功后清理其他状态
    onlineUserStore.clearOnlineUsers()
    onlineMessageStore.clearMessages()
    messageStore.clearMessages()

    // 跳转到登录页
    router.push('/login')
  } catch (error) {
    console.error('登出失败:', error)
    message.error('登出失败，请重试')
  }
}

const emit = defineEmits(['collapse'])
</script>

<style lang="less" scoped>
.header {
  padding: 0;
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