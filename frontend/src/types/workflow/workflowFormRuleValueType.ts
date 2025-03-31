export interface LeanWorkflowFormRuleValueType {
  id: number
  name: string
  code: string
  description?: string
  createTime: string
  updateTime?: string
}

export interface LeanWorkflowFormRuleValueTypeQueryDto {
  pageSize?: number
  pageIndex?: number
  name?: string
  code?: string
  startTime?: string
  endTime?: string
}

export interface LeanWorkflowFormRuleValueTypeCreateDto {
  name: string
  code: string
  description?: string
}

export interface LeanWorkflowFormRuleValueTypeUpdateDto extends LeanWorkflowFormRuleValueTypeCreateDto {
  id: number
}

export interface LeanWorkflowFormRuleValueTypeDto extends LeanWorkflowFormRuleValueType {
  // 扩展字段
} 