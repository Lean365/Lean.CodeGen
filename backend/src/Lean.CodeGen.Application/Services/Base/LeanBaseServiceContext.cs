using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Lean.CodeGen.Common.Options;
using Lean.CodeGen.Application.Services.Security;

namespace Lean.CodeGen.Application.Services.Base;

/// <summary>
/// 基础服务上下文
/// </summary>
/// <remarks>
/// 封装应用层基础服务的共享依赖项，避免在子类中重复注入
/// </remarks>
public class LeanBaseServiceContext
{
  /// <summary>
  /// SQL注入防护服务
  /// </summary>
  public ILeanSqlSafeService SqlSafeService { get; }

  /// <summary>
  /// 安全配置选项
  /// </summary>
  public LeanSecurityOptions SecurityOptions { get; }

  /// <summary>
  /// 日志记录器
  /// </summary>
  public ILogger Logger { get; }

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="sqlSafeService">SQL注入防护服务</param>
  /// <param name="securityOptions">安全配置选项</param>
  /// <param name="logger">日志记录器</param>
  public LeanBaseServiceContext(
      ILeanSqlSafeService sqlSafeService,
      IOptions<LeanSecurityOptions> securityOptions,
      ILogger logger)
  {
    SqlSafeService = sqlSafeService;
    SecurityOptions = securityOptions.Value;
    Logger = logger;
  }
}