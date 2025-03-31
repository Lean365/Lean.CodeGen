<template>
  <div class="business-table">
    <a-table :columns="columns" :data-source="dataSource" :loading="loading" :row-key="rowKey" :scroll="scroll"
      :pagination="false" @change="handleTableChange" v-bind="$attrs">
      <!-- 序号列 -->
      <template #bodyCell="{ column, index }">
        <template v-if="column.dataIndex === 'index'">
          {{ (current - 1) * pageSize + index + 1 }}
        </template>
        <template v-else>
          <slot :name="column.dataIndex" :record="$attrs.record" :index="index"></slot>
        </template>
      </template>

      <!-- 自定义列插槽 -->
      <template v-for="slot in Object.keys($slots)" #[slot]="data">
        <slot :name="slot" v-bind="data"></slot>
      </template>
    </a-table>
  </div>
</template>

<script lang="ts" setup>
import type { TableProps } from 'ant-design-vue'

interface Props {
  // 表格列配置
  columns: any[]
  // 表格数据
  dataSource: any[]
  // 加载状态
  loading?: boolean
  // 行数据的唯一标识
  rowKey?: string
  // 表格滚动配置
  scroll?: TableProps['scroll']
  // 当前页码
  current?: number
  // 每页条数
  pageSize?: number
}

const props = withDefaults(defineProps<Props>(), {
  loading: false,
  rowKey: 'id',
  current: 1,
  pageSize: 10
})

// 表格变化事件
const emit = defineEmits<{
  (e: 'change', pagination: any, filters: any, sorter: any): void
}>()

// 处理表格变化
const handleTableChange = (pagination: any, filters: any, sorter: any) => {
  emit('change', pagination, filters, sorter)
}

// 暴露方法给父组件
defineExpose({
  // 获取表格数据
  getDataSource: () => props.dataSource,
  // 获取加载状态
  getLoading: () => props.loading
})
</script>

<style lang="less" scoped>
.business-table {
  :deep(.ant-table-wrapper) {
    padding: 16px;
  }
}
</style>