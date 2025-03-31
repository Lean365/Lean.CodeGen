<template>
  <a-modal v-model:open="visible" title="发送消息" @ok="handleSendMessage">
    <a-form :model="form" :label-col="{ span: 4 }" :wrapper-col="{ span: 20 }">
      <a-form-item label="接收用户">
        {{ user?.userName }}
      </a-form-item>
      <a-form-item label="消息内容" required>
        <a-textarea v-model:value="form.content" :rows="4" placeholder="请输入消息内容" />
      </a-form-item>
    </a-form>
  </a-modal>
</template>

<script lang="ts" setup>
import { reactive } from 'vue';
import { message } from 'ant-design-vue';
import type { LeanOnlineUser } from '@/types/signalr/onlineUser';
import { sendMessage } from '@/api/signalr/onlineMessage';

const props = defineProps<{
  visible: boolean;
  user?: LeanOnlineUser;
}>();

const emit = defineEmits<{
  (e: 'update:visible', visible: boolean): void;
  (e: 'success'): void;
}>();

const form = reactive({
  content: ''
});

const handleSendMessage = async () => {
  if (!form.content || !props.user) {
    message.warning('请输入消息内容');
    return;
  }

  try {
    await sendMessage({
      content: form.content,
      toUserId: Number(props.user.userId)
    });
    message.success('发送成功');
    form.content = '';
    emit('success');
  } catch (error) {
    message.error('发送失败');
  }
};
</script>