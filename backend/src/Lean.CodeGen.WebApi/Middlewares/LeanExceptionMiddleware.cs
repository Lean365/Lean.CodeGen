using System.Net;
using System.Text.Json;
using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Common.Exceptions;
using Lean.CodeGen.Common.Models;
using Microsoft.AspNetCore.Http;
 using NLog;

namespace Lean.CodeGen.WebApi.Middlewares;

/// <summary>
/// 全局异常处理中间件
/// </summary>
public class LeanExceptionMiddleware
{
  private readonly RequestDelegate _next;
  private readonly ILogger<LeanExceptionMiddleware> _logger;

  public LeanExceptionMiddleware(RequestDelegate next, ILogger<LeanExceptionMiddleware> logger)
  {
    _next = next;
    _logger = logger;
  }

  public async Task InvokeAsync(HttpContext context)
  {
    try
    {
      await _next(context);
    }
    catch (Exception ex)
    {
      await HandleExceptionAsync(context, ex);
    }
  }

  private async Task HandleExceptionAsync(HttpContext context, Exception exception)
  {
    context.Response.ContentType = "application/json";
    LeanApiResult<object?> result;

    switch (exception)
    {
      case LeanException ex:
        result = LeanApiResult<object?>.Error(ex.Message, ex.ErrorCode);
        context.Response.StatusCode = (int)HttpStatusCode.OK;
        break;

      case UnauthorizedAccessException:
        result = LeanApiResult<object?>.Error("未授权",  LeanErrorCode.Status401Unauthorized);
        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        break;

      default:
        _logger.LogError(exception, "系统错误");
        result = LeanApiResult<object?>.Error("系统错误", LeanErrorCode.SystemError);
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        break;
    }

    result.TraceId = context.TraceIdentifier;
    var json = JsonSerializer.Serialize(result);
    await context.Response.WriteAsync(json);
  }
}