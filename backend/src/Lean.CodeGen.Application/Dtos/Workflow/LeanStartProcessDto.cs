namespace Lean.CodeGen.Application.Dtos.Workflow;

/// <summary>
/// 启动流程请求DTO
/// </summary>
public class LeanStartProcessDto
{
  /// <summary>
  /// 流程定义ID
  /// </summary>
  public long DefinitionId { get; set; }

  /// <summary>
  /// 业务键
  /// </summary>
  public string BusinessKey { get; set; }

  /// <summary>
  /// 业务类型
  /// </summary>
  public string BusinessType { get; set; }

  /// <summary>
  /// 流程标题
  /// </summary>
  public string Title { get; set; }

  /// <summary>
  /// 发起人ID
  /// </summary>
  public long InitiatorId { get; set; }

  /// <summary>
  /// 发起人姓名
  /// </summary>
  public string InitiatorName { get; set; }

  /// <summary>
  /// 发起人部门ID
  /// </summary>
  public long InitiatorDeptId { get; set; }

  /// <summary>
  /// 发起人部门名称
  /// </summary>
  public string InitiatorDeptName { get; set; }

  /// <summary>
  /// 流程变量
  /// </summary>
  public Dictionary<string, object> Variables { get; set; }

  /// <summary>
  /// 表单数据
  /// </summary>
  public Dictionary<string, object> FormData { get; set; }
}