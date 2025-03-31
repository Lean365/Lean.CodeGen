import request from '@/utils/request'
import type { LeanWorkflowFlow, LeanWorkflowFlowQueryDto, LeanWorkflowFlowCreateDto, LeanWorkflowFlowUpdateDto, LeanWorkflowFlowChangeStatusDto, LeanWorkflowFlowDto } from '@/types/workflow/workflowFlow'

export function getWorkflowFlowList(params: LeanWorkflowFlowQueryDto) {
  return request<LeanWorkflowFlowDto[]>({
    url: '/workflow/flow/list',
    method: 'get',
    params
  })
}

export function getWorkflowFlow(id: number) {
  return request<LeanWorkflowFlowDto>({
    url: `/workflow/flow/${id}`,
    method: 'get'
  })
}

export function createWorkflowFlow(data: LeanWorkflowFlowCreateDto) {
  return request({
    url: '/workflow/flow',
    method: 'post',
    data
  })
}

export function updateWorkflowFlow(id: number, data: LeanWorkflowFlowUpdateDto) {
  return request({
    url: `/workflow/flow/${id}`,
    method: 'put',
    data
  })
}

export function deleteWorkflowFlow(ids: number[]) {
  return request({
    url: '/workflow/flow',
    method: 'delete',
    data: { ids }
  })
}

export function changeWorkflowFlowStatus(data: LeanWorkflowFlowChangeStatusDto) {
  return request({
    url: '/workflow/flow/status',
    method: 'put',
    data
  })
} 