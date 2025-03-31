<template>
  <div class="online-message-container">
    <!-- 查询区域 -->
    <lean-query>
      <lean-form-item label="发送者">
        <lean-input v-model="queryParams.fromUserName" placeholder="请输入发送者用户名" />
      </lean-form-item>
      <lean-form-item label="接收者">
        <lean-input v-model="queryParams.toUserName" placeholder="请输入接收者用户名" />
      </lean-form-item>
      <lean-form-item label="消息类型">
        <lean-select v-model="queryParams.messageType" placeholder="请选择消息类型">
          <lean-select-option value="1">个人消息</lean-select-option>
          <lean-select-option value="2">群组消息</lean-select-option>
        </lean-select>
      </lean-form-item>
    </lean-query>

    <!-- 工具栏 -->
    <lean-toolbar>
      <template #left>
        <lean-button type="primary" @click="handleRefresh">刷新</lean-button>
      </template>
    </lean-toolbar>

    <!-- 表格区域 -->
    <lean-table v-model:selectedKeys="selectedKeys" :columns="columns" :data-source="messageList" :loading="loading"
      :pagination="pagination" @change="handleTableChange">
      <template #bodyCell="{ column, record }">
        <!-- 消息类型列 -->
        <template v-if="column.key === 'messageType'">
          <lean-tag :type="record.messageType === 1 ? 'primary' : 'success'">
            {{ record.messageType === 1 ? '个人消息' : '群组消息' }}
          </lean-tag>
        </template>

        <!-- 状态列 -->
        <template v-if="column.key === 'status'">
          <lean-tag :type="record.status === 1 ? 'success' : 'warning'">
            {{ record.status === 1 ? '已读' : '未读' }}
          </lean-tag>
        </template>

        <!-- 操作列 -->
        <template v-if="column.key === 'action'">
          <lean-space>
            <lean-button type="link" @click="handleViewDetails(record)">查看</lean-button>
            <lean-button v-if="record.status === 0" type="link" @click="handleMarkRead(record)">标记已读</lean-button>
          </lean-space>
        </template>
      </template>
    </lean-table>

    <!-- 分页 -->
    <lean-pagination v-model:current="pagination.current" v-model:pageSize="pagination.pageSize"
      :total="pagination.total" @change="handlePageChange" />

    <!-- 消息详情弹窗 -->
    <lean-modal v-model:open="detailsVisible" title="消息详情" :footer="null" width="600px">
      <lean-descriptions bordered>
        <lean-descriptions-item label="消息ID">{{ selectedMessage?.id }}</lean-descriptions-item>
        <lean-descriptions-item label="发送者">{{ selectedMessage?.senderName }}</lean-descriptions-item>
        <lean-descriptions-item label="接收者">{{ selectedMessage?.receiverName }}</lean-descriptions-item>
        <lean-descriptions-item label="消息类型">
          <lean-tag :type="selectedMessage?.messageType === '1' ? 'primary' : 'success'">
            {{ selectedMessage?.messageType === '1' ? '个人消息' : '群组消息' }}
          </lean-tag>
        </lean-descriptions-item>
        <lean-descriptions-item label="发送时间">{{ selectedMessage?.sendTime }}</lean-descriptions-item>
        <lean-descriptions-item label="状态">
          <lean-tag :type="selectedMessage?.isRead === 1 ? 'success' : 'warning'">
            {{ selectedMessage?.isRead === 1 ? '已读' : '未读' }}
          </lean-tag>
        </lean-descriptions-item>
        <lean-descriptions-item label="消息内容">
          {{ selectedMessage?.content }}
        </lean-descriptions-item>
      </lean-descriptions>
    </lean-modal>
  </div>
</template>

<script lang="ts" setup>
import { ref, onMounted, reactive } from 'vue';
import type { LeanOnlineMessage } from '@/types/signalr/onlineMessage';
import {
  getMessageList,
  getMessageInfo,
  markMessageRead
} from '@/api/signalr/onlineMessage';

// 表格列定义
const columns = [
  {
    title: '消息ID',
    dataIndex: 'messageId',
    key: 'messageId'
  },
  {
    title: '发送者',
    dataIndex: 'fromUserName',
    key: 'fromUserName'
  },
  {
    title: '接收者',
    dataIndex: 'toUserName',
    key: 'toUserName'
  },
  {
    title: '消息类型',
    dataIndex: 'messageType',
    key: 'messageType'
  },
  {
    title: '消息内容',
    dataIndex: 'content',
    key: 'content',
    ellipsis: true
  },
  {
    title: '发送时间',
    dataIndex: 'sendTime',
    key: 'sendTime'
  },
  {
    title: '状态',
    dataIndex: 'status',
    key: 'status'
  },
  {
    title: '操作',
    key: 'action',
    width: 150
  }
];

// 查询参数
const queryParams = reactive({
  fromUserName: '',
  toUserName: '',
  messageType: undefined,
  pageSize: 10,
  pageIndex: 1
});

// 数据和状态
const loading = ref(false);
const messageList = ref<LeanOnlineMessage[]>([]);
const selectedKeys = ref<string[]>([]);
const pagination = reactive({
  current: 1,
  pageSize: 10,
  total: 0
});

// 弹窗控制
const detailsVisible = ref(false);
const selectedMessage = ref<LeanOnlineMessage>();

// 获取消息列表
const fetchMessageList = async () => {
  loading.value = true;
  try {
    const res = await getMessageList({
      ...queryParams,
      pageIndex: pagination.current,
      pageSize: pagination.pageSize
    });
    const { code, data } = res.data;
    if (code === 200 && data) {
      messageList.value = data.items;
      pagination.total = data.total;
    }
  } finally {
    loading.value = false;
  }
};

// 刷新
const handleRefresh = () => {
  fetchMessageList();
};

// 页面变化
const handlePageChange = (page: number, pageSize: number) => {
  pagination.current = page;
  pagination.pageSize = pageSize;
  fetchMessageList();
};

// 表格变化
const handleTableChange = () => {
  fetchMessageList();
};

// 查看详情
const handleViewDetails = async (record: LeanOnlineMessage) => {
  const res = await getMessageInfo(record.id);
  const { code, data } = res.data;
  if (code === 200 && data) {
    selectedMessage.value = data;
    detailsVisible.value = true;
  }
};

// 标记已读
const handleMarkRead = async (record: LeanOnlineMessage) => {
  const res = await markMessageRead(record.id);
  const { code } = res.data;
  if (code === 200) {
    fetchMessageList();
  }
};

onMounted(() => {
  fetchMessageList();
});
</script>

<style scoped>
.online-message-container {
  padding: 24px;
}
</style>