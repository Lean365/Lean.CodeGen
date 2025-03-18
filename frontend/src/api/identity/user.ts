import type { ApiResponse } from '@/types/common/api'
import type { 
  LeanUserDto, 
  LeanUserInfoDto, 
  LeanUserCreateDto, 
  LeanUserUpdateDto, 
  LeanUserDeleteDto, 
  LeanUserQueryDto, 
  LeanUserChangeStatusDto, 
  LeanUserResetPasswordDto, 
  LeanUserChangePasswordDto, 
  LeanUserExportDto 
} from '@/types/identity/user'
import type { UserInfo } from '@/types/identity/auth'
import request from '@/utils/request'

// 获取当前用户信息
export function getCurrentUserAsync(): Promise<ApiResponse<LeanUserInfoDto>> {
  return request({
    url: 'api/LeanUser/current',
    method: 'get'
  })
}

// 获取用户列表
export function getUsersAsync(params: LeanUserQueryDto): Promise<ApiResponse<LeanUserDto[]>> {
  return request({
    url: 'api/LeanUser',
    method: 'get',
    params
  })
}

// 获取用户详情
export function getUserAsync(id: number): Promise<ApiResponse<LeanUserDto>> {
  return request({
    url: `api/LeanUser/${id}`,
    method: 'get'
  })
}

// 创建用户
export function createUserAsync(data: LeanUserCreateDto): Promise<ApiResponse<void>> {
  return request({
    url: 'api/LeanUser',
    method: 'post',
    data
  })
}

// 更新用户
export function updateUserAsync(data: LeanUserUpdateDto): Promise<ApiResponse<void>> {
  return request({
    url: 'api/LeanUser',
    method: 'put',
    data
  })
}

// 删除用户
export function deleteUserAsync(data: LeanUserDeleteDto): Promise<ApiResponse<void>> {
  return request({
    url: 'api/LeanUser',
    method: 'delete',
    data
  })
}

// 修改用户状态
export function changeUserStatusAsync(data: LeanUserChangeStatusDto): Promise<ApiResponse<void>> {
  return request({
    url: 'api/LeanUser/status',
    method: 'put',
    data
  })
}

// 重置用户密码
export function resetUserPasswordAsync(data: LeanUserResetPasswordDto): Promise<ApiResponse<void>> {
  return request({
    url: 'api/LeanUser/reset-password',
    method: 'put',
    data
  })
}

// 修改用户密码
export function changeUserPasswordAsync(data: LeanUserChangePasswordDto): Promise<ApiResponse<void>> {
  return request({
    url: 'api/LeanUser/change-password',
    method: 'put',
    data
  })
}

// 导出用户
export function exportUserAsync(params: LeanUserExportDto): Promise<ApiResponse<Blob>> {
  return request({
    url: 'api/LeanUser/export',
    method: 'get',
    params,
    responseType: 'blob'
  })
}

// 导入用户
export function importUserAsync(file: FormData): Promise<ApiResponse<void>> {
  return request({
    url: 'api/LeanUser/import',
    method: 'post',
    headers: {
      'Content-Type': 'multipart/form-data'
    },
    data: file
  })
}

// 获取导入模板
export function getImportTemplateAsync(): Promise<ApiResponse<Blob>> {
  return request({
    url: 'api/LeanUser/import-template',
    method: 'get',
    responseType: 'blob'
  })
}

// 获取用户信息
export function getUserInfoAsync(): Promise<ApiResponse<UserInfo>> {
  return request({
    url: 'api/LeanAuth/user/info',
    method: 'get'
  })
}

// 更新用户信息
export function updateUserInfoAsync(data: Partial<UserInfo>): Promise<ApiResponse<void>> {
  return request({
    url: 'api/LeanAuth/user/info',
    method: 'put',
    data
  })
}

// 修改密码
export interface ChangePasswordRequest {
  oldPassword: string
  newPassword: string
  confirmPassword: string
}

export function changePasswordAsync(data: ChangePasswordRequest): Promise<ApiResponse<void>> {
  return request({
    url: 'api/LeanAuth/user/password',
    method: 'put',
    data
  })
} 