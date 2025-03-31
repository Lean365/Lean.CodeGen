<template>
  <a-modal v-model:open="visible" title="用户详情" :footer="null" width="600px">
    <a-descriptions bordered>
      <a-descriptions-item label="用户ID">{{ user?.userId }}</a-descriptions-item>
      <a-descriptions-item label="用户名">{{ user?.userName }}</a-descriptions-item>
      <a-descriptions-item label="状态">
        <a-tag :color="user?.isOnline ? 'green' : 'red'">
          {{ user?.isOnline ? '在线' : '离线' }}
        </a-tag>
      </a-descriptions-item>
      <a-descriptions-item label="最后活动时间">
        {{ user?.lastActiveTime }}
      </a-descriptions-item>
      <a-descriptions-item label="连接ID">
        {{ user?.connectionId }}
      </a-descriptions-item>
    </a-descriptions>
  </a-modal>
</template>

<script lang="ts" setup>
import type { LeanOnlineUser } from '@/types/signalr/onlineUser';

defineProps<{
  visible: boolean;
  user?: LeanOnlineUser;
}>();

defineEmits<{
  (e: 'update:visible', visible: boolean): void;
}>();
</script>