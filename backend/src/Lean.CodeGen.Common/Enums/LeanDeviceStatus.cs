//===================================================
// 项目名: Lean.CodeGen.Common
// 文件名: LeanDeviceStatus.cs
// 功能描述: 设备状态枚举
// 创建时间: 2024-03-26
// 作者: Lean
// 版本: 1.0
//===================================================

namespace Lean.CodeGen.Common.Enums;

/// <summary>
/// 设备状态枚举
/// </summary>
public enum LeanDeviceStatus
{
  /// <summary>
  /// 正常
  /// </summary>
  Normal = 0,

  /// <summary>
  /// 禁用
  /// </summary>
  Disabled = 1,

  /// <summary>
  /// 锁定
  /// </summary>
  Locked = 2
}