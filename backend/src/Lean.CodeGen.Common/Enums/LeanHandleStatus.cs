//===================================================
// 项目名: Lean.CodeGen.Common
// 文件名: LeanHandleStatus.cs
// 功能描述: 处理状态枚举
// 创建时间: 2024-03-26
// 作者: Lean
// 版本: 1.0
//===================================================

namespace Lean.CodeGen.Common.Enums;

/// <summary>
/// 处理状态枚举
/// </summary>
public enum LeanHandleStatus
{
  /// <summary>
  /// 待处理
  /// </summary>
  Pending = 0,

  /// <summary>
  /// 处理中
  /// </summary>
  Processing = 1,

  /// <summary>
  /// 已处理
  /// </summary>
  Handled = 2,

  /// <summary>
  /// 已忽略
  /// </summary>
  Ignored = 3
}