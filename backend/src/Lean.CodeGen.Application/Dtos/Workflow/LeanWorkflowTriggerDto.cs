namespace Lean.CodeGen.Application.Dtos.Workflow;

/// <summary>
/// 工作流触发器DTO
/// </summary>
public class LeanWorkflowTriggerDto
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
  /// 触发器名称
  /// </summary>
  public string TriggerName { get; set; } = string.Empty;

  /// <summary>
  /// 触发器类型(Timer=定时触发,Signal=信号触发,Event=事件触发)
  /// </summary>
  public string TriggerType { get; set; } = string.Empty;

  /// <summary>
  /// 触发器配置JSON
  /// </summary>
  public string? TriggerConfig { get; set; }

  /// <summary>
  /// 是否启用
  /// 0-停用
  /// 1-启用
  /// </summary>
  public int Status { get; set; }

  /// <summary>
  /// 上次触发时间
  /// </summary>
  public DateTime? LastTriggerTime { get; set; }

  /// <summary>
  /// 下次触发时间
  /// </summary>
  public DateTime? NextTriggerTime { get; set; }

  /// <summary>
  /// 触发次数
  /// </summary>
  public int TriggerCount { get; set; }

  /// <summary>
  /// 触发条件JSON
  /// </summary>
  public string? TriggerCondition { get; set; }

  /// <summary>
  /// 输入参数JSON
  /// </summary>
  public string? InputParameters { get; set; }

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