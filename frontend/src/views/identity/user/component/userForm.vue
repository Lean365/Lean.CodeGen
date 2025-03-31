<template>
  <a-modal :visible="visible" :title="title" :confirm-loading="loading" :mask-closable="false" @ok="handleSubmit"
    @cancel="handleCancel">
    <a-form ref="formRef" :model="formData" :rules="rules" :label-col="{ span: 4 }" :wrapper-col="{ span: 19 }">
      <a-form-item label="用户名称" name="userName">
        <a-input v-model:value="formData.userName" placeholder="请输入用户名称" :disabled="!!formData.id" />
      </a-form-item>
      <a-form-item label="用户昵称" name="nickName">
        <a-input v-model:value="formData.nickName" placeholder="请输入用户昵称" />
      </a-form-item>
      <a-form-item label="部门" name="deptId">
        <business-tree-select v-model:value="formData.deptId" :tree-data="deptOptions" placeholder="请选择部门" />
      </a-form-item>
      <a-form-item label="手机号码" name="phoneNumber">
        <a-input v-model:value="formData.phoneNumber" placeholder="请输入手机号码" />
      </a-form-item>
      <a-form-item label="邮箱" name="email">
        <a-input v-model:value="formData.email" placeholder="请输入邮箱" />
      </a-form-item>
      <a-form-item label="用户性别" name="sex">
        <business-select v-model:value="formData.sex" dict-type="sys_user_sex" placeholder="请选择用户性别" />
      </a-form-item>
      <a-form-item label="状态" name="status">
        <business-select v-model:value="formData.status" dict-type="sys_normal_disable" placeholder="请选择状态" />
      </a-form-item>
      <a-form-item label="岗位" name="postIds">
        <business-select v-model:value="formData.postIds" :options="postOptions" mode="multiple" placeholder="请选择岗位" />
      </a-form-item>
      <a-form-item label="角色" name="roleIds">
        <business-select v-model:value="formData.roleIds" :options="roleOptions" mode="multiple" placeholder="请选择角色" />
      </a-form-item>
      <a-form-item v-if="!formData.id" label="密码" name="password">
        <a-input-password v-model:value="formData.password" placeholder="请输入密码" />
      </a-form-item>
      <a-form-item label="备注" name="remark">
        <a-textarea v-model:value="formData.remark" placeholder="请输入备注" :rows="4" />
      </a-form-item>
    </a-form>
  </a-modal>
</template>

<script lang="ts" setup>
import { ref } from 'vue'
import type { FormInstance } from 'ant-design-vue'
import type { RuleObject } from 'ant-design-vue/es/form/interface'
import { message } from 'ant-design-vue'
import { createUser, updateUser } from '@/api/identity/user'

interface Props {
  visible: boolean
  title?: string
  formData: Record<string, any>
  deptOptions: any[]
  postOptions: any[]
  roleOptions: any[]
}

const props = withDefaults(defineProps<Props>(), {
  visible: false,
  title: '添加用户',
  formData: () => ({}),
  deptOptions: () => [],
  postOptions: () => [],
  roleOptions: () => []
})

const emit = defineEmits<{
  (e: 'update:visible', visible: boolean): void
  (e: 'success'): void
}>()

// 表单引用
const formRef = ref<FormInstance>()

// 加载状态
const loading = ref(false)

// 表单校验规则
const rules: Record<string, RuleObject[]> = {
  userName: [
    { required: true, message: '请输入用户名称' },
    { min: 2, max: 20, message: '用户名称长度必须介于 2 和 20 之间' }
  ],
  nickName: [
    { required: true, message: '请输入用户昵称' },
    { min: 2, max: 20, message: '用户昵称长度必须介于 2 和 20 之间' }
  ],
  deptId: [
    { required: true, message: '请选择部门' }
  ],
  phoneNumber: [
    { pattern: /^1[3-9]\d{9}$/, message: '请输入正确的手机号码' }
  ],
  email: [
    { type: 'email', message: '请输入正确的邮箱地址' }
  ],
  sex: [
    { required: true, message: '请选择用户性别' }
  ],
  status: [
    { required: true, message: '请选择状态' }
  ],
  password: [
    { required: true, message: '请输入密码' },
    { min: 6, max: 20, message: '密码长度必须介于 6 和 20 之间' }
  ]
}

// 提交表单
const handleSubmit = async () => {
  try {
    await formRef.value?.validate()
    loading.value = true

    if (props.formData.id) {
      await updateUser(props.formData.id, props.formData)
      message.success('修改成功')
    } else {
      await createUser(props.formData)
      message.success('新增成功')
    }

    emit('success')
    handleCancel()
  } catch (error) {
    console.error('表单提交失败：', error)
  } finally {
    loading.value = false
  }
}

// 取消
const handleCancel = () => {
  formRef.value?.resetFields()
  emit('update:visible', false)
}
</script>