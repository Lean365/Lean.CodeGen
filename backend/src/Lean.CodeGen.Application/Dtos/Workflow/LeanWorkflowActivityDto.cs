namespace Lean.CodeGen.Application.Dtos.Workflow;

/// <summary>
/// 工作流活动DTO
/// </summary>
public class LeanWorkflowActivityDto
{
  /// <summary>
  /// ID
  /// </summary>
  public long Id { get; set; }

  /// <summary>
  /// 工作流定义ID
  /// </summary>
  public long DefinitionId { get; set; }

  /// <summary>
  /// 活动ID
  /// </summary>
  public string ActivityId { get; set; } = string.Empty;

  /// <summary>
  /// 活动名称
  /// </summary>
  public string ActivityName { get; set; } = string.Empty;

  /// <summary>
  /// 活动类型
  /// </summary>
  public string ActivityType { get; set; } = string.Empty;

  /// <summary>
  /// 活动显示名称
  /// </summary>
  public string? DisplayName { get; set; }

  /// <summary>
  /// 活动描述
  /// </summary>
  public string? Description { get; set; }

  /// <summary>
  /// 是否启用
  /// 0-停用
  /// 1-启用
  /// </summary>
  public int Status { get; set; }

  /// <summary>
  /// 活动属性JSON
  /// </summary>
  public string? Properties { get; set; }

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