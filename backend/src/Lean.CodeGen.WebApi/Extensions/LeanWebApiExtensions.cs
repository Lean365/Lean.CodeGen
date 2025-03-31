using Lean.CodeGen.Domain.Interfaces.Hubs;
using Lean.CodeGen.WebApi.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Lean.CodeGen.WebApi.Filters;
using Lean.CodeGen.Common.Http;
using Microsoft.AspNetCore.Http;
using Lean.CodeGen.Common.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Lean.CodeGen.Common.Helpers;
using Microsoft.AspNetCore.Hosting;
using Lean.CodeGen.WebApi.Services;

namespace Lean.CodeGen.WebApi.Extensions;

/// <summary>
/// WebApi 扩展方法
/// </summary>
public static class LeanWebApiExtensions
{
  /// <summary>
  /// 添加 Web 服务
  /// </summary>
  public static IServiceCollection AddLeanWebServices(this IServiceCollection services)
  {
    // 添加控制器服务
    services.AddControllers(options =>
    {
      // 添加权限过滤器
      options.Filters.Add<LeanPermissionFilter>();
      // 添加限流过滤器
      options.Filters.Add<LeanRateLimitFilter>();
    });

    // 添加 HTTP 上下文访问器
    services.AddHttpContextAccessor();
    services.AddScoped<ILeanHttpContextAccessor, LeanHttpContextAccessor>();

    return services;
  }

  /// <summary>
  /// 添加 CORS 服务
  /// </summary>
  public static IServiceCollection AddLeanCors(this IServiceCollection services)
  {
    services.AddCors(options =>
    {
      options.AddDefaultPolicy(policy =>
      {
        policy.WithOrigins(
                "http://localhost:5173", // Vite 开发服务器默认端口
                "http://localhost:5153",
                "https://localhost:5153",
                "https://localhost:7152",
                "http://localhost:5152")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
            .WithExposedHeaders("Content-Disposition", "X-Suggested-Filename")
            .SetPreflightMaxAge(TimeSpan.FromMinutes(10));
      });
    });

    return services;
  }

  /// <summary>
  /// 添加 SignalR 服务
  /// </summary>
  public static IServiceCollection AddLeanSignalR(this IServiceCollection services)
  {
    // 添加SignalR实时通信服务
    services.AddSignalR(options =>
    {
      options.EnableDetailedErrors = true;
      options.MaximumReceiveMessageSize = 102400; // 100KB
      options.ClientTimeoutInterval = TimeSpan.FromSeconds(30);
      options.KeepAliveInterval = TimeSpan.FromSeconds(15);
      options.HandshakeTimeout = TimeSpan.FromSeconds(15);
    });

    // 注册SignalR Hub服务
    services.AddScoped<ILeanSignalRHub, LeanSignalRHub>();
    services.AddScoped<ILeanOnlineUserHub, LeanOnlineUserHub>();
    services.AddScoped<ILeanOnlineMessageHub, LeanOnlineMessageHub>();

    return services;
  }

  /// <summary>
  /// 添加防伪服务
  /// </summary>
  public static IServiceCollection AddLeanAntiforgery(
      this IServiceCollection services,
      IConfiguration configuration)
  {
    var securityOptions = configuration
        .GetSection(LeanSecurityOptions.Position)
        .Get<LeanSecurityOptions>();

    services.AddAntiforgery(options =>
    {
      // 配置防伪令牌头部
      options.HeaderName = securityOptions?.Antiforgery.HeaderName ?? "X-XSRF-TOKEN";
      // 配置防伪令牌Cookie
      options.Cookie.Name = securityOptions?.Antiforgery.CookieName ?? "XSRF-TOKEN";
    });

    return services;
  }

  /// <summary>
  /// 添加滑块验证码服务
  /// </summary>
  public static IServiceCollection AddLeanSliderCaptcha(
      this IServiceCollection services,
      IConfiguration configuration)
  {
    services.Configure<LeanSliderCaptchaOptions>(configuration.GetSection("SliderCaptcha"));
    services.AddSingleton<LeanSliderCaptchaHelper>(sp =>
    {
      var env = sp.GetRequiredService<IWebHostEnvironment>();
      var options = sp.GetRequiredService<IOptions<LeanSliderCaptchaOptions>>();
      return new LeanSliderCaptchaHelper(env.WebRootPath, options);
    });
    services.AddHostedService<LeanSliderCaptchaInitializer>();

    return services;
  }
}