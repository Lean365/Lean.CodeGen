<template>
  <a-dropdown>
    <a-button type="text" class="action-button">
      <a-avatar :size="28" :src="userStore.avatar" class="avatar">
        <template #icon>
          <UserOutlined />
        </template>
      </a-avatar>
    </a-button>
    <template #overlay>
      <a-menu>
        <a-menu-item key="profile" @click="handleProfile">
          <template #icon>
            <UserOutlined />
          </template>
          个人中心
        </a-menu-item>
        <a-menu-item key="settings" @click="handleSettings">
          <template #icon>
            <SettingOutlined />
          </template>
          设置
        </a-menu-item>
        <a-menu-divider />
        <a-menu-item key="logout" @click="handleLogout">
          <template #icon>
            <LogoutOutlined />
          </template>
          退出登录
        </a-menu-item>
      </a-menu>
    </template>
  </a-dropdown>
</template>

<script setup lang="ts">
import { UserOutlined, SettingOutlined, LogoutOutlined } from '@ant-design/icons-vue'
import { useRouter } from 'vue-router'
import { useUserStore } from '@/stores/user'

const router = useRouter()
const userStore = useUserStore()
const emit = defineEmits(['profile', 'settings', 'logout'])

const handleProfile = () => {
  emit('profile')
  router.push('/user/profile')
}

const handleSettings = () => {
  emit('settings')
  router.push('/user/settings')
}

const handleLogout = () => {
  emit('logout')
}
</script>

<style lang="less" scoped>
.action-button {
  display: flex;
  align-items: center;
  justify-content: center;
  width: 48px;
  height: 48px;
  padding: 0;

  .avatar {
    &:deep(.ant-avatar-string) {
      color: #fff;
      line-height: 28px;
    }
  }

  &:hover {
    background-color: rgba(0, 0, 0, 0.025);
  }
}

:deep(.ant-dropdown-menu-item) {
  min-width: 160px;
}
</style>