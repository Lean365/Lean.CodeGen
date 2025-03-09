using Lean.CodeGen.Common.Enums;

namespace Lean.CodeGen.Application.Dtos.Workflow;

/// <summary>
/// 工作流历史DTO
/// </summary>
public class LeanWorkflowHistoryDto
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
  /// 操作类型
  /// </summary>
  public int OperationType { get; set; }

  /// <summary>
  /// 操作人ID
  /// </summary>
  public long OperatorId { get; set; }

  /// <summary>
  /// 操作人名称
  /// </summary>
  public string OperatorName { get; set; } = string.Empty;

  /// <summary>
  /// 操作人部门ID
  /// </summary>
  public long OperatorDeptId { get; set; }

  /// <summary>
  /// 操作人部门名称
  /// </summary>
  public string OperatorDeptName { get; set; } = string.Empty;

  /// <summary>
  /// 操作时间
  /// </summary>
  public DateTime OperationTime { get; set; }

  /// <summary>
  /// 操作说明
  /// </summary>
  public string? OperationDesc { get; set; }

  /// <summary>
  /// IP地址
  /// </summary>
  public string? IpAddress { get; set; }

  /// <summary>
  /// 设备信息
  /// </summary>
  public string? DeviceInfo { get; set; }

  /// <summary>
  /// 表单数据列表
  /// </summary>
  public List<LeanWorkflowFormDataDto> FormDataList { get; set; } = new();

  /// <summary>
  /// 变量数据列表
  /// </summary>
  public List<LeanWorkflowVariableDataDto> VariableDataList { get; set; } = new();

  /// <summary>
  /// 创建时间
  /// </summary>
  public DateTime CreateTime { get; set; }

  /// <summary>
  /// 更新时间
  /// </summary>
  public DateTime? UpdateTime { get; set; }
}