using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using Lean.CodeGen.Domain.Interfaces.Caching;
using Lean.CodeGen.Common.Options;

namespace Lean.CodeGen.Infrastructure.Services.Caching;

/// <summary>
/// 基于Redis的缓存服务实现
/// </summary>
public class LeanRedisCacheService : ILeanCacheService
{
  private readonly IConnectionMultiplexer _redis;
  private readonly IDatabase _db;
  private readonly LeanCacheOptions _options;

  public LeanRedisCacheService(
      IConnectionMultiplexer redis,
      IOptions<LeanCacheOptions> options)
  {
    _redis = redis;
    _db = redis.GetDatabase();
    _options = options.Value;
  }

  public async Task<T?> GetAsync<T>(string key)
  {
    var value = await _db.StringGetAsync(key);
    if (!value.HasValue)
    {
      return default;
    }
    return JsonSerializer.Deserialize<T>(value!);
  }

  public Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
  {
    var jsonValue = JsonSerializer.Serialize(value);
    return _db.StringSetAsync(
        key,
        jsonValue,
        expiration ?? _options.DefaultExpiration);
  }

  public Task RemoveAsync(string key)
  {
    return _db.KeyDeleteAsync(key);
  }

  public Task<bool> ExistsAsync(string key)
  {
    return _db.KeyExistsAsync(key);
  }

  public async Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiration = null)
  {
    var value = await GetAsync<T>(key);
    if (value != null)
    {
      return value;
    }

    var newValue = await factory();
    await SetAsync(key, newValue, expiration);
    return newValue;
  }

  public async Task ClearAsync()
  {
    var endpoints = _redis.GetEndPoints();
    foreach (var endpoint in endpoints)
    {
      var server = _redis.GetServer(endpoint);
      await server.FlushDatabaseAsync();
    }
  }
}