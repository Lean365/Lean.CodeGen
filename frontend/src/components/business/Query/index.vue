<template>
  <div class="business-query">
    <a-form :model="formState" :layout="layout" :label-col="labelCol" :wrapper-col="wrapperCol" :colon="colon"
      @finish="handleFinish" @valuesChange="handleValuesChange" v-bind="$attrs">
      <slot :form="formState"></slot>

      <a-form-item :wrapper-col="submitWrapperCol" v-if="showSubmit">
        <a-space>
          <a-button type="primary" html-type="submit" :loading="loading">
            <template #icon>
              <SearchOutlined />
            </template>
            {{ submitText }}
          </a-button>
          <a-button @click="handleReset" :disabled="loading">
            <template #icon>
              <ReloadOutlined />
            </template>
            {{ resetText }}
          </a-button>
        </a-space>
      </a-form-item>
    </a-form>
  </div>
</template>

<script lang="ts" setup>
import { ref, computed } from 'vue'
import type { FormProps } from 'ant-design-vue'
import { SearchOutlined, ReloadOutlined } from '@ant-design/icons-vue'

interface Props {
  // 表单布局
  layout?: FormProps['layout']
  // 标签列配置
  labelCol?: { span: number }
  // 包装列配置
  wrapperCol?: { span: number }
  // 是否显示冒号
  colon?: boolean
  // 是否显示提交按钮
  showSubmit?: boolean
  // 提交按钮文本
  submitText?: string
  // 重置按钮文本
  resetText?: string
  // 加载状态
  loading?: boolean
  // 初始值
  initialValues?: Record<string, any>
}

const props = withDefaults(defineProps<Props>(), {
  layout: 'inline',
  labelCol: () => ({ span: 6 }),
  wrapperCol: () => ({ span: 18 }),
  colon: true,
  showSubmit: true,
  submitText: '搜索',
  resetText: '重置',
  loading: false,
  initialValues: () => ({})
})

// 表单状态
const formState = ref<Record<string, any>>({ ...props.initialValues })

// 提交按钮的包装列配置
const submitWrapperCol = computed(() => {
  if (props.layout === 'inline') {
    return { span: 24 }
  }
  return {
    span: 24 - (props.labelCol?.span || 6)
  }
})

// 表单提交事件
const emit = defineEmits<{
  (e: 'finish', values: Record<string, any>): void
  (e: 'reset'): void
  (e: 'valuesChange', changedValues: Record<string, any>, allValues: Record<string, any>): void
}>()

// 处理表单提交
const handleFinish = (values: Record<string, any>) => {
  emit('finish', values)
}

// 处理表单重置
const handleReset = () => {
  formState.value = { ...props.initialValues }
  emit('reset')
}

// 处理表单值变化
const handleValuesChange = (changedValues: Record<string, any>, allValues: Record<string, any>) => {
  emit('valuesChange', changedValues, allValues)
}

// 暴露方法给父组件
defineExpose({
  // 获取表单值
  getValues: () => formState.value,
  // 设置表单值
  setValues: (values: Record<string, any>) => {
    formState.value = { ...formState.value, ...values }
  },
  // 重置表单
  reset: handleReset,
  // 提交表单
  submit: () => {
    handleFinish(formState.value)
  }
})
</script>

<style lang="less" scoped>
.business-query {
  background: var(--color-bg-container);
  padding: 16px;
  border-radius: 2px;
  margin-bottom: 16px;
  border: 1px solid var(--color-border);
}
</style>