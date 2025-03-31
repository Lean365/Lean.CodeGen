using Lean.CodeGen.Domain.Entities.Identity;

namespace Lean.CodeGen.Domain.Context;

/// <summary>
/// 租户上下文接口
/// </summary>
public interface ILeanTenantContext
{
  /// <summary>
  /// 当前租户
  /// </summary>
  LeanTenant? CurrentTenant { get; }

  /// <summary>
  /// 当前租户ID
  /// </summary>
  long? CurrentTenantId { get; }

  /// <summary>
  /// 设置当前租户
  /// </summary>
  void SetCurrentTenant(LeanTenant? tenant);

  /// <summary>
  /// 设置当前租户ID
  /// </summary>
  void SetCurrentTenantId(long? tenantId);

  /// <summary>
  /// 获取当前租户ID
  /// </summary>
  long? GetCurrentTenantId();

  /// <summary>
  /// 获取当前租户编码
  /// </summary>
  string GetCurrentTenantCode();

  /// <summary>
  /// 获取当前租户名称
  /// </summary>
  string GetCurrentTenantName();

  /// <summary>
  /// 获取当前租户状态
  /// </summary>
  string GetCurrentTenantStatus();

  /// <summary>
  /// 获取当前租户过期时间
  /// </summary>
  DateTime? GetCurrentTenantExpireTime();

  /// <summary>
  /// 获取当前租户配置
  /// </summary>
  string GetCurrentTenantConfig();
}