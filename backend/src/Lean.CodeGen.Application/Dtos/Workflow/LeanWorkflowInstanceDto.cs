using Lean.CodeGen.Common.Enums;

namespace Lean.CodeGen.Application.Dtos.Workflow;

/// <summary>
/// 工作流实例DTO
/// </summary>
public class LeanWorkflowInstanceDto
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
  /// 工作流定义
  /// </summary>
  public LeanWorkflowDefinitionDto Definition { get; set; } = null!;

  /// <summary>
  /// 工作流定义版本号
  /// </summary>
  public int DefinitionVersion { get; set; }

  /// <summary>
  /// 业务主键
  /// </summary>
  public string BusinessKey { get; set; } = string.Empty;

  /// <summary>
  /// 业务类型
  /// </summary>
  public string BusinessType { get; set; } = string.Empty;

  /// <summary>
  /// 实例标题
  /// </summary>
  public string Title { get; set; } = string.Empty;

  /// <summary>
  /// 优先级
  /// </summary>
  public int Priority { get; set; }

  /// <summary>
  /// 发起人ID
  /// </summary>
  public long InitiatorId { get; set; }

  /// <summary>
  /// 发起人名称
  /// </summary>
  public string InitiatorName { get; set; } = string.Empty;

  /// <summary>
  /// 发起人部门ID
  /// </summary>
  public long InitiatorDeptId { get; set; }

  /// <summary>
  /// 发起人部门名称
  /// </summary>
  public string InitiatorDeptName { get; set; } = string.Empty;

  /// <summary>
  /// 当前节点ID
  /// </summary>
  public string? CurrentNodeId { get; set; }

  /// <summary>
  /// 当前节点名称
  /// </summary>
  public string? CurrentNodeName { get; set; }

  /// <summary>
  /// 开始时间
  /// </summary>
  public DateTime? StartTime { get; set; }

  /// <summary>
  /// 结束时间
  /// </summary>
  public DateTime? EndTime { get; set; }

  /// <summary>
  /// 工作流状态
  /// </summary>
  public int WorkflowStatus { get; set; }

  /// <summary>
  /// 是否暂停
  /// 0-否
  /// 1-是
  /// </summary>
  public int IsSuspended { get; set; }

  /// <summary>
  /// 是否已归档
  /// 0-否
  /// 1-是
  /// </summary>
  public int IsArchived { get; set; }

  /// <summary>
  /// 表单数据列表
  /// </summary>
  public List<LeanWorkflowFormDataDto> FormDataList { get; set; } = new();

  /// <summary>
  /// 变量数据列表
  /// </summary>
  public List<LeanWorkflowVariableDataDto> VariableDataList { get; set; } = new();

  /// <summary>
  /// 任务列表
  /// </summary>
  public List<LeanWorkflowTaskDto> Tasks { get; set; } = new();

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