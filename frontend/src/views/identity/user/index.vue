<template>
  <div class="user-manage">
    <!-- 搜索区域 -->
    <LeanQuery :loading="loading" @search="handleSearch" @reset="handleReset">
      <a-form-item label="用户名">
        <a-input v-model:value="searchForm.username" placeholder="请输入用户名" />
      </a-form-item>
      <a-form-item label="状态">
        <LeanSelect v-model:value="searchForm.status" :options="statusOptions" placeholder="请选择状态" allow-clear />
      </a-form-item>
      <a-form-item label="创建时间">
        <a-range-picker v-model:value="searchForm.dateRange" />
      </a-form-item>
    </LeanQuery>

    <!-- 工具栏 -->
    <LeanToolbar>
      <template #left>
        <a-button type="primary" @click="handleAdd">
          <template #icon>
            <PlusOutlined />
          </template>
          新增用户
        </a-button>
        <a-button danger :disabled="!selectedRowKeys.length" @click="handleBatchDelete">
          <template #icon>
            <DeleteOutlined />
          </template>
          批量删除
        </a-button>
      </template>
      <template #right>
        <a-button @click="handleExport">
          <template #icon>
            <DownloadOutlined />
          </template>
          导出
        </a-button>
      </template>
    </LeanToolbar>

    <!-- 表格区域 -->
    <LeanTable :columns="columns" :data-source="tableData" :loading="loading" :current="current" :page-size="pageSize"
      :row-selection="rowSelection" @change="handleTableChange">
      <!-- 状态列 -->
      <template #status="{ record }">
        <a-tag :color="record.status === '1' ? 'success' : 'error'">
          {{ record.status === '1' ? '启用' : '禁用' }}
        </a-tag>
      </template>

      <!-- 操作列 -->
      <template #action="{ record }">
        <a-space>
          <a @click="handleEdit(record)">编辑</a>
          <a-divider type="vertical" />
          <a-popconfirm title="确定要删除该用户吗？" @confirm="handleDelete(record)">
            <a class="text-danger">删除</a>
          </a-popconfirm>
        </a-space>
      </template>
    </LeanTable>

    <!-- 分页区域 -->
    <LeanPagination v-model:current="current" v-model:pageSize="pageSize" :total="total" :loading="loading"
      @change="handlePageChange" />

    <!-- 用户表单弹窗 -->
    <LeanModal v-model:open="modalVisible" :title="modalTitle" :confirm-loading="submitLoading" @ok="handleSubmit"
      @cancel="handleCancel">
      <a-form ref="formRef" :model="formState" :rules="rules" :label-col="{ span: 4 }" :wrapper-col="{ span: 20 }">
        <a-form-item label="用户名" name="username">
          <a-input v-model:value="formState.username" placeholder="请输入用户名" />
        </a-form-item>
        <a-form-item label="密码" name="password" :rules="[{ required: !formState.id, message: '请输入密码' }]">
          <a-input-password v-model:value="formState.password" placeholder="请输入密码" />
        </a-form-item>
        <a-form-item label="状态" name="status">
          <LeanSelect v-model:value="formState.status" :options="statusOptions" placeholder="请选择状态" />
        </a-form-item>
        <a-form-item label="角色" name="roleIds">
          <LeanSelect v-model:value="formState.roleIds" :options="roleOptions" mode="multiple" placeholder="请选择角色" />
        </a-form-item>
      </a-form>
    </LeanModal>
  </div>
</template>

<script lang="ts" setup>
import { ref, computed } from 'vue'
import { message } from 'ant-design-vue'
import type { FormInstance } from 'ant-design-vue'
import type { Dayjs } from 'dayjs'
import { PlusOutlined, DeleteOutlined, DownloadOutlined } from '@ant-design/icons-vue'
import { getUserList, createUser, updateUser, deleteUser, batchDeleteUser } from '@/api/identity/user'
import { getRoleList } from '@/api/identity/role'

// 表格列配置
const columns = [
  {
    title: '序号',
    dataIndex: 'index',
    width: 60
  },
  {
    title: '用户名',
    dataIndex: 'username'
  },
  {
    title: '状态',
    dataIndex: 'status',
    slots: { customRender: 'status' }
  },
  {
    title: '创建时间',
    dataIndex: 'creationTime'
  },
  {
    title: '操作',
    dataIndex: 'action',
    slots: { customRender: 'action' },
    width: 120
  }
]

