using Lean.CodeGen.Common.Enums;

namespace Lean.CodeGen.Application.Dtos.Identity;

/// <summary>
/// 用户岗位关联DTO
/// </summary>
public class LeanUserPostDto
{
  /// <summary>
  /// 岗位ID
  /// </summary>
  public long PostId { get; set; }

  /// <summary>
  /// 岗位名称
  /// </summary>
  public string PostName { get; set; } = string.Empty;

  /// <summary>
  /// 岗位编码
  /// </summary>
  public string PostCode { get; set; } = string.Empty;

  /// <summary>
  /// 是否主岗位
  /// </summary>
  public LeanPrimaryStatus IsPrimary { get; set; }
}