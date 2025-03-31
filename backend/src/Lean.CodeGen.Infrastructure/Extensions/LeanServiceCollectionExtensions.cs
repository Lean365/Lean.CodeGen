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
using Microsoft.Extensions.Configuration;
using Lean.CodeGen.Infrastructure.Configuration;
using Lean.CodeGen.Application.Services.Workflow;
using Lean.CodeGen.Application.Services.Workflow.Executors;
using Lean.CodeGen.Application.Services.Workflow.Parsers;
using Lean.CodeGen.Common.Helpers;
using Lean.CodeGen.Domain.Context;
using Lean.CodeGen.Infrastructure.Context.User;
using Lean.CodeGen.Infrastructure.Context.Tenant;
using StackExchange.Redis;
using Microsoft.Extensions.Caching.Memory;
using Lean.CodeGen.Domain.Interfaces.Caching;
using Lean.CodeGen.Infrastructure.Services.Caching;
using Lean.CodeGen.Application.Services.Routine;
using Lean.CodeGen.Common.Localization;
using Lean.CodeGen.Application.Services.Localization;
using Lean.CodeGen.Infrastructure.Data.Initializer;

namespace Lean.CodeGen.Infrastructure.Extensions;

/// <summary>
/// 服务集合扩展
/// </summary>
public static class LeanServiceCollectionExtensions
{
  /// <summary>
  /// 检查服务是否已注册
  /// </summary>
  private static bool IsServiceRegistered<TService>(this IServiceCollection services)
  {
    return services.Any(x => x.ServiceType == typeof(TService));
  }

  /// <summary>
  /// 检查服务是否已注册（泛型类型）
  /// </summary>
  private static bool IsServiceRegistered(this IServiceCollection services, Type serviceType)
  {
    return services.Any(x => x.ServiceType == serviceType);
  }

