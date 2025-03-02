using System.ComponentModel.DataAnnotations;

namespace Lean.CodeGen.Infrastructure.CodeGen.Models;

/// <summary>
/// 控制器生成选项
/// </summary>
public class LeanControllerGenOptions : LeanCodeGenOptions
{
  /// <summary>
  /// 控制器名称
  /// </summary>
  [Required(ErrorMessage = "控制器名称不能为空")]
  public string ControllerName { get; set; }

  /// <summary>
  /// 命名空间
  /// </summary>
  [Required(ErrorMessage = "命名空间不能为空")]
  public string Namespace { get; set; }

  /// <summary>
  /// API版本
  /// </summary>
  public string Version { get; set; }
}