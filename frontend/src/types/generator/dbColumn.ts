export interface LeanDbColumn {
  id: number
  tableId: number
  columnName: string
  columnComment: string
  columnType: string
  length: number
  precision: number
  scale: number
  nullable: boolean
  defaultValue?: string
  isPrimaryKey: boolean
  isAutoIncrement: boolean
  ordinalPosition: number
  createTime: string
  updateTime?: string
} 