using System.ComponentModel;

namespace Lean.CodeGen.Common.Excel;

/// <summary>
/// Excel导入结果
/// </summary>
/// <typeparam name="T">数据类型</typeparam>
public class LeanExcelImportResult<T>
{
  /// <summary>
  /// 导入的数据列表
  /// </summary>
  public List<T> Data { get; set; } = new();

  /// <summary>
  /// 错误列表
  /// </summary>
  public List<LeanExcelImportError> Errors { get; set; } = new();

  /// <summary>
  /// 错误消息
  /// </summary>
  public string? ErrorMessage { get; set; }

  /// <summary>
  /// 是否成功
  /// </summary>
  public bool IsSuccess => string.IsNullOrEmpty(ErrorMessage) && !Errors.Any();

  /// <summary>
  /// 总记录数
  /// </summary>
  public int TotalCount => Data.Count + Errors.Count;

  /// <summary>
  /// 成功记录数
  /// </summary>
  public int SuccessCount => Data.Count;

  /// <summary>
  /// 失败记录数
  /// </summary>
  public int FailCount => Errors.Count;
}

/// <summary>
/// Excel导入错误
/// </summary>
public class LeanExcelImportError
{
  /// <summary>
  /// 行号
  /// </summary>
  public int RowIndex { get; set; }

  /// <summary>
  /// 错误消息
  /// </summary>
  public string? ErrorMessage { get; set; }
}

/// <summary>
/// Excel文件格式
/// </summary>
public enum LeanExcelFormat
{
  /// <summary>
  /// Excel 2007+ (.xlsx)
  /// </summary>
  [Description("xlsx")]
  Xlsx,

  /// <summary>
  /// CSV (.csv)
  /// </summary>
  [Description("csv")]
  Csv
}

/// <summary>
/// 忽略Excel导入导出特性
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class ExcelIgnoreAttribute : Attribute
{
}

/// <summary>
/// Excel列特性
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class LeanExcelColumnAttribute : Attribute
{
  /// <summary>
  /// 列名
  /// </summary>
  public string Name { get; }

  /// <summary>
  /// 数据类型
  /// </summary>
  public LeanExcelDataType DataType { get; set; } = LeanExcelDataType.String;

  /// <summary>
  /// 格式化字符串
  /// </summary>
  public string? Format { get; set; }

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="name">列名</param>
  public LeanExcelColumnAttribute(string name)
  {
    Name = name;
  }
}

/// <summary>
/// Excel数据类型
/// </summary>
public enum LeanExcelDataType
{
  /// <summary>
  /// 字符串
  /// </summary>
  String,

  /// <summary>
  /// 整数
  /// </summary>
  Int,

  /// <summary>
  /// 长整数
  /// </summary>
  Long,

  /// <summary>
  /// 小数
  /// </summary>
  Decimal,

  /// <summary>
  /// 日期时间
  /// </summary>
  DateTime,

  /// <summary>
  /// 布尔值
  /// </summary>
  Bool,

  /// <summary>
  /// 布尔值
  /// </summary>
  Boolean = Bool
}