//===================================================
// 项目名: Lean.CodeGen.Common
// 文件名: LeanOperationType.cs
// 功能描述: 操作类型枚举
// 创建时间: 2024-03-26
// 作者: Lean
// 版本: 1.0
//===================================================

namespace Lean.CodeGen.Common.Enums;

/// <summary>
/// 操作类型枚举
/// </summary>
public enum LeanOperationType
{
  /// <summary>
  /// 查看
  /// </summary>
  View = 1,

  /// <summary>
  /// 新增
  /// </summary>
  Create = 2,

  /// <summary>
  /// 修改
  /// </summary>
  Update = 3,

  /// <summary>
  /// 删除
  /// </summary>
  Delete = 4,

  /// <summary>
  /// 导入
  /// </summary>
  Import = 5,

  /// <summary>
  /// 导出
  /// </summary>
  Export = 6
}
