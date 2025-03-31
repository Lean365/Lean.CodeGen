export interface LeanWorkflowFormField {
  id: number
  formId: number
  name: string
  code: string
  type: string
  label: string
  placeholder?: string
  required: boolean
  defaultValue?: string
  validation?: string
  orderNum: number
  createTime: string
  updateTime?: string
}

export interface LeanWorkflowFormFieldQueryDto {
  pageSize?: number
  pageIndex?: number
  formId?: number
  name?: string
  code?: string
  type?: string
  startTime?: string
  endTime?: string
}

export interface LeanWorkflowFormFieldCreateDto {
  formId: number
  name: string
  code: string
  type: string
  label: string
  placeholder?: string
  required: boolean
  defaultValue?: string
  validation?: string
  orderNum: number
}

export interface LeanWorkflowFormFieldUpdateDto extends LeanWorkflowFormFieldCreateDto {
  id: number
}

export interface LeanWorkflowFormFieldDto extends LeanWorkflowFormField {
  // 扩展字段
  formName: string
} 