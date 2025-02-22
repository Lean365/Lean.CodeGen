namespace Lean.CodeGen.Common.Enums;

/// <summary>
/// 工作流状态枚举
/// </summary>
public enum LeanWorkflowStatus
{
  /// <summary>
  /// 草稿
  /// </summary>
  Draft = 0,

  /// <summary>
  /// 已发布
  /// </summary>
  Published = 1,

  /// <summary>
  /// 已禁用
  /// </summary>
  Disabled = 2,

  /// <summary>
  /// 已归档
  /// </summary>
  Archived = 3
}