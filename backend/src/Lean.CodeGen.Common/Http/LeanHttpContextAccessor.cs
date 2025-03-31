using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace Lean.CodeGen.Common.Http;

/// <summary>
/// HTTP 上下文访问器接口
/// </summary>
public interface ILeanHttpContextAccessor
{
  /// <summary>
  /// 获取当前语言
  /// </summary>
  /// <returns>当前语言代码</returns>
  string GetCurrentLanguage();

  /// <summary>
  /// 获取客户端IP
  /// </summary>
  string GetClientIp();

  /// <summary>
  /// 获取用户代理
  /// </summary>
  string GetUserAgent();

  /// <summary>
  /// 获取请求路径
  /// </summary>
  string GetRequestPath();

  /// <summary>
  /// 获取请求方法
  /// </summary>
  string GetRequestMethod();

  /// <summary>
  /// 获取请求URL
  /// </summary>
  string GetRequestUrl();

  /// <summary>
  /// 获取请求来源
  /// </summary>
  string GetReferer();

  /// <summary>
  /// 获取Web根目录路径
  /// </summary>
  /// <returns>Web根目录的完整路径</returns>
  /// <exception cref="InvalidOperationException">当Web根目录不存在或无法访问时抛出</exception>
  string GetWebRootPath();

  /// <summary>
  /// 获取HTTP上下文
  /// </summary>
  HttpContext? HttpContext { get; }

  /// <summary>
  /// 获取Web环境
  /// </summary>
  IWebHostEnvironment WebHostEnvironment { get; }
}

/// <summary>
/// HTTP 上下文访问器实现
/// </summary>
public class LeanHttpContextAccessor : ILeanHttpContextAccessor
{
  private readonly IHttpContextAccessor _httpContextAccessor;
  private readonly IWebHostEnvironment _env;

  public LeanHttpContextAccessor(IHttpContextAccessor httpContextAccessor, IWebHostEnvironment env)
  {
    _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    _env = env ?? throw new ArgumentNullException(nameof(env));
  }

  /// <summary>
  /// 获取当前语言
  /// </summary>
  public string GetCurrentLanguage()
  {
    return _httpContextAccessor.HttpContext?.Request.Headers["Accept-Language"].ToString() ?? "zh-CN";
  }

  /// <summary>
  /// 获取客户端IP
  /// </summary>
  public string GetClientIp()
  {
    var httpContext = _httpContextAccessor.HttpContext;
    if (httpContext == null) return string.Empty;

    var forwardedFor = httpContext.Request.Headers["X-Forwarded-For"].ToString();
    if (!string.IsNullOrEmpty(forwardedFor))
    {
      return forwardedFor.Split(',')[0].Trim();
    }

    return httpContext.Connection.RemoteIpAddress?.ToString() ?? string.Empty;
  }

  /// <summary>
  /// 获取用户代理
  /// </summary>
  public string GetUserAgent()
  {
    return _httpContextAccessor.HttpContext?.Request.Headers["User-Agent"].ToString() ?? string.Empty;
  }

  /// <summary>
  /// 获取请求路径
  /// </summary>
  public string GetRequestPath()
  {
    return _httpContextAccessor.HttpContext?.Request.Path.Value ?? string.Empty;
  }

  /// <summary>
  /// 获取请求方法
  /// </summary>
  public string GetRequestMethod()
  {
    return _httpContextAccessor.HttpContext?.Request.Method ?? string.Empty;
  }

  /// <summary>
  /// 获取请求URL
  /// </summary>
  public string GetRequestUrl()
  {
    var request = _httpContextAccessor.HttpContext?.Request;
    if (request == null) return string.Empty;

    var scheme = request.Scheme;
    var host = request.Host.Value;
    var path = request.Path.Value;
    var queryString = request.QueryString.Value;

    return $"{scheme}://{host}{path}{queryString}";
  }

  /// <summary>
  /// 获取请求来源
  /// </summary>
  public string GetReferer()
  {
    return _httpContextAccessor.HttpContext?.Request.Headers["Referer"].ToString() ?? string.Empty;
  }

  /// <summary>
  /// 获取Web根目录路径
  /// </summary>
  /// <returns>Web根目录的完整路径</returns>
  /// <exception cref="InvalidOperationException">当Web根目录不存在或无法访问时抛出</exception>
  public string GetWebRootPath()
  {
    if (_env == null)
    {
      throw new InvalidOperationException("WebHostEnvironment is not initialized.");
    }

    var webRootPath = _env.WebRootPath;
    if (string.IsNullOrEmpty(webRootPath))
    {
      throw new InvalidOperationException("WebRootPath is not configured.");
    }

    if (!Directory.Exists(webRootPath))
    {
      throw new InvalidOperationException($"WebRootPath directory does not exist: {webRootPath}");
    }

    return webRootPath;
  }

  /// <summary>
  /// 获取HTTP上下文
  /// </summary>
  public HttpContext? HttpContext => _httpContextAccessor.HttpContext;

  /// <summary>
  /// 获取Web环境
  /// </summary>
  public IWebHostEnvironment WebHostEnvironment => _env;
}