using System.ComponentModel.DataAnnotations;

namespace Lean.CodeGen.Common.Models;

/// <summary>
/// 导入错误基类
/// </summary>
public class LeanImportError
{
  /// <summary>
  /// 行号
  /// </summary>
  public int RowIndex { get; set; }

  /// <summary>
  /// 关键字
  /// </summary>
  public string Key { get; set; } = string.Empty;

  /// <summary>
  /// 错误消息
  /// </summary>
  public string? ErrorMessage { get; set; }
}

/// <summary>
/// 导入结果基类
/// </summary>
public class LeanImportResult
{
  /// <summary>
  /// 错误信息列表
  /// </summary>
  public List<LeanImportError> Errors { get; set; } = new();

  /// <summary>
  /// 错误信息
  /// </summary>
  public string? ErrorMessage { get; set; }

  /// <summary>
  /// 成功记录数
  /// </summary>
  public int SuccessCount { get; set; }

  /// <summary>
  /// 失败记录数
  /// </summary>
  public int FailCount { get; set; }

  /// <summary>
  /// 总记录数
  /// </summary>
  public int TotalCount { get; set; }

  /// <summary>
  /// 添加错误信息
  /// </summary>
  public virtual void AddError(string key, string errorMessage)
  {
    FailCount++;
    TotalCount++;
    Errors.Add(new LeanImportError
    {
      RowIndex = TotalCount,
      Key = key,
      ErrorMessage = errorMessage
    });
  }
}

/// <summary>
/// 导出查询基类
/// </summary>
public class LeanExportQuery : LeanPage
{
  /// <summary>
  /// 导出字段列表
  /// </summary>
  public List<string> ExportFields { get; set; } = new();

  /// <summary>
  /// 文件格式
  /// </summary>
  [Required(ErrorMessage = "文件格式不能为空")]
  public string FileFormat { get; set; } = "xlsx";

  /// <summary>
  /// 是否导出全部
  /// </summary>
  public bool IsExportAll { get; set; }

  /// <summary>
  /// 选中的ID列表
  /// </summary>
  public List<long> SelectedIds { get; set; } = new();
}