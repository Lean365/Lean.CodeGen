import request from '@/utils/request'
import type { LeanWorkflowFormField, LeanWorkflowFormFieldQueryDto, LeanWorkflowFormFieldCreateDto, LeanWorkflowFormFieldUpdateDto, LeanWorkflowFormFieldDto } from '@/types/workflow/workflowFormField'

export function getWorkflowFormFieldList(params: LeanWorkflowFormFieldQueryDto) {
  return request<LeanWorkflowFormFieldDto[]>({
    url: '/workflow/form-field/list',
    method: 'get',
    params
  })
}

export function getWorkflowFormField(id: number) {
  return request<LeanWorkflowFormFieldDto>({
    url: `/workflow/form-field/${id}`,
    method: 'get'
  })
}

export function createWorkflowFormField(data: LeanWorkflowFormFieldCreateDto) {
  return request({
    url: '/workflow/form-field',
    method: 'post',
    data
  })
}

export function updateWorkflowFormField(id: number, data: LeanWorkflowFormFieldUpdateDto) {
  return request({
    url: `/workflow/form-field/${id}`,
    method: 'put',
    data
  })
}

export function deleteWorkflowFormField(ids: number[]) {
  return request({
    url: '/workflow/form-field',
    method: 'delete',
    data: { ids }
  })
} 