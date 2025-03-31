import request from '@/utils/request';
import type { LeanOnlineMessage, LeanOnlineMessageSend, LeanOnlineMessageMarkAsRead } from '@/types/signalr/onlineMessage';
import type { LeanApiResult, LeanPage, LeanPageResult } from '@/types/common/api';

// 获取消息列表
export function getMessageList(params: LeanPage) {
  return request<LeanApiResult<LeanPageResult<LeanOnlineMessage>>>({
    url: '/api/LeanOnlineMessage',
    method: 'get',
    params
  });
}

// 获取消息详情
export function getMessageInfo(messageId: number) {
  return request<LeanApiResult<LeanOnlineMessage>>({
    url: `/api/LeanOnlineMessage/${messageId}`,
    method: 'get'
  });
}

// 发送消息
export function sendMessage(data: LeanOnlineMessageSend) {
  return request<LeanApiResult<LeanOnlineMessage>>({
    url: '/api/LeanOnlineMessage/send',
    method: 'post',
    data
  });
}

// 发送群组消息
export function sendGroupMessage(data: { groupName: string; content: string }) {
  return request<LeanApiResult<void>>({
    url: '/api/LeanOnlineMessage/group/send',
    method: 'post',
    data
  });
}

// 获取未读消息
export function getUnreadMessages(userId: number) {
  return request<LeanApiResult<LeanOnlineMessage[]>>({
    url: `/api/LeanOnlineMessage/unread/${userId}`,
    method: 'get'
  });
}

// 标记消息已读
export function markMessageAsRead(data: LeanOnlineMessageMarkAsRead) {
  return request<LeanApiResult<boolean>>({
    url: '/api/LeanOnlineMessage/read',
    method: 'post',
    data
  });
}

// 加入消息组
export function joinMessageGroup(groupName: string) {
  return request<LeanApiResult<void>>({
    url: `/api/LeanOnlineMessage/groups/${groupName}/join`,
    method: 'post'
  });
}

// 离开消息组
export function leaveMessageGroup(groupName: string) {
  return request<LeanApiResult<void>>({
    url: `/api/LeanOnlineMessage/groups/${groupName}/leave`,
    method: 'post'
  });
}

// 获取消息历史
export function getMessageHistory(userId: number, pageSize: number, pageIndex: number) {
  return request<LeanApiResult<LeanPageResult<LeanOnlineMessage>>>({
    url: `/api/LeanOnlineMessage/history/${userId}`,
    method: 'get',
    params: { pageSize, pageIndex }
  });
} 