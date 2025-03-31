import type { LeanApiResult, LeanPage, LeanPageResult, LeanBaseEntity } from '@/types/common/api'

// 在线消息
export interface LeanOnlineMessage {
  id: number;
  senderId: number;
  senderName: string;
  senderAvatar?: string;
  receiverId: number;
  receiverName: string;
  receiverAvatar?: string;
  content: string;
  messageType: string;
  isRead: number;
  sendTime: string;
  createTime: string;
  updateTime?: string;
}

// 在线消息查询参数
export interface LeanOnlineMessageQuery extends LeanPage {
  senderId?: number;
  receiverId?: number;
  isRead?: number;
  messageType?: string;
  startTime?: string;
  endTime?: string;
}

// 在线消息发送参数
export interface LeanOnlineMessageSend {
  senderId: number;
  receiverId?: number;
  content: string;
  messageType?: string;
}

// 在线消息标记已读参数
export interface LeanOnlineMessageMarkAsRead {
  userId: number;
  senderId: number;
}

// 在线消息API
export namespace LeanOnlineMessageApi {
  // 发送消息
  export interface SendRequest {
    senderId: number;
    receiverId?: number;
    content: string;
    messageType?: string;
  }
  export type SendResponse = LeanApiResult<LeanOnlineMessage>

  // 发送群组消息
  export interface SendGroupRequest {
    groupName: string;
    content: string;
  }
  export type SendGroupResponse = LeanApiResult<LeanOnlineMessage>

  // 查询消息历史
  export interface QueryRequest extends LeanPage {
    userId: number;
  }
  export type QueryResponse = LeanApiResult<LeanPageResult<LeanOnlineMessage>>

  // 标记消息已读
  export interface MarkReadRequest {
    messageId: number;
  }
  export type MarkReadResponse = LeanApiResult<boolean>

  // 获取未读消息
  export interface GetUnreadRequest {
    userId: number;
  }
  export type GetUnreadResponse = LeanApiResult<LeanOnlineMessage[]>

  // 获取消息详情
  export interface GetDetailRequest {
    messageId: number;
  }
  export type GetDetailResponse = LeanApiResult<LeanOnlineMessage>

  // 群组管理
  export interface GroupManageRequest {
    groupName: string;
    userIds: number[];
  }
  export type GroupManageResponse = LeanApiResult<boolean>
} 