import request from '@/utils/request'
import type { 
  LeanDictType, 
  LeanDictTypeQueryDto, 
  LeanDictTypeCreateDto, 
  LeanDictTypeUpdateDto, 
  LeanDictTypeChangeStatusDto, 
  LeanDictTypeDto } from '@/types/admin/dictType'

export function getDictTypeList(params: LeanDictTypeQueryDto) {
  return request<LeanDictTypeDto[]>({
    url: '/admin/dictType/list',
    method: 'get',
    params
  })
}

export function getDictType(id: number) {
  return request<LeanDictTypeDto>({
    url: `/admin/dictType/${id}`,
    method: 'get'
  })
}

export function createDictType(data: LeanDictTypeCreateDto) {
  return request({
    url: '/admin/dictType',
    method: 'post',
    data
  })
}

export function updateDictType(id: number, data: LeanDictTypeUpdateDto) {
  return request({
    url: `/admin/dictType/${id}`,
    method: 'put',
    data
  })
}

export function deleteDictType(ids: number[]) {
  return request({
    url: '/admin/dictType',
    method: 'delete',
    data: { ids }
  })
}

export function changeDictTypeStatus(data: LeanDictTypeChangeStatusDto) {
  return request({
    url: '/admin/dictType/status',
    method: 'put',
    data
  })
} 