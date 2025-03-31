import request from '@/utils/request'
import type { LeanWorkflowHistory, LeanWorkflowHistoryQueryDto, LeanWorkflowHistoryDto } from '@/types/workflow/workflowHistory'

export function getWorkflowHistoryList(params: LeanWorkflowHistoryQueryDto) {
  return request<LeanWorkflowHistoryDto[]>({
    url: '/workflow/history/list',
    method: 'get',
    params
  })
}

export function getWorkflowHistory(id: number) {
  return request<LeanWorkflowHistoryDto>({
    url: `/workflow/history/${id}`,
    method: 'get'
  })
}

export function deleteWorkflowHistory(ids: number[]) {
  return request({
    url: '/workflow/history',
    method: 'delete',
    data: { ids }
  })
} 