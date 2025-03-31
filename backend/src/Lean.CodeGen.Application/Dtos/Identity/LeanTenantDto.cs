using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Application.Dtos;
using Lean.CodeGen.Common.Excel;

namespace Lean.CodeGen.Application.Dtos.Identity;

/// <summary>
/// 租户基础信息
/// </summary>
public class LeanTenantDto : LeanBaseDto
{
  #region 基础标识字段
  /// <summary>
  /// 租户编码
  /// </summary>
  /// <remarks>
  /// 租户的唯一标识编码
  /// </remarks>
  public string TenantCode { get; set; } = string.Empty;

  /// <summary>
  /// 租户域名
  /// </summary>
  /// <remarks>
  /// 租户的域名或URL前缀，用于URL路由
  /// </remarks>
  public string? TenantDomain { get; set; }

  /// <summary>
  /// 租户Logo
  /// </summary>
  /// <remarks>
  /// 租户的Logo图片URL
  /// </remarks>
  public string? TenantLogo { get; set; }

  /// <summary>
  /// 租户类型
  /// </summary>
  /// <remarks>
  /// 租户类型：0-企业，1-个人，2-政府，3-其他
  /// </remarks>
  public int TenantType { get; set; }
  #endregion

  #region 基本信息字段
  /// <summary>
  /// 租户名称
  /// </summary>
  /// <remarks>
  /// 租户的显示名称
  /// </remarks>
  public string TenantName { get; set; } = string.Empty;

  /// <summary>
  /// 租户简称
  /// </summary>
  /// <remarks>
  /// 租户的简称，用于显示
  /// </remarks>
  public string? TenantShortName { get; set; }

  /// <summary>
  /// 租户描述
  /// </summary>
  /// <remarks>
  /// 租户的详细描述信息
  /// </remarks>
  public string? TenantDescription { get; set; }

  /// <summary>
  /// 所有者ID
  /// </summary>
  /// <remarks>
  /// 租户的所有者用户ID
  /// </remarks>
  public long? TenantOwnerId { get; set; }

  /// <summary>
  /// 计划ID
  /// </summary>
  /// <remarks>
  /// 租户订阅的服务计划ID
  /// </remarks>
  public long? TenantPlanId { get; set; }
  #endregion

  #region 状态相关字段
  /// <summary>
  /// 租户状态
  /// </summary>
  /// <remarks>
  /// 租户状态：0-正常，1-停用，2-过期
  /// </remarks>
  public int TenantStatus { get; set; }

  /// <summary>
  /// 是否内置
  /// </summary>
  /// <remarks>
  /// 是否为系统内置租户：0-否，1-是
  /// </remarks>
  public int IsBuiltin { get; set; }
  #endregion

  #region 时间相关字段
  /// <summary>
  /// 开始时间
  /// </summary>
  /// <remarks>
  /// 租户的开始时间
  /// </remarks>
  public DateTime TenantStartTime { get; set; }

  /// <summary>
  /// 过期时间
  /// </summary>
  /// <remarks>
  /// 租户的过期时间
  /// </remarks>
  public DateTime? TenantExpireTime { get; set; }

  /// <summary>
  /// 试用结束时间
  /// </summary>
  /// <remarks>
  /// 租户的试用结束时间
  /// </remarks>
  public DateTime? TenantTrialEndTime { get; set; }
  #endregion

  #region 配置相关字段
  /// <summary>
  /// 租户配置
  /// </summary>
  /// <remarks>
  /// 租户的配置信息，JSON格式
  /// </remarks>
  public string? TenantConfig { get; set; }

  /// <summary>
  /// 计费信息
  /// </summary>
  /// <remarks>
  /// 租户的计费相关信息，JSON格式
  /// </remarks>
  public string? TenantBillingInfo { get; set; }

  /// <summary>
  /// 联系人信息
  /// </summary>
  /// <remarks>
  /// 租户的联系人信息，JSON格式
  /// </remarks>
  public string? TenantContactInfo { get; set; }
  #endregion
}

/// <summary>
/// 租户查询参数
/// </summary>
public class LeanTenantQueryDto : LeanPage
{
  /// <summary>
  /// ID
  /// </summary>
  public long? Id { get; set; }

  /// <summary>
  /// 租户编码
  /// </summary>
  public string? TenantCode { get; set; }

  /// <summary>
  /// 租户名称
  /// </summary>
  public string? TenantName { get; set; }

  /// <summary>
  /// 租户类型
  /// 0-企业
  /// 1-个人
  /// </summary>
  public int? TenantType { get; set; }

  /// <summary>
  /// 联系人
  /// </summary>
  public string? ContactPerson { get; set; }

