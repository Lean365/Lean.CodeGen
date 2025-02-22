namespace Lean.CodeGen.Application.Dtos.Workflow;

/// <summary>
/// 工作流活动类型DTO
/// </summary>
public class LeanWorkflowActivityTypeDto
{
  /// <summary>
  /// 活动类型名称
  /// </summary>
  public string TypeName { get; set; } = string.Empty;

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
  public string Category { get; set; } = string.Empty;

  /// <summary>
  /// 图标
  /// </summary>
  public string? Icon { get; set; }

  /// <summary>
  /// 颜色
  /// </summary>
  public string? Color { get; set; }

  /// <summary>
  /// 输入参数定义JSON
  /// </summary>
  public string? InputParameters { get; set; }

  /// <summary>
  /// 输出参数定义JSON
  /// </summary>
  public string? OutputParameters { get; set; }

  /// <summary>
  /// 属性定义JSON
  /// </summary>
  public string? Properties { get; set; }

  /// <summary>
  /// 结果定义JSON
  /// </summary>
  public string? Outcomes { get; set; }

  /// <summary>
  /// 是否支持补偿
  /// </summary>
  public bool SupportsCompensation { get; set; }

  /// <summary>
  /// 是否为阻塞活动
  /// </summary>
  public bool IsBlocking { get; set; }

  /// <summary>
  /// 是否为触发器活动
  /// </summary>
  public bool IsTrigger { get; set; }

  /// <summary>
  /// 是否为容器活动
  /// </summary>
  public bool IsContainer { get; set; }

  /// <summary>
  /// 是否为系统活动
  /// </summary>
  public bool IsSystem { get; set; }

  /// <summary>
  /// 是否启用
  /// </summary>
  public bool Status { get; set; }

  /// <summary>
  /// 自定义属性JSON
  /// </summary>
  public string? CustomAttributes { get; set; }

  /// <summary>
  /// 排序号
  /// </summary>
  public int OrderNum { get; set; }
}