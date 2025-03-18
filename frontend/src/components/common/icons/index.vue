<template>
  <component :is="iconComponent" :type="isIconfont ? type : undefined" :style="{
    fontSize: size + 'px',
    color,
    transform: rotate ? `rotate(${rotate}deg)` : undefined
  }" :spin="spin" @click="handleClick" />
</template>

<script setup>
import { computed } from 'vue'
import * as Icons from '@ant-design/icons-vue'
import { IconFont } from '@/plugins/icons'

const props = defineProps({
  icon: {
    type: String,
    required: true
  },
  isIconfont: {
    type: Boolean,
    default: false
  },
  size: {
    type: Number,
    default: 16
  },
  color: {
    type: String,
    default: 'inherit'
  },
  rotate: {
    type: Number,
    default: 0
  },
  spin: {
    type: Boolean,
    default: false
  }
})

const emit = defineEmits(['click'])

// 获取图标组件
const iconComponent = computed(() => {
  if (props.isIconfont) {
    return IconFont
  }
  // 转换图标名称为组件名称
  const componentName = props.icon
    .split('-')
    .map(word => word.charAt(0).toUpperCase() + word.slice(1))
    .join('')
  return Icons[componentName] || null
})

// 处理点击事件
const handleClick = (event) => {
  emit('click', event)
}

// iconfont 的 type 属性
const type = computed(() => {
  return props.isIconfont ? `icon-${props.icon}` : undefined
})
</script>

<style>
@import '//at.alicdn.com/t/font_8d5l8fzk5b87iudi.css';

.anticon {
  color: var(--ant-color-text);
}
</style>