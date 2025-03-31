import { defineStore } from 'pinia'
import { ref } from 'vue'
import type { DictData } from '@/types/admin/dictData'
import type { DictData } from '@/types/admin/dictType'

export const useDictStore = defineStore('dict', () => {
  const dictData = ref<Record<string, DictData[]>>({})

  // 获取字典数据
  const getDictData = (dictType: string) => {
    return dictData.value[dictType] || []
  }

  // 设置字典数据
  const setDictData = (dictType: string, data: DictData[]) => {
    dictData.value[dictType] = data
  }

  // 加载字典数据
  const loadDictData = async (dictType: string) => {
    try {
      // TODO: 调用后端API获取字典数据
      const response = await fetch(`/api/system/dict/data/type/${dictType}`)
      const data = await response.json()
      setDictData(dictType, data)
      return data
    } catch (error) {
      console.error('加载字典数据失败:', error)
      return []
    }
  }

  // 获取字典标签
  const getDictLabel = (dictType: string, value: number | string) => {
    const data = getDictData(dictType)
    const item = data.find(item => item.value === value)
    return item ? item.label : value
  }

  // 获取字典样式类
  const getDictClass = (dictType: string, value: number | string) => {
    const data = getDictData(dictType)
    const item = data.find(item => item.value === value)
    return item ? item.listClass : ''
  }

  // 获取字典国际化key
  const getDictTransKey = (dictType: string, value: number | string) => {
    const data = getDictData(dictType)
    const item = data.find(item => item.value === value)
    return item ? item.transKey : ''
  }

  // 加载字典
  const loadDict = async (dictType: string) => {
    if (!dictData.value[dictType]) {
      await loadDictData(dictType)
    }
  }

  return {
    dictData,
    getDictData,
    setDictData,
    loadDictData,
    getDictLabel,
    getDictClass,
    getDictTransKey,
    loadDict
  }
}) 