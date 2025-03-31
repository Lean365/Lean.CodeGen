export interface LeanDictType {
  id: number
  dictName: string
  dictType: string
  status: number
  remark?: string
  createTime: string
  updateTime?: string
}

export interface LeanDictTypeQueryDto {
  pageSize?: number
  pageIndex?: number
  keyword?: string
  status?: number
  startTime?: string
  endTime?: string
}

export interface LeanDictTypeCreateDto {
  dictName: string
  dictType: string
  status: number
  remark?: string
}

export interface LeanDictTypeUpdateDto extends LeanDictTypeCreateDto {
  id: number
}

export interface LeanDictTypeChangeStatusDto {
  id: number
  status: number
}

export interface LeanDictTypeDto extends LeanDictType {
  // 扩展字段
} 