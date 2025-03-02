using System.ComponentModel.DataAnnotations;

namespace Lean.CodeGen.Infrastructure.CodeGen.Models;

/// <summary>
/// 代码生成选项基类
/// </summary>
public class LeanCodeGenOptions
{
  /// <summary>
  /// 模板路径
  /// </summary>
  [Required(ErrorMessage = "模板路径不能为空")]
  public string TemplatePath { get; set; }

  /// <summary>
  /// 输出路径
  /// </summary>
  [Required(ErrorMessage = "输出路径不能为空")]
  public string OutputPath { get; set; }

  /// <summary>
  /// 是否覆盖已存在的文件
  /// </summary>
  public bool Overwrite { get; set; }

  /// <summary>
  /// 模型数据
  /// </summary>
  public object Model { get; set; }
}