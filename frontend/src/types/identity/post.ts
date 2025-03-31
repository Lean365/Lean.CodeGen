// 岗位基础接口
export interface LeanPost {
  id: number
  postName: string
  postCode: string
  postDescription?: string
  orderNum: number
  postStatus: number
  isBuiltin: number
  createTime: string
  updateTime?: string
}

// 岗位创建参数
export interface LeanPostCreateDto {
  postName: string
  postCode: string
  postDescription?: string
  orderNum: number
}

// 岗位更新参数
export interface LeanPostUpdateDto extends Omit<LeanPostCreateDto, 'postCode'> {
  id: number
}

// 岗位查询参数
export interface LeanPostQueryDto {
  pageIndex: number
  pageSize: number
  keyword?: string
  status?: number
  startTime?: string
  endTime?: string
}

// 岗位状态修改参数
export interface LeanPostChangeStatusDto {
  id: number
  status: number
}

// 岗位详情响应
export interface LeanPostDto extends LeanPost {}

// 岗位下拉选项
export interface LeanPostOptionDto {
  id: number
  postName: string
  postCode: string
} 