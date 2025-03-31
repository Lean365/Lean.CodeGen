import request from '@/utils/request'
import type { LeanWorkflowOutcome, LeanWorkflowOutcomeQueryDto, LeanWorkflowOutcomeCreateDto, LeanWorkflowOutcomeUpdateDto, LeanWorkflowOutcomeDto } from '@/types/workflow/workflowOutcome'

export function getWorkflowOutcomeList(params: LeanWorkflowOutcomeQueryDto) {
  return request<LeanWorkflowOutcomeDto[]>({
    url: '/workflow/outcome/list',
    method: 'get',
    params
  })
}

export function getWorkflowOutcome(id: number) {
  return request<LeanWorkflowOutcomeDto>({
    url: `/workflow/outcome/${id}`,
    method: 'get'
  })
}

export function createWorkflowOutcome(data: LeanWorkflowOutcomeCreateDto) {
  return request({
    url: '/workflow/outcome',
    method: 'post',
    data
  })
}

export function updateWorkflowOutcome(id: number, data: LeanWorkflowOutcomeUpdateDto) {
  return request({
    url: `/workflow/outcome/${id}`,
    method: 'put',
    data
  })
}

export function deleteWorkflowOutcome(ids: number[]) {
  return request({
    url: '/workflow/outcome',
    method: 'delete',
    data: { ids }
  })
} 