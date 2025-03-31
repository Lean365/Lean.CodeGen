export interface LeanGenHistory {
  id: number
  taskId: number
  tableName: string
  templateName: string
  fileName: string
  content: string
  createTime: string
  updateTime?: string
}

export interface LeanGenHistoryQueryDto {
  pageSize?: number
  pageIndex?: number
  taskId?: number
  tableName?: string
  templateName?: string
  startTime?: string
  endTime?: string
}

export interface LeanGenHistoryDto extends LeanGenHistory {
  // 扩展字段
  taskConfigName: string
} 