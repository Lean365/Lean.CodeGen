using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Security.Cryptography;
using System.Net.Http;
using Path = System.IO.Path;
using Lean.CodeGen.Common.Options;
using Microsoft.Extensions.Options;
using System.Numerics;
using System.Threading.Tasks;
using System.Threading;

namespace Lean.CodeGen.Common.Helpers;

/// <summary>
/// 滑块验证码帮助类
/// </summary>
/// <remarks>
/// 该类用于生成和验证滑块验证码，主要功能包括：
/// 1. 生成带有滑块缺口的背景图片
/// 2. 生成可拖动的滑块图片
/// 3. 验证滑块拖动位置是否正确
/// 4. 生成验证码唯一标识键
/// </remarks>
public class LeanSliderCaptchaHelper
{
  private readonly HttpClient _httpClient;
  private readonly string _webRootPath;
  private readonly LeanSliderCaptchaOptions _options;
  private readonly Random _random;
  private const int TEMPLATE_COUNT = 5;  // 模板数量

  /// <summary>
  /// 初始化滑块验证码帮助类
  /// </summary>
  /// <param name="webRootPath">网站根目录路径</param>
  /// <param name="options">验证码配置选项</param>
  public LeanSliderCaptchaHelper(string webRootPath, IOptions<LeanSliderCaptchaOptions> options)
  {
    _webRootPath = webRootPath;
    _options = options.Value;
    _httpClient = new HttpClient();
    _random = new Random();
  }

  /// <summary>
  /// 初始化图片缓存
  /// </summary>
  public async Task InitializeAsync()
  {
    var backgroundDir = Path.Combine(_webRootPath, "images", "slider", "background");
    if (!Directory.Exists(backgroundDir))
    {
      Directory.CreateDirectory(backgroundDir);
    }

    // 清理旧的图片文件
    foreach (var file in Directory.GetFiles(backgroundDir, "*.jpg"))
    {
      try
      {
        File.Delete(file);
      }
      catch (Exception ex)
      {
        Console.WriteLine($"清理旧图片失败: {file}");
        Console.WriteLine($"错误: {ex.Message}");
      }
    }

    // 使用信号量限制并发下载数量
    using var semaphore = new SemaphoreSlim(2); // 最多同时下载2张图片
    var tasks = new List<Task>();
    var retryCount = 3; // 最大重试次数
    var retryDelay = TimeSpan.FromSeconds(2); // 增加重试延迟到2秒

    for (int i = 1; i <= _options.ImageCount; i++)
    {
      var index = i;
      var task = Task.Run(async () =>
      {
        var imagePath = Path.Combine(backgroundDir, $"image_{index}.jpg");
        for (int retry = 0; retry <= retryCount; retry++)
        {
          try
          {
            await semaphore.WaitAsync();
            await DownloadImageAsync(index, imagePath);
            break; // 下载成功，跳出重试循环
          }
          catch (Exception ex) when (retry < retryCount)
          {
            Console.WriteLine($"第 {index} 张图片下载失败，第 {retry + 1} 次重试");
            Console.WriteLine($"错误: {ex.Message}");
            await Task.Delay(retryDelay * (retry + 1)); // 递增重试延迟
          }
          finally
          {
            semaphore.Release();
          }
        }
      });
      tasks.Add(task);
    }

    try
    {
      await Task.WhenAll(tasks);
      Console.WriteLine("所有图片下载完成");
    }
    catch (Exception ex)
    {
      Console.WriteLine("部分图片下载失败");
      Console.WriteLine($"错误: {ex.Message}");
      throw;
    }
  }

  /// <summary>
  /// 下载图片
  /// </summary>
  private async Task DownloadImageAsync(int index, string savePath)
  {
    try
    {
      // 构建图片下载 URL
      var url = _options.Source.UrlTemplate
        .Replace("{width}", _options.Width.ToString())
        .Replace("{height}", _options.Height.ToString());

      Console.WriteLine($"开始下载第 {index} 张图片");
      Console.WriteLine($"下载URL: {url}");

      // 增加超时时间到30秒
      using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));

      // 下载图片
      using var response = await _httpClient.GetAsync(url, cts.Token);
      response.EnsureSuccessStatusCode();

      var contentType = response.Content.Headers.ContentType?.MediaType;
      Console.WriteLine($"Content-Type: {contentType}");

      // 验证内容类型
      if (string.IsNullOrEmpty(contentType) || !contentType.StartsWith("image/"))
      {
        throw new InvalidOperationException($"无效的内容类型: {contentType}");
      }

