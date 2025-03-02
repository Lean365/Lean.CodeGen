namespace Lean.CodeGen.Common.Options;

/// <summary>
/// IP 配置选项
/// </summary>
public class LeanIpOptions
{
  /// <summary>
  /// IP2Region 数据库文件路径
  /// </summary>
  public string DbPath { get; set; } = "ip2region.xdb";

  /// <summary>
  /// 是否启用缓存
  /// </summary>
  public bool EnableCache { get; set; } = true;

  /// <summary>
  /// 缓存过期时间（分钟）
  /// </summary>
  public int CacheExpiration { get; set; } = 60;
}