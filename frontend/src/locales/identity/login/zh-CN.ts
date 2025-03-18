export default {
  login: {
    title: {
      text: '用户登录',
      description: '欢迎使用代码生成器'
    },
    form: {
      username: {
        label: '用户名',
        placeholder: '请输入用户名',
        required: '请输入用户名'
      },
      password: {
        label: '密码',
        placeholder: '请输入密码',
        required: '请输入密码'
      },
      remember: {
        text: '记住我',
        tip: '7天内自动登录'
      },
      submit: {
        text: '登录',
        loading: '登录中...'
      }
    },
    links: {
      forgot: {
        text: '忘记密码',
        tip: '请联系管理员重置密码'
      },
      terms: {
        text: '服务条款',
        tip: '查看服务条款'
      },
      privacy: {
        text: '隐私政策',
        tip: '查看隐私政策'
      }
    },
    messages: {
      success: '登录成功',
      failed: '登录失败',
      validateFailed: '请检查输入信息',
      accountLocked: '账号已锁定',
      accountDisabled: '账号已禁用',
      accountExpired: '账号已过期',
      passwordExpired: '密码已过期'
    }
  }
}