// 用户创建参数
export interface LeanUserCreateDto {
  userName: string
  password: string
  nickName: string
  email?: string
  phoneNumber?: string
  gender: number
  avatar?: string
  deptIds: number[]
  postIds: number[]
  roleIds: number[]
}

// 用户更新参数
export interface LeanUserUpdateDto {
  id: number
  nickName: string
  email?: string
  phoneNumber?: string
  gender: number
  avatar?: string
  deptIds: number[]
  postIds: number[]
  roleIds: number[]
}

// 用户删除参数
export interface LeanUserDeleteDto {
  ids: number[]
}

// 用户查询参数
export interface LeanUserQueryDto {
  pageIndex: number
  pageSize: number
  keyword?: string
  deptId?: number
  status?: number
  startTime?: string
  endTime?: string
}

// 用户状态修改参数
export interface LeanUserChangeStatusDto {
  id: number
  status: number
}

// 重置密码参数
export interface LeanUserResetPasswordDto {
  id: number
  password: string
}

// 修改密码参数
export interface LeanUserChangePasswordDto {
  oldPassword: string
  newPassword: string
}

// 用户导出参数
export interface LeanUserExportDto {
  keyword?: string
  deptId?: number
  status?: number
  startTime?: string
  endTime?: string
}

// 用户基本信息
export interface LeanUserInfoDto {
  userId: number
  userName: string
  nickName: string
  avatar?: string
  roleNames: string[]
  permissions: string[]
}

// 用户详细信息
export interface LeanUserDto {
  userId: number
  userName: string
  nickName: string
  avatar?: string
  email: string
  phoneNumber: string
  gender: number
  status: number
  createTime: string
  lastLoginTime?: string
  depts: LeanUserDeptDto[]
  posts: LeanUserPostDto[]
  roles: LeanUserRoleDto[]
}

// 用户部门关系
export interface LeanUserDeptDto {
  userId: number
  deptId: number
  isPrimary: boolean
}

// 用户岗位关系
export interface LeanUserPostDto {
  userId: number
  postId: number
  isPrimary: boolean
}

// 用户角色关系
export interface LeanUserRoleDto {
  userId: number
  roleId: number
} 