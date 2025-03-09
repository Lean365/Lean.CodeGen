using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Lean.CodeGen.Common.Excel;
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
  /// 显示顺序
  /// </summary>
  public int OrderNum { get; set; }

  /// <summary>
  /// 角色状态
  /// 0-正常
  /// 1-停用
  /// </summary>
  public int RoleStatus { get; set; }

  /// <summary>
  /// 是否内置
  /// 0-否
  /// 1-是
  /// </summary>
  public int IsBuiltin { get; set; }

  /// <summary>
  /// 数据权限类型
  /// 0-全部数据权限
  /// 1-自定义数据权限
  /// 2-本部门数据权限
  /// 3-本部门及以下数据权限
  /// 4-仅本人数据权限
  /// </summary>
  public int DataScope { get; set; }

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
public class LeanRoleQueryDto : LeanPage
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
  /// 0-正常
  /// 1-停用
  /// </summary>
  public int? RoleStatus { get; set; }

  /// <summary>
  /// 数据权限范围
  /// 0-全部数据权限
  /// 1-自定义数据权限
  /// 2-本部门数据权限
  /// 3-本部门及以下数据权限
  /// 4-仅本人数据权限
  /// </summary>
  public int? DataScope { get; set; }

  /// <summary>
  /// 是否内置
  /// 0-否
  /// 1-是
  /// </summary>
  public int? IsBuiltin { get; set; }

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
public class LeanRoleCreateDto
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
  /// 0-正常
  /// 1-停用
  /// </summary>
  public int RoleStatus { get; set; } = 0;

  /// <summary>
  /// 数据权限范围
  /// 0-全部数据权限
  /// 1-自定义数据权限
  /// 2-本部门数据权限
  /// 3-本部门及以下数据权限
  /// 4-仅本人数据权限
  /// </summary>
  public int DataScope { get; set; } = 4;

  /// <summary>
  /// 是否内置
  /// 0-否
  /// 1-是
  /// </summary>
  public int IsBuiltin { get; set; } = 0;

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
public class LeanRoleUpdateDto
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
  /// 0-正常
  /// 1-停用
  /// </summary>
  [Required(ErrorMessage = "状态不能为空")]
  public int RoleStatus { get; set; }

  /// <summary>
  /// 数据权限范围
  /// 0-全部数据权限
  /// 1-自定义数据权限
  /// 2-本部门数据权限
  /// 3-本部门及以下数据权限
  /// 4-仅本人数据权限
  /// </summary>
  public int DataScope { get; set; }

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
public class LeanRoleChangeStatusDto
{
  /// <summary>
  /// 角色ID
  /// </summary>
  [Required(ErrorMessage = "角色ID不能为空")]
  public long Id { get; set; }

  /// <summary>
  /// 状态
  /// 0-正常
  /// 1-停用
  /// </summary>
  [Required(ErrorMessage = "状态不能为空")]
  public int RoleStatus { get; set; }
}

/// <summary>
/// 角色导出查询参数
/// </summary>
public class LeanRoleExportQueryDto : LeanRoleQueryDto
{
  /// <summary>
  /// 导出字段列表
  /// </summary>
  public List<string> ExportFields { get; set; } = new();

  /// <summary>
  /// 文件格式
  /// </summary>
  [Required(ErrorMessage = "文件格式不能为空")]
  public string FileType { get; set; } = default!;

  /// <summary>
  /// 是否导出全部
  /// 0-否
  /// 1-是
  /// </summary>
  public int IsExportAll { get; set; }

  /// <summary>
  /// 选中的ID列表
  /// </summary>
  public List<long> SelectedIds { get; set; } = new();
}

/// <summary>
/// 角色删除参数
/// </summary>
public class LeanRoleDeleteDto
{
  /// <summary>
  /// 角色ID
  /// </summary>
  [Required(ErrorMessage = "角色ID不能为空")]
  public long Id { get; set; }
}

/// <summary>
/// 角色导入参数
/// </summary>
public class LeanRoleImportDto
{
  /// <summary>
  /// 角色名称
  /// </summary>
  [Required(ErrorMessage = "角色名称不能为空")]
  [LeanExcelColumn("角色名称")]
  public string RoleName { get; set; } = default!;

  /// <summary>
  /// 角色编码
  /// </summary>
  [Required(ErrorMessage = "角色编码不能为空")]
  [LeanExcelColumn("角色编码")]
  public string RoleCode { get; set; } = default!;

  /// <summary>
  /// 显示顺序
  /// </summary>
  [LeanExcelColumn("显示顺序")]
  public int OrderNum { get; set; }

