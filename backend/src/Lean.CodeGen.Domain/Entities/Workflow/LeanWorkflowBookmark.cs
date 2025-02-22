using SqlSugar;

namespace Lean.CodeGen.Domain.Entities.Workflow;

/// <summary>
/// 工作流书签实体
/// </summary>
[SugarTable("lean_workflow_bookmark", "工作流书签表")]
[SugarIndex("idx_instance", nameof(InstanceId), OrderByType.Asc)]
public class LeanWorkflowBookmark : LeanBaseEntity
{
  /// <summary>
  /// 工作流实例ID
  /// </summary>
  [SugarColumn(ColumnName = "instance_id", ColumnDescription = "工作流实例ID", IsNullable = false)]
  public long InstanceId { get; set; }

  /// <summary>
  /// 工作流实例
  /// </summary>
  [Navigate(NavigateType.OneToOne, nameof(InstanceId))]
  public virtual LeanWorkflowInstance Instance { get; set; } = null!;

  /// <summary>
  /// 活动ID
  /// </summary>
  [SugarColumn(ColumnName = "activity_id", ColumnDescription = "活动ID", Length = 50, IsNullable = false)]
  public string ActivityId { get; set; } = string.Empty;

  /// <summary>
  /// 活动名称
  /// </summary>
  [SugarColumn(ColumnName = "activity_name", ColumnDescription = "活动名称", Length = 100, IsNullable = false)]
  public string ActivityName { get; set; } = string.Empty;

  /// <summary>
  /// 书签名称
  /// </summary>
  [SugarColumn(ColumnName = "bookmark_name", ColumnDescription = "书签名称", Length = 100, IsNullable = false)]
  public string BookmarkName { get; set; } = string.Empty;

  /// <summary>
  /// 书签数据JSON
  /// </summary>
  [SugarColumn(ColumnName = "bookmark_data", ColumnDescription = "书签数据JSON", IsNullable = true)]
  public string? BookmarkData { get; set; }

  /// <summary>
  /// 关联键
  /// </summary>
  [SugarColumn(ColumnName = "correlation_id", ColumnDescription = "关联键", Length = 50, IsNullable = true)]
  public string? CorrelationId { get; set; }

  /// <summary>
  /// 书签状态(0=无效,1=有效)
  /// </summary>
  [SugarColumn(ColumnName = "status", ColumnDescription = "书签状态", IsNullable = false, DefaultValue = "1")]
  public int Status { get; set; } = 1;

  /// <summary>
  /// 过期时间
  /// </summary>
  [SugarColumn(ColumnName = "expire_time", ColumnDescription = "过期时间", IsNullable = true)]
  public DateTime? ExpireTime { get; set; }

  /// <summary>
  /// 自定义属性JSON
  /// </summary>
  [SugarColumn(ColumnName = "custom_attributes", ColumnDescription = "自定义属性JSON", IsNullable = true)]
  public string? CustomAttributes { get; set; }
}