<template>
  <a-tooltip :title="t('header.language')" placement="left">
    <a-dropdown>
      <a-button type="text">
        <template #icon>
          <transition name="fade" mode="out-in">
            <TranslationOutlined />
          </transition>
        </template>
      </a-button>
      <template #overlay>
        <transition name="zoom">
          <a-menu @click="handleLocaleChange" v-model:selectedKeys="selectedKeys">
            <a-menu-item key="zh-CN">简体中文</a-menu-item>
            <a-menu-item key="zh-TW">繁體中文</a-menu-item>
            <a-menu-item key="en-US">English</a-menu-item>
            <a-menu-item key="ja-JP">日本語</a-menu-item>
            <a-menu-item key="ko-KR">한국어</a-menu-item>
            <a-menu-item key="fr-FR">Français</a-menu-item>
            <a-menu-item key="es-ES">Español</a-menu-item>
            <a-menu-item key="ar-EG">العربية</a-menu-item>
            <a-menu-item key="ru-RU">Русский</a-menu-item>
          </a-menu>
        </transition>
      </template>
    </a-dropdown>
  </a-tooltip>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { TranslationOutlined } from '@ant-design/icons-vue'
import { useI18n } from 'vue-i18n'
import { useAppStore } from '@/stores/app'
import type { MenuInfo } from 'ant-design-vue/es/menu/src/interface'
import type { Locale } from '@/stores/app'

const { t } = useI18n()
const appStore = useAppStore()

const selectedKeys = computed(() => [appStore.currentLocale])

const handleLocaleChange = (info: MenuInfo) => {
  const key = info.key as Locale
  appStore.setLocale(key)
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

.zoom-enter-active,
.zoom-leave-active {
  transition: all 0.2s cubic-bezier(0.645, 0.045, 0.355, 1);
}

.zoom-enter-from,
.zoom-leave-to {
  opacity: 0;
  transform: scale(0.8);
}
</style>