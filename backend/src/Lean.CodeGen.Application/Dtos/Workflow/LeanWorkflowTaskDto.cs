using Lean.CodeGen.Common.Enums;

namespace Lean.CodeGen.Application.Dtos.Workflow;

/// <summary>
/// 工作流任务DTO
/// </summary>
public class LeanWorkflowTaskDto
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
  /// 任务名称
  /// </summary>
  public string TaskName { get; set; } = string.Empty;

  /// <summary>
  /// 任务类型
  /// </summary>
  public LeanWorkflowTaskType TaskType { get; set; }

  /// <summary>
  /// 任务节点编码
  /// </summary>
  public string TaskNode { get; set; } = string.Empty;

  /// <summary>
  /// 任务优先级
  /// </summary>
  public int Priority { get; set; }

  /// <summary>
  /// 处理人ID
  /// </summary>
  public long? AssigneeId { get; set; }

  /// <summary>
  /// 处理人名称
  /// </summary>
  public string? AssigneeName { get; set; }

  /// <summary>
  /// 处理人部门ID
  /// </summary>
  public long? AssigneeDeptId { get; set; }

  /// <summary>
  /// 处理人部门名称
  /// </summary>
  public string? AssigneeDeptName { get; set; }

  /// <summary>
  /// 原处理人ID
  /// </summary>
  public long? OriginalAssigneeId { get; set; }

  /// <summary>
  /// 原处理人名称
  /// </summary>
  public string? OriginalAssigneeName { get; set; }

  /// <summary>
  /// 任务状态
  /// </summary>
  public LeanWorkflowTaskStatus TaskStatus { get; set; }

  /// <summary>
  /// 开始时间
  /// </summary>
  public DateTime? StartTime { get; set; }

  /// <summary>
  /// 结束时间
  /// </summary>
  public DateTime? EndTime { get; set; }

  /// <summary>
  /// 到期时间
  /// </summary>
  public DateTime? DueTime { get; set; }

  /// <summary>
  /// 提醒时间
  /// </summary>
  public DateTime? RemindTime { get; set; }

  /// <summary>
  /// 处理意见
  /// </summary>
  public string? Comment { get; set; }

  /// <summary>
  /// 是否超时
  /// </summary>
  public bool IsTimeout { get; set; }

  /// <summary>
  /// 是否已读
  /// </summary>
  public bool IsRead { get; set; }

  /// <summary>
  /// 是否已催办
  /// </summary>
  public bool IsUrged { get; set; }

  /// <summary>
  /// 表单数据列表
  /// </summary>
  public List<LeanWorkflowFormDataDto> FormDataList { get; set; } = new();

  /// <summary>
  /// 变量数据列表
  /// </summary>
  public List<LeanWorkflowVariableDataDto> VariableDataList { get; set; } = new();

  /// <summary>
  /// 历史记录列表
  /// </summary>
  public List<LeanWorkflowHistoryDto> Histories { get; set; } = new();

  /// <summary>
  /// 创建时间
  /// </summary>
  public DateTime CreateTime { get; set; }

  /// <summary>
  /// 更新时间
  /// </summary>
  public DateTime? UpdateTime { get; set; }
}