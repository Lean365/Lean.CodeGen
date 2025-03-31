import request from '@/utils/request'
import type {
  LeanSqlDiffLogQueryDto,
  LeanSqlDiffLogDto,
  LeanSqlDiffLogExportDto,
  LeanSqlDiffLogStatsDto
} from '@/types/audit/sqlDiffLog'

// 获取SQL差异日志列表
export function getSqlDiffLogList(params: LeanSqlDiffLogQueryDto) {
  return request<LeanSqlDiffLogDto[]>({
    url: '/audit/sqldiff/list',
    method: 'get',
    params
  })
}

// 获取SQL差异日志详情
export function getSqlDiffLog(id: number) {
  return request<LeanSqlDiffLogDto>({
    url: `/audit/sqldiff/${id}`,
    method: 'get'
  })
}

// 删除SQL差异日志
export function deleteSqlDiffLog(ids: number[]) {
  return request({
    url: '/audit/sqldiff',
    method: 'delete',
    data: { ids }
  })
}

// 清空SQL差异日志
export function clearSqlDiffLog() {
  return request({
    url: '/audit/sqldiff/clear',
    method: 'delete'
  })
}

// 导出SQL差异日志
export function exportSqlDiffLog(params: LeanSqlDiffLogExportDto) {
  return request({
    url: '/audit/sqldiff/export',
    method: 'get',
    params,
    responseType: 'blob'
  })
}

// 获取SQL差异日志统计信息
export function getSqlDiffLogStats(params?: { startTime?: string; endTime?: string }) {
  return request<LeanSqlDiffLogStatsDto>({
    url: '/audit/sqldiff/stats',
    method: 'get',
    params
  })
} 