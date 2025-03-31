import type { LeanWorkflowHistoryDto } from './workflowHistory'

export interface LeanWorkflowInstance {
  id: number
  flowId: number
  name: string
  status: number
  currentActivityId?: number
  input?: Record<string, any>
  output?: Record<string, any>
  error?: string
  startTime: string
  endTime?: string
  createTime: string
  updateTime?: string
}

export interface LeanWorkflowInstanceQueryDto {
  pageSize?: number
  pageIndex?: number
  flowId?: number
  name?: string
  status?: number
  startTime?: string
  endTime?: string
}

export interface LeanWorkflowInstanceCreateDto {
  flowId: number
  name: string
  input?: Record<string, any>
}

export interface LeanWorkflowInstanceDto extends LeanWorkflowInstance {
  // 扩展字段
  flowName: string
  currentActivityName?: string
  histories: LeanWorkflowHistoryDto[]
}