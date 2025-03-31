using System;
using System.Text;
using System.Security.Cryptography;
using Lean.CodeGen.Common.Enums;

namespace Lean.CodeGen.Common.Helpers;

/// <summary>
/// 设备帮助类
/// </summary>
public static class LeanDeviceHelper
{
  /// <summary>
  /// 生成连接ID
  /// </summary>
  /// <param name="deviceFingerprint">设备指纹</param>
  /// <param name="userAgent">用户代理</param>
  /// <param name="deviceType">设备类型</param>
  /// <returns>32位连接ID</returns>
  public static string GenerateConnectionId(string deviceFingerprint, string userAgent, string deviceType)
  {
    if (string.IsNullOrEmpty(deviceFingerprint))
      throw new ArgumentNullException(nameof(deviceFingerprint), "设备指纹不能为空");
    if (string.IsNullOrEmpty(userAgent))
      throw new ArgumentNullException(nameof(userAgent), "用户代理不能为空");
    if (string.IsNullOrEmpty(deviceType))
      throw new ArgumentNullException(nameof(deviceType), "设备类型不能为空");

    var input = $"{deviceFingerprint}|{userAgent}|{deviceType}|{DateTime.UtcNow.Ticks}";
    using var md5 = MD5.Create();
    var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
    return BitConverter.ToString(hash).Replace("-", "").ToLower();
  }

  /// <summary>
  /// 生成设备ID
  /// </summary>
  /// <param name="deviceFingerprint">设备指纹</param>
  /// <param name="userAgent">用户代理</param>
  /// <param name="deviceType">设备类型</param>
  /// <returns>32位设备ID</returns>
  public static string GenerateDeviceId(string deviceFingerprint, string userAgent, string deviceType)
  {
    if (string.IsNullOrEmpty(deviceFingerprint))
      throw new ArgumentNullException(nameof(deviceFingerprint), "设备指纹不能为空");
    if (string.IsNullOrEmpty(userAgent))
      throw new ArgumentNullException(nameof(userAgent), "用户代理不能为空");
    if (string.IsNullOrEmpty(deviceType))
      throw new ArgumentNullException(nameof(deviceType), "设备类型不能为空");

    var input = $"{deviceFingerprint}|{userAgent}|{deviceType}";
    using var md5 = MD5.Create();
    var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
    return BitConverter.ToString(hash).Replace("-", "").ToLower();
  }
}