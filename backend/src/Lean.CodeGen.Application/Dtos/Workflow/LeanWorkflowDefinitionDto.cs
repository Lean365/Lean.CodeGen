using Lean.CodeGen.Common.Enums;

namespace Lean.CodeGen.Application.Dtos.Workflow;

/// <summary>
/// 工作流定义DTO
/// </summary>
public class LeanWorkflowDefinitionDto
{
  /// <summary>
  /// ID
  /// </summary>
  public long Id { get; set; }

  /// <summary>
  /// 工作流名称
  /// </summary>
  public string WorkflowName { get; set; } = string.Empty;

  /// <summary>
  /// 工作流编码
  /// </summary>
  public string WorkflowCode { get; set; } = string.Empty;

  /// <summary>
  /// 显示名称
  /// </summary>
  public string? DisplayName { get; set; }

  /// <summary>
  /// 工作流描述
  /// </summary>
  public string? WorkflowDescription { get; set; }

  /// <summary>
  /// 工作流版本号
  /// </summary>
  public int Version { get; set; }

  /// <summary>
  /// 是否为最新版本
  /// </summary>
  public bool IsLatest { get; set; }

  /// <summary>
  /// 是否已发布
  /// </summary>
  public bool IsPublished { get; set; }

  /// <summary>
  /// 工作流状态
  /// </summary>
  public LeanWorkflowStatus WorkflowStatus { get; set; }

  /// <summary>
  /// 是否启用
  /// </summary>
  public bool Status { get; set; }

  /// <summary>
  /// 是否删除已完成实例
  /// </summary>
  public bool DeleteCompletedInstances { get; set; }

  /// <summary>
  /// 排序号
  /// </summary>
  public int OrderNum { get; set; }

  /// <summary>
  /// 是否内置
  /// </summary>
  public bool IsBuiltin { get; set; }

  /// <summary>
  /// 表单定义列表
  /// </summary>
  public List<LeanWorkflowFormDefinitionDto> Forms { get; set; } = new();

  /// <summary>
  /// 活动列表
  /// </summary>
  public List<LeanWorkflowActivityDto> Activities { get; set; } = new();

  /// <summary>
  /// 变量列表
  /// </summary>
  public List<LeanWorkflowVariableDto> Variables { get; set; } = new();

  /// <summary>
  /// 触发器列表
  /// </summary>
  public List<LeanWorkflowTriggerDto> Triggers { get; set; } = new();

  /// <summary>
  /// 创建时间
  /// </summary>
  public DateTime CreateTime { get; set; }

  /// <summary>
  /// 更新时间
  /// </summary>
  public DateTime? UpdateTime { get; set; }
}