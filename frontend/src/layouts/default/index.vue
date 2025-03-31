<template>
  <a-layout class="layout-container">
    <!-- 侧边栏 -->
    <a-layout-sider v-model:collapsed="collapsed" :trigger="null" collapsible class="layout-sider">
      <div class="logo">
        <img src="@/assets/images/logo/logo.svg" alt="logo" />
        <h1 v-show="!collapsed">{{ t('app.name') }}</h1>
      </div>
      <Sidemenu />
    </a-layout-sider>

    <a-layout>
      <!-- 顶部导航 -->
      <HbtHeader @collapse="handleCollapse" />

      <!-- 内容区域 -->
      <a-layout-content>
        <!-- 面包屑导航 -->
        <div class="content-header">
          <HbtBreadcrumb />
        </div>

        <!-- 主要内容 -->
        <div class="content-main">
          <router-view v-slot="{ Component }">
            <transition name="fade" mode="out-in">
              <component :is="Component" />
            </transition>
          </router-view>
        </div>
      </a-layout-content>

      <!-- 页脚 -->
      <a-layout-footer class="layout-footer">
        <HbtFooter />
      </a-layout-footer>
    </a-layout>
  </a-layout>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useI18n } from 'vue-i18n'
import HbtHeader from '@/components/navigation/header/index.vue'
import Sidemenu from '@/components/navigation/sidemenu/index.vue'
import HbtFooter from '@/components/navigation/footer/index.vue'
import HbtBreadcrumb from '@/components/navigation/breadcrumb/index.vue'

const { t } = useI18n()
const collapsed = ref(false)

const handleCollapse = (value: boolean) => {
  collapsed.value = value
}
</script>

<style lang="less" scoped>
.layout-container {
  height: 100vh;
  display: flex;
  overflow: hidden;

  .layout-sider {
    height: 100vh;
    overflow: hidden;
    display: flex;
    flex-direction: column;

    .logo {
      height: 64px;
      padding: 16px;
      display: flex;
      align-items: center;
      overflow: hidden;
      flex-shrink: 0;

      img {
        width: 32px;
        height: 32px;
      }

      h1 {
        margin: 0 0 0 12px;
        font-weight: 600;
        font-size: 18px;
        line-height: 32px;
        white-space: nowrap;
        overflow: hidden;
      }
    }

    :deep(.ant-menu) {
      flex: 1;
      overflow-y: auto;
      overflow-x: hidden;
    }
  }

  .ant-layout {
    height: 100vh;
    display: flex;
    flex-direction: column;
    overflow: hidden;

    .ant-layout-header {
      flex-shrink: 0;
    }

    .ant-layout-content {
      flex: 1;
      overflow: auto;
      padding: 16px 24px;
      background: var(--color-bg-container);

      .content-header {
        margin-bottom: 8px;
        height: 32px;
        line-height: 32px;
      }

      .content-main {
        background: var(--color-bg-container);
        padding: 16px;
        border-radius: 4px;
        min-height: 280px;
      }
    }

    .ant-layout-footer {
      flex-shrink: 0;
      text-align: center;
      padding: 16px 50px;
    }
  }
}

// 页面切换动画
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.3s ease;
}

.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}
</style>