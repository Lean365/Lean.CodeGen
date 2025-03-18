// 引入 echarts 核心模块
import * as echarts from 'echarts/core'

// 引入图表类型
import {
  BarChart,
  LineChart,
  PieChart,
  ScatterChart,
  RadarChart,
  GaugeChart
} from 'echarts/charts'

// 引入提示框、标题、直角坐标系、数据集、内置数据转换器组件
import {
  TitleComponent,
  TooltipComponent,
  GridComponent,
  DatasetComponent,
  TransformComponent,
  LegendComponent,
  ToolboxComponent
} from 'echarts/components'

// 标签自动布局、全局过渡动画等特性
import { LabelLayout, UniversalTransition } from 'echarts/features'

// 引入 Canvas 渲染器
import { CanvasRenderer } from 'echarts/renderers'

// 注册必须的组件
echarts.use([
  BarChart,
  LineChart,
  PieChart,
  ScatterChart,
  RadarChart,
  GaugeChart,
  TitleComponent,
  TooltipComponent,
  GridComponent,
  DatasetComponent,
  TransformComponent,
  LegendComponent,
  ToolboxComponent,
  LabelLayout,
  UniversalTransition,
  CanvasRenderer
])

export { echarts } 