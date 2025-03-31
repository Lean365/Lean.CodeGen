using System;
using System.Text;
using System.Security.Cryptography;

namespace Lean.CodeGen.Common.Helpers;

/// <summary>
/// 哈希帮助类
/// </summary>
public static class LeanHashHelper
{
  /// <summary>
  /// 生成MD5哈希
  /// </summary>
  /// <param name="input">输入字符串</param>
  /// <returns>MD5哈希值</returns>
  public static string GenerateMd5(string input)
  {
    if (string.IsNullOrEmpty(input))
      throw new ArgumentNullException(nameof(input), "输入字符串不能为空");

    using (var md5 = MD5.Create())
    {
      var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
      return BitConverter.ToString(hash).Replace("-", "").ToLower();
    }
  }

  /// <summary>
  /// 生成SHA256哈希
  /// </summary>
  /// <param name="input">输入字符串</param>
  /// <returns>SHA256哈希值</returns>
  public static string GenerateSha256(string input)
  {
    if (string.IsNullOrEmpty(input))
      throw new ArgumentNullException(nameof(input), "输入字符串不能为空");

    using (var sha256 = SHA256.Create())
    {
      var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
      return BitConverter.ToString(hash).Replace("-", "").ToLower();
    }
  }

  /// <summary>
  /// 生成SHA512哈希
  /// </summary>
  /// <param name="input">输入字符串</param>
  /// <returns>SHA512哈希值</returns>
  public static string GenerateSha512(string input)
  {
    if (string.IsNullOrEmpty(input))
      throw new ArgumentNullException(nameof(input), "输入字符串不能为空");

    using (var sha512 = SHA512.Create())
    {
      var hash = sha512.ComputeHash(Encoding.UTF8.GetBytes(input));
      return BitConverter.ToString(hash).Replace("-", "").ToLower();
    }
  }
}