import request from '@/utils/request'
import type { LeanDbTable, LeanDbTableQueryDto, LeanDbTableImportDto, LeanDbTableDto, LeanDbTableSyncDto } from '@/types/generator/dbTable'

export function getDbTableList(params: LeanDbTableQueryDto) {
  return request<LeanDbTableDto[]>({
    url: '/generator/dbTable/list',
    method: 'get',
    params
  })
}

export function getDbTable(id: number) {
  return request<LeanDbTableDto>({
    url: `/generator/dbTable/${id}`,
    method: 'get'
  })
}

export function importDbTable(data: LeanDbTableImportDto) {
  return request({
    url: '/generator/dbTable/import',
    method: 'post',
    data
  })
}

export function syncDbTable(data: LeanDbTableSyncDto) {
  return request({
    url: '/generator/dbTable/sync',
    method: 'put',
    data
  })
}

export function deleteDbTable(ids: number[]) {
  return request({
    url: '/generator/dbTable',
    method: 'delete',
    data: { ids }
  })
} 