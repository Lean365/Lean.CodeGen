<template>
  <a-tooltip :title="t('header.theme')" placement="left">
    <a-button type="text" @click="handleThemeChange">
      <template #icon>
        <transition name="fade" mode="out-in">
          <BulbOutlined v-if="!isDark" />
          <BulbFilled v-else />
        </transition>
      </template>
    </a-button>
  </a-tooltip>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { BulbOutlined, BulbFilled } from '@ant-design/icons-vue'
import { useI18n } from 'vue-i18n'
import { useAppStore } from '@/stores/app'

const { t } = useI18n()
const appStore = useAppStore()

const isDark = computed(() => appStore.isDark)

const handleThemeChange = () => {
  appStore.toggleTheme()
}
</script>

<style scoped>
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.2s ease;
}

.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}
</style>