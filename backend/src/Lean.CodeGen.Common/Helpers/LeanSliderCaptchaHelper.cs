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
  }

  /// <summary>
  /// 初始化图片缓存
  /// </summary>
  public async Task InitializeAsync()
  {
    var cacheDir = Path.Combine(_webRootPath, _options.ImageCacheDir);
    if (!Directory.Exists(cacheDir))
    {
      Directory.CreateDirectory(cacheDir);
    }

    // 清理旧的图片文件
    foreach (var file in Directory.GetFiles(cacheDir, "*.jpg"))
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

    // 并行下载新的随机图片
    var tasks = new List<Task>();
    var retryCount = 3; // 最大重试次数
    var retryDelay = TimeSpan.FromSeconds(1); // 重试延迟

    for (int i = 1; i <= _options.ImageCount; i++)
    {
      var index = i;
      var task = Task.Run(async () =>
      {
        var imagePath = Path.Combine(cacheDir, $"image_{index}.jpg");
        for (int retry = 0; retry <= retryCount; retry++)
        {
          try
          {
            await DownloadImageAsync(index, imagePath);
            break; // 下载成功，跳出重试循环
          }
          catch (Exception ex) when (retry < retryCount)
          {
            Console.WriteLine($"第 {index} 张图片下载失败，第 {retry + 1} 次重试");
            Console.WriteLine($"错误: {ex.Message}");
            await Task.Delay(retryDelay);
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

      // 设置超时时间
      using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));

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
    var cacheDir = Path.Combine(_webRootPath, _options.ImageCacheDir);
    var files = Directory.GetFiles(cacheDir, "*.jpg");
    if (files.Length == 0)
    {
      throw new InvalidOperationException("没有可用的背景图片");
    }

    var random = new Random();
    return files[random.Next(files.Length)];
  }

  /// <summary>
  /// 生成滑块验证码
  /// </summary>
  /// <returns>
  /// 返回一个元组，包含：
  /// - backgroundImage: 带有滑块缺口的背景图片字节数组
  /// - sliderImage: 可拖动的滑块图片字节数组
  /// - x: 滑块正确位置的X坐标
  /// - y: 滑块正确位置的Y坐标
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
  public (byte[] backgroundImage, byte[] sliderImage, int x, int y) Generate()
  {
    // 随机选择一张背景图
    var imagePath = GetRandomImagePath();

    // 加载并调整背景图片尺寸
    using var originalImage = Image.Load<Rgba32>(imagePath);
    originalImage.Mutate(x => x.Resize(_options.Width, _options.Height));

    // 随机生成滑块位置
    var random = new Random();
    var x = random.Next(_options.MinX, _options.MaxX);
    var y = random.Next(_options.MinY, _options.MaxY);

    // 创建滑块形状
    var sliderPath = CreateSliderPath(x, y);

    // 从原图中裁剪出滑块
    using var sliderImage = new Image<Rgba32>(_options.SliderWidth, _options.SliderHeight);
    sliderImage.Mutate(ctx => ctx.DrawImage(originalImage, new Point(-x, -y), 1f));
    sliderImage.Mutate(ctx => ctx.SetGraphicsOptions(new GraphicsOptions
    {
      AlphaCompositionMode = PixelAlphaCompositionMode.DestOut
    }).Fill(Color.Black, sliderPath));

    // 在背景图上绘制滑块遮罩
    originalImage.Mutate(ctx => ctx.Fill(Color.Gray.WithAlpha(0.7f), sliderPath));

    // 转换为字节数组
    using var backgroundStream = new MemoryStream();
    originalImage.SaveAsJpeg(backgroundStream);
    var backgroundBytes = backgroundStream.ToArray();

    using var sliderStream = new MemoryStream();
    sliderImage.SaveAsPng(sliderStream);
    var sliderBytes = sliderStream.ToArray();

    return (backgroundBytes, sliderBytes, x, y);
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
  public bool Validate(int x, int y, int correctX, int correctY)
  {
    return Math.Abs(x - correctX) <= _options.Tolerance && Math.Abs(y - correctY) <= _options.Tolerance;
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

  /// <summary>
  /// 创建不规则的滑块形状
  /// </summary>
  /// <param name="x">滑块位置的X坐标</param>
  /// <param name="y">滑块位置的Y坐标</param>
  /// <returns>表示滑块形状的路径对象</returns>
  /// <remarks>
  /// 创建的滑块形状包括：
  /// 1. 基本的矩形轮廓
  /// 2. 顶部和底部的凸起
  /// 这种不规则的形状可以增加验证码的辨识度和趣味性
  /// </remarks>
  private IPath CreateSliderPath(int x, int y)
  {
    var pathBuilder = new PathBuilder();

    // 创建基本的矩形轮廓
    pathBuilder.AddLine(x, y, x + _options.SliderWidth, y);
    pathBuilder.AddLine(x + _options.SliderWidth, y, x + _options.SliderWidth, y + _options.SliderHeight);
    pathBuilder.AddLine(x + _options.SliderWidth, y + _options.SliderHeight, x, y + _options.SliderHeight);
    pathBuilder.AddLine(x, y + _options.SliderHeight, x, y);

    // 添加顶部和底部的凸起
    var points = new PointF[]
    {
            new(x + _options.SliderWidth / 2 - 10, y),
            new(x + _options.SliderWidth / 2, y - 5),
            new(x + _options.SliderWidth / 2 + 10, y),
            new(x + _options.SliderWidth / 2 - 10, y + _options.SliderHeight),
            new(x + _options.SliderWidth / 2, y + _options.SliderHeight + 5),
            new(x + _options.SliderWidth / 2 + 10, y + _options.SliderHeight)
    };

    // 连接顶部凸起的点
    pathBuilder.AddLine(points[0], points[1]);
    pathBuilder.AddLine(points[1], points[2]);

    // 连接底部凸起的点
    pathBuilder.AddLine(points[3], points[4]);
    pathBuilder.AddLine(points[4], points[5]);

    return pathBuilder.Build();
  }
}