export interface LeanGenTask {
  id: number
  configId: number
  tableNames: string[]
  status: number
  error?: string
  startTime?: string
  endTime?: string
  createTime: string
  updateTime?: string
}

export interface LeanGenTaskQueryDto {
  pageSize?: number
  pageIndex?: number
  configId?: number
  status?: number
  startTime?: string
  endTime?: string
}

export interface LeanGenTaskCreateDto {
  configId: number
  tableNames: string[]
}

export interface LeanGenTaskDto extends LeanGenTask {
  // 扩展字段
  configName: string
} 