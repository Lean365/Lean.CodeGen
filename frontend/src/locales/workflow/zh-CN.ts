export default {
  /** 流程控制 */
  control: {
    start: '发起流程',
    submit: '提交审批',
    approve: '同意',
    reject: '拒绝',
    revoke: '撤回',
    transfer: '转办',
    delegate: '委托',
    suspend: '挂起',
    resume: '恢复',
    terminate: '终止'
  },

  /** 流程节点 */
  node: {
    add: '添加节点',
    edit: '编辑节点',
    delete: '删除节点',
    save: '保存节点',
    connect: '连接节点',
    disconnect: '断开连接',
    condition: '条件节点',
    gateway: '网关节点',
    task: '任务节点',
    start: '开始节点',
    end: '结束节点'
  },

  /** 流程设计 */
  design: {
    save: '保存设计',
    publish: '发布流程',
    import: '导入流程',
    export: '导出流程',
    test: '测试流程',
    validate: '验证流程',
    deploy: '部署流程',
    version: '版本管理'
  },

  /** 流程状态 */
  status: {
    draft: '草稿',
    pending: '待审批',
    processing: '处理中',
    approved: '已通过',
    rejected: '已拒绝',
    revoked: '已撤回',
    terminated: '已终止',
    suspended: '已挂起'
  },

  /** 流程角色 */
  role: {
    initiator: '发起人',
    approver: '审批人',
    handler: '处理人',
    observer: '观察者',
    admin: '管理员'
  }
} 