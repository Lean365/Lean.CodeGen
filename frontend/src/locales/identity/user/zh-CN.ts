export default {
  username: '用户名',
  nickname: '昵称',
  password: '密码',
  email: '邮箱',
  mobile: '手机号',
  status: '状态',
  createTime: '创建时间',
  role: '角色',
  dept: '部门',
  remark: '备注',
  enable: '启用',
  disable: '禁用',
  resetPassword: '重置密码',
  modifyPassword: '修改密码',
  oldPassword: '原密码',
  newPassword: '新密码',
  confirmPassword: '确认密码',
  passwordNotMatch: '两次输入的密码不一致',
  modifySuccess: '修改成功',
  modifyFailed: '修改失败',
  resetSuccess: '重置成功',
  resetFailed: '重置失败',
  identity: {
    title: '身份认证',
    user: {
      title: '用户管理',
      add: '新增用户',
      edit: '编辑用户',
      search: {
        username: '请输入用户名',
        status: {
          all: '全部状态',
          enabled: '已启用',
          disabled: '已禁用'
        }
      },
      table: {
        username: '用户名',
        email: '邮箱',
        status: '状态',
        lastLoginTime: '最后登录时间'
      },
      form: {
        username: '用户名',
        password: '密码',
        email: '邮箱',
        status: '状态'
      },
      status: {
        enabled: '已启用',
        disabled: '已禁用'
      },
      rules: {
        username: {
          required: '请输入用户名',
          length: '用户名长度必须在2-20个字符之间'
        },
        password: {
          required: '请输入密码',
          length: '密码长度必须在6-20个字符之间'
        },
        email: {
          required: '请输入邮箱',
          format: '请输入正确的邮箱格式'
        }
      }
    }
  }
} 