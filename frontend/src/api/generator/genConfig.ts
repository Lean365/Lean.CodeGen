import request from '@/utils/request'
import type { LeanGenConfig, LeanGenConfigQueryDto, LeanGenConfigCreateDto, LeanGenConfigUpdateDto, LeanGenConfigDto } from '@/types/generator/genConfig'

export function getGenConfigList(params: LeanGenConfigQueryDto) {
  return request<LeanGenConfigDto[]>({
    url: '/generator/genConfig/list',
    method: 'get',
    params
  })
}

export function getGenConfig(id: number) {
  return request<LeanGenConfigDto>({
    url: `/generator/genConfig/${id}`,
    method: 'get'
  })
}

export function createGenConfig(data: LeanGenConfigCreateDto) {
  return request({
    url: '/generator/genConfig',
    method: 'post',
    data
  })
}

export function updateGenConfig(id: number, data: LeanGenConfigUpdateDto) {
  return request({
    url: `/generator/genConfig/${id}`,
    method: 'put',
    data
  })
}

export function deleteGenConfig(ids: number[]) {
  return request({
    url: '/generator/genConfig',
    method: 'delete',
    data: { ids }
  })
} 