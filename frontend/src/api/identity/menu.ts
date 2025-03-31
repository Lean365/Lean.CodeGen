import request from '@/utils/request'
import type {
  LeanMenuCreateDto,
  LeanMenuUpdateDto,
  LeanMenuQueryDto,
  LeanMenuChangeStatusDto,
  LeanMenuDto,
  LeanRouteMenu
} from '@/types/identity/menu'

// 1. 分页查询菜单
export function getMenuList(params: LeanMenuQueryDto) {
  return request<LeanMenuDto[]>({
    url: '/api/LeanMenu/list',
    method: 'get',
    params
  })
}

// 2. 获取菜单信息
export function getMenu(id: number) {
  return request<LeanMenuDto>({
    url: `/api/LeanMenu/${id}`,
    method: 'get'
  })
}

// 3. 创建菜单
export function createMenu(data: LeanMenuCreateDto) {
  return request({
    url: '/api/LeanMenu',
    method: 'post',
    data
  })
}

// 4. 更新菜单
export function updateMenu(data: LeanMenuUpdateDto) {
  return request({
    url: '/api/LeanMenu',
    method: 'put',
    data
  })
}

// 5. 排序菜单
export function sortMenu(data: any[]) {
  return request({
    url: '/api/LeanMenu/sort',
    method: 'put',
    data
  })
}

// 6. 删除菜单
export function deleteMenu(id: number) {
  return request({
    url: `/api/LeanMenu/${id}`,
    method: 'delete'
  })
}

// 7. 批量删除菜单
export function batchDeleteMenu(ids: number[]) {
  return request({
    url: '/api/LeanMenu/batch',
    method: 'delete',
    data: ids
  })
}

// 8. 导出菜单数据
export function exportMenu(params: LeanMenuQueryDto) {
  return request({
    url: '/api/LeanMenu/export',
    method: 'get',
    params,
    responseType: 'blob'
  })
}

// 9. 获取导入模板
export function getMenuTemplate() {
  return request({
    url: '/api/LeanMenu/template',
    method: 'get',
    responseType: 'blob'
  })
}

// 10. 导入菜单数据
export function importMenu(data: FormData) {
  return request({
    url: '/api/LeanMenu/import',
    method: 'post',
    data,
    headers: {
      'Content-Type': 'multipart/form-data'
    }
  })
}

// 11. 设置菜单状态
export function setMenuStatus(data: LeanMenuChangeStatusDto) {
  return request({
    url: '/api/LeanMenu/status',
    method: 'put',
    data
  })
}

// 12. 获取菜单树形结构
export function getMenuTree(params?: LeanMenuQueryDto) {
  return request<LeanMenuDto[]>({
    url: '/api/LeanMenu/tree',
    method: 'get',
    params
  })
}

// 13. 获取角色菜单树
export function getRoleMenuTree(roleId: number) {
  return request<LeanMenuDto[]>({
    url: `/api/LeanMenu/role/${roleId}/tree`,
    method: 'get'
  })
}

// 14. 获取当前登录用户的菜单树
export function getCurrentUserMenu() {
  console.log('开始获取用户菜单...')
  return request<LeanMenuDto[]>({
    url: '/api/LeanMenu/user/tree',
    method: 'get'
  })
}

// 15. 获取当前登录用户的权限清单
export function getCurrentUserPermissions() {
  console.log('开始获取用户权限清单...')
  return request<string[]>({
    url: '/api/LeanMenu/user/permissions',
    method: 'get'
  })
} 