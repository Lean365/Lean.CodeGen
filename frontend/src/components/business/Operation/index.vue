<template>
  <a-space :size="4">
    <!-- 基础操作 -->
    <a-tooltip v-if="hasPermission('create')" title="创建">
      <a-button type="link" size="small" @click="handleCreate">
        <template #icon>
          <PlusOutlined />
        </template>
      </a-button>
    </a-tooltip>
    <a-tooltip v-if="hasPermission('view')" title="查看">
      <a-button type="link" size="small" @click="handleView">
        <template #icon>
          <EyeOutlined />
        </template>
      </a-button>
    </a-tooltip>
    <a-tooltip v-if="hasPermission('edit')" title="编辑">
      <a-button type="link" size="small" @click="handleEdit">
        <template #icon>
          <EditOutlined />
        </template>
      </a-button>
    </a-tooltip>
    <a-tooltip v-if="hasPermission('delete')" title="删除">
      <a-button type="link" size="small" danger @click="handleDelete">
        <template #icon>
          <DeleteOutlined />
        </template>
      </a-button>
    </a-tooltip>
    <a-tooltip v-if="hasPermission('copy')" title="复制">
      <a-button type="link" size="small" @click="handleCopy">
        <template #icon>
          <CopyOutlined />
        </template>
      </a-button>
    </a-tooltip>

    <!-- 更多操作下拉菜单 -->
    <a-dropdown v-if="hasMoreOperations">
      <a-button type="link" size="small">
        <template #icon>
          <MoreOutlined />
        </template>
      </a-button>
      <template #overlay>
        <a-menu>
          <!-- 基础操作组 -->
          <a-menu-item-group title="基础操作" v-if="hasBasicOperations">
            <a-menu-item v-if="hasPermission('create')" key="create">
              <template #icon>
                <PlusOutlined />
              </template>创建
            </a-menu-item>
            <a-menu-item v-if="hasPermission('export')" key="export">
              <template #icon>
                <ExportOutlined />
              </template>导出
            </a-menu-item>
            <a-menu-item v-if="hasPermission('import')" key="import">
              <template #icon>
                <ImportOutlined />
              </template>导入
            </a-menu-item>
            <a-menu-item v-if="hasPermission('print')" key="print">
              <template #icon>
                <PrinterOutlined />
              </template>打印
            </a-menu-item>
            <a-menu-item v-if="hasPermission('download')" key="download">
              <template #icon>
                <DownloadOutlined />
              </template>下载
            </a-menu-item>
            <a-menu-item v-if="hasPermission('upload')" key="upload">
              <template #icon>
                <UploadOutlined />
              </template>上传
            </a-menu-item>
            <a-menu-item v-if="hasPermission('clone')" key="clone">
              <template #icon>
                <CopyOutlined />
              </template>克隆
            </a-menu-item>
          </a-menu-item-group>

          <!-- 权限操作组 -->
          <a-menu-item-group title="权限操作" v-if="hasAuthOperations">
            <a-menu-item v-if="hasPermission('assign')" key="assign">
              <template #icon>
                <TeamOutlined />
              </template>分配用户
            </a-menu-item>
            <a-menu-item v-if="hasPermission('assignRole')" key="assignRole">
              <template #icon>
                <UsergroupAddOutlined />
              </template>分配角色
            </a-menu-item>
            <a-menu-item v-if="hasPermission('authorize')" key="authorize">
              <template #icon>
                <SafetyCertificateOutlined />
              </template>授权
            </a-menu-item>
            <a-menu-item v-if="hasPermission('dataScope')" key="dataScope">
              <template #icon>
                <DatabaseOutlined />
              </template>数据权限
            </a-menu-item>
          </a-menu-item-group>

          <!-- 工作流操作组 -->
          <a-menu-item-group title="工作流操作" v-if="hasWorkflowOperations">
            <a-menu-item v-if="hasPermission('submit')" key="submit">
              <template #icon>
                <SendOutlined />
              </template>提交
            </a-menu-item>
            <a-menu-item v-if="hasPermission('approve')" key="approve">
              <template #icon>
                <CheckCircleOutlined />
              </template>审批
            </a-menu-item>
            <a-menu-item v-if="hasPermission('reject')" key="reject">
              <template #icon>
                <CloseCircleOutlined />
              </template>驳回
            </a-menu-item>
            <a-menu-item v-if="hasPermission('revoke')" key="revoke">
              <template #icon>
                <RollbackOutlined />
              </template>撤销
            </a-menu-item>
            <a-menu-item v-if="hasPermission('transfer')" key="transfer">
              <template #icon>
                <SwapOutlined />
              </template>转办
            </a-menu-item>
            <a-menu-item v-if="hasPermission('delegate')" key="delegate">
              <template #icon>
                <SelectOutlined />
              </template>委托
            </a-menu-item>
            <a-menu-item v-if="hasPermission('flowChart')" key="flowChart">
              <template #icon>
                <NodeIndexOutlined />
              </template>流程图
            </a-menu-item>
            <a-menu-item v-if="hasPermission('flowLog')" key="flowLog">
              <template #icon>
                <HistoryOutlined />
              </template>流程日志
            </a-menu-item>
          </a-menu-item-group>

          <!-- 代码生成操作组 -->
          <a-menu-item-group title="代码生成" v-if="hasCodeGenOperations">
            <a-menu-item v-if="hasPermission('preview')" key="preview">
              <template #icon>
                <EyeOutlined />
              </template>预览
            </a-menu-item>
            <a-menu-item v-if="hasPermission('generate')" key="generate">
              <template #icon>
                <CodeOutlined />
              </template>生成代码
            </a-menu-item>
            <a-menu-item v-if="hasPermission('sync')" key="sync">
              <template #icon>
                <SyncOutlined />
              </template>同步
            </a-menu-item>
            <a-menu-item v-if="hasPermission('config')" key="config">
              <template #icon>
                <SettingOutlined />
              </template>配置
            </a-menu-item>
          </a-menu-item-group>

          <!-- 邮件操作组 -->
          <a-menu-item-group title="邮件操作" v-if="hasMailOperations">
            <a-menu-item v-if="hasPermission('sendMail')" key="sendMail">
              <template #icon>
                <MailOutlined />
              </template>发送邮件
            </a-menu-item>
            <a-menu-item v-if="hasPermission('sendTest')" key="sendTest">
              <template #icon>
                <BugOutlined />
              </template>发送测试邮件
            </a-menu-item>
          </a-menu-item-group>

          <!-- 通知操作组 -->
          <a-menu-item-group title="通知操作" v-if="hasNotifyOperations">
            <a-menu-item v-if="hasPermission('notify')" key="notify">
              <template #icon>
                <NotificationOutlined />
              </template>发送通知
            </a-menu-item>
            <a-menu-item v-if="hasPermission('message')" key="message">
              <template #icon>
                <MessageOutlined />
              </template>发送消息
            </a-menu-item>
            <a-menu-item v-if="hasPermission('subscribe')" key="subscribe">
              <template #icon>
                <BellOutlined />
              </template>订阅
            </a-menu-item>
          </a-menu-item-group>

          <!-- 其他操作组 -->
          <a-menu-item-group title="其他" v-if="hasOtherOperations">
            <a-menu-item v-if="hasPermission('log')" key="log">
              <template #icon>
                <ProfileOutlined />
              </template>日志
            </a-menu-item>
          </a-menu-item-group>
        </a-menu>
      </template>
    </a-dropdown>

    <!-- 自定义操作插槽 -->
    <slot name="operations"></slot>
  </a-space>
