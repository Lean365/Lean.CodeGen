//===================================================
// 项目名: Lean.CodeGen.Common
// 文件名: LeanAuditOperationType.cs
// 功能描述: 审计操作类型枚举
// 创建时间: 2024-03-26
// 作者: Lean
// 版本: 1.0
//===================================================

namespace Lean.CodeGen.Common.Enums;

/// <summary>
/// 审计操作类型枚举
/// </summary>
public enum LeanAuditOperationType
{
  /// <summary>
  /// 创建
  /// </summary>
  Create = 0,

  /// <summary>
  /// 更新
  /// </summary>
  Update = 1,

  /// <summary>
  /// 删除
  /// </summary>
  Delete = 2,

  /// <summary>
  /// 其他
  /// </summary>
  Other = 3
}