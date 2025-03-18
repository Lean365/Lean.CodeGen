//===================================================
// 项目名: Lean.CodeGen.Common
// 文件名: LeanBusinessType.cs
// 功能描述: 业务操作类型枚举
// 创建时间: 2024-03-26
// 作者: Lean
// 版本: 1.0
//===================================================

using System.ComponentModel;

namespace Lean.CodeGen.Common.Enums;

/// <summary>
/// 业务操作类型枚举
/// </summary>
public enum LeanBusinessType
{
  /// <summary>
  /// 其他
  /// </summary>
  [Description("其他")]
  Other = 0,

  /// <summary>
  /// 查询
  /// </summary>
  [Description("查询")]
  Query = 1,

  /// <summary>
  /// 新增
  /// </summary>
  [Description("新增")]
  Create = 2,

  /// <summary>
  /// 修改
  /// </summary>
  [Description("修改")]
  Update = 3,

  /// <summary>
  /// 删除
  /// </summary>
  [Description("删除")]
  Delete = 4,

  /// <summary>
  /// 导出
  /// </summary>
  [Description("导出")]
  Export = 5,

  /// <summary>
  /// 导入
  /// </summary>
  [Description("导入")]
  Import = 6,

  /// <summary>
  /// 上传
  /// </summary>
  [Description("上传")]
  Upload = 7,

  /// <summary>
  /// 下载
  /// </summary>
  [Description("下载")]
  Download = 8,

  /// <summary>
  /// 打印
  /// </summary>
  [Description("打印")]
  Print = 9,

  /// <summary>
  /// 预览
  /// </summary>
  [Description("预览")]
  Preview = 10,

  /// <summary>
  /// 复制
  /// </summary>
  [Description("复制")]
  Copy = 11,

  /// <summary>
  /// 移动
  /// </summary>
  [Description("移动")]
  Move = 12,

  /// <summary>
  /// 重命名
  /// </summary>
  [Description("重命名")]
  Rename = 13,

  /// <summary>
  /// 合并
  /// </summary>
  [Description("合并")]
  Merge = 14,

  /// <summary>
  /// 分割
  /// </summary>
  [Description("分割")]
  Split = 15,

  /// <summary>
  /// 排序
  /// </summary>
  [Description("排序")]
  Sort = 16,

  /// <summary>
  /// 过滤
  /// </summary>
  [Description("过滤")]
  Filter = 17,

  /// <summary>
  /// 分组
  /// </summary>
  [Description("分组")]
  Group = 18,

  /// <summary>
  /// 统计
  /// </summary>
  [Description("统计")]
  Statistics = 19,

  /// <summary>
  /// 分析
  /// </summary>
  [Description("分析")]
  Analysis = 20,

  /// <summary>
  /// 比较
  /// </summary>
  [Description("比较")]
  Compare = 21,

  /// <summary>
  /// 验证
  /// </summary>
  [Description("验证")]
  Validate = 22,

  /// <summary>
  /// 审核
  /// </summary>
  [Description("审核")]
  Audit = 23,

  /// <summary>
  /// 审批
  /// </summary>
  [Description("审批")]
  Approve = 24,

  /// <summary>
  /// 驳回
  /// </summary>
  [Description("驳回")]
  Reject = 25,

  /// <summary>
  /// 提交
  /// </summary>
  [Description("提交")]
  Submit = 26,

  /// <summary>
  /// 撤回
  /// </summary>
  [Description("撤回")]
  Revoke = 27,

  /// <summary>
  /// 发布
  /// </summary>
  [Description("发布")]
  Publish = 28,

  /// <summary>
  /// 撤销发布
  /// </summary>
  [Description("撤销发布")]
  Unpublish = 29,

  /// <summary>
  /// 启用
  /// </summary>
  [Description("启用")]
  Enable = 30,

  /// <summary>
  /// 禁用
  /// </summary>
  [Description("禁用")]
  Disable = 31,

  /// <summary>
  /// 锁定
  /// </summary>
  [Description("锁定")]
  Lock = 32,

