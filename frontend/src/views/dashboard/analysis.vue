<template>
  <div class="analysis-container">
    <a-row :gutter="24">
      <!-- 趋势图表 -->
      <a-col :span="24">
        <a-card :bordered="false" :title="t('dashboard.analysis.trend')">
          <div ref="trendChartRef" class="trend-chart"></div>
        </a-card>
      </a-col>

      <!-- 数据分布 -->
      <a-col :span="12">
        <a-card :bordered="false" :title="t('dashboard.analysis.distribution')">
          <div ref="pieChartRef" class="pie-chart"></div>
        </a-card>
      </a-col>

      <!-- 排行榜 -->
      <a-col :span="12">
        <a-card :bordered="false" :title="t('dashboard.analysis.ranking')">
          <a-table :columns="rankColumns" :data-source="rankData" :pagination="false" size="small">
            <template #bodyCell="{ column, record }">
              <template v-if="column.key === 'index'">
                <span :class="['rank-index', record.index <= 3 ? 'top-3' : '']">
                  {{ record.index }}
                </span>
              </template>
            </template>
          </a-table>
        </a-card>
      </a-col>
    </a-row>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, onUnmounted } from 'vue'
import { useI18n } from 'vue-i18n'
import * as echarts from 'echarts'

const { t } = useI18n()

// 图表实例
const trendChartRef = ref<HTMLDivElement>()
const pieChartRef = ref<HTMLDivElement>()
let trendChart: echarts.ECharts | null = null
let pieChart: echarts.ECharts | null = null

// 排行榜列定义
const rankColumns = [
  {
    title: t('dashboard.analysis.rank'),
    dataIndex: 'index',
    key: 'index',
    width: 80
  },
  {
    title: t('dashboard.analysis.name'),
    dataIndex: 'name',
    key: 'name'
  },
  {
    title: t('dashboard.analysis.count'),
    dataIndex: 'count',
    key: 'count',
    align: 'right'
  }
]

// 排行榜数据
const rankData = [
  { index: 1, name: 'User Management', count: 1234 },
  { index: 2, name: 'Role Management', count: 923 },
  { index: 3, name: 'Menu Management', count: 845 },
  { index: 4, name: 'Department Management', count: 678 },
  { index: 5, name: 'Post Management', count: 567 }
]

// 初始化趋势图表
const initTrendChart = () => {
  if (!trendChartRef.value) return

  trendChart = echarts.init(trendChartRef.value)
  const option = {
    tooltip: {
      trigger: 'axis'
    },
    legend: {
      data: ['访问量', '用户数']
    },
    grid: {
      left: '3%',
      right: '4%',
      bottom: '3%',
      containLabel: true
    },
    xAxis: {
      type: 'category',
      boundaryGap: false,
      data: ['周一', '周二', '周三', '周四', '周五', '周六', '周日']
    },
    yAxis: {
      type: 'value'
    },
    series: [
      {
        name: '访问量',
        type: 'line',
        data: [120, 132, 101, 134, 90, 230, 210]
      },
      {
        name: '用户数',
        type: 'line',
        data: [220, 182, 191, 234, 290, 330, 310]
      }
    ]
  }
  trendChart.setOption(option)
}

// 初始化饼图
const initPieChart = () => {
  if (!pieChartRef.value) return

  pieChart = echarts.init(pieChartRef.value)
  const option = {
    tooltip: {
      trigger: 'item'
    },
    legend: {
      orient: 'vertical',
      left: 'left'
    },
    series: [
      {
        name: '访问来源',
        type: 'pie',
        radius: '50%',
        data: [
          { value: 1048, name: '搜索引擎' },
          { value: 735, name: '直接访问' },
          { value: 580, name: '邮件营销' },
          { value: 484, name: '联盟广告' },
          { value: 300, name: '视频广告' }
        ],
        emphasis: {
          itemStyle: {
            shadowBlur: 10,
            shadowOffsetX: 0,
            shadowColor: 'rgba(0, 0, 0, 0.5)'
          }
        }
      }
    ]
  }
  pieChart.setOption(option)
}

// 监听窗口大小变化
const handleResize = () => {
  trendChart?.resize()
  pieChart?.resize()
}

onMounted(() => {
  initTrendChart()
  initPieChart()
  window.addEventListener('resize', handleResize)
})

onUnmounted(() => {
  window.removeEventListener('resize', handleResize)
  trendChart?.dispose()
  pieChart?.dispose()
})
</script>

<style lang="less" scoped>
.analysis-container {
  padding: 24px;

  .trend-chart {
    height: 400px;
  }

  .pie-chart {
    height: 300px;
  }

  .rank-index {
    display: inline-block;
    width: 20px;
    height: 20px;
    line-height: 20px;
    text-align: center;
    border-radius: 50%;
    background-color: #f0f0f0;

    &.top-3 {
      color: #fff;
      background-color: #1890ff;
    }
  }

  :deep(.ant-card) {
    margin-bottom: 24px;
  }
}
</style>