</template>

<script lang="ts" setup>
import {
  EyeOutlined,
  EditOutlined,
  DeleteOutlined,
  CopyOutlined,
  ExportOutlined,
  ImportOutlined,
  PrinterOutlined,
  DownloadOutlined,
  UploadOutlined,
  TeamOutlined,
  UsergroupAddOutlined,
  SafetyCertificateOutlined,
  DatabaseOutlined,
  SendOutlined,
  CheckCircleOutlined,
  CloseCircleOutlined,
  RollbackOutlined,
  SwapOutlined,
  SelectOutlined,
  NodeIndexOutlined,
  HistoryOutlined,
  CodeOutlined,
  SyncOutlined,
  SettingOutlined,
  MailOutlined,
  BugOutlined,
  NotificationOutlined,
  MessageOutlined,
  BellOutlined,
  ProfileOutlined,
  MoreOutlined,
  PlusOutlined
} from '@ant-design/icons-vue'
import { computed } from 'vue'
import type { MenuInfo } from 'ant-design-vue/es/menu/src/interface'

interface Props {
  // 记录数据
  record: Record<string, any>
  // 权限配置
  permissions?: string[]
}

// 所有可用的操作类型
const ALL_OPERATIONS = {
  // 基础操作
  basic: ['create', 'view', 'edit', 'delete', 'copy', 'clone', 'export', 'import', 'print', 'download', 'upload'],
  // 权限操作
  auth: ['assign', 'assignRole', 'authorize', 'dataScope'],
  // 工作流操作
  workflow: ['submit', 'approve', 'reject', 'revoke', 'transfer', 'delegate', 'flowChart', 'flowLog'],
  // 代码生成操作
  codeGen: ['preview', 'generate', 'sync', 'config'],
  // 邮件操作
  mail: ['sendMail', 'sendTest'],
  // 通知操作
  notify: ['notify', 'message', 'subscribe'],
  // 其他操作
  other: ['log']
} as const

