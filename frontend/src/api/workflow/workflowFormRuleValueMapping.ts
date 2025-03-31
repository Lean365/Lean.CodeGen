import request from '@/utils/request'
import type { LeanWorkflowFormRuleValueMapping, LeanWorkflowFormRuleValueMappingQueryDto, LeanWorkflowFormRuleValueMappingCreateDto, LeanWorkflowFormRuleValueMappingUpdateDto, LeanWorkflowFormRuleValueMappingDto } from '@/types/workflow/workflowFormRuleValueMapping'

export function getWorkflowFormRuleValueMappingList(params: LeanWorkflowFormRuleValueMappingQueryDto) {
  return request<LeanWorkflowFormRuleValueMappingDto[]>({
    url: '/workflow/form-rule-value-mapping/list',
    method: 'get',
    params
  })
}

export function getWorkflowFormRuleValueMapping(id: number) {
  return request<LeanWorkflowFormRuleValueMappingDto>({
    url: `/workflow/form-rule-value-mapping/${id}`,
    method: 'get'
  })
}

export function createWorkflowFormRuleValueMapping(data: LeanWorkflowFormRuleValueMappingCreateDto) {
  return request({
    url: '/workflow/form-rule-value-mapping',
    method: 'post',
    data
  })
}

export function updateWorkflowFormRuleValueMapping(id: number, data: LeanWorkflowFormRuleValueMappingUpdateDto) {
  return request({
    url: `/workflow/form-rule-value-mapping/${id}`,
    method: 'put',
    data
  })
}

export function deleteWorkflowFormRuleValueMapping(ids: number[]) {
  return request({
    url: '/workflow/form-rule-value-mapping',
    method: 'delete',
    data: { ids }
  })
} 