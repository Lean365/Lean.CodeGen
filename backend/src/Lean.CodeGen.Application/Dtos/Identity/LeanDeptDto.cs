using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Application.Dtos;
using Lean.CodeGen.Common.Excel;

namespace Lean.CodeGen.Application.Dtos.Identity;

/// <summary>
/// 部门基础信息
/// </summary>
public class LeanDeptDto : LeanBaseDto
{
  /// <summary>
  /// 父级ID
  /// </summary>
  public long? ParentId { get; set; }

  /// <summary>
  /// 部门名称
  /// </summary>
  [LeanExcelColumn("部门名称")]
  [Required]
  public string DeptName { get; set; } = string.Empty;

  /// <summary>
  /// 部门编码
  /// </summary>
  [LeanExcelColumn("部门编码")]
  [Required]
  public string DeptCode { get; set; } = string.Empty;

  /// <summary>
  /// 部门描述
  /// </summary>
  public string? DeptDescription { get; set; }

  /// <summary>
  /// 负责人
  /// </summary>
  public string? Leader { get; set; }

  /// <summary>
  /// 联系电话
  /// </summary>
  public string? Phone { get; set; }

  /// <summary>
  /// 邮箱
  /// </summary>
  public string? Email { get; set; }

  /// <summary>
  /// 排序号
  /// </summary>
  public int OrderNum { get; set; }

  /// <summary>
  /// 部门状态（0-正常，1-停用）
  /// </summary>
  public int DeptStatus { get; set; }

  /// <summary>
  /// 是否内置（0-否，1-是）
  /// </summary>
  public int IsBuiltin { get; set; }

  /// <summary>
  /// 数据权限范围（0-全部数据权限，1-自定义数据权限，2-本部门数据权限，3-本部门及以下数据权限，4-仅本人数据权限）
  /// </summary>
  public int DataScope { get; set; }



  /// <summary>
  /// 子部门列表
  /// </summary>
  public List<LeanDeptDto> Children { get; set; } = new();

  /// <summary>
  /// 创建时间
  /// </summary>
  public DateTime CreateTime { get; set; }

  /// <summary>
  /// 更新时间
  /// </summary>
  public DateTime? UpdateTime { get; set; }
}

/// <summary>
/// 部门查询参数
/// </summary>
public class LeanDeptQueryDto : LeanPage
{
  /// <summary>
  /// ID
  /// </summary>
  public long? Id { get; set; }

  /// <summary>
  /// 租户ID
  /// </summary>
  public long? TenantId { get; set; }

  /// <summary>
  /// 父级ID
  /// </summary>
  public long? ParentId { get; set; }

  /// <summary>
  /// 部门名称
  /// </summary>
  public string? DeptName { get; set; }

  /// <summary>
  /// 部门编码
  /// </summary>
  public string? DeptCode { get; set; }

  /// <summary>
  /// 部门描述
  /// </summary>
  public string? DeptDescription { get; set; }

  /// <summary>
  /// 负责人
  /// </summary>
  public string? Leader { get; set; }

  /// <summary>
  /// 联系电话
  /// </summary>
  public string? Phone { get; set; }

  /// <summary>
  /// 邮箱
  /// </summary>
  public string? Email { get; set; }

  /// <summary>
  /// 排序号
  /// </summary>
  public int? OrderNum { get; set; }

  /// <summary>
  /// 部门状态（0-正常，1-停用）
  /// </summary>
  public int? DeptStatus { get; set; }

  /// <summary>
  /// 是否内置（0-否，1-是）
  /// </summary>
  public int? IsBuiltin { get; set; }

  /// <summary>
  /// 数据权限范围（0-全部数据权限，1-自定义数据权限，2-本部门数据权限，3-本部门及以下数据权限，4-仅本人数据权限）
  /// </summary>
  public int? DataScope { get; set; }

  /// <summary>
  /// 状态（0-正常，1-停用）
  /// </summary>


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
  /// 审核状态（0-未审核，1-已审核，2-已驳回）
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
  /// 撤销人员ID
  /// </summary>
  public long? RevokeUserId { get; set; }

