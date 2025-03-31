import request from '@/utils/request'
import type {
  LeanQuartzLogQueryDto,
  LeanQuartzLogDto,
  LeanQuartzLogExportDto,
  LeanQuartzLogStatsDto
} from '@/types/audit/quartzLog'

// 获取定时任务日志列表
export function getQuartzLogList(params: LeanQuartzLogQueryDto) {
  return request<LeanQuartzLogDto[]>({
    url: '/audit/quartz/list',
    method: 'get',
    params
  })
}

// 获取定时任务日志详情
export function getQuartzLog(id: number) {
  return request<LeanQuartzLogDto>({
    url: `/audit/quartz/${id}`,
    method: 'get'
  })
}

// 删除定时任务日志
export function deleteQuartzLog(ids: number[]) {
  return request({
    url: '/audit/quartz',
    method: 'delete',
    data: { ids }
  })
}

// 清空定时任务日志
export function clearQuartzLog() {
  return request({
    url: '/audit/quartz/clear',
    method: 'delete'
  })
}

// 导出定时任务日志
export function exportQuartzLog(params: LeanQuartzLogExportDto) {
  return request({
    url: '/audit/quartz/export',
    method: 'get',
    params,
    responseType: 'blob'
  })
}

// 获取定时任务日志统计信息
export function getQuartzLogStats(params?: { startTime?: string; endTime?: string }) {
  return request<LeanQuartzLogStatsDto>({
    url: '/audit/quartz/stats',
    method: 'get',
    params
  })
} 