<template>
  <div class="monitor-container">
    <a-row :gutter="24">
      <!-- CPU使用率 -->
      <a-col :span="12">
        <a-card :bordered="false" :title="t('dashboard.monitor.cpu')">
          <div ref="cpuChartRef" class="gauge-chart"></div>
          <div class="chart-footer">
            <div class="data-item">
              <span class="label">{{ t('dashboard.monitor.usage') }}</span>
              <span class="value">{{ cpuUsage }}%</span>
            </div>
            <div class="data-item">
              <span class="label">{{ t('dashboard.monitor.cores') }}</span>
              <span class="value">8</span>
            </div>
          </div>
        </a-card>
      </a-col>

      <!-- 内存使用率 -->
      <a-col :span="12">
        <a-card :bordered="false" :title="t('dashboard.monitor.memory')">
          <div ref="memoryChartRef" class="gauge-chart"></div>
          <div class="chart-footer">
            <div class="data-item">
              <span class="label">{{ t('dashboard.monitor.usage') }}</span>
              <span class="value">{{ memoryUsage }}%</span>
            </div>
            <div class="data-item">
              <span class="label">{{ t('dashboard.monitor.total') }}</span>
              <span class="value">16GB</span>
            </div>
          </div>
        </a-card>
      </a-col>

      <!-- 系统信息 -->
      <a-col :span="24">
        <a-card :bordered="false" :title="t('dashboard.monitor.system')">
          <a-descriptions :column="3">
            <a-descriptions-item :label="t('dashboard.monitor.os')">
              Windows 10 Pro
            </a-descriptions-item>
            <a-descriptions-item :label="t('dashboard.monitor.ip')">
              192.168.1.100
            </a-descriptions-item>
            <a-descriptions-item :label="t('dashboard.monitor.runtime')">
              24h 30m
            </a-descriptions-item>
            <a-descriptions-item :label="t('dashboard.monitor.framework')">
              .NET 8.0
            </a-descriptions-item>
            <a-descriptions-item :label="t('dashboard.monitor.database')">
              SQL Server 2019
            </a-descriptions-item>
            <a-descriptions-item :label="t('dashboard.monitor.redis')">
              Redis 7.0
            </a-descriptions-item>
          </a-descriptions>
        </a-card>
      </a-col>

      <!-- 实时日志 -->
      <a-col :span="24">
        <a-card :bordered="false" :title="t('dashboard.monitor.logs')">
          <a-list :data-source="logs" :loading="loading" size="small">
            <template #renderItem="{ item }">
              <a-list-item>
                <a-list-item-meta>
                  <template #title>
                    <span :class="['log-level', item.level.toLowerCase()]">
                      [{{ item.level }}]
                    </span>
                    {{ item.message }}
                  </template>
                  <template #description>
                    {{ item.time }}
                  </template>
                </a-list-item-meta>
              </a-list-item>
            </template>
          </a-list>
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
const cpuChartRef = ref<HTMLDivElement>()
const memoryChartRef = ref<HTMLDivElement>()
let cpuChart: echarts.ECharts | null = null
let memoryChart: echarts.ECharts | null = null

// 使用率数据
const cpuUsage = ref(65)
const memoryUsage = ref(45)

// 日志数据
const loading = ref(false)
const logs = ref([
  {
    level: 'INFO',
    message: 'System started successfully',
    time: '2024-03-20 10:30:00'
  },
  {
    level: 'WARN',
    message: 'High CPU usage detected',
    time: '2024-03-20 10:29:30'
  },
  {
    level: 'ERROR',
    message: 'Database connection timeout',
    time: '2024-03-20 10:28:45'
  },
  {
    level: 'INFO',
    message: 'New user registered',
    time: '2024-03-20 10:28:00'
  },
  {
    level: 'DEBUG',
    message: 'Cache cleared',
    time: '2024-03-20 10:27:30'
  }
])

