namespace Lean.CodeGen.Application.Dtos.Workflow;

/// <summary>
/// 工作流输入DTO
/// </summary>
public class LeanWorkflowInputDto
{
  /// <summary>
  /// 工作流实例ID
  /// </summary>
  public long InstanceId { get; set; }

  /// <summary>
  /// 活动实例ID
  /// </summary>
  public long? ActivityInstanceId { get; set; }

  /// <summary>
  /// 输入名称
  /// </summary>
  public string InputName { get; set; } = string.Empty;

  /// <summary>
  /// 输入类型
  /// </summary>
  public string InputType { get; set; } = string.Empty;

  /// <summary>
  /// 输入值JSON
  /// </summary>
  public string? InputValue { get; set; }

  /// <summary>
  /// 是否必填
  /// </summary>
  public bool IsRequired { get; set; }

  /// <summary>
  /// 验证规则JSON
  /// </summary>
  public string? ValidationRules { get; set; }

  /// <summary>
  /// 默认值JSON
  /// </summary>
  public string? DefaultValue { get; set; }

  /// <summary>
  /// 创建时间
  /// </summary>
  public DateTime CreateTime { get; set; }
}