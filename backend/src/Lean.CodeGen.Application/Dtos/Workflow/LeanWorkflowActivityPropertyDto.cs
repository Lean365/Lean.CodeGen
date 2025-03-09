namespace Lean.CodeGen.Application.Dtos.Workflow;

/// <summary>
/// 工作流活动属性DTO
/// </summary>
public class LeanWorkflowActivityPropertyDto
{
  /// <summary>
  /// ID
  /// </summary>
  public long Id { get; set; }

  /// <summary>
  /// 活动ID
  /// </summary>
  public long ActivityId { get; set; }

  /// <summary>
  /// 属性名称
  /// </summary>
  public string PropertyName { get; set; } = string.Empty;

  /// <summary>
  /// 属性类型
  /// </summary>
  public string PropertyType { get; set; } = string.Empty;

  /// <summary>
  /// 显示名称
  /// </summary>
  public string? DisplayName { get; set; }

  /// <summary>
  /// 描述
  /// </summary>
  public string? Description { get; set; }

  /// <summary>
  /// 分类
  /// </summary>
  public string? Category { get; set; }

  /// <summary>
  /// 默认值JSON
  /// </summary>
  public string? DefaultValue { get; set; }

  /// <summary>
  /// 选项JSON
  /// </summary>
  public string? Options { get; set; }

  /// <summary>
  /// 验证规则JSON
  /// </summary>
  public string? ValidationRules { get; set; }

  /// <summary>
  /// UI提示JSON
  /// </summary>
  public string? UIHints { get; set; }

  /// <summary>
  /// 是否必填
  /// 0-否
  /// 1-是
  /// </summary>
  public int IsRequired { get; set; }

  /// <summary>
  /// 是否支持表达式
  /// 0-否
  /// 1-是
  /// </summary>
  public int SupportsExpressions { get; set; }

  /// <summary>
  /// 是否设计时属性
  /// 0-否
  /// 1-是
  /// </summary>
  public int IsDesignTime { get; set; }

  /// <summary>
  /// 是否运行时属性
  /// 0-否
  /// 1-是
  /// </summary>
  public int IsRunTime { get; set; }

  /// <summary>
  /// 是否启用
  /// 0-停用
  /// 1-启用
  /// </summary>
  public int Status { get; set; }

  /// <summary>
  /// 自定义属性JSON
  /// </summary>
  public string? CustomAttributes { get; set; }

  /// <summary>
  /// 排序号
  /// </summary>
  public int OrderNum { get; set; }

  /// <summary>
  /// 创建时间
  /// </summary>
  public DateTime CreateTime { get; set; }

  /// <summary>
  /// 更新时间
  /// </summary>
  public DateTime? UpdateTime { get; set; }
}