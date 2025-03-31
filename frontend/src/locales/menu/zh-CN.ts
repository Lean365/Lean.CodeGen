export default {
  menu: {
    // 静态菜单
    dashboard: {
      _self: '仪表盘',
      index: '主页',
      analysis: '分析页',
      monitor: '监控页'
    },
    about: {
      _self: '关于',
      index: '关于',
      terms: '服务条款',
      privacy: '隐私政策'
    },
    // 动态菜单
    identity: {
      _self: '身份认证',
      user: '用户管理',
      role: '角色管理',
      menu: '菜单管理',
      dept: '部门管理',
      post: '岗位管理'
    },
    admin: {
      _self: '系统管理',
      dicttype: '字典类型',
      dictdata: '字典数据',
      config: '参数配置',
      language: '语言管理',
      translation: '翻译管理',
      localization: '本地化'
    },
    generator: {
      _self: '代码生成',
      gentask: '生成任务',
      gentemplate: '生成模板',
      genconfig: '生成配置',
      genhistory: '生成历史',
      datasource: '数据源',
      dbtable: '数据库表',
      tableconfig: '表配置'
    },
    workflow: {
      _self: '工作流',
      workflowdefinition: '流程定义',
      workflowinstance: '流程实例',
      workflowtask: '流程任务',
      workflowform: '流程表单',
      workflowvariable: '流程变量',
      workflowvariabledata: '变量数据',
      workflowactivitytype: '活动类型',
      workflowactivityproperty: '活动属性',
      workflowactivityinstance: '活动实例',
      workflowoutput: '流程输出',
      workflowoutcome: '流程结果',
      workflowhistory: '流程历史',
      workflowcorrelation: '流程关联'
    },
    signalr: {
      _self: '实时通信',
      onlineuser: '在线用户',
      onlinemessage: '在线消息'
    },
    audit: {
      _self: '审计日志',
      auditlog: '审计日志',
      operationlog: '操作日志',
      loginlog: '登录日志',
      exceptionlog: '异常日志',
      sqldifflog: 'SQL差异日志'
    }
  },
  button: {
    query: '查询',
    create: '新增',
    update: '修改',
    delete: '删除',
    clear: '清空',
    template: '模板',
    import: '导入',
    export: '导出',
    preview: '预览',
    print: '打印',
    audit: '审核',
    revoke: '撤销',
    translate: '翻译',
    icon: '图标'
  }
}