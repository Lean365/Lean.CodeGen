namespace Lean.CodeGen.Common.Models;

/// <summary>
/// 分页查询参数
/// </summary>
public class LeanPage
{
  /// <summary>
  /// 当前页码
  /// </summary>
  /// <remarks>
  /// 从1开始
  /// </remarks>
  public int PageIndex { get; set; } = 1;

  /// <summary>
  /// 每页大小
  /// </summary>
  /// <remarks>
  /// 默认20条
  /// </remarks>
  public int PageSize { get; set; } = 20;

  /// <summary>
  /// 排序字段
  /// </summary>
  public string? OrderBy { get; set; }

  /// <summary>
  /// 是否升序
  /// </summary>
  public bool IsAsc { get; set; } = true;

  /// <summary>
  /// 获取跳过的记录数
  /// </summary>
  public int Skip => (PageIndex - 1) * PageSize;

  /// <summary>
  /// 获取排序表达式
  /// </summary>
  public string OrderByExpression => $"{OrderBy} {(IsAsc ? "asc" : "desc")}";
}

/// <summary>
/// 分页结果
/// </summary>
/// <typeparam name="T">数据类型</typeparam>
public class LeanPageResult<T>
{
  /// <summary>
  /// 总记录数
  /// </summary>
  public long Total { get; set; }

  /// <summary>
  /// 当前页数据
  /// </summary>
  public List<T> Items { get; set; } = new();

  /// <summary>
  /// 是否有上一页
  /// </summary>
  public bool HasPrevious => PageIndex > 1;

  /// <summary>
  /// 是否有下一页
  /// </summary>
  public bool HasNext => PageIndex < TotalPages;

  /// <summary>
  /// 当前页码
  /// </summary>
  public int PageIndex { get; set; }

  /// <summary>
  /// 每页大小
  /// </summary>
  public int PageSize { get; set; }

  /// <summary>
  /// 总页数
  /// </summary>
  public int TotalPages => (int)Math.Ceiling(Total / (double)PageSize);
}