  /// <summary>
  /// 撤销人员
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
  /// </summary>
  public int? IsDeleted { get; set; }

  /// <summary>
  /// 删除者ID
  /// </summary>
  public long? DeleteUserId { get; set; }

  /// <summary>
  /// 删除者
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
/// 部门创建参数
/// </summary>
public class LeanDeptCreateDto
{
  /// <summary>
  /// 父级ID
  /// </summary>
  public long? ParentId { get; set; }

  /// <summary>
  /// 部门名称
  /// </summary>
  [Required(ErrorMessage = "部门名称不能为空")]
  [StringLength(50, MinimumLength = 2, ErrorMessage = "部门名称长度必须在2-50个字符之间")]
  public string DeptName { get; set; } = string.Empty;

  /// <summary>
  /// 部门编码
  /// </summary>
  [Required(ErrorMessage = "部门编码不能为空")]
  [StringLength(50, MinimumLength = 2, ErrorMessage = "部门编码长度必须在2-50个字符之间")]
  public string DeptCode { get; set; } = string.Empty;

  /// <summary>
  /// 部门描述
  /// </summary>
  [StringLength(500, ErrorMessage = "部门描述长度不能超过500个字符")]
  public string? DeptDescription { get; set; }

  /// <summary>
  /// 负责人
  /// </summary>
  [StringLength(50, ErrorMessage = "负责人长度不能超过50个字符")]
  public string? Leader { get; set; }

  /// <summary>
  /// 联系电话
  /// </summary>
  [Phone(ErrorMessage = "联系电话格式不正确")]
  [StringLength(20, ErrorMessage = "联系电话长度不能超过20个字符")]
  public string? Phone { get; set; }

  /// <summary>
  /// 邮箱
  /// </summary>
  [EmailAddress(ErrorMessage = "邮箱格式不正确")]
  [StringLength(100, ErrorMessage = "邮箱长度不能超过100个字符")]
  public string? Email { get; set; }

  /// <summary>
  /// 排序号
  /// </summary>
  public int OrderNum { get; set; }

  /// <summary>
  /// 部门状态（0-正常，1-停用）
  /// </summary>
  public int DeptStatus { get; set; } = 0;

  /// <summary>
  /// 是否内置（0-否，1-是）
  /// </summary>
  public int IsBuiltin { get; set; } = 0;

  /// <summary>
  /// 数据权限范围（0-全部数据权限，1-自定义数据权限，2-本部门数据权限，3-本部门及以下数据权限，4-仅本人数据权限）
  /// </summary>
  public int DataScope { get; set; } = 4;
}

/// <summary>
/// 部门更新参数
/// </summary>
public class LeanDeptUpdateDto
{
  /// <summary>
  /// 部门ID
  /// </summary>
  [Required(ErrorMessage = "部门ID不能为空")]
  public long Id { get; set; }

  /// <summary>
  /// 父级ID
  /// </summary>
  public long? ParentId { get; set; }

  /// <summary>
  /// 部门名称
  /// </summary>
  [Required(ErrorMessage = "部门名称不能为空")]
  [StringLength(50, MinimumLength = 2, ErrorMessage = "部门名称长度必须在2-50个字符之间")]
  public string DeptName { get; set; } = string.Empty;

  /// <summary>
  /// 部门描述
  /// </summary>
  [StringLength(500, ErrorMessage = "部门描述长度不能超过500个字符")]
  public string? DeptDescription { get; set; }

  /// <summary>
  /// 负责人
  /// </summary>
  [StringLength(50, ErrorMessage = "负责人长度不能超过50个字符")]
  public string? Leader { get; set; }

  /// <summary>
  /// 联系电话
  /// </summary>
  [Phone(ErrorMessage = "联系电话格式不正确")]
  [StringLength(20, ErrorMessage = "联系电话长度不能超过20个字符")]
  public string? Phone { get; set; }

  /// <summary>
  /// 邮箱
  /// </summary>
  [EmailAddress(ErrorMessage = "邮箱格式不正确")]
  [StringLength(100, ErrorMessage = "邮箱长度不能超过100个字符")]
  public string? Email { get; set; }

  /// <summary>
  /// 排序号
  /// </summary>
  public int OrderNum { get; set; }

