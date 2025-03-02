using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;

namespace Lean.CodeGen.Common.Helpers;

/// <summary>
/// 客户端系统信息帮助类
/// </summary>
public class LeanClientInfoHelper
{
  private readonly IHttpContextAccessor _httpContextAccessor;

  public LeanClientInfoHelper(IHttpContextAccessor httpContextAccessor)
  {
    _httpContextAccessor = httpContextAccessor;
  }

  /// <summary>
  /// 获取客户端系统信息
  /// </summary>
  public ClientSystemInfo GetSystemInfo()
  {
    var context = _httpContextAccessor.HttpContext;
    var userAgent = context?.Request.Headers["User-Agent"].ToString() ?? "";
    var ipAddress = context?.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1";

    return new ClientSystemInfo
    {
      IpAddress = ipAddress,
      UserAgent = userAgent,
      BrowserInfo = GetBrowserInfo(userAgent)
    };
  }

  /// <summary>
  /// 获取浏览器信息
  /// </summary>
  private BrowserInfo GetBrowserInfo(string userAgent)
  {
    return new BrowserInfo
    {
      UserAgent = userAgent,
      Browser = GetBrowserName(userAgent),
      Version = GetBrowserVersion(userAgent),
      Platform = GetPlatform(userAgent),
      IsMobile = IsMobileDevice(userAgent)
    };
  }

  /// <summary>
  /// 获取浏览器名称
  /// </summary>
  private string GetBrowserName(string userAgent)
  {
    if (string.IsNullOrEmpty(userAgent)) return "Unknown";

    if (userAgent.Contains("Edge"))
      return "Microsoft Edge";
    if (userAgent.Contains("Chrome"))
      return "Google Chrome";
    if (userAgent.Contains("Firefox"))
      return "Mozilla Firefox";
    if (userAgent.Contains("Safari") && !userAgent.Contains("Chrome"))
      return "Apple Safari";
    if (userAgent.Contains("MSIE") || userAgent.Contains("Trident"))
      return "Internet Explorer";

    return "Other";
  }

  /// <summary>
  /// 获取浏览器版本
  /// </summary>
  private string GetBrowserVersion(string userAgent)
  {
    try
    {
      if (string.IsNullOrEmpty(userAgent)) return "Unknown";

      var match = Regex.Match(userAgent, @"(Chrome|Firefox|Safari|Edge|MSIE|rv(?=:))[\/\s](\d+\.\d+)");
      return match.Success ? match.Groups[2].Value : "Unknown";
    }
    catch
    {
      return "Unknown";
    }
  }

  /// <summary>
  /// 获取平台信息
  /// </summary>
  private string GetPlatform(string userAgent)
  {
    if (string.IsNullOrEmpty(userAgent)) return "Unknown";

    if (userAgent.Contains("Windows"))
      return "Windows";
    if (userAgent.Contains("Macintosh") || userAgent.Contains("Mac OS"))
      return "macOS";
    if (userAgent.Contains("Linux") && !userAgent.Contains("Android"))
      return "Linux";
    if (userAgent.Contains("Android"))
      return "Android";
    if (userAgent.Contains("iPhone") || userAgent.Contains("iPad") || userAgent.Contains("iPod"))
      return "iOS";

    return "Other";
  }

  /// <summary>
  /// 判断是否为移动设备
  /// </summary>
  private bool IsMobileDevice(string userAgent)
  {
    if (string.IsNullOrEmpty(userAgent)) return false;

    return Regex.IsMatch(userAgent, @"(Android|iPhone|iPad|iPod|Windows Phone|webOS|BlackBerry|Mobile)", RegexOptions.IgnoreCase);
  }
}

/// <summary>
/// 客户端系统信息
/// </summary>
public class ClientSystemInfo
{
  /// <summary>
  /// IP地址
  /// </summary>
  public string IpAddress { get; set; } = "";

  /// <summary>
  /// 用户代理字符串
  /// </summary>
  public string UserAgent { get; set; } = "";

  /// <summary>
  /// 浏览器信息
  /// </summary>
  public BrowserInfo BrowserInfo { get; set; } = new();

  public override string ToString()
  {
    return JsonConvert.SerializeObject(this, Formatting.Indented);
  }
}

/// <summary>
/// 浏览器信息
/// </summary>
public class BrowserInfo
{
  /// <summary>
  /// 用户代理字符串
  /// </summary>
  public string UserAgent { get; set; } = "";

  /// <summary>
  /// 浏览器名称
  /// </summary>
  public string Browser { get; set; } = "";

  /// <summary>
  /// 浏览器版本
  /// </summary>
  public string Version { get; set; } = "";

  /// <summary>
  /// 平台
  /// </summary>
  public string Platform { get; set; } = "";

  /// <summary>
  /// 是否移动设备
  /// </summary>
  public bool IsMobile { get; set; }
}