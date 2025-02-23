using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Mvc.Controllers;
using NLog;

namespace Lean.CodeGen.WebApi.Configurations;

/// <summary>
/// Swagger 配置
/// </summary>
public static class LeanSwaggerSetup
{
  private static readonly NLog.ILogger _logger = LogManager.GetCurrentClassLogger();

  /// <summary>
  /// 添加 Swagger 服务
  /// </summary>
  public static IServiceCollection AddLeanSwagger(this IServiceCollection services)
  {
    services.AddSwaggerGen(options =>
    {
      // 1. 添加文档
      var docs = new[]
      {
        new { Name = "identity", Title = "身份认证", Description = "用户认证、授权等接口" },
        new { Name = "admin", Title = "系统管理", Description = "系统管理相关接口" },
        new { Name = "generator", Title = "代码生成", Description = "代码生成相关接口" },
        new { Name = "workflow", Title = "工作流程", Description = "工作流程相关接口" },
        new { Name = "audit", Title = "审计日志", Description = "审计、操作日志相关接口" },
        new { Name = "signalr", Title = "实时通信", Description = "SignalR实时通信相关接口" }
      };

      foreach (var doc in docs)
      {
        options.SwaggerDoc(doc.Name, new OpenApiInfo
        {
          Title = doc.Title,
          Version = "v1",
          Description = $"\uD83D\uDCDD {doc.Description}",
          Contact = new OpenApiContact
          {
            Name = "\uD83D\uDCE7 Lean",
            Email = "Lean365@outlook.com",
            Url = new Uri("https://gitee.com/lean365/LeanCodeGen")
          },
          License = new OpenApiLicense
          {
            Name = "\uD83D\uDC68\u200D\uD83D\uDCBB Lean365",
            Url = new Uri("https://gitee.com/lean365")
          }
        });
      }

      // 2. 配置分组
      options.DocInclusionPredicate((docName, apiDesc) =>
      {
        if (!apiDesc.TryGetMethodInfo(out var methodInfo)) return false;

        var groupName = methodInfo.DeclaringType?
            .GetCustomAttributes(true)
            .OfType<ApiExplorerSettingsAttribute>()
            .Select(attr => attr.GroupName)
            .FirstOrDefault();

        return docName == groupName;
      });

      // 3. JWT认证配置
      options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
      {
        Description = "JWT Authorization header using the Bearer scheme.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
      });

      options.AddSecurityRequirement(new OpenApiSecurityRequirement
      {
        {
          new OpenApiSecurityScheme
          {
            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
          },
          Array.Empty<string>()
        }
      });

      // 4. XML注释
      var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
      var xmlNames = new[] { "Lean.CodeGen.WebApi.xml", "Lean.CodeGen.Application.xml", "Lean.CodeGen.Domain.xml" };
      foreach (var xmlName in xmlNames)
      {
        var xmlPath = Path.Combine(baseDirectory, xmlName);
        if (File.Exists(xmlPath))
        {
          options.IncludeXmlComments(xmlPath, true);
        }
      }
    });

    return services;
  }

  /// <summary>
  /// 使用 Swagger
  /// </summary>
  public static IApplicationBuilder UseLeanSwagger(this IApplicationBuilder app)
  {
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
      // 配置文档端点
      options.SwaggerEndpoint("/swagger/identity/swagger.json", "\uD83D\uDCDD 身份认证");
      options.SwaggerEndpoint("/swagger/admin/swagger.json", "\uD83D\uDCDD 系统管理");
      options.SwaggerEndpoint("/swagger/generator/swagger.json", "\uD83D\uDCDD 代码生成");
      options.SwaggerEndpoint("/swagger/workflow/swagger.json", "\uD83D\uDCDD 工作流程");
      options.SwaggerEndpoint("/swagger/audit/swagger.json", "\uD83D\uDCDD 审计日志");
      options.SwaggerEndpoint("/swagger/signalr/swagger.json", "\uD83D\uDCDD 实时通信");

      options.RoutePrefix = "swagger";
    });

    return app;
  }
}
