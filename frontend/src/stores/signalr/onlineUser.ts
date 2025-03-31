import { defineStore } from 'pinia';
import { ref } from 'vue';
import type { LeanOnlineUser } from '@/types/signalr/onlineUser';

export const useOnlineUserStore = defineStore('onlineUser', () => {
  // 在线用户列表
  const onlineUsers = ref<LeanOnlineUser[]>([]);
  // 用户在线状态映射
  const userOnlineStatus = ref<Record<string, number>>({});

  // 更新用户在线状态
  const updateUserOnlineStatus = (userId: number, isOnline: number) => {
    userOnlineStatus.value[userId] = isOnline;
  };

  // 更新在线用户列表
  const updateOnlineUsers = (users: LeanOnlineUser[]) => {
    onlineUsers.value = users;
    // 更新用户在线状态
    users.forEach(user => {
      userOnlineStatus.value[user.userId] = user.isOnline;
    });
  };

  // 检查用户是否在线
  const isUserOnline = (userId: number) => {
    return userOnlineStatus.value[userId] === 1;
  };

  // 获取在线用户数量
  const getOnlineUserCount = () => {
    return onlineUsers.value.length;
  };

  // 清空在线用户数据
  const clearOnlineUsers = () => {
    onlineUsers.value = [];
    userOnlineStatus.value = {};
  };

  return {
    onlineUsers,
    userOnlineStatus,
    updateUserOnlineStatus,
    updateOnlineUsers,
    isUserOnline,
    getOnlineUserCount,
    clearOnlineUsers
  };
}); 