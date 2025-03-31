// 配置基础接口
export interface LeanConfig {
  id: number
  configName: string
  configKey: string
  configValue: string
  configType: number
  isBuiltin: number
  configStatus: number
  configGroup?: string
  orderNum: number
  createTime: string
  updateTime?: string
}

// 配置查询参数
export interface LeanConfigQueryDto {
  pageIndex: number
  pageSize: number
  configName?: string
  configKey?: string
  configType?: number
  configStatus?: number
  configGroup?: string
  startTime?: string
  endTime?: string
}

// 配置创建参数
export interface LeanConfigCreateDto {
  configName: string
  configKey: string
  configValue: string
  configType: number
  configStatus: number
  configGroup?: string
  orderNum: number
}

// 配置更新参数
export interface LeanConfigUpdateDto extends LeanConfigCreateDto {
  id: number
}

// 配置状态修改参数
export interface LeanConfigChangeStatusDto {
  id: number
  configStatus: number
}

// 配置导出参数
export interface LeanConfigExportDto {
  configName?: string
  configKey?: string
  configType?: number
  configStatus?: number
  configGroup?: string
  startTime?: string
  endTime?: string
}

// 配置详情响应
export interface LeanConfigDto extends LeanConfig {
  createBy?: string
  updateBy?: string
} 
export interface LeanConfigDto extends LeanConfig {} 