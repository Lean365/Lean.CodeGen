export interface LeanMenu {
  id: number
  parentId: number
  name: string
  path: string
  component: string
  icon: string
  orderNum: number
  isVisible: boolean
  isCache: boolean
  menuType: string
  perms: string
  createTime: string
  updateTime: string
}

export interface LeanMenuQueryDto {
  pageSize?: number
  pageIndex?: number
  name?: string
  path?: string
  menuType?: string
  startTime?: string
  endTime?: string
}

export interface LeanMenuCreateDto {
  parentId: number
  name: string
  path: string
  component: string
  icon: string
  orderNum: number
  isVisible: boolean
  isCache: boolean
  menuType: string
  perms: string
}

export interface LeanMenuUpdateDto extends LeanMenuCreateDto {
  id: number
}

export interface LeanMenuDto extends LeanMenu {
  children?: LeanMenuDto[]
} 