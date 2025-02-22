namespace Lean.CodeGen.Application.Dtos.Workflow;

/// <summary>
/// 工作流变量数据DTO
/// </summary>
public class LeanWorkflowVariableDataDto
{
  /// <summary>
  /// ID
  /// </summary>
  public long Id { get; set; }

  /// <summary>
  /// 工作流实例ID
  /// </summary>
  public long InstanceId { get; set; }

  /// <summary>
  /// 工作流实例
  /// </summary>
  public LeanWorkflowInstanceDto Instance { get; set; } = null!;

  /// <summary>
  /// 任务ID
  /// </summary>
  public long? TaskId { get; set; }

  /// <summary>
  /// 工作流任务
  /// </summary>
  public LeanWorkflowTaskDto? Task { get; set; }

  /// <summary>
  /// 变量定义ID
  /// </summary>
  public long VariableId { get; set; }

  /// <summary>
  /// 变量名称
  /// </summary>
  public string VariableName { get; set; } = string.Empty;

  /// <summary>
  /// 变量类型
  /// </summary>
  public string VariableType { get; set; } = string.Empty;

  /// <summary>
  /// 变量值
  /// </summary>
  public string? VariableValue { get; set; }

  /// <summary>
  /// 操作人ID
  /// </summary>
  public long OperatorId { get; set; }

  /// <summary>
  /// 操作人名称
  /// </summary>
  public string OperatorName { get; set; } = string.Empty;

  /// <summary>
  /// 操作时间
  /// </summary>
  public DateTime OperationTime { get; set; }

  /// <summary>
  /// 版本号
  /// </summary>
  public int Version { get; set; }

  /// <summary>
  /// 备注
  /// </summary>
  public string? Remark { get; set; }

  /// <summary>
  /// 创建时间
  /// </summary>
  public DateTime CreateTime { get; set; }

  /// <summary>
  /// 更新时间
  /// </summary>
  public DateTime? UpdateTime { get; set; }
}