  /// <summary>
  /// 联系电话
  /// </summary>
  public string? ContactPhone { get; set; }

  /// <summary>
  /// 邮箱
  /// </summary>
  public string? Email { get; set; }

  /// <summary>
  /// 租户状态
  /// 0-正常
  /// 1-停用
  /// </summary>
  public int? TenantStatus { get; set; }

  /// <summary>
  /// 是否内置
  /// 0-否
  /// 1-是
  /// </summary>
  public int? IsBuiltin { get; set; }

  /// <summary>
  /// 创建者ID
  /// </summary>
  public long? CreateUserId { get; set; }

  /// <summary>
  /// 创建者
  /// </summary>
  public string? CreateUserName { get; set; }

  /// <summary>
  /// 创建时间范围-开始
  /// </summary>
  public DateTime? StartTime { get; set; }

  /// <summary>
  /// 创建时间范围-结束
  /// </summary>
  public DateTime? EndTime { get; set; }

  /// <summary>
  /// 更新者ID
  /// </summary>
  public long? UpdateUserId { get; set; }

  /// <summary>
  /// 更新者
  /// </summary>
  public string? UpdateUserName { get; set; }

  /// <summary>
  /// 更新时间范围-开始
  /// </summary>
  public DateTime? UpdateStartTime { get; set; }

  /// <summary>
  /// 更新时间范围-结束
  /// </summary>
  public DateTime? UpdateEndTime { get; set; }

  /// <summary>
  /// 审核状态
  /// 0-未审核
  /// 1-已审核
  /// 2-已驳回
  /// </summary>
  public int? AuditStatus { get; set; }

  /// <summary>
  /// 审核人员ID
  /// </summary>
  public long? AuditUserId { get; set; }

  /// <summary>
  /// 审核人员
  /// </summary>
  public string? AuditUserName { get; set; }

  /// <summary>
  /// 审核时间范围-开始
  /// </summary>
  public DateTime? AuditStartTime { get; set; }

  /// <summary>
  /// 审核时间范围-结束
  /// </summary>
  public DateTime? AuditEndTime { get; set; }

  /// <summary>
  /// 审核意见
  /// </summary>
  public string? AuditOpinion { get; set; }

  /// <summary>
  /// 撤销人ID
  /// </summary>
  public long? RevokeUserId { get; set; }

  /// <summary>
  /// 撤销人
  /// </summary>
  public string? RevokeUserName { get; set; }

  /// <summary>
  /// 撤销时间范围-开始
  /// </summary>
  public DateTime? RevokeStartTime { get; set; }

  /// <summary>
  /// 撤销时间范围-结束
  /// </summary>
  public DateTime? RevokeEndTime { get; set; }

  /// <summary>
  /// 撤销意见
  /// </summary>
  public string? RevokeOpinion { get; set; }

  /// <summary>
  /// 是否删除
  /// 0-否
  /// 1-是
  /// </summary>
  public int? IsDeleted { get; set; }

  /// <summary>
  /// 删除人ID
  /// </summary>
  public long? DeleteUserId { get; set; }

  /// <summary>
  /// 删除人
  /// </summary>
  public string? DeleteUserName { get; set; }

  /// <summary>
  /// 删除时间范围-开始
  /// </summary>
  public DateTime? DeleteStartTime { get; set; }

  /// <summary>
  /// 删除时间范围-结束
  /// </summary>
  public DateTime? DeleteEndTime { get; set; }

  /// <summary>
  /// 备注
  /// </summary>
  public string? Remark { get; set; }
}

/// <summary>
/// 租户创建参数
/// </summary>
public class LeanTenantCreateDto
{
  /// <summary>
  /// 租户编码
  /// </summary>
  [Required(ErrorMessage = "租户编码不能为空")]
  [StringLength(50, ErrorMessage = "租户编码长度不能超过50个字符")]
  public string TenantCode { get; set; } = string.Empty;

  /// <summary>
  /// 租户名称
  /// </summary>
  [Required(ErrorMessage = "租户名称不能为空")]
  [StringLength(100, ErrorMessage = "租户名称长度不能超过100个字符")]
  public string TenantName { get; set; } = string.Empty;

  /// <summary>
  /// 租户类型
  /// 0-企业
  /// 1-个人
  /// </summary>
  public int TenantType { get; set; } = 0;

  /// <summary>
  /// 联系人
  /// </summary>
  [StringLength(50, ErrorMessage = "联系人长度不能超过50个字符")]
  public string? ContactPerson { get; set; }

  /// <summary>
  /// 联系电话
  /// </summary>
  [Phone(ErrorMessage = "联系电话格式不正确")]
  public string? ContactPhone { get; set; }

