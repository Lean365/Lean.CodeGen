//===================================================
// 项目名: Lean.CodeGen.WebApi
// 文件名: Program.cs
// 功能描述: 应用程序入口点和配置
// 创建时间: 2024-03-26
// 作者: Lean
// 版本: 1.0
//===================================================

using Lean.CodeGen.WebApi.Middlewares;
using Lean.CodeGen.WebApi.Configurations;
using Lean.CodeGen.WebApi.Filters;
using Lean.CodeGen.Common.Options;
using Lean.CodeGen.Common.Extensions;
using Lean.CodeGen.WebApi.Hubs;

// 创建Web应用程序构建器
var builder = WebApplication.CreateBuilder(args);

// 添加 NLog 日志支持
builder.AddLeanNLog();

// 配置安全选项
builder.Services.Configure<LeanSecurityOptions>(
    builder.Configuration.GetSection(LeanSecurityOptions.Position));

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

// 添加内存缓存服务（用于限流）
builder.Services.AddMemoryCache();

// 添加SQL注入防护服务
builder.Services.AddScoped<LeanSqlSafeService>();

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

// 配置HTTP请求处理管道
if (app.Environment.IsDevelopment())
{
  // 在开发环境中启用Swagger
  app.UseSwagger();
  app.UseSwaggerUI();
  app.UseLeanSwagger();
}

// 启用HTTPS重定向
app.UseHttpsRedirection();

// 启用路由
app.UseRouting();

// 启用身份认证
app.UseAuthentication();
// 启用授权
app.UseAuthorization();

// 添加全局异常处理中间件
app.UseMiddleware<LeanExceptionMiddleware>();

// 配置SignalR实时通信Hub
app.MapHub<LeanSignalrHub>("/hbtSignalrHub");

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
