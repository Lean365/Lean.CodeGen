import request from '@/utils/request'
import type { LeanWorkflowForm, LeanWorkflowFormQueryDto, LeanWorkflowFormCreateDto, LeanWorkflowFormUpdateDto, LeanWorkflowFormChangeStatusDto, LeanWorkflowFormDto } from '@/types/workflow/workflowForm'

export function getWorkflowFormList(params: LeanWorkflowFormQueryDto) {
  return request<LeanWorkflowFormDto[]>({
    url: '/workflow/form/list',
    method: 'get',
    params
  })
}

export function getWorkflowForm(id: number) {
  return request<LeanWorkflowFormDto>({
    url: `/workflow/form/${id}`,
    method: 'get'
  })
}

export function createWorkflowForm(data: LeanWorkflowFormCreateDto) {
  return request({
    url: '/workflow/form',
    method: 'post',
    data
  })
}

export function updateWorkflowForm(id: number, data: LeanWorkflowFormUpdateDto) {
  return request({
    url: `/workflow/form/${id}`,
    method: 'put',
    data
  })
}

export function deleteWorkflowForm(ids: number[]) {
  return request({
    url: '/workflow/form',
    method: 'delete',
    data: { ids }
  })
}

export function changeWorkflowFormStatus(data: LeanWorkflowFormChangeStatusDto) {
  return request({
    url: '/workflow/form/status',
    method: 'put',
    data
  })
} 