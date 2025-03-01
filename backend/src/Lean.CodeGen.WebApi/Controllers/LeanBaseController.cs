using Microsoft.AspNetCore.Mvc;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Common.Enums;

namespace Lean.CodeGen.WebApi.Controllers;

/// <summary>
/// 控制器基类
/// </summary>
[ApiController]
[Route("api/[controller]")]
public abstract class LeanBaseController : ControllerBase
{
  /// <summary>
  /// 处理API结果（泛型版本）
  /// </summary>
  protected IActionResult ApiResult<T>(LeanApiResult<T> result)
  {
    if (result == null)
    {
      return StatusCode((int)LeanHttpStatusCode.BadRequest, "无效的API响应");
    }

    return result.Success
      ? StatusCode((int)LeanHttpStatusCode.OK, result.Data)
      : StatusCode((int)LeanHttpStatusCode.BadRequest, result.Message);
  }

  /// <summary>
  /// 处理API结果（无数据版本）
  /// </summary>
  protected IActionResult ApiResult(LeanApiResult result)
  {
    if (result == null)
    {
      return StatusCode((int)LeanHttpStatusCode.BadRequest, "无效的API响应");
    }

    return result.Success
      ? StatusCode((int)LeanHttpStatusCode.OK)
      : StatusCode((int)LeanHttpStatusCode.BadRequest, result.Message);
  }

  /// <summary>
  /// 处理文件下载结果
  /// </summary>
  protected IActionResult FileResult<T>(LeanApiResult<T> result, string fileName, string contentType = "application/octet-stream")
  {
    if (result == null)
    {
      return StatusCode((int)LeanHttpStatusCode.BadRequest, "无效的文件下载响应");
    }

    if (!result.Success)
    {
      return StatusCode((int)LeanHttpStatusCode.BadRequest, result.Message);
    }

    if (result.Data == null)
    {
      return StatusCode((int)LeanHttpStatusCode.NotFound, "文件内容为空");
    }

    // 处理字节数组类型
    if (result.Data is byte[] fileBytes)
    {
      return File(fileBytes, contentType, fileName, true);
    }

    return StatusCode((int)LeanHttpStatusCode.UnsupportedMediaType, "不支持的文件类型");
  }

  /// <summary>
  /// 返回成功结果（带数据）
  /// </summary>
  protected IActionResult Success<T>(T data, LeanBusinessType businessType = LeanBusinessType.Other)
  {
    var result = LeanApiResult<T>.Ok(data, businessType);
    return Ok(result);
  }

  /// <summary>
  /// 返回成功结果（无数据）
  /// </summary>
  protected IActionResult Success(LeanBusinessType businessType = LeanBusinessType.Other)
  {
    var result = LeanApiResult.Ok(businessType);
    return Ok(result);
  }

  /// <summary>
  /// 返回错误结果
  /// </summary>
  protected IActionResult Error(string message, LeanErrorCode code = LeanErrorCode.SystemError, LeanBusinessType businessType = LeanBusinessType.Other)
  {
    var result = LeanApiResult.Error(message, code, businessType);
    return BadRequest(result);
  }

  /// <summary>
  /// 返回未找到结果
  /// </summary>
  protected IActionResult NotFound(string message = "未找到请求的资源", LeanBusinessType businessType = LeanBusinessType.Other)
  {
    var result = LeanApiResult.Error(message, LeanErrorCode.NotFound, businessType);
    return NotFound(result);
  }

  /// <summary>
  /// 返回未授权结果
  /// </summary>
  protected IActionResult Unauthorized(string message = "未经授权的访问", LeanBusinessType businessType = LeanBusinessType.Other)
  {
    var result = LeanApiResult.Error(message, LeanErrorCode.Unauthorized, businessType);
    return StatusCode(401, result);
  }

  /// <summary>
  /// 返回禁止访问结果
  /// </summary>
  protected IActionResult Forbidden(string message = "禁止访问", LeanBusinessType businessType = LeanBusinessType.Other)
  {
    var result = LeanApiResult.Error(message, LeanErrorCode.Forbidden, businessType);
    return StatusCode(403, result);
  }

  /// <summary>
  /// 返回服务器错误结果
  /// </summary>
  protected IActionResult ServerError(string message = "服务器内部错误", LeanBusinessType businessType = LeanBusinessType.Other)
  {
    var result = LeanApiResult.Error(message, LeanErrorCode.SystemError, businessType);
    return StatusCode(500, result);
  }

  /// <summary>
  /// 返回自定义状态码结果
  /// </summary>
  protected IActionResult StatusResult(LeanHttpStatusCode statusCode, object? data = null)
  {
    return StatusCode((int)statusCode, data);
  }

  /// <summary>
  /// 返回文件下载结果
  /// </summary>
  protected IActionResult FileDownload(byte[] fileBytes, string fileName, string contentType = "application/octet-stream")
  {
    if (fileBytes == null || fileBytes.Length == 0)
    {
      return NotFound("文件内容为空");
    }
    return File(fileBytes, contentType, fileName);
  }
}