  /// <summary>
  /// 邮箱
  /// </summary>
  [EmailAddress(ErrorMessage = "邮箱格式不正确")]
  public string? Email { get; set; }

  /// <summary>
  /// 地址
  /// </summary>
  [StringLength(500, ErrorMessage = "地址长度不能超过500个字符")]
  public string? Address { get; set; }

  /// <summary>
  /// 租户状态
  /// 0-正常
  /// 1-停用
  /// </summary>
  public int TenantStatus { get; set; } = 0;

  /// <summary>
  /// 是否内置
  /// 0-否
  /// 1-是
  /// </summary>
  public int IsBuiltin { get; set; } = 0;

  /// <summary>
  /// 过期时间
  /// </summary>
  public DateTime? ExpireTime { get; set; }

  /// <summary>
  /// 备注
  /// </summary>
  [StringLength(500, ErrorMessage = "备注长度不能超过500个字符")]
  public string? Remark { get; set; }
}

/// <summary>
/// 租户更新参数
/// </summary>
public class LeanTenantUpdateDto
{
  /// <summary>
  /// 租户ID
  /// </summary>
  [Required(ErrorMessage = "租户ID不能为空")]
  public long Id { get; set; }

  /// <summary>
  /// 租户名称
  /// </summary>
  [Required(ErrorMessage = "租户名称不能为空")]
  [StringLength(100, ErrorMessage = "租户名称长度不能超过100个字符")]
  public string TenantName { get; set; } = string.Empty;

  /// <summary>
  /// 租户类型
  /// 0-企业
  /// 1-个人
  /// </summary>
  public int TenantType { get; set; }

  /// <summary>
  /// 联系人
  /// </summary>
  [StringLength(50, ErrorMessage = "联系人长度不能超过50个字符")]
  public string? ContactPerson { get; set; }

  /// <summary>
  /// 联系电话
  /// </summary>
  [Phone(ErrorMessage = "联系电话格式不正确")]
  public string? ContactPhone { get; set; }

  /// <summary>
  /// 邮箱
  /// </summary>
  [EmailAddress(ErrorMessage = "邮箱格式不正确")]
  public string? Email { get; set; }

  /// <summary>
  /// 地址
  /// </summary>
  [StringLength(500, ErrorMessage = "地址长度不能超过500个字符")]
  public string? Address { get; set; }

  /// <summary>
  /// 租户状态
  /// 0-正常
  /// 1-停用
  /// </summary>
  public int TenantStatus { get; set; }

  /// <summary>
  /// 过期时间
  /// </summary>
  public DateTime? ExpireTime { get; set; }

  /// <summary>
  /// 备注
  /// </summary>
  [StringLength(500, ErrorMessage = "备注长度不能超过500个字符")]
  public string? Remark { get; set; }
}

/// <summary>
/// 租户状态变更参数
/// </summary>
public class LeanTenantChangeStatusDto
{
  /// <summary>
  /// 租户ID
  /// </summary>
  [Required(ErrorMessage = "租户ID不能为空")]
  public long Id { get; set; }

  /// <summary>
  /// 租户状态
  /// 0-正常
  /// 1-停用
  /// </summary>
  [Required(ErrorMessage = "租户状态不能为空")]
  public int TenantStatus { get; set; }
}

/// <summary>
/// 租户导入模板
/// </summary>
public class LeanTenantImportTemplateDto
{
  /// <summary>
  /// 租户编码
  /// </summary>
  [Required(ErrorMessage = "租户编码不能为空")]
  [StringLength(50, ErrorMessage = "租户编码长度不能超过50个字符")]
  public string TenantCode { get; set; } = string.Empty;

  /// <summary>
  /// 租户名称
  /// </summary>
  [Required(ErrorMessage = "租户名称不能为空")]
  [StringLength(100, ErrorMessage = "租户名称长度不能超过100个字符")]
  public string TenantName { get; set; } = string.Empty;

  /// <summary>
  /// 租户类型
  /// 0-企业
  /// 1-个人
  /// </summary>
  public int TenantType { get; set; } = 0;

  /// <summary>
  /// 联系人
  /// </summary>
  [StringLength(50, ErrorMessage = "联系人长度不能超过50个字符")]
  public string? ContactPerson { get; set; }

  /// <summary>
  /// 联系电话
  /// </summary>
  [Phone(ErrorMessage = "联系电话格式不正确")]
  public string? ContactPhone { get; set; }

  /// <summary>
  /// 邮箱
  /// </summary>
  [EmailAddress(ErrorMessage = "邮箱格式不正确")]
  public string? Email { get; set; }
}

