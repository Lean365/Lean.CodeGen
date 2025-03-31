<template>
  <div class="business-modal">
    <a-modal v-model:open="visible" :title="title" :width="width" :centered="centered" :mask-closable="maskClosable"
      :keyboard="keyboard" :destroy-on-close="destroyOnClose" :z-index="zIndex" :class-name="className"
      :wrap-class-name="wrapClassName" :mask-class-name="maskClassName" :body-class-name="bodyClassName" :footer="null"
      :confirm-loading="confirmLoading" :ok-text="okText" :cancel-text="cancelText" :ok-type="okType"
      :cancel-type="cancelType" :ok-button-props="okButtonProps" :cancel-button-props="cancelButtonProps" @ok="handleOk"
      @cancel="handleCancel" @after-visible-change="handleAfterVisibleChange" v-bind="$attrs">
      <!-- 标题插槽 -->
      <template #title>
        <slot name="title"></slot>
      </template>

      <!-- 内容插槽 -->
      <slot></slot>

      <!-- 底部按钮插槽 -->
      <template #footer>
        <slot name="footer">
          <a-space>
            <a-button v-if="showCancel" :type="cancelType" :loading="cancelLoading" v-bind="cancelButtonProps"
              @click="handleCancel">
              {{ cancelText }}
            </a-button>
            <a-button v-if="showOk" :type="okType" :loading="confirmLoading" v-bind="okButtonProps" @click="handleOk">
              {{ okText }}
            </a-button>
          </a-space>
        </slot>
      </template>
    </a-modal>
  </div>
</template>

<script lang="ts" setup>
import { ref, computed } from 'vue'
import type { ButtonProps } from 'ant-design-vue'

interface Props {
  // 是否显示
  open?: boolean
  // 标题
  title?: string
  // 宽度
  width?: number | string
  // 是否垂直居中展示
  centered?: boolean
  // 点击蒙层是否允许关闭
  maskClosable?: boolean
  // 是否支持键盘 esc 关闭
  keyboard?: boolean
  // 关闭时是否销毁 Modal 里的子元素
  destroyOnClose?: boolean
  // 设置 Modal 的 z-index
  zIndex?: number
  // 设置 Modal 的 class
  className?: string
  // 设置 Modal 的 wrap class
  wrapClassName?: string
  // 设置 Modal 的 mask class
  maskClassName?: string
  // 设置 Modal 的 body class
  bodyClassName?: string
  // 确认按钮 loading
  confirmLoading?: boolean
  // 取消按钮 loading
  cancelLoading?: boolean
  // 确认按钮文字
  okText?: string
  // 取消按钮文字
  cancelText?: string
  // 确认按钮类型
  okType?: ButtonProps['type']
  // 取消按钮类型
  cancelType?: ButtonProps['type']
  // 确认按钮 props
  okButtonProps?: ButtonProps
  // 取消按钮 props
  cancelButtonProps?: ButtonProps
  // 是否显示确认按钮
  showOk?: boolean
  // 是否显示取消按钮
  showCancel?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  open: false,
  title: '',
  width: 520,
  centered: true,
  maskClosable: true,
  keyboard: true,
  destroyOnClose: false,
  zIndex: 1000,
  className: '',
  wrapClassName: '',
  maskClassName: '',
  bodyClassName: '',
  confirmLoading: false,
  cancelLoading: false,
  okText: '确定',
  cancelText: '取消',
  okType: 'primary',
  cancelType: 'default',
  okButtonProps: () => ({}),
  cancelButtonProps: () => ({}),
  showOk: true,
  showCancel: true
})

// 显示状态
const visible = computed({
  get: () => props.open,
  set: (val) => emit('update:open', val)
})

// Modal事件
const emit = defineEmits<{
  (e: 'update:open', open: boolean): void
  (e: 'ok', event: MouseEvent): void
  (e: 'cancel', event: MouseEvent): void
  (e: 'afterVisibleChange', visible: boolean): void
}>()

// 处理确认
const handleOk = (e: MouseEvent) => {
  emit('ok', e)
}

// 处理取消
const handleCancel = (e: MouseEvent) => {
  emit('cancel', e)
}

// 处理显示状态变化后
const handleAfterVisibleChange = (visible: boolean) => {
  emit('afterVisibleChange', visible)
}

// 暴露方法给父组件
defineExpose({
  // 打开弹窗
  open: () => {
    visible.value = true
  },
  // 关闭弹窗
  close: () => {
    visible.value = false
  },
  // 获取显示状态
  isOpen: () => visible.value
})
</script>

<style lang="less" scoped>
.business-modal {
  :deep(.ant-modal-content) {
    padding: 0;
  }

  :deep(.ant-modal-body) {
    padding: 24px;
  }

  :deep(.ant-modal-footer) {
    padding: 16px 24px;
    border-top: 1px solid #f0f0f0;
  }
}
</style>