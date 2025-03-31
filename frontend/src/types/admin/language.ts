// 语言基础接口
export interface LeanLanguage {
  id: number
  langName: string
  langCode: string
  langIcon?: string
  isDefault: number
  orderNum: number
  langStatus: number
  isBuiltin: number
  createTime: string
  updateTime?: string
}

// 语言查询参数
export interface LeanLanguageQueryDto {
  pageIndex: number
  pageSize: number
  langName?: string
  langCode?: string
  langStatus?: number
  startTime?: string
  endTime?: string
}

// 语言创建参数
export interface LeanLanguageCreateDto {
  langName: string
  langCode: string
  langIcon?: string
  isDefault: number
  orderNum: number
  langStatus: number
}

// 语言更新参数
export interface LeanLanguageUpdateDto extends LeanLanguageCreateDto {
  id: number
}

// 语言状态修改参数
export interface LeanLanguageChangeStatusDto {
  id: number
  langStatus: number
}

// 语言导出参数
export interface LeanLanguageExportDto {
  langName?: string
  langCode?: string
  langStatus?: number
  startTime?: string
  endTime?: string
}

// 语言详情响应
export interface LeanLanguageDto extends LeanLanguage {
  createBy?: string
  updateBy?: string
  translationCount?: number
} 