using System;

namespace Lean.CodeGen.Common.Options;

/// <summary>
/// 缓存配置选项
/// </summary>
public class LeanCacheOptions
{
  /// <summary>
  /// 配置节点名称
  /// </summary>
  public const string Position = "Cache";

  /// <summary>
  /// 默认过期时间
  /// </summary>
  public TimeSpan? DefaultExpiration { get; set; }

  /// <summary>
  /// 是否启用Redis
  /// </summary>
  public bool EnableRedis { get; set; }

  /// <summary>
  /// 缓存键前缀
  /// </summary>
  public string KeyPrefix { get; set; } = "lean:";

  /// <summary>
  /// 是否启用统计
  /// </summary>
  public bool EnableStats { get; set; }

  /// <summary>
  /// 是否启用压缩
  /// </summary>
  public bool EnableCompression { get; set; }

  /// <summary>
  /// 压缩阈值（字节）
  /// </summary>
  public int CompressionThreshold { get; set; } = 1024;

  /// <summary>
  /// Redis配置
  /// </summary>
  public RedisOptions Redis { get; set; } = new();

  /// <summary>
  /// 内存缓存配置
  /// </summary>
  public MemoryOptions Memory { get; set; } = new();
}

/// <summary>
/// Redis配置选项
/// </summary>
public class RedisOptions
{
  /// <summary>
  /// 连接字符串
  /// </summary>
  public string? ConnectionString { get; set; }

  /// <summary>
  /// 数据库索引
  /// </summary>
  public int Database { get; set; }

  /// <summary>
  /// 连接池大小
  /// </summary>
  public int PoolSize { get; set; } = 50;

  /// <summary>
  /// 连接超时时间（毫秒）
  /// </summary>
  public int ConnectionTimeout { get; set; } = 5000;

  /// <summary>
  /// 操作超时时间（毫秒）
  /// </summary>
  public int OperationTimeout { get; set; } = 5000;

  /// <summary>
  /// 重试次数
  /// </summary>
  public int RetryCount { get; set; } = 3;

  /// <summary>
  /// 重试间隔（毫秒）
  /// </summary>
  public int RetryInterval { get; set; } = 1000;

  /// <summary>
  /// 是否启用SSL
  /// </summary>
  public bool EnableSsl { get; set; }

  /// <summary>
  /// 密码
  /// </summary>
  public string? Password { get; set; }

  /// <summary>
  /// 是否允许管理员操作
  /// </summary>
  public bool AllowAdmin { get; set; }

  /// <summary>
  /// 是否启用集群
  /// </summary>
  public bool EnableCluster { get; set; }
}

/// <summary>
/// 内存缓存配置选项
/// </summary>
public class MemoryOptions
{
  /// <summary>
  /// 缓存大小限制（MB）
  /// </summary>
  public int SizeLimit { get; set; } = 1024;

  /// <summary>
  /// 压缩比例
  /// </summary>
  public double CompactionPercentage { get; set; } = 0.05;

  /// <summary>
  /// 过期扫描频率（秒）
  /// </summary>
  public int ExpirationScanFrequency { get; set; } = 60;

  /// <summary>
  /// 是否启用滑动过期
  /// </summary>
  public bool EnableSlidingExpiration { get; set; }

  /// <summary>
  /// 滑动过期时间
  /// </summary>
  public TimeSpan? SlidingExpiration { get; set; }
}