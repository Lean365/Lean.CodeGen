namespace Lean.CodeGen.Common.Options;

/// <summary>
/// 滑块验证码配置
/// </summary>
public class LeanSliderCaptchaOptions
{
  /// <summary>
  /// 图片缓存目录
  /// </summary>
  public string ImageCacheDir { get; set; } = "images/slider";

  /// <summary>
  /// 图片下载源配置
  /// </summary>
  public LeanSliderCaptchaSourceOptions Source { get; set; } = new();

  /// <summary>
  /// 验证码有效期（秒）
  /// </summary>
  public int ExpirySeconds { get; set; } = 60;

  /// <summary>
  /// 需要下载的图片数量
  /// </summary>
  public int ImageCount { get; set; } = 5;

  /// <summary>
  /// 背景图片配置
  /// </summary>
  public List<LeanSliderCaptchaImageOptions> Images { get; set; } = new()
    {
        new() { Name = "landscape.jpg", Category = "nature", Keywords = "landscape,mountain,forest" },
        new() { Name = "girl.jpg", Category = "people", Keywords = "girl,portrait,fashion" },
        new() { Name = "city.jpg", Category = "city", Keywords = "city,architecture,building" },
        new() { Name = "child.jpg", Category = "people", Keywords = "child,kid,happy" },
        new() { Name = "ocean.jpg", Category = "nature", Keywords = "ocean,sea,beach" }
    };

  /// <summary>
  /// 图片宽度
  /// </summary>
  public int Width { get; set; } = 280;

  /// <summary>
  /// 图片高度
  /// </summary>
  public int Height { get; set; } = 155;

  /// <summary>
  /// 滑块宽度
  /// </summary>
  public int SliderWidth { get; set; } = 50;

  /// <summary>
  /// 滑块高度
  /// </summary>
  public int SliderHeight { get; set; } = 50;

  /// <summary>
  /// 滑块X坐标最小值
  /// </summary>
  public int MinX { get; set; } = 10;

  /// <summary>
  /// 滑块X坐标最大值
  /// </summary>
  public int MaxX { get; set; } = 220;

  /// <summary>
  /// 滑块Y坐标最小值
  /// </summary>
  public int MinY { get; set; } = 10;

  /// <summary>
  /// 滑块Y坐标最大值
  /// </summary>
  public int MaxY { get; set; } = 95;

  /// <summary>
  /// 验证位置允许的误差范围（像素）
  /// </summary>
  public int Tolerance { get; set; } = 5;
}

/// <summary>
/// 滑块验证码图片配置
/// </summary>
public class LeanSliderCaptchaImageOptions
{
  /// <summary>
  /// 图片名称
  /// </summary>
  public string Name { get; set; } = string.Empty;

  /// <summary>
  /// 图片分类
  /// </summary>
  public string Category { get; set; } = string.Empty;

  /// <summary>
  /// 搜索关键词
  /// </summary>
  public string Keywords { get; set; } = string.Empty;
}

/// <summary>
/// 滑块验证码图片下载源配置
/// </summary>
public class LeanSliderCaptchaSourceOptions
{
  /// <summary>
  /// 下载源类型
  /// </summary>
  public string Type { get; set; } = "Unsplash";

  /// <summary>
  /// API 密钥
  /// </summary>
  public string AccessKey { get; set; } = string.Empty;

  /// <summary>
  /// 下载源URL模板
  /// </summary>
  public string UrlTemplate { get; set; } = "https://api.unsplash.com/photos/random?w={width}&h={height}&orientation=landscape&client_id={accessKey}";
}