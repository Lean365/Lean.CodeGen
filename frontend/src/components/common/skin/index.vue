<template>
  <a-config-provider :theme="themeConfig">
    <a-dropdown>
      <a-button type="text" class="action-button">
        <SkinOutlined />
      </a-button>
      <template #overlay>
        <a-menu @click="handleSkinChange">
          <a-menu-item key="default">
            <CheckOutlined v-if="currentSkin === 'default'" />
            <span>{{ t('skin.default') }}</span>
          </a-menu-item>
          <a-menu-item key="blue">
            <CheckOutlined v-if="currentSkin === 'blue'" />
            <span>{{ t('skin.blue') }}</span>
          </a-menu-item>
          <a-menu-item key="green">
            <CheckOutlined v-if="currentSkin === 'green'" />
            <span>{{ t('skin.green') }}</span>
          </a-menu-item>
          <a-menu-item key="purple">
            <CheckOutlined v-if="currentSkin === 'purple'" />
            <span>{{ t('skin.purple') }}</span>
          </a-menu-item>
          <a-menu-divider />
          <a-menu-item key="custom">
            <SettingOutlined />
            <span>{{ t('skin.custom') }}</span>
          </a-menu-item>
        </a-menu>
      </template>
    </a-dropdown>

    <a-modal v-model:open="customVisible" :title="t('skin.customTitle')" @ok="handleCustomSave"
      @cancel="handleCustomCancel">
      <div class="custom-skin-form">
        <a-form :model="customForm" layout="vertical">
          <a-form-item :label="t('skin.primaryColor')">
            <a-input v-model:value="customForm.primary" type="color" style="width: 180px;" />
            <a-select v-model:value="customForm.borderRadius" style="width: 120px; margin-left: 8px;">
              <a-select-option value="2">{{ t('skin.borderRadius2') }}</a-select-option>
              <a-select-option value="4">{{ t('skin.borderRadius4') }}</a-select-option>
              <a-select-option value="6">{{ t('skin.borderRadius6') }}</a-select-option>
              <a-select-option value="8">{{ t('skin.borderRadius8') }}</a-select-option>
            </a-select>
          </a-form-item>
          <a-form-item :label="t('skin.backgroundColor')">
            <a-input v-model:value="customForm.background" type="color" style="width: 180px;" />
            <a-select v-model:value="customForm.colorSaturation" style="width: 120px; margin-left: 8px;">
              <a-select-option value="95">{{ t('skin.lightColor') }}</a-select-option>
              <a-select-option value="85">{{ t('skin.mediumColor') }}</a-select-option>
              <a-select-option value="75">{{ t('skin.darkColor') }}</a-select-option>
            </a-select>
          </a-form-item>
          <a-form-item :label="t('skin.textColor')">
            <a-input v-model:value="customForm.text" type="color" style="width: 180px;" />
            <a-select v-model:value="customForm.fontSize" style="width: 120px; margin-left: 8px;">
              <a-select-option value="12">{{ t('skin.smallFont') }}</a-select-option>
              <a-select-option value="14">{{ t('skin.mediumFont') }}</a-select-option>
              <a-select-option value="16">{{ t('skin.largeFont') }}</a-select-option>
            </a-select>
          </a-form-item>
          <a-form-item :label="t('skin.linkColor')">
            <a-input v-model:value="customForm.link" type="color" style="width: 180px;" />
            <a-switch v-model:checked="customForm.enableHover" :checked-children="t('skin.hoverEffect')"
              :un-checked-children="t('skin.noEffect')" style="margin-left: 8px;" />
          </a-form-item>
          <a-form-item :label="t('skin.shadowStyle')">
            <a-select v-model:value="customForm.shadowStyle" style="width: 100%;">
              <a-select-option value="none">{{ t('skin.noShadow') }}</a-select-option>
              <a-select-option value="light">{{ t('skin.lightShadow') }}</a-select-option>
              <a-select-option value="medium">{{ t('skin.mediumShadow') }}</a-select-option>
              <a-select-option value="strong">{{ t('skin.strongShadow') }}</a-select-option>
            </a-select>
          </a-form-item>
        </a-form>
      </div>
    </a-modal>
  </a-config-provider>
</template>

<script setup lang="ts">
import { ref, reactive, computed } from 'vue'
import { SkinOutlined, CheckOutlined, SettingOutlined } from '@ant-design/icons-vue'
import { useI18n } from 'vue-i18n'
import { useSkinStore } from '@/stores/skin'
import type { SkinType } from '@/stores/skin'
import type { MenuClickEventHandler } from 'ant-design-vue/es/menu/src/interface'
import { theme } from 'ant-design-vue'

const { t } = useI18n()
const skinStore = useSkinStore()
const customVisible = ref(false)

const currentSkin = computed(() => skinStore.currentSkin)

const themeConfig = computed(() => ({
  token: {
    colorPrimary: customForm.primary,
    borderRadius: parseInt(customForm.borderRadius),
    colorBgContainer: customForm.background,
    colorText: customForm.text,
    colorLink: customForm.link,
    fontSize: parseInt(customForm.fontSize),
    colorBgElevated: customForm.background,
    boxShadow: customForm.shadowStyle === 'none' ? 'none' :
      customForm.shadowStyle === 'light' ? '0 2px 8px rgba(0, 0, 0, 0.15)' :
        customForm.shadowStyle === 'medium' ? '0 4px 12px rgba(0, 0, 0, 0.15)' :
          '0 8px 24px rgba(0, 0, 0, 0.15)',
    algorithm: theme.defaultAlgorithm
  }
}))

const customForm = reactive({
  primary: '#1890ff',
  background: '#ffffff',
  text: '#000000',
  link: '#1890ff',
  borderRadius: '4',
  colorSaturation: '85',
  fontSize: '14',
  enableHover: true,
  shadowStyle: 'light'
})

const handleSkinChange: MenuClickEventHandler = (e) => {
  if (e.key === 'custom') {
    customVisible.value = true
  } else {
    skinStore.setSkin(e.key as SkinType)
  }
}

const handleCustomSave = () => {
  skinStore.setCustomColors({
    primary: customForm.primary,
    background: customForm.background,
    text: customForm.text,
    link: customForm.link,
    borderRadius: parseInt(customForm.borderRadius),
    colorSaturation: parseInt(customForm.colorSaturation),
    fontSize: parseInt(customForm.fontSize),
    enableHover: customForm.enableHover,
    shadowStyle: customForm.shadowStyle
  })
  customVisible.value = false
}

const handleCustomCancel = () => {
  customVisible.value = false
}
</script>

<style lang="less" scoped>
.action-button {
  display: flex;
  align-items: center;
  justify-content: center;
  width: 40px;
  height: 40px;
  color: var(--color-text);
}

.custom-skin-form {
  padding: 16px;

  :deep(.ant-form-item) {
    margin-bottom: 16px;
  }
}
</style>