// 状态选项
const statusOptions = [
  { label: '启用', value: '1' },
  { label: '禁用', value: '0' }
]

// 角色选项
const roleOptions = ref<any[]>([])

// 数据
const tableData = ref<any[]>([])
const loading = ref(false)
const current = ref(1)
const pageSize = ref(10)
const total = ref(0)
const selectedRowKeys = ref<string[]>([])
const modalVisible = ref(false)
const submitLoading = ref(false)
const formRef = ref<FormInstance>()

// 搜索表单
const searchForm = ref<{
  username: string
  status: string | undefined
  dateRange: [Dayjs, Dayjs] | undefined
}>({
  username: '',
  status: undefined,
  dateRange: undefined
})

// 表单数据
const formState = ref({
  id: '',
  username: '',
  password: '',
  status: '1',
  roleIds: []
})

// 表单校验规则
const rules = {
  username: [{ required: true, message: '请输入用户名' }],
  status: [{ required: true, message: '请选择状态' }],
  roleIds: [{ required: true, message: '请选择角色' }]
}

// 表格选择配置
const rowSelection = {
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: string[]) => {
    selectedRowKeys.value = keys
  }
}

// 弹窗标题
const modalTitle = computed(() => formState.value.id ? '编辑用户' : '新增用户')

// 加载数据
const loadData = async () => {
  loading.value = true
  try {
    const res = await getUserList({
      page: current.value,
      pageSize: pageSize.value,
      username: searchForm.value.username,
      status: searchForm.value.status,
      dateRange: searchForm.value.dateRange
    })
    tableData.value = res.data.data
    total.value = res.data.total
  } finally {
    loading.value = false
  }
}

// 加载角色列表
const loadRoleList = async () => {
  const res = await getRoleList()
  roleOptions.value = res.data.data.map((item: any) => ({
    label: item.name,
    value: item.id
  }))
}

// 处理搜索
const handleSearch = () => {
  current.value = 1
  loadData()
}

// 处理重置
const handleReset = () => {
  searchForm.value = {
    username: '',
    status: undefined,
    dateRange: undefined
  }
  current.value = 1
  loadData()
}

// 处理表格变化
const handleTableChange = (pagination: any, filters: any, sorter: any) => {
  // 处理排序和筛选
  loadData()
}

// 处理分页变化
const handlePageChange = (page: number, size: number) => {
  current.value = page
  pageSize.value = size
  loadData()
}

// 处理新增
const handleAdd = () => {
  formState.value = {
    id: '',
    username: '',
    password: '',
    status: '1',
    roleIds: []
  }
  modalVisible.value = true
}

// 处理编辑
const handleEdit = (record: any) => {
  formState.value = {
    ...record,
    password: '' // 编辑时不显示密码
  }
  modalVisible.value = true
}

// 处理删除
const handleDelete = async (record: any) => {
  try {
    await deleteUser(record.id)
    message.success('删除成功')
    loadData()
  } catch (error) {
    console.error('删除失败：', error)
  }
}

// 处理批量删除
const handleBatchDelete = async () => {
  if (!selectedRowKeys.value.length) return
  try {
    await batchDeleteUser(selectedRowKeys.value)
    message.success('批量删除成功')
    selectedRowKeys.value = []
    loadData()
  } catch (error) {
    console.error('批量删除失败：', error)
  }
}

// 处理导出
const handleExport = () => {
  // 实现导出逻辑
}

// 处理表单提交
const handleSubmit = async () => {
  try {
    await formRef.value?.validate()
    submitLoading.value = true
    if (formState.value.id) {
      await updateUser(formState.value.id, formState.value)
      message.success('更新成功')
    } else {
      await createUser(formState.value)
      message.success('创建成功')
    }
    modalVisible.value = false
    loadData()
  } catch (error) {
    console.error('提交失败：', error)
  } finally {
    submitLoading.value = false
  }
}

// 处理取消
const handleCancel = () => {
  modalVisible.value = false
  formRef.value?.resetFields()
}

// 初始化
loadData()
loadRoleList()
</script>

<style lang="less" scoped>
.user-manage {
  padding: 16px;
  min-height: 100%;
}
</style>