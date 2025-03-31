import request from '@/utils/request'
import type { LeanWorkflowVariable, LeanWorkflowVariableQueryDto, LeanWorkflowVariableCreateDto, LeanWorkflowVariableUpdateDto, LeanWorkflowVariableDto } from '@/types/workflow/workflowVariable'

export function getWorkflowVariableList(params: LeanWorkflowVariableQueryDto) {
  return request<LeanWorkflowVariableDto[]>({
    url: '/workflow/variable/list',
    method: 'get',
    params
  })
}

export function getWorkflowVariable(id: number) {
  return request<LeanWorkflowVariableDto>({
    url: `/workflow/variable/${id}`,
    method: 'get'
  })
}

export function createWorkflowVariable(data: LeanWorkflowVariableCreateDto) {
  return request({
    url: '/workflow/variable',
    method: 'post',
    data
  })
}

export function updateWorkflowVariable(id: number, data: LeanWorkflowVariableUpdateDto) {
  return request({
    url: `/workflow/variable/${id}`,
    method: 'put',
    data
  })
}

export function deleteWorkflowVariable(ids: number[]) {
  return request({
    url: '/workflow/variable',
    method: 'delete',
    data: { ids }
  })
} 