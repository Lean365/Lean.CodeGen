import request from '@/utils/request'
import type { LeanWorkflowVariableValue, LeanWorkflowVariableValueQueryDto, LeanWorkflowVariableValueCreateDto, LeanWorkflowVariableValueUpdateDto, LeanWorkflowVariableValueDto } from '@/types/workflow/workflowVariableValue'

export function getWorkflowVariableValueList(params: LeanWorkflowVariableValueQueryDto) {
  return request<LeanWorkflowVariableValueDto[]>({
    url: '/workflow/variable-value/list',
    method: 'get',
    params
  })
}

export function getWorkflowVariableValue(id: number) {
  return request<LeanWorkflowVariableValueDto>({
    url: `/workflow/variable-value/${id}`,
    method: 'get'
  })
}

export function createWorkflowVariableValue(data: LeanWorkflowVariableValueCreateDto) {
  return request({
    url: '/workflow/variable-value',
    method: 'post',
    data
  })
}

export function updateWorkflowVariableValue(id: number, data: LeanWorkflowVariableValueUpdateDto) {
  return request({
    url: `/workflow/variable-value/${id}`,
    method: 'put',
    data
  })
}

export function deleteWorkflowVariableValue(ids: number[]) {
  return request({
    url: '/workflow/variable-value',
    method: 'delete',
    data: { ids }
  })
} 