import request from '@/utils/request'
import type {
  LeanLanguageQueryDto,
  LeanLanguageDto,
  LeanLanguageCreateDto,
  LeanLanguageUpdateDto,
  LeanLanguageChangeStatusDto,
  LeanLanguageExportDto
} from '@/types/admin/language'

// 获取语言列表
export function getLanguageList(params: LeanLanguageQueryDto) {
  return request<LeanLanguageDto[]>({
    url: '/admin/language/list',
    method: 'get',
    params
  })
}

// 获取语言详情
export function getLanguage(id: number) {
  return request<LeanLanguageDto>({
    url: `/admin/language/${id}`,
    method: 'get'
  })
}

// 创建语言
export function createLanguage(data: LeanLanguageCreateDto) {
  return request({
    url: '/admin/language',
    method: 'post',
    data
  })
}

// 更新语言
export function updateLanguage(id: number, data: LeanLanguageUpdateDto) {
  return request({
    url: `/admin/language/${id}`,
    method: 'put',
    data
  })
}

// 删除语言
export function deleteLanguage(ids: number[]) {
  return request({
    url: '/admin/language',
    method: 'delete',
    data: { ids }
  })
}

// 修改语言状态
export function changeLanguageStatus(data: LeanLanguageChangeStatusDto) {
  return request({
    url: '/admin/language/status',
    method: 'put',
    data
  })
}

// 导出语言
export function exportLanguage(params: LeanLanguageExportDto) {
  return request({
    url: '/admin/language/export',
    method: 'get',
    params,
    responseType: 'blob'
  })
}

// 设置默认语言
export function setDefaultLanguage(id: number) {
  return request({
    url: `/admin/language/default/${id}`,
    method: 'put'
  })
}

// 获取当前语言
export function getCurrentLanguage() {
  return request<LeanLanguageDto>({
    url: '/admin/language/current',
    method: 'get'
  })
}

// 切换语言
export function switchLanguage(langCode: string) {
  return request({
    url: `/admin/language/switch/${langCode}`,
    method: 'put'
  })
} 