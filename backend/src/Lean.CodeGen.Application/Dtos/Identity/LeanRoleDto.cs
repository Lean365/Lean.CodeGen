using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Common.Models;

namespace Lean.CodeGen.Application.Dtos.Identity;

/// <summary>
/// 角色信息
/// </summary>
public class LeanRoleDto
{
  /// <summary>
  /// 角色ID
  /// </summary>
  public long Id { get; set; }

  /// <summary>
  /// 角色名称
  /// </summary>
  public string RoleName { get; set; }

  /// <summary>
  /// 角色编码
  /// </summary>
  public string RoleCode { get; set; }

  /// <summary>
  /// 角色状态
  /// </summary>
  public LeanRoleStatus RoleStatus { get; set; }

  /// <summary>
  /// 是否内置
  /// </summary>
  public LeanBuiltinStatus IsBuiltin { get; set; }

  /// <summary>
  /// 数据权限类型
  /// </summary>
  public LeanDataScopeType DataScope { get; set; }

  /// <summary>
  /// 备注
  /// </summary>
  public string Remark { get; set; }

  /// <summary>
  /// 菜单ID列表
  /// </summary>
  public List<long> MenuIds { get; set; } = new();

  /// <summary>
  /// 数据权限部门ID列表
  /// </summary>
  public List<long> DataScopeDeptIds { get; set; } = new();

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
/// 角色查询参数
/// </summary>
public class LeanQueryRoleDto : LeanPage
{
  /// <summary>
  /// 父级ID
  /// </summary>
  public long? ParentId { get; set; }

  /// <summary>
  /// 角色名称
  /// </summary>
  public string? RoleName { get; set; }

  /// <summary>
  /// 角色编码
  /// </summary>
  public string? RoleCode { get; set; }

  /// <summary>
  /// 状态
  /// </summary>
  public LeanRoleStatus? RoleStatus { get; set; }

  /// <summary>
  /// 数据权限范围
  /// </summary>
  public LeanDataScopeType? DataScope { get; set; }

  /// <summary>
  /// 是否内置
  /// </summary>
  public LeanBuiltinStatus? IsBuiltin { get; set; }

  /// <summary>
  /// 创建时间范围-开始
  /// </summary>
  public DateTime? StartTime { get; set; }

  /// <summary>
  /// 创建时间范围-结束
  /// </summary>
  public DateTime? EndTime { get; set; }
}

/// <summary>
/// 角色创建参数
/// </summary>
public class LeanCreateRoleDto
{
  /// <summary>
  /// 父级ID
  /// </summary>
  public long? ParentId { get; set; }

  /// <summary>
  /// 角色名称
  /// </summary>
  [Required(ErrorMessage = "角色名称不能为空")]
  [StringLength(50, MinimumLength = 2, ErrorMessage = "角色名称长度必须在2-50个字符之间")]
  public string RoleName { get; set; } = string.Empty;

  /// <summary>
  /// 角色编码
  /// </summary>
  [Required(ErrorMessage = "角色编码不能为空")]
  [StringLength(50, MinimumLength = 2, ErrorMessage = "角色编码长度必须在2-50个字符之间")]
  public string RoleCode { get; set; } = string.Empty;

  /// <summary>
  /// 角色描述
  /// </summary>
  [StringLength(500, ErrorMessage = "角色描述长度不能超过500个字符")]
  public string? RoleDescription { get; set; }

  /// <summary>
  /// 排序号
  /// </summary>
  public int OrderNum { get; set; }

  /// <summary>
  /// 状态
  /// </summary>
  public LeanRoleStatus RoleStatus { get; set; } = LeanRoleStatus.Normal;

  /// <summary>
  /// 数据权限范围
  /// </summary>
  public LeanDataScopeType DataScope { get; set; } = LeanDataScopeType.Self;

  /// <summary>
  /// 是否内置
  /// </summary>
  public LeanBuiltinStatus IsBuiltin { get; set; } = LeanBuiltinStatus.No;

  /// <summary>
  /// 部门ID列表
  /// </summary>
  public List<long> DeptIds { get; set; } = new();

  /// <summary>
  /// 菜单ID列表
  /// </summary>
  public List<long> MenuIds { get; set; } = new();
}

/// <summary>
/// 角色更新参数
/// </summary>
public class LeanUpdateRoleDto
{
  /// <summary>
  /// 角色ID
  /// </summary>
  [Required(ErrorMessage = "角色ID不能为空")]
  public long Id { get; set; }

  /// <summary>
  /// 父级ID
  /// </summary>
  public long? ParentId { get; set; }

