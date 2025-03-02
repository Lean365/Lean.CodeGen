using Lean.CodeGen.Common.Helpers;
using Microsoft.Extensions.Hosting;

namespace Lean.CodeGen.WebApi.Services;

/// <summary>
/// 滑块验证码初始化服务
/// </summary>
public class LeanSliderCaptchaInitializer : IHostedService
{
  private readonly LeanSliderCaptchaHelper _captchaHelper;
  private readonly ILogger<LeanSliderCaptchaInitializer> _logger;

  public LeanSliderCaptchaInitializer(
      LeanSliderCaptchaHelper captchaHelper,
      ILogger<LeanSliderCaptchaInitializer> logger)
  {
    _captchaHelper = captchaHelper;
    _logger = logger;
  }

  public async Task StartAsync(CancellationToken cancellationToken)
  {
    try
    {
      _logger.LogInformation("开始初始化滑块验证码图片...");
      await _captchaHelper.InitializeAsync();
      _logger.LogInformation("滑块验证码图片初始化完成");
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "初始化滑块验证码图片时发生错误");
      throw;
    }
  }

  public Task StopAsync(CancellationToken cancellationToken)
  {
    return Task.CompletedTask;
  }
}