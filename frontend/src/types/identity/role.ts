// 角色基础接口
export interface LeanRole {
  id: number
  parentId?: number
  roleName: string
  roleCode: string
  roleDescription?: string
  orderNum: number
  roleStatus: number
  dataScope: number
  isBuiltin: number
  createTime: string
  updateTime?: string
}

// 角色创建参数
export interface LeanRoleCreateDto {
  parentId?: number
  roleName: string
  roleCode: string
  roleDescription?: string
  orderNum: number
  dataScope: number
  menuIds: number[]
  deptIds?: number[]
}

// 角色更新参数
export interface LeanRoleUpdateDto extends Omit<LeanRoleCreateDto, 'roleCode'> {
  id: number
}

// 角色查询参数
export interface LeanRoleQueryDto {
  pageIndex: number
  pageSize: number
  keyword?: string
  status?: number
  startTime?: string
  endTime?: string
}

// 角色状态修改参数
export interface LeanRoleChangeStatusDto {
  id: number
  status: number
}

// 角色详情响应
export interface LeanRoleDto extends LeanRole {
  menuIds: number[]
  deptIds: number[]
}

// 角色下拉选项
export interface LeanRoleOptionDto {
  id: number
  roleName: string
  roleCode: string
} 