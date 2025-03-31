import request from '@/utils/request'
import type { LeanExceptionLogQueryDto, LeanExceptionLogHandleDto } from '@/types/audit/exceptionLog'

// 获取异常日志列表
export function getExceptionLogList(params: LeanExceptionLogQueryDto) {
  return request({
    url: '/audit/exception',
    method: 'get',
    params
  })
}

// 获取异常日志详情
export function getExceptionLogDetail(id: number) {
  return request({
    url: `/audit/exception/${id}`,
    method: 'get'
  })
}

// 处理异常日志
export function handleExceptionLog(id: number, data: LeanExceptionLogHandleDto) {
  return request({
    url: `/audit/exception/${id}/handle`,
    method: 'post',
    data
  })
}

// 清空异常日志
export function clearExceptionLog() {
  return request({
    url: '/audit/exception/clear',
    method: 'delete'
  })
}

// 导出异常日志
export function exportExceptionLog(params: LeanExceptionLogQueryDto) {
  return request({
    url: '/audit/exception/export',
    method: 'get',
    params,
    responseType: 'blob'
  })
} 