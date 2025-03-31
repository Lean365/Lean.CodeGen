import request from '@/utils/request'
import type { LeanGenHistory, LeanGenHistoryQueryDto, LeanGenHistoryDto } from '@/types/generator/genHistory'

export function getGenHistoryList(params: LeanGenHistoryQueryDto) {
  return request<LeanGenHistoryDto[]>({
    url: '/generator/genHistory/list',
    method: 'get',
    params
  })
}

export function getGenHistory(id: number) {
  return request<LeanGenHistoryDto>({
    url: `/generator/genHistory/${id}`,
    method: 'get'
  })
}

export function deleteGenHistory(ids: number[]) {
  return request({
    url: '/generator/genHistory',
    method: 'delete',
    data: { ids }
  })
}

export function previewGenHistory(id: number) {
  return request<string>({
    url: `/generator/genHistory/preview/${id}`,
    method: 'get'
  })
} 