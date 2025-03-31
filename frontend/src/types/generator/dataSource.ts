export interface LeanDataSource {
  id: number
  name: string
  dbType: string
  host: string
  port: number
  database: string
  username: string
  password: string
  createTime: string
  updateTime?: string
}

export interface LeanDataSourceQueryDto {
  pageSize?: number
  pageIndex?: number
  keyword?: string
  dbType?: string
  startTime?: string
  endTime?: string
}

export interface LeanDataSourceCreateDto {
  name: string
  dbType: string
  host: string
  port: number
  database: string
  username: string
  password: string
}

export interface LeanDataSourceUpdateDto extends LeanDataSourceCreateDto {
  id: number
}

export interface LeanDataSourceDto extends LeanDataSource {
  // 扩展字段
}

export interface LeanDataSourceTestDto {
  dbType: string
  host: string
  port: number
  database: string
  username: string
  password: string
} 