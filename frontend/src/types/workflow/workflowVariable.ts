export interface LeanWorkflowVariable {
  id: number
  flowId: number
  name: string
  code: string
  type: string
  description?: string
  defaultValue?: string
  createTime: string
  updateTime?: string
}

export interface LeanWorkflowVariableQueryDto {
  pageSize?: number
  pageIndex?: number
  flowId?: number
  name?: string
  code?: string
  type?: string
  startTime?: string
  endTime?: string
}

export interface LeanWorkflowVariableCreateDto {
  flowId: number
  name: string
  code: string
  type: string
  description?: string
  defaultValue?: string
}

export interface LeanWorkflowVariableUpdateDto extends LeanWorkflowVariableCreateDto {
  id: number
}

export interface LeanWorkflowVariableDto extends LeanWorkflowVariable {
  // 扩展字段
  flowName: string
} 