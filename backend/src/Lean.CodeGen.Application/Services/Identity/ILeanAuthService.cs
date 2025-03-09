using Lean.CodeGen.Application.Dtos.Identity;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Application.Dtos.Captcha;

namespace Lean.CodeGen.Application.Services.Identity;

/// <summary>
/// 认证服务接口
/// </summary>
public interface ILeanAuthService
{
  /// <summary>
  /// 登录
  /// </summary>
  /// <param name="request">登录请求</param>
  /// <returns>登录响应</returns>
  Task<LeanApiResult<LeanLoginResponseDto>> LoginAsync(LeanLoginRequestDto request);

  /// <summary>
  /// 获取滑块验证码
  /// </summary>
  /// <returns>滑块验证码信息</returns>
  Task<LeanApiResult<LeanSliderCaptchaResponseDto>> GetSliderCaptchaAsync();

  /// <summary>
  /// 验证滑块验证码
  /// </summary>
  /// <param name="request">验证请求</param>
  /// <returns>验证结果</returns>
  Task<LeanApiResult> ValidateSliderCaptchaAsync(LeanSliderCaptchaRequestDto request);

  /// <summary>
  /// 登出
  /// </summary>
  /// <returns>操作结果</returns>
  Task<LeanApiResult> LogoutAsync();
}