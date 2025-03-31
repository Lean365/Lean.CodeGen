import type { LeanWorkflowFormRuleOperatorDto } from './workflowFormRuleOperator'
import type { LeanWorkflowFormRuleValueDto } from './workflowFormRuleValue'

export interface LeanWorkflowFormRuleCondition {
  id: number
  ruleId: number
  fieldId: number
  operatorId: number
  valueId: number
  orderNum: number
  createTime: string
  updateTime?: string
}

export interface LeanWorkflowFormRuleConditionQueryDto {
  pageSize?: number
  pageIndex?: number
  ruleId?: number
  fieldId?: number
  operatorId?: number
  valueId?: number
  startTime?: string
  endTime?: string
}

export interface LeanWorkflowFormRuleConditionCreateDto {
  ruleId: number
  fieldId: number
  operatorId: number
  valueId: number
  orderNum: number
}

export interface LeanWorkflowFormRuleConditionUpdateDto extends LeanWorkflowFormRuleConditionCreateDto {
  id: number
}

export interface LeanWorkflowFormRuleConditionDto extends LeanWorkflowFormRuleCondition {
  // 扩展字段
  ruleName: string
  fieldName: string
  fieldCode: string
  fieldType: string
  fieldLabel: string
  operator: LeanWorkflowFormRuleOperatorDto
  value: LeanWorkflowFormRuleValueDto
} 