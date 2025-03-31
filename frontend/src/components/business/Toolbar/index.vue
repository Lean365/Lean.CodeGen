<template>
  <div class="business-toolbar">
    <a-space :size="size" :wrap="wrap">
      <!-- 左侧按钮组 -->
      <a-space class="left-buttons">
        <a-button type="primary" @click="handleAdd">
          <template #icon>
            <PlusOutlined />
          </template>
          新增
        </a-button>
        <a-button @click="handleEdit">
          <template #icon>
            <EditOutlined />
          </template>
          编辑
        </a-button>
        <a-button danger @click="handleDelete">
          <template #icon>
            <DeleteOutlined />
          </template>
          删除
        </a-button>
        <a-dropdown>
          <a-button>
            <template #icon>
              <ImportOutlined />
            </template>
            导入
          </a-button>
          <template #overlay>
            <a-menu @click="handleImport">
              <a-menu-item key="template">
                <template #icon>
                  <DownloadOutlined />
                </template>
                下载模板
              </a-menu-item>
              <a-menu-item key="import">
                <template #icon>
                  <UploadOutlined />
                </template>
                导入数据
              </a-menu-item>
            </a-menu>
          </template>
        </a-dropdown>
        <a-button @click="handleExport">
          <template #icon>
            <ExportOutlined />
          </template>
          导出
        </a-button>
        <a-button type="primary" @click="handleAudit">
          <template #icon>
            <CheckOutlined />
          </template>
          审核
        </a-button>
        <a-button type="primary" @click="handleRevoke">
          <template #icon>
            <RollbackOutlined />
          </template>
          撤消
        </a-button>
        <!-- 自定义工具插槽 -->
        <slot name="tools"></slot>
      </a-space>

      <!-- 右侧按钮组 -->
      <a-space class="right-buttons">
        <a-tooltip title="刷新">
          <a-button @click="handleRefresh">
            <template #icon>
              <ReloadOutlined />
            </template>
          </a-button>
        </a-tooltip>
        <a-tooltip title="全屏">
          <a-button @click="handleFullscreen">
            <template #icon>
              <FullscreenOutlined />
            </template>
          </a-button>
        </a-tooltip>
        <a-tooltip title="显隐字段">
          <a-button @click="handleToggleColumns">
            <template #icon>
              <SettingOutlined />
            </template>
          </a-button>
        </a-tooltip>
        <a-tooltip title="显隐查询">
          <a-button @click="handleToggleSearch">
            <template #icon>
              <SearchOutlined />
            </template>
          </a-button>
        </a-tooltip>
      </a-space>
    </a-space>
  </div>
</template>

<script lang="ts" setup>
import { ref } from 'vue'
import type { SpaceProps } from 'ant-design-vue'
import type { MenuClickEventHandler } from 'ant-design-vue/es/menu/src/interface'
import {
  PlusOutlined,
  EditOutlined,
  DeleteOutlined,
  ImportOutlined,
  DownloadOutlined,
  UploadOutlined,
  ExportOutlined,
  CheckOutlined,
  RollbackOutlined,
  ReloadOutlined,
  FullscreenOutlined,
  SettingOutlined,
  SearchOutlined
} from '@ant-design/icons-vue'

interface Props {
  // 间距大小
  size?: SpaceProps['size']
  // 是否自动换行
  wrap?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  size: 'middle',
  wrap: true
})

// 工具栏事件
const emit = defineEmits<{
  (e: 'add'): void
  (e: 'edit'): void
  (e: 'delete'): void
  (e: 'import', type: 'template' | 'import'): void
  (e: 'export'): void
  (e: 'audit'): void
  (e: 'revoke'): void
  (e: 'refresh'): void
  (e: 'fullscreen'): void
  (e: 'toggleColumns'): void
  (e: 'toggleSearch'): void
}>()

// 处理新增
const handleAdd = () => {
  emit('add')
}

// 处理编辑
const handleEdit = () => {
  emit('edit')
}

// 处理删除
const handleDelete = () => {
  emit('delete')
}

// 处理导入
const handleImport: MenuClickEventHandler = (info) => {
  emit('import', info.key as 'template' | 'import')
}

// 处理导出
const handleExport = () => {
  emit('export')
}

// 处理审核
const handleAudit = () => {
  emit('audit')
}

// 处理撤销
const handleRevoke = () => {
  emit('revoke')
}

// 处理刷新
const handleRefresh = () => {
  emit('refresh')
}

// 处理全屏
const handleFullscreen = () => {
  emit('fullscreen')
}

// 处理显隐字段
const handleToggleColumns = () => {
  emit('toggleColumns')
}

// 处理显隐查询
const handleToggleSearch = () => {
  emit('toggleSearch')
}
</script>

<style lang="less" scoped>
.business-toolbar {
  margin-bottom: 16px;
  padding: 16px;
  background: var(--color-bg-container);
  border-radius: 2px;
  border: 1px solid var(--color-border);

  :deep(.ant-space) {
    width: 100%;
    justify-content: space-between;
  }

  // 左侧按钮组样式
  :deep(.left-buttons .ant-btn) {
    display: inline-flex;
    align-items: center;
    justify-content: center;
    min-width: 68px;
    width: 68px;
    padding: 0 12px;
    height: 32px;

    .anticon {
      margin-right: 0;
      font-size: 14px;
    }

    span {
      line-height: 1;
    }
  }

  // 右侧按钮组样式
  :deep(.right-buttons .ant-btn) {
    min-width: 32px;
    width: 32px;
    padding: 0;
  }
}
</style>