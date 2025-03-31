export interface LeanWorkflowFormRuleValueMapping {
  id: number
  sourceId: number
  targetId: number
  sourceValue: string
  targetValue: string
  createTime: string
  updateTime?: string
}

export interface LeanWorkflowFormRuleValueMappingQueryDto {
  pageSize?: number
  pageIndex?: number
  sourceId?: number
  targetId?: number
  sourceValue?: string
  targetValue?: string
  startTime?: string
  endTime?: string
}

export interface LeanWorkflowFormRuleValueMappingCreateDto {
  sourceId: number
  targetId: number
  sourceValue: string
  targetValue: string
}

export interface LeanWorkflowFormRuleValueMappingUpdateDto extends LeanWorkflowFormRuleValueMappingCreateDto {
  id: number
}

export interface LeanWorkflowFormRuleValueMappingDto extends LeanWorkflowFormRuleValueMapping {
  // 扩展字段
  sourceName: string
  sourceCode: string
  targetName: string
  targetCode: string
}
