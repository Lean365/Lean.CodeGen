using System.ComponentModel.DataAnnotations;
using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Common.Models;

namespace Lean.CodeGen.Application.Dtos.Identity;

/// <summary>
/// 用户岗位关联查询参数
/// </summary>
public class LeanQueryUserPostDto : LeanPage
{
  /// <summary>
  /// 用户ID
  /// </summary>
  public long? UserId { get; set; }

  /// <summary>
  /// 岗位ID
  /// </summary>
  public long? PostId { get; set; }

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
/// 用户岗位关联创建参数
/// </summary>
public class LeanCreateUserPostDto
{
  /// <summary>
  /// 用户ID
  /// </summary>
  [Required(ErrorMessage = "用户ID不能为空")]
  public long UserId { get; set; }

  /// <summary>
  /// 岗位ID
  /// </summary>
  [Required(ErrorMessage = "岗位ID不能为空")]
  public long PostId { get; set; }

  /// <summary>
  /// 是否主岗位
  /// </summary>
  public LeanPrimaryStatus IsPrimary { get; set; }
}

/// <summary>
/// 用户岗位关联更新参数
/// </summary>
public class LeanUpdateUserPostDto : LeanCreateUserPostDto
{
  /// <summary>
  /// 关联ID
  /// </summary>
  [Required(ErrorMessage = "关联ID不能为空")]
  public long Id { get; set; }
}

/// <summary>
/// 用户岗位关联详情
/// </summary>
public class LeanUserPostDetailDto
{
  /// <summary>
  /// 关联ID
  /// </summary>
  public long Id { get; set; }

  /// <summary>
  /// 用户ID
  /// </summary>
  public long UserId { get; set; }

  /// <summary>
  /// 用户名称
  /// </summary>
  public string UserName { get; set; } = default!;

  /// <summary>
  /// 岗位ID
  /// </summary>
  public long PostId { get; set; }

  /// <summary>
  /// 岗位名称
  /// </summary>
  public string PostName { get; set; } = default!;

  /// <summary>
  /// 是否主岗位
  /// </summary>
  public LeanPrimaryStatus IsPrimary { get; set; }

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
/// 用户岗位关联 DTO
/// </summary>
public class LeanUserPostDto
{
  /// <summary>
  /// 关联ID
  /// </summary>
  public long Id { get; set; }

  /// <summary>
  /// 用户ID
  /// </summary>
  public long UserId { get; set; }

  /// <summary>
  /// 用户名称
  /// </summary>
  public string UserName { get; set; } = default!;

  /// <summary>
  /// 岗位ID
  /// </summary>
  public long PostId { get; set; }

  /// <summary>
  /// 岗位名称
  /// </summary>
  public string PostName { get; set; } = default!;

  /// <summary>
  /// 是否主岗位
  /// </summary>
  public LeanPrimaryStatus IsPrimary { get; set; }
}