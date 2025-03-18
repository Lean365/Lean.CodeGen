<!-- 隐私政策 -->
<template>
  <div class="privacy-container">
    <a-card>
      <template #title>{{ t('about.privacy.title') }}</template>
      <a-timeline>
        <a-timeline-item>
          <template #dot>
            <SafetyOutlined />
          </template>
          <h3>{{ t('about.privacy.dataCollection.title') }}</h3>
          <p>{{ t('about.privacy.dataCollection.content') }}</p>
        </a-timeline-item>

        <a-timeline-item>
          <template #dot>
            <SecurityScanOutlined />
          </template>
          <h3>{{ t('about.privacy.dataSecurity.title') }}</h3>
          <p>{{ t('about.privacy.dataSecurity.content') }}</p>
        </a-timeline-item>

        <a-timeline-item>
          <template #dot>
            <ShareAltOutlined />
          </template>
          <h3>{{ t('about.privacy.dataSharing.title') }}</h3>
          <p>{{ t('about.privacy.dataSharing.content') }}</p>
        </a-timeline-item>

        <a-timeline-item>
          <template #dot>
            <UserOutlined />
          </template>
          <h3>{{ t('about.privacy.userRights.title') }}</h3>
          <p>{{ t('about.privacy.userRights.content') }}</p>
        </a-timeline-item>

        <a-timeline-item>
          <template #dot>
            <ContactsOutlined />
          </template>
          <h3>{{ t('about.privacy.contact.title') }}</h3>
          <p>{{ t('about.privacy.contact.content') }}</p>
        </a-timeline-item>
      </a-timeline>

      <div class="privacy-footer">
        <p>{{ t('about.privacy.lastUpdated') }}: 2024-03-15</p>
      </div>
    </a-card>
  </div>
</template>

<script setup lang="ts">
import { useI18n } from 'vue-i18n'
import {
  SafetyOutlined,
  SecurityScanOutlined,
  ShareAltOutlined,
  UserOutlined,
  ContactsOutlined
} from '@ant-design/icons-vue'

const { t } = useI18n()
</script>

<style lang="less" scoped>
.privacy-container {
  h3 {
    color: var(--color-text);
    margin-bottom: 8px;
  }

  p {
    color: var(--color-text-secondary);
    margin-bottom: 16px;
  }

  .privacy-footer {
    margin-top: 24px;
    padding-top: 16px;
    border-top: 1px solid var(--color-border);
    text-align: right;
    color: var(--color-text-secondary);
  }
}
</style>