  /// <summary>
  /// 部门状态（0-正常，1-停用）
  /// </summary>
  public int DeptStatus { get; set; }

  /// <summary>
  /// 数据权限范围（0-全部数据权限，1-自定义数据权限，2-本部门数据权限，3-本部门及以下数据权限，4-仅本人数据权限）
  /// </summary>
  public int DataScope { get; set; }
}

/// <summary>
/// 部门状态变更参数
/// </summary>
public class LeanDeptChangeStatusDto
{
  /// <summary>
  /// 部门ID
  /// </summary>
  [Required(ErrorMessage = "部门ID不能为空")]
  public long Id { get; set; }

  /// <summary>
  /// 部门状态（0-正常，1-停用）
  /// </summary>
  [Required(ErrorMessage = "部门状态不能为空")]
  public int DeptStatus { get; set; }
}

/// <summary>
/// 部门删除参数
/// </summary>
public class LeanDeptDeleteDto
{
  /// <summary>
  /// 部门ID
  /// </summary>
  [Required(ErrorMessage = "部门ID不能为空")]
  public long Id { get; set; }
}

/// <summary>
/// 部门导入模板参数
/// </summary>
public class LeanDeptImportTemplateDto
{
  /// <summary>
  /// 部门编码
  /// </summary>
  [Required(ErrorMessage = "部门编码不能为空")]
  [StringLength(50, MinimumLength = 2, ErrorMessage = "部门编码长度必须在2-50个字符之间")]
  public string DeptCode { get; set; } = string.Empty;

  /// <summary>
  /// 部门名称
  /// </summary>
  [Required(ErrorMessage = "部门名称不能为空")]
  [StringLength(50, MinimumLength = 2, ErrorMessage = "部门名称长度必须在2-50个字符之间")]
  public string DeptName { get; set; } = string.Empty;

  /// <summary>
  /// 上级部门编码
  /// </summary>
  [StringLength(50, ErrorMessage = "上级部门编码长度不能超过50个字符")]
  public string? ParentCode { get; set; }

  /// <summary>
  /// 部门描述
  /// </summary>
  [StringLength(500, ErrorMessage = "部门描述长度不能超过500个字符")]
  public string? DeptDescription { get; set; }

  /// <summary>
  /// 负责人
  /// </summary>
  [StringLength(50, ErrorMessage = "负责人长度不能超过50个字符")]
  public string? Leader { get; set; }

  /// <summary>
  /// 联系电话
  /// </summary>
  [Phone(ErrorMessage = "联系电话格式不正确")]
  [StringLength(20, ErrorMessage = "联系电话长度不能超过20个字符")]
  public string? Phone { get; set; }

  /// <summary>
  /// 邮箱
  /// </summary>
  [EmailAddress(ErrorMessage = "邮箱格式不正确")]
  [StringLength(100, ErrorMessage = "邮箱长度不能超过100个字符")]
  public string? Email { get; set; }

  /// <summary>
  /// 排序号
  /// </summary>
  public int OrderNum { get; set; }
}

/// <summary>
/// 部门导入错误参数
/// </summary>
public class LeanDeptImportErrorDto : LeanImportError
{
  /// <summary>
  /// 部门编码
  /// </summary>
  public string DeptCode
  {
    get => Key;
    set => Key = value;
  }
}

/// <summary>
/// 部门导入结果参数
/// </summary>
public class LeanDeptImportResultDto : LeanImportResult
{
  /// <summary>
  /// 错误信息列表
  /// </summary>
  public new List<LeanDeptImportErrorDto> Errors { get; set; } = new();

  /// <summary>
  /// 错误信息
  /// </summary>
  public string? ErrorMessage { get; set; }

  /// <summary>
  /// 添加错误信息
  /// </summary>
  public override void AddError(string deptCode, string errorMessage)
  {
    base.AddError(deptCode, errorMessage);
    Errors.Add(new LeanDeptImportErrorDto
    {
      RowIndex = TotalCount,
      DeptCode = deptCode,
      ErrorMessage = errorMessage
    });
  }
}

/// <summary>
/// 部门导出查询参数
/// </summary>
public class LeanDeptExportQueryDto : LeanDeptQueryDto
{
  /// <summary>
  /// 导出字段列表
  /// </summary>
  public List<string> ExportFields { get; set; } = new();

