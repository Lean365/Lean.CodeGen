namespace Lean.CodeGen.Common.Http;

/// <summary>
/// HTTP 上下文访问器接口
/// </summary>
public interface ILeanHttpContextAccessor
{
  /// <summary>
  /// 获取当前用户ID
  /// </summary>
  long? GetCurrentUserId();

  /// <summary>
  /// 获取当前用户名
  /// </summary>
  string GetCurrentUserName();

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
  string WebRootPath { get; }
}