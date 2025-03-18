<template>
  <div class="footer">
    <div class="copyright">
      Copyright © {{ new Date().getFullYear() }} {{ t('app.name') }}
    </div>
    <div class="links">
      <a-space>
        <router-link to="/about/terms">{{ t('about.terms') }}</router-link>
        <router-link to="/about/privacy">{{ t('about.privacy') }}</router-link>
      </a-space>
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

  .copyright {
    color: var(--color-text-secondary);
  }

  .links {
    a {
      color: var(--color-text-secondary);
      text-decoration: none;

      &:hover {
        color: var(--color-primary);
      }
    }
  }
}
</style>