      // 下载并保存图片
      var imageBytes = await response.Content.ReadAsByteArrayAsync(cts.Token);
      Console.WriteLine($"图片大小: {imageBytes.Length} 字节");

      // 验证图片大小
      if (imageBytes.Length < 1024) // 小于1KB的图片可能是错误的
      {
        throw new InvalidOperationException($"图片大小异常: {imageBytes.Length} 字节");
      }

      await File.WriteAllBytesAsync(savePath, imageBytes, cts.Token);
      Console.WriteLine($"成功保存图片到: {savePath}");
    }
    catch (OperationCanceledException)
    {
      Console.WriteLine($"下载第 {index} 张图片超时");
      throw;
    }
    catch (HttpRequestException ex)
    {
      Console.WriteLine($"下载第 {index} 张图片失败");
      Console.WriteLine($"错误: {ex.Message}");
      Console.WriteLine($"状态码: {ex.StatusCode}");
      throw;
    }
    catch (Exception ex)
    {
      Console.WriteLine($"处理第 {index} 张图片时发生错误");
      Console.WriteLine($"错误类型: {ex.GetType().Name}");
      Console.WriteLine($"错误信息: {ex.Message}");
      throw;
    }
  }

  /// <summary>
  /// 获取随机背景图片路径
  /// </summary>
  private string GetRandomImagePath()
  {
    var backgroundDir = Path.Combine(_webRootPath, "images", "slider", "background");

    // 检查目录是否存在
    if (!Directory.Exists(backgroundDir))
    {
      throw new DirectoryNotFoundException($"背景图片目录不存在: {backgroundDir}");
    }

    var files = Directory.GetFiles(backgroundDir, "*.jpg");
    if (files.Length == 0)
    {
      throw new InvalidOperationException("没有可用的背景图片");
    }

    var selectedPath = files[_random.Next(files.Length)];
    Console.WriteLine($"选择背景图片: {selectedPath}");
    return selectedPath;
  }

  private (string sliderTemplatePath, string holeTemplatePath) GetRandomTemplate()
  {
    var templateIndex = _random.Next(1, TEMPLATE_COUNT + 1);
    var templateDir = Path.Combine(_webRootPath, "images", "slider", "template", templateIndex.ToString());

    // 检查目录是否存在
    if (!Directory.Exists(templateDir))
    {
      throw new DirectoryNotFoundException($"模板目录不存在: {templateDir}");
    }

    var sliderPath = Path.Combine(templateDir, "slider.png");
    var holePath = Path.Combine(templateDir, "hole.png");

    // 检查文件是否存在
    if (!File.Exists(sliderPath))
    {
      throw new FileNotFoundException($"滑块模板文件不存在: {sliderPath}");
    }
    if (!File.Exists(holePath))
    {
      throw new FileNotFoundException($"滑块缺口模板文件不存在: {holePath}");
    }

    Console.WriteLine($"使用模板目录: {templateDir}");
    Console.WriteLine($"滑块模板: {sliderPath}");
    Console.WriteLine($"缺口模板: {holePath}");

    return (sliderPath, holePath);
  }

  /// <summary>
  /// 生成滑块验证码
  /// </summary>
  /// <returns>
  /// 返回一个元组，包含：
  /// - backgroundImage: 带有滑块缺口的背景图片字节数组
  /// - sliderImage: 可拖动的滑块图片字节数组
  /// - targetX: 滑块正确位置的X坐标
  /// - targetY: 滑块正确位置的Y坐标
  /// </returns>
  /// <remarks>
  /// 生成过程包括：
  /// 1. 随机选择背景图片
  /// 2. 调整图片尺寸
  /// 3. 随机生成滑块位置
  /// 4. 创建不规则的滑块形状
  /// 5. 在背景图上绘制滑块遮罩
  /// 6. 生成滑块图片
  /// </remarks>
  public (byte[] backgroundImage, byte[] sliderImage, int targetX, int targetY) Generate()
  {
    try
    {
      Console.WriteLine($"WebRootPath: {_webRootPath}");

      // 1. 加载原始图片和模板
      var backgroundPath = GetRandomImagePath();
      Console.WriteLine($"完整背景图片路径: {backgroundPath}");
      Console.WriteLine($"背景图片是否存在: {File.Exists(backgroundPath)}");

      var (sliderTemplatePath, holeTemplatePath) = GetRandomTemplate();
      Console.WriteLine($"完整滑块模板路径: {sliderTemplatePath}");
      Console.WriteLine($"完整滑块缺口路径: {holeTemplatePath}");
      Console.WriteLine($"滑块模板是否存在: {File.Exists(sliderTemplatePath)}");
      Console.WriteLine($"滑块缺口是否存在: {File.Exists(holeTemplatePath)}");

      using var originalImage = Image.Load<Rgba32>(backgroundPath);
      using var sliderTemplate = Image.Load<Rgba32>(sliderTemplatePath);
      using var holeTemplate = Image.Load<Rgba32>(holeTemplatePath);

      // 调整背景图片尺寸
      using var backgroundImage = originalImage.Clone();
      backgroundImage.Mutate(ctx =>
      {
        ctx.Resize(new Size(_options.Width, _options.Height));
      });

      // 调整滑块模板尺寸
      sliderTemplate.Mutate(ctx =>
      {
        ctx.Resize(new Size(_options.SliderWidth, _options.SliderHeight));
      });

      // 调整hole模板尺寸
      holeTemplate.Mutate(ctx =>
      {
        ctx.Resize(new Size(_options.HoleWidth, _options.HoleHeight));
      });

      // 计算垂直中心位置（确保滑块和缺口在同一水平线上）
      int centerY = (_options.Height - _options.SliderHeight) / 2;

      // hole的位置（固定在右侧）
      int holeX = _options.Width - _options.HoleWidth - 50;  // 距离右边50px
      int holeY = centerY;  // 使用相同的centerY确保水平对齐

      // 在背景图上叠加hole
      backgroundImage.Mutate(ctx =>
      {
        ctx.DrawImage(holeTemplate, new Point(holeX, holeY), 1f);
      });

      // 保存图片
      using var backgroundStream = new MemoryStream();
      using var sliderStream = new MemoryStream();
      backgroundImage.SaveAsJpeg(backgroundStream);
      sliderTemplate.SaveAsPng(sliderStream);

      return (backgroundStream.ToArray(), sliderStream.ToArray(), holeX, holeY);
    }
    catch (Exception ex)
    {
      Console.WriteLine($"生成滑块验证码时发生错误: {ex.Message}");
      Console.WriteLine($"错误堆栈: {ex.StackTrace}");
      throw;
    }
  }

  /// <summary>
  /// 验证滑块位置是否正确
  /// </summary>
  /// <param name="x">用户滑动的X坐标</param>
  /// <param name="y">用户滑动的Y坐标</param>
  /// <param name="correctX">正确的X坐标</param>
  /// <param name="correctY">正确的Y坐标</param>
  /// <returns>如果用户滑动位置在允许的误差范围内，则返回true；否则返回false</returns>
  /// <remarks>
  /// 验证时会考虑一定的误差范围（Tolerance），使验证码更易于使用。
  /// 水平和垂直方向的误差都必须在允许范围内才算验证通过。
  /// </remarks>
  public (bool isValid, string message) Validate(int x, int y, int correctX, int correctY)
  {
    try
    {
      Console.WriteLine("收到滑块验证请求");
      var message = $"输入坐标: ({x}, {y}), 目标坐标: ({correctX}, {correctY})";
      Console.WriteLine(message);

      if (x < 0 || y < 0 || correctX < 0 || correctY < 0)
      {
        return (false, "坐标值不能为负数");
      }

      var xOffset = Math.Abs(x - correctX);
      var yOffset = Math.Abs(y - correctY);

      var offsetMessage = $"偏移量: X={xOffset}, Y={yOffset}, 允许误差={_options.Tolerance}";
      Console.WriteLine(offsetMessage);

      var result = xOffset <= _options.Tolerance && yOffset <= _options.Tolerance;
      var resultMessage = $"验证{(result ? "通过" : "失败")} - {offsetMessage}";
      Console.WriteLine(resultMessage);

      return (result, resultMessage);
    }
    catch (Exception ex)
    {
      var errorMessage = $"验证异常: {ex.Message}";
      Console.WriteLine(errorMessage);
      return (false, errorMessage);
    }
  }

  /// <summary>
  /// 生成验证码唯一标识键
  /// </summary>
  /// <returns>返回Base64编码的随机字符串，用作验证码的唯一标识</returns>
  /// <remarks>
  /// 使用加密安全的随机数生成器生成32字节的随机数，
  /// 然后将其转换为Base64字符串，确保唯一性和安全性。
  /// </remarks>
  public string GenerateKey()
  {
    var bytes = new byte[32];
    using var rng = RandomNumberGenerator.Create();
    rng.GetBytes(bytes);
    return Convert.ToBase64String(bytes);
  }
}