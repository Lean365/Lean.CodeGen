using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using NLog;
using IP2Region.Net.XDB;
using Lean.CodeGen.Common.Options;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;

namespace Lean.CodeGen.Common.Helpers;

/// <summary>
/// IP 帮助类
/// </summary>
public class LeanIpHelper
{
  private readonly ILogger<LeanIpHelper> _logger;
  private readonly Searcher _searcher;
  private readonly LeanIpOptions _options;
  private readonly IWebHostEnvironment _env;

  /// <summary>
  /// 构造函数
  /// </summary>
  public LeanIpHelper(
      ILogger<LeanIpHelper> logger,
      IOptions<LeanIpOptions> options,
      IWebHostEnvironment env)
  {
    _logger = logger;
    _options = options.Value;
    _env = env;

    try
    {
      var dbPath = Path.Combine(_env.WebRootPath, "data", _options.DbPath);
      _searcher = new Searcher(CachePolicy.Content, dbPath);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "初始化 IP2Region 搜索器失败");
      throw;
    }
  }

  /// <summary>
  /// 获取 IP 地址信息
  /// </summary>
  /// <param name="ip">IP 地址</param>
  /// <returns>地理位置信息</returns>
  public async Task<string> GetIpLocationAsync(string? ip)
  {
    return await Task.FromResult(GetIpLocation(ip));
  }

  /// <summary>
  /// 获取 IP 地址信息（同步版本）
  /// </summary>
  /// <param name="ip">IP 地址</param>
  /// <returns>地理位置信息</returns>
  public string GetIpLocation(string? ip)
  {
    if (string.IsNullOrEmpty(ip))
    {
      return "未知位置";
    }

    if (IsPrivateIp(ip))
    {
      return "内网IP";
    }

    try
    {
      // 搜索 IP 地址信息
      var region = _searcher.Search(ip!);
      if (string.IsNullOrEmpty(region))
      {
        return "未知位置";
      }

      // 解析地理位置信息
      var locations = region.Split('|', StringSplitOptions.RemoveEmptyEntries);
      var sb = new StringBuilder();

      foreach (var location in locations)
      {
        if (location == "0")
        {
          continue;
        }
        if (sb.Length > 0)
        {
          sb.Append(" ");
        }
        sb.Append(location);
      }

      return sb.Length > 0 ? sb.ToString() : "未知位置";
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "获取IP地址 {Ip} 的地理位置信息失败", ip);
      return "未知位置";
    }
  }

  /// <summary>
  /// 判断是否为内网 IP
  /// </summary>
  private bool IsPrivateIp(string? ipAddress)
  {
    if (string.IsNullOrEmpty(ipAddress)) return false;

    if (IPAddress.TryParse(ipAddress, out IPAddress ip))
    {
      var bytes = ip.GetAddressBytes();
      return IsPrivateIpAddress(bytes);
    }
    return false;
  }

  /// <summary>
  /// 判断是否为内网 IP 地址
  /// </summary>
  private bool IsPrivateIpAddress(byte[] ipBytes)
  {
    // 10.0.0.0 - 10.255.255.255
    if (ipBytes[0] == 10)
    {
      return true;
    }

    // 172.16.0.0 - 172.31.255.255
    if (ipBytes[0] == 172 && ipBytes[1] >= 16 && ipBytes[1] <= 31)
    {
      return true;
    }

    // 192.168.0.0 - 192.168.255.255
    if (ipBytes[0] == 192 && ipBytes[1] == 168)
    {
      return true;
    }

    return false;
  }
}