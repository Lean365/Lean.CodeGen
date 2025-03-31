import request from '@/utils/request'
import type { LeanWorkflowActivity, LeanWorkflowActivityQueryDto, LeanWorkflowActivityCreateDto, LeanWorkflowActivityUpdateDto, LeanWorkflowActivityDto } from '@/types/workflow/workflowActivity'

export function getWorkflowActivityList(params: LeanWorkflowActivityQueryDto) {
  return request<LeanWorkflowActivityDto[]>({
    url: '/workflow/activity/list',
    method: 'get',
    params
  })
}

export function getWorkflowActivity(id: number) {
  return request<LeanWorkflowActivityDto>({
    url: `/workflow/activity/${id}`,
    method: 'get'
  })
}

export function createWorkflowActivity(data: LeanWorkflowActivityCreateDto) {
  return request({
    url: '/workflow/activity',
    method: 'post',
    data
  })
}

export function updateWorkflowActivity(id: number, data: LeanWorkflowActivityUpdateDto) {
  return request({
    url: `/workflow/activity/${id}`,
    method: 'put',
    data
  })
}

export function deleteWorkflowActivity(ids: number[]) {
  return request({
    url: '/workflow/activity',
    method: 'delete',
    data: { ids }
  })
}