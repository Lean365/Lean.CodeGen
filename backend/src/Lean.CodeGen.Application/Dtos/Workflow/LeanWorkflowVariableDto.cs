namespace Lean.CodeGen.Application.Dtos.Workflow;

/// <summary>
/// 工作流变量DTO
/// </summary>
public class LeanWorkflowVariableDto
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
  /// 变量名称
  /// </summary>
  public string VariableName { get; set; } = string.Empty;

  /// <summary>
  /// 变量类型
  /// </summary>
  public string VariableType { get; set; } = string.Empty;

  /// <summary>
  /// 变量显示名称
  /// </summary>
  public string? DisplayName { get; set; }

  /// <summary>
  /// 变量描述
  /// </summary>
  public string? Description { get; set; }

  /// <summary>
  /// 默认值
  /// </summary>
  public string? DefaultValue { get; set; }

  /// <summary>
  /// 是否必填
  /// </summary>
  public bool IsRequired { get; set; }

  /// <summary>
  /// 是否只读
  /// </summary>
  public bool IsReadonly { get; set; }

  /// <summary>
  /// 是否启用
  /// </summary>
  public bool Status { get; set; }

  /// <summary>
  /// 变量属性JSON
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