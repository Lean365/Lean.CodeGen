import request from '@/utils/request'
import type { LeanGenTask, LeanGenTaskQueryDto, LeanGenTaskCreateDto, LeanGenTaskDto } from '@/types/generator/genTask'

export function getGenTaskList(params: LeanGenTaskQueryDto) {
  return request<LeanGenTaskDto[]>({
    url: '/generator/genTask/list',
    method: 'get',
    params
  })
}

export function getGenTask(id: number) {
  return request<LeanGenTaskDto>({
    url: `/generator/genTask/${id}`,
    method: 'get'
  })
}

export function createGenTask(data: LeanGenTaskCreateDto) {
  return request({
    url: '/generator/genTask',
    method: 'post',
    data
  })
}

export function deleteGenTask(ids: number[]) {
  return request({
    url: '/generator/genTask',
    method: 'delete',
    data: { ids }
  })
}

export function downloadGenTask(id: number) {
  return request({
    url: `/generator/genTask/download/${id}`,
    method: 'get',
    responseType: 'blob'
  })
} 