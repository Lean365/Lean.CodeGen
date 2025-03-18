<template>
  <div class="user-container">
    <!-- 搜索工具栏 -->
    <div class="toolbar">
      <a-form layout="inline" :model="searchForm">
        <a-form-item>
          <a-input v-model:value="searchForm.username" :placeholder="t('user.search.username')" />
        </a-form-item>
        <a-form-item>
          <a-select v-model:value="searchForm.status" style="width: 120px">
            <a-select-option value="">{{ t('user.search.status.all') }}</a-select-option>
            <a-select-option value="enabled">{{ t('user.search.status.enabled') }}</a-select-option>
            <a-select-option value="disabled">{{ t('user.search.status.disabled') }}</a-select-option>
          </a-select>
        </a-form-item>
        <a-form-item>
          <a-button type="primary" @click="handleSearch">
            <template #icon>
              <SearchOutlined />
            </template>
            {{ t('common.search') }}
          </a-button>
          <a-button style="margin-left: 8px" @click="handleReset">
            <template #icon>
              <ReloadOutlined />
            </template>
            {{ t('common.reset') }}
          </a-button>
        </a-form-item>
      </a-form>

      <a-button type="primary" @click="handleAdd">
        <template #icon>
          <PlusOutlined />
        </template>
        {{ t('user.add') }}
      </a-button>
    </div>

    <!-- 用户列表 -->
    <a-table :columns="columns" :data-source="users" :loading="loading" :pagination="pagination"
      @change="handleTableChange">
      <!-- 用户名列 -->
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'username'">
          <a @click="handleEdit(record)">{{ record.username }}</a>
        </template>

        <!-- 状态列 -->
        <template v-else-if="column.key === 'status'">
          <a-tag :color="record.status === 'enabled' ? 'success' : 'error'">
            {{ t(\`user.status.\${record.status}\`) }}
          </a-tag>
        </template>

        <!-- 操作列 -->
        <template v-else-if="column.key === 'action'">
          <a-space>
            <a-button type="link" @click="handleEdit(record)">
              {{ t('common.edit') }}
            </a-button>
            <a-button type="link" @click="handleDelete(record)">
              {{ t('common.delete') }}
            </a-button>
          </a-space>
        </template>
      </template>
    </a-table>

    <!-- 用户表单对话框 -->
    <a-modal v-model:visible="modalVisible" :title="modalTitle" @ok="handleModalOk" @cancel="handleModalCancel">
      <a-form ref="userFormRef" :model="userForm" :rules="userRules" :label-col="{ span: 6 }"
        :wrapper-col="{ span: 16 }">
        <a-form-item name="username" :label="t('user.form.username')">
          <a-input v-model:value="userForm.username" />
        </a-form-item>
        <a-form-item v-if="!userForm.id" name="password" :label="t('user.form.password')">
          <a-input-password v-model:value="userForm.password" />
        </a-form-item>
        <a-form-item name="email" :label="t('user.form.email')">
          <a-input v-model:value="userForm.email" />
        </a-form-item>
        <a-form-item name="status" :label="t('user.form.status')">
          <a-switch v-model:checked="userForm.status" :checked-value="'enabled'" :un-checked-value="'disabled'" />
        </a-form-item>
      </a-form>
    </a-modal>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive } from 'vue'
import { useI18n } from 'vue-i18n'
import type { FormInstance } from 'ant-design-vue'
import {
  SearchOutlined,
  ReloadOutlined,
  PlusOutlined
} from '@ant-design/icons-vue'

const { t } = useI18n()

// 搜索表单
const searchForm = reactive({
  username: '',
  status: ''
})

// 表格列定义
const columns = [
  {
    title: t('user.table.username'),
    dataIndex: 'username',
    key: 'username',
    sorter: true
  },
  {
    title: t('user.table.email'),
    dataIndex: 'email',
    key: 'email'
  },
  {
    title: t('user.table.status'),
    dataIndex: 'status',
    key: 'status',
    filters: [
      { text: t('user.status.enabled'), value: 'enabled' },
      { text: t('user.status.disabled'), value: 'disabled' }
    ]
  },
  {
    title: t('user.table.lastLoginTime'),
    dataIndex: 'lastLoginTime',
    key: 'lastLoginTime',
    sorter: true
  },
  {
    title: t('common.action'),
    key: 'action',
    width: 150
  }
]

// 表格数据
const loading = ref(false)
const users = ref([])
const pagination = reactive({
  current: 1,
  pageSize: 10,
  total: 0,
  showSizeChanger: true,
  showQuickJumper: true
})

// 表单对话框
const modalVisible = ref(false)
const modalTitle = ref('')
const userFormRef = ref<FormInstance>()
const userForm = reactive({
  id: '',
  username: '',
  password: '',
  email: '',
  status: 'enabled'
})

// 表单验证规则
const userRules = {
  username: [
    { required: true, message: t('user.rules.username.required') },
    { min: 2, max: 20, message: t('user.rules.username.length') }
  ],
  password: [
    { required: true, message: t('user.rules.password.required') },
    { min: 6, max: 20, message: t('user.rules.password.length') }
  ],
  email: [
    { required: true, message: t('user.rules.email.required') },
    { type: 'email', message: t('user.rules.email.format') }
  ]
}

// 搜索处理
const handleSearch = () => {
  pagination.current = 1
  fetchUsers()
}

// 重置搜索
const handleReset = () => {
  searchForm.username = ''
  searchForm.status = ''
  handleSearch()
}

// 表格变化处理
const handleTableChange = (pag: any, filters: any, sorter: any) => {
  pagination.current = pag.current
  pagination.pageSize = pag.pageSize
  fetchUsers()
}

// 添加用户
const handleAdd = () => {
  Object.assign(userForm, {
    id: '',
    username: '',
    password: '',
    email: '',
    status: 'enabled'
  })
  modalTitle.value = t('user.add')
  modalVisible.value = true
}

// 编辑用户
const handleEdit = (record: any) => {
  Object.assign(userForm, record)
  modalTitle.value = t('user.edit')
  modalVisible.value = true
}

// 删除用户
const handleDelete = async (record: any) => {
  // TODO: 实现删除逻辑
}

// 对话框确认
const handleModalOk = async () => {
  try {
    await userFormRef.value?.validate()
    // TODO: 实现保存逻辑
    modalVisible.value = false
    fetchUsers()
  } catch (error) {
    console.error('表单验证失败:', error)
  }
}

// 对话框取消
const handleModalCancel = () => {
  modalVisible.value = false
  userFormRef.value?.resetFields()
}

// 获取用户列表
const fetchUsers = async () => {
  try {
    loading.value = true
    // TODO: 实现获取用户列表逻辑
  } finally {
    loading.value = false
  }
}

// 初始化
fetchUsers()
</script>

<style lang="less" scoped>
.user-container {
  .toolbar {
    display: flex;
    justify-content: space-between;
    margin-bottom: 16px;
  }
}
</style>