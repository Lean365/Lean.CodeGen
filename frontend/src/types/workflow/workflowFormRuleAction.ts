export interface LeanWorkflowFormRuleAction {
  id: number
  ruleId: number
  fieldId: number
  type: number
  value: string
  orderNum: number
  createTime: string
  updateTime?: string
}

export interface LeanWorkflowFormRuleActionQueryDto {
  pageSize?: number
  pageIndex?: number
  ruleId?: number
  fieldId?: number
  type?: number
  startTime?: string
  endTime?: string
}

export interface LeanWorkflowFormRuleActionCreateDto {
  ruleId: number
  fieldId: number
  type: number
  value: string
  orderNum: number
}

export interface LeanWorkflowFormRuleActionUpdateDto extends LeanWorkflowFormRuleActionCreateDto {
  id: number
}

export interface LeanWorkflowFormRuleActionDto extends LeanWorkflowFormRuleAction {
  // 扩展字段
  ruleName: string
  fieldName: string
  fieldCode: string
  fieldType: string
  fieldLabel: string
  typeName: string
} 