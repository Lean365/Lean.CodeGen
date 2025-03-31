<template>
  <div class="business-select">
    <!-- Select 选择器 -->
    <a-select v-if="type === 'select'" v-model:value="selectedValue" :mode="mode" :placeholder="placeholder"
      :allow-clear="allowClear" :disabled="disabled" :loading="loading" :show-search="showSearch"
      :filter-option="filterOption" :max-tag-count="maxTagCount" :max-tag-placeholder="maxTagPlaceholder"
      :show-arrow="showArrow" :bordered="bordered" :size="size" :options="mergedOptions"
      :not-found-content="notFoundContent" :dropdown-match-select-width="dropdownMatchSelectWidth"
      :dropdown-style="dropdownStyle" :dropdown-class-name="dropdownClassName" :popup-container="popupContainer"
      :get-popup-container="getPopupContainer" :token-separators="tokenSeparators"
      :show-checked-strategy="showCheckedStrategy" :tag-render="tagRender" :virtual-scroll="virtualScroll"
      @change="handleChange" @deselect="handleDeselect" @select="handleSelect" @search="handleSearch"
      @dropdown-visible-change="handleDropdownVisibleChange" @blur="handleBlur" @focus="handleFocus" v-bind="$attrs">
      <template v-if="mode === 'tags'" #suffixIcon>
        <slot name="suffixIcon"></slot>
      </template>
      <template v-if="mode === 'multiple'" #maxTagPlaceholder="omittedValues">
        <slot name="maxTagPlaceholder" v-bind="omittedValues"></slot>
      </template>
      <template #notFoundContent>
        <slot name="notFoundContent"></slot>
      </template>
      <template #option="{ data }">
        <slot name="option" v-bind="data"></slot>
      </template>
    </a-select>

    <!-- Radio 单选框 -->
    <a-radio-group v-else v-model:value="selectedValue" :disabled="disabled" v-bind="$attrs" @change="handleChange">
      <template v-if="buttonStyle">
        <a-radio-button v-for="option in mergedOptions" :key="option.value" :value="option.value"
          :disabled="option.disabled">
          {{ option.label }}
        </a-radio-button>
      </template>
      <template v-else>
        <a-radio v-for="option in mergedOptions" :key="option.value" :value="option.value" :disabled="option.disabled">
          {{ option.label }}
        </a-radio>
      </template>
    </a-radio-group>
  </div>
</template>

<script lang="ts" setup>
import { ref, computed, watch } from 'vue'
import type { SelectProps, RadioChangeEvent } from 'ant-design-vue'
import type { CSSProperties } from 'vue'
import { getDictDataOptions } from '@/api/admin/dictData'
import type { LeanDictDataDto } from '@/types/admin/dictData'
import { useDebounceFn } from '@vueuse/core'

interface Props {
  // 组件类型：select-下拉选择器，radio-单选框
  type?: 'select' | 'radio'
  // 单选框按钮样式
  buttonStyle?: boolean
  // 选择器模式
  mode?: 'multiple' | 'tags'
  // 占位符
  placeholder?: string
  // 是否允许清除
  allowClear?: boolean
  // 是否禁用
  disabled?: boolean
  // 加载状态
  loading?: boolean
  // 是否显示搜索框
  showSearch?: boolean
  // 是否根据输入项进行筛选
  filterOption?: boolean | ((input: string, option: any) => boolean)
  // 最多显示多少个 tag
  maxTagCount?: number
  // 隐藏 tag 时显示的内容
  maxTagPlaceholder?: (omittedValues: any[]) => string
  // 是否显示下拉箭头
  showArrow?: boolean
  // 是否有边框
  bordered?: boolean
  // 选择器大小
  size?: 'large' | 'middle' | 'small'
  // 可选项数据源
  options?: any[]
  // 字典类型
  dictType?: string
  // 当下拉列表为空时显示的内容
  notFoundContent?: string
  // 下拉菜单和选择器同宽
  dropdownMatchSelectWidth?: boolean
  // 下拉菜单的 style 属性
  dropdownStyle?: CSSProperties
  // 下拉菜单的 className 属性
  dropdownClassName?: string
  // 设置弹窗的父容器
  popupContainer?: HTMLElement
  // 自定义下拉框渲染容器
  getPopupContainer?: (triggerNode: HTMLElement) => HTMLElement
  // 在 tags 和 multiple 模式下自动分词的分隔符
  tokenSeparators?: string[]
  // 多选时选中项的显示策略
  showCheckedStrategy?: 'SHOW_ALL' | 'SHOW_PARENT' | 'SHOW_CHILD'
  // 自定义 tag 渲染
  tagRender?: (props: any) => any
  // 是否开启虚拟滚动
  virtualScroll?: boolean
  // 初始值
  value?: any
  // 是否启用远程搜索
  remote?: boolean
  // 远程搜索防抖时间
  remoteDebounceTime?: number
  // 远程搜索方法
  remoteMethod?: (query: string) => Promise<any[]>
  // 是否显示全部选项（仅在radio模式下生效）
  showAll?: boolean
  // 全部选项的标签文本
  allLabel?: string
  // 全部选项的值
  allValue?: number
}

const props = withDefaults(defineProps<Props>(), {
  type: 'select',
  buttonStyle: false,
  mode: undefined,
  placeholder: '请选择',
  allowClear: true,
  disabled: false,
  loading: false,
  showSearch: false,
  filterOption: true,
  maxTagCount: undefined,
  maxTagPlaceholder: (omittedValues: any[]) => `+ ${omittedValues.length} ...`,
  showArrow: true,
  bordered: true,
  size: 'middle',
  options: () => [],
  dictType: '',
  notFoundContent: '无匹配结果',
  dropdownMatchSelectWidth: true,
  dropdownStyle: () => ({} as CSSProperties),
  dropdownClassName: '',
  tokenSeparators: () => [],
  showCheckedStrategy: 'SHOW_CHILD',
  virtualScroll: false,
  value: undefined,
  remote: false,
  remoteDebounceTime: 300,
  remoteMethod: undefined,
  showAll: true,
  allLabel: '全部',
  allValue: -1
})

// 选中值
const selectedValue = computed({
  get: () => {
    // 如果是多选模式，确保数组中的值都是数值类型
    if (Array.isArray(props.value)) {
      return props.value.map(v => Number(v))
    }
    // 单选模式，确保值是数值类型
    return props.value !== undefined ? Number(props.value) : undefined
  },
  set: (val) => {
    // 如果是多选模式，确保数组中的值都是数值类型
    if (Array.isArray(val)) {
      emit('update:value', val.map(v => Number(v)))
    } else {
      // 单选模式，确保值是数值类型
      emit('update:value', val !== undefined ? Number(val) : undefined)
    }
  }
})

// 组件事件
const emit = defineEmits<{
  (e: 'update:value', value: any): void
  (e: 'change', value: any, option: any): void
  (e: 'deselect', value: any, option: any): void
  (e: 'select', value: any, option: any): void
  (e: 'search', value: string): void
  (e: 'dropdownVisibleChange', open: boolean): void
  (e: 'blur', event: FocusEvent): void
  (e: 'focus', event: FocusEvent): void
}>()

// 选项数据
const dictOptions = ref<any[]>([])

// 加载状态
const loading = ref(false)

// 搜索关键词
const searchValue = ref('')

// 监听字典类型变化
watch(() => props.dictType, async (type: string | undefined) => {
  if (type) {
    loading.value = true
    try {
      const { data } = await getDictDataOptions(type)
      dictOptions.value = data.map((item: LeanDictDataDto) => ({
        value: Number(item.dictDataValue), // 确保value是数值类型
        label: item.dictDataLabel,
        disabled: item.dictDataStatus === 1
      }))
    } catch (error) {
      console.error('获取字典数据失败：', error)
    } finally {
      loading.value = false
    }
  }
}, { immediate: true })

// 最终的选项数据（合并props.options和字典数据）
const mergedOptions = computed(() => {
  let options = props.options.length > 0 ? props.options : dictOptions.value

  // 在radio模式下，如果需要显示全部选项，则添加到选项列表的最前面
  if (props.type === 'radio' && props.showAll) {
    options = [
      {
        value: props.allValue,
        label: props.allLabel,
        disabled: false
      },
      ...options
    ]
  }

  return options
})

// 处理值变化
const handleChange = (value: any, option?: any) => {
  if (props.type === 'radio') {
    // Radio模式下的值转换
    const radioValue = (value as RadioChangeEvent).target.value
    emit('change', Number(radioValue), option)
  } else {
    // Select模式下的值转换
    if (Array.isArray(value)) {
      // 多选模式
      emit('change', value.map(v => Number(v)), option)
    } else {
      // 单选模式
      emit('change', value !== undefined ? Number(value) : undefined, option)
    }
  }
}

// 处理取消选择
const handleDeselect = (value: any, option: any) => {
  emit('deselect', Number(value), option)
}

// 处理选择
const handleSelect = (value: any, option: any) => {
  emit('select', Number(value), option)
}

// 处理搜索
const handleSearch = async (value: string) => {
  searchValue.value = value
  emit('search', value)

  if (props.remote && props.remoteMethod) {
    loading.value = true
    try {
      const options = await props.remoteMethod(value)
      dictOptions.value = options.map((item: any) => ({
        value: Number(item.value), // 确保远程数据的value也是数值类型
        label: item.label,
        disabled: item.disabled
      }))
    } catch (error) {
      console.error('远程搜索失败：', error)
    } finally {
      loading.value = false
    }
  }
}

// 防抖处理
const debouncedSearch = useDebounceFn(handleSearch, props.remoteDebounceTime)

// 监听搜索值变化
watch(searchValue, (value) => {
  if (props.remote) {
    debouncedSearch(value)
  }
})

// 处理下拉框显示状态变化
const handleDropdownVisibleChange = (open: boolean) => {
  emit('dropdownVisibleChange', open)
}

// 处理失焦
const handleBlur = (event: FocusEvent) => {
  emit('blur', event)
}

// 处理聚焦
const handleFocus = (event: FocusEvent) => {
  emit('focus', event)
}

// 暴露方法给父组件
defineExpose({
  // 获取选中值
  getValue: () => selectedValue.value,
  // 设置选中值
  setValue: (value: any) => {
    selectedValue.value = value
  },
  // 清空选中值
  clear: () => {
    selectedValue.value = undefined
  },
  // 聚焦
  focus: () => {
    // 实现聚焦逻辑
  },
  // 失焦
  blur: () => {
    // 实现失焦逻辑
  }
})
</script>

<style lang="less" scoped>
.business-select {
  width: 100%;

  :deep(.ant-radio-group) {
    display: flex;
    flex-wrap: wrap;
    gap: 8px;
  }
}
</style>