  /// <summary>
  /// 角色名称
  /// </summary>
  [Required(ErrorMessage = "角色名称不能为空")]
  [StringLength(50, MinimumLength = 2, ErrorMessage = "角色名称长度必须在2-50个字符之间")]
  public string RoleName { get; set; } = string.Empty;

  /// <summary>
  /// 角色编码
  /// </summary>
  [Required(ErrorMessage = "角色编码不能为空")]
  [StringLength(50, MinimumLength = 2, ErrorMessage = "角色编码长度必须在2-50个字符之间")]
  public string RoleCode { get; set; } = string.Empty;

  /// <summary>
  /// 角色描述
  /// </summary>
  [StringLength(500, ErrorMessage = "角色描述长度不能超过500个字符")]
  public string? RoleDescription { get; set; }

  /// <summary>
  /// 排序号
  /// </summary>
  public int OrderNum { get; set; }

  /// <summary>
  /// 状态
  /// </summary>
  public LeanRoleStatus RoleStatus { get; set; }

  /// <summary>
  /// 数据权限范围
  /// </summary>
  public LeanDataScopeType DataScope { get; set; }

  /// <summary>
  /// 部门ID列表
  /// </summary>
  public List<long> DeptIds { get; set; } = new();

  /// <summary>
  /// 菜单ID列表
  /// </summary>
  public List<long> MenuIds { get; set; } = new();
}

/// <summary>
/// 角色状态变更参数
/// </summary>
public class LeanChangeRoleStatusDto
{
  /// <summary>
  /// 角色ID
  /// </summary>
  [Required(ErrorMessage = "角色ID不能为空")]
  public long Id { get; set; }

  /// <summary>
  /// 状态
  /// </summary>
  [Required(ErrorMessage = "状态不能为空")]
  public LeanRoleStatus RoleStatus { get; set; }
}

/// <summary>
/// 角色导出参数
/// </summary>
public class LeanExportRoleDto : LeanQueryRoleDto
{
  /// <summary>
  /// 导出字段列表
  /// </summary>
  public List<string> ExportFields { get; set; } = new();

  /// <summary>
  /// 文件格式
  /// </summary>
  [Required(ErrorMessage = "文件格式不能为空")]
  public string FileFormat { get; set; } = "xlsx";

  /// <summary>
  /// 是否导出全部
  /// </summary>
  public bool IsExportAll { get; set; }

  /// <summary>
  /// 选中的ID列表
  /// </summary>
  public List<long> SelectedIds { get; set; } = new();
}

/// <summary>
/// 角色删除参数
/// </summary>
public class LeanDeleteRoleDto
{
  /// <summary>
  /// 角色ID
  /// </summary>
  [Required(ErrorMessage = "角色ID不能为空")]
  public long Id { get; set; }
}

/// <summary>
/// 角色导入模板
/// </summary>
public class LeanImportTemplateRoleDto
{
  /// <summary>
  /// 角色名称
  /// </summary>
  [Required(ErrorMessage = "角色名称不能为空")]
  [StringLength(50, MinimumLength = 2, ErrorMessage = "角色名称长度必须在2-50个字符之间")]
  public string RoleName { get; set; } = string.Empty;

  /// <summary>
  /// 角色编码
  /// </summary>
  [Required(ErrorMessage = "角色编码不能为空")]
  [StringLength(50, MinimumLength = 2, ErrorMessage = "角色编码长度必须在2-50个字符之间")]
  public string RoleCode { get; set; } = string.Empty;

  /// <summary>
  /// 角色描述
  /// </summary>
  [StringLength(500, ErrorMessage = "角色描述长度不能超过500个字符")]
  public string? RoleDescription { get; set; }

  /// <summary>
  /// 排序号
  /// </summary>
  public int OrderNum { get; set; }

  /// <summary>
  /// 数据权限范围
  /// </summary>
  public LeanDataScopeType DataScope { get; set; } = LeanDataScopeType.Self;
}

/// <summary>
/// 角色导入错误信息
/// </summary>
public class LeanImportRoleErrorDto : LeanImportError
{
  /// <summary>
  /// 角色编码
  /// </summary>
  public string RoleCode
  {
    get => Key;
    set => Key = value;
  }
}

/// <summary>
/// 角色导入结果
/// </summary>
public class LeanImportRoleResultDto : LeanImportResult
{
  /// <summary>
  /// 错误信息列表
  /// </summary>
  public new List<LeanImportRoleErrorDto> Errors { get; set; } = new();

  /// <summary>
  /// 添加错误信息
  /// </summary>
  public override void AddError(string roleCode, string errorMessage)
  {
    base.AddError(roleCode, errorMessage);
    Errors.Add(new LeanImportRoleErrorDto
    {
      RowIndex = TotalCount,
      RoleCode = roleCode,
      ErrorMessage = errorMessage
    });
  }
}