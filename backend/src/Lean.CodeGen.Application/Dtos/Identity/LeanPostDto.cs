using System.ComponentModel.DataAnnotations;
using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Common.Models;

namespace Lean.CodeGen.Application.Dtos.Identity;

/// <summary>
/// 岗位查询参数
/// </summary>
public class LeanQueryPostDto : LeanPage
{
  /// <summary>
  /// 岗位名称
  /// </summary>
  public string? PostName { get; set; }

  /// <summary>
  /// 岗位编码
  /// </summary>
  public string? PostCode { get; set; }

  /// <summary>
  /// 岗位状态
  /// </summary>
  public LeanPostStatus? PostStatus { get; set; }

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
/// 岗位创建参数
/// </summary>
public class LeanCreatePostDto
{
  /// <summary>
  /// 岗位名称
  /// </summary>
  [Required(ErrorMessage = "岗位名称不能为空")]
  [StringLength(50, MinimumLength = 2, ErrorMessage = "岗位名称长度必须在2-50个字符之间")]
  public string PostName { get; set; } = default!;

  /// <summary>
  /// 岗位编码
  /// </summary>
  [Required(ErrorMessage = "岗位编码不能为空")]
  [StringLength(50, MinimumLength = 2, ErrorMessage = "岗位编码长度必须在2-50个字符之间")]
  public string PostCode { get; set; } = default!;

  /// <summary>
  /// 岗位描述
  /// </summary>
  [StringLength(500, ErrorMessage = "岗位描述长度不能超过500个字符")]
  public string? PostDescription { get; set; }

  /// <summary>
  /// 排序号
  /// </summary>
  public int OrderNum { get; set; }
}

/// <summary>
/// 岗位更新参数
/// </summary>
public class LeanUpdatePostDto : LeanCreatePostDto
{
  /// <summary>
  /// 岗位ID
  /// </summary>
  [Required(ErrorMessage = "岗位ID不能为空")]
  public long Id { get; set; }
}

/// <summary>
/// 岗位详情
/// </summary>
public class LeanPostDetailDto
{
  /// <summary>
  /// 岗位ID
  /// </summary>
  public long Id { get; set; }

  /// <summary>
  /// 岗位名称
  /// </summary>
  public string PostName { get; set; } = default!;

  /// <summary>
  /// 岗位编码
  /// </summary>
  public string PostCode { get; set; } = default!;

  /// <summary>
  /// 岗位描述
  /// </summary>
  public string? PostDescription { get; set; }

  /// <summary>
  /// 排序号
  /// </summary>
  public int OrderNum { get; set; }

  /// <summary>
  /// 状态
  /// </summary>
  public LeanPostStatus PostStatus { get; set; }

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
/// 岗位状态变更参数
/// </summary>
public class LeanChangePostStatusDto
{
  /// <summary>
  /// 岗位ID
  /// </summary>
  [Required(ErrorMessage = "岗位ID不能为空")]
  public long Id { get; set; }

  /// <summary>
  /// 岗位状态
  /// </summary>
  [Required(ErrorMessage = "岗位状态不能为空")]
  public LeanPostStatus PostStatus { get; set; }
}

/// <summary>
/// 岗位 DTO
/// </summary>
public class LeanPostDto
{
  /// <summary>
  /// 岗位ID
  /// </summary>
  public long Id { get; set; }

  /// <summary>
  /// 岗位名称
  /// </summary>
  public string PostName { get; set; } = default!;

  /// <summary>
  /// 岗位编码
  /// </summary>
  public string PostCode { get; set; } = default!;

  /// <summary>
  /// 岗位描述
  /// </summary>
  public string? PostDescription { get; set; }

  /// <summary>
  /// 排序号
  /// </summary>
  public int OrderNum { get; set; }

  /// <summary>
  /// 状态
  /// </summary>
  public LeanPostStatus PostStatus { get; set; }
}

/// <summary>
/// 岗位导入模板
/// </summary>
public class LeanImportTemplatePostDto
{
  /// <summary>
  /// 岗位名称
  /// </summary>
  [Required(ErrorMessage = "岗位名称不能为空")]
  public string PostName { get; set; } = default!;

  /// <summary>
  /// 岗位编码
  /// </summary>
  [Required(ErrorMessage = "岗位编码不能为空")]
  public string PostCode { get; set; } = default!;
}

/// <summary>
/// 岗位导入结果
/// </summary>
public class LeanImportPostResultDto : LeanImportResult
{
  /// <summary>
  /// 错误信息列表
  /// </summary>
  public List<string> ErrorMessages { get; set; } = new();
}

/// <summary>
/// 岗位导出参数
/// </summary>
public class LeanExportPostDto : LeanQueryPostDto
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
/// 岗位树形结构 DTO
/// </summary>
public class LeanPostTreeDto : LeanPostDto
{
  /// <summary>
  /// 子岗位列表
  /// </summary>
  public List<LeanPostTreeDto> Children { get; set; } = new();
}

/// <summary>
/// 岗位权限信息
/// </summary>
public class LeanPostPermissionsDto
{
  /// <summary>
  /// 菜单权限列表
  /// </summary>
  public List<string> MenuPermissions { get; set; } = new();

  /// <summary>
  /// API权限列表
  /// </summary>
  public List<string> ApiPermissions { get; set; } = new();
}

/// <summary>
/// 设置岗位继承关系参数
/// </summary>
public class LeanSetPostInheritanceDto
{
  /// <summary>
  /// 岗位ID
  /// </summary>
  [Required(ErrorMessage = "岗位ID不能为空")]
  public long PostId { get; set; }

  /// <summary>
  /// 继承岗位ID列表
  /// </summary>
  public List<long> InheritedPostIds { get; set; } = new();
}

/// <summary>
/// 设置岗位权限参数
/// </summary>
public class LeanSetPostPermissionDto
{
  /// <summary>
  /// 岗位ID
  /// </summary>
  [Required(ErrorMessage = "岗位ID不能为空")]
  public long PostId { get; set; }

  /// <summary>
  /// 菜单ID列表
  /// </summary>
  public List<long> MenuIds { get; set; } = new();

  /// <summary>
  /// API ID列表
  /// </summary>
  public List<long> ApiIds { get; set; } = new();
}