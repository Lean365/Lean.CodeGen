export interface LeanGenConfig {
  id: number
  name: string
  templateGroup: string
  packageName: string
  moduleName: string
  businessName: string
  functionName: string
  functionAuthor: string
  options: Record<string, any>
  createTime: string
  updateTime?: string
}

export interface LeanGenConfigQueryDto {
  pageSize?: number
  pageIndex?: number
  keyword?: string
  templateGroup?: string
  startTime?: string
  endTime?: string
}

export interface LeanGenConfigCreateDto {
  name: string
  templateGroup: string
  packageName: string
  moduleName: string
  businessName: string
  functionName: string
  functionAuthor: string
  options?: Record<string, any>
}

export interface LeanGenConfigUpdateDto extends LeanGenConfigCreateDto {
  id: number
}

export interface LeanGenConfigDto extends LeanGenConfig {
  // 扩展字段
} 