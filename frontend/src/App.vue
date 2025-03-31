<!-- App.vue -->
<template>
  <a-config-provider :theme="themeConfig" :locale="antdLocale">
    <router-view></router-view>
  </a-config-provider>
</template>

<script setup lang="ts">
import { computed, watch } from 'vue'
import { RouterView } from 'vue-router'
import { ConfigProvider, theme } from 'ant-design-vue'
import { useAppStore } from '@/stores/app'
import { useSkinStore } from '@/stores/skin'
import { useI18n } from 'vue-i18n'
// Ant Design Vue 语言包
import zhCN from 'ant-design-vue/es/locale/zh_CN'
import zhTW from 'ant-design-vue/es/locale/zh_TW'
import enUS from 'ant-design-vue/es/locale/en_US'
import jaJP from 'ant-design-vue/es/locale/ja_JP'
import koKR from 'ant-design-vue/es/locale/ko_KR'
import frFR from 'ant-design-vue/es/locale/fr_FR'
import esES from 'ant-design-vue/es/locale/es_ES'
import arEG from 'ant-design-vue/es/locale/ar_EG'
import ruRU from 'ant-design-vue/es/locale/ru_RU'

const { darkAlgorithm, defaultAlgorithm } = theme
const appStore = useAppStore()
const skinStore = useSkinStore()
const { locale } = useI18n()

const currentTheme = computed(() => appStore.theme)

const antdLocale = computed(() => {
  switch (appStore.locale) {
    // 中文（简体）
    case 'zh-CN':
      return zhCN
    // 中文（繁体）
    case 'zh-TW':
      return zhTW
    // 英语
    case 'en-US':
      return enUS
    // 日语
    case 'ja-JP':
      return jaJP
    // 韩语
    case 'ko-KR':
      return koKR
    // 法语（联合国官方语言）
    case 'fr-FR':
      return frFR
    // 西班牙语（联合国官方语言）
    case 'es-ES':
      return esES
    // 阿拉伯语（联合国官方语言）
    case 'ar-EG':
      return arEG
    // 俄语（联合国官方语言）
    case 'ru-RU':
      return ruRU
    // 默认使用英语
    default:
      return enUS
  }
})

const themeConfig = computed(() => {
  const baseTheme = skinStore.currentTheme
  const algorithm = appStore.isDark ? theme.darkAlgorithm : theme.defaultAlgorithm
  return {
    algorithm,
    token: {
      ...baseTheme.token,
      borderRadius: 4,
      fontSize: 14,
      colorBgContainer: appStore.isDark ? '#141414' : '#ffffff',
      colorBgElevated: appStore.isDark ? '#1f1f1f' : '#ffffff',
      colorBorder: appStore.isDark ? '#303030' : '#f0f0f0',
      colorText: appStore.isDark ? '#ffffff' : '#000000',
      colorTextSecondary: appStore.isDark ? 'rgba(255, 255, 255, 0.85)' : 'rgba(0, 0, 0, 0.85)'
    }
  }
})

// 监听语言变化
watch(() => appStore.locale, (newLocale) => {
  locale.value = newLocale
})
</script>

<style>
#app {
  width: 100%;
  height: 100%;
}
</style>
