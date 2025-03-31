// 翻译基础接口
export interface LeanTranslation {
  id: number
  langId: number
  transKey: string
  transValue: string
  moduleName?: string
  orderNum: number
  transStatus: number
  isBuiltin: number
  createTime: string
  updateTime?: string
}

// 翻译查询参数
export interface LeanTranslationQueryDto {
  pageIndex: number
  pageSize: number
  langId?: number
  transKey?: string
  moduleName?: string
  transStatus?: number
  startTime?: string
  endTime?: string
}

// 翻译创建参数
export interface LeanTranslationCreateDto {
  langId: number
  transKey: string
  transValue: string
  moduleName?: string
  orderNum: number
  transStatus: number
}

// 翻译更新参数
export interface LeanTranslationUpdateDto extends LeanTranslationCreateDto {
  id: number
}

// 翻译状态修改参数
export interface LeanTranslationChangeStatusDto {
  id: number
  transStatus: number
}

// 翻译导出参数
export interface LeanTranslationExportDto {
  langId?: number
  transKey?: string
  moduleName?: string
  transStatus?: number
  startTime?: string
  endTime?: string
}

// 翻译导入参数
export interface LeanTranslationImportDto {
  langId: number
  file: File
  updateSupport?: boolean
}

// 翻译详情响应
export interface LeanTranslationDto extends LeanTranslation {
  createBy?: string
  updateBy?: string
  langName?: string
  langCode?: string
}

// 语言包响应
export interface LeanLanguagePackDto {
  langCode: string
  translations: Record<string, string>
} 