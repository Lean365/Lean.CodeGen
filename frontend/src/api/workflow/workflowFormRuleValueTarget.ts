import request from '@/utils/request'
import type { LeanWorkflowFormRuleValueTarget, LeanWorkflowFormRuleValueTargetQueryDto, LeanWorkflowFormRuleValueTargetCreateDto, LeanWorkflowFormRuleValueTargetUpdateDto, LeanWorkflowFormRuleValueTargetDto } from '@/types/workflow/workflowFormRuleValueTarget'

export function getWorkflowFormRuleValueTargetList(params: LeanWorkflowFormRuleValueTargetQueryDto) {
  return request<LeanWorkflowFormRuleValueTargetDto[]>({
    url: '/workflow/form-rule-value-target/list',
    method: 'get',
    params
  })
}

export function getWorkflowFormRuleValueTarget(id: number) {
  return request<LeanWorkflowFormRuleValueTargetDto>({
    url: `/workflow/form-rule-value-target/${id}`,
    method: 'get'
  })
}

export function createWorkflowFormRuleValueTarget(data: LeanWorkflowFormRuleValueTargetCreateDto) {
  return request({
    url: '/workflow/form-rule-value-target',
    method: 'post',
    data
  })
}

export function updateWorkflowFormRuleValueTarget(id: number, data: LeanWorkflowFormRuleValueTargetUpdateDto) {
  return request({
    url: `/workflow/form-rule-value-target/${id}`,
    method: 'put',
    data
  })
}

export function deleteWorkflowFormRuleValueTarget(ids: number[]) {
  return request({
    url: '/workflow/form-rule-value-target',
    method: 'delete',
    data: { ids }
  })
} 