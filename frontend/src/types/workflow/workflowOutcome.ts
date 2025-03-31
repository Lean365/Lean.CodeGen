export interface LeanWorkflowOutcome {
  id: number
  activityId: number
  name: string
  condition: string
  nextActivityId?: number
  createTime: string
  updateTime?: string
}

export interface LeanWorkflowOutcomeQueryDto {
  pageSize?: number
  pageIndex?: number
  activityId?: number
  name?: string
  startTime?: string
  endTime?: string
}

export interface LeanWorkflowOutcomeCreateDto {
  activityId: number
  name: string
  condition: string
  nextActivityId?: number
}

export interface LeanWorkflowOutcomeUpdateDto extends LeanWorkflowOutcomeCreateDto {
  id: number
}

export interface LeanWorkflowOutcomeDto extends LeanWorkflowOutcome {
  // 扩展字段
  activityName: string
  nextActivityName?: string
} 