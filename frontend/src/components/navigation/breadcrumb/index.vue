<template>
  <a-breadcrumb class="breadcrumb">
    <a-breadcrumb-item>
      <router-link to="/dashboard/index">
        <HomeOutlined class="breadcrumb-icon" />
        <span>{{ t('menu.dashboard.index') }}</span>
      </router-link>
    </a-breadcrumb-item>
    <a-breadcrumb-item v-for="item in breadcrumbItems" :key="item.path">
      <router-link v-if="item.path && !item.isLast" :to="item.path">
        <component :is="item.icon" class="breadcrumb-icon" />
        <span>{{ item.transKey ? t(item.transKey) : item.title }}</span>
      </router-link>
      <span v-else>
        <component :is="item.icon" class="breadcrumb-icon" />
        <span>{{ item.transKey ? t(item.transKey) : item.title }}</span>
      </span>
    </a-breadcrumb-item>
  </a-breadcrumb>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { useRoute } from 'vue-router'
import { useI18n } from 'vue-i18n'
import { HomeOutlined } from '@ant-design/icons-vue'

const { t } = useI18n()
const route = useRoute()

interface BreadcrumbItem {
  title: string
  path?: string
  isLast?: boolean
  icon?: string
  transKey?: string
}

const breadcrumbItems = computed(() => {
  const { matched } = route
  return matched
    .filter(item => item.path !== '/' && item.path !== '/dashboard/index')
    .map((item, index, filteredMatched) => ({
      title: item.meta?.title as string || '',
      path: item.path,
      isLast: index === filteredMatched.length - 1,
      icon: item.meta?.icon as string,
      transKey: item.meta?.transKey as string
    }))
})
</script>

<style lang="less" scoped>
.breadcrumb {
  margin: 16px 0;
  padding: 0 24px;
  line-height: 1.5715;
  display: flex;
  align-items: center;

  :deep(.ant-breadcrumb-separator) {
    margin: 0 8px;
    display: inline-flex;
    align-items: center;
    height: 22px;
    line-height: 22px;
  }

  :deep(.ant-breadcrumb-link) {
    display: inline-flex;
    align-items: center;
    height: 22px;
    line-height: 22px;

    .breadcrumb-icon {
      margin-right: 4px;
      font-size: 14px;
      display: inline-flex;
      align-items: center;
      height: inherit;
    }

    a {
      display: inline-flex;
      align-items: center;
      height: inherit;
      line-height: inherit;
    }

    span {
      font-weight: 500;
      display: inline-flex;
      align-items: center;
      height: inherit;
      line-height: inherit;
    }
  }
}
</style>