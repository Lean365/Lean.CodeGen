import request from '@/utils/request'
import type { LeanTableConfig, LeanTableConfigQueryDto, LeanTableConfigCreateDto, LeanTableConfigUpdateDto, LeanTableConfigDto } from '@/types/generator/tableConfig'

export function getTableConfigList(params: LeanTableConfigQueryDto) {
  return request<LeanTableConfigDto[]>({
    url: '/generator/tableConfig/list',
    method: 'get',
    params
  })
}

export function getTableConfig(id: number) {
  return request<LeanTableConfigDto>({
    url: `/generator/tableConfig/${id}`,
    method: 'get'
  })
}

export function createTableConfig(data: LeanTableConfigCreateDto) {
  return request({
    url: '/generator/tableConfig',
    method: 'post',
    data
  })
}

export function updateTableConfig(id: number, data: LeanTableConfigUpdateDto) {
  return request({
    url: `/generator/tableConfig/${id}`,
    method: 'put',
    data
  })
}

export function deleteTableConfig(ids: number[]) {
  return request({
    url: '/generator/tableConfig',
    method: 'delete',
    data: { ids }
  })
} 