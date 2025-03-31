import request from '@/utils/request'
import type { LeanWorkflowFormRuleOperator, LeanWorkflowFormRuleOperatorQueryDto, LeanWorkflowFormRuleOperatorCreateDto, LeanWorkflowFormRuleOperatorUpdateDto, LeanWorkflowFormRuleOperatorDto } from '@/types/workflow/workflowFormRuleOperator'

export function getWorkflowFormRuleOperatorList(params: LeanWorkflowFormRuleOperatorQueryDto) {
  return request<LeanWorkflowFormRuleOperatorDto[]>({
    url: '/workflow/form-rule-operator/list',
    method: 'get',
    params
  })
}

export function getWorkflowFormRuleOperator(id: number) {
  return request<LeanWorkflowFormRuleOperatorDto>({
    url: `/workflow/form-rule-operator/${id}`,
    method: 'get'
  })
}

export function createWorkflowFormRuleOperator(data: LeanWorkflowFormRuleOperatorCreateDto) {
  return request({
    url: '/workflow/form-rule-operator',
    method: 'post',
    data
  })
}

export function updateWorkflowFormRuleOperator(id: number, data: LeanWorkflowFormRuleOperatorUpdateDto) {
  return request({
    url: `/workflow/form-rule-operator/${id}`,
    method: 'put',
    data
  })
}

export function deleteWorkflowFormRuleOperator(ids: number[]) {
  return request({
    url: '/workflow/form-rule-operator',
    method: 'delete',
    data: { ids }
  })
} 