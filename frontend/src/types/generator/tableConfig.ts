export interface LeanTableConfig {
  id: number
  configId: number
  tableName: string
  tableComment: string
  packageName: string
  moduleName: string
  businessName: string
  functionName: string
  functionAuthor: string
  options: Record<string, any>
  createTime: string
  updateTime?: string
}

export interface LeanTableConfigQueryDto {
  pageSize?: number
  pageIndex?: number
  configId?: number
  tableName?: string
  startTime?: string
  endTime?: string
}

export interface LeanTableConfigCreateDto {
  configId: number
  tableName: string
  tableComment: string
  packageName: string
  moduleName: string
  businessName: string
  functionName: string
  functionAuthor: string
  options?: Record<string, any>
}

export interface LeanTableConfigUpdateDto extends LeanTableConfigCreateDto {
  id: number
}

export interface LeanTableConfigDto extends LeanTableConfig {
  // 扩展字段
} 