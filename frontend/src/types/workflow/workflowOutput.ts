export interface LeanWorkflowOutput {
  id: number
  activityId: number
  name: string
  type: string
  value: string
  createTime: string
  updateTime?: string
}

export interface LeanWorkflowOutputQueryDto {
  pageSize?: number
  pageIndex?: number
  activityId?: number
  name?: string
  type?: string
  startTime?: string
  endTime?: string
}

export interface LeanWorkflowOutputCreateDto {
  activityId: number
  name: string
  type: string
  value: string
}

export interface LeanWorkflowOutputUpdateDto extends LeanWorkflowOutputCreateDto {
  id: number
}

export interface LeanWorkflowOutputDto extends LeanWorkflowOutput {
  // 扩展字段
  activityName: string
} 