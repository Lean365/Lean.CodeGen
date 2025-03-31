<template>
  <a-modal :visible="visible" title="重置密码" :confirm-loading="loading" :mask-closable="false" @ok="handleSubmit"
    @cancel="handleCancel">
    <a-form ref="formRef" :model="formData" :rules="rules" :label-col="{ span: 4 }" :wrapper-col="{ span: 19 }">
      <a-form-item label="新密码" name="password">
        <a-input-password v-model:value="formData.password" placeholder="请输入新密码" />
      </a-form-item>
      <a-form-item label="确认密码" name="confirmPassword">
        <a-input-password v-model:value="formData.confirmPassword" placeholder="请再次输入新密码" />
      </a-form-item>
    </a-form>
  </a-modal>
</template>

<script lang="ts" setup>
import { ref, reactive } from 'vue'
import type { FormInstance } from 'ant-design-vue'
import type { RuleObject } from 'ant-design-vue/es/form/interface'
import { message } from 'ant-design-vue'
import { resetUserPassword } from '@/api/identity/user'

interface Props {
  visible: boolean
  userId: number
}

const props = withDefaults(defineProps<Props>(), {
  visible: false,
  userId: 0
})

const emit = defineEmits<{
  (e: 'update:visible', visible: boolean): void
  (e: 'success'): void
}>()

const formRef = ref<FormInstance>()
const loading = ref(false)

const formData = reactive({
  password: '',
  confirmPassword: ''
})

const validateConfirmPassword = async (_rule: RuleObject, value: string) => {
  if (value !== formData.password) {
    return Promise.reject('两次输入的密码不一致')
  }
  return Promise.resolve()
}

const rules = {
  password: [
    { required: true, message: '请输入新密码' },
    { min: 6, max: 20, message: '密码长度必须介于 6 和 20 之间' }
  ],
  confirmPassword: [
    { required: true, message: '请再次输入新密码' },
    { validator: validateConfirmPassword, trigger: 'change' }
  ]
}

const handleSubmit = async () => {
  try {
    await formRef.value?.validate()
    loading.value = true

    await resetUserPassword(props.userId, formData.password)
    message.success('密码重置成功')
    emit('success')
    handleCancel()
  } catch (error) {
    console.error('重置密码失败：', error)
  } finally {
    loading.value = false
  }
}

const handleCancel = () => {
  formRef.value?.resetFields()
  emit('update:visible', false)
}
</script>