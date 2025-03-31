import request from '@/utils/request'
import type { LeanWorkflowFormRuleType, LeanWorkflowFormRuleTypeQueryDto, LeanWorkflowFormRuleTypeCreateDto, LeanWorkflowFormRuleTypeUpdateDto, LeanWorkflowFormRuleTypeDto } from '@/types/workflow/workflowFormRuleType'

export function getWorkflowFormRuleTypeList(params: LeanWorkflowFormRuleTypeQueryDto) {
  return request<LeanWorkflowFormRuleTypeDto[]>({
    url: '/workflow/form-rule-type/list',
    method: 'get',
    params
  })
}

export function getWorkflowFormRuleType(id: number) {
  return request<LeanWorkflowFormRuleTypeDto>({
    url: `/workflow/form-rule-type/${id}`,
    method: 'get'
  })
}

export function createWorkflowFormRuleType(data: LeanWorkflowFormRuleTypeCreateDto) {
  return request({
    url: '/workflow/form-rule-type',
    method: 'post',
    data
  })
}

export function updateWorkflowFormRuleType(id: number, data: LeanWorkflowFormRuleTypeUpdateDto) {
  return request({
    url: `/workflow/form-rule-type/${id}`,
    method: 'put',
    data
  })
}

export function deleteWorkflowFormRuleType(ids: number[]) {
  return request({
    url: '/workflow/form-rule-type',
    method: 'delete',
    data: { ids }
  })
} 