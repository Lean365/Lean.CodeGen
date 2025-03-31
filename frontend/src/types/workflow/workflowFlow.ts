import type { LeanWorkflowActivityDto } from './workflowActivity'

export interface LeanWorkflowFlow {
  id: number
  name: string
  code: string
  description?: string
  version: number
  status: number
  startActivityId?: number
  createTime: string
  updateTime?: string
}

export interface LeanWorkflowFlowQueryDto {
  pageSize?: number
  pageIndex?: number
  name?: string
  code?: string
  status?: number
  startTime?: string
  endTime?: string
}

export interface LeanWorkflowFlowCreateDto {
  name: string
  code: string
  description?: string
  version: number
  status: number
  startActivityId?: number
}

export interface LeanWorkflowFlowUpdateDto extends LeanWorkflowFlowCreateDto {
  id: number
}

export interface LeanWorkflowFlowChangeStatusDto {
  id: number
  status: number
}

export interface LeanWorkflowFlowDto extends LeanWorkflowFlow {
  // 扩展字段
  startActivityName?: string
  activities: LeanWorkflowActivityDto[]
} 