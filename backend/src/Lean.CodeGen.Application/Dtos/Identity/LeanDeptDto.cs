using System.ComponentModel.DataAnnotations;
using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Domain.Entities;

namespace Lean.CodeGen.Application.Dtos.Identity;

/// <summary>
/// 部门查询参数
/// </summary>
public class LeanQueryDeptDto : LeanPage
{
  /// <summary>
  /// 部门名称
  /// </summary>
  public string? DeptName { get; set; }

  /// <summary>
  /// 部门编码
  /// </summary>
  public string? DeptCode { get; set; }

  /// <summary>
  /// 部门状态
  /// </summary>
  public LeanDeptStatus? DeptStatus { get; set; }

  /// <summary>
  /// 开始时间
  /// </summary>
  public DateTime? StartTime { get; set; }

  /// <summary>
  /// 结束时间
  /// </summary>
  public DateTime? EndTime { get; set; }
}

/// <summary>
/// 部门创建参数
/// </summary>
public class LeanCreateDeptDto
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
  public string DeptName { get; set; } = default!;

  /// <summary>
  /// 部门编码
  /// </summary>
  [Required(ErrorMessage = "部门编码不能为空")]
  [StringLength(50, MinimumLength = 2, ErrorMessage = "部门编码长度必须在2-50个字符之间")]
  public string DeptCode { get; set; } = default!;

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
/// 部门更新参数
/// </summary>
public class LeanUpdateDeptDto : LeanCreateDeptDto
{
  /// <summary>
  /// 部门ID
  /// </summary>
  [Required(ErrorMessage = "部门ID不能为空")]
  public long Id { get; set; }
}

/// <summary>
/// 部门详情
/// </summary>
public class LeanDeptDetailDto
{
  /// <summary>
  /// 部门ID
  /// </summary>
  public long Id { get; set; }

  /// <summary>
  /// 父级ID
  /// </summary>
  public long? ParentId { get; set; }

  /// <summary>
  /// 部门名称
  /// </summary>
  public string DeptName { get; set; } = default!;

  /// <summary>
  /// 部门编码
  /// </summary>
  public string DeptCode { get; set; } = default!;

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
  /// 状态
  /// </summary>
  public LeanDeptStatus DeptStatus { get; set; }

  /// <summary>
  /// 是否内置
  /// </summary>
  public LeanBuiltinStatus IsBuiltin { get; set; }

  /// <summary>
  /// 创建时间
  /// </summary>
  public DateTime CreateTime { get; set; }

  /// <summary>
  /// 创建者
  /// </summary>
  public string? CreateUserName { get; set; }

  /// <summary>
  /// 更新时间
  /// </summary>
  public DateTime? UpdateTime { get; set; }

  /// <summary>
  /// 更新者
  /// </summary>
  public string? UpdateUserName { get; set; }
}

/// <summary>
/// 部门状态变更参数
/// </summary>
public class LeanChangeDeptStatusDto
{
  /// <summary>
  /// 部门ID
  /// </summary>
  [Required(ErrorMessage = "部门ID不能为空")]
  public long Id { get; set; }

  /// <summary>
  /// 部门状态
  /// </summary>
  [Required(ErrorMessage = "部门状态不能为空")]
  public LeanDeptStatus DeptStatus { get; set; }
}

/// <summary>
/// 部门 DTO
/// </summary>
public class LeanDeptDto : LeanBaseEntity
{
  /// <summary>
  /// 父级ID
  /// </summary>
  public long? ParentId { get; set; }

  /// <summary>
  /// 部门名称
  /// </summary>
  public string DeptName { get; set; } = default!;

  /// <summary>
  /// 部门编码
  /// </summary>
  public string DeptCode { get; set; } = default!;

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
  /// 状态
  /// </summary>
  public LeanDeptStatus DeptStatus { get; set; }

  /// <summary>
  /// 是否内置
  /// </summary>
  public LeanBuiltinStatus IsBuiltin { get; set; }
}

/// <summary>
/// 部门树形结构 DTO
/// </summary>
public class LeanDeptTreeDto : LeanBaseEntity
{
  /// <summary>
  /// 父级ID
  /// </summary>
  public long? ParentId { get; set; }

  /// <summary>
  /// 部门名称
  /// </summary>
  public string DeptName { get; set; } = default!;

  /// <summary>
  /// 部门编码
  /// </summary>
  public string DeptCode { get; set; } = default!;

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
  /// 状态
  /// </summary>
  public LeanDeptStatus DeptStatus { get; set; }

  /// <summary>
  /// 是否内置
  /// </summary>
  public LeanBuiltinStatus IsBuiltin { get; set; }

  /// <summary>
  /// 子部门列表
  /// </summary>
  public List<LeanDeptTreeDto> Children { get; set; } = new();
}

/// <summary>
/// 部门导入模板
/// </summary>
public class LeanImportTemplateDeptDto
{
  /// <summary>
  /// 上级部门编码
  /// </summary>
  public string? ParentDeptCode { get; set; }

  /// <summary>
  /// 部门名称
  /// </summary>
  [Required(ErrorMessage = "部门名称不能为空")]
  public string DeptName { get; set; } = default!;

  /// <summary>
  /// 部门编码
  /// </summary>
  [Required(ErrorMessage = "部门编码不能为空")]
  public string DeptCode { get; set; } = default!;
}

/// <summary>
/// 部门导入结果
/// </summary>
public class LeanImportDeptResultDto : LeanImportResult
{
  /// <summary>
  /// 错误信息列表
  /// </summary>
  public List<string> ErrorMessages { get; set; } = new();
}

/// <summary>
/// 部门导出参数
/// </summary>
public class LeanExportDeptDto : LeanQueryDeptDto
{
  /// <summary>
  /// 导出字段列表
  /// </summary>
  public List<string> ExportFields { get; set; } = new();

  /// <summary>
  /// 文件格式
  /// </summary>
  public string FileFormat { get; set; } = "xlsx";
}

/// <summary>
/// 设置部门数据权限参数
/// </summary>
public class LeanSetDeptDataPermissionDto
{
  /// <summary>
  /// 部门ID
  /// </summary>
  [Required(ErrorMessage = "部门ID不能为空")]
  public long DeptId { get; set; }

  /// <summary>
  /// 数据权限类型
  /// </summary>
  public LeanDataScopeType DataScope { get; set; }

  /// <summary>
  /// 可访问的部门ID列表
  /// </summary>
  public List<long> AccessibleDeptIds { get; set; } = new();
}

/// <summary>
/// 部门数据权限 DTO
/// </summary>
public class LeanDeptDataPermissionDto
{
  /// <summary>
  /// 数据权限类型
  /// </summary>
  public LeanDataScopeType DataScope { get; set; }

  /// <summary>
  /// 可访问的部门ID列表
  /// </summary>
  public List<long> AccessibleDeptIds { get; set; } = new();
}