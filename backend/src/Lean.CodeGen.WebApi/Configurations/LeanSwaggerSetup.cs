// -----------------------------------------------------------------------
// <copyright file="LeanSwaggerSetup.cs" company="Lean">
// Copyright (c) Lean. All rights reserved.
// </copyright>
// <author>Lean</author>
// <created>2024-03-08</created>
// <summary>Swagger API文档配置</summary>
// -----------------------------------------------------------------------

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Mvc.Controllers;
using NLog;
using Lean.CodeGen.WebApi.Filters;

namespace Lean.CodeGen.WebApi.Configurations;

/// <summary>
/// Swagger API文档配置类
/// </summary>
/// <remarks>
/// 提供Swagger文档的配置和初始化，包括：
/// 1. API分组配置
/// 2. JWT认证配置
/// 3. XML注释配置
/// 4. UI界面配置
/// </remarks>
public static class LeanSwaggerSetup
{
  /// <summary>
  /// 日志记录器实例
  /// </summary>
  private static readonly NLog.ILogger _logger = LogManager.GetLogger("LeanLog");

  /// <summary>
  /// 添加Swagger服务到依赖注入容器
  /// </summary>
  /// <param name="services">服务集合</param>
  /// <returns>配置后的服务集合</returns>
  /// <remarks>
  /// 此方法配置Swagger服务，包括：
  /// 1. 添加API文档信息（标题、版本、描述等）
  /// 2. 配置API分组（身份认证、系统管理等模块）
  /// 3. 配置JWT认证
  /// 4. 加载XML注释文件
  /// </remarks>
  public static IServiceCollection AddLeanSwagger(this IServiceCollection services)
  {
    services.AddSwaggerGen(options =>
    {
      try
      {
        // 1. 添加API文档信息
        var docs = new[]
        {
          new { Name = "admin", Title = "系统管理", Description = "系统管理相关接口" },
          new { Name = "audit", Title = "审计日志", Description = "审计、操作日志相关接口" },
          new { Name = "controlling", Title = "成本会计", Description = "成本核算、预算控制、分析等接口" },
          new { Name = "finance", Title = "财务管理", Description = "财务账务、报表、收支等接口" },
          new { Name = "generator", Title = "代码生成", Description = "代码生成相关接口" },
          new { Name = "identity", Title = "身份认证", Description = "用户认证、授权等接口" },
          new { Name = "material", Title = "物料管理", Description = "物料、库存、出入库等接口" },
          new { Name = "production", Title = "生产管理", Description = "生产计划、执行、监控等接口" },
          new { Name = "quality", Title = "质量管理", Description = "质量检验、控制、追溯等接口" },
          new { Name = "routine", Title = "日常事务", Description = "日常事务处理相关接口" },
          new { Name = "sales", Title = "销售管理", Description = "销售订单、客户管理等接口" },
          new { Name = "signalr", Title = "实时通信", Description = "SignalR实时通信相关接口" },
          new { Name = "workflow", Title = "工作流程", Description = "工作流程相关接口" }
        };

        foreach (var doc in docs)
        {
          _logger.Info($"正在配置 {doc.Title} 的 API 文档...");
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
          _logger.Info($"成功配置 {doc.Title} 的 API 文档");
        }

        // 2. 配置API分组规则
        options.DocInclusionPredicate((docName, apiDesc) =>
        {
          try
          {
            if (!apiDesc.TryGetMethodInfo(out var methodInfo))
            {
              return false;
            }

            var controllerActionDescriptor = apiDesc.ActionDescriptor as ControllerActionDescriptor;
            if (controllerActionDescriptor == null)
            {
              return false;
            }

            var groupName = controllerActionDescriptor.ControllerTypeInfo
                .GetCustomAttributes(true)
                .OfType<ApiExplorerSettingsAttribute>()
                .Select(attr => attr.GroupName)
                .FirstOrDefault();

            if (string.IsNullOrEmpty(groupName))
            {
              return false;
            }

            return docName == groupName;
          }
          catch (Exception ex)
          {
            _logger.Error(ex, "API分组规则处理出错");
            return false;
          }
        });

        // 3. 配置JWT认证
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
          Description = "JWT授权头，使用Bearer方案。示例: \"Authorization: Bearer {token}\"",
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

        // 4. 加载XML注释文件
        var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        _logger.Info($"基础目录: {baseDirectory}");
        var xmlNames = new[] { "Lean.CodeGen.WebApi.xml", "Lean.CodeGen.Application.xml", "Lean.CodeGen.Domain.xml" };

        foreach (var xmlName in xmlNames)
        {
          var xmlPath = Path.Combine(baseDirectory, xmlName);
          try
          {
            if (File.Exists(xmlPath))
            {
              _logger.Info($"正在加载 XML 文档文件: {xmlPath}");
              options.IncludeXmlComments(xmlPath, true);
              _logger.Info($"成功加载 XML 文档文件: {xmlPath}");
            }
            else
            {
              _logger.Warn($"未找到 XML 文档文件: {xmlPath}，文件路径: {xmlPath}");
              // 尝试在其他可能的位置查找
              var altPath = Path.Combine(Directory.GetCurrentDirectory(), xmlName);
              if (File.Exists(altPath))
              {
                _logger.Info($"在备选位置找到 XML 文档文件: {altPath}");
                options.IncludeXmlComments(altPath, true);
                _logger.Info($"成功加载备选位置的 XML 文档文件: {altPath}");
              }
            }
          }
          catch (Exception ex)
          {
            _logger.Error(ex, $"加载 XML 文档文件时出错: {xmlPath}");
          }
        }

        // 5. 启用注解
        options.EnableAnnotations();

        // 6. 配置文件上传支持
        options.MapType<IFormFile>(() => new OpenApiSchema
        {
          Type = "file",
          Format = "binary"
        });

        // 添加对文件上传的支持
        options.OperationFilter<LeanFileUploadOperationFilter>();

        // 7. 配置操作过滤器
        options.OperationFilter<LeanSwaggerOperationFilter>();

        // 8. 自定义 Swagger 文档
        options.DocumentFilter<LeanSwaggerDocumentFilter>();

      }
      catch (Exception ex)
      {
        _logger.Error(ex, "Swagger 配置过程中发生错误");
        throw;
      }
    });

    return services;
  }

