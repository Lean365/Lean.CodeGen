namespace Lean.CodeGen.Application.Dtos.Workflow;

/// <summary>
/// 工作流表单数据DTO
/// </summary>
public class LeanWorkflowFormDataDto
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
  /// 表单定义ID
  /// </summary>
  public long FormId { get; set; }

  /// <summary>
  /// 表单定义
  /// </summary>
  public LeanWorkflowFormDefinitionDto Form { get; set; } = null!;

  /// <summary>
  /// 字段编码
  /// </summary>
  public string FieldCode { get; set; } = string.Empty;

  /// <summary>
  /// 字段名称
  /// </summary>
  public string FieldName { get; set; } = string.Empty;

  /// <summary>
  /// 字段类型
  /// </summary>
  public string FieldType { get; set; } = string.Empty;

  /// <summary>
  /// 字段值
  /// </summary>
  public string? FieldValue { get; set; }

  /// <summary>
  /// 显示值
  /// </summary>
  public string? DisplayValue { get; set; }

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