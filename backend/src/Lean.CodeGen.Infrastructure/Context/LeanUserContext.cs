using Microsoft.AspNetCore.Http;
using Lean.CodeGen.Domain.Context;
using Lean.CodeGen.Domain.Entities.Identity;
using Lean.CodeGen.Common.Http;

namespace Lean.CodeGen.Infrastructure.Context.User;

/// <summary>
/// 用户上下文实现
/// </summary>
public class LeanUserContext : ILeanUserContext
{
  private readonly ILeanHttpContextAccessor _httpContextAccessor;
  private LeanUser? _currentUser;
  private long? _currentUserId;

  public LeanUserContext(ILeanHttpContextAccessor httpContextAccessor)
  {
    _httpContextAccessor = httpContextAccessor;
  }

  /// <summary>
  /// 当前用户
  /// </summary>
  public LeanUser? CurrentUser => _currentUser;

  /// <summary>
  /// 当前用户ID
  /// </summary>
  public long? CurrentUserId => _currentUserId;

  /// <summary>
  /// 设置当前用户
  /// </summary>
  public void SetCurrentUser(LeanUser? user)
  {
    _currentUser = user;
    _currentUserId = user?.Id;
  }

  /// <summary>
  /// 设置当前用户ID
  /// </summary>
  public void SetCurrentUserId(long? userId)
  {
    _currentUserId = userId;
  }

  /// <summary>
  /// 获取当前用户ID
  /// </summary>
  public long? GetCurrentUserId()
  {
    var userId = _httpContextAccessor.HttpContext?.User.FindFirst("UserId")?.Value;
    return userId != null ? long.Parse(userId) : null;
  }

  /// <summary>
  /// 获取当前用户名
  /// </summary>
  public string GetCurrentUserName()
  {
    return _httpContextAccessor.HttpContext?.User.Identity?.Name ?? string.Empty;
  }

  /// <summary>
  /// 获取当前用户真实姓名
  /// </summary>
  public string GetCurrentUserRealName()
  {
    return _httpContextAccessor.HttpContext?.User.FindFirst("RealName")?.Value ?? string.Empty;
  }

  /// <summary>
  /// 获取当前用户英文名
  /// </summary>
  public string GetCurrentUserEnglishName()
  {
    return _httpContextAccessor.HttpContext?.User.FindFirst("EnglishName")?.Value ?? string.Empty;
  }

  /// <summary>
  /// 获取当前用户昵称
  /// </summary>
  public string GetCurrentUserNickname()
  {
    return _httpContextAccessor.HttpContext?.User.FindFirst("Nickname")?.Value ?? string.Empty;
  }

  /// <summary>
  /// 获取当前用户类型
  /// </summary>
  public string GetCurrentUserType()
  {
    return _httpContextAccessor.HttpContext?.User.FindFirst("UserType")?.Value ?? string.Empty;
  }

  /// <summary>
  /// 获取当前用户头像
  /// </summary>
  public string GetCurrentUserAvatar()
  {
    return _httpContextAccessor.HttpContext?.User.FindFirst("Avatar")?.Value ?? string.Empty;
  }

  /// <summary>
  /// 获取当前用户状态
  /// </summary>
  public string GetCurrentUserStatus()
  {
    return _httpContextAccessor.HttpContext?.User.FindFirst("UserStatus")?.Value ?? string.Empty;
  }

  /// <summary>
  /// 获取当前用户角色列表
  /// </summary>
  public string[] GetCurrentUserRoles()
  {
    var roles = _httpContextAccessor.HttpContext?.User.FindFirst("UserRoles")?.Value;
    return roles?.Split(',', StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>();
  }

  /// <summary>
  /// 获取当前语言
  /// </summary>
  public string GetCurrentLanguage()
  {
    return _httpContextAccessor.GetCurrentLanguage();
  }

  /// <summary>
  /// 获取客户端IP
  /// </summary>
  public string GetClientIp()
  {
    return _httpContextAccessor.GetClientIp();
  }

  /// <summary>
  /// 获取用户代理
  /// </summary>
  public string GetUserAgent()
  {
    return _httpContextAccessor.GetUserAgent();
  }

  /// <summary>
  /// 获取请求路径
  /// </summary>
  public string GetRequestPath()
  {
    return _httpContextAccessor.GetRequestPath();
  }

  /// <summary>
  /// 获取请求方法
  /// </summary>
  public string GetRequestMethod()
  {
    return _httpContextAccessor.GetRequestMethod();
  }

  /// <summary>
  /// 获取请求URL
  /// </summary>
  public string GetRequestUrl()
  {
    return _httpContextAccessor.GetRequestUrl();
  }

  /// <summary>
  /// 获取请求来源
  /// </summary>
  public string GetReferer()
  {
    return _httpContextAccessor.GetReferer();
  }

  /// <summary>
  /// 获取当前租户ID
  /// </summary>
  public long? GetCurrentTenantId()
  {
    var tenantId = _httpContextAccessor.HttpContext?.User.FindFirst("TenantId")?.Value;
    return tenantId != null ? long.Parse(tenantId) : null;
  }
}