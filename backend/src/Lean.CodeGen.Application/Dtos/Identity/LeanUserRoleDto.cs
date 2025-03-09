using System.ComponentModel.DataAnnotations;
using Lean.CodeGen.Common.Models;

namespace Lean.CodeGen.Application.Dtos.Identity;

/// <summary>
/// 用户角色关联基础信息
/// </summary>
public class LeanUserRoleDto : LeanBaseDto
{
  /// <summary>
  /// 用户ID
  /// </summary>
  public long UserId { get; set; }

  /// <summary>
  /// 角色ID
  /// </summary>
  public long RoleId { get; set; }

  /// <summary>
  /// 关联的用户信息
  /// </summary>
  public LeanUserDto? User { get; set; }

  /// <summary>
  /// 关联的角色信息
  /// </summary>
  public LeanRoleDto? Role { get; set; }
}

/// <summary>
/// 用户角色关联查询参数
/// </summary>
public class LeanUserRoleQueryDto : LeanPage
{
  /// <summary>
  /// 用户ID
  /// </summary>
  public long? UserId { get; set; }

  /// <summary>
  /// 角色ID
  /// </summary>
  public long? RoleId { get; set; }

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
/// 用户角色关联创建参数
/// </summary>
public class LeanUserRoleCreateDto
{
  /// <summary>
  /// 用户ID
  /// </summary>
  [Required(ErrorMessage = "用户ID不能为空")]
  public long UserId { get; set; }

  /// <summary>
  /// 角色ID
  /// </summary>
  [Required(ErrorMessage = "角色ID不能为空")]
  public long RoleId { get; set; }
}

/// <summary>
/// 用户角色关联更新参数
/// </summary>
public class LeanUserRoleUpdateDto
{
  /// <summary>
  /// 用户角色关联ID
  /// </summary>
  [Required(ErrorMessage = "用户角色关联ID不能为空")]
  public long Id { get; set; }

  /// <summary>
  /// 用户ID
  /// </summary>
  [Required(ErrorMessage = "用户ID不能为空")]
  public long UserId { get; set; }

  /// <summary>
  /// 角色ID
  /// </summary>
  [Required(ErrorMessage = "角色ID不能为空")]
  public long RoleId { get; set; }
}

/// <summary>
/// 用户角色关联删除参数
/// </summary>
public class LeanUserRoleDeleteDto
{
  /// <summary>
  /// 用户角色关联ID
  /// </summary>
  [Required(ErrorMessage = "用户角色关联ID不能为空")]
  public long Id { get; set; }
}

/// <summary>
/// 用户角色关联批量创建参数
/// </summary>
public class LeanUserRoleBatchCreateDto
{
  /// <summary>
  /// 用户ID
  /// </summary>
  [Required(ErrorMessage = "用户ID不能为空")]
  public long UserId { get; set; }

  /// <summary>
  /// 角色ID列表
  /// </summary>
  [Required(ErrorMessage = "角色ID列表不能为空")]
  public List<long> RoleIds { get; set; } = new();
}