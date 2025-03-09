//===================================================
// 项目名: Lean.CodeGen.WebApi
// 文件名: Program.cs
// 功能描述: 应用程序入口点和配置
// 创建时间: 2024-03-26
// 作者: Lean
// 版本: 1.0
//===================================================

using System.Net.WebSockets;
using Microsoft.AspNetCore.Http;
using Lean.CodeGen.WebApi.Middlewares;
using Lean.CodeGen.WebApi.Configurations;
using Lean.CodeGen.WebApi.Filters;
using Lean.CodeGen.Common.Options;
using Lean.CodeGen.Common.Extensions;
using Lean.CodeGen.WebApi.Hubs;
using Lean.CodeGen.Infrastructure.Services;
using Lean.CodeGen.Application.Services.Security;
using Lean.CodeGen.Domain.Interfaces.Caching;
using Lean.CodeGen.Infrastructure.Services.Caching;
using Lean.CodeGen.Infrastructure.Extensions;
using Lean.CodeGen.Infrastructure.Data.Context;
using Lean.CodeGen.Infrastructure.Data.Initializer;
using Lean.CodeGen.Infrastructure.Services.Logging;
using Lean.CodeGen.WebApi.Http;
using Lean.CodeGen.Common.Http;
using Lean.CodeGen.Application.Services.Admin;
using StackExchange.Redis;
using Lean.CodeGen.Application.Services.Base;
using Lean.CodeGen.Domain.Interfaces.Repositories;
using Lean.CodeGen.Infrastructure.Repositories;
using Lean.CodeGen.Domain.Entities.Admin;
using NLog;
using NLog.Web;
using ILogger = NLog.ILogger;
using Microsoft.Extensions.Options;
using Lean.CodeGen.Common.Helpers;
using Lean.CodeGen.WebApi.Services;
using Microsoft.AspNetCore.Hosting;

// 创建Web应用程序构建器
var builder = WebApplication.CreateBuilder(args);

// 设置控制台编码为 UTF-8
Console.OutputEncoding = System.Text.Encoding.UTF8;

// 添加 NLog 日志支持
builder.Host.UseNLog();

// 配置安全选项
builder.Services.Configure<LeanSecurityOptions>(
    builder.Configuration.GetSection(LeanSecurityOptions.Position));

// 配置本地化选项
builder.Services.Configure<LeanLocalizationOptions>(
    builder.Configuration.GetSection(LeanLocalizationOptions.Position));

// 配置数据库选项
builder.Services.Configure<LeanDatabaseOptions>(
    builder.Configuration.GetSection(LeanDatabaseOptions.Position));

// 配置 IP 选项
builder.Services.Configure<LeanIpOptions>(
    builder.Configuration.GetSection("Ip"));

// 配置邮件选项
builder.Services.Configure<LeanMailOptions>(
    builder.Configuration.GetSection("Mail"));

// 添加数据库上下文
builder.Services.AddScoped<LeanDbContext>();

// 添加应用服务（包含仓储注册）
builder.Services.AddApplicationServices();

// 添加数据库初始化器
builder.Services.AddScoped<LeanDbInitializer>();

// 配置缓存选项
builder.Services.Configure<LeanCacheOptions>(
    builder.Configuration.GetSection(LeanCacheOptions.Position));

// 添加缓存服务
var cacheOptions = builder.Configuration
    .GetSection(LeanCacheOptions.Position)
    .Get<LeanCacheOptions>();

if (cacheOptions?.EnableRedis == true && !string.IsNullOrEmpty(cacheOptions.Redis.ConnectionString))
{
  // 添加Redis缓存
  builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
      ConnectionMultiplexer.Connect(cacheOptions.Redis.ConnectionString));
  builder.Services.AddScoped<ILeanCacheService, LeanRedisCacheService>();
}
else
{
  // 添加内存缓存
  builder.Services.AddMemoryCache();
  builder.Services.AddScoped<ILeanCacheService, LeanMemoryCacheService>();
}

