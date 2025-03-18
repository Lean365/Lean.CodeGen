<template>
  <div class="dashboard-container">
    <a-row :gutter="24">
      <!-- 欢迎卡片 -->
      <a-col :span="24">
        <a-card :bordered="false" class="welcome-card">
          <template #title>
            <span>{{ t('dashboard.welcome.title') }}</span>
          </template>
          <a-row>
            <a-col :span="16">
              <div class="welcome-info">
                <h2>{{ t('dashboard.welcome.greeting', { time: getGreetingTime() }) }}</h2>
                <p>{{ t('dashboard.welcome.description') }}</p>
              </div>
            </a-col>
            <a-col :span="8">
              <div class="welcome-image">
                <img src="@/assets/images/welcome.svg" alt="welcome" />
              </div>
            </a-col>
          </a-row>
        </a-card>
      </a-col>

      <!-- 统计卡片 -->
      <a-col :span="6">
        <a-card :bordered="false">
          <statistic :title="t('dashboard.stats.users')" :value="1234" :precision="0" style="margin-right: 50px">
            <template #prefix>
              <UserOutlined />
            </template>
          </statistic>
        </a-card>
      </a-col>
      <a-col :span="6">
        <a-card :bordered="false">
          <statistic :title="t('dashboard.stats.projects')" :value="32" :precision="0">
            <template #prefix>
              <ProjectOutlined />
            </template>
          </statistic>
        </a-card>
      </a-col>
      <a-col :span="6">
        <a-card :bordered="false">
          <statistic :title="t('dashboard.stats.tasks')" :value="156" :precision="0">
            <template #prefix>
              <CheckCircleOutlined />
            </template>
          </statistic>
        </a-card>
      </a-col>
      <a-col :span="6">
        <a-card :bordered="false">
          <statistic :title="t('dashboard.stats.messages')" :value="45" :precision="0">
            <template #prefix>
              <MessageOutlined />
            </template>
          </statistic>
        </a-card>
      </a-col>
    </a-row>
  </div>
</template>

<script setup lang="ts">
import { useI18n } from 'vue-i18n'
import { Statistic } from 'ant-design-vue'
import {
  UserOutlined,
  ProjectOutlined,
  CheckCircleOutlined,
  MessageOutlined
} from '@ant-design/icons-vue'

const { t } = useI18n()

const getGreetingTime = () => {
  const hour = new Date().getHours()
  if (hour < 12) {
    return t('dashboard.welcome.morning')
  } else if (hour < 18) {
    return t('dashboard.welcome.afternoon')
  } else {
    return t('dashboard.welcome.evening')
  }
}
</script>

<style lang="less" scoped>
.dashboard-container {
  padding: 24px;

  .welcome-card {
    margin-bottom: 24px;

    .welcome-info {
      h2 {
        margin-bottom: 12px;
        color: rgba(0, 0, 0, 0.85);
        font-weight: 500;
      }

      p {
        color: rgba(0, 0, 0, 0.45);
        font-size: 14px;
      }
    }

    .welcome-image {
      text-align: right;

      img {
        max-width: 100%;
        height: auto;
      }
    }
  }

  :deep(.ant-card) {
    border-radius: 2px;
  }
}
</style>