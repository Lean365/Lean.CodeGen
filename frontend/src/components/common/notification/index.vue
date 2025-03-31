<template>
  <a-dropdown>
    <a-button type="text" class="action-button">
      <BellOutlined />
      <a-badge :count="unreadCount" :dot="unreadCount > 0" />
    </a-button>
    <template #overlay>
      <a-menu>
        <a-menu-item key="all">
          <a-list :data-source="notifications" style="width: 300px; max-height: 400px; overflow-y: auto;">
            <template #renderItem="{ item }">
              <a-list-item>
                <a-list-item-meta>
                  <template #title>{{ item.title }}</template>
                  <template #description>{{ item.content }}</template>
                </a-list-item-meta>
                <template #extra>
                  <small>{{ formatTime(item.createTime) }}</small>
                </template>
              </a-list-item>
            </template>
          </a-list>
        </a-menu-item>
        <a-menu-divider />
        <a-menu-item key="viewAll">
          <a-button type="link" block>查看全部通知</a-button>
        </a-menu-item>
      </a-menu>
    </template>
  </a-dropdown>
</template>

<script setup lang="ts">
import { ref, onMounted, onUnmounted } from 'vue'
import { BellOutlined } from '@ant-design/icons-vue'
import { useI18n } from 'vue-i18n'
import dayjs from 'dayjs'
import { HubConnectionBuilder, LogLevel, HttpTransportType } from '@microsoft/signalr'

interface Notification {
  title: string
  content: string
  createTime: string
}

const { t } = useI18n()
const notifications = ref<Notification[]>([])
const unreadCount = ref(0)

// 格式化时间
const formatTime = (time: string) => {
  return dayjs(time).format('YYYY-MM-DD HH:mm:ss')
}

// 初始化 SignalR 连接
const initSignalR = async () => {
  try {
    const token = localStorage.getItem('token')
    if (!token) {
      console.warn('未找到认证token，无法建立实时通信连接')
      return
    }

    const connection = new HubConnectionBuilder()
      .withUrl('/signalr/hubs', {
        accessTokenFactory: () => token,
        withCredentials: true,
        skipNegotiation: true,
        transport: HttpTransportType.WebSockets
      })
      .configureLogging(LogLevel.Information)
      .build()

    connection.on('ReceiveNotification', (notification) => {
      notifications.value.unshift(notification)
      unreadCount.value++
    })

    await connection.start()
    console.log('通知 SignalR 连接成功')
  } catch (error) {
    console.error('SignalR connection error:', error)
  }
}

// 监听 token 变化
const watchToken = () => {
  const token = localStorage.getItem('token')
  if (token) {
    initSignalR()
  }
}

onMounted(() => {
  // 如果已经有 token，直接初始化
  if (localStorage.getItem('token')) {
    initSignalR()
  }
  // 监听 storage 事件
  window.addEventListener('storage', watchToken)
})

onUnmounted(() => {
  window.removeEventListener('storage', watchToken)
})
</script>

<style lang="less" scoped>
.action-button {
  display: flex;
  align-items: center;
  justify-content: center;
  width: 40px;
  height: 40px;
  color: var(--color-text);
}
</style>