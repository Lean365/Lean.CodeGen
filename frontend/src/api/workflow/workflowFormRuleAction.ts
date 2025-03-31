import request from '@/utils/request'
import type { LeanWorkflowFormRuleAction, LeanWorkflowFormRuleActionQueryDto, LeanWorkflowFormRuleActionCreateDto, LeanWorkflowFormRuleActionUpdateDto, LeanWorkflowFormRuleActionDto } from '@/types/workflow/workflowFormRuleAction'

export function getWorkflowFormRuleActionList(params: LeanWorkflowFormRuleActionQueryDto) {
  return request<LeanWorkflowFormRuleActionDto[]>({
    url: '/workflow/form-rule-action/list',
    method: 'get',
    params
  })
}

export function getWorkflowFormRuleAction(id: number) {
  return request<LeanWorkflowFormRuleActionDto>({
    url: `/workflow/form-rule-action/${id}`,
    method: 'get'
  })
}

export function createWorkflowFormRuleAction(data: LeanWorkflowFormRuleActionCreateDto) {
  return request({
    url: '/workflow/form-rule-action',
    method: 'post',
    data
  })
}

export function updateWorkflowFormRuleAction(id: number, data: LeanWorkflowFormRuleActionUpdateDto) {
  return request({
    url: `/workflow/form-rule-action/${id}`,
    method: 'put',
    data
  })
}

export function deleteWorkflowFormRuleAction(ids: number[]) {
  return request({
    url: '/workflow/form-rule-action',
    method: 'delete',
    data: { ids }
  })
} 