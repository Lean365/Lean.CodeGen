import request from '@/utils/request'
import type { LeanWorkflowFormValue, LeanWorkflowFormValueQueryDto, LeanWorkflowFormValueCreateDto, LeanWorkflowFormValueUpdateDto, LeanWorkflowFormValueDto } from '@/types/workflow/workflowFormValue'

export function getWorkflowFormValueList(params: LeanWorkflowFormValueQueryDto) {
  return request<LeanWorkflowFormValueDto[]>({
    url: '/workflow/form-value/list',
    method: 'get',
    params
  })
}

export function getWorkflowFormValue(id: number) {
  return request<LeanWorkflowFormValueDto>({
    url: `/workflow/form-value/${id}`,
    method: 'get'
  })
}

export function createWorkflowFormValue(data: LeanWorkflowFormValueCreateDto) {
  return request({
    url: '/workflow/form-value',
    method: 'post',
    data
  })
}

export function updateWorkflowFormValue(id: number, data: LeanWorkflowFormValueUpdateDto) {
  return request({
    url: `/workflow/form-value/${id}`,
    method: 'put',
    data
  })
}

export function deleteWorkflowFormValue(ids: number[]) {
  return request({
    url: '/workflow/form-value',
    method: 'delete',
    data: { ids }
  })
} 