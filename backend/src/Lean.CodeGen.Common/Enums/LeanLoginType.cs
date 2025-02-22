//===================================================
// 项目名: Lean.CodeGen.Common
// 文件名: LeanLoginType.cs
// 功能描述: 登录方式枚举
// 创建时间: 2024-03-26
// 作者: Lean
// 版本: 1.0
//===================================================

namespace Lean.CodeGen.Common.Enums;

/// <summary>
/// 登录方式枚举
/// </summary>
public enum LeanLoginType
{
  /// <summary>
  /// 密码登录
  /// </summary>
  Password = 0,

  /// <summary>
  /// 验证码登录
  /// </summary>
  Code = 1,

  /// <summary>
  /// 令牌登录
  /// </summary>
  Token = 2,

  /// <summary>
  /// 其他方式
  /// </summary>
  Other = 3
}