<!-- 服务条款 -->
<template>
  <div class="terms-container">
    <a-card>
      <template #title>{{ t('about.terms.title') }}</template>
      <a-collapse v-model:activeKey="activeKey">
        <a-collapse-panel key="1" :header="t('about.terms.service.title')">
          <div class="terms-section">
            <h3>{{ t('about.terms.service.description.title') }}</h3>
            <p>{{ t('about.terms.service.description.content') }}</p>
            <ul>
              <li>{{ t('about.terms.service.description.item1') }}</li>
              <li>{{ t('about.terms.service.description.item2') }}</li>
              <li>{{ t('about.terms.service.description.item3') }}</li>
            </ul>
          </div>
        </a-collapse-panel>

        <a-collapse-panel key="2" :header="t('about.terms.usage.title')">
          <div class="terms-section">
            <h3>{{ t('about.terms.usage.rules.title') }}</h3>
            <p>{{ t('about.terms.usage.rules.content') }}</p>
            <ul>
              <li>{{ t('about.terms.usage.rules.item1') }}</li>
              <li>{{ t('about.terms.usage.rules.item2') }}</li>
              <li>{{ t('about.terms.usage.rules.item3') }}</li>
            </ul>
          </div>
        </a-collapse-panel>

        <a-collapse-panel key="3" :header="t('about.terms.intellectual.title')">
          <div class="terms-section">
            <h3>{{ t('about.terms.intellectual.property.title') }}</h3>
            <p>{{ t('about.terms.intellectual.property.content') }}</p>
            <ul>
              <li>{{ t('about.terms.intellectual.property.item1') }}</li>
              <li>{{ t('about.terms.intellectual.property.item2') }}</li>
              <li>{{ t('about.terms.intellectual.property.item3') }}</li>
            </ul>
          </div>
        </a-collapse-panel>

        <a-collapse-panel key="4" :header="t('about.terms.liability.title')">
          <div class="terms-section">
            <h3>{{ t('about.terms.liability.limitation.title') }}</h3>
            <p>{{ t('about.terms.liability.limitation.content') }}</p>
            <ul>
              <li>{{ t('about.terms.liability.limitation.item1') }}</li>
              <li>{{ t('about.terms.liability.limitation.item2') }}</li>
              <li>{{ t('about.terms.liability.limitation.item3') }}</li>
            </ul>
          </div>
        </a-collapse-panel>
      </a-collapse>

      <div class="terms-footer">
        <p>{{ t('about.terms.lastUpdated') }}: 2024-03-15</p>
        <p>{{ t('about.terms.contact') }}: support@example.com</p>
      </div>
    </a-card>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useI18n } from 'vue-i18n'

const { t } = useI18n()
const activeKey = ref(['1'])
</script>

<style lang="less" scoped>
.terms-container {
  .terms-section {
    h3 {
      color: var(--color-text);
      margin-bottom: 8px;
    }

    p {
      color: var(--color-text-secondary);
      margin-bottom: 16px;
    }

    ul {
      list-style: disc;
      margin-left: 20px;
      margin-bottom: 16px;

      li {
        color: var(--color-text-secondary);
        margin-bottom: 8px;
      }
    }
  }

  .terms-footer {
    margin-top: 24px;
    padding-top: 16px;
    border-top: 1px solid var(--color-border);
    text-align: right;
    color: var(--color-text-secondary);

    p {
      margin-bottom: 8px;
    }
  }
}
</style>