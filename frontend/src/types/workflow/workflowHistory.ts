export interface LeanWorkflowHistory {
  id: number
  instanceId: number
  activityId: number
  status: number
  input?: Record<string, any>
  output?: Record<string, any>
  error?: string
  startTime: string
  endTime?: string
  createTime: string
  updateTime?: string
}

export interface LeanWorkflowHistoryQueryDto {
  pageSize?: number
  pageIndex?: number
  instanceId?: number
  activityId?: number
  status?: number
  startTime?: string
  endTime?: string
}

export interface LeanWorkflowHistoryDto extends LeanWorkflowHistory {
  // 扩展字段
  instanceName: string
  activityName: string
  flowName: string
} 