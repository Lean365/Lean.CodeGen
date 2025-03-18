import type { ApiResponse } from '@/types/common/api'
import request from '@/utils/request'

/**
 * 上传单个文件
 * @param file 文件对象
 * @returns 文件访问URL
 */
export function uploadFileAsync(file: File): Promise<ApiResponse<string>> {
  const formData = new FormData()
  formData.append('file', file)
  return request({
    url: 'api/File/upload',
    method: 'post',
    data: formData,
    headers: {
      'Content-Type': 'multipart/form-data'
    }
  })
}

/**
 * 批量上传文件
 * @param files 文件列表
 * @returns 文件访问URL列表
 */
export function uploadFilesAsync(files: File[]): Promise<ApiResponse<string[]>> {
  const formData = new FormData()
  files.forEach(file => {
    formData.append('files', file)
  })
  return request({
    url: 'api/File/upload/batch',
    method: 'post',
    data: formData,
    headers: {
      'Content-Type': 'multipart/form-data'
    }
  })
} 