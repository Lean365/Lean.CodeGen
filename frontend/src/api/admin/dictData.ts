import request from '@/utils/request'
import type { 
  LeanDictData, 
  LeanDictDataQueryDto, 
  LeanDictDataCreateDto, 
  LeanDictDataUpdateDto, 
  LeanDictDataChangeStatusDto, 
  LeanDictDataDto } from '@/types/admin/dictData'

export function getDictDataList(params: LeanDictDataQueryDto) {
  return request<LeanDictDataDto[]>({
    url: '/admin/dictData/list',
    method: 'get',
    params
  })
}

export function getDictData(id: number) {
  return request<LeanDictDataDto>({
    url: `/admin/dictData/${id}`,
    method: 'get'
  })
}

export function createDictData(data: LeanDictDataCreateDto) {
  return request({
    url: '/admin/dictData',
    method: 'post',
    data
  })
}

export function updateDictData(id: number, data: LeanDictDataUpdateDto) {
  return request({
    url: `/admin/dictData/${id}`,
    method: 'put',
    data
  })
}

export function deleteDictData(ids: number[]) {
  return request({
    url: '/admin/dictData',
    method: 'delete',
    data: { ids }
  })
}

export function changeDictDataStatus(data: LeanDictDataChangeStatusDto) {
  return request({
    url: '/admin/dictData/status',
    method: 'put',
    data
  })
}

export function getDictDataOptions(dictType: string) {
  return request<LeanDictDataDto[]>({
    url: `/admin/dictData/options/${dictType}`,
    method: 'get'
  })
} 