export default {
  dashboard: {
    title: '仪表盘',
    welcome: {
      title: '欢迎回来',
      subtitle: '今天是个不错的日子',
      morning: '早上好',
      afternoon: '下午好',
      evening: '晚上好',
      greeting: '{time}，{name}',
      description: '欢迎使用 Lean.CodeGen 代码生成平台'
    },
    stats: {
      users: '用户总数',
      projects: '项目数量',
      tasks: '任务数量',
      messages: '消息数量'
    },
    analysis: {
      title: '数据分析',
      trend: {
        title: '访问趋势',
        visitors: '访问人数',
        users: '用户数量'
      },
      distribution: {
        title: '来源分布',
        direct: '直接访问',
        social: '社交媒体',
        search: '搜索引擎',
        other: '其他来源'
      },
      ranking: {
        title: '功能排行',
        column: {
          rank: '排名',
          feature: '功能',
          count: '使用次数'
        }
      }
    },
    monitor: {
      title: '系统监控',
      cpu: {
        title: 'CPU监控',
        usage: '使用率',
        cores: '核心数',
        speed: '主频',
        temperature: '温度'
      },
      memory: {
        title: '内存监控',
        usage: '使用率',
        total: '总内存',
        available: '可用内存'
      },
      system: {
        title: '系统信息',
        os: '操作系统',
        ip: 'IP地址',
        runtime: '运行时间',
        framework: '框架版本',
        database: '数据库',
        redis: 'Redis版本'
      },
      logs: {
        title: '实时日志',
        level: {
          info: '信息',
          warn: '警告',
          error: '错误',
          debug: '调试'
        },
        time: '时间',
        content: '内容'
      }
    },
    notification: {
      title: '通知中心',
      empty: '暂无通知',
      viewAll: '查看全部',
      markAllRead: '全部已读',
      clear: '清空通知',
      types: {
        system: '系统通知',
        business: '业务通知',
        warning: '预警通知'
      },
      status: {
        unread: '未读',
        read: '已读'
      }
    }
  }
}
