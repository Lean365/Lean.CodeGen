//===================================================
// 项目名: Lean.CodeGen.Domain
// 文件名: LeanTenant.cs
// 功能描述: 租户实体
// 创建时间: 2024-03-30
// 作者: Lean
// 版本: 1.0
//===================================================

using Lean.CodeGen.Domain.Entities;
using SqlSugar;
using System;

namespace Lean.CodeGen.Domain.Entities.Identity;

/// <summary>
/// 租户实体
/// </summary>
[SugarTable("lean_id_tenant", "租户表")]
[SugarIndex("uk_code", nameof(TenantCode), OrderByType.Asc, true)]
[SugarIndex("idx_name", nameof(TenantName), OrderByType.Asc)]
public class LeanTenant : LeanBaseEntity
{
  #region 基础标识字段
  /// <summary>
  /// 租户编码
  /// </summary>
  /// <remarks>
  /// 租户的唯一标识编码
  /// </remarks>
  [SugarColumn(ColumnName = "tenant_code", ColumnDescription = "租户编码", Length = 50, IsNullable = false, UniqueGroupNameList = new[] { "uk_tenant_code" }, ColumnDataType = "nvarchar")]
  public string TenantCode { get; set; } = string.Empty;

  /// <summary>
  /// 租户域名
  /// </summary>
  /// <remarks>
  /// 租户的域名或URL前缀，用于URL路由
  /// </remarks>
  [SugarColumn(ColumnName = "tenant_domain", ColumnDescription = "租户域名", Length = 100, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? TenantDomain { get; set; }

  /// <summary>
  /// 租户Logo
  /// </summary>
  /// <remarks>
  /// 租户的Logo图片URL
  /// </remarks>
  [SugarColumn(ColumnName = "tenant_logo", ColumnDescription = "租户Logo", Length = 500, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? TenantLogo { get; set; }

  /// <summary>
  /// 租户类型
  /// </summary>
  /// <remarks>
  /// 租户类型：0-企业，1-个人，2-政府，3-其他
  /// </remarks>
  [SugarColumn(ColumnName = "tenant_type", ColumnDescription = "租户类型", IsNullable = false, DefaultValue = "0", ColumnDataType = "int")]
  public int TenantType { get; set; }
  #endregion

  #region 基本信息字段
  /// <summary>
  /// 租户名称
  /// </summary>
  /// <remarks>
  /// 租户的显示名称
  /// </remarks>
  [SugarColumn(ColumnName = "tenant_name", ColumnDescription = "租户名称", Length = 100, IsNullable = false, ColumnDataType = "nvarchar")]
  public string TenantName { get; set; } = string.Empty;

  /// <summary>
  /// 租户简称
  /// </summary>
  /// <remarks>
  /// 租户的简称，用于显示
  /// </remarks>
  [SugarColumn(ColumnName = "tenant_short_name", ColumnDescription = "租户简称", Length = 50, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? TenantShortName { get; set; }

  /// <summary>
  /// 租户描述
  /// </summary>
  /// <remarks>
  /// 租户的详细描述信息
  /// </remarks>
  [SugarColumn(ColumnName = "tenant_description", ColumnDescription = "租户描述", Length = 500, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? TenantDescription { get; set; }

  /// <summary>
  /// 所有者ID
  /// </summary>
  /// <remarks>
  /// 租户的所有者用户ID
  /// </remarks>
  [SugarColumn(ColumnName = "tenant_owner_id", ColumnDescription = "所有者ID", IsNullable = true, ColumnDataType = "bigint")]
  public long? TenantOwnerId { get; set; }

  /// <summary>
  /// 计划ID
  /// </summary>
  /// <remarks>
  /// 租户订阅的服务计划ID
  /// </remarks>
  [SugarColumn(ColumnName = "tenant_plan_id", ColumnDescription = "计划ID", IsNullable = true, ColumnDataType = "bigint")]
  public long? TenantPlanId { get; set; }
  #endregion

  #region 状态相关字段
  /// <summary>
  /// 租户状态
  /// </summary>
  /// <remarks>
  /// 租户状态：0-正常，1-停用，2-过期
  /// </remarks>
  [SugarColumn(ColumnName = "tenant_status", ColumnDescription = "租户状态", IsNullable = false, DefaultValue = "0", ColumnDataType = "int")]
  public int TenantStatus { get; set; }

  /// <summary>
  /// 是否内置
  /// </summary>
  /// <remarks>
  /// 是否为系统内置租户：0-否，1-是
  /// </remarks>
  [SugarColumn(ColumnName = "is_builtin", ColumnDescription = "是否内置", IsNullable = false, DefaultValue = "0", ColumnDataType = "int")]
  public int IsBuiltin { get; set; }
  #endregion

  #region 时间相关字段
  /// <summary>
  /// 开始时间
  /// </summary>
  /// <remarks>
  /// 租户的开始时间
  /// </remarks>
  [SugarColumn(ColumnName = "tenant_start_time", ColumnDescription = "开始时间", IsNullable = false, ColumnDataType = "datetime")]
  public DateTime TenantStartTime { get; set; }

  /// <summary>
  /// 过期时间
  /// </summary>
  /// <remarks>
  /// 租户的过期时间
  /// </remarks>
  [SugarColumn(ColumnName = "tenant_expire_time", ColumnDescription = "过期时间", IsNullable = true, ColumnDataType = "datetime")]
  public DateTime? TenantExpireTime { get; set; }

  /// <summary>
  /// 试用结束时间
  /// </summary>
  /// <remarks>
  /// 租户的试用结束时间
  /// </remarks>
  [SugarColumn(ColumnName = "tenant_trial_end_time", ColumnDescription = "试用结束时间", IsNullable = true, ColumnDataType = "datetime")]
  public DateTime? TenantTrialEndTime { get; set; }
  #endregion

  #region 配置相关字段
  /// <summary>
  /// 租户配置
  /// </summary>
  /// <remarks>
  /// 租户的配置信息，JSON格式
  /// </remarks>
  [SugarColumn(ColumnName = "tenant_config", ColumnDescription = "租户配置", Length = 4000, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? TenantConfig { get; set; }

  /// <summary>
  /// 计费信息
  /// </summary>
  /// <remarks>
  /// 租户的计费相关信息，JSON格式
  /// </remarks>
  [SugarColumn(ColumnName = "tenant_billing_info", ColumnDescription = "计费信息", Length = 2000, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? TenantBillingInfo { get; set; }

  /// <summary>
  /// 联系人信息
  /// </summary>
  /// <remarks>
  /// 租户的联系人信息，JSON格式
  /// </remarks>
  [SugarColumn(ColumnName = "tenant_contact_info", ColumnDescription = "联系人信息", Length = 2000, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? TenantContactInfo { get; set; }
  #endregion

  #region 关系字段
  /// <summary>
  /// 租户用户列表
  /// </summary>
  /// <remarks>
  /// 租户与用户的一对多关系
  /// </remarks>
  [Navigate(NavigateType.OneToMany, nameof(LeanUser.TenantId))]
  public virtual List<LeanUser> TenantUsers { get; set; } = new();
  #endregion
}