  /// <summary>
  /// 添加应用服务
  /// </summary>
  public static IServiceCollection AddApplicationServices(this IServiceCollection services)
  {
    // 添加 NLog 日志服务
    if (!services.IsServiceRegistered<ILogger>())
    {
      services.AddSingleton<ILogger>(LogManager.GetCurrentClassLogger());
    }

    // 添加日志服务
    if (!services.IsServiceRegistered<ILeanLogService>())
    {
      services.AddScoped<ILeanLogService, LeanLogService>();
    }

    // 添加本地化服务
    if (!services.IsServiceRegistered<ILeanLocalizationService>())
    {
      services.AddScoped<ILeanLocalizationService, Lean.CodeGen.Application.Services.Admin.LeanLocalizationService>();
    }

    // 添加 SqlSugar 数据库服务
    if (!services.IsServiceRegistered<ISqlSugarClient>())
    {
      services.AddScoped<ISqlSugarClient>(sp =>
      {
        var options = sp.GetRequiredService<IOptions<LeanDatabaseOptions>>().Value;
        var config = new ConnectionConfig
        {
          DbType = options.DbType,
          ConnectionString = options.ConnectionString,
          IsAutoCloseConnection = true,
          ConfigureExternalServices = new ConfigureExternalServices
          {
            EntityNameService = (type, entity) =>
            {
              entity.IsDisabledUpdateAll = true;
              if (options.EnableUnderLine)
              {
                entity.DbTableName = Common.Utils.LeanNameConvert.ToUnderline(entity.DbTableName);
              }
            }
          }
        };
        return new SqlSugarScope(config);
      });
    }

    // 添加数据库上下文
    if (!services.IsServiceRegistered<LeanDbContext>())
    {
      services.AddScoped<LeanDbContext>();
    }

    // 添加数据库初始化器
    if (!services.IsServiceRegistered<LeanDbInitializer>())
    {
      services.AddScoped<LeanDbInitializer>();
    }

    // 添加仓储服务 - 移除检查，因为泛型服务需要直接注册
    services.AddScoped(typeof(ILeanRepository<>), typeof(LeanRepository<>));
    services.AddScoped(typeof(ILeanRepository<,>), typeof(LeanRepository<,>));

    // 添加安全服务
    if (!services.IsServiceRegistered<ILeanSqlSafeService>())
    {
      services.AddScoped<ILeanSqlSafeService, LeanSqlSafeService>();
    }
    if (!services.IsServiceRegistered<ITokenService>())
    {
      services.AddScoped<ITokenService, JwtTokenService>();
    }

    // 添加基础服务上下文
    if (!services.IsServiceRegistered<LeanBaseServiceContext>())
    {
      services.AddScoped<LeanBaseServiceContext>();
    }

    // 添加身份服务
    if (!services.IsServiceRegistered<ILeanUserService>())
    {
      services.AddScoped<ILeanUserService, LeanUserService>();
    }
    if (!services.IsServiceRegistered<ILeanRoleService>())
    {
      services.AddScoped<ILeanRoleService, LeanRoleService>();
    }
    if (!services.IsServiceRegistered<ILeanMenuService>())
    {
      services.AddScoped<ILeanMenuService, LeanMenuService>();
    }
    if (!services.IsServiceRegistered<ILeanDeptService>())
    {
      services.AddScoped<ILeanDeptService, LeanDeptService>();
    }
    if (!services.IsServiceRegistered<ILeanPostService>())
    {
      services.AddScoped<ILeanPostService, LeanPostService>();
    }
    if (!services.IsServiceRegistered<ILeanAuthService>())
    {
      services.AddScoped<ILeanAuthService, LeanAuthService>();
    }

    // 添加审计服务
    if (!services.IsServiceRegistered<ILeanAuditLogService>())
    {
      services.AddScoped<ILeanAuditLogService, LeanAuditLogService>();
    }
    if (!services.IsServiceRegistered<ILeanLoginLogService>())
    {
      services.AddScoped<ILeanLoginLogService, LeanLoginLogService>();
    }
    if (!services.IsServiceRegistered<ILeanOperationLogService>())
    {
      services.AddScoped<ILeanOperationLogService, LeanOperationLogService>();
    }
    if (!services.IsServiceRegistered<ILeanExceptionLogService>())
    {
      services.AddScoped<ILeanExceptionLogService, LeanExceptionLogService>();
    }
    if (!services.IsServiceRegistered<ILeanSqlDiffLogService>())
    {
      services.AddScoped<ILeanSqlDiffLogService, LeanSqlDiffLogService>();
    }

    // 添加管理服务
    if (!services.IsServiceRegistered<ILeanConfigService>())
    {
      services.AddScoped<ILeanConfigService, LeanConfigService>();
    }
    if (!services.IsServiceRegistered<ILeanDictTypeService>())
    {
      services.AddScoped<ILeanDictTypeService, LeanDictTypeService>();
    }
    if (!services.IsServiceRegistered<ILeanDictDataService>())
    {
      services.AddScoped<ILeanDictDataService, LeanDictDataService>();
    }
    if (!services.IsServiceRegistered<ILeanLanguageService>())
    {
      services.AddScoped<ILeanLanguageService, LeanLanguageService>();
    }
    if (!services.IsServiceRegistered<ILeanTranslationService>())
    {
      services.AddScoped<ILeanTranslationService, LeanTranslationService>();
    }

    // 添加代码生成服务
    if (!services.IsServiceRegistered<ILeanDataSourceService>())
    {
      services.AddScoped<ILeanDataSourceService, LeanDataSourceService>();
    }
    if (!services.IsServiceRegistered<ILeanDbTableService>())
    {
      services.AddScoped<ILeanDbTableService, LeanDbTableService>();
    }
    if (!services.IsServiceRegistered<ILeanTableConfigService>())
    {
      services.AddScoped<ILeanTableConfigService, LeanTableConfigService>();
    }
    if (!services.IsServiceRegistered<ILeanGenConfigService>())
    {
      services.AddScoped<ILeanGenConfigService, LeanGenConfigService>();
    }
    if (!services.IsServiceRegistered<ILeanGenTemplateService>())
    {
      services.AddScoped<ILeanGenTemplateService, LeanGenTemplateService>();
    }
    if (!services.IsServiceRegistered<ILeanGenTaskService>())
    {
      services.AddScoped<ILeanGenTaskService, LeanGenTaskService>();
    }
    if (!services.IsServiceRegistered<ILeanGenHistoryService>())
    {
      services.AddScoped<ILeanGenHistoryService, LeanGenHistoryService>();
    }

    // 添加在线服务
    if (!services.IsServiceRegistered<ILeanOnlineUserService>())
    {
      services.AddScoped<ILeanOnlineUserService, LeanOnlineUserService>();
    }
    if (!services.IsServiceRegistered<ILeanOnlineMessageService>())
    {
      services.AddScoped<ILeanOnlineMessageService, LeanOnlineMessageService>();
    }

    // 注册工作流服务
    if (!services.IsServiceRegistered<ILeanWorkflowEngine>())
    {
      services.AddScoped<ILeanWorkflowEngine, LeanWorkflowEngine>();
    }
    if (!services.IsServiceRegistered<LeanBpmnParser>())
    {
      services.AddScoped<LeanBpmnParser>();
    }
    if (!services.IsServiceRegistered<LeanNodeExecutor>())
    {
      services.AddScoped<LeanNodeExecutor>();
    }
    if (!services.IsServiceRegistered<LeanConditionParser>())
    {
      services.AddScoped<LeanConditionParser>();
    }

    return services;
  }

