<template>
  <div class="search-component">
    <a-button type="text" class="action-button" @click="showSearch">
      <SearchOutlined />
    </a-button>
    <a-modal v-model:open="visible" :footer="null" :closable="false" width="600px" wrapClassName="search-modal">
      <a-input-search v-model:value="searchText" placeholder="搜索..." size="large" @search="onSearch" @change="onChange">
        <template #prefix>
          <SearchOutlined />
        </template>
      </a-input-search>
      <div class="search-results" v-if="searchResults.length > 0">
        <a-list :data-source="searchResults" class="result-list">
          <template #renderItem="{ item }">
            <a-list-item>
              <a @click="handleResultClick(item)">{{ item.title }}</a>
            </a-list-item>
          </template>
        </a-list>
      </div>
    </a-modal>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { SearchOutlined } from '@ant-design/icons-vue'
import { useRouter } from 'vue-router'

const router = useRouter()
const visible = ref(false)
const searchText = ref('')
const searchResults = ref<any[]>([])

const showSearch = () => {
  visible.value = true
}

const onSearch = (value: string) => {
  // 实现搜索逻辑
  searchResults.value = []  // 这里添加实际的搜索结果
}

const onChange = (e: any) => {
  searchText.value = e.target.value
}

const handleResultClick = (item: any) => {
  visible.value = false
  // 处理搜索结果点击
  if (item.path) {
    router.push(item.path)
  }
}
</script>

<style lang="less" scoped>
.search-component {
  .action-button {
    display: flex;
    align-items: center;
    justify-content: center;
    width: 40px;
    height: 40px;
  }
}

.search-modal {
  :deep(.ant-modal-body) {
    padding: 24px;
  }

  .search-results {
    margin-top: 16px;
    max-height: 400px;
    overflow-y: auto;

    .result-list {
      border-radius: 2px;
    }
  }
}
</style>