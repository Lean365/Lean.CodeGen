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
using Lean.CodeGen.Infrastructure.Services;
using Lean.CodeGen.Application.Services.Security;
using Lean.CodeGen.Domain.Interfaces.Caching;
using Lean.CodeGen.Infrastructure.Services.Caching;
using Lean.CodeGen.Infrastructure.Extensions;
using Lean.CodeGen.Infrastructure.Data.Context;
using Lean.CodeGen.Infrastructure.Data.Initializer;
using Lean.CodeGen.Infrastructure.Services.Logging;
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
using Lean.CodeGen.Application.Services.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Lean.CodeGen.WebApi.Hubs;
using Lean.CodeGen.Domain.Interfaces.Hubs;
using Lean.CodeGen.Infrastructure.Context.User;
using Lean.CodeGen.Infrastructure.Context.Tenant;
using Lean.CodeGen.Domain.Context;
using Lean.CodeGen.Application.Services.Routine;
using Lean.CodeGen.Domain.Entities.Identity;
using Lean.CodeGen.Domain.Entities.Workflow;
using Lean.CodeGen.Domain.Entities.Signalr;
using Lean.CodeGen.WebApi.Extensions;

// 创建Web应用程序构建器
var builder = WebApplication.CreateBuilder(args);

// 设置控制台编码为 UTF-8
Console.OutputEncoding = System.Text.Encoding.UTF8;

// 清除默认日志提供程序并添加NLog
builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);
builder.Host.UseNLog();

// 添加配置服务（包含所有配置选项）
builder.Services.AddLeanConfiguration(builder.Configuration);

// 添加应用服务（包含仓储注册）
builder.Services.AddApplicationServices();

// 添加基础设施服务（包含帮助类和上下文服务）
builder.Services.AddInfrastructureServices();

// 添加数据库初始化器
builder.Services.AddScoped<LeanDbInitializer>();

// 添加缓存服务
builder.Services.AddLeanCache(builder.Configuration);

// 添加 Web 服务（包含控制器、HTTP上下文等）
builder.Services.AddLeanWebServices();

// 添加 CORS 服务
builder.Services.AddLeanCors();

// 添加 SignalR 服务
builder.Services.AddLeanSignalR();

// 添加防伪服务
builder.Services.AddLeanAntiforgery(builder.Configuration);

// 添加API文档服务
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLeanSwagger();

// 添加JWT身份认证服务
builder.Services.AddLeanJwt(builder.Configuration);

// 添加滑块验证码服务
builder.Services.AddLeanSliderCaptcha(builder.Configuration);

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
else
{
  // 即使不初始化数据，也要确保表结构正确
  using var scope = app.Services.CreateScope();
  var dbContext = scope.ServiceProvider.GetRequiredService<LeanDbContext>();
  dbContext.ConfigureEntities();
}

// 配置中间件
app.UseStaticFiles();

// 启用CORS（移到HTTPS重定向之前）
app.UseCors();

// 仅在生产环境启用HTTPS重定向
if (!app.Environment.IsDevelopment())
{
  app.UseHttpsRedirection();
}

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
app.MapHub<LeanSignalRHub>("/signalr/hubs");
app.MapHub<LeanOnlineUserHub>("/signalr/hubs/online");
app.MapHub<LeanOnlineMessageHub>("/signalr/hubs/message");

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
