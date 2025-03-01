//===================================================
// 项目名: Lean.CodeGen.Common
// 文件名: LeanFileResult.cs
// 功能描述: 文件结果类
// 创建时间: 2024-03-26
// 作者: Lean
// 版本: 1.0
//===================================================

using System.IO;

namespace Lean.CodeGen.Common.Models
{
  /// <summary>
  /// 文件结果类
  /// </summary>
  public class LeanFileResult
  {
    /// <summary>
    /// 文件名
    /// </summary>
    public string FileName { get; set; }

    /// <summary>
    /// 文件内容类型
    /// </summary>
    public string ContentType { get; set; }

    /// <summary>
    /// 文件流
    /// </summary>
    public Stream Stream { get; set; }
  }

  /// <summary>
  /// 文件信息类
  /// </summary>
  public class LeanFileInfo
  {
    /// <summary>
    /// 文件名
    /// </summary>
    public string FileName { get; set; }

    /// <summary>
    /// 文件路径
    /// </summary>
    public string FilePath { get; set; }

    /// <summary>
    /// 文件大小(字节)
    /// </summary>
    public long FileSize { get; set; }

    /// <summary>
    /// 文件内容类型
    /// </summary>
    public string ContentType { get; set; }

    /// <summary>
    /// 文件流
    /// </summary>
    public Stream Stream { get; set; }
  }
}