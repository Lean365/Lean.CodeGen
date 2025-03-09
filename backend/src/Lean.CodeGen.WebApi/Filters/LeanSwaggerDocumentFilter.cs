using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Lean.CodeGen.WebApi.Filters;

/// <summary>
/// Swagger 文档过滤器
/// </summary>
public class LeanSwaggerDocumentFilter : IDocumentFilter
{
  /// <summary>
  /// 应用过滤器
  /// </summary>
  public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
  {
    // 对所有路径进行排序
    var paths = swaggerDoc.Paths.OrderBy(e => e.Key).ToList();
    swaggerDoc.Paths.Clear();
    paths.ForEach(x => swaggerDoc.Paths.Add(x.Key, x.Value));

    // 添加全局标签
    swaggerDoc.Tags = swaggerDoc.Tags?
        .OrderBy(x => x.Name)
        .ToList();
  }
}