import request from '@/utils/request'
import type { LeanWorkflowOutput, LeanWorkflowOutputQueryDto, LeanWorkflowOutputCreateDto, LeanWorkflowOutputUpdateDto, LeanWorkflowOutputDto } from '@/types/workflow/workflowOutput'

export function getWorkflowOutputList(params: LeanWorkflowOutputQueryDto) {
  return request<LeanWorkflowOutputDto[]>({
    url: '/workflow/output/list',
    method: 'get',
    params
  })
}

export function getWorkflowOutput(id: number) {
  return request<LeanWorkflowOutputDto>({
    url: `/workflow/output/${id}`,
    method: 'get'
  })
}

export function createWorkflowOutput(data: LeanWorkflowOutputCreateDto) {
  return request({
    url: '/workflow/output',
    method: 'post',
    data
  })
}

export function updateWorkflowOutput(id: number, data: LeanWorkflowOutputUpdateDto) {
  return request({
    url: `/workflow/output/${id}`,
    method: 'put',
    data
  })
}

export function deleteWorkflowOutput(ids: number[]) {
  return request({
    url: '/workflow/output',
    method: 'delete',
    data: { ids }
  })
} 