  /// <summary>
  /// 配置Swagger中间件
  /// </summary>
  /// <param name="app">应用程序构建器</param>
  /// <returns>配置后的应用程序构建器</returns>
  /// <remarks>
  /// 此方法配置Swagger中间件，包括：
  /// 1. 启用Swagger中间件
  /// 2. 配置SwaggerUI界面
  /// 3. 设置各模块的文档访问端点
  /// </remarks>
  public static IApplicationBuilder UseLeanSwagger(this IApplicationBuilder app)
  {
    try
    {
      app.UseSwagger(c =>
      {
        c.SerializeAsV2 = false;
        c.RouteTemplate = "swagger/{documentName}/swagger.json";
      });

      app.UseSwaggerUI(options =>
      {
        // 系统功能
        options.SwaggerEndpoint("/swagger/identity/swagger.json", "\uD83D\uDCDD 身份认证");
        options.SwaggerEndpoint("/swagger/admin/swagger.json", "\uD83D\uDCDD 系统管理");
        options.SwaggerEndpoint("/swagger/audit/swagger.json", "\uD83D\uDCDD 审计日志");
        options.SwaggerEndpoint("/swagger/generator/swagger.json", "\uD83D\uDCDD 代码生成");
        options.SwaggerEndpoint("/swagger/signalr/swagger.json", "\uD83D\uDCDD 实时通信");

        // 业务功能
        options.SwaggerEndpoint("/swagger/production/swagger.json", "\uD83D\uDCDD 生产管理");
        options.SwaggerEndpoint("/swagger/material/swagger.json", "\uD83D\uDCDD 物料管理");
        options.SwaggerEndpoint("/swagger/quality/swagger.json", "\uD83D\uDCDD 质量管理");
        options.SwaggerEndpoint("/swagger/sales/swagger.json", "\uD83D\uDCDD 销售管理");

        // 财务功能
        options.SwaggerEndpoint("/swagger/controlling/swagger.json", "\uD83D\uDCDD 成本会计");
        options.SwaggerEndpoint("/swagger/finance/swagger.json", "\uD83D\uDCDD 财务管理");

        // 流程功能
        options.SwaggerEndpoint("/swagger/workflow/swagger.json", "\uD83D\uDCDD 工作流程");
        options.SwaggerEndpoint("/swagger/routine/swagger.json", "\uD83D\uDCDD 日常事务");

        options.RoutePrefix = "swagger";
        options.DocumentTitle = "Lean.CodeGen API 文档";
        options.DefaultModelExpandDepth(2);
        options.DefaultModelRendering(Swashbuckle.AspNetCore.SwaggerUI.ModelRendering.Model);
        options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
        options.DisplayRequestDuration();
        options.EnableDeepLinking();
        options.EnableFilter();
      });

      return app;
    }
    catch (Exception ex)
    {
      _logger.Error(ex, "配置 Swagger 中间件时发生错误");
      throw;
    }
  }
}
