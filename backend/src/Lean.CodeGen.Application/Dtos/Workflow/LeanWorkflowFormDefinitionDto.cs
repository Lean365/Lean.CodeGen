namespace Lean.CodeGen.Application.Dtos.Workflow;

/// <summary>
/// 工作流表单定义DTO
/// </summary>
public class LeanWorkflowFormDefinitionDto
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
  /// 表单编码
  /// </summary>
  public string FormCode { get; set; } = string.Empty;

  /// <summary>
  /// 表单名称
  /// </summary>
  public string FormName { get; set; } = string.Empty;

  /// <summary>
  /// 表单类型(预设/自定义)
  /// </summary>
  public string FormType { get; set; } = string.Empty;

  /// <summary>
  /// 表单版本号
  /// </summary>
  public int Version { get; set; }

  /// <summary>
  /// 是否为最新版本
  /// 0-否
  /// 1-是
  /// </summary>
  public int IsLatest { get; set; }

  /// <summary>
  /// 表单布局
  /// </summary>
  public string? Layout { get; set; }

  /// <summary>
  /// 表单配置
  /// </summary>
  public string? Config { get; set; }

  /// <summary>
  /// 表单样式
  /// </summary>
  public string? Styles { get; set; }

  /// <summary>
  /// 是否启用
  /// 0-停用
  /// 1-启用
  /// </summary>
  public int Status { get; set; }

  /// <summary>
  /// 备注
  /// </summary>
  public string? Remark { get; set; }

  /// <summary>
  /// 排序号
  /// </summary>
  public int OrderNum { get; set; }

  /// <summary>
  /// 字段列表
  /// </summary>
  public List<LeanWorkflowFormFieldDto> Fields { get; set; } = new();

  /// <summary>
  /// 创建时间
  /// </summary>
  public DateTime CreateTime { get; set; }

  /// <summary>
  /// 更新时间
  /// </summary>
  public DateTime? UpdateTime { get; set; }
}