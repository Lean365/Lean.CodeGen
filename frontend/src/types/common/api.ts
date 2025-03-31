import { LeanBusinessType } from './businessType'
import { LeanErrorCode } from './errorCode'

// 实体基类
export interface LeanBaseEntity {
  /** 主键 */
  id: number

  /** 租户ID，用于多租户隔离 */
  tenantId?: number

  /** 创建者 */
  createBy?: string

  /** 创建时间 */
  createTime: Date

  /** 更新者 */
  updateBy?: string

  /** 更新时间 */
  updateTime?: Date

  /** 审核状态：0-无需审核，1-待审核，2-已审核，3-已驳回 */
  auditStatus: number

  /** 审核人员 */
  auditBy?: string

  /** 审核时间 */
  auditTime?: Date

  /** 审核意见，最大长度2000 */
  auditOpinion?: string

  /** 撤销人员 */
  revokeBy?: string

  /** 撤销时间 */
  revokeTime?: Date

  /** 撤销意见，最大长度2000 */
  revokeOpinion?: string

  /** 是否删除：0-未删除，1-已删除 */
  isDeleted: number

  /** 删除者 */
  deleteBy?: string

  /** 删除时间 */
  deleteTime?: Date

  /** 备注 */
  remark?: string
}

// API 统一返回结果
export interface LeanApiResult<T = any> {
  /** 是否成功 */
  success: boolean

  /** 错误代码 */
  code: LeanErrorCode

  /** 错误消息 */
  message: string | null

  /** 业务类型 */
  businessType: LeanBusinessType

  /** 跟踪ID */
  traceId?: string

  /** 时间戳（毫秒） */
  timestamp: number

  /** 返回数据 */
  data: T | null
}

// 分页请求参数
export interface LeanPage {
  /** 页码（从1开始） */
  pageIndex: number
  
  /** 每页大小（默认20条） */
  pageSize: number
  
  /** 排序字段 */
  orderBy?: string
  
  /** 是否升序（默认true） */
  isAsc?: boolean
}

// 分页响应数据
export interface LeanPageResult<T> {
  /** 总记录数 */
  total: number
  
  /** 当前页数据 */
  items: T[]
  
  /** 是否有上一页 */
  hasPrevious: boolean
  
  /** 是否有下一页 */
  hasNext: boolean
  
  /** 当前页码 */
  pageIndex: number
  
  /** 每页大小 */
  pageSize: number
  
  /** 总页数 */
  totalPages: number
}

// 导出查询参数
export interface LeanExportQuery extends LeanPage {
  /** 导出字段列表 */
  exportFields: string[]
  
  /** 文件格式 */
  fileFormat: string
  
  /** 是否导出全部 */
  isExportAll: boolean
} 