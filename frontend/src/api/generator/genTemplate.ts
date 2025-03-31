import request from '@/utils/request'
import type { LeanGenTemplate, LeanGenTemplateQueryDto, LeanGenTemplateCreateDto, LeanGenTemplateUpdateDto, LeanGenTemplateDto } from '@/types/generator/genTemplate'

export function getGenTemplateList(params: LeanGenTemplateQueryDto) {
  return request<LeanGenTemplateDto[]>({
    url: '/generator/genTemplate/list',
    method: 'get',
    params
  })
}

export function getGenTemplate(id: number) {
  return request<LeanGenTemplateDto>({
    url: `/generator/genTemplate/${id}`,
    method: 'get'
  })
}

export function createGenTemplate(data: LeanGenTemplateCreateDto) {
  return request({
    url: '/generator/genTemplate',
    method: 'post',
    data
  })
}

export function updateGenTemplate(id: number, data: LeanGenTemplateUpdateDto) {
  return request({
    url: `/generator/genTemplate/${id}`,
    method: 'put',
    data
  })
}

export function deleteGenTemplate(ids: number[]) {
  return request({
    url: '/generator/genTemplate',
    method: 'delete',
    data: { ids }
  })
}

export function getDefaultTemplates() {
  return request<LeanGenTemplateDto[]>({
    url: '/generator/genTemplate/defaults',
    method: 'get'
  })
} 