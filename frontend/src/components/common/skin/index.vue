<template>
  <a-dropdown>
    <a-button type="text" class="action-button">
      <SkinOutlined />
    </a-button>
    <template #overlay>
      <a-menu @click="handleSkinChange">
        <a-menu-item key="default">
          <CheckOutlined v-if="currentSkin === 'default'" />
          <span>默认皮肤</span>
        </a-menu-item>
        <a-menu-item key="blue">
          <CheckOutlined v-if="currentSkin === 'blue'" />
          <span>蓝色</span>
        </a-menu-item>
        <a-menu-item key="green">
          <CheckOutlined v-if="currentSkin === 'green'" />
          <span>绿色</span>
        </a-menu-item>
        <a-menu-item key="purple">
          <CheckOutlined v-if="currentSkin === 'purple'" />
          <span>紫色</span>
        </a-menu-item>
        <a-menu-divider />
        <a-menu-item key="custom">
          <SettingOutlined />
          <span>自定义</span>
        </a-menu-item>
      </a-menu>
    </template>
  </a-dropdown>

  <a-modal v-model:visible="customVisible" title="自定义皮肤" @ok="handleCustomSave" @cancel="handleCustomCancel">
    <div class="custom-skin-form">
      <a-form :model="customForm" layout="vertical">
        <a-form-item label="主色调">
          <a-color-picker v-model:value="customForm.primary" />
          <a-select v-model:value="customForm.borderRadius" style="width: 120px; margin-left: 8px;">
            <a-select-option value="2">圆角 2px</a-select-option>
            <a-select-option value="4">圆角 4px</a-select-option>
            <a-select-option value="6">圆角 6px</a-select-option>
            <a-select-option value="8">圆角 8px</a-select-option>
          </a-select>
        </a-form-item>
        <a-form-item label="背景色">
          <a-color-picker v-model:value="customForm.background" />
          <a-select v-model:value="customForm.colorSaturation" style="width: 120px; margin-left: 8px;">
            <a-select-option value="95">浅色</a-select-option>
            <a-select-option value="85">适中</a-select-option>
            <a-select-option value="75">深色</a-select-option>
          </a-select>
        </a-form-item>
        <a-form-item label="文字颜色">
          <a-color-picker v-model:value="customForm.text" />
          <a-select v-model:value="customForm.fontSize" style="width: 120px; margin-left: 8px;">
            <a-select-option value="12">小号</a-select-option>
            <a-select-option value="14">中号</a-select-option>
            <a-select-option value="16">大号</a-select-option>
          </a-select>
        </a-form-item>
        <a-form-item label="链接颜色">
          <a-color-picker v-model:value="customForm.link" />
          <a-switch v-model:checked="customForm.enableHover" checkedChildren="悬停效果" unCheckedChildren="无效果"
            style="margin-left: 8px;" />
        </a-form-item>
        <a-form-item label="阴影效果">
          <a-select v-model:value="customForm.shadowStyle" style="width: 100%;">
            <a-select-option value="none">无阴影</a-select-option>
            <a-select-option value="light">浅阴影</a-select-option>
            <a-select-option value="medium">中等阴影</a-select-option>
            <a-select-option value="strong">深阴影</a-select-option>
          </a-select>
        </a-form-item>
      </a-form>
    </div>
  </a-modal>
</template>

<script setup lang="ts">
import { ref, reactive, computed } from 'vue'
import { SkinOutlined, CheckOutlined, SettingOutlined } from '@ant-design/icons-vue'
import { useSkinStore } from '@/stores/skin'
import type { SkinType } from '@/stores/skin'

const skinStore = useSkinStore()
const customVisible = ref(false)

const currentSkin = computed(() => skinStore.currentSkin)

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

const handleSkinChange = (e: { key: SkinType }) => {
  if (e.key === 'custom') {
    customVisible.value = true
  } else {
    skinStore.setSkin(e.key)
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

  :deep(.ant-color-picker) {
    width: 180px;
  }
}
</style>