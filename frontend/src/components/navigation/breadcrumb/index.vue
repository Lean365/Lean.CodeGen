<template>
  <a-breadcrumb class="breadcrumb">
    <a-breadcrumb-item v-for="item in breadcrumbItems" :key="item.path">
      <router-link v-if="item.path && !item.isLast" :to="item.path">
        <component :is="item.icon || 'HomeOutlined'" class="breadcrumb-icon" />
        <span>{{ t(item.title) }}</span>
      </router-link>
      <span v-else>
        <component :is="item.icon || 'HomeOutlined'" class="breadcrumb-icon" />
        <span>{{ t(item.title) }}</span>
      </span>
    </a-breadcrumb-item>
  </a-breadcrumb>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { useRoute } from 'vue-router'
import { useI18n } from 'vue-i18n'

const { t } = useI18n()
const route = useRoute()

interface BreadcrumbItem {
  title: string
  path?: string
  isLast?: boolean
  icon?: string
}

const breadcrumbItems = computed<BreadcrumbItem[]>(() => {
  const { matched } = route
  return matched.map((item, index) => ({
    title: item.meta?.title as string || '',
    path: item.path,
    isLast: index === matched.length - 1,
    icon: item.meta?.icon || 'HomeOutlined'
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
    color: rgba(0, 0, 0, 0.45);
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
      color: rgba(0, 0, 0, 0.45);
      transition: color 0.3s;
      display: inline-flex;
      align-items: center;
      height: inherit;
      line-height: inherit;

      &:hover {
        color: var(--ant-primary-color);
      }
    }

    span {
      color: rgba(0, 0, 0, 0.85);
      font-weight: 500;
      display: inline-flex;
      align-items: center;
      height: inherit;
      line-height: inherit;
    }
  }
}
</style>