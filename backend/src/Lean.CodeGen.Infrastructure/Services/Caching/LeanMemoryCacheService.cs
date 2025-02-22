using System;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Lean.CodeGen.Domain.Interfaces.Caching;
using Lean.CodeGen.Common.Options;

namespace Lean.CodeGen.Infrastructure.Services.Caching;

/// <summary>
/// 基于内存的缓存服务实现
/// </summary>
public class LeanMemoryCacheService : ILeanCacheService
{
  private readonly IMemoryCache _cache;
  private readonly LeanCacheOptions _options;
  private readonly ILogger<LeanMemoryCacheService> _logger;
  private readonly ConcurrentDictionary<string, long> _keySizes;
  private long _currentSize;

  public LeanMemoryCacheService(
      IMemoryCache cache,
      IOptions<LeanCacheOptions> options,
      ILogger<LeanMemoryCacheService> logger)
  {
    _cache = cache;
    _options = options.Value;
    _logger = logger;
    _keySizes = new ConcurrentDictionary<string, long>();
    _currentSize = 0;
  }

  public Task<T?> GetAsync<T>(string key)
  {
    var cacheKey = GetCacheKey(key);
    var value = _cache.Get<T>(cacheKey);

    if (value != null && _options.Memory.EnableSlidingExpiration)
    {
      // 更新滑动过期时间
      var options = new MemoryCacheEntryOptions();
      if (_options.Memory.SlidingExpiration.HasValue)
      {
        options.SlidingExpiration = _options.Memory.SlidingExpiration.Value;
      }
      _cache.Set(cacheKey, value, options);
    }

    return Task.FromResult(value);
  }

  public Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
  {
    var cacheKey = GetCacheKey(key);
    var options = new MemoryCacheEntryOptions();

    // 设置过期时间
    if (expiration.HasValue)
    {
      options.AbsoluteExpirationRelativeToNow = expiration.Value;
    }
    else if (_options.DefaultExpiration.HasValue)
    {
      options.AbsoluteExpirationRelativeToNow = _options.DefaultExpiration.Value;
    }

    // 设置滑动过期
    if (_options.Memory.EnableSlidingExpiration && _options.Memory.SlidingExpiration.HasValue)
    {
      options.SlidingExpiration = _options.Memory.SlidingExpiration.Value;
    }

    // 计算缓存项大小
    var size = CalculateSize(value);

    // 检查是否需要进行缓存清理
    if (NeedEviction(size))
    {
      EvictItems();
    }

    // 注册回调以跟踪缓存项大小
    options.RegisterPostEvictionCallback(OnPostEviction);
    options.SetSize(size);

    _cache.Set(cacheKey, value, options);
    _keySizes.AddOrUpdate(cacheKey, size, (_, _) => size);
    Interlocked.Add(ref _currentSize, size);

    return Task.CompletedTask;
  }

  public Task RemoveAsync(string key)
  {
    var cacheKey = GetCacheKey(key);
    _cache.Remove(cacheKey);
    return Task.CompletedTask;
  }

  public Task<bool> ExistsAsync(string key)
  {
    var cacheKey = GetCacheKey(key);
    return Task.FromResult(_cache.TryGetValue(cacheKey, out _));
  }

  public async Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiration = null)
  {
    var cacheKey = GetCacheKey(key);
    if (await ExistsAsync(key))
    {
      var value = await GetAsync<T>(key);
      if (value != null)
      {
        return value;
      }
    }

    var newValue = await factory();
    await SetAsync(key, newValue, expiration);
    return newValue;
  }

  public Task ClearAsync()
  {
    if (_cache is MemoryCache memoryCache)
    {
      memoryCache.Compact(1.0);
      _keySizes.Clear();
      _currentSize = 0;
    }
    return Task.CompletedTask;
  }

  private string GetCacheKey(string key)
  {
    return $"{_options.KeyPrefix}{key}";
  }

  private long CalculateSize<T>(T value)
  {
    if (value == null) return 0;

    // 简单估算对象大小
    var size = 0L;
    var type = value.GetType();

    if (type.IsPrimitive)
    {
      size = System.Runtime.InteropServices.Marshal.SizeOf(type);
    }
    else if (value is string str)
    {
      size = str.Length * 2; // UTF-16 编码，每个字符2字节
    }
    else
    {
      // 对于复杂对象，使用序列化后的大小作为估算
      var json = System.Text.Json.JsonSerializer.Serialize(value);
      size = json.Length * 2;
    }

    return size;
  }

  private bool NeedEviction(long newItemSize)
  {
    var sizeLimit = _options.Memory.SizeLimit * 1024 * 1024; // 转换为字节
    return (_currentSize + newItemSize) > sizeLimit;
  }

  private void EvictItems()
  {
    if (_cache is MemoryCache memoryCache)
    {
      var compactionPercentage = _options.Memory.CompactionPercentage;
      _logger.LogInformation($"触发缓存清理，当前大小: {_currentSize / (1024 * 1024)}MB, 压缩比例: {compactionPercentage}");
      memoryCache.Compact(compactionPercentage);
    }
  }

  private void OnPostEviction(object key, object? value, EvictionReason reason, object? state)
  {
    if (key is string cacheKey && _keySizes.TryRemove(cacheKey, out var size))
    {
      Interlocked.Add(ref _currentSize, -size);
      _logger.LogDebug($"缓存项被移除: {cacheKey}, 原因: {reason}, 大小: {size / 1024}KB");
    }
  }
}