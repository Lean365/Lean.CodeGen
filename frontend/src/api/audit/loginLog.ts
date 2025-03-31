import request from '@/utils/request'
import type {
  LeanLoginLogQueryDto,
  LeanLoginLogDto,
  LeanLoginLogExportDto,
  LeanLoginLogStatsDto
} from '@/types/audit/loginLog'

// 获取登录日志列表
export function getLoginLogList(params: LeanLoginLogQueryDto) {
  return request<LeanLoginLogDto[]>({
    url: '/audit/login/list',
    method: 'get',
    params
  })
}

// 获取登录日志详情
export function getLoginLog(id: number) {
  return request<LeanLoginLogDto>({
    url: `/audit/login/${id}`,
    method: 'get'
  })
}

// 删除登录日志
export function deleteLoginLog(ids: number[]) {
  return request({
    url: '/audit/login',
    method: 'delete',
    data: { ids }
  })
}

// 清空登录日志
export function clearLoginLog() {
  return request({
    url: '/audit/login/clear',
    method: 'delete'
  })
}

// 导出登录日志
export function exportLoginLog(params: LeanLoginLogExportDto) {
  return request({
    url: '/audit/login/export',
    method: 'get',
    params,
    responseType: 'blob'
  })
}

// 获取登录日志统计信息
export function getLoginLogStats(params?: { startTime?: string; endTime?: string }) {
  return request<LeanLoginLogStatsDto>({
    url: '/audit/login/stats',
    method: 'get',
    params
  })
}

// 获取在线用户列表
export function getOnlineUserList(params: { pageIndex: number; pageSize: number }) {
  return request<LeanLoginLogDto[]>({
    url: '/audit/login/online',
    method: 'get',
    params
  })
}

// 强制退出用户
export function forceLogout(userId: number) {
  return request({
    url: `/audit/login/force-logout/${userId}`,
    method: 'post'
  })
} 