  /// <summary>
  /// 文件格式
  /// </summary>
  /// <remarks>
  /// 支持的格式：xlsx、csv
  /// </remarks>
  [Required(ErrorMessage = "文件格式不能为空")]
  public string FileFormat { get; set; } = "xlsx";

  /// <summary>
  /// 是否导出全部
  /// 0-否
  /// 1-是
  /// </summary>
  public int IsExportAll { get; set; }

  /// <summary>
  /// 选中的ID列表，当IsExportAll为false时使用
  /// </summary>
  public List<long> SelectedIds { get; set; } = new();
}

/// <summary>
/// 部门树形结构参数
/// </summary>
public class LeanDeptTreeDto : LeanDeptDto
{
  /// <summary>
  /// 子部门列表
  /// </summary>
  public new List<LeanDeptTreeDto> Children { get; set; } = new();
}

/// <summary>
/// 部门导入参数
/// </summary>
public class LeanDeptImportDto
{
  /// <summary>
  /// 部门编码
  /// </summary>
  [Required(ErrorMessage = "部门编码不能为空")]
  [StringLength(50, MinimumLength = 2, ErrorMessage = "部门编码长度必须在2-50个字符之间")]
  [LeanExcelColumn("部门编码", DataType = LeanExcelDataType.String)]
  public string DeptCode { get; set; } = default!;

  /// <summary>
  /// 部门名称
  /// </summary>
  [Required(ErrorMessage = "部门名称不能为空")]
  [StringLength(50, MinimumLength = 2, ErrorMessage = "部门名称长度必须在2-50个字符之间")]
  [LeanExcelColumn("部门名称", DataType = LeanExcelDataType.String)]
  public string DeptName { get; set; } = default!;

  /// <summary>
  /// 父级部门ID
  /// </summary>
  [LeanExcelColumn("父级部门ID", DataType = LeanExcelDataType.Long)]
  public long? ParentId { get; set; }

  /// <summary>
  /// 显示顺序
  /// </summary>
  [LeanExcelColumn("显示顺序", DataType = LeanExcelDataType.Int)]
  public int OrderNum { get; set; }

  /// <summary>
  /// 负责人
  /// </summary>
  [LeanExcelColumn("负责人", DataType = LeanExcelDataType.String)]
  public string? Leader { get; set; }

  /// <summary>
  /// 联系电话
  /// </summary>
  [LeanExcelColumn("联系电话", DataType = LeanExcelDataType.String)]
  public string? Phone { get; set; }

  /// <summary>
  /// 邮箱
  /// </summary>
  [LeanExcelColumn("邮箱", DataType = LeanExcelDataType.String)]
  [EmailAddress(ErrorMessage = "邮箱格式不正确")]
  public string? Email { get; set; }
}

/// <summary>
/// 角色菜单设置参数
/// </summary>
public class LeanRoleMenuSetDto
{
  /// <summary>
  /// 角色ID
  /// </summary>
  public long RoleId { get; set; }

  /// <summary>
  /// 菜单ID列表
  /// </summary>
  public List<long> MenuIds { get; set; }
}

/// <summary>
/// 部门导出参数
/// </summary>
public class LeanDeptExportDto : LeanDeptQueryDto
{
  /// <summary>
  /// 导出字段列表
  /// </summary>
  public List<string> ExportFields { get; set; } = new();

  /// <summary>
  /// 文件格式
  /// </summary>
  /// <remarks>
  /// 支持的格式：xlsx、csv
  /// </remarks>
  [Required(ErrorMessage = "文件格式不能为空")]
  public string FileFormat { get; set; } = "xlsx";

  /// <summary>
  /// 是否导出全部
  /// 0-否
  /// 1-是
  /// </summary>
  public int IsExportAll { get; set; }

  /// <summary>
  /// 选中的ID列表，当IsExportAll为false时使用
  /// </summary>
  public List<long> SelectedIds { get; set; } = new();
}