  /// <summary>
  /// 数据范围
  /// 0-全部数据权限
  /// 1-自定义数据权限
  /// 2-本部门数据权限
  /// 3-本部门及以下数据权限
  /// 4-仅本人数据权限
  /// </summary>
  [LeanExcelColumn("数据范围")]
  public int DataScope { get; set; }

  /// <summary>
  /// 备注
  /// </summary>
  [LeanExcelColumn("备注")]
  public string? Remark { get; set; }
}

/// <summary>
/// 角色导入模板参数
/// </summary>
public class LeanRoleImportTemplateDto
{
  /// <summary>
  /// 角色名称
  /// </summary>
  [Required(ErrorMessage = "角色名称不能为空")]
  [StringLength(50, MinimumLength = 2, ErrorMessage = "角色名称长度必须在2-50个字符之间")]
  public string RoleName { get; set; } = default!;

  /// <summary>
  /// 角色编码
  /// </summary>
  [Required(ErrorMessage = "角色编码不能为空")]
  [StringLength(50, MinimumLength = 2, ErrorMessage = "角色编码长度必须在2-50个字符之间")]
  public string RoleCode { get; set; } = default!;

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
  /// 0-全部数据权限
  /// 1-自定义数据权限
  /// 2-本部门数据权限
  /// 3-本部门及以下数据权限
  /// 4-仅本人数据权限
  /// </summary>
  public int DataScope { get; set; } = 4;
}

/// <summary>
/// 角色导入错误参数
/// </summary>
public class LeanRoleImportErrorDto : LeanImportError
{
  /// <summary>
  /// 角色编码
  /// </summary>
  public string RoleCode { get; set; } = default!;
}

/// <summary>
/// 角色导入结果参数
/// </summary>
public class LeanRoleImportResultDto : LeanImportResult
{
  /// <summary>
  /// 错误列表
  /// </summary>
  public new List<LeanRoleImportErrorDto> Errors { get; set; } = new();

  /// <summary>
  /// 添加错误
  /// </summary>
  /// <param name="roleCode">角色编码</param>
  /// <param name="errorMessage">错误消息</param>
  public override void AddError(string roleCode, string errorMessage)
  {
    Errors.Add(new LeanRoleImportErrorDto
    {
      RoleCode = roleCode,
      ErrorMessage = errorMessage
    });
  }
}

/// <summary>
/// 角色导出参数
/// </summary>
public class LeanRoleExportDto
{
  /// <summary>
  /// 角色名称
  /// </summary>
  [LeanExcelColumn("角色名称", DataType = LeanExcelDataType.String)]
  public string RoleName { get; set; } = default!;

  /// <summary>
  /// 角色编码
  /// </summary>
  [LeanExcelColumn("角色编码", DataType = LeanExcelDataType.String)]
  public string RoleCode { get; set; } = default!;

  /// <summary>
  /// 角色状态
  /// 0-正常
  /// 1-停用
  /// </summary>
  [LeanExcelColumn("角色状态", DataType = LeanExcelDataType.Int)]
  public int RoleStatus { get; set; }

  /// <summary>
  /// 显示顺序
  /// </summary>
  [LeanExcelColumn("显示顺序", DataType = LeanExcelDataType.Int)]
  public int OrderNum { get; set; }

  /// <summary>
  /// 是否内置
  /// 0-否
  /// 1-是
  /// </summary>
  [LeanExcelColumn("是否内置", DataType = LeanExcelDataType.Int)]
  public int IsBuiltin { get; set; }

  /// <summary>
  /// 数据权限
  /// 0-全部数据权限
  /// 1-自定义数据权限
  /// 2-本部门数据权限
  /// 3-本部门及以下数据权限
  /// 4-仅本人数据权限
  /// </summary>
  [LeanExcelColumn("数据权限", DataType = LeanExcelDataType.Int)]
  public int DataScope { get; set; }

  /// <summary>
  /// 角色描述
  /// </summary>
  [LeanExcelColumn("角色描述", DataType = LeanExcelDataType.String)]
  public string? RoleDescription { get; set; }

  /// <summary>
  /// 创建时间
  /// </summary>
  [LeanExcelColumn("创建时间", DataType = LeanExcelDataType.DateTime)]
  public DateTime CreateTime { get; set; }
}

/// <summary>
/// 角色菜单分配参数
/// </summary>
public class LeanRoleSetMenusDto
{
  /// <summary>
  /// 角色ID
  /// </summary>
  public long RoleId { get; set; }

  /// <summary>
  /// 菜单ID列表
  /// </summary>
  public List<long> MenuIds { get; set; } = new();
}