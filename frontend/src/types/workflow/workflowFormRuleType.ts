export interface LeanWorkflowFormRuleType {
  id: number
  name: string
  code: string
  description?: string
  createTime: string
  updateTime?: string
}

export interface LeanWorkflowFormRuleTypeQueryDto {
  pageSize?: number
  pageIndex?: number
  name?: string
  code?: string
  startTime?: string
  endTime?: string
}

export interface LeanWorkflowFormRuleTypeCreateDto {
  name: string
  code: string
  description?: string
}

export interface LeanWorkflowFormRuleTypeUpdateDto extends LeanWorkflowFormRuleTypeCreateDto {
  id: number
}

export interface LeanWorkflowFormRuleTypeDto extends LeanWorkflowFormRuleType {
  // 扩展字段
}
