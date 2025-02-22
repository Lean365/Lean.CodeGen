namespace Lean.CodeGen.Application.Dtos.Workflow;

/// <summary>
/// 工作流表单字段DTO
/// </summary>
public class LeanWorkflowFormFieldDto
{
  /// <summary>
  /// ID
  /// </summary>
  public long Id { get; set; }

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
  /// 控件类型
  /// </summary>
  public string ControlType { get; set; } = string.Empty;

  /// <summary>
  /// 默认值
  /// </summary>
  public string? DefaultValue { get; set; }

  /// <summary>
  /// 占位提示
  /// </summary>
  public string? Placeholder { get; set; }

  /// <summary>
  /// 是否必填
  /// </summary>
  public bool IsRequired { get; set; }

  /// <summary>
  /// 是否只读
  /// </summary>
  public bool IsReadonly { get; set; }

  /// <summary>
  /// 是否隐藏
  /// </summary>
  public bool IsHidden { get; set; }

  /// <summary>
  /// 验证规则
  /// </summary>
  public string? ValidationRules { get; set; }

  /// <summary>
  /// 数据源配置
  /// </summary>
  public string? DatasourceConfig { get; set; }

  /// <summary>
  /// 联动规则
  /// </summary>
  public string? LinkageRules { get; set; }

  /// <summary>
  /// 权限配置
  /// </summary>
  public string? PermissionConfig { get; set; }

  /// <summary>
  /// UI配置
  /// </summary>
  public string? UIConfig { get; set; }

  /// <summary>
  /// 是否启用
  /// </summary>
  public bool Status { get; set; }

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