namespace Lean.CodeGen.Infrastructure.CodeGen.Models;

/// <summary>
/// 代码生成结果
/// </summary>
public class LeanCodeGenResult
{
  /// <summary>
  /// 是否成功
  /// </summary>
  public bool Success { get; set; }

  /// <summary>
  /// 错误信息
  /// </summary>
  public string Error { get; set; }

  /// <summary>
  /// 生成的文件路径
  /// </summary>
  public string FilePath { get; set; }

  /// <summary>
  /// 生成的代码内容
  /// </summary>
  public string Code { get; set; }

  /// <summary>
  /// 创建成功结果
  /// </summary>
  public static LeanCodeGenResult CreateSuccess(string filePath, string code) =>
      new() { Success = true, FilePath = filePath, Code = code };

  /// <summary>
  /// 创建错误结果
  /// </summary>
  public static LeanCodeGenResult CreateError(string error) =>
      new() { Success = false, Error = error };
}