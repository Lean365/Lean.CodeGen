using System.ComponentModel;

namespace Lean.CodeGen.Common.Enums;

/// <summary>
/// 主要状态枚举
/// </summary>
/// <remarks>
/// 用于表示是否为主要项：0-否，1-是
/// </remarks>
public enum LeanPrimaryStatus
{
  /// <summary>
  /// 否
  /// </summary>
  [Description("否")]
  No = 0,

  /// <summary>
  /// 是
  /// </summary>
  [Description("是")]
  Yes = 1
}