using System.ComponentModel.DataAnnotations;

namespace Lean.CodeGen.Infrastructure.CodeGen.Models;

/// <summary>
/// 实体生成选项
/// </summary>
public class LeanEntityGenOptions : LeanCodeGenOptions
{
  /// <summary>
  /// 实体名称
  /// </summary>
  [Required(ErrorMessage = "实体名称不能为空")]
  public string EntityName { get; set; }

  /// <summary>
  /// 命名空间
  /// </summary>
  [Required(ErrorMessage = "命名空间不能为空")]
  public string Namespace { get; set; }
}