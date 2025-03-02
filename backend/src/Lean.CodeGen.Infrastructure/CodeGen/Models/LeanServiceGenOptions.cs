using System.ComponentModel.DataAnnotations;

namespace Lean.CodeGen.Infrastructure.CodeGen.Models;

/// <summary>
/// 服务生成选项
/// </summary>
public class LeanServiceGenOptions : LeanCodeGenOptions
{
  /// <summary>
  /// 服务名称
  /// </summary>
  [Required(ErrorMessage = "服务名称不能为空")]
  public string ServiceName { get; set; }

  /// <summary>
  /// 命名空间
  /// </summary>
  [Required(ErrorMessage = "命名空间不能为空")]
  public string Namespace { get; set; }

  /// <summary>
  /// 是否生成接口
  /// </summary>
  public bool GenerateInterface { get; set; }
}