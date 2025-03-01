using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;

namespace Lean.CodeGen.Infrastructure.Configuration;

/// <summary>
/// 系统配置扩展方法
/// </summary>
public static class LeanConfigExtensions
{
  /// <summary>
  /// 添加系统配置
  /// </summary>
  /// <param name="services">服务集合</param>
  /// <param name="configuration">配置</param>
  /// <returns>服务集合</returns>
  public static IServiceCollection AddLeanConfig(
      this IServiceCollection services,
      IConfiguration configuration)
  {
    // 将现有配置添加到服务中
    services.Configure<IConfiguration>(config =>
    {
      foreach (var setting in configuration.AsEnumerable())
      {
        config[setting.Key] = setting.Value;
      }
    });

    // 添加配置源到配置构建器
    var configBuilder = (IConfigurationBuilder)configuration;
    var serviceProvider = services.BuildServiceProvider();
    var db = serviceProvider.GetRequiredService<ISqlSugarClient>();

    configBuilder.Add(new LeanConfigSource(db, configuration));

    return services;
  }
}