export interface LeanDictDataDto {
  id: number
  typeId: number
  dictDataLabel: string
  dictDataValue: string
  dictDataStatus: number
  cssClass?: string
  listClass?: string
  transKey?: string
  orderNum: number
  isBuiltin: number
  remark?: string
  createTime: string
  updateTime?: string
}

export interface LeanDictDataQueryDto {
  typeId?: number
  dictDataLabel?: string
  dictDataValue?: string
  dictDataStatus?: number
  startTime?: string
  endTime?: string
}

export interface LeanDictDataCreateDto {
  typeId: number
  dictDataLabel: string
  dictDataValue: string
  transKey?: string
  cssClass?: string
  listClass?: string
  orderNum: number
  remark?: string
}

export interface LeanDictDataUpdateDto extends LeanDictDataCreateDto {
  id: number
}

export interface LeanDictDataChangeStatusDto {
  id: number
  dictDataStatus: number
} 