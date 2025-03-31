<template>
  <a-modal :visible="visible" title="更换头像" :confirm-loading="loading" :mask-closable="false" @ok="handleSubmit"
    @cancel="handleCancel">
    <div class="avatar-upload-container">
      <div class="avatar-preview">
        <img :src="previewUrl || defaultAvatar" alt="头像预览" />
      </div>
      <div class="upload-area">
        <a-upload accept="image/*" :show-upload-list="false" :before-upload="beforeUpload">
          <a-button type="primary">
            <upload-outlined />
            选择图片
          </a-button>
        </a-upload>
        <div class="upload-tip">
          请上传jpg/png格式的图片，大小不超过2MB
        </div>
      </div>
    </div>
  </a-modal>
</template>

<script lang="ts" setup>
import { ref } from 'vue'
import { message } from 'ant-design-vue'
import { UploadOutlined } from '@ant-design/icons-vue'
import { updateUserAvatarAsync } from '@/api/identity/user'
import defaultAvatar from '@/assets/images/default-avatar.png'

interface Props {
  visible: boolean
}

const props = withDefaults(defineProps<Props>(), {
  visible: false
})

const emit = defineEmits<{
  (e: 'update:visible', visible: boolean): void
  (e: 'success', avatarUrl: string): void
}>()

const loading = ref(false)
const previewUrl = ref('')
let fileToUpload: File | null = null

const beforeUpload = (file: File) => {
  const isImage = file.type.startsWith('image/')
  if (!isImage) {
    message.error('请上传图片文件！')
    return false
  }

  const isLt2M = file.size / 1024 / 1024 < 2
  if (!isLt2M) {
    message.error('图片大小不能超过2MB！')
    return false
  }

  // 预览图片
  const reader = new FileReader()
  reader.readAsDataURL(file)
  reader.onload = (e) => {
    previewUrl.value = e.target?.result as string
  }

  fileToUpload = file
  return false
}

const handleSubmit = async () => {
  if (!fileToUpload) {
    message.error('请先选择图片')
    return
  }

  try {
    loading.value = true
    const formData = new FormData()
    formData.append('avatar', fileToUpload)

    const { avatarUrl } = await updateUserAvatarAsync(formData)
    message.success('头像更新成功')
    emit('success', avatarUrl)
    handleCancel()
  } catch (error) {
    console.error('上传头像失败：', error)
  } finally {
    loading.value = false
  }
}

const handleCancel = () => {
  previewUrl.value = ''
  fileToUpload = null
  emit('update:visible', false)
}
</script>

<style lang="less" scoped>
.avatar-upload-container {
  display: flex;
  align-items: center;
  justify-content: space-around;
  padding: 20px;

  .avatar-preview {
    width: 128px;
    height: 128px;
    border-radius: 50%;
    overflow: hidden;
    border: 1px solid #d9d9d9;

    img {
      width: 100%;
      height: 100%;
      object-fit: cover;
    }
  }

  .upload-area {
    flex: 1;
    margin-left: 32px;
    text-align: center;

    .upload-tip {
      margin-top: 8px;
      color: #999;
      font-size: 12px;
    }
  }
}
</style>