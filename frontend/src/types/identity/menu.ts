// 菜单基础接口
export interface LeanMenu {
  id: number
  parentId?: number
  menuName: string
  perms: string
  menuType: number
  orderNum: number
  icon?: string
  transKey?: string
  path?: string
  component?: string
  redirect?: string
  menuStatus: number
  visible: number
  isFrame: number
  isCached: number
  isBuiltin: number
  createTime: string
  updateTime?: string
}

// 菜单创建参数
export interface LeanMenuCreateDto {
  menuName: string
  parentId: number
  path: string
  component: string
  isFrame: number
  isCached: number
  visible: number
  menuStatus: number
  menuType: number
  icon: string
  transKey: string
  perms: string
  isBuiltin: number
  children?: LeanMenuCreateDto[]
}

// 菜单更新参数
export interface LeanMenuUpdateDto extends LeanMenuCreateDto {
  id: number
}

// 菜单查询参数
export interface LeanMenuQueryDto {
  keyword?: string
  status?: number
  menuType?: number
}

// 菜单状态修改参数
export interface LeanMenuChangeStatusDto {
  id: number
  status: number
}

// 菜单详情响应
export interface LeanMenuDto {
  id: number
  parentId: number
  menuName: string
  path: string
  component: string
  isFrame: number
  isCached: number
  visible: number
  menuStatus: number
  menuType: number
  icon: string
  transKey?: string
  perms: string
  isBuiltin: number
  children?: LeanMenuDto[]
}

// 路由菜单
export interface LeanRouteMenu {
  name: string
  path: string
  hidden?: boolean
  redirect?: string
  component?: string
  alwaysShow?: boolean
  meta: {
    title: string
    icon?: string
    noCache?: boolean
    link?: string
  }
  children?: LeanRouteMenu[]
} 