const props = withDefaults(defineProps<Props>(), {
  permissions: () => ['view', 'edit', 'delete']
})

// 操作事件
const emit = defineEmits<{
  // 基础操作
  (e: 'create', record: Record<string, any>): void
  (e: 'view', record: Record<string, any>): void
  (e: 'edit', record: Record<string, any>): void
  (e: 'delete', record: Record<string, any>): void
  (e: 'copy', record: Record<string, any>): void
  (e: 'clone', record: Record<string, any>): void
  (e: 'export', record: Record<string, any>): void
  (e: 'import', record: Record<string, any>): void
  (e: 'print', record: Record<string, any>): void
  (e: 'download', record: Record<string, any>): void
  (e: 'upload', record: Record<string, any>): void
  // 权限操作
  (e: 'assign', record: Record<string, any>): void
  (e: 'assignRole', record: Record<string, any>): void
  (e: 'authorize', record: Record<string, any>): void
  (e: 'dataScope', record: Record<string, any>): void
  // 工作流操作
  (e: 'submit', record: Record<string, any>): void
  (e: 'approve', record: Record<string, any>): void
  (e: 'reject', record: Record<string, any>): void
  (e: 'revoke', record: Record<string, any>): void
  (e: 'transfer', record: Record<string, any>): void
  (e: 'delegate', record: Record<string, any>): void
  (e: 'flowChart', record: Record<string, any>): void
  (e: 'flowLog', record: Record<string, any>): void
  // 代码生成操作
  (e: 'preview', record: Record<string, any>): void
  (e: 'generate', record: Record<string, any>): void
  (e: 'sync', record: Record<string, any>): void
  (e: 'config', record: Record<string, any>): void
  // 邮件操作
  (e: 'sendMail', record: Record<string, any>): void
  (e: 'sendTest', record: Record<string, any>): void
  // 通知操作
  (e: 'notify', record: Record<string, any>): void
  (e: 'message', record: Record<string, any>): void
  (e: 'subscribe', record: Record<string, any>): void
  // 其他操作
  (e: 'log', record: Record<string, any>): void
}>()

// 判断是否有更多操作
const hasMoreOperations = computed(() => {
  return (Object.values(ALL_OPERATIONS) as unknown as string[][])
    .reduce((acc, val) => acc.concat(val), [] as string[])
    .some((op: string) => !['view', 'edit', 'delete', 'copy'].includes(op) && hasPermission(op))
})

// 判断各操作组是否显示
const hasBasicOperations = computed(() => ALL_OPERATIONS.basic.some(op => hasPermission(op)))
const hasAuthOperations = computed(() => ALL_OPERATIONS.auth.some(op => hasPermission(op)))
const hasWorkflowOperations = computed(() => ALL_OPERATIONS.workflow.some(op => hasPermission(op)))
const hasCodeGenOperations = computed(() => ALL_OPERATIONS.codeGen.some(op => hasPermission(op)))
const hasMailOperations = computed(() => ALL_OPERATIONS.mail.some(op => hasPermission(op)))
const hasNotifyOperations = computed(() => ALL_OPERATIONS.notify.some(op => hasPermission(op)))
const hasOtherOperations = computed(() => ALL_OPERATIONS.other.some(op => hasPermission(op)))

// 权限判断
const hasPermission = (permission: string) => {
  return props.permissions.includes(permission)
}

// 基础操作处理函数
const handleCreate = () => emit('create', props.record)
const handleView = () => emit('view', props.record)
const handleEdit = () => emit('edit', props.record)
const handleDelete = () => emit('delete', props.record)
const handleCopy = () => emit('copy', props.record)
const handleClone = () => emit('clone', props.record)

// 处理更多操作
const handleMoreOperation = (info: MenuInfo) => {
  emit(info.key as keyof typeof emit, props.record)
}
</script>

<style lang="less" scoped>
:deep(.ant-btn-link) {
  padding: 0 4px;
  height: 24px;

  .anticon {
    font-size: 14px;
  }
}

:deep(.ant-dropdown-menu-item-group-title) {
  color: rgba(0, 0, 0, 0.45);
  font-size: 12px;
  padding: 6px 12px;
}
</style>