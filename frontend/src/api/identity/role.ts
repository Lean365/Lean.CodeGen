import request from '@/utils/request'
import type {
  LeanRoleCreateDto,
  LeanRoleUpdateDto,
  LeanRoleQueryDto,
  LeanRoleChangeStatusDto,
  LeanRoleDto,
  LeanRoleOptionDto
} from '@/types/identity/role'

export interface RoleData {
  id: string
  name: string
  code: string
}

// 获取角色列表
export const getRoleList = (params?: { page?: number; pageSize?: number }) => {
  return request.get<{ data: RoleData[]; total: number }>('/api/identity/roles', { params })
}

// 获取角色详情
export function getRole(id: number) {
  return request<LeanRoleDto>({
    url: `/identity/role/${id}`,
    method: 'get'
  })
}

// 创建角色
export function createRole(data: LeanRoleCreateDto) {
  return request({
    url: '/identity/role',
    method: 'post',
    data
  })
}

// 更新角色
export function updateRole(id: number, data: LeanRoleUpdateDto) {
  return request({
    url: `/identity/role/${id}`,
    method: 'put',
    data
  })
}

// 删除角色
export function deleteRole(ids: number[]) {
  return request({
    url: '/identity/role',
    method: 'delete',
    data: { ids }
  })
}

// 修改角色状态
export function changeRoleStatus(data: LeanRoleChangeStatusDto) {
  return request({
    url: '/identity/role/status',
    method: 'put',
    data
  })
}

// 获取角色下拉选项
export function getRoleOptions() {
  return request<LeanRoleOptionDto[]>({
    url: '/identity/role/options',
    method: 'get'
  })
} 