using System;

namespace Lean.CodeGen.Common.Options;

/// <summary>
/// 防伪配置选项
/// </summary>
public class LeanAntiforgeryOptions
{
  /// <summary>
  /// 是否启用
  /// </summary>
  public bool Enabled { get; set; } = true;

  /// <summary>
  /// Cookie名称
  /// </summary>
  public string CookieName { get; set; } = "XSRF-TOKEN";

  /// <summary>
  /// Header名称
  /// </summary>
  public string HeaderName { get; set; } = "X-XSRF-TOKEN";

  /// <summary>
  /// 路径
  /// </summary>
  public string Path { get; set; } = "/";

  /// <summary>
  /// 域名
  /// </summary>
  public string Domain { get; set; }

  /// <summary>
  /// 是否启用HTTPS
  /// </summary>
  public bool RequireHttps { get; set; } = true;
}