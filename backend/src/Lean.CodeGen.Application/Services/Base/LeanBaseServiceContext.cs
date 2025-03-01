using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Lean.CodeGen.Common.Options;
using Lean.CodeGen.Application.Services.Security;
using Lean.CodeGen.Application.Services.Admin;
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
  /// 当前用户ID
  /// </summary>
  public long? CurrentUserId { get; set; }

  /// <summary>
  /// 当前用户名
  /// </summary>
  public string? CurrentUserName { get; set; }

  /// <summary>
  /// 当前租户ID
  /// </summary>
  public long? CurrentTenantId { get; set; }

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
  /// 本地化服务
  /// </summary>
  public ILeanLocalizationService LocalizationService { get; }

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="logger">日志记录器</param>
  /// <param name="sqlSafeService">SQL注入防护服务</param>
  /// <param name="securityOptions">安全配置选项</param>
  /// <param name="localizationService">本地化服务</param>
  public LeanBaseServiceContext(
      ILogger logger,
      ILeanSqlSafeService sqlSafeService,
      IOptions<LeanSecurityOptions> securityOptions,
      ILeanLocalizationService localizationService)
  {
    Logger = logger;
    SqlSafeService = sqlSafeService;
    SecurityOptions = securityOptions.Value;
    LocalizationService = localizationService;
  }

  /// <summary>
  /// 获取系统翻译
  /// </summary>
  public async Task<string> GetSystemTranslationAsync(string key, object model = null)
  {
    var translation = await LocalizationService.GetTranslationAsync("zh-CN", key);
    if (model != null)
    {
      foreach (var prop in model.GetType().GetProperties())
      {
        var value = prop.GetValue(model)?.ToString() ?? string.Empty;
        translation = translation.Replace($"{{{prop.Name}}}", value);
      }
    }
    return translation;
  }
}