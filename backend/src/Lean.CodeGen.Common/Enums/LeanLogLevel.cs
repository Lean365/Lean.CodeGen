//===================================================
// 项目名: Lean.CodeGen.Common
// 文件名: LeanLogLevel.cs
// 功能描述: 日志级别枚举
// 创建时间: 2024-03-26
// 作者: Lean
// 版本: 1.0
//===================================================

namespace Lean.CodeGen.Common.Enums;

/// <summary>
/// 日志级别枚举
/// </summary>
public enum LeanLogLevel
{
  /// <summary>
  /// 调试
  /// </summary>
  Debug = 0,

  /// <summary>
  /// 信息
  /// </summary>
  Information = 1,

  /// <summary>
  /// 警告
  /// </summary>
  Warning = 2,

  /// <summary>
  /// 错误
  /// </summary>
  Error = 3,

  /// <summary>
  /// 致命错误
  /// </summary>
  Fatal = 4
}