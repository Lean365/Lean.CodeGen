using System;
using System.Threading.Tasks;

namespace Lean.CodeGen.Domain.Interfaces.Caching;

/// <summary>
/// 缓存服务接口
/// </summary>
public interface ILeanCacheService
{
  /// <summary>
  /// 获取缓存
  /// </summary>
  Task<T?> GetAsync<T>(string key);

  /// <summary>
  /// 设置缓存
  /// </summary>
  Task SetAsync<T>(string key, T value, TimeSpan? expiration = null);

  /// <summary>
  /// 移除缓存
  /// </summary>
  Task RemoveAsync(string key);

  /// <summary>
  /// 判断缓存是否存在
  /// </summary>
  Task<bool> ExistsAsync(string key);

  /// <summary>
  /// 获取缓存，如果不存在则通过工厂方法创建并缓存
  /// </summary>
  Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiration = null);

  /// <summary>
  /// 清空所有缓存
  /// </summary>
  Task ClearAsync();
}