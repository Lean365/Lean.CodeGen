import request from '@/utils/request'
import type { LeanWorkflowFormRuleValueType, LeanWorkflowFormRuleValueTypeQueryDto, LeanWorkflowFormRuleValueTypeCreateDto, LeanWorkflowFormRuleValueTypeUpdateDto, LeanWorkflowFormRuleValueTypeDto } from '@/types/workflow/workflowFormRuleValueType'

export function getWorkflowFormRuleValueTypeList(params: LeanWorkflowFormRuleValueTypeQueryDto) {
  return request<LeanWorkflowFormRuleValueTypeDto[]>({
    url: '/workflow/form-rule-value-type/list',
    method: 'get',
    params
  })
}

export function getWorkflowFormRuleValueType(id: number) {
  return request<LeanWorkflowFormRuleValueTypeDto>({
    url: `/workflow/form-rule-value-type/${id}`,
    method: 'get'
  })
}

export function createWorkflowFormRuleValueType(data: LeanWorkflowFormRuleValueTypeCreateDto) {
  return request({
    url: '/workflow/form-rule-value-type',
    method: 'post',
    data
  })
}

export function updateWorkflowFormRuleValueType(id: number, data: LeanWorkflowFormRuleValueTypeUpdateDto) {
  return request({
    url: `/workflow/form-rule-value-type/${id}`,
    method: 'put',
    data
  })
}

export function deleteWorkflowFormRuleValueType(ids: number[]) {
  return request({
    url: '/workflow/form-rule-value-type',
    method: 'delete',
    data: { ids }
  })
} 