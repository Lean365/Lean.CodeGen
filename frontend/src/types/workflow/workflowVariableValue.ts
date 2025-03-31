export interface LeanWorkflowVariableValue {
  id: number
  instanceId: number
  variableId: number
  value: string
  createTime: string
  updateTime?: string
}

export interface LeanWorkflowVariableValueQueryDto {
  pageSize?: number
  pageIndex?: number
  instanceId?: number
  variableId?: number
  startTime?: string
  endTime?: string
}

export interface LeanWorkflowVariableValueCreateDto {
  instanceId: number
  variableId: number
  value: string
}

export interface LeanWorkflowVariableValueUpdateDto extends LeanWorkflowVariableValueCreateDto {
  id: number
}

export interface LeanWorkflowVariableValueDto extends LeanWorkflowVariableValue {
  // 扩展字段
  instanceName: string
  variableName: string
  variableCode: string
  variableType: string
} 