<template>
  <div class="app-container">
    <a-card :bordered="false">
      <a-row :gutter="24">
        <a-col :span="7">
          <a-card :bordered="false">
            <template #cover>
              <div class="avatar-wrapper" @click="handleChangeAvatar">
                <a-avatar :size="104" :src="userInfo.avatar" />
                <div class="avatar-upload">
                  <upload-outlined />
                  <div class="ant-upload-text">更换头像</div>
                </div>
              </div>
            </template>
            <template #actions>
              <div @click="handleChangePassword">
                <lock-outlined />
                修改密码
              </div>
            </template>
            <a-card-meta :title="userInfo.nickName">
              <template #description>
                <div>{{ userInfo.deptName }}</div>
                <div>{{ userInfo.email }}</div>
                <div>{{ userInfo.phoneNumber }}</div>
              </template>
            </a-card-meta>
          </a-card>
        </a-col>
        <a-col :span="17">
          <a-tabs v-model:activeKey="activeTabKey">
            <a-tab-pane key="basic" tab="基本信息">
              <a-form ref="formRef" :model="formData" :rules="rules" :label-col="{ span: 4 }"
                :wrapper-col="{ span: 19 }">
                <a-form-item label="用户昵称" name="nickName">
                  <a-input v-model:value="formData.nickName" placeholder="请输入用户昵称" />
                </a-form-item>
                <a-form-item label="手机号码" name="phoneNumber">
                  <a-input v-model:value="formData.phoneNumber" placeholder="请输入手机号码" />
                </a-form-item>
                <a-form-item label="邮箱" name="email">
                  <a-input v-model:value="formData.email" placeholder="请输入邮箱" />
                </a-form-item>
                <a-form-item label="性别" name="gender">
                  <business-select v-model:value="formData.gender" dict-type="sys_user_sex" placeholder="请选择性别" />
                </a-form-item>
                <a-form-item :wrapper-col="{ span: 19, offset: 4 }">
                  <a-button type="primary" :loading="loading" @click="handleSubmit">保存</a-button>
                </a-form-item>
              </a-form>
            </a-tab-pane>
            <a-tab-pane key="log" tab="操作日志">
              <div class="table-page-search-wrapper">
                <a-form layout="inline">
                  <a-row :gutter="48">
                    <a-col :md="8" :sm="24">
                      <a-form-item label="操作时间">
                        <a-range-picker v-model:value="dateRange" show-time :show-time-format="'HH:mm:ss'"
                          value-format="YYYY-MM-DD HH:mm:ss" :placeholder="['开始时间', '结束时间']" style="width: 100%" />
                      </a-form-item>
                    </a-col>
                    <a-col :md="8" :sm="24">
                      <span class="table-page-search-submitButtons">
                        <a-button type="primary" @click="handleSearch">查询</a-button>
                        <a-button style="margin-left: 8px" @click="handleReset">重置</a-button>
                      </span>
                    </a-col>
                  </a-row>
                </a-form>
              </div>
              <a-table :columns="logColumns" :data-source="logList" :loading="logLoading" :pagination="logPagination"
                @change="handleLogTableChange">
                <template #bodyCell="{ column, record }">
                  <template v-if="column.key === 'createTime'">
                    {{ formatDateTime(record.createTime) }}
                  </template>
                  <template v-else-if="column.key === 'operationStatus'">
                    <a-tag :color="record.operationStatus === 0 ? 'success' : 'error'">
                      {{ record.operationStatus === 0 ? '成功' : '失败' }}
                    </a-tag>
                  </template>
                </template>
              </a-table>
            </a-tab-pane>
          </a-tabs>
        </a-col>
      </a-row>
    </a-card>

    <!-- 修改密码弹窗 -->
    <change-password v-model:visible="changePasswordVisible" :user-id="userInfo.id" @success="handlePasswordChanged" />

    <!-- 修改头像弹窗 -->
    <user-avatar v-model:visible="changeAvatarVisible" @success="handleAvatarChanged" />
  </div>
</template>

