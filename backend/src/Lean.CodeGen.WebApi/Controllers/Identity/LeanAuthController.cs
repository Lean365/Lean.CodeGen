using Microsoft.AspNetCore.Mvc;
using Lean.CodeGen.Application.Dtos.Identity;
using Lean.CodeGen.Application.Services.Identity;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Common.Attributes;
using Microsoft.Extensions.Configuration;
using Lean.CodeGen.Application.Services.Admin;
using Lean.CodeGen.Application.Dtos.Captcha;

namespace Lean.CodeGen.WebApi.Controllers.Identity;

/// <summary>
/// 认证控制器
/// </summary>
[ApiController]
[Route("api/[controller]")]
[ApiExplorerSettings(GroupName = "identity")]
public class LeanAuthController : LeanBaseController
{
  private readonly ILeanAuthService _authService;

  /// <summary>
  /// 构造函数
  /// </summary>
  public LeanAuthController(
      ILeanAuthService authService,
      ILeanLocalizationService localizationService,
      IConfiguration configuration)
      : base(localizationService, configuration)
  {
    _authService = authService;
  }

  /// <summary>
  /// 用户登录
  /// </summary>
  /// <param name="request">登录请求参数</param>
  /// <returns>登录结果</returns>
  [HttpPost("login")]
  [AllowAnonymous]
  public async Task<IActionResult> LoginAsync([FromBody] LeanLoginRequestDto request)
  {
    var result = await _authService.LoginAsync(request);
    if (!result.Success)
    {
      return await ErrorAsync(result.Message ?? "登录失败");
    }
    return Success(result.Data, LeanBusinessType.Other);
  }

  /// <summary>
  /// 获取滑块验证码
  /// </summary>
  /// <returns>滑块验证码信息</returns>
  [HttpGet("captcha/slider")]
  [AllowAnonymous]
  public async Task<IActionResult> GetSliderCaptchaAsync()
  {
    var result = await _authService.GetSliderCaptchaAsync();
    if (!result.Success)
    {
      return await ErrorAsync(result.Message ?? "获取验证码失败");
    }
    return Success(result.Data, LeanBusinessType.Other);
  }

  /// <summary>
  /// 验证滑块验证码
  /// </summary>
  /// <param name="request">验证请求参数</param>
  /// <returns>验证结果</returns>
  [HttpPost("captcha/slider/validate")]
  [AllowAnonymous]
  public async Task<IActionResult> ValidateSliderCaptchaAsync([FromBody] LeanSliderCaptchaRequestDto request)
  {
    var result = await _authService.ValidateSliderCaptchaAsync(request);
    if (!result.Success)
    {
      return await ErrorAsync(result.Message ?? "验证失败");
    }
    return Success(null, LeanBusinessType.Other);
  }

  /// <summary>
  /// 用户登出
  /// </summary>
  /// <returns>登出结果</returns>
  [HttpPost("logout")]
  public async Task<IActionResult> LogoutAsync()
  {
    var result = await _authService.LogoutAsync();
    if (!result.Success)
    {
      return await ErrorAsync(result.Message ?? "登出失败");
    }
    return Success(null, LeanBusinessType.Other);
  }
}