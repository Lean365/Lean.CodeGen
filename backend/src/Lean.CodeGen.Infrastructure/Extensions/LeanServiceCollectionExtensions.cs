using Microsoft.Extensions.DependencyInjection;
using Lean.CodeGen.Application.Services.Signalr;
using Lean.CodeGen.Domain.Interfaces.Repositories;
using Lean.CodeGen.Infrastructure.Repositories;
using Lean.CodeGen.Application.Services.Identity;
using Lean.CodeGen.Application.Services.Admin;
using Lean.CodeGen.Application.Services.Audit;
using Lean.CodeGen.Application.Services.Generator;
using Lean.CodeGen.Infrastructure.Data.Context;
using Lean.CodeGen.Common.Options;
using Microsoft.Extensions.Options;
using SqlSugar;
using Lean.CodeGen.Application.Services.Security;
using Lean.CodeGen.Common.Security;
using Lean.CodeGen.Infrastructure.Services;
using Lean.CodeGen.Application.Services.Base;
using Lean.CodeGen.Infrastructure.Services.Logging;
using NLog;
using ILogger = NLog.ILogger;

namespace Lean.CodeGen.Infrastructure.Extensions;

/// <summary>
/// 服务集合扩展
/// </summary>
public static class LeanServiceCollectionExtensions
{
  /// <summary>
  /// 添加应用服务
  /// </summary>
  public static IServiceCollection AddApplicationServices(this IServiceCollection services)
  {
    // 添加日志服务
    services.AddSingleton<ILogger>(provider => LogManager.GetCurrentClassLogger());
    services.AddScoped<ILeanLogService, LeanLogService>();

    // 添加基础服务上下文
    services.AddScoped<LeanBaseServiceContext>();

    // 添加安全服务
    services.AddScoped<ILeanSqlSafeService, LeanSqlSafeService>();
    services.AddScoped<ITokenService, JwtTokenService>();

    // 添加身份服务
    services.AddScoped<ILeanUserService, LeanUserService>();
    services.AddScoped<ILeanRoleService, LeanRoleService>();
    services.AddScoped<ILeanMenuService, LeanMenuService>();
    services.AddScoped<ILeanDeptService, LeanDeptService>();
    services.AddScoped<ILeanPostService, LeanPostService>();

    // 添加审计服务
    services.AddScoped<ILeanAuditLogService, LeanAuditLogService>();
    services.AddScoped<ILeanLoginLogService, LeanLoginLogService>();
    services.AddScoped<ILeanOperationLogService, LeanOperationLogService>();
    services.AddScoped<ILeanExceptionLogService, LeanExceptionLogService>();
    services.AddScoped<ILeanSqlDiffLogService, LeanSqlDiffLogService>();

    // 添加管理服务
    services.AddScoped<ILeanConfigService, LeanConfigService>();
    services.AddScoped<ILeanDictTypeService, LeanDictTypeService>();
    services.AddScoped<ILeanDictDataService, LeanDictDataService>();
    services.AddScoped<ILeanLanguageService, LeanLanguageService>();
    services.AddScoped<ILeanTranslationService, LeanTranslationService>();
    services.AddScoped<ILeanLocalizationService, LeanLocalizationService>();

    // 添加代码生成服务
    services.AddScoped<ILeanDataSourceService, LeanDataSourceService>();
    services.AddScoped<ILeanDbTableService, LeanDbTableService>();
    services.AddScoped<ILeanTableConfigService, LeanTableConfigService>();
    services.AddScoped<ILeanGenConfigService, LeanGenConfigService>();
    services.AddScoped<ILeanGenTemplateService, LeanGenTemplateService>();
    services.AddScoped<ILeanGenTaskService, LeanGenTaskService>();
    services.AddScoped<ILeanGenHistoryService, LeanGenHistoryService>();

    // 添加在线服务
    services.AddScoped<ILeanOnlineUserService, LeanOnlineUserService>();
    services.AddScoped<ILeanOnlineMessageService, LeanOnlineMessageService>();

    return services;
  }

  /// <summary>
  /// 添加仓储服务
  /// </summary>
  public static IServiceCollection AddRepositories(this IServiceCollection services)
  {
    // 注册泛型仓储
    services.AddScoped(typeof(ILeanRepository<>), typeof(LeanRepository<>));
    services.AddScoped(typeof(ILeanRepository<,>), typeof(LeanRepository<,>));

    return services;
  }
}