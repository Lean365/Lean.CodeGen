import request from '@/utils/request';
import type { LeanOnlineUser } from '@/types/signalr/onlineUser';
import type { LeanApiResult, LeanPage, LeanPageResult } from '@/types/common/api';

// 获取在线用户列表
export function getOnlineUserList(params: LeanPage) {
  return request<LeanApiResult<LeanPageResult<LeanOnlineUser>>>({
    url: '/api/LeanOnlineUser/list',
    method: 'get',
    params
  });
}

// 获取在线用户详情
export function getOnlineUserInfo(userId: number) {
  return request<LeanApiResult<LeanOnlineUser>>({
    url: `/api/LeanOnlineUser/${userId}`,
    method: 'get'
  });
}

// 更新用户状态
export function updateOnlineUserStatus(userId: number, status: number) {
  return request<LeanApiResult<void>>({
    url: `/api/LeanOnlineUser/${userId}/status`,
    method: 'put',
    data: { status }
  });
}

// 更新用户信息
export function updateOnlineUserInfo(userId: number, data: { userName: string; avatar?: string }) {
  return request<LeanApiResult<void>>({
    url: `/api/LeanOnlineUser/${userId}/info`,
    method: 'put',
    data
  });
}

// 更新最后活动时间
export function updateOnlineUserLastActive(userId: number) {
  return request<LeanApiResult<void>>({
    url: `/api/LeanOnlineUser/${userId}/last-active`,
    method: 'put'
  });
}

// 获取用户连接ID
export function getOnlineUserConnection(userId: number) {
  return request<LeanApiResult<string>>({
    url: `/api/LeanOnlineUser/${userId}/connection`,
    method: 'get'
  });
}

// 加入用户组
export function joinOnlineUserGroup(groupName: string) {
  return request<LeanApiResult<void>>({
    url: `/api/LeanOnlineUser/groups/${groupName}/join`,
    method: 'post'
  });
}

// 离开用户组
export function leaveOnlineUserGroup(groupName: string) {
  return request<LeanApiResult<void>>({
    url: `/api/LeanOnlineUser/groups/${groupName}/leave`,
    method: 'post'
  });
} 