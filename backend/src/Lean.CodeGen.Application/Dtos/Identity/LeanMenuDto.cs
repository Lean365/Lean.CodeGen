using System.ComponentModel.DataAnnotations;
using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Domain.Entities;

namespace Lean.CodeGen.Application.Dtos.Identity;

/// <summary>
/// 菜单查询参数
/// </summary>
public class LeanQueryMenuDto : LeanPage
{
  /// <summary>
  /// 菜单名称
  /// </summary>
  public string? MenuName { get; set; }

  /// <summary>
  /// 菜单编码
  /// </summary>
  public string? MenuCode { get; set; }

  /// <summary>
  /// 菜单状态
  /// </summary>
  public LeanMenuStatus? MenuStatus { get; set; }

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
/// 菜单创建参数
/// </summary>
public class LeanCreateMenuDto
{
  /// <summary>
  /// 父级ID
  /// </summary>
  public long? ParentId { get; set; }

  /// <summary>
  /// 菜单名称
  /// </summary>
  [Required(ErrorMessage = "菜单名称不能为空")]
  [StringLength(50, MinimumLength = 2, ErrorMessage = "菜单名称长度必须在2-50个字符之间")]
  public string MenuName { get; set; } = default!;

  /// <summary>
  /// 菜单编码
  /// </summary>
  [Required(ErrorMessage = "菜单编码不能为空")]
  [StringLength(50, MinimumLength = 2, ErrorMessage = "菜单编码长度必须在2-50个字符之间")]
  public string MenuCode { get; set; } = default!;

  /// <summary>
  /// 菜单类型
  /// </summary>
  public LeanMenuType MenuType { get; set; }

  /// <summary>
  /// 路由地址
  /// </summary>
  [StringLength(200, ErrorMessage = "路由地址长度不能超过200个字符")]
  public string? Path { get; set; }

  /// <summary>
  /// 组件路径
  /// </summary>
  [StringLength(200, ErrorMessage = "组件路径长度不能超过200个字符")]
  public string? Component { get; set; }

  /// <summary>
  /// 重定向路径
  /// </summary>
  [StringLength(200, ErrorMessage = "重定向路径长度不能超过200个字符")]
  public string? Redirect { get; set; }

  /// <summary>
  /// 菜单图标
  /// </summary>
  [StringLength(100, ErrorMessage = "菜单图标长度不能超过100个字符")]
  public string? Icon { get; set; }

  /// <summary>
  /// 国际化翻译键
  /// </summary>
  [StringLength(100, ErrorMessage = "国际化翻译键长度不能超过100个字符")]
  public string? TransKey { get; set; }

  /// <summary>
  /// 排序号
  /// </summary>
  public int OrderNum { get; set; }

  /// <summary>
  /// 显示状态
  /// </summary>
  public int Visible { get; set; }

  /// <summary>
  /// 是否外链
  /// </summary>
  public int IsFrame { get; set; }

  /// <summary>
  /// 是否缓存
  /// </summary>
  public int IsCached { get; set; }
}

/// <summary>
/// 菜单更新参数
/// </summary>
public class LeanUpdateMenuDto : LeanCreateMenuDto
{
  /// <summary>
  /// 菜单ID
  /// </summary>
  [Required(ErrorMessage = "菜单ID不能为空")]
  public long Id { get; set; }
}

/// <summary>
/// 菜单详情
/// </summary>
public class LeanMenuDetailDto
{
  /// <summary>
  /// 菜单ID
  /// </summary>
  public long Id { get; set; }

  /// <summary>
  /// 父级ID
  /// </summary>
  public long? ParentId { get; set; }

  /// <summary>
  /// 菜单名称
  /// </summary>
  public string MenuName { get; set; } = default!;

  /// <summary>
  /// 菜单编码
  /// </summary>
  public string MenuCode { get; set; } = default!;

  /// <summary>
  /// 菜单类型
  /// </summary>
  public LeanMenuType MenuType { get; set; }

  /// <summary>
  /// 路由地址
  /// </summary>
  public string? Path { get; set; }

  /// <summary>
  /// 组件路径
  /// </summary>
  public string? Component { get; set; }

  /// <summary>
  /// 重定向路径
  /// </summary>
  public string? Redirect { get; set; }

  /// <summary>
  /// 菜单图标
  /// </summary>
  public string? Icon { get; set; }

  /// <summary>
  /// 国际化翻译键
  /// </summary>
  public string? TransKey { get; set; }

  /// <summary>
  /// 排序号
  /// </summary>
  public int OrderNum { get; set; }

  /// <summary>
  /// 显示状态
  /// </summary>
  public int Visible { get; set; }

  /// <summary>
  /// 是否外链
  /// </summary>
  public int IsFrame { get; set; }

  /// <summary>
  /// 是否缓存
  /// </summary>
  public int IsCached { get; set; }

  /// <summary>
  /// 状态
  /// </summary>
  public LeanMenuStatus MenuStatus { get; set; }

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
/// 菜单状态变更参数
/// </summary>
public class LeanChangeMenuStatusDto
{
  /// <summary>
  /// 菜单ID
  /// </summary>
  [Required(ErrorMessage = "菜单ID不能为空")]
  public long Id { get; set; }

  /// <summary>
  /// 菜单状态
  /// </summary>
  [Required(ErrorMessage = "菜单状态不能为空")]
  public LeanMenuStatus MenuStatus { get; set; }
}

/// <summary>
/// 菜单 DTO
/// </summary>
public class LeanMenuDto : LeanBaseEntity
{
  /// <summary>
  /// 父级ID
  /// </summary>
  public long? ParentId { get; set; }

  /// <summary>
  /// 菜单名称
  /// </summary>
  public string MenuName { get; set; } = default!;

  /// <summary>
  /// 菜单编码
  /// </summary>
  public string MenuCode { get; set; } = default!;

  /// <summary>
  /// 菜单类型
  /// </summary>
  public LeanMenuType MenuType { get; set; }

  /// <summary>
  /// 路由路径
  /// </summary>
  public string? Path { get; set; }

  /// <summary>
  /// 组件路径
  /// </summary>
  public string? Component { get; set; }

  /// <summary>
  /// 重定向路径
  /// </summary>
  public string? Redirect { get; set; }

  /// <summary>
  /// 菜单图标
  /// </summary>
  public string? Icon { get; set; }

  /// <summary>
  /// 国际化翻译键
  /// </summary>
  public string? TransKey { get; set; }

  /// <summary>
  /// 排序号
  /// </summary>
  public int OrderNum { get; set; }

  /// <summary>
  /// 显示状态
  /// </summary>
  public int Visible { get; set; }

  /// <summary>
  /// 是否外链
  /// </summary>
  public int IsFrame { get; set; }

  /// <summary>
  /// 是否缓存
  /// </summary>
  public int IsCached { get; set; }

  /// <summary>
  /// 状态
  /// </summary>
  public LeanMenuStatus MenuStatus { get; set; }

  /// <summary>
  /// 是否内置
  /// </summary>
  public LeanBuiltinStatus IsBuiltin { get; set; }
}

/// <summary>
/// 菜单树形结构 DTO
/// </summary>
public class LeanMenuTreeDto : LeanBaseEntity
{
  /// <summary>
  /// 父级ID
  /// </summary>
  public long? ParentId { get; set; }

  /// <summary>
  /// 菜单名称
  /// </summary>
  public string MenuName { get; set; } = default!;

  /// <summary>
  /// 菜单编码
  /// </summary>
  public string MenuCode { get; set; } = default!;

  /// <summary>
  /// 菜单类型
  /// </summary>
  public LeanMenuType MenuType { get; set; }

  /// <summary>
  /// 路由路径
  /// </summary>
  public string? Path { get; set; }

  /// <summary>
  /// 组件路径
  /// </summary>
  public string? Component { get; set; }

  /// <summary>
  /// 重定向路径
  /// </summary>
  public string? Redirect { get; set; }

  /// <summary>
  /// 菜单图标
  /// </summary>
  public string? Icon { get; set; }

  /// <summary>
  /// 国际化翻译键
  /// </summary>
  public string? TransKey { get; set; }

  /// <summary>
  /// 排序号
  /// </summary>
  public int OrderNum { get; set; }

  /// <summary>
  /// 显示状态
  /// </summary>
  public int Visible { get; set; }

  /// <summary>
  /// 是否外链
  /// </summary>
  public int IsFrame { get; set; }

  /// <summary>
  /// 是否缓存
  /// </summary>
  public int IsCached { get; set; }

  /// <summary>
  /// 状态
  /// </summary>
  public LeanMenuStatus MenuStatus { get; set; }

  /// <summary>
  /// 是否内置
  /// </summary>
  public LeanBuiltinStatus IsBuiltin { get; set; }

  /// <summary>
  /// 子菜单列表
  /// </summary>
  public List<LeanMenuTreeDto> Children { get; set; } = new();
}

/// <summary>
/// 菜单操作权限 DTO
/// </summary>
public class LeanMenuOperationDto
{
  /// <summary>
  /// 操作编码
  /// </summary>
  public string Code { get; set; } = default!;

  /// <summary>
  /// 操作名称
  /// </summary>
  public string Name { get; set; } = default!;
}

/// <summary>
/// 设置菜单操作权限参数
/// </summary>
public class LeanSetMenuOperationDto
{
  /// <summary>
  /// 菜单ID
  /// </summary>
  [Required(ErrorMessage = "菜单ID不能为空")]
  public long MenuId { get; set; }

  /// <summary>
  /// 操作权限列表
  /// </summary>
  public List<LeanMenuOperationDto> Operations { get; set; } = new();
}
