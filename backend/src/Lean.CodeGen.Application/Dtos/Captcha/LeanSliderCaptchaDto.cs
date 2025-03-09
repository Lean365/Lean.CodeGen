using System.ComponentModel.DataAnnotations;

namespace Lean.CodeGen.Application.Dtos.Captcha;

/// <summary>
/// 滑块验证码请求
/// </summary>
public class LeanSliderCaptchaRequestDto
{
  /// <summary>
  /// 验证码键
  /// </summary>
  [Required(ErrorMessage = "验证码键不能为空")]
  public string CaptchaKey { get; set; } = default!;

  /// <summary>
  /// 滑动位置X坐标
  /// </summary>
  [Required(ErrorMessage = "滑动位置不能为空")]
  public int X { get; set; }

  /// <summary>
  /// 滑动位置Y坐标
  /// </summary>
  [Required(ErrorMessage = "滑动位置不能为空")]
  public int Y { get; set; }
}

/// <summary>
/// 滑块验证码响应
/// </summary>
public class LeanSliderCaptchaResponseDto
{
  /// <summary>
  /// 验证码键
  /// </summary>
  public string CaptchaKey { get; set; } = default!;

  /// <summary>
  /// 背景图片（Base64）
  /// </summary>
  public string BackgroundImage { get; set; } = default!;

  /// <summary>
  /// 滑块图片（Base64）
  /// </summary>
  public string SliderImage { get; set; } = default!;

  /// <summary>
  /// 滑块初始位置Y坐标
  /// </summary>
  public int Y { get; set; }

  /// <summary>
  /// 图片宽度
  /// </summary>
  public int Width { get; set; }

  /// <summary>
  /// 图片高度
  /// </summary>
  public int Height { get; set; }
}