<template>
  <a-tag v-if="finalLabel" :class="['hbt-dict-tag', tagClass]">{{ finalLabel }}</a-tag>
  <a-tag v-else color="default" class="hbt-dict-tag">{{ t('common.unknown') }}</a-tag>
</template>

<script lang="ts" setup>
import { computed, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import { useDictStore } from '@/stores/dict'
import '@/assets/styles/dict-tag.less'

const { t } = useI18n()

interface Props {
  // 字典类型
  dictType: string
  // 字典值
  value: number | string
  // 是否使用国际化
  useI18n?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  useI18n: false
})

// 使用字典Hook
const dictStore = useDictStore()

// 计算最终显示的标签
const finalLabel = computed(() => {
  const label = dictStore.getDictLabel(props.dictType, Number(props.value))
  if (props.useI18n) {
    const transKey = dictStore.getDictTransKey(props.dictType, Number(props.value))
    return transKey ? t(transKey) : label
  }
  return label
})

// 计算标签样式类
const tagClass = computed(() => {
  const baseClass = dictStore.getDictClass(props.dictType, Number(props.value))
  return baseClass ? `hbt-dict-tag-${baseClass}` : ''
})

onMounted(() => {
  if (props.dictType) {
    dictStore.loadDict(props.dictType)
  }
})

const dictClass = computed(() => props.dictType ? dictStore.getDictClass(props.dictType, props.value) : '')
const dictLabel = computed(() => props.dictType ? dictStore.getDictLabel(props.dictType, props.value) : props.value)
</script>

<style lang="less" scoped>
.dict-tag {
  display: inline-block;
  padding: 2px 8px;
  border-radius: 2px;
  font-size: 12px;
  line-height: 1.5;

  // 基础状态颜色
  &.default {
    background: var(--ant-color-bg-container);
  }

  &.primary {
    background: var(--ant-color-primary-bg);
  }

  &.success {
    background: var(--ant-color-success-bg);
  }

  &.warning {
    background: var(--ant-color-warning-bg);
  }

  &.error {
    background: var(--ant-color-error-bg);
  }

  &.info {
    background: var(--ant-color-info-bg);
  }

  // 系统状态颜色
  &.system-running {
    background: var(--ant-color-success-bg);
  }

  &.system-stopped {
    background: var(--ant-color-error-bg);
  }

  &.system-error {
    background: var(--ant-color-error-bg);
  }

  &.system-warning {
    background: var(--ant-color-warning-bg);
  }

  &.system-info {
    background: var(--ant-color-info-bg);
  }

  // 任务状态颜色
  &.task-pending {
    background: var(--ant-color-warning-bg);
  }

  &.task-running {
    background: var(--ant-color-primary-bg);
  }

  &.task-completed {
    background: var(--ant-color-success-bg);
  }

  &.task-failed {
    background: var(--ant-color-error-bg);
  }

  &.task-cancelled {
    background: var(--ant-color-gray-bg);
  }

  // 审批状态颜色
  &.approval-pending {
    background: var(--ant-color-warning-bg);
  }

  &.approval-processing {
    background: var(--ant-color-primary-bg);
  }

  &.approval-approved {
    background: var(--ant-color-success-bg);
  }

  &.approval-rejected {
    background: var(--ant-color-error-bg);
  }

  &.approval-cancelled {
    background: var(--ant-color-gray-bg);
  }

  // 支付状态颜色
  &.payment-unpaid {
    background: var(--ant-color-warning-bg);
  }

  &.payment-processing {
    background: var(--ant-color-primary-bg);
  }

  &.payment-paid {
    background: var(--ant-color-success-bg);
  }

  &.payment-failed {
    background: var(--ant-color-error-bg);
  }

  &.payment-refunded {
    background: var(--ant-color-gray-bg);
  }

  // 订单状态颜色
  &.order-pending {
    background: var(--ant-color-warning-bg);
  }

  &.order-processing {
    background: var(--ant-color-primary-bg);
  }

  &.order-shipped {
    background: var(--ant-color-info-bg);
  }

  &.order-delivered {
    background: var(--ant-color-success-bg);
  }

  &.order-cancelled {
    background: var(--ant-color-gray-bg);
  }

  // 用户状态颜色
  &.user-active {
    background: var(--ant-color-success-bg);
  }

  &.user-inactive {
    background: var(--ant-color-warning-bg);
  }

  &.user-locked {
    background: var(--ant-color-error-bg);
  }

  &.user-deleted {
    background: var(--ant-color-gray-bg);
  }

  &.user-disabled {
    background: var(--ant-color-gray-bg);
  }

  // 数据状态颜色
  &.data-draft {
    background: var(--ant-color-warning-bg);
  }

  &.data-published {
    background: var(--ant-color-success-bg);
  }

  &.data-archived {
    background: var(--ant-color-gray-bg);
  }

  &.data-deleted {
    background: var(--ant-color-error-bg);
  }

  &.data-disabled {
    background: var(--ant-color-gray-bg);
  }

  // 日志状态颜色
  &.log-info {
    background: var(--ant-color-info-bg);
  }

  &.log-success {
    background: var(--ant-color-success-bg);
  }

  &.log-warning {
    background: var(--ant-color-warning-bg);
  }

  &.log-error {
    background: var(--ant-color-error-bg);
  }

  &.log-debug {
    background: var(--ant-color-gray-bg);
  }

  // 通知状态颜色
  &.notification-unread {
    background: var(--ant-color-primary-bg);
  }

  &.notification-read {
    background: var(--ant-color-gray-bg);
  }

  &.notification-archived {
    background: var(--ant-color-gray-bg);
  }

  &.notification-deleted {
    background: var(--ant-color-error-bg);
  }

  &.notification-disabled {
    background: var(--ant-color-gray-bg);
  }
}
</style>