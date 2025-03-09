using System.ComponentModel.DataAnnotations;
using Lean.CodeGen.Common.Models;

namespace Lean.CodeGen.Application.Dtos.Identity;

/// <summary>
/// 角色部门关联基础信息
/// </summary>
public class LeanRoleDeptDto : LeanBaseDto
{
  /// <summary>
  /// 角色ID
  /// </summary>
  public long RoleId { get; set; }

  /// <summary>
  /// 部门ID
  /// </summary>
  public long DeptId { get; set; }

  /// <summary>
  /// 关联的角色信息
  /// </summary>
  public LeanRoleDto? Role { get; set; }

  /// <summary>
  /// 关联的部门信息
  /// </summary>
  public LeanDeptDto? Dept { get; set; }
}

/// <summary>
/// 角色部门关联查询参数
/// </summary>
public class LeanRoleDeptQueryDto : LeanPage
{
  /// <summary>
  /// 角色ID
  /// </summary>
  public long? RoleId { get; set; }

  /// <summary>
  /// 部门ID
  /// </summary>
  public long? DeptId { get; set; }

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
/// 角色部门关联创建参数
/// </summary>
public class LeanRoleDeptCreateDto
{
  /// <summary>
  /// 角色ID
  /// </summary>
  [Required(ErrorMessage = "角色ID不能为空")]
  public long RoleId { get; set; }

  /// <summary>
  /// 部门ID
  /// </summary>
  [Required(ErrorMessage = "部门ID不能为空")]
  public long DeptId { get; set; }
}

/// <summary>
/// 角色部门关联更新参数
/// </summary>
public class LeanRoleDeptUpdateDto
{
  /// <summary>
  /// 角色部门关联ID
  /// </summary>
  [Required(ErrorMessage = "角色部门关联ID不能为空")]
  public long Id { get; set; }

  /// <summary>
  /// 角色ID
  /// </summary>
  [Required(ErrorMessage = "角色ID不能为空")]
  public long RoleId { get; set; }

  /// <summary>
  /// 部门ID
  /// </summary>
  [Required(ErrorMessage = "部门ID不能为空")]
  public long DeptId { get; set; }
}

/// <summary>
/// 角色部门关联删除参数
/// </summary>
public class LeanRoleDeptDeleteDto
{
  /// <summary>
  /// 角色部门关联ID
  /// </summary>
  [Required(ErrorMessage = "角色部门关联ID不能为空")]
  public long Id { get; set; }
}

/// <summary>
/// 角色部门关联批量创建参数
/// </summary>
public class LeanRoleDeptBatchCreateDto
{
  /// <summary>
  /// 角色ID
  /// </summary>
  [Required(ErrorMessage = "角色ID不能为空")]
  public long RoleId { get; set; }

  /// <summary>
  /// 部门ID列表
  /// </summary>
  [Required(ErrorMessage = "部门ID列表不能为空")]
  public List<long> DeptIds { get; set; } = new();
}