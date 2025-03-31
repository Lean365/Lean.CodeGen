using Lean.CodeGen.Domain.Entities.Identity;

namespace Lean.CodeGen.Domain.Context;

/// <summary>
/// 用户上下文接口
/// </summary>
public interface ILeanUserContext
{
  /// <summary>
  /// 当前用户
  /// </summary>
  LeanUser? CurrentUser { get; }

  /// <summary>
  /// 当前用户ID
  /// </summary>
  long? CurrentUserId { get; }

  /// <summary>
  /// 设置当前用户
  /// </summary>
  void SetCurrentUser(LeanUser? user);

  /// <summary>
  /// 设置当前用户ID
  /// </summary>
  void SetCurrentUserId(long? userId);

  /// <summary>
  /// 获取当前用户ID
  /// </summary>
  long? GetCurrentUserId();

  /// <summary>
  /// 获取当前用户名
  /// </summary>
  string GetCurrentUserName();

  /// <summary>
  /// 获取当前用户真实姓名
  /// </summary>
  string GetCurrentUserRealName();

  /// <summary>
  /// 获取当前用户英文名
  /// </summary>
  string GetCurrentUserEnglishName();

  /// <summary>
  /// 获取当前用户昵称
  /// </summary>
  string GetCurrentUserNickname();

  /// <summary>
  /// 获取当前用户类型
  /// </summary>
  string GetCurrentUserType();

  /// <summary>
  /// 获取当前用户头像
  /// </summary>
  string GetCurrentUserAvatar();

  /// <summary>
  /// 获取当前用户状态
  /// </summary>
  string GetCurrentUserStatus();

  /// <summary>
  /// 获取当前用户角色列表
  /// </summary>
  string[] GetCurrentUserRoles();

  /// <summary>
  /// 获取当前语言
  /// </summary>
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
  /// 获取当前租户ID
  /// </summary>
  long? GetCurrentTenantId();
}