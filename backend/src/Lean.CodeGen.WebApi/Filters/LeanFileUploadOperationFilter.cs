using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Lean.CodeGen.Common.Models;

namespace Lean.CodeGen.WebApi.Filters;

/// <summary>
/// 文件上传操作过滤器
/// </summary>
public class LeanFileUploadOperationFilter : IOperationFilter
{
  /// <summary>
  /// 应用过滤器
  /// </summary>
  /// <param name="operation">OpenAPI操作</param>
  /// <param name="context">操作过滤器上下文</param>
  public void Apply(OpenApiOperation operation, OperationFilterContext context)
  {
    var actionDescriptor = context.ApiDescription.ActionDescriptor as ControllerActionDescriptor;
    if (actionDescriptor == null) return;

    var fileParameters = actionDescriptor.Parameters
        .Where(p => p.ParameterType == typeof(IFormFile) || p.ParameterType == typeof(LeanFileInfo))
        .ToList();

    if (!fileParameters.Any()) return;

    // 移除文件参数的原始定义
    foreach (var fileParam in fileParameters)
    {
      var existingParam = operation.Parameters.FirstOrDefault(p => p.Name == fileParam.Name);
      if (existingParam != null)
      {
        operation.Parameters.Remove(existingParam);
      }
    }

    // 添加 multipart/form-data 请求体
    operation.RequestBody = new OpenApiRequestBody
    {
      Required = true,
      Content = new Dictionary<string, OpenApiMediaType>
      {
        {
          "multipart/form-data",
          new OpenApiMediaType
          {
            Schema = new OpenApiSchema
            {
              Type = "object",
              Properties = fileParameters.ToDictionary(
                param => param.Name,
                param => new OpenApiSchema
                {
                  Type = "string",
                  Format = "binary",
                  Description = GetParameterDescription(param)
                }
              ),
              Required = new HashSet<string>(fileParameters.Select(p => p.Name))
            }
          }
        }
      }
    };
  }

  /// <summary>
  /// 获取参数描述
  /// </summary>
  /// <param name="parameter">参数描述符</param>
  /// <returns>参数描述</returns>
  private string GetParameterDescription(ParameterDescriptor parameter)
  {
    return parameter.ParameterType == typeof(IFormFile) ? "选择要上传的文件" : "Excel文件";
  }
}