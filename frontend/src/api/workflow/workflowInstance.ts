import request from '@/utils/request'
import type { LeanWorkflowInstance, LeanWorkflowInstanceQueryDto, LeanWorkflowInstanceCreateDto, LeanWorkflowInstanceDto } from '@/types/workflow/workflowInstance'

export function getWorkflowInstanceList(params: LeanWorkflowInstanceQueryDto) {
  return request<LeanWorkflowInstanceDto[]>({
    url: '/workflow/instance/list',
    method: 'get',
    params
  })
}

export function getWorkflowInstance(id: number) {
  return request<LeanWorkflowInstanceDto>({
    url: `/workflow/instance/${id}`,
    method: 'get'
  })
}

export function createWorkflowInstance(data: LeanWorkflowInstanceCreateDto) {
  return request({
    url: '/workflow/instance',
    method: 'post',
    data
  })
}

export function deleteWorkflowInstance(ids: number[]) {
  return request({
    url: '/workflow/instance',
    method: 'delete',
    data: { ids }
  })
}

export function stopWorkflowInstance(id: number) {
  return request({
    url: `/workflow/instance/stop/${id}`,
    method: 'put'
  })
}

export function retryWorkflowInstance(id: number) {
  return request({
    url: `/workflow/instance/retry/${id}`,
    method: 'put'
  })
} 