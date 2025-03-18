<!-- App.vue -->
<template>
  <a-config-provider :locale="antdLocale" :theme="{
    algorithm: currentTheme === 'dark' ? darkAlgorithm : defaultAlgorithm,
    ...skinStore.currentTheme
  }">
    <router-view></router-view>
  </a-config-provider>
</template>

<script setup lang="ts">
import { computed } from 'vue'
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
  switch (locale.value) {
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
</script>

<style>
#app {
  width: 100%;
  height: 100%;
}
</style>
