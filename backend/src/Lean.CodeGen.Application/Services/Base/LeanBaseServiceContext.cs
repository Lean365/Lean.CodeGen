using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Lean.CodeGen.Common.Options;
using Lean.CodeGen.Application.Services.Security;
using Lean.CodeGen.Domain.Context;
using NLog;

namespace Lean.CodeGen.Application.Services.Base;

/// <summary>
/// 应用服务上下文
/// </summary>
/// <remarks>
/// 封装应用层基础服务的共享依赖项，避免在子类中重复注入
/// </remarks>
public class LeanBaseServiceContext
{
  /// <summary>
  /// 用户上下文
  /// </summary>
  public ILeanUserContext UserContext { get; }

  /// <summary>
  /// 日志记录器
  /// </summary>
  public ILogger Logger { get; set; }

  /// <summary>
  /// SQL安全服务
  /// </summary>
  public ILeanSqlSafeService SqlSafeService { get; }

  /// <summary>
  /// 安全选项
  /// </summary>
  public LeanSecurityOptions SecurityOptions { get; }

  /// <summary>
  /// 当前用户名
  /// </summary>
  public string CurrentUserName => UserContext.GetCurrentUserName();

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="userContext">用户上下文</param>
  /// <param name="logger">日志记录器</param>
  /// <param name="sqlSafeService">SQL注入防护服务</param>
  /// <param name="securityOptions">安全配置选项</param>
  public LeanBaseServiceContext(
      ILeanUserContext userContext,
      ILogger logger,
      ILeanSqlSafeService sqlSafeService,
      IOptions<LeanSecurityOptions> securityOptions)
  {
    UserContext = userContext;
    Logger = logger;
    SqlSafeService = sqlSafeService;
    SecurityOptions = securityOptions.Value;
  }
}