using System.ComponentModel.DataAnnotations;
using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Domain.Entities;

namespace Lean.CodeGen.Application.Dtos.Identity;

/// <summary>
/// 角色查询参数
/// </summary>
public class LeanQueryRoleDto : LeanPage
{
  /// <summary>
  /// 角色名称
  /// </summary>
  public string? RoleName { get; set; }

  /// <summary>
  /// 角色编码
  /// </summary>
  public string? RoleCode { get; set; }

  /// <summary>
  /// 角色状态
  /// </summary>
  public LeanRoleStatus? RoleStatus { get; set; }

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
  /// 数据权限类型
  /// </summary>
  public LeanDataScopeType DataScope { get; set; }
}

/// <summary>
/// 角色更新参数
/// </summary>
public class LeanUpdateRoleDto : LeanCreateRoleDto
{
  /// <summary>
  /// 角色ID
  /// </summary>
  [Required(ErrorMessage = "角色ID不能为空")]
  public long Id { get; set; }
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
  /// 角色状态
  /// </summary>
  [Required(ErrorMessage = "角色状态不能为空")]
  public LeanRoleStatus RoleStatus { get; set; }
}

/// <summary>
/// 分配角色菜单权限参数
/// </summary>
public class LeanAssignRoleMenuDto
{
  /// <summary>
  /// 角色ID
  /// </summary>
  [Required(ErrorMessage = "角色ID不能为空")]
  public long RoleId { get; set; }

  /// <summary>
  /// 菜单ID列表
  /// </summary>
  public List<long> MenuIds { get; set; } = new();
}

/// <summary>
/// 分配角色API权限参数
/// </summary>
public class LeanAssignRoleApiDto
{
  /// <summary>
  /// 角色ID
  /// </summary>
  [Required(ErrorMessage = "角色ID不能为空")]
  public long RoleId { get; set; }

  /// <summary>
  /// API ID列表
  /// </summary>
  public List<long> ApiIds { get; set; } = new();
}

/// <summary>
/// 分配角色数据权限参数
/// </summary>
public class LeanAssignRoleDataScopeDto
{
  /// <summary>
  /// 角色ID
  /// </summary>
  [Required(ErrorMessage = "角色ID不能为空")]
  public long RoleId { get; set; }

  /// <summary>
  /// 数据权限类型
  /// </summary>
  [Required(ErrorMessage = "数据权限类型不能为空")]
  public LeanDataScopeType DataScope { get; set; }

  /// <summary>
  /// 部门ID列表
  /// </summary>
  public List<long> DeptIds { get; set; } = new();
}

/// <summary>
/// 角色 DTO
/// </summary>
public class LeanRoleDto : LeanBaseEntity
{
  /// <summary>
  /// 父级ID
  /// </summary>
  public long? ParentId { get; set; }

  /// <summary>
  /// 角色名称
  /// </summary>
  public string RoleName { get; set; } = default!;

  /// <summary>
  /// 角色编码
  /// </summary>
  public string RoleCode { get; set; } = default!;

  /// <summary>
  /// 角色描述
  /// </summary>
  public string? RoleDescription { get; set; }

  /// <summary>
  /// 排序号
  /// </summary>
  public int OrderNum { get; set; }

  /// <summary>
  /// 数据权限类型
  /// </summary>
  public LeanDataScopeType DataScope { get; set; }

  /// <summary>
  /// 状态
  /// </summary>
  public LeanRoleStatus RoleStatus { get; set; }

  /// <summary>
  /// 是否内置
  /// </summary>
  public LeanBuiltinStatus IsBuiltin { get; set; }
}

/// <summary>
/// 设置角色前置条件参数
/// </summary>
public class LeanSetRolePrerequisiteDto
{
  /// <summary>
  /// 角色ID
  /// </summary>
  [Required(ErrorMessage = "角色ID不能为空")]
  public long RoleId { get; set; }

  /// <summary>
  /// 前置角色ID列表
  /// </summary>
  public List<long> PrerequisiteRoleIds { get; set; } = new();
}

/// <summary>
/// 设置角色互斥关系参数
/// </summary>
public class LeanSetRoleMutexDto
{
  /// <summary>
  /// 角色ID
  /// </summary>
  [Required(ErrorMessage = "角色ID不能为空")]
  public long RoleId { get; set; }

  /// <summary>
  /// 互斥角色ID列表
  /// </summary>
  public List<long> MutexRoleIds { get; set; } = new();
}