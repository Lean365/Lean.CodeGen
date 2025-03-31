export default {
  /** 通用操作 */
  actions: {
    /** 基础操作 */
    create: '创建',
    update: '更新',
    delete: '删除',
    preview: '预览',
    detail: '详情',
    save: '保存',
    submit: '提交',
    cancel: '取消',
    confirm: '确认',
    back: '返回',
    
    /** 查询操作 */
    search: '搜索',
    filter: '筛选',
    reset: '重置',
    refresh: '刷新',
    
    /** 数据操作 */
    import: '导入',
    export: '导出',
    download: '下载',
    upload: '上传',
    copy: '复制',
    paste: '粘贴',
    print: '打印',
    
    /** 列表操作 */
    more: '更多',
    expand: '展开',
    collapse: '收起',
    next: '下一个',
    previous: '上一个',
    
    /** 其他操作 */
    enable: '启用',
    disable: '禁用',
    config: '配置',
    settings: '设置',
    help: '帮助',
    about: '关于'
  },

  /** 通用状态 */
  status: {
    loading: '加载中',
    success: '成功',
    error: '错误',
    warning: '警告',
    info: '提示',
    
    /** 处理状态 */
    process: {
      pending: '待处理',
      processing: '处理中',
      completed: '已完成',
      failed: '失败',
      canceled: '已取消'
    }
  },

  /** 表单相关 */
  form: {
    /** 必填和选填 */
    required: '必填项',
    optional: '选填项',
    
    /** 输入提示 */
    pleaseSelect: '请选择',
    pleaseInput: '请输入',
    pleaseConfirm: '请确认',
    pleaseUpload: '请上传',
    
    /** 验证提示 */
    invalid: '输入无效',
    formatError: '格式错误',
    lengthError: '长度错误',
    rangeError: '超出范围',
    duplicated: '数据重复',
    
    /** 选择类型 */
    single: '单选',
    multiple: '多选',
    all: '全选',
    none: '取消全选',
    
    /** 文件相关 */
    file: {
      select: '选择文件',
      upload: '上传文件',
      preview: '预览文件',
      download: '下载文件',
      delete: '删除文件',
      size: '文件大小',
      type: '文件类型',
      name: '文件名称'
    },
    
    /** 图片相关 */
    image: {
      select: '选择图片',
      upload: '上传图片',
      preview: '预览图片',
      crop: '裁剪图片',
      rotate: '旋转图片',
      delete: '删除图片'
    }
  },

  /** 基础选项 */
  options: {
    yes: '是',
    no: '否'
  },

  /** HTTP 请求相关 */
  http: {
    /** 基础错误类型 */
    error: {
      default: '请求失败',
      network: '网络错误',
      timeout: '请求超时',
      config: '请求配置错误',
      serverError: '服务器错误',
      unknown: '未知错误',
      unauthorized: '未授权，请重新登录'
    },

    /** HTTP 状态码对应的错误信息 */
    status: {
      /** 客户端错误 4xx */
      400: '请求参数错误',
      401: '未授权，请重新登录',
      402: '需要付费',
      403: '拒绝访问',
      404: '请求的资源不存在',
      405: '不支持的请求方法',
      406: '不接受的请求',
      408: '请求超时',
      413: '请求实体过大',
      415: '不支持的媒体类型',
      422: '请求实体无法处理',
      423: '资源被锁定',
      428: '需要前置条件',
      429: '请求过于频繁',

      /** 服务器错误 5xx */
      500: '服务器内部错误',
      501: '服务未实现',
      502: '网关错误',
      503: '服务不可用',
      504: '网关超时',
      505: 'HTTP版本不支持'
    },

    /** 业务相关错误 */
    business: {
      success: '操作成功',
      error: '操作失败',
      networkError: '网络连接错误',
      serverError: '服务器响应错误',
      requestError: '请求参数错误',
      optionError: '请求选项错误',
      rateLimit: '请求频率超限，剩余可用请求次数: {remaining}'
    }
  },

  /** 页面头部 */
  header: {
    language: '切换语言',
    theme: '切换主题',
    profile: '个人信息',
    settings: '系统设置',
    logout: '退出登录'
  },

  /** 页面底部 */
  footer: {
    copyright: '© {year} HBT 代码生成器. 保留所有权利.',
    about: '关于我们',
    privacy: '隐私政策',
    terms: '服务条款'
  },

  /** 浏览器兼容性 */
  browser: {
    unsupported: {
      browser: '请使用以下浏览器访问：Chrome、Firefox、Safari、Edge、Opera、Brave 或 Vivaldi',
      windows: '请使用 Windows 7 及以上版本的操作系统',
      macos: '请使用 macOS Big Sur (11.0) 及以上版本的操作系统',
      linux: '请使用 Ubuntu、Debian、Fedora、CentOS 或 Arch Linux 等主流 Linux 发行版',
      android: '请使用 Android 11 及以上版本的操作系统',
      ios: '请使用 iOS 14 及以上版本的操作系统',
      os: '请使用以下操作系统：Windows、macOS、Linux、Android 或 iOS'
    }
  }
} 