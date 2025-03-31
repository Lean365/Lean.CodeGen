import { defineStore } from 'pinia';
import { ref } from 'vue';

interface Message {
  id: number;
  fromUserId: number;
  toUserId: number;
  content: string;
  sendTime: string;
  isRead: boolean;
}

export const useMessageStore = defineStore('message', () => {
  // 消息列表
  const messages = ref<Message[]>([]);
  // 未读消息
  const unreadMessages = ref<Message[]>([]);
  // 消息历史
  const messageHistory = ref<Message[]>([]);

  // 添加消息
  const addMessage = (message: Message) => {
    messages.value.push(message);
    if (!message.isRead) {
      unreadMessages.value.push(message);
    }
  };

  // 设置未读消息
  const setUnreadMessages = (messages: Message[]) => {
    unreadMessages.value = messages;
  };

  // 设置消息历史
  const setMessageHistory = (messages: Message[]) => {
    messageHistory.value = messages;
  };

  // 标记消息已读
  const markMessageAsRead = (messageId: number) => {
    const message = messages.value.find(m => m.id === messageId);
    if (message) {
      message.isRead = true;
    }
    unreadMessages.value = unreadMessages.value.filter(m => m.id !== messageId);
  };

  // 获取未读消息数量
  const getUnreadCount = () => {
    return unreadMessages.value.length;
  };

  // 清空消息
  const clearMessages = () => {
    messages.value = [];
    unreadMessages.value = [];
    messageHistory.value = [];
  };

  return {
    messages,
    unreadMessages,
    messageHistory,
    addMessage,
    setUnreadMessages,
    setMessageHistory,
    markMessageAsRead,
    getUnreadCount,
    clearMessages
  };
}); 