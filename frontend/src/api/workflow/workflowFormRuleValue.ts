import request from '@/utils/request'
import type { LeanWorkflowFormRuleValue, LeanWorkflowFormRuleValueQueryDto, LeanWorkflowFormRuleValueCreateDto, LeanWorkflowFormRuleValueUpdateDto, LeanWorkflowFormRuleValueDto } from '@/types/workflow/workflowFormRuleValue'

export function getWorkflowFormRuleValueList(params: LeanWorkflowFormRuleValueQueryDto) {
  return request<LeanWorkflowFormRuleValueDto[]>({
    url: '/workflow/form-rule-value/list',
    method: 'get',
    params
  })
}

export function getWorkflowFormRuleValue(id: number) {
  return request<LeanWorkflowFormRuleValueDto>({
    url: `/workflow/form-rule-value/${id}`,
    method: 'get'
  })
}

export function createWorkflowFormRuleValue(data: LeanWorkflowFormRuleValueCreateDto) {
  return request({
    url: '/workflow/form-rule-value',
    method: 'post',
    data
  })
}

export function updateWorkflowFormRuleValue(id: number, data: LeanWorkflowFormRuleValueUpdateDto) {
  return request({
    url: `/workflow/form-rule-value/${id}`,
    method: 'put',
    data
  })
}

export function deleteWorkflowFormRuleValue(ids: number[]) {
  return request({
    url: '/workflow/form-rule-value',
    method: 'delete',
    data: { ids }
  })
} 