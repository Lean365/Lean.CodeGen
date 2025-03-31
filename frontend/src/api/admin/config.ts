import request from '@/utils/request'
import type {
  LeanConfigQueryDto,
  LeanConfigDto,
  LeanConfigCreateDto,
  LeanConfigUpdateDto,
  LeanConfigChangeStatusDto,
  LeanConfigExportDto
} from '@/types/admin/config'

// 获取配置列表
export function getConfigList(params: LeanConfigQueryDto) {
  return request<LeanConfigDto[]>({
    url: '/admin/config/list',
    method: 'get',
    params
  })
}

// 获取配置详情
export function getConfig(id: number) {
  return request<LeanConfigDto>({
    url: `/admin/config/${id}`,
    method: 'get'
  })
}

// 创建配置
export function createConfig(data: LeanConfigCreateDto) {
  return request({
    url: '/admin/config',
    method: 'post',
    data
  })
}

// 更新配置
export function updateConfig(id: number, data: LeanConfigUpdateDto) {
  return request({
    url: `/admin/config/${id}`,
    method: 'put',
    data
  })
}

// 删除配置
export function deleteConfig(ids: number[]) {
  return request({
    url: '/admin/config',
    method: 'delete',
    data: { ids }
  })
}

// 修改配置状态
export function changeConfigStatus(data: LeanConfigChangeStatusDto) {
  return request({
    url: '/admin/config/status',
    method: 'put',
    data
  })
}

// 导出配置
export function exportConfig(params: LeanConfigExportDto) {
  return request({
    url: '/admin/config/export',
    method: 'get',
    params,
    responseType: 'blob'
  })
}

// 刷新配置缓存
export function refreshConfigCache() {
  return request({
    url: '/admin/config/refresh',
    method: 'post'
  })
}

// 获取配置键值
export function getConfigValue(key: string) {
  return request<string>({
    url: `/admin/config/value/${key}`,
    method: 'get'
  })
} 