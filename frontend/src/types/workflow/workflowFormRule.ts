import type { LeanWorkflowFormRuleConditionDto } from './workflowFormRuleCondition'
import type { LeanWorkflowFormRuleActionDto } from './workflowFormRuleAction'

export interface LeanWorkflowFormRule {
  id: number
  formId: number
  name: string
  code: string
  description?: string
  type: number
  status: number
  createTime: string
  updateTime?: string
}

export interface LeanWorkflowFormRuleQueryDto {
  pageSize?: number
  pageIndex?: number
  formId?: number
  name?: string
  code?: string
  type?: number
  status?: number
  startTime?: string
  endTime?: string
}

export interface LeanWorkflowFormRuleCreateDto {
  formId: number
  name: string
  code: string
  description?: string
  type: number
  status: number
}

export interface LeanWorkflowFormRuleUpdateDto extends LeanWorkflowFormRuleCreateDto {
  id: number
}

export interface LeanWorkflowFormRuleChangeStatusDto {
  id: number
  status: number
}

export interface LeanWorkflowFormRuleDto extends LeanWorkflowFormRule {
  // 扩展字段
  formName: string
  typeName: string
  conditions: LeanWorkflowFormRuleConditionDto[]
  actions: LeanWorkflowFormRuleActionDto[]
} 