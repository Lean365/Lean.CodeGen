using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using System.Net;

namespace Lean.CodeGen.WebApi.Filters;

/// <summary>
/// 接口限流特性
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class LeanRateLimitAttribute : Attribute
{
  /// <summary>
  /// 时间窗口（秒）
  /// </summary>
  public int Seconds { get; }

  /// <summary>
  /// 最大请求次数
  /// </summary>
  public int MaxRequests { get; }

  public LeanRateLimitAttribute(int seconds = 1, int maxRequests = 1)
  {
    Seconds = seconds;
    MaxRequests = maxRequests;
  }
}

/// <summary>
/// 接口限流过滤器
/// </summary>
public class LeanRateLimitFilter : IAsyncActionFilter
{
  private readonly IMemoryCache _cache;
  private readonly ILogger<LeanRateLimitFilter> _logger;

  public LeanRateLimitFilter(IMemoryCache cache, ILogger<LeanRateLimitFilter> logger)
  {
    _cache = cache;
    _logger = logger;
  }

  public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
  {
    var attribute = context.ActionDescriptor.EndpointMetadata
        .OfType<LeanRateLimitAttribute>()
        .FirstOrDefault();

    if (attribute == null)
    {
      await next();
      return;
    }

    var key = GenerateKey(context);
    var requestCount = await GetRequestCount(key);

    if (requestCount >= attribute.MaxRequests)
    {
      _logger.LogWarning("IP: {IP} 请求过于频繁", context.HttpContext.Connection.RemoteIpAddress);
      context.Result = new StatusCodeResult((int)HttpStatusCode.TooManyRequests);
      return;
    }

    await IncrementRequestCount(key, attribute.Seconds);
    await next();
  }

  private static string GenerateKey(ActionExecutingContext context)
  {
    var ip = context.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
    var path = context.HttpContext.Request.Path.Value?.ToLower() ?? "";
    return $"rate_limit:{ip}:{path}";
  }

  private async Task<int> GetRequestCount(string key)
  {
    return await Task.FromResult(_cache.GetOrCreate(key, entry => 0));
  }

  private async Task IncrementRequestCount(string key, int seconds)
  {
    await Task.FromResult(_cache.GetOrCreate(key, entry =>
    {
      entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(seconds);
      return 1;
    }));
  }
}