// 初始化仪表盘
const initGaugeChart = (chart: echarts.ECharts, value: number) => {
  const option = {
    series: [
      {
        type: 'gauge',
        startAngle: 180,
        endAngle: 0,
        min: 0,
        max: 100,
        splitNumber: 10,
        itemStyle: {
          color: '#58D9F9',
          shadowColor: 'rgba(0,138,255,0.45)',
          shadowBlur: 10,
          shadowOffsetX: 2,
          shadowOffsetY: 2
        },
        progress: {
          show: true,
          roundCap: true,
          width: 18
        },
        pointer: {
          icon: 'path://M2090.36389,615.30999 L2090.36389,615.30999 C2091.48372,615.30999 2092.40383,616.194028 2092.44859,617.312956 L2096.90698,728.755929 C2097.05155,732.369577 2094.2393,735.416212 2090.62566,735.56078 C2090.53845,735.564269 2090.45117,735.566014 2090.36389,735.566014 L2090.36389,735.566014 C2086.74736,735.566014 2083.81557,732.63423 2083.81557,729.017692 C2083.81557,728.930412 2083.81732,728.84314 2083.82081,728.755929 L2088.2792,617.312956 C2088.32396,616.194028 2089.24407,615.30999 2090.36389,615.30999 Z',
          length: '75%',
          width: 16,
          offsetCenter: [0, '5%']
        },
        axisLine: {
          roundCap: true,
          lineStyle: {
            width: 18
          }
        },
        axisTick: {
          splitNumber: 2,
          lineStyle: {
            width: 2,
            color: '#999'
          }
        },
        splitLine: {
          length: 12,
          lineStyle: {
            width: 3,
            color: '#999'
          }
        },
        axisLabel: {
          distance: 30,
          color: '#999',
          fontSize: 12
        },
        title: {
          show: false
        },
        detail: {
          backgroundColor: '#fff',
          borderColor: '#999',
          borderWidth: 2,
          width: '60%',
          lineHeight: 40,
          height: 40,
          borderRadius: 8,
          offsetCenter: [0, '35%'],
          valueAnimation: true,
          formatter: function (value: number) {
            return '{value|' + value.toFixed(1) + '}{unit|%}'
          },
          rich: {
            value: {
              fontSize: 30,
              fontWeight: 'bolder',
              color: '#777'
            },
            unit: {
              fontSize: 14,
              color: '#999',
              padding: [0, 0, -20, 10]
            }
          }
        },
        data: [
          {
            value: value
          }
        ]
      }
    ]
  }
  chart.setOption(option)
}

// 初始化图表
const initCharts = () => {
  if (cpuChartRef.value) {
    cpuChart = echarts.init(cpuChartRef.value)
    initGaugeChart(cpuChart, cpuUsage.value)
  }
  if (memoryChartRef.value) {
    memoryChart = echarts.init(memoryChartRef.value)
    initGaugeChart(memoryChart, memoryUsage.value)
  }
}

// 监听窗口大小变化
const handleResize = () => {
  cpuChart?.resize()
  memoryChart?.resize()
}

onMounted(() => {
  initCharts()
  window.addEventListener('resize', handleResize)
})

onUnmounted(() => {
  window.removeEventListener('resize', handleResize)
  cpuChart?.dispose()
  memoryChart?.dispose()
})
</script>

<style lang="less" scoped>
.monitor-container {
  padding: 24px;

  .gauge-chart {
    height: 300px;
  }

  .chart-footer {
    display: flex;
    justify-content: space-around;
    padding: 16px 0;
    border-top: 1px solid #f0f0f0;

    .data-item {
      text-align: center;

      .label {
        display: block;
        color: rgba(0, 0, 0, 0.45);
        font-size: 14px;
      }

      .value {
        margin-top: 4px;
        font-size: 24px;
        font-weight: 500;
      }
    }
  }

  .log-level {
    padding: 2px 6px;
    border-radius: 4px;
    margin-right: 8px;
    font-size: 12px;

    &.info {
      background-color: #e6f7ff;
      color: #1890ff;
    }

    &.warn {
      background-color: #fffbe6;
      color: #faad14;
    }

    &.error {
      background-color: #fff2f0;
      color: #ff4d4f;
    }

    &.debug {
      background-color: #f6ffed;
      color: #52c41a;
    }
  }

  :deep(.ant-card) {
    margin-bottom: 24px;
  }
}
</style>