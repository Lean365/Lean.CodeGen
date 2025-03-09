using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Lean.CodeGen.WebApi.Filters;

/// <summary>
/// Swagger 操作过滤器
/// </summary>
public class LeanSwaggerOperationFilter : IOperationFilter
{
  /// <summary>
  /// 应用过滤器
  /// </summary>
  public void Apply(OpenApiOperation operation, OperationFilterContext context)
  {
    // 添加通用响应
    operation.Responses.TryAdd("400", new OpenApiResponse { Description = "请求参数错误" });
    operation.Responses.TryAdd("401", new OpenApiResponse { Description = "未授权" });
    operation.Responses.TryAdd("403", new OpenApiResponse { Description = "禁止访问" });
    operation.Responses.TryAdd("500", new OpenApiResponse { Description = "服务器内部错误" });
  }
}