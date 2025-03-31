<template>
  <div class="business-pagination">
    <a-pagination v-model:current="current" v-model:pageSize="pageSize" :total="total"
      :show-size-changer="showSizeChanger" :show-quick-jumper="showQuickJumper" :show-total="showTotal"
      :page-size-options="pageSizeOptions" :disabled="disabled" @change="handleChange"
      @showSizeChange="handleShowSizeChange" v-bind="$attrs">
      <template #buildOptionText="props">
        <slot name="buildOptionText" v-bind="props"></slot>
      </template>
    </a-pagination>
  </div>
</template>

<script lang="ts" setup>
import { computed } from 'vue'
import type { PaginationProps } from 'ant-design-vue'

interface Props {
  // 当前页码
  current?: number
  // 每页条数
  pageSize?: number
  // 总条数
  total?: number
  // 是否显示每页条数选择器
  showSizeChanger?: boolean
  // 是否显示快速跳转
  showQuickJumper?: boolean
  // 是否显示总数
  showTotal?: (total: number, range: [number, number]) => string
  // 每页条数选项
  pageSizeOptions?: string[]
  // 是否禁用
  disabled?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  current: 1,
  pageSize: 10,
  total: 0,
  showSizeChanger: true,
  showQuickJumper: true,
  showTotal: (total: number, range: [number, number]) => `共 ${total} 条`,
  pageSizeOptions: () => ['10', '20', '50', '100'],
  disabled: false
})

// 当前页码
const current = computed({
  get: () => props.current,
  set: (val) => emit('update:current', val)
})

// 每页条数
const pageSize = computed({
  get: () => props.pageSize,
  set: (val) => emit('update:pageSize', val)
})

// 分页变化事件
const emit = defineEmits<{
  (e: 'update:current', current: number): void
  (e: 'update:pageSize', pageSize: number): void
  (e: 'change', page: number, pageSize: number): void
  (e: 'showSizeChange', current: number, size: number): void
}>()

// 处理页码变化
const handleChange = (page: number, pageSize: number) => {
  emit('change', page, pageSize)
}

// 处理每页条数变化
const handleShowSizeChange = (current: number, size: number) => {
  emit('showSizeChange', current, size)
}

// 暴露方法给父组件
defineExpose({
  // 获取当前页码
  getCurrent: () => current.value,
  // 获取每页条数
  getPageSize: () => pageSize.value,
  // 获取总条数
  getTotal: () => props.total,
  // 设置当前页码
  setCurrent: (page: number) => {
    current.value = page
  },
  // 设置每页条数
  setPageSize: (size: number) => {
    pageSize.value = size
  }
})
</script>

<style lang="less" scoped>
.business-pagination {
  margin-top: 16px;
  text-align: right;
  padding: 10px 0;
}
</style>