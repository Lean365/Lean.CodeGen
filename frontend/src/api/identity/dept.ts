import request from '@/utils/request'
import type {
  LeanDeptCreateDto,
  LeanDeptUpdateDto,
  LeanDeptQueryDto,
  LeanDeptChangeStatusDto,
  LeanDeptDto,
  LeanDeptOptionDto
} from '@/types/identity/dept'

// 获取部门列表
export function getDeptList(params?: LeanDeptQueryDto) {
  return request<LeanDeptDto[]>({
    url: '/identity/dept/list',
    method: 'get',
    params
  })
}

// 获取部门详情
export function getDept(id: number) {
  return request<LeanDeptDto>({
    url: `/identity/dept/${id}`,
    method: 'get'
  })
}

// 创建部门
export function createDept(data: LeanDeptCreateDto) {
  return request({
    url: '/identity/dept',
    method: 'post',
    data
  })
}

// 更新部门
export function updateDept(id: number, data: LeanDeptUpdateDto) {
  return request({
    url: `/identity/dept/${id}`,
    method: 'put',
    data
  })
}

// 删除部门
export function deleteDept(ids: number[]) {
  return request({
    url: '/identity/dept',
    method: 'delete',
    data: { ids }
  })
}

// 修改部门状态
export function changeDeptStatus(data: LeanDeptChangeStatusDto) {
  return request({
    url: '/identity/dept/status',
    method: 'put',
    data
  })
}

// 获取部门下拉选项
export function getDeptOptions() {
  return request<LeanDeptOptionDto[]>({
    url: '/identity/dept/options',
    method: 'get'
  })
} 