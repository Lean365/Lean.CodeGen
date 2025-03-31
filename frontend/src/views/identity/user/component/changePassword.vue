<template>
  <a-modal :visible="visible" title="修改密码" :confirm-loading="loading" :mask-closable="false" @ok="handleSubmit"
    @cancel="handleCancel">
    <a-form ref="formRef" :model="formData" :rules="rules" :label-col="{ span: 4 }" :wrapper-col="{ span: 19 }">
      <a-form-item label="原密码" name="oldPassword">
        <a-input-password v-model:value="formData.oldPassword" placeholder="请输入原密码" />
      </a-form-item>
      <a-form-item label="新密码" name="newPassword">
        <a-input-password v-model:value="formData.newPassword" placeholder="请输入新密码" />
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
import { changeUserPassword } from '@/api/identity/user'

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
  oldPassword: '',
  newPassword: '',
  confirmPassword: ''
})

const validateConfirmPassword = async (_rule: RuleObject, value: string) => {
  if (value !== formData.newPassword) {
    return Promise.reject('两次输入的密码不一致')
  }
  return Promise.resolve()
}

const rules = {
  oldPassword: [
    { required: true, message: '请输入原密码' },
    { min: 6, max: 20, message: '密码长度必须介于 6 和 20 之间' }
  ],
  newPassword: [
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

    await changeUserPassword({
      id: props.userId,
      oldPassword: formData.oldPassword,
      newPassword: formData.newPassword
    })

    message.success('密码修改成功')
    emit('success')
    handleCancel()
  } catch (error) {
    console.error('修改密码失败：', error)
  } finally {
    loading.value = false
  }
}

const handleCancel = () => {
  formRef.value?.resetFields()
  emit('update:visible', false)
}
</script>