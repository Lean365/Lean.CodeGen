export interface LeanWorkflowFormRuleValue {
  id: number
  name: string
  code: string
  type: number
  value: string
  createTime: string
  updateTime?: string
}

export interface LeanWorkflowFormRuleValueQueryDto {
  pageSize?: number
  pageIndex?: number
  name?: string
  code?: string
  type?: number
  startTime?: string
  endTime?: string
}

export interface LeanWorkflowFormRuleValueCreateDto {
  name: string
  code: string
  type: number
  value: string
}

export interface LeanWorkflowFormRuleValueUpdateDto extends LeanWorkflowFormRuleValueCreateDto {
  id: number
}

export interface LeanWorkflowFormRuleValueDto extends LeanWorkflowFormRuleValue {
  // 扩展字段
  typeName: string
} 