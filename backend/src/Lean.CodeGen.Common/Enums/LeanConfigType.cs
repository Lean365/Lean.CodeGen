//===================================================
// 项目名: Lean.CodeGen.Common
// 文件名: LeanConfigType.cs
// 功能描述: 配置类型枚举
// 创建时间: 2024-03-26
// 作者: Lean
// 版本: 1.0
//===================================================

namespace Lean.CodeGen.Common.Enums;

/// <summary>
/// 配置类型枚举
/// </summary>
public enum LeanConfigType
{
  /// <summary>
  /// 字符串
  /// </summary>
  String = 0,

  /// <summary>
  /// 数值
  /// </summary>
  Number = 1,

  /// <summary>
  /// 布尔值
  /// </summary>
  Boolean = 2,

  /// <summary>
  /// JSON对象
  /// </summary>
  Json = 3,

  /// <summary>
  /// 其他
  /// </summary>
  Other = 4
}