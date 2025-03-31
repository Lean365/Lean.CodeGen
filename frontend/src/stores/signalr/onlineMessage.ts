import { defineStore } from 'pinia';
import { ref } from 'vue';
import type { LeanOnlineMessage } from '@/types/signalr/onlineMessage';

export const useOnlineMessageStore = defineStore('onlineMessage', () => {
  // 消息列表
  const messages = ref<LeanOnlineMessage[]>([]);
  // 未读消息数量
  const unreadCount = ref<number>(0);

  // 更新消息列表
  const updateMessages = (newMessages: LeanOnlineMessage[]) => {
    messages.value = newMessages;
  };

  // 添加新消息
  const addMessage = (message: LeanOnlineMessage) => {
    messages.value.push(message);
  };

  // 更新未读消息数量
  const updateUnreadCount = (count: number) => {
    unreadCount.value = count;
  };

  // 标记消息已读
  const markMessageAsRead = (messageId: number) => {
    const message = messages.value.find(m => m.id === messageId);
    if (message) {
      message.isRead = 1;
    }
  };

  // 清空消息数据
  const clearMessages = () => {
    messages.value = [];
    unreadCount.value = 0;
  };

  return {
    messages,
    unreadCount,
    updateMessages,
    addMessage,
    updateUnreadCount,
    markMessageAsRead,
    clearMessages
  };
}); 