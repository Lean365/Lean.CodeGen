import type { LeanApiResult, LeanPage, LeanPageResult, LeanBaseEntity } from '@/types/common/api'

// 在线用户信息
export interface LeanOnlineUser {
  id: number;
  userId: number;
  userName: string;
  avatar?: string;
  connectionId: string;
  deviceId: string;
  deviceName?: string;
  deviceType?: number;
  isOnline: number;
  lastActiveTime: string;
  createTime: string;
  updateTime?: string;
  ipAddress?: string;
  browser?: string;
  os?: string;
}

// 在线用户查询参数
export interface LeanOnlineUserQuery extends LeanPage {
  userId?: number;
  userName?: string;
  isOnline?: number;
  startTime?: string;
  endTime?: string;
}

// 在线用户状态更新参数
export interface LeanOnlineUserStatusUpdate {
  userId: number;
  isOnline: number;
}

// 在线用户信息更新参数
export interface LeanOnlineUserInfoUpdate {
  userId: number;
  userName: string;
  avatar?: string;
}

// 在线用户API
export namespace LeanOnlineUserApi {
  // 获取在线用户列表
  export interface GetListRequest extends LeanPage {
    userName?: string;
    isOnline?: number;
  }
  export type GetListResponse = LeanApiResult<LeanPageResult<LeanOnlineUser>>

  // 获取在线用户详情
  export interface GetDetailRequest {
    userId: number;
  }
  export type GetDetailResponse = LeanApiResult<LeanOnlineUser>

  // 更新在线状态
  export interface UpdateStatusRequest {
    userId: number;
    isOnline: number;
  }
  export type UpdateStatusResponse = LeanApiResult<void>

  // 更新用户信息
  export interface UpdateInfoRequest {
    userId: number;
    userName: string;
    avatar?: string;
  }
  export type UpdateInfoResponse = LeanApiResult<void>

  // 更新最后活动时间
  export interface UpdateLastActiveRequest {
    userId: number;
  }
  export type UpdateLastActiveResponse = LeanApiResult<void>

  // 获取连接ID
  export interface GetConnectionRequest {
    userId: number;
  }
  export type GetConnectionResponse = LeanApiResult<string>

  // 群组管理
  export interface GroupManageRequest {
    groupName: string;
    userIds: number[];
  }
  export type GroupManageResponse = LeanApiResult<void>
} 