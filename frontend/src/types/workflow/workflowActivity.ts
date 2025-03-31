export interface LeanWorkflowActivity {
  id: number
  flowId: number
  name: string
  code: string
  type: number
  description?: string
  orderNum: number
  createTime: string
  updateTime?: string
}

export interface LeanWorkflowActivityQueryDto {
  pageSize?: number
  pageIndex?: number
  flowId?: number
  name?: string
  code?: string
  type?: number
  startTime?: string
  endTime?: string
}

export interface LeanWorkflowActivityCreateDto {
  flowId: number
  name: string
  code: string
  type: number
  description?: string
  orderNum: number
}

export interface LeanWorkflowActivityUpdateDto extends LeanWorkflowActivityCreateDto {
  id: number
}

export interface LeanWorkflowActivityDto extends LeanWorkflowActivity {
  // 扩展字段
  flowName: string
  typeName: string
} 