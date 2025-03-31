import request from '@/utils/request'
import type {
  LeanOperationLogQueryDto,
  LeanOperationLogDto,
  LeanOperationLogExportDto,
  LeanOperationLogStatsDto
} from '@/types/audit/operationLog'

interface PageResult<T> {
  data: T[]
  total: number
}

// 获取操作日志列表
export function getOperationLogList(params: LeanOperationLogQueryDto) {
  return request<PageResult<LeanOperationLogDto>>({
    url: '/audit/operation/list',
    method: 'get',
    params
  })
}

// 获取操作日志详情
export function getOperationLog(id: number) {
  return request<LeanOperationLogDto>({
    url: `/audit/operation/${id}`,
    method: 'get'
  })
}

// 删除操作日志
export function deleteOperationLog(ids: number[]) {
  return request({
    url: '/audit/operation',
    method: 'delete',
    data: { ids }
  })
}

// 清空操作日志
export function clearOperationLog() {
  return request({
    url: '/audit/operation/clear',
    method: 'delete'
  })
}

// 导出操作日志
export function exportOperationLog(params: LeanOperationLogExportDto) {
  return request({
    url: '/audit/operation/export',
    method: 'get',
    params,
    responseType: 'blob'
  })
}

// 获取操作日志统计信息
export function getOperationLogStats(params?: { startTime?: string; endTime?: string }) {
  return request<LeanOperationLogStatsDto>({
    url: '/audit/operation/stats',
    method: 'get',
    params
  })
} 