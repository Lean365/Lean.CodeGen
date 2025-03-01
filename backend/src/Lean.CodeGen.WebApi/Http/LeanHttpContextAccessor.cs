using Lean.CodeGen.Common.Http;
using Microsoft.AspNetCore.Http;

namespace Lean.CodeGen.WebApi.Http;

/// <summary>
/// HTTP 上下文访问器实现
/// </summary>
public class LeanHttpContextAccessor : ILeanHttpContextAccessor
{
  private readonly IHttpContextAccessor _httpContextAccessor;

  /// <summary>
  /// 构造函数
  /// </summary>
  public LeanHttpContextAccessor(IHttpContextAccessor httpContextAccessor)
  {
    _httpContextAccessor = httpContextAccessor;
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