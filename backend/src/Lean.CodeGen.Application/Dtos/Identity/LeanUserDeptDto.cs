using System.ComponentModel.DataAnnotations;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Common.Enums;

namespace Lean.CodeGen.Application.Dtos.Identity;

/// <summary>
/// 用户部门关联基础信息
/// </summary>
public class LeanUserDeptDto : LeanBaseDto
{
  /// <summary>
  /// 用户ID
  /// </summary>
  public long UserId { get; set; }

  /// <summary>
  /// 部门ID
  /// </summary>
  public long DeptId { get; set; }

  /// <summary>
  /// 关联的用户信息
  /// </summary>
  public LeanUserDto? User { get; set; }

  /// <summary>
  /// 关联的部门信息
  /// </summary>
  public LeanDeptDto? Dept { get; set; }
}

/// <summary>
/// 用户部门关联查询参数
/// </summary>
public class LeanQueryUserDeptDto : LeanPage
{
  /// <summary>
  /// 用户ID
  /// </summary>
  public long? UserId { get; set; }

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
/// 用户部门关联创建参数
/// </summary>
public class LeanCreateUserDeptDto
{
  /// <summary>
  /// 用户ID
  /// </summary>
  [Required(ErrorMessage = "用户ID不能为空")]
  public long UserId { get; set; }

  /// <summary>
  /// 部门ID
  /// </summary>
  [Required(ErrorMessage = "部门ID不能为空")]
  public long DeptId { get; set; }
}

/// <summary>
/// 用户部门关联更新参数
/// </summary>
public class LeanUpdateUserDeptDto
{
  /// <summary>
  /// 用户部门关联ID
  /// </summary>
  [Required(ErrorMessage = "用户部门关联ID不能为空")]
  public long Id { get; set; }

  /// <summary>
  /// 用户ID
  /// </summary>
  [Required(ErrorMessage = "用户ID不能为空")]
  public long UserId { get; set; }

  /// <summary>
  /// 部门ID
  /// </summary>
  [Required(ErrorMessage = "部门ID不能为空")]
  public long DeptId { get; set; }
}

/// <summary>
/// 用户部门关联删除参数
/// </summary>
public class LeanDeleteUserDeptDto
{
  /// <summary>
  /// 用户部门关联ID
  /// </summary>
  [Required(ErrorMessage = "用户部门关联ID不能为空")]
  public long Id { get; set; }
}

/// <summary>
/// 用户部门关联批量创建参数
/// </summary>
public class LeanBatchCreateUserDeptDto
{
  /// <summary>
  /// 用户ID
  /// </summary>
  [Required(ErrorMessage = "用户ID不能为空")]
  public long UserId { get; set; }

  /// <summary>
  /// 部门ID列表
  /// </summary>
  [Required(ErrorMessage = "部门ID列表不能为空")]
  public List<long> DeptIds { get; set; } = new();
}

/// <summary>
/// 用户部门信息DTO
/// </summary>
public class LeanUserDeptInfoDto
{
  /// <summary>
  /// 部门ID
  /// </summary>
  public long DeptId { get; set; }

  /// <summary>
  /// 部门名称
  /// </summary>
  public string DeptName { get; set; } = string.Empty;

  /// <summary>
  /// 部门编码
  /// </summary>
  public string DeptCode { get; set; } = string.Empty;

  /// <summary>
  /// 是否主部门
  /// </summary>
  public LeanPrimaryStatus IsPrimary { get; set; }
}