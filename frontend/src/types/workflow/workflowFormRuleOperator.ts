export interface LeanWorkflowFormRuleOperator {
  id: number
  name: string
  code: string
  description?: string
  type: number
  createTime: string
  updateTime?: string
}

export interface LeanWorkflowFormRuleOperatorQueryDto {
  pageSize?: number
  pageIndex?: number
  name?: string
  code?: string
  type?: number
  startTime?: string
  endTime?: string
}

export interface LeanWorkflowFormRuleOperatorCreateDto {
  name: string
  code: string
  description?: string
  type: number
}

export interface LeanWorkflowFormRuleOperatorUpdateDto extends LeanWorkflowFormRuleOperatorCreateDto {
  id: number
}

export interface LeanWorkflowFormRuleOperatorDto extends LeanWorkflowFormRuleOperator {
  // 扩展字段
  typeName: string
}
