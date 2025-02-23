//===================================================
// 项目名: Lean.CodeGen.WebApi
// 文件名: Program.cs
// 功能描述: 应用程序入口点和配置
// 创建时间: 2024-03-26
// 作者: Lean
// 版本: 1.0
//===================================================

using Microsoft.Extensions.Logging;
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
using StackExchange.Redis;

// 创建Web应用程序构建器
var builder = WebApplication.CreateBuilder(args);

// 设置控制台编码为 UTF-8
Console.OutputEncoding = System.Text.Encoding.UTF8;

// 添加 NLog 日志支持
builder.AddLeanNLog();

// 添加日志服务
builder.Services.AddScoped<ILeanLogService, LeanLogService>();

// 配置安全选项
builder.Services.Configure<LeanSecurityOptions>(
    builder.Configuration.GetSection(LeanSecurityOptions.Position));

// 配置数据库选项
builder.Services.Configure<LeanDatabaseOptions>(
    builder.Configuration.GetSection(LeanDatabaseOptions.Position));

// 添加数据库上下文
builder.Services.AddScoped<LeanDbContext>();

// 添加仓储服务
builder.Services.AddRepositories();

// 添加应用服务
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

// 添加SQL注入防护服务
builder.Services.AddScoped<ILeanSqlSafeService, LeanSqlSafeService>();

// 添加SignalR实时通信服务
builder.Services.AddSignalR();

// 添加API文档服务
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLeanSwagger();

// 添加JWT身份认证服务
builder.Services.AddLeanJwt(builder.Configuration);

// 构建应用程序
var app = builder.Build();



// 初始化数据库
if (builder.Configuration.GetSection("Database:EnableInitData").Get<bool>())
{
  using var scope = app.Services.CreateScope();
  var initializer = scope.ServiceProvider.GetRequiredService<LeanDbInitializer>();
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
