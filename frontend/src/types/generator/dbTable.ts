import type { LeanDbColumn } from './dbColumn'

export interface LeanDbTable {
  id: number
  dataSourceId: number
  tableName: string
  tableComment: string
  engine: string
  charset: string
  collation: string
  createTime: string
  updateTime?: string
}

export interface LeanDbTableQueryDto {
  pageSize?: number
  pageIndex?: number
  dataSourceId?: number
  tableName?: string
  tableComment?: string
  startTime?: string
  endTime?: string
}

export interface LeanDbTableImportDto {
  dataSourceId: number
  tableNames: string[]
}

export interface LeanDbTableDto extends LeanDbTable {
  columns: LeanDbColumn[]
}

export interface LeanDbTableSyncDto {
  dataSourceId: number
  tableName: string
} 