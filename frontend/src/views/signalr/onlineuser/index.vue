<template>
  <div class="online-user-container">
    <!-- 查询区域 -->
    <lean-query>
      <lean-form-item label="用户名">
        <lean-input v-model="queryParams.userName" placeholder="请输入用户名" />
      </lean-form-item>
    </lean-query>

    <!-- 工具栏 -->
    <lean-toolbar>
      <template #left>
        <lean-button type="primary" @click="handleRefresh">刷新</lean-button>
      </template>
    </lean-toolbar>

    <!-- 表格区域 -->
    <lean-table v-model:selectedKeys="selectedKeys" :columns="columns" :data-source="userList" :loading="loading"
      :pagination="pagination" @change="handleTableChange">
      <template #bodyCell="{ column, record }">
        <!-- 状态列 -->
        <template v-if="column.key === 'status'">
          <lean-tag :type="record.isOnline ? 'success' : 'error'">
            {{ record.isOnline ? '在线' : '离线' }}
          </lean-tag>
        </template>

        <!-- 操作列 -->
        <template v-if="column.key === 'action'">
          <lean-space>
            <lean-button type="link" @click="handleViewDetails(record)">查看</lean-button>
            <lean-button type="link" @click="handleOpenMessageModal(record)">发送消息</lean-button>
            <lean-dropdown>
              <lean-button type="link">
                更多 <lean-icon type="down" />
              </lean-button>
              <template #overlay>
                <lean-menu>
                  <lean-menu-item @click="handleUpdateStatus(record)">
                    更新状态
                  </lean-menu-item>
                  <lean-menu-item @click="handleUpdateInfo(record)">
                    更新信息
                  </lean-menu-item>
                </lean-menu>
              </template>
            </lean-dropdown>
          </lean-space>
        </template>
      </template>
    </lean-table>

    <!-- 分页 -->
    <lean-pagination v-model:current="pagination.current" v-model:pageSize="pagination.pageSize"
      :total="pagination.total" @change="handlePageChange" />

    <!-- 用户详情弹窗 -->
    <lean-modal v-model:open="detailsVisible" title="用户详情" :footer="null" width="600px">
      <lean-descriptions bordered>
        <lean-descriptions-item label="用户ID">{{ selectedUser?.userId }}</lean-descriptions-item>
        <lean-descriptions-item label="用户名">{{ selectedUser?.userName }}</lean-descriptions-item>
        <lean-descriptions-item label="状态">
          <lean-tag :type="selectedUser?.isOnline ? 'success' : 'error'">
            {{ selectedUser?.isOnline ? '在线' : '离线' }}
          </lean-tag>
        </lean-descriptions-item>
        <lean-descriptions-item label="最后活动时间">
          {{ selectedUser?.lastActiveTime }}
        </lean-descriptions-item>
        <lean-descriptions-item label="连接ID">
          {{ selectedUser?.connectionId }}
        </lean-descriptions-item>
      </lean-descriptions>
    </lean-modal>

    <!-- 发送消息弹窗 -->
    <lean-modal v-model:open="messageVisible" title="发送消息" @ok="handleSendMessage">
      <lean-form :model="messageForm">
        <lean-form-item label="接收用户">
          {{ selectedUser?.userName }}
        </lean-form-item>
        <lean-form-item label="消息内容" required>
          <lean-textarea v-model="messageForm.content" :rows="4" placeholder="请输入消息内容" />
        </lean-form-item>
      </lean-form>
    </lean-modal>
  </div>
</template>

<script lang="ts" setup>
import { ref, onMounted, reactive } from 'vue';
import type { LeanOnlineUser } from '@/types/signalr/onlineUser';
import {
  getOnlineUserList,
  getOnlineUserInfo,
  updateOnlineUserStatus
} from '@/api/signalr/onlineUser';
import { sendMessage } from '@/api/signalr/onlineMessage';

// 表格列定义
const columns = [
  {
    title: '用户ID',
    dataIndex: 'userId',
    key: 'userId'
  },
  {
    title: '用户名',
    dataIndex: 'userName',
    key: 'userName'
  },
  {
    title: '状态',
    dataIndex: 'status',
    key: 'status'
  },
  {
    title: '最后活动时间',
    dataIndex: 'lastActiveTime',
    key: 'lastActiveTime'
  },
  {
    title: '操作',
    key: 'action',
    width: 200
  }
];

// 查询参数
const queryParams = reactive({
  userName: '',
  pageSize: 10,
  pageIndex: 1
});

// 数据和状态
const loading = ref(false);
const userList = ref<LeanOnlineUser[]>([]);
const selectedKeys = ref<string[]>([]);
const pagination = reactive({
  current: 1,
  pageSize: 10,
  total: 0
});

// 弹窗控制
const detailsVisible = ref(false);
const messageVisible = ref(false);
const selectedUser = ref<LeanOnlineUser>();
const messageForm = reactive({
  content: ''
});

// 获取用户列表
const fetchUserList = async () => {
  loading.value = true;
  try {
    const res = await getOnlineUserList({
      ...queryParams,
      pageIndex: pagination.current,
      pageSize: pagination.pageSize
    });
    if (res.data.success && res.data.data) {
      userList.value = res.data.data.items;
      pagination.total = res.data.data.total;
    }
  } finally {
    loading.value = false;
  }
};

// 刷新
const handleRefresh = () => {
  fetchUserList();
};

// 页面变化
const handlePageChange = (page: number, pageSize: number) => {
  pagination.current = page;
  pagination.pageSize = pageSize;
  fetchUserList();
};

// 表格变化
const handleTableChange = () => {
  fetchUserList();
};

// 查看详情
const handleViewDetails = async (record: LeanOnlineUser) => {
  const res = await getOnlineUserInfo(record.userId);
  if (res.data.success && res.data.data) {
    selectedUser.value = res.data.data;
    detailsVisible.value = true;
  }
};

// 打开消息弹窗
const handleOpenMessageModal = (record: LeanOnlineUser) => {
  selectedUser.value = record;
  messageVisible.value = true;
};

// 发送消息
const handleSendMessage = async () => {
  if (!messageForm.content || !selectedUser.value) {
    return;
  }

  const res = await sendMessage({
    content: messageForm.content,
    toUserId: Number(selectedUser.value.userId)
  });

  if (res.data.success) {
    messageVisible.value = false;
    messageForm.content = '';
  }
};

// 更新状态
const handleUpdateStatus = async (record: LeanOnlineUser) => {
  const res = await updateOnlineUserStatus(
    record.userId,
    record.isOnline ? 0 : 1
  );
  if (res.data.success) {
    fetchUserList();
  }
};

// 更新信息
const handleUpdateInfo = (record: LeanOnlineUser) => {
  // TODO: 实现更新用户信息的逻辑
};

onMounted(() => {
  fetchUserList();
});
</script>

<style scoped>
.online-user-container {
  padding: 24px;
}
</style>