using System.ComponentModel.DataAnnotations;
using Lean.CodeGen.Common.Models;

namespace Lean.CodeGen.Application.Dtos.Identity;

/// <summary>
/// 角色前置关联查询参数
/// </summary>
public class LeanQueryRolePrerequisiteDto : LeanPage
{
  /// <summary>
  /// 角色ID
  /// </summary>
  public long? RoleId { get; set; }

  /// <summary>
  /// 前置角色ID
  /// </summary>
  public long? PrerequisiteRoleId { get; set; }

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
/// 角色前置关联创建参数
/// </summary>
public class LeanCreateRolePrerequisiteDto
{
  /// <summary>
  /// 角色ID
  /// </summary>
  [Required(ErrorMessage = "角色ID不能为空")]
  public long RoleId { get; set; }

  /// <summary>
  /// 前置角色ID
  /// </summary>
  [Required(ErrorMessage = "前置角色ID不能为空")]
  public long PrerequisiteRoleId { get; set; }
}

/// <summary>
/// 角色前置关联更新参数
/// </summary>
public class LeanUpdateRolePrerequisiteDto : LeanCreateRolePrerequisiteDto
{
  /// <summary>
  /// 关联ID
  /// </summary>
  [Required(ErrorMessage = "关联ID不能为空")]
  public long Id { get; set; }
}

/// <summary>
/// 角色前置关联详情
/// </summary>
public class LeanRolePrerequisiteDetailDto
{
  /// <summary>
  /// 关联ID
  /// </summary>
  public long Id { get; set; }

  /// <summary>
  /// 角色ID
  /// </summary>
  public long RoleId { get; set; }

  /// <summary>
  /// 角色名称
  /// </summary>
  public string RoleName { get; set; } = default!;

  /// <summary>
  /// 前置角色ID
  /// </summary>
  public long PrerequisiteRoleId { get; set; }

  /// <summary>
  /// 前置角色名称
  /// </summary>
  public string PrerequisiteRoleName { get; set; } = default!;

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
/// 角色前置关联 DTO
/// </summary>
public class LeanRolePrerequisiteDto
{
  /// <summary>
  /// 关联ID
  /// </summary>
  public long Id { get; set; }

  /// <summary>
  /// 角色ID
  /// </summary>
  public long RoleId { get; set; }

  /// <summary>
  /// 角色名称
  /// </summary>
  public string RoleName { get; set; } = default!;

  /// <summary>
  /// 前置角色ID
  /// </summary>
  public long PrerequisiteRoleId { get; set; }

  /// <summary>
  /// 前置角色名称
  /// </summary>
  public string PrerequisiteRoleName { get; set; } = default!;
}