using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Lean.CodeGen.WebApi.Configurations;

/// <summary>
/// Swagger配置
/// </summary>
public static class LeanSwaggerSetup
{
  public static void AddLeanSwagger(this IServiceCollection services)
  {
    services.AddSwaggerGen(c =>
    {
      c.SwaggerDoc("v1", new OpenApiInfo
      {
        Title = "Lean.CodeGen API",
        Version = "v1",
        Description = "代码生成服务接口文档",
        Contact = new OpenApiContact
        {
          Name = "Lean",
          Email = "lean@example.com"
        }
      });

      // 添加JWT认证配置
      c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
      {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
      });

      c.AddSecurityRequirement(new OpenApiSecurityRequirement
      {
        {
          new OpenApiSecurityScheme
          {
            Reference = new OpenApiReference
            {
              Type = ReferenceType.SecurityScheme,
              Id = "Bearer"
            }
          },
          Array.Empty<string>()
        }
      });

      // 启用注解
      c.EnableAnnotations();

      // 添加XML注释
      var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
      var xmlFiles = Directory.GetFiles(baseDirectory, "Lean.CodeGen.*.xml");
      foreach (var xmlFile in xmlFiles)
      {
        c.IncludeXmlComments(xmlFile);
      }

      // 自定义配置
      c.OrderActionsBy(apiDesc => $"{apiDesc.ActionDescriptor.RouteValues["controller"]}_{apiDesc.RelativePath}");
      c.DocInclusionPredicate((docName, apiDesc) => true);
      c.UseInlineDefinitionsForEnums();
    });
  }

  public static void UseLeanSwagger(this IApplicationBuilder app)
  {
    app.UseSwagger(c =>
    {
      c.RouteTemplate = "swagger/{documentName}/swagger.json";
      c.SerializeAsV2 = false;
    });

    app.UseSwaggerUI(c =>
    {
      c.SwaggerEndpoint("/swagger/v1/swagger.json", "Lean.CodeGen API V1");
      c.RoutePrefix = "swagger";
      c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
      c.EnableFilter();
      c.EnableDeepLinking();
      c.DisplayRequestDuration();
    });
  }
}