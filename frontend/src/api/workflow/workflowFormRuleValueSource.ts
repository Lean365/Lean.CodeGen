import request from '@/utils/request'
import type { LeanWorkflowFormRuleValueSource, LeanWorkflowFormRuleValueSourceQueryDto, LeanWorkflowFormRuleValueSourceCreateDto, LeanWorkflowFormRuleValueSourceUpdateDto, LeanWorkflowFormRuleValueSourceDto } from '@/types/workflow/workflowFormRuleValueSource'

export function getWorkflowFormRuleValueSourceList(params: LeanWorkflowFormRuleValueSourceQueryDto) {
  return request<LeanWorkflowFormRuleValueSourceDto[]>({
    url: '/workflow/form-rule-value-source/list',
    method: 'get',
    params
  })
}

export function getWorkflowFormRuleValueSource(id: number) {
  return request<LeanWorkflowFormRuleValueSourceDto>({
    url: `/workflow/form-rule-value-source/${id}`,
    method: 'get'
  })
}

export function createWorkflowFormRuleValueSource(data: LeanWorkflowFormRuleValueSourceCreateDto) {
  return request({
    url: '/workflow/form-rule-value-source',
    method: 'post',
    data
  })
}

export function updateWorkflowFormRuleValueSource(id: number, data: LeanWorkflowFormRuleValueSourceUpdateDto) {
  return request({
    url: `/workflow/form-rule-value-source/${id}`,
    method: 'put',
    data
  })
}

export function deleteWorkflowFormRuleValueSource(ids: number[]) {
  return request({
    url: '/workflow/form-rule-value-source',
    method: 'delete',
    data: { ids }
  })
} 