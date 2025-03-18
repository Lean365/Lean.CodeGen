<template>
  <a-dropdown>
    <a-button type="text" class="action-button">
      <FontSizeOutlined />
    </a-button>
    <template #overlay>
      <a-menu @click="handleFontSizeChange">
        <a-menu-item key="small">小号</a-menu-item>
        <a-menu-item key="medium">中号</a-menu-item>
        <a-menu-item key="large">大号</a-menu-item>
      </a-menu>
    </template>
  </a-dropdown>
</template>

<script setup lang="ts">
import { FontSizeOutlined } from '@ant-design/icons-vue'

const emit = defineEmits(['change'])

const handleFontSizeChange = (e: any) => {
  emit('change', e.key)
}
</script>

<style lang="less" scoped>
.action-button {
  display: flex;
  align-items: center;
  justify-content: center;
  width: 40px;
  height: 40px;
}
</style>