<script lang="ts" setup>
import { ref, reactive, onMounted, watch } from 'vue'
import type { FormInstance } from 'ant-design-vue'
import type { RuleObject } from 'ant-design-vue/es/form/interface'
import { message } from 'ant-design-vue'
import { LockOutlined, UploadOutlined } from '@ant-design/icons-vue'
import { getUserAsync, updateUserAsync } from '@/api/identity/user'
import { getOperationLogList } from '@/api/audit/operationLog'
import { getLoginLogList } from '@/api/audit/loginLog'
import { getQuartzLogList } from '@/api/audit/quartzLog'
import { getSqlDiffLogList } from '@/api/audit/sqlDiffLog'
import { getAuditLogList } from '@/api/audit/auditLog'
import { getExceptionLogList } from '@/api/audit/exceptionLog'
import { formatDateTime } from '@/utils/formatter'
import type { LeanUserDto } from '@/types/identity/user'
import type { LeanOperationLogDto } from '@/types/audit/operationLog'
import type { LeanLoginLogDto } from '@/types/audit/loginLog'
import type { LeanQuartzLogDto } from '@/types/audit/quartzLog'
import type { LeanSqlDiffLogDto } from '@/types/audit/sqlDiffLog'
import type { LeanAuditLogDto } from '@/types/audit/auditLog'
import type { LeanExceptionLogDto } from '@/types/audit/exceptionLog'
import ChangePassword from './changePassword.vue'
import UserAvatar from './userAvatar.vue'

interface UserInfo {
  id: number
  userName: string
  nickName: string
  avatar: string
  email: string
  phoneNumber: string
  deptName: string
  gender: number
  [key: string]: any
}

const activeTabKey = ref('basic')
const changePasswordVisible = ref(false)
const changeAvatarVisible = ref(false)
const loading = ref(false)
const formRef = ref<FormInstance>()

const userInfo = ref<UserInfo>({
  id: 0,
  userName: '',
  nickName: '',
  avatar: '',
  email: '',
  phoneNumber: '',
  deptName: '',
  gender: 0
})

const formData = reactive({
  nickName: '',
  phoneNumber: '',
  email: '',
  gender: undefined as number | undefined
})

const rules: Record<string, RuleObject[]> = {
  nickName: [
    { required: true, message: '请输入用户昵称' },
    { min: 2, max: 20, message: '用户昵称长度必须介于 2 和 20 之间' }
  ],
  phoneNumber: [
    { pattern: /^1[3-9]\d{9}$/, message: '请输入正确的手机号码' }
  ],
  email: [
    { type: 'email', message: '请输入正确的邮箱地址' }
  ],
  gender: [
    { required: true, message: '请选择性别' }
  ]
}

// 日志相关
const logLoading = ref(false)
const logList = ref<LeanOperationLogDto[]>([])
const loginLogList = ref<LeanLoginLogDto[]>([])
const quartzLogList = ref<LeanQuartzLogDto[]>([])
const sqlDiffLogList = ref<LeanSqlDiffLogDto[]>([])
const auditLogList = ref<LeanAuditLogDto[]>([])
const exceptionLogList = ref<LeanExceptionLogDto[]>([])
const logPagination = reactive({
  current: 1,
  pageSize: 10,
  total: 0,
  showSizeChanger: true,
  showTotal: (total: number) => `共 ${total} 条`
})

const logColumns = [
  {
    title: '操作模块',
    dataIndex: 'module',
    key: 'module'
  },
  {
    title: '操作类型',
    dataIndex: 'operation',
    key: 'operation',
    width: 120
  },
  {
    title: '操作时间',
    dataIndex: 'createTime',
    key: 'createTime',
    width: 180
  },
  {
    title: '操作状态',
    dataIndex: 'operationStatus',
    key: 'operationStatus',
    width: 100
  },
  {
    title: '操作信息',
    dataIndex: 'errorMsg',
    key: 'errorMsg'
  }
]

// 日志查询参数
const dateRange = ref<[string, string]>(['', ''])

const loadUserInfo = async () => {
  try {
    const { data } = await getUserAsync(userInfo.value.id)
    if (data) {
      const userDto = data as LeanUserDto
      userInfo.value = {
        id: userDto.userId,
        userName: userDto.userName,
        nickName: userDto.nickName,
        avatar: userDto.avatar || '',
        email: userDto.email,
        phoneNumber: userDto.phoneNumber,
        deptName: '', // 暂时不显示部门名称，需要额外获取
        gender: userDto.gender
      }
      Object.assign(formData, {
        nickName: userDto.nickName,
        phoneNumber: userDto.phoneNumber,
        email: userDto.email,
        gender: userDto.gender || 0
      })
    }
  } catch (error) {
    console.error('获取用户信息失败：', error)
    message.error('获取用户信息失败')
  }
}

const loadLogList = async () => {
  try {
    logLoading.value = true
    const [startTime, endTime] = dateRange.value
    const response = await getOperationLogList({
      pageSize: logPagination.pageSize,
      pageIndex: logPagination.current,
      userId: userInfo.value.id,
      startTime,
      endTime
    })
    if (response?.data?.data) {
      logList.value = response.data.data
      logPagination.total = response.data.total || 0
    }
  } catch (error) {
    console.error('获取日志列表失败：', error)
    message.error('获取日志列表失败')
  } finally {
    logLoading.value = false
  }
}

const handleLogTableChange = (pagination: any) => {
  logPagination.current = pagination.current
  logPagination.pageSize = pagination.pageSize
  loadLogList()
}

const handleChangePassword = () => {
  changePasswordVisible.value = true
}

const handleChangeAvatar = () => {
  changeAvatarVisible.value = true
}

const handlePasswordChanged = () => {
  message.success('密码修改成功')
}

const handleAvatarChanged = (avatarUrl: string) => {
  userInfo.value.avatar = avatarUrl
  message.success('头像更新成功')
}

const handleSubmit = async () => {
  try {
    await formRef.value?.validate()
    loading.value = true

    await updateUserAsync({
      ...formData,
      id: userInfo.value.id,
      gender: formData.gender || 0,
      deptIds: [],
      postIds: [],
      roleIds: []
    })
    message.success('保存成功')
    await loadUserInfo()
  } catch (error) {
    console.error('保存失败：', error)
  } finally {
    loading.value = false
  }
}

const handleSearch = () => {
  logPagination.current = 1
  loadLogList()
}

const handleReset = () => {
  dateRange.value = ['', '']
  logPagination.current = 1
  loadLogList()
}

const loadLoginLogList = async () => {
  try {
    const response = await getLoginLogList({
      pageSize: logPagination.pageSize,
      pageIndex: logPagination.current,
      userId: userInfo.value.id
    })
    if (response?.data) {
      loginLogList.value = response.data
      logPagination.total = response.data.length || 0
    }
  } catch (error) {
    console.error('加载登录日志失败:', error)
  }
}

const loadQuartzLogList = async () => {
  try {
    const response = await getQuartzLogList({
      pageSize: logPagination.pageSize,
      pageIndex: logPagination.current
    })
    if (response?.data) {
      quartzLogList.value = response.data
      logPagination.total = response.data.length || 0
    }
  } catch (error) {
    console.error('加载定时任务日志失败:', error)
  }
}

const loadSqlDiffLogList = async () => {
  try {
    const response = await getSqlDiffLogList({
      pageSize: logPagination.pageSize,
      pageIndex: logPagination.current
    })
    if (response?.data) {
      sqlDiffLogList.value = response.data
      logPagination.total = response.data.length || 0
    }
  } catch (error) {
    console.error('加载SQL差异日志失败:', error)
  }
}

const loadAuditLogList = async () => {
  try {
    const response = await getAuditLogList({
      pageSize: logPagination.pageSize,
      pageIndex: logPagination.current,
      userId: userInfo.value.id
    })
    if (response?.data?.data) {
      auditLogList.value = response.data.data
      logPagination.total = response.data.total || 0
    }
  } catch (error) {
    console.error('加载审计日志失败:', error)
  }
}

const loadExceptionLogList = async () => {
  try {
    const response = await getExceptionLogList({
      pageSize: logPagination.pageSize,
      pageIndex: logPagination.current,
      userId: userInfo.value.id
    })
    if (response?.data?.data) {
      exceptionLogList.value = response.data.data
      logPagination.total = response.data.total || 0
    }
  } catch (error) {
    console.error('加载异常日志失败:', error)
  }
}

watch(
  () => activeTabKey.value,
  (newVal) => {
    if (newVal === 'log') {
      loadLogList()
    } else if (newVal === 'loginLog') {
      loadLoginLogList()
    } else if (newVal === 'quartzLog') {
      loadQuartzLogList()
    } else if (newVal === 'sqlDiffLog') {
      loadSqlDiffLogList()
    } else if (newVal === 'auditLog') {
      loadAuditLogList()
    } else if (newVal === 'exceptionLog') {
      loadExceptionLogList()
    }
  }
)

onMounted(() => {
  loadUserInfo()
})
</script>

<style lang="less" scoped>
.avatar-wrapper {
  position: relative;
  margin: 0 auto;
  width: 104px;
  height: 104px;
  cursor: pointer;

  &:hover .avatar-upload {
    opacity: 1;
  }

  .avatar-upload {
    position: absolute;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    color: #fff;
    background: rgba(0, 0, 0, 0.5);
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
    opacity: 0;
    transition: opacity 0.3s;
    border-radius: 50%;

    .ant-upload-text {
      margin-top: 8px;
      font-size: 12px;
    }
  }
}

.table-page-search-wrapper {
  padding-bottom: 24px;

  .table-page-search-submitButtons {
    display: block;
    margin-bottom: 24px;
    white-space: nowrap;
  }
}
</style>