export interface LeanWorkflowFormRuleValueSource {
  id: number
  name: string
  code: string
  description?: string
  createTime: string
  updateTime?: string
}

export interface LeanWorkflowFormRuleValueSourceQueryDto {
  pageSize?: number
  pageIndex?: number
  name?: string
  code?: string
  startTime?: string
  endTime?: string
}

export interface LeanWorkflowFormRuleValueSourceCreateDto {
  name: string
  code: string
  description?: string
}

export interface LeanWorkflowFormRuleValueSourceUpdateDto extends LeanWorkflowFormRuleValueSourceCreateDto {
  id: number
}

export interface LeanWorkflowFormRuleValueSourceDto extends LeanWorkflowFormRuleValueSource {
  // 扩展字段
} 