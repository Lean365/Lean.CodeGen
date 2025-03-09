namespace Lean.CodeGen.Common.Enums;

/// <summary>
/// 业务操作类型
/// </summary>
public enum LeanBusinessType
{
  /// <summary>
  /// 其他
  /// </summary>
  Other = 0,

  /// <summary>
  /// 新增
  /// </summary>
  Create = 1,

  /// <summary>
  /// 修改
  /// </summary>
  Update = 2,

  /// <summary>
  /// 删除
  /// </summary>
  Delete = 3,

  /// <summary>
  /// 授权
  /// </summary>
  Grant = 4,

  /// <summary>
  /// 导出
  /// </summary>
  Export = 5,

  /// <summary>
  /// 导入
  /// </summary>
  Import = 6,

  /// <summary>
  /// 强退
  /// </summary>
  ForceLogout = 7,

  /// <summary>
  /// 生成代码
  /// </summary>
  GenCode = 8,

  /// <summary>
  /// 清空数据
  /// </summary>
  Clean = 9,

  /// <summary>
  /// 查询
  /// </summary>
  Query = 10
}