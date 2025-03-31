using Lean.CodeGen.Domain.Context;
using Lean.CodeGen.Domain.Entities.Identity;
using Microsoft.AspNetCore.Http;

namespace Lean.CodeGen.Infrastructure.Context.Tenant;

/// <summary>
/// 租户上下文实现
/// </summary>
public class LeanTenantContext : ILeanTenantContext
{
  private readonly IHttpContextAccessor _httpContextAccessor;
  private LeanTenant? _currentTenant;
  private long? _currentTenantId;

  public LeanTenantContext(IHttpContextAccessor httpContextAccessor)
  {
    _httpContextAccessor = httpContextAccessor;
  }

  /// <summary>
  /// 当前租户
  /// </summary>
  public LeanTenant? CurrentTenant => _currentTenant;

  /// <summary>
  /// 当前租户ID
  /// </summary>
  public long? CurrentTenantId => _currentTenantId;

  /// <summary>
  /// 设置当前租户
  /// </summary>
  public void SetCurrentTenant(LeanTenant? tenant)
  {
    _currentTenant = tenant;
    _currentTenantId = tenant?.Id;
  }

  /// <summary>
  /// 设置当前租户ID
  /// </summary>
  public void SetCurrentTenantId(long? tenantId)
  {
    _currentTenantId = tenantId;
  }

  /// <summary>
  /// 获取当前租户ID
  /// </summary>
  public long? GetCurrentTenantId()
  {
    var tenantId = _httpContextAccessor.HttpContext?.User.FindFirst("TenantId")?.Value;
    return tenantId != null ? long.Parse(tenantId) : null;
  }

  /// <summary>
  /// 获取当前租户编码
  /// </summary>
  public string GetCurrentTenantCode()
  {
    return _httpContextAccessor.HttpContext?.User.FindFirst("TenantCode")?.Value ?? string.Empty;
  }

  /// <summary>
  /// 获取当前租户名称
  /// </summary>
  public string GetCurrentTenantName()
  {
    return _httpContextAccessor.HttpContext?.User.FindFirst("TenantName")?.Value ?? string.Empty;
  }

  /// <summary>
  /// 获取当前租户状态
  /// </summary>
  public string GetCurrentTenantStatus()
  {
    return _httpContextAccessor.HttpContext?.User.FindFirst("TenantStatus")?.Value ?? string.Empty;
  }

  /// <summary>
  /// 获取当前租户过期时间
  /// </summary>
  public DateTime? GetCurrentTenantExpireTime()
  {
    var expireTime = _httpContextAccessor.HttpContext?.User.FindFirst("TenantExpireTime")?.Value;
    return expireTime != null ? DateTime.Parse(expireTime) : null;
  }

  /// <summary>
  /// 获取当前租户配置
  /// </summary>
  public string GetCurrentTenantConfig()
  {
    return _httpContextAccessor.HttpContext?.User.FindFirst("TenantConfig")?.Value ?? string.Empty;
  }
}