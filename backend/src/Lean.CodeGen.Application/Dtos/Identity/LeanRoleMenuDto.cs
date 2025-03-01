using System.ComponentModel.DataAnnotations;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Common.Enums;

namespace Lean.CodeGen.Application.Dtos.Identity;

/// <summary>
/// 角色菜单关联基础信息
/// </summary>
public class LeanRoleMenuDto : LeanBaseDto
{
  /// <summary>
  /// 角色ID
  /// </summary>
  public long RoleId { get; set; }

  /// <summary>
  /// 菜单ID
  /// </summary>
  public long MenuId { get; set; }

  /// <summary>
  /// 关联的角色信息
  /// </summary>
  public LeanRoleDto? Role { get; set; }

  /// <summary>
  /// 关联的菜单信息
  /// </summary>
  public LeanMenuDto? Menu { get; set; }
}

/// <summary>
/// 角色菜单关联查询参数
/// </summary>
public class LeanQueryRoleMenuDto : LeanPage
{
  /// <summary>
  /// 角色ID
  /// </summary>
  public long? RoleId { get; set; }

  /// <summary>
  /// 菜单ID
  /// </summary>
  public long? MenuId { get; set; }

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
/// 角色菜单关联创建参数
/// </summary>
public class LeanCreateRoleMenuDto
{
  /// <summary>
  /// 角色ID
  /// </summary>
  [Required(ErrorMessage = "角色ID不能为空")]
  public long RoleId { get; set; }

  /// <summary>
  /// 菜单ID
  /// </summary>
  [Required(ErrorMessage = "菜单ID不能为空")]
  public long MenuId { get; set; }
}

/// <summary>
/// 角色菜单关联更新参数
/// </summary>
public class LeanUpdateRoleMenuDto
{
  /// <summary>
  /// 角色菜单关联ID
  /// </summary>
  [Required(ErrorMessage = "角色菜单关联ID不能为空")]
  public long Id { get; set; }

  /// <summary>
  /// 角色ID
  /// </summary>
  [Required(ErrorMessage = "角色ID不能为空")]
  public long RoleId { get; set; }

  /// <summary>
  /// 菜单ID
  /// </summary>
  [Required(ErrorMessage = "菜单ID不能为空")]
  public long MenuId { get; set; }
}

/// <summary>
/// 角色菜单关联删除参数
/// </summary>
public class LeanDeleteRoleMenuDto
{
  /// <summary>
  /// 角色菜单关联ID
  /// </summary>
  [Required(ErrorMessage = "角色菜单关联ID不能为空")]
  public long Id { get; set; }
}

/// <summary>
/// 角色菜单关联批量创建参数
/// </summary>
public class LeanBatchCreateRoleMenuDto
{
  /// <summary>
  /// 角色ID
  /// </summary>
  [Required(ErrorMessage = "角色ID不能为空")]
  public long RoleId { get; set; }

  /// <summary>
  /// 菜单ID列表
  /// </summary>
  [Required(ErrorMessage = "菜单ID列表不能为空")]
  public List<long> MenuIds { get; set; } = new();
}

/// <summary>
/// 角色菜单分配参数
/// </summary>
public class LeanAssignRoleMenuDto
{
  /// <summary>
  /// 角色ID
  /// </summary>
  public long Id { get; set; }

  /// <summary>
  /// 菜单ID列表
  /// </summary>
  public List<long> MenuIds { get; set; }
}

/// <summary>
/// 角色数据权限分配参数
/// </summary>
public class LeanAssignRoleDataScopeDto
{
  /// <summary>
  /// 角色ID
  /// </summary>
  public long Id { get; set; }

  /// <summary>
  /// 数据权限类型
  /// </summary>
  public LeanDataScopeType DataScope { get; set; }

  /// <summary>
  /// 部门ID列表
  /// </summary>
  public List<long> DeptIds { get; set; }
}

/// <summary>
/// 角色数据权限信息
/// </summary>
public class LeanRoleDataScopeDto
{
  /// <summary>
  /// 角色ID
  /// </summary>
  public long Id { get; set; }

  /// <summary>
  /// 数据权限类型
  /// </summary>
  public LeanDataScopeType DataScope { get; set; }

  /// <summary>
  /// 部门ID列表
  /// </summary>
  public List<long> DeptIds { get; set; }
}