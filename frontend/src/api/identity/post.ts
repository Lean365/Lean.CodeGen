import request from '@/utils/request'
import type {
  LeanPostCreateDto,
  LeanPostUpdateDto,
  LeanPostQueryDto,
  LeanPostChangeStatusDto,
  LeanPostDto,
  LeanPostOptionDto
} from '@/types/identity/post'

// 获取岗位列表
export function getPostList(params: LeanPostQueryDto) {
  return request<LeanPostDto[]>({
    url: '/identity/post/list',
    method: 'get',
    params
  })
}

// 获取岗位详情
export function getPost(id: number) {
  return request<LeanPostDto>({
    url: `/identity/post/${id}`,
    method: 'get'
  })
}

// 创建岗位
export function createPost(data: LeanPostCreateDto) {
  return request({
    url: '/identity/post',
    method: 'post',
    data
  })
}

// 更新岗位
export function updatePost(id: number, data: LeanPostUpdateDto) {
  return request({
    url: `/identity/post/${id}`,
    method: 'put',
    data
  })
}

// 删除岗位
export function deletePost(ids: number[]) {
  return request({
    url: '/identity/post',
    method: 'delete',
    data: { ids }
  })
}

// 修改岗位状态
export function changePostStatus(data: LeanPostChangeStatusDto) {
  return request({
    url: '/identity/post/status',
    method: 'put',
    data
  })
}

// 获取岗位下拉选项
export function getPostOptions() {
  return request<LeanPostOptionDto[]>({
    url: '/identity/post/options',
    method: 'get'
  })
} 