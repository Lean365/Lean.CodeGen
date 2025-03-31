import request from '@/utils/request'
import type {
  LeanTranslationQueryDto,
  LeanTranslationDto,
  LeanTranslationCreateDto,
  LeanTranslationUpdateDto,
  LeanTranslationChangeStatusDto,
  LeanTranslationExportDto,
  LeanTranslationImportDto,
  LeanLanguagePackDto
} from '@/types/admin/translation'

// 获取翻译列表
export function getTranslationList(params: LeanTranslationQueryDto) {
  return request<LeanTranslationDto[]>({
    url: '/admin/translation/list',
    method: 'get',
    params
  })
}

// 获取翻译详情
export function getTranslation(id: number) {
  return request<LeanTranslationDto>({
    url: `/admin/translation/${id}`,
    method: 'get'
  })
}

// 创建翻译
export function createTranslation(data: LeanTranslationCreateDto) {
  return request({
    url: '/admin/translation',
    method: 'post',
    data
  })
}

// 更新翻译
export function updateTranslation(id: number, data: LeanTranslationUpdateDto) {
  return request({
    url: `/admin/translation/${id}`,
    method: 'put',
    data
  })
}

// 删除翻译
export function deleteTranslation(ids: number[]) {
  return request({
    url: '/admin/translation',
    method: 'delete',
    data: { ids }
  })
}

// 修改翻译状态
export function changeTranslationStatus(data: LeanTranslationChangeStatusDto) {
  return request({
    url: '/admin/translation/status',
    method: 'put',
    data
  })
}

// 导出翻译
export function exportTranslation(params: LeanTranslationExportDto) {
  return request({
    url: '/admin/translation/export',
    method: 'get',
    params,
    responseType: 'blob'
  })
}

// 导入翻译
export function importTranslation(data: LeanTranslationImportDto) {
  const formData = new FormData()
  formData.append('langId', data.langId.toString())
  formData.append('file', data.file)
  if (data.updateSupport !== undefined) {
    formData.append('updateSupport', data.updateSupport.toString())
  }

  return request({
    url: '/admin/translation/import',
    method: 'post',
    data: formData,
    headers: {
      'Content-Type': 'multipart/form-data'
    }
  })
}

// 获取语言包
export function getLanguagePack(langCode: string) {
  return request<LeanLanguagePackDto>({
    url: `/admin/translation/pack/${langCode}`,
    method: 'get'
  })
}

// 刷新翻译缓存
export function refreshTranslationCache() {
  return request({
    url: '/admin/translation/refresh',
    method: 'post'
  })
} 