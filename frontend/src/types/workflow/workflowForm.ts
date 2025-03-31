import type { LeanWorkflowFormFieldDto } from './workflowFormField'

export interface LeanWorkflowForm {
  id: number
  flowId: number
  name: string
  code: string
  description?: string
  status: number
  createTime: string
  updateTime?: string
}

export interface LeanWorkflowFormQueryDto {
  pageSize?: number
  pageIndex?: number
  flowId?: number
  name?: string
  code?: string
  status?: number
  startTime?: string
  endTime?: string
}

export interface LeanWorkflowFormCreateDto {
  flowId: number
  name: string
  code: string
  description?: string
  status: number
}

export interface LeanWorkflowFormUpdateDto extends LeanWorkflowFormCreateDto {
  id: number
}

export interface LeanWorkflowFormChangeStatusDto {
  id: number
  status: number
}

export interface LeanWorkflowFormDto extends LeanWorkflowForm {
  // 扩展字段
  flowName: string
  fields: LeanWorkflowFormFieldDto[]
} 