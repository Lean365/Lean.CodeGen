using System.ComponentModel;

namespace Lean.CodeGen.Common.Enums;

/// <summary>
/// 业务操作类型
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
  /// 创建
  /// </summary>
  [Description("创建")]
  Create = 2,

  /// <summary>
  /// 更新
  /// </summary>
  [Description("更新")]
  Update = 3,

  /// <summary>
  /// 删除
  /// </summary>
  [Description("删除")]
  Delete = 4,

  /// <summary>
  /// 授权
  /// </summary>
  [Description("授权")]
  Grant = 5,

  /// <summary>
  /// 导出
  /// </summary>
  [Description("导出")]
  Export = 6,

  /// <summary>
  /// 导入
  /// </summary>
  [Description("导入")]
  Import = 7,

  /// <summary>
  /// 登出
  /// </summary>
  [Description("登出")]
  Logout = 8,

  /// <summary>
  /// 生成代码
  /// </summary>
  [Description("生成代码")]
  Generate = 9,

  /// <summary>
  /// 清空
  /// </summary>
  [Description("清空")]
  Clean = 10,

  /// <summary>
  /// 上传
  /// </summary>
  [Description("上传")]
  Upload = 11,

  /// <summary>
  /// 下载
  /// </summary>
  [Description("下载")]
  Download = 12,

  /// <summary>
  /// 审核
  /// </summary>
  [Description("审核")]
  Audit = 13,

  /// <summary>
  /// 同步
  /// </summary>
  [Description("同步")]
  Sync = 14,

  /// <summary>
  /// 备份
  /// </summary>
  [Description("备份")]
  Backup = 15,

  /// <summary>
  /// 恢复
  /// </summary>
  [Description("恢复")]
  Restore = 16,

  /// <summary>
  /// 重置
  /// </summary>
  [Description("重置")]
  Reset = 17,

  /// <summary>
  /// 启用
  /// </summary>
  [Description("启用")]
  Enable = 18,

  /// <summary>
  /// 禁用
  /// </summary>
  [Description("禁用")]
  Disable = 19,

  /// <summary>
  /// 配置
  /// </summary>
  [Description("配置")]
  Configure = 20
}