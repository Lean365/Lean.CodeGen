<template>
  <div class="about-container">
    <a-row :gutter="[24, 24]">
      <a-col :span="24">
        <a-card>
          <template #title>{{ t('about.description.title') }}</template>
          <div class="company-info">
            <img src="@/assets/images/logo/logo.svg" alt="logo" class="logo" />
            <h1>{{ t('app.name') }}</h1>
            <p>{{ t('app.description') }}</p>
          </div>
        </a-card>
      </a-col>

      <a-col :span="12">
        <a-card>
          <template #title>{{ t('about.version.title') }}</template>
          <a-descriptions :column="1">
            <a-descriptions-item :label="t('about.version.current')">v1.0.0</a-descriptions-item>
            <a-descriptions-item :label="t('about.version.update')">2024-03-15</a-descriptions-item>
          </a-descriptions>
        </a-card>
      </a-col>

      <a-col :span="12">
        <a-card>
          <template #title>{{ t('about.contact.title') }}</template>
          <a-descriptions :column="1">
            <a-descriptions-item :label="t('about.contact.email')">support@example.com</a-descriptions-item>
            <a-descriptions-item :label="t('about.contact.phone')">+86 123 4567 8901</a-descriptions-item>
          </a-descriptions>
        </a-card>
      </a-col>

      <a-col :span="24">
        <a-card>
          <template #title>{{ t('about.team.title') }}</template>
          <a-row :gutter="[16, 16]">
            <a-col :span="8">
              <a-card>
                <template #title>{{ t('about.team.developer') }}</template>
                <div class="team-member">
                  <a-avatar :size="64" :icon="UserOutlined" />
                  <h3>开发团队</h3>
                  <p>负责系统的设计与开发</p>
                </div>
              </a-card>
            </a-col>
            <a-col :span="8">
              <a-card>
                <template #title>{{ t('about.team.designer') }}</template>
                <div class="team-member">
                  <a-avatar :size="64" :icon="UserOutlined" />
                  <h3>设计团队</h3>
                  <p>负责用户界面与交互设计</p>
                </div>
              </a-card>
            </a-col>
            <a-col :span="8">
              <a-card>
                <template #title>{{ t('about.team.tester') }}</template>
                <div class="team-member">
                  <a-avatar :size="64" :icon="UserOutlined" />
                  <h3>测试团队</h3>
                  <p>负责质量保证与测试</p>
                </div>
              </a-card>
            </a-col>
          </a-row>
        </a-card>
      </a-col>
    </a-row>
  </div>
</template>

<script setup lang="ts">
import { useI18n } from 'vue-i18n'
import { UserOutlined } from '@ant-design/icons-vue'

const { t } = useI18n()
</script>

<style lang="less" scoped>
.about-container {
  .company-info {
    text-align: center;
    padding: 24px 0;

    .logo {
      width: 120px;
      height: 120px;
      margin-bottom: 24px;
    }

    h1 {
      font-size: 28px;
      margin-bottom: 16px;
      color: var(--color-text);
    }

    p {
      font-size: 16px;
      color: var(--color-text-secondary);
    }
  }

  .team-member {
    text-align: center;
    padding: 16px;

    h3 {
      margin: 16px 0 8px;
      color: var(--color-text);
    }

    p {
      color: var(--color-text-secondary);
    }
  }
}
</style>