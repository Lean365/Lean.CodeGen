import request from '@/utils/request'
import type { LeanAuditLogQueryDto } from '@/types/audit/auditLog'

// 获取审计日志列表
export function getAuditLogList(params: LeanAuditLogQueryDto) {
  return request({
    url: '/audit/audit',
    method: 'get',
    params
  })
}

// 获取审计日志详情
export function getAuditLogDetail(id: number) {
  return request({
    url: `/audit/audit/${id}`,
    method: 'get'
  })
}

// 导出审计日志
export function exportAuditLog(params: LeanAuditLogQueryDto) {
  return request({
    url: '/audit/audit/export',
    method: 'get',
    params,
    responseType: 'blob'
  })
} 