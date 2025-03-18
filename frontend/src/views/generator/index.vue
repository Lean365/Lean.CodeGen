<template>
  <div class="generator-container">
    <a-card :bordered="false">
      <template #title>
        <span>{{ t('generator.title') }}</span>
      </template>

      <!-- 数据源配置 -->
      <a-form :model="dataSourceForm" layout="vertical">
        <a-form-item :label="t('generator.dataSource.type')">
          <a-select v-model:value="dataSourceForm.type">
            <a-select-option value="mysql">MySQL</a-select-option>
            <a-select-option value="postgresql">PostgreSQL</a-select-option>
            <a-select-option value="sqlserver">SQL Server</a-select-option>
          </a-select>
        </a-form-item>

        <a-form-item :label="t('generator.dataSource.host')">
          <a-input v-model:value="dataSourceForm.host" />
        </a-form-item>

        <a-form-item :label="t('generator.dataSource.port')">
          <a-input-number v-model:value="dataSourceForm.port" />
        </a-form-item>

        <a-form-item :label="t('generator.dataSource.database')">
          <a-input v-model:value="dataSourceForm.database" />
        </a-form-item>

        <a-form-item :label="t('generator.dataSource.username')">
          <a-input v-model:value="dataSourceForm.username" />
        </a-form-item>

        <a-form-item :label="t('generator.dataSource.password')">
          <a-input-password v-model:value="dataSourceForm.password" />
        </a-form-item>

        <a-form-item>
          <a-button type="primary" @click="handleTestConnection">
            {{ t('generator.dataSource.test') }}
          </a-button>
        </a-form-item>
      </a-form>

      <!-- 表格选择 -->
      <a-divider />

      <a-form :model="generatorForm" layout="vertical">
        <a-form-item :label="t('generator.table.select')">
          <a-table :columns="tableColumns" :data-source="tables"
            :row-selection="{ selectedRowKeys: selectedTables, onChange: onSelectChange }" :pagination="false"
            size="small">
            <template #bodyCell="{ column, record }">
              <template v-if="column.key === 'action'">
                <a @click="handlePreview(record)">{{ t('generator.table.preview') }}</a>
              </template>
            </template>
          </a-table>
        </a-form-item>

        <a-form-item>
          <a-button type="primary" @click="handleGenerate" :disabled="!selectedTables.length">
            {{ t('generator.generate') }}
          </a-button>
        </a-form-item>
      </a-form>
    </a-card>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive } from 'vue'
import { useI18n } from 'vue-i18n'

const { t } = useI18n()

// 数据源表单
const dataSourceForm = reactive({
  type: 'mysql',
  host: 'localhost',
  port: 3306,
  database: '',
  username: '',
  password: ''
})

// 生成器表单
const generatorForm = reactive({})

// 表格列定义
const tableColumns = [
  {
    title: t('generator.table.name'),
    dataIndex: 'name',
    key: 'name'
  },
  {
    title: t('generator.table.comment'),
    dataIndex: 'comment',
    key: 'comment'
  },
  {
    title: t('generator.table.action'),
    key: 'action'
  }
]

// 表格数据
const tables = ref([])
const selectedTables = ref<string[]>([])

// 测试连接
const handleTestConnection = async () => {
  // TODO: 实现测试数据库连接的逻辑
}

// 选择表格
const onSelectChange = (selected: string[]) => {
  selectedTables.value = selected
}

// 预览代码
const handlePreview = (record: any) => {
  // TODO: 实现预览生成代码的逻辑
}

// 生成代码
const handleGenerate = async () => {
  // TODO: 实现生成代码的逻辑
}
</script>

<style lang="less" scoped>
.generator-container {
  padding: 24px;

  .ant-card {
    margin-bottom: 24px;
  }

  .ant-divider {
    margin: 24px 0;
  }
}
</style>