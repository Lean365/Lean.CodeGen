using Microsoft.AspNetCore.Mvc;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Application.Services.Admin;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using Microsoft.Extensions.Configuration;

namespace Lean.CodeGen.WebApi.Controllers;

/// <summary>
/// 控制器基类
/// </summary>
[ApiController]
[Route("api/[controller]")]
public abstract class LeanBaseController : ControllerBase
{
  /// <summary>
  /// 本地化服务
  /// </summary>
  protected readonly ILeanLocalizationService LocalizationService;

  /// <summary>
  /// 配置
  /// </summary>
  protected readonly IConfiguration Configuration;

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="localizationService">本地化服务</param>
  /// <param name="configuration">配置</param>
  protected LeanBaseController(
      ILeanLocalizationService localizationService,
      IConfiguration configuration)
  {
    LocalizationService = localizationService;
    Configuration = configuration;
  }

  /// <summary>
  /// 获取当前用户界面语言
  /// </summary>
  protected virtual string GetCurrentUiLanguage()
  {
    // 1. 从Cookie获取
    var langCookie = Request.Cookies["lang"];
    if (!string.IsNullOrEmpty(langCookie))
    {
      return langCookie;
    }

    // 2. 从Accept-Language头获取
    var acceptLanguages = Request.GetTypedHeaders().AcceptLanguage;
    if (acceptLanguages != null && acceptLanguages.Count > 0)
    {
      return acceptLanguages.First().Value.ToString();
    }

    // 3. 从配置文件获取
    var configLang = Configuration["DefaultLanguage"];
    if (!string.IsNullOrEmpty(configLang))
    {
      return configLang;
    }

    // 4. 从服务器系统语言获取
    var currentCulture = CultureInfo.CurrentCulture;
    return currentCulture.Name;
  }

  /// <summary>
  /// 设置当前用户界面语言
  /// </summary>
  protected virtual void SetCurrentUiLanguage(string langCode)
  {
    var cookieOptions = new CookieOptions
    {
      HttpOnly = true,
      Expires = DateTime.UtcNow.AddDays(30)
    };

    Response.Cookies.Append("lang", langCode, cookieOptions);
  }

  /// <summary>
  /// 获取系统语言代码
  /// </summary>
  /// <returns>系统语言代码</returns>
  protected virtual string GetSystemLanguage()
  {
    // 1. 从配置文件获取
    var configLang = Configuration["SystemLanguage"];
    if (!string.IsNullOrEmpty(configLang))
    {
      return configLang;
    }

    // 2. 从服务器系统语言获取
    var currentCulture = CultureInfo.CurrentCulture;
    return currentCulture.Name;
  }

  /// <summary>
  /// 获取用户界面翻译
  /// </summary>
  /// <param name="key">翻译键</param>
  /// <param name="langCode">语言代码，如果为null则使用当前用户界面语言</param>
  /// <returns>翻译值</returns>
  protected async Task<string> GetUiTranslationAsync(string key, string? langCode = null)
  {
    return await LocalizationService.GetTranslationAsync(langCode ?? GetCurrentUiLanguage(), key);
  }

  /// <summary>
  /// 获取系统翻译（用于日志和系统消息）
  /// </summary>
  /// <param name="key">翻译键</param>
  /// <returns>翻译值</returns>
  protected async Task<string> GetSystemTranslationAsync(string key)
  {
    return await LocalizationService.GetTranslationAsync(GetSystemLanguage(), key);
  }

  /// <summary>
  /// 获取翻译
  /// </summary>
  /// <param name="key">翻译键</param>
  /// <param name="langCode">语言代码，如果为null则使用当前语言</param>
  /// <returns>翻译值</returns>
  protected async Task<string> GetTranslationAsync(string key, string? langCode = null)
  {
    return await LocalizationService.GetTranslationAsync(langCode ?? GetCurrentUiLanguage(), key);
  }

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
  /// 返回成功结果
  /// </summary>
  protected IActionResult Success(object? data = null, LeanBusinessType businessType = LeanBusinessType.Other)
  {
    if (data == null)
    {
      return Ok(LeanApiResult.Ok(businessType));
    }
    return Ok(LeanApiResult<object>.Ok(data, businessType));
  }

  /// <summary>
  /// 返回成功结果（泛型版本）
  /// </summary>
  protected IActionResult Success<T>(T data, LeanBusinessType businessType = LeanBusinessType.Other)
  {
    return Ok(LeanApiResult<T>.Ok(data, businessType));
  }

  /// <summary>
  /// 返回错误结果
  /// </summary>
  protected async Task<IActionResult> ErrorAsync(string messageKey, LeanErrorCode errorCode = LeanErrorCode.UnknownError)
  {
    var message = await GetTranslationAsync(messageKey);
    return BadRequest(LeanApiResult.Error(message, errorCode));
  }

  /// <summary>
  /// 返回未找到结果
  /// </summary>
  protected async Task<IActionResult> NotFoundAsync(string messageKey = "common.error.not_found", LeanBusinessType businessType = LeanBusinessType.Other)
  {
    var message = await GetUiTranslationAsync(messageKey);
    var result = LeanApiResult.Error(message, LeanErrorCode.NotFound, businessType);
    return NotFound(result);
  }

  /// <summary>
  /// 返回未授权结果
  /// </summary>
  protected async Task<IActionResult> UnauthorizedAsync(string messageKey = "common.error.unauthorized", LeanBusinessType businessType = LeanBusinessType.Other)
  {
    var message = await GetUiTranslationAsync(messageKey);
    var result = LeanApiResult.Error(message, LeanErrorCode.Unauthorized, businessType);
    return StatusCode(401, result);
  }

  /// <summary>
  /// 返回禁止访问结果
  /// </summary>
  protected async Task<IActionResult> ForbiddenAsync(string messageKey = "common.error.forbidden", LeanBusinessType businessType = LeanBusinessType.Other)
  {
    var message = await GetUiTranslationAsync(messageKey);
    var result = LeanApiResult.Error(message, LeanErrorCode.Forbidden, businessType);
    return StatusCode(403, result);
  }

  /// <summary>
  /// 返回服务器错误结果
  /// </summary>
  protected async Task<IActionResult> ServerErrorAsync(string messageKey = "common.error.server_error", LeanBusinessType businessType = LeanBusinessType.Other)
  {
    var message = await GetUiTranslationAsync(messageKey);
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

  protected IActionResult Error(string message, LeanErrorCode code = LeanErrorCode.SystemError, LeanBusinessType businessType = LeanBusinessType.Other)
  {
    return ApiResult(LeanApiResult.Error(message, code, businessType));
  }
}