<template>
  <div class="footer">
    <div class="copyright">
      {{ t('footer.copyright', { year: new Date().getFullYear() }) }}
    </div>
    <div class="links">
      <a href="/about">{{ t('footer.about') }}</a>
      <a href="/privacy">{{ t('footer.privacy') }}</a>
      <a href="/terms">{{ t('footer.terms') }}</a>
      <a href="https://beian.miit.gov.cn/" target="_blank">ICP123456</a>
    </div>
  </div>
</template>

<script setup lang="ts">
import { useI18n } from 'vue-i18n'

const { t } = useI18n()
</script>

<style lang="less" scoped>
.footer {
  display: flex;
  justify-content: space-between;
  align-items: center;
  height: 100%;

  .copyright {
    color: var(--color-text-secondary);
  }

  .links {
    a {
      color: var(--color-text-secondary);
      margin-left: 24px;
      text-decoration: none;

      &:hover {
        color: var(--color-primary);
      }
    }
  }
}
</style>