  /// <summary>
  /// 解锁
  /// </summary>
  [Description("解锁")]
  Unlock = 33,

  /// <summary>
  /// 授权
  /// </summary>
  [Description("授权")]
  Grant = 34,

  /// <summary>
  /// 取消授权
  /// </summary>
  [Description("取消授权")]
  Ungrant = 35,

  /// <summary>
  /// 分配
  /// </summary>
  [Description("分配")]
  Assign = 36,

  /// <summary>
  /// 取消分配
  /// </summary>
  [Description("取消分配")]
  Unassign = 37,

  /// <summary>
  /// 转办
  /// </summary>
  [Description("转办")]
  Transfer = 38,

  /// <summary>
  /// 委托
  /// </summary>
  [Description("委托")]
  Delegate = 39,

  /// <summary>
  /// 签收
  /// </summary>
  [Description("签收")]
  Sign = 40,

  /// <summary>
  /// 退回
  /// </summary>
  [Description("退回")]
  Return = 41,

  /// <summary>
  /// 归档
  /// </summary>
  [Description("归档")]
  Archive = 42,

  /// <summary>
  /// 备份
  /// </summary>
  [Description("备份")]
  Backup = 43,

  /// <summary>
  /// 还原
  /// </summary>
  [Description("还原")]
  Restore = 44,

  /// <summary>
  /// 清理
  /// </summary>
  [Description("清理")]
  Clean = 45,

  /// <summary>
  /// 同步
  /// </summary>
  [Description("同步")]
  Sync = 46,

  /// <summary>
  /// 刷新
  /// </summary>
  [Description("刷新")]
  Refresh = 47,

  /// <summary>
  /// 重置
  /// </summary>
  [Description("重置")]
  Reset = 48,

  /// <summary>
  /// 初始化
  /// </summary>
  [Description("初始化")]
  Initialize = 49,

  /// <summary>
  /// 配置
  /// </summary>
  [Description("配置")]
  Configure = 50,

  /// <summary>
  /// 安装
  /// </summary>
  [Description("安装")]
  Install = 51,

  /// <summary>
  /// 卸载
  /// </summary>
  [Description("卸载")]
  Uninstall = 52,

  /// <summary>
  /// 升级
  /// </summary>
  [Description("升级")]
  Upgrade = 53,

  /// <summary>
  /// 降级
  /// </summary>
  [Description("降级")]
  Downgrade = 54,

  /// <summary>
  /// 启动
  /// </summary>
  [Description("启动")]
  Start = 55,

  /// <summary>
  /// 停止
  /// </summary>
  [Description("停止")]
  Stop = 56,

  /// <summary>
  /// 重启
  /// </summary>
  [Description("重启")]
  Restart = 57,

  /// <summary>
  /// 暂停
  /// </summary>
  [Description("暂停")]
  Pause = 58,

  /// <summary>
  /// 恢复
  /// </summary>
  [Description("恢复")]
  Resume = 59,

  /// <summary>
  /// 发送
  /// </summary>
  [Description("发送")]
  Send = 60,

  /// <summary>
  /// 接收
  /// </summary>
  [Description("接收")]
  Receive = 61,

  /// <summary>
  /// 转发
  /// </summary>
  [Description("转发")]
  Forward = 62,

  /// <summary>
  /// 回复
  /// </summary>
  [Description("回复")]
  Reply = 63,

  /// <summary>
  /// 登录
  /// </summary>
  [Description("登录")]
  Login = 64,

  /// <summary>
  /// 注销
  /// </summary>
  [Description("注销")]
  Logout = 65,

  /// <summary>
  /// 注册
  /// </summary>
  [Description("注册")]
  Register = 66,

  /// <summary>
  /// 注销账号
  /// </summary>
  [Description("注销账号")]
  Unregister = 67,

  /// <summary>
  /// 生成
  /// </summary>
  [Description("生成")]
  Generate = 68,

  /// <summary>
  /// 执行
  /// </summary>
  [Description("执行")]
  Execute = 69,

  /// <summary>
  /// 测试
  /// </summary>
  [Description("测试")]
  Test = 70
}