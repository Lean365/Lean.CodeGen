export interface LeanWorkflowFormValue {
  id: number
  instanceId: number
  formId: number
  fieldId: number
  value: string
  createTime: string
  updateTime?: string
}

export interface LeanWorkflowFormValueQueryDto {
  pageSize?: number
  pageIndex?: number
  instanceId?: number
  formId?: number
  fieldId?: number
  startTime?: string
  endTime?: string
}

export interface LeanWorkflowFormValueCreateDto {
  instanceId: number
  formId: number
  fieldId: number
  value: string
}

export interface LeanWorkflowFormValueUpdateDto extends LeanWorkflowFormValueCreateDto {
  id: number
}

export interface LeanWorkflowFormValueDto extends LeanWorkflowFormValue {
  // 扩展字段
  instanceName: string
  formName: string
  fieldName: string
  fieldCode: string
  fieldType: string
  fieldLabel: string
} 