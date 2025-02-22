namespace Lean.CodeGen.Application.Dtos.Workflow;

/// <summary>
/// 工作流连接DTO
/// </summary>
public class LeanWorkflowConnectionDto
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
  /// 连接ID
  /// </summary>
  public string ConnectionId { get; set; } = string.Empty;

  /// <summary>
  /// 源活动ID
  /// </summary>
  public string SourceActivityId { get; set; } = string.Empty;

  /// <summary>
  /// 目标活动ID
  /// </summary>
  public string TargetActivityId { get; set; } = string.Empty;

  /// <summary>
  /// 连接类型
  /// </summary>
  public string ConnectionType { get; set; } = string.Empty;

  /// <summary>
  /// 连接名称
  /// </summary>
  public string? ConnectionName { get; set; }

  /// <summary>
  /// 连接描述
  /// </summary>
  public string? Description { get; set; }

  /// <summary>
  /// 连接条件JSON
  /// </summary>
  public string? Condition { get; set; }

  /// <summary>
  /// 连接属性JSON
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