/// <summary>
/// 租户导入错误
/// </summary>
public class LeanTenantImportErrorDto : LeanImportError
{
  /// <summary>
  /// 租户编码
  /// </summary>
  public string TenantCode { get; set; } = default!;
}

/// <summary>
/// 租户导入结果
/// </summary>
public class LeanTenantImportResultDto : LeanImportResult
{
  /// <summary>
  /// 错误列表
  /// </summary>
  public new List<LeanTenantImportErrorDto> Errors { get; set; } = new();

  /// <summary>
  /// 添加错误
  /// </summary>
  /// <param name="tenantCode">租户编码</param>
  /// <param name="errorMessage">错误信息</param>
  public override void AddError(string tenantCode, string errorMessage)
  {
    Errors.Add(new LeanTenantImportErrorDto
    {
      TenantCode = tenantCode,
      ErrorMessage = errorMessage
    });
  }
}

/// <summary>
/// 租户导出查询参数
/// </summary>
public class LeanTenantExportQueryDto : LeanTenantQueryDto
{
  /// <summary>
  /// 导出字段列表
  /// </summary>
  public List<string> ExportFields { get; set; } = new();

  /// <summary>
  /// 文件格式
  /// </summary>
  [Required(ErrorMessage = "文件格式不能为空")]
  public string FileFormat { get; set; } = string.Empty;

  /// <summary>
  /// 是否导出全部
  /// 0-否
  /// 1-是
  /// </summary>
  public int IsExportAll { get; set; }

  /// <summary>
  /// 选中ID列表
  /// </summary>
  public List<long> SelectedIds { get; set; } = new();
}

/// <summary>
/// 租户删除参数
/// </summary>
public class LeanTenantDeleteDto
{
  /// <summary>
  /// 租户ID
  /// </summary>
  [Required(ErrorMessage = "租户ID不能为空")]
  public long Id { get; set; }
}

/// <summary>
/// 租户导出
/// </summary>
public class LeanTenantExportDto
{
  /// <summary>
  /// 租户编码
  /// </summary>
  [LeanExcelColumn("租户编码", DataType = LeanExcelDataType.String)]
  public string TenantCode { get; set; } = string.Empty;

  /// <summary>
  /// 租户名称
  /// </summary>
  [LeanExcelColumn("租户名称", DataType = LeanExcelDataType.String)]
  public string TenantName { get; set; } = string.Empty;

  /// <summary>
  /// 租户类型
  /// </summary>
  [LeanExcelColumn("租户类型", DataType = LeanExcelDataType.Int)]
  public int TenantType { get; set; }

  /// <summary>
  /// 联系人
  /// </summary>
  [LeanExcelColumn("联系人", DataType = LeanExcelDataType.String)]
  public string? ContactPerson { get; set; }

  /// <summary>
  /// 联系电话
  /// </summary>
  [LeanExcelColumn("联系电话", DataType = LeanExcelDataType.String)]
  public string? ContactPhone { get; set; }

  /// <summary>
  /// 邮箱
  /// </summary>
  [LeanExcelColumn("邮箱", DataType = LeanExcelDataType.String)]
  public string? Email { get; set; }

  /// <summary>
  /// 租户状态
  /// </summary>
  [LeanExcelColumn("租户状态", DataType = LeanExcelDataType.Int)]
  public int TenantStatus { get; set; }

  /// <summary>
  /// 是否内置
  /// </summary>
  [LeanExcelColumn("是否内置", DataType = LeanExcelDataType.Int)]
  public int IsBuiltin { get; set; }

  /// <summary>
  /// 创建时间
  /// </summary>
  [LeanExcelColumn("创建时间", DataType = LeanExcelDataType.DateTime)]
  public DateTime CreateTime { get; set; }
}

/// <summary>
/// 租户导入
/// </summary>
public class LeanTenantImportDto
{
  /// <summary>
  /// 租户编码
  /// </summary>
  [LeanExcelColumn("租户编码")]
  public string TenantCode { get; set; } = string.Empty;

  /// <summary>
  /// 租户名称
  /// </summary>
  [LeanExcelColumn("租户名称")]
  public string TenantName { get; set; } = string.Empty;

  /// <summary>
  /// 租户类型
  /// </summary>
  [LeanExcelColumn("租户类型")]
  public int TenantType { get; set; }

  /// <summary>
  /// 联系人
  /// </summary>
  [LeanExcelColumn("联系人")]
  public string? ContactPerson { get; set; }

  /// <summary>
  /// 联系电话
  /// </summary>
  [LeanExcelColumn("联系电话")]
  public string? ContactPhone { get; set; }

  /// <summary>
  /// 邮箱
  /// </summary>
  [LeanExcelColumn("邮箱")]
  public string? Email { get; set; }
}