  /// <summary>
  /// 添加配置服务
  /// </summary>
  /// <param name="services">服务集合</param>
  /// <param name="configuration">配置</param>
  /// <returns>服务集合</returns>
  public static IServiceCollection AddLeanConfiguration(
      this IServiceCollection services,
      IConfiguration configuration)
  {
    // 添加配置服务
    if (!services.IsServiceRegistered<IConfiguration>())
    {
      services.AddLeanConfig(configuration);
    }

    // 添加配置选项
    if (!services.IsServiceRegistered<IOptions<LeanDatabaseOptions>>())
    {
      services.Configure<LeanDatabaseOptions>(configuration.GetSection("Database"));
    }
    if (!services.IsServiceRegistered<IOptions<LeanCacheOptions>>())
    {
      services.Configure<LeanCacheOptions>(configuration.GetSection("Cache"));
    }
    if (!services.IsServiceRegistered<IOptions<LeanJwtOptions>>())
    {
      services.Configure<LeanJwtOptions>(configuration.GetSection("JwtSettings"));
    }
    if (!services.IsServiceRegistered<IOptions<LeanSecurityOptions>>())
    {
      services.Configure<LeanSecurityOptions>(configuration.GetSection("Security"));
    }
    if (!services.IsServiceRegistered<IOptions<LeanLocalizationOptions>>())
    {
      services.Configure<LeanLocalizationOptions>(configuration.GetSection("LocalizationSettings"));
    }
    if (!services.IsServiceRegistered<IOptions<LeanIpOptions>>())
    {
      services.Configure<LeanIpOptions>(configuration.GetSection("Ip"));
    }
    if (!services.IsServiceRegistered<IOptions<LeanMailOptions>>())
    {
      services.Configure<LeanMailOptions>(configuration.GetSection("Mail"));
    }
    if (!services.IsServiceRegistered<IOptions<LeanSliderCaptchaOptions>>())
    {
      services.Configure<LeanSliderCaptchaOptions>(configuration.GetSection("SliderCaptcha"));
    }

    return services;
  }

  /// <summary>
  /// 添加基础设施服务
  /// </summary>
  public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
  {
    // 添加系统信息帮助类
    if (!services.IsServiceRegistered<LeanServerInfoHelper>())
    {
      services.AddScoped<LeanServerInfoHelper>();
    }
    if (!services.IsServiceRegistered<LeanClientInfoHelper>())
    {
      services.AddScoped<LeanClientInfoHelper>();
    }

    // 添加 IP 帮助类
    if (!services.IsServiceRegistered<LeanIpHelper>())
    {
      services.AddSingleton<LeanIpHelper>();
    }

    // 添加邮件帮助类
    if (!services.IsServiceRegistered<LeanMailHelper>())
    {
      services.AddScoped<LeanMailHelper>();
    }
    if (!services.IsServiceRegistered<ILeanMailService>())
    {
      services.AddScoped<ILeanMailService, LeanMailService>();
    }

    // 添加通知服务
    if (!services.IsServiceRegistered<ILeanNotificationService>())
    {
      services.AddScoped<ILeanNotificationService, LeanNotificationService>();
    }

    // 添加定时任务帮助类
    if (!services.IsServiceRegistered<LeanQuartzHelper>())
    {
      services.AddSingleton<LeanQuartzHelper>();
    }

    // 添加会话服务
    if (!services.IsServiceRegistered<ILeanSessionService>())
    {
      services.AddScoped<ILeanSessionService, LeanSessionService>();
    }

    // 添加用户上下文服务
    if (!services.IsServiceRegistered<ILeanUserContext>())
    {
      services.AddScoped<ILeanUserContext, LeanUserContext>();
    }
    if (!services.IsServiceRegistered<ILeanTenantContext>())
    {
      services.AddScoped<ILeanTenantContext, LeanTenantContext>();
    }

    return services;
  }

  /// <summary>
  /// 添加缓存服务
  /// </summary>
  public static IServiceCollection AddLeanCache(
      this IServiceCollection services,
      IConfiguration configuration)
  {
    var cacheOptions = configuration
        .GetSection(LeanCacheOptions.Position)
        .Get<LeanCacheOptions>();

    if (cacheOptions?.EnableRedis == true && !string.IsNullOrEmpty(cacheOptions.Redis.ConnectionString))
    {
      // 添加Redis缓存
      if (!services.IsServiceRegistered<IConnectionMultiplexer>())
      {
        services.AddSingleton<IConnectionMultiplexer>(sp =>
            ConnectionMultiplexer.Connect(cacheOptions.Redis.ConnectionString));
      }
      if (!services.IsServiceRegistered<ILeanCacheService>())
      {
        services.AddScoped<ILeanCacheService, LeanRedisCacheService>();
      }
    }
    else
    {
      // 添加内存缓存
      if (!services.IsServiceRegistered<IMemoryCache>())
      {
        services.AddMemoryCache();
      }
      if (!services.IsServiceRegistered<ILeanCacheService>())
      {
        services.AddScoped<ILeanCacheService, LeanMemoryCacheService>();
      }
    }

    return services;
  }
}