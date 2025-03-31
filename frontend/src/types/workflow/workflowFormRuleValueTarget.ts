export interface LeanWorkflowFormRuleValueTarget {
  id: number
  name: string
  code: string
  description?: string
  createTime: string
  updateTime?: string
}

export interface LeanWorkflowFormRuleValueTargetQueryDto {
  pageSize?: number
  pageIndex?: number
  name?: string
  code?: string
  startTime?: string
  endTime?: string
}

export interface LeanWorkflowFormRuleValueTargetCreateDto {
  name: string
  code: string
  description?: string
}

export interface LeanWorkflowFormRuleValueTargetUpdateDto extends LeanWorkflowFormRuleValueTargetCreateDto {
  id: number
}

export interface LeanWorkflowFormRuleValueTargetDto extends LeanWorkflowFormRuleValueTarget {
  // 扩展字段
} 