<template>
  <a-button type="text" class="action-button" @click="toggleFullscreen">
    <FullscreenOutlined v-if="!isFullscreen" />
    <FullscreenExitOutlined v-else />
  </a-button>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { FullscreenOutlined, FullscreenExitOutlined } from '@ant-design/icons-vue'

const isFullscreen = ref(false)

const toggleFullscreen = () => {
  if (!document.fullscreenElement) {
    document.documentElement.requestFullscreen()
    isFullscreen.value = true
  } else {
    if (document.exitFullscreen) {
      document.exitFullscreen()
      isFullscreen.value = false
    }
  }
}
</script>

<style lang="less" scoped>
.action-button {
  display: flex;
  align-items: center;
  justify-content: center;
  width: 40px;
  height: 40px;
  color: var(--color-text);

  :deep(.anticon) {
    font-size: 16px;
  }
}
</style>