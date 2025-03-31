export interface LeanGenTemplate {
  id: number
  groupName: string
  name: string
  content: string
  fileName: string
  filePath: string
  language: string
  framework: string
  isDefault: boolean
  createTime: string
  updateTime?: string
}

export interface LeanGenTemplateQueryDto {
  pageSize?: number
  pageIndex?: number
  groupName?: string
  name?: string
  language?: string
  framework?: string
  startTime?: string
  endTime?: string
}

export interface LeanGenTemplateCreateDto {
  groupName: string
  name: string
  content: string
  fileName: string
  filePath: string
  language: string
  framework: string
  isDefault: boolean
}

export interface LeanGenTemplateUpdateDto extends LeanGenTemplateCreateDto {
  id: number
}

export interface LeanGenTemplateDto extends LeanGenTemplate {
  // 扩展字段
} 