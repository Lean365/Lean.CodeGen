import request from '@/utils/request'
import type { LeanDataSource, LeanDataSourceQueryDto, LeanDataSourceCreateDto, LeanDataSourceUpdateDto, LeanDataSourceDto, LeanDataSourceTestDto } from '@/types/generator/dataSource'

export function getDataSourceList(params: LeanDataSourceQueryDto) {
  return request<LeanDataSourceDto[]>({
    url: '/generator/dataSource/list',
    method: 'get',
    params
  })
}

export function getDataSource(id: number) {
  return request<LeanDataSourceDto>({
    url: `/generator/dataSource/${id}`,
    method: 'get'
  })
}

export function createDataSource(data: LeanDataSourceCreateDto) {
  return request({
    url: '/generator/dataSource',
    method: 'post',
    data
  })
}

export function updateDataSource(id: number, data: LeanDataSourceUpdateDto) {
  return request({
    url: `/generator/dataSource/${id}`,
    method: 'put',
    data
  })
}

export function deleteDataSource(ids: number[]) {
  return request({
    url: '/generator/dataSource',
    method: 'delete',
    data: { ids }
  })
}

export function testDataSourceConnection(data: LeanDataSourceTestDto) {
  return request({
    url: '/generator/dataSource/test',
    method: 'post',
    data
  })
}