// 添加控制器服务
builder.Services.AddControllers(options =>
{
  // 添加权限过滤器
  options.Filters.Add<LeanPermissionFilter>();
  // 添加限流过滤器
  options.Filters.Add<LeanRateLimitFilter>();
});

// 配置防伪服务
var securityOptions = builder.Configuration
    .GetSection(LeanSecurityOptions.Position)
    .Get<LeanSecurityOptions>();

builder.Services.AddAntiforgery(options =>
{
  // 配置防伪令牌头部
  options.HeaderName = securityOptions?.Antiforgery.HeaderName ?? "X-XSRF-TOKEN";
  // 配置防伪令牌Cookie
  options.Cookie.Name = securityOptions?.Antiforgery.CookieName ?? "XSRF-TOKEN";
});

// 添加SignalR实时通信服务
builder.Services.AddSignalR();

// 添加API文档服务
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLeanSwagger();

// 添加JWT身份认证服务
builder.Services.AddLeanJwt(builder.Configuration);

// 添加本地化服务
builder.Services.AddScoped<ILeanLocalizationService, LeanLocalizationService>();
builder.Services.AddScoped<ILeanConfigService, LeanConfigService>();
builder.Services.AddScoped<ILeanHttpContextAccessor, LeanHttpContextAccessor>();

// 注册滑块验证码服务
builder.Services.Configure<LeanSliderCaptchaOptions>(builder.Configuration.GetSection("SliderCaptcha"));
builder.Services.AddSingleton<LeanSliderCaptchaHelper>(sp =>
{
  var env = sp.GetRequiredService<IWebHostEnvironment>();
  var options = sp.GetRequiredService<IOptions<LeanSliderCaptchaOptions>>();
  return new LeanSliderCaptchaHelper(env.WebRootPath, options);
});
builder.Services.AddHostedService<LeanSliderCaptchaInitializer>();

// 注册系统信息帮助类
builder.Services.AddScoped<LeanServerInfoHelper>();
builder.Services.AddScoped<LeanClientInfoHelper>();

// 注册 IP 帮助类
builder.Services.AddSingleton<LeanIpHelper>();

// 注册邮件帮助类
builder.Services.AddScoped<LeanMailHelper>();

// 注册定时任务帮助类
builder.Services.AddSingleton<LeanQuartzHelper>();

// 构建应用程序
var app = builder.Build();



// 初始化数据库
if (builder.Configuration.GetSection("Database:EnableInitData").Get<bool>())
{
  using var scope = app.Services.CreateScope();
  var initializer = new LeanDbInitializer(
      scope.ServiceProvider.GetRequiredService<LeanDbContext>(),
      scope.ServiceProvider.GetRequiredService<IOptions<LeanSecurityOptions>>());
  await initializer.InitializeAsync();
}

// 配置中间件
app.UseHttpsRedirection();

// 启用静态文件
app.UseStaticFiles();

app.UseRouting();

if (app.Environment.IsDevelopment())
{
  app.UseLeanSwagger();
}

// 启用身份认证
app.UseAuthentication();
// 启用授权
app.UseAuthorization();

// 添加全局异常处理中间件
app.UseMiddleware<LeanExceptionMiddleware>();

// 配置SignalR实时通信Hub
app.MapHub<LeanSignalrHub>("/signalr/hubs");
// 输出欢迎信息
using var welcomeScope = app.Services.CreateScope();
var logService = welcomeScope.ServiceProvider.GetRequiredService<ILeanLogService>();
logService.LogWelcomeInfo();
// 启用端点映射
app.MapControllers();

// 示例天气预报接口（可以删除）
var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
  var forecast = Enumerable.Range(1, 5).Select(index =>
      new WeatherForecast
      (
          DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
          Random.Shared.Next(-20, 55),
          summaries[Random.Shared.Next(summaries.Length)]
      ))
      .ToArray();
  return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

// 运行应用程序
app.Run();

/// <summary>
/// 天气预报记录类（示例）
/// </summary>
/// <param name="Date">日期</param>
/// <param name="TemperatureC">摄氏温度</param>
/// <param name="Summary">天气描述</param>
record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
  /// <summary>
  /// 华氏温度
  /// </summary>
  public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
