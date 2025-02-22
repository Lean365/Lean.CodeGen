//===================================================
// 项目名: Lean.CodeGen.Common
// 文件名: LeanDiffType.cs
// 功能描述: 差异类型枚举
// 创建时间: 2024-03-26
// 作者: Lean
// 版本: 1.0
//===================================================

namespace Lean.CodeGen.Common.Enums;

/// <summary>
/// 差异类型枚举
/// </summary>
public enum LeanDiffType
{
  /// <summary>
  /// 新增
  /// </summary>
  Added = 0,

  /// <summary>
  /// 修改
  /// </summary>
  Modified = 1,

  /// <summary>
  /// 删除
  /// </summary>
  Deleted = 2
}