// 部门基础接口
export interface LeanDept {
  id: number
  parentId?: number
  deptName: string
  deptCode: string
  deptDescription?: string
  leader?: string
  phone?: string
  email?: string
  orderNum: number
  deptStatus: number
  isBuiltin: number
  dataScope: number
  createTime: string
  updateTime?: string
}

// 部门创建参数
export interface LeanDeptCreateDto {
  parentId?: number
  deptName: string
  deptCode: string
  deptDescription?: string
  leader?: string
  phone?: string
  email?: string
  orderNum: number
  dataScope: number
}

// 部门更新参数
export interface LeanDeptUpdateDto extends Omit<LeanDeptCreateDto, 'deptCode'> {
  id: number
}

// 部门查询参数
export interface LeanDeptQueryDto {
  keyword?: string
  status?: number
}

// 部门状态修改参数
export interface LeanDeptChangeStatusDto {
  id: number
  status: number
}

// 部门详情响应
export interface LeanDeptDto extends LeanDept {
  children?: LeanDeptDto[]
  parentName?: string
}

// 部门下拉选项
export interface LeanDeptOptionDto {
  id: number
  deptName: string
  deptCode: string
} 