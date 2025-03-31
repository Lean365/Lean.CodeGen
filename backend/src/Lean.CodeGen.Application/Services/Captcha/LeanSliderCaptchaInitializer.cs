using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using NLog;
using Lean.CodeGen.Common.Helpers;
namespace Lean.CodeGen.WebApi.Services;

/// <summary>
/// 滑块验证码初始化服务
/// </summary>
public class LeanSliderCaptchaInitializer : BackgroundService
{
  private readonly ILogger _logger;
  private readonly LeanSliderCaptchaHelper _helper;

  public LeanSliderCaptchaInitializer(
      LeanSliderCaptchaHelper helper)
  {
    _helper = helper;
    _logger = LogManager.GetLogger("LeanLog");
  }

  protected override async Task ExecuteAsync(CancellationToken stoppingToken)
  {
    try
    {
      _logger.Info("开始初始化滑块验证码图片...");
      await _helper.InitializeAsync();
      _logger.Info("滑块验证码图片初始化完成");
    }
    catch (Exception ex)
    {
      _logger.Error(ex, "初始化滑块验证码图片时发生错误");
      throw;
    }
  }
}