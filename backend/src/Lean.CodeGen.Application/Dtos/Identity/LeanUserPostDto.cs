using System.ComponentModel.DataAnnotations;

namespace Lean.CodeGen.Application.Dtos.Identity;

/// <summary>
/// 用户岗位DTO
/// </summary>
public class LeanUserPostDto
{
  /// <summary>
  /// 用户ID
  /// </summary>
  [Required(ErrorMessage = "用户ID不能为空")]
  public long UserId { get; set; }

  /// <summary>
  /// 岗位ID列表
  /// </summary>
  [Required(ErrorMessage = "岗位ID列表不能为空")]
  public List<long> PostIds { get; set; } = new();
}