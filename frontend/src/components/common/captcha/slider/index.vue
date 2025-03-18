<template>
  <div class="slider-captcha" :class="{ 'slider-captcha--verified': isVerified }">
    <div class="slider-captcha__image-container" @click="handleImageClick" :data-refresh-tip="t('slider.refreshTip')">
      <img :src="getImageUrl(captcha?.backgroundImage)" :alt="t('slider.backgroundAlt')"
        class="slider-captcha__background" />
      <img v-if="captcha?.sliderImage" :src="getImageUrl(captcha?.sliderImage)" :alt="t('slider.sliderAlt')"
        class="slider-captcha__slider" :style="{ left: `${sliderLeft}px`, top: `${captcha?.y}px` }" />
    </div>
    <div class="slider-captcha__control">
      <a-slider v-model:value="sliderLeft" :min="0" :max="maxSliderLeft" :tooltip-open="false"
        :disabled="isVerified || isValidating" @change="handleSliderMove" @afterChange="handleSliderRelease" />
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, watch, onUnmounted } from 'vue';
import { useI18n } from 'vue-i18n';
import type { SliderCaptcha } from '@/types/identity/auth';
import type { SliderProps } from 'ant-design-vue';
import { useUserStore } from '@/stores/user';
import { message } from 'ant-design-vue';
import axios from 'axios';

const { t } = useI18n();
const userStore = useUserStore();
const props = defineProps<{
  captcha: SliderCaptcha | null;
}>();

const emit = defineEmits<{
  (e: 'refresh'): void;
  (e: 'validate', x: number, y: number): void;
}>();

const sliderLeft = ref<number>(0);
const maxSliderLeft = ref<number>(0);
const sliderWidth = ref<number>(0);
const isValidating = ref(false);
const validateTimer = ref<number | null>(null);
const refreshTimer = ref<number | null>(null);
const retryCount = ref(0);
const MAX_RETRIES = 3;
const isVerified = ref(false);
const isInitialized = ref(false);
const AUTO_REFRESH_INTERVAL = 10000;

// 处理图片URL
const getImageUrl = (imageData: string | undefined) => {
  // console.log('处理图片URL:', {
  //   hasData: !!imageData,
  //   isBase64: imageData?.startsWith('data:'),
  //   length: imageData?.length
  // })
  if (!imageData) return ''
  return imageData.startsWith('data:') ? imageData : `data:image/jpeg;base64,${imageData}`
}

// 滑块移动时的处理
const handleSliderMove = (value: number | number[]) => {
  // console.log('滑块移动:', { value, maxSliderLeft: maxSliderLeft.value })
}

// 清理所有定时器
const clearTimers = () => {
  if (validateTimer.value) {
    clearTimeout(validateTimer.value);
    validateTimer.value = null;
  }
  if (refreshTimer.value) {
    clearTimeout(refreshTimer.value);
    refreshTimer.value = null;
  }
};

// 延迟函数
const delay = (ms: number) => new Promise(resolve => setTimeout(resolve, ms));

// 设置自动刷新
const setupAutoRefresh = () => {
  clearTimers()
  if (!isVerified.value) {
    refreshTimer.value = window.setTimeout(refreshCaptcha, AUTO_REFRESH_INTERVAL);
  }
};

// 验证滑块位置
const validateSliderPosition = async (x: number, y: number): Promise<any> => {
  try {
    const result = await userStore.validateSliderCaptcha({
      captchaKey: props.captcha!.captchaKey,
      x,
      y
    })
    return result
  } catch (error) {
    if (retryCount.value < MAX_RETRIES) {
      retryCount.value++
      await delay(1000)
      return validateSliderPosition(x, y)
    }
    throw error
  }
}

// 滑块释放时触发验证
const handleSliderRelease = async (value: number | number[]) => {
  if (isValidating.value || isVerified.value) return

  if (typeof value === 'number' && props.captcha?.y !== undefined) {
    isValidating.value = true
    retryCount.value = 0
    console.info('开始验证滑块位置')

    try {
      clearTimers()
      const timeoutPromise = new Promise((_, reject) => {
        validateTimer.value = window.setTimeout(() => {
          reject(new Error(t('slider.timeout')))
        }, 10000)
      })

      const result = await Promise.race([
        validateSliderPosition(value, props.captcha.y),
        timeoutPromise
      ])

      clearTimers()

      if (result.success) {
        console.info('滑块验证通过')
        message.success(t('slider.success'))
        isVerified.value = true
        emit('validate', value, props.captcha.y)
      } else {
        console.warn('滑块验证失败:', result.message)
        message.error(result.message || t('slider.failed'))
        emit('refresh')
      }
    } catch (error) {
      if (axios.isAxiosError(error)) {
        if (error.code === 'ERR_NETWORK') {
          message.error(t('slider.networkError'))
        } else if (error.code === 'ECONNABORTED') {
          message.error(t('slider.requestTimeout'))
        } else {
          message.error(error.message || t('slider.error'))
        }
      } else {
        message.error(error instanceof Error ? error.message : t('slider.error'))
      }
      emit('refresh')
    } finally {
      clearTimers()
      isValidating.value = false
      if (!isVerified.value) {
        retryCount.value = 0
      }
    }
  }
}

// 处理图片点击
const handleImageClick = () => {
  if (!isValidating.value) {
    refreshCaptcha();
  }
};

// 点击背景图片刷新验证码
const refreshCaptcha = () => {
  // console.log('触发刷新验证码')
  clearTimers()
  emit('refresh')
}

// 监听验证码数据变化时重置所有状态
watch(() => props.captcha, (newCaptcha) => {
  // console.log('验证码数据更新:', {
  //   hasCaptcha: !!newCaptcha,
  //   captchaKey: newCaptcha?.captchaKey,
  //   hasBackgroundImage: !!newCaptcha?.backgroundImage,
  //   hasSliderImage: !!newCaptcha?.sliderImage,
  //   y: newCaptcha?.y
  // })

  if (!newCaptcha) return

  // 重置状态
  isValidating.value = false
  retryCount.value = 0
  sliderLeft.value = 0
  isVerified.value = false

  // 更新图片
  if (newCaptcha.backgroundImage && newCaptcha.sliderImage) {
    // 加载背景图片获取宽度
    const bgImg = new Image()
    bgImg.onload = () => {
      // 加载滑块图片获取宽度
      const sliderImg = new Image()
      sliderImg.onload = () => {
        sliderWidth.value = sliderImg.width
        maxSliderLeft.value = bgImg.width - sliderImg.width
        // 设置自动刷新
        setupAutoRefresh()
      }
      sliderImg.src = getImageUrl(newCaptcha.sliderImage)
    }
    bgImg.src = getImageUrl(newCaptcha.backgroundImage)
  }
}, { immediate: true })

// 组件卸载时清理所有状态
onUnmounted(() => {
  // console.log('组件卸载，清理状态:', {
  //   sliderLeft: sliderLeft.value,
  //   maxSliderLeft: maxSliderLeft.value,
  //   sliderWidth: sliderWidth.value,
  //   isValidating: isValidating.value,
  //   retryCount: retryCount.value,
  //   isVerified: isVerified.value
  // })
  clearTimers()
  sliderLeft.value = 0
  maxSliderLeft.value = 0
  sliderWidth.value = 0
  isValidating.value = false
  retryCount.value = 0
  isVerified.value = false
})
</script>

<style scoped lang="less">
.slider-captcha {
  width: 350px;

  &--verified {
    .slider-captcha__image-container {
      cursor: default;

      &:hover::after {
        display: none;
      }
    }

    .slider-captcha__slider {
      border: 2px solid var(--ant-color-primary);
    }
  }

  &__image-container {
    position: relative;
    width: 100%;
    height: 150px;
    cursor: pointer;

    &:hover::after {
      content: attr(data-refresh-tip);
      position: absolute;
      top: 50%;
      left: 50%;
      transform: translate(-50%, -50%);
      padding: 8px 16px;
      border-radius: 4px;
      font-size: 14px;
      color: #fff;
      background: rgba(0, 0, 0, 0.6);
      white-space: nowrap;
      z-index: 1;
    }
  }

  &__background {
    width: 100%;
    height: 100%;
    display: block;
    object-fit: cover;
  }

  &__slider {
    position: absolute;
    width: 48px;
    height: 48px;
    transition: left 0.1s linear;
    background: var(--ant-color-primary);
  }

  &__control {
    margin-top: 16px;
  }
}
</style>