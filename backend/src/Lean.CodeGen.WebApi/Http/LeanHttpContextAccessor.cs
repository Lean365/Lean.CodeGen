using Lean.CodeGen.Common.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.Security.Claims;

namespace Lean.CodeGen.WebApi.Http;

/// <summary>
/// HTTP 上下文访问器实现
/// </summary>
public class LeanHttpContextAccessor : ILeanHttpContextAccessor
{
  private readonly IHttpContextAccessor _httpContextAccessor;
  private readonly IWebHostEnvironment _webHostEnvironment;

  /// <summary>
  /// 构造函数
  /// </summary>
  public LeanHttpContextAccessor(
    IHttpContextAccessor httpContextAccessor,
    IWebHostEnvironment webHostEnvironment)
  {
    _httpContextAccessor = httpContextAccessor;
    _webHostEnvironment = webHostEnvironment;
  }

  /// <summary>
  /// 获取Web根目录路径
  /// </summary>
  public string WebRootPath => _webHostEnvironment.WebRootPath;

  /// <summary>
  /// 获取当前用户ID
  /// </summary>
  public long? GetCurrentUserId()
  {
    var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    return userId != null ? long.Parse(userId) : null;
  }

  /// <summary>
  /// 获取当前用户名
  /// </summary>
  public string GetCurrentUserName()
  {
    return _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? string.Empty;
  }

  /// <summary>
  /// 获取客户端IP
  /// </summary>
  public string GetClientIp()
  {
    return _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? string.Empty;
  }

  /// <summary>
  /// 获取用户代理
  /// </summary>
  public string GetUserAgent()
  {
    return _httpContextAccessor.HttpContext?.Request?.Headers["User-Agent"].ToString() ?? string.Empty;
  }

  /// <summary>
  /// 获取请求路径
  /// </summary>
  public string GetRequestPath()
  {
    return _httpContextAccessor.HttpContext?.Request?.Path.Value ?? string.Empty;
  }

  /// <summary>
  /// 获取请求方法
  /// </summary>
  public string GetRequestMethod()
  {
    return _httpContextAccessor.HttpContext?.Request?.Method ?? string.Empty;
  }

  /// <summary>
  /// 获取请求URL
  /// </summary>
  public string GetRequestUrl()
  {
    var request = _httpContextAccessor.HttpContext?.Request;
    if (request == null) return string.Empty;

    return $"{request.Scheme}://{request.Host}{request.PathBase}{request.Path}{request.QueryString}";
  }

  /// <summary>
  /// 获取请求来源
  /// </summary>
  public string GetReferer()
  {
    return _httpContextAccessor.HttpContext?.Request?.Headers["Referer"].ToString() ?? string.Empty;
  }

  /// <summary>
  /// 获取当前语言
  /// </summary>
  public string GetCurrentLanguage()
  {
    var context = _httpContextAccessor.HttpContext;
    if (context == null)
    {
      return "en-US";
    }

    // 优先从 Cookie 中获取
    if (context.Request.Cookies.TryGetValue("lang", out var langCode))
    {
      return langCode;
    }

    // 从 Accept-Language 头中获取
    var acceptLanguage = context.Request.Headers["Accept-Language"].ToString();
    if (!string.IsNullOrEmpty(acceptLanguage))
    {
      return acceptLanguage.Split(',')[0];
    }

    return "en-US";
  }
}