using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Common.Models;

namespace Lean.CodeGen.Application.Dtos.Identity;

/// <summary>
/// 菜单信息
/// </summary>
public class LeanMenuDto
{
  /// <summary>
  /// 菜单ID
  /// </summary>
  public long Id { get; set; }

  /// <summary>
  /// 父级菜单ID
  /// </summary>
  public long? ParentId { get; set; }

  /// <summary>
  /// 菜单名称
  /// </summary>
  public string MenuName { get; set; }

  /// <summary>
  /// 菜单类型
  /// </summary>
  public LeanMenuType MenuType { get; set; }

  /// <summary>
  /// 菜单状态
  /// </summary>
  public LeanMenuStatus MenuStatus { get; set; }

  /// <summary>
  /// 权限标识
  /// </summary>
  public string Perms { get; set; }

  /// <summary>
  /// 路由地址
  /// </summary>
  public string Path { get; set; }

  /// <summary>
  /// 组件路径
  /// </summary>
  public string Component { get; set; }

  /// <summary>
  /// 菜单图标
  /// </summary>
  public string Icon { get; set; }

  /// <summary>
  /// 排序号
  /// </summary>
  public int OrderNum { get; set; }

  /// <summary>
  /// 是否内置
  /// </summary>
  public LeanBuiltinStatus IsBuiltin { get; set; }

  /// <summary>
  /// 是否选中（用于树形结构）
  /// </summary>
  public bool IsChecked { get; set; }

  /// <summary>
  /// 创建时间
  /// </summary>
  public DateTime CreateTime { get; set; }

  /// <summary>
  /// 更新时间
  /// </summary>
  public DateTime? UpdateTime { get; set; }
}

/// <summary>
/// 菜单树形结构
/// </summary>
public class LeanMenuTreeDto : LeanMenuDto
{
  /// <summary>
  /// 子菜单列表
  /// </summary>
  public List<LeanMenuTreeDto> Children { get; set; } = new();
}

/// <summary>
/// 菜单查询参数
/// </summary>
public class LeanQueryMenuDto : LeanPage
{
  /// <summary>
  /// 父级菜单ID
  /// </summary>
  public long? ParentId { get; set; }

  /// <summary>
  /// 菜单名称
  /// </summary>
  public string? MenuName { get; set; }

  /// <summary>
  /// 权限标识
  /// </summary>
  public string? Perms { get; set; }

  /// <summary>
  /// 菜单类型
  /// </summary>
  public LeanMenuType? MenuType { get; set; }

  /// <summary>
  /// 菜单状态
  /// </summary>
  public LeanMenuStatus? MenuStatus { get; set; }

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
/// 创建菜单参数
/// </summary>
public class LeanCreateMenuDto
{
  /// <summary>
  /// 父级菜单ID
  /// </summary>
  public long? ParentId { get; set; }

  /// <summary>
  /// 菜单名称
  /// </summary>
  [Required(ErrorMessage = "菜单名称不能为空")]
  [StringLength(50, MinimumLength = 2, ErrorMessage = "菜单名称长度必须在2-50个字符之间")]
  public string MenuName { get; set; }

  /// <summary>
  /// 菜单类型
  /// </summary>
  [Required(ErrorMessage = "菜单类型不能为空")]
  public LeanMenuType MenuType { get; set; }

  /// <summary>
  /// 权限标识
  /// </summary>
  [Required(ErrorMessage = "权限标识不能为空")]
  [StringLength(100, MinimumLength = 2, ErrorMessage = "权限标识长度必须在2-100个字符之间")]
  public string Perms { get; set; }

  /// <summary>
  /// 路由地址
  /// </summary>
  [StringLength(200, ErrorMessage = "路由路径长度不能超过200个字符")]
  public string Path { get; set; }

  /// <summary>
  /// 组件路径
  /// </summary>
  [StringLength(200, ErrorMessage = "组件路径长度不能超过200个字符")]
  public string Component { get; set; }

  /// <summary>
  /// 菜单图标
  /// </summary>
  [StringLength(100, ErrorMessage = "图标长度不能超过100个字符")]
  public string Icon { get; set; }

  /// <summary>
  /// 排序号
  /// </summary>
  public int OrderNum { get; set; }

  /// <summary>
  /// 菜单状态
  /// </summary>
  public LeanMenuStatus MenuStatus { get; set; } = LeanMenuStatus.Normal;
}

/// <summary>
/// 更新菜单参数
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
/// 删除菜单参数
/// </summary>
public class LeanDeleteMenuDto
{
  /// <summary>
  /// 菜单ID
  /// </summary>
  [Required(ErrorMessage = "菜单ID不能为空")]
  public long Id { get; set; }
}

/// <summary>
/// 修改菜单状态参数
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
/// 菜单导入模板
/// </summary>
public class LeanImportTemplateMenuDto
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
  public string MenuName { get; set; } = string.Empty;

  /// <summary>
  /// 权限标识
  /// </summary>
  [Required(ErrorMessage = "权限标识不能为空")]
  [StringLength(100, MinimumLength = 2, ErrorMessage = "权限标识长度必须在2-100个字符之间")]
  public string Perms { get; set; } = string.Empty;

  /// <summary>
  /// 菜单类型
  /// </summary>
  [Required(ErrorMessage = "菜单类型不能为空")]
  public LeanMenuType MenuType { get; set; }

  /// <summary>
  /// 排序号
  /// </summary>
  public int OrderNum { get; set; }

  /// <summary>
  /// 图标
  /// </summary>
  [StringLength(100, ErrorMessage = "图标长度不能超过100个字符")]
  public string? Icon { get; set; }

  /// <summary>
  /// 路由路径
  /// </summary>
  [StringLength(200, ErrorMessage = "路由路径长度不能超过200个字符")]
  public string? Path { get; set; }

  /// <summary>
  /// 组件路径
  /// </summary>
  [StringLength(200, ErrorMessage = "组件路径长度不能超过200个字符")]
  public string? Component { get; set; }
}

/// <summary>
/// 菜单导入错误信息
/// </summary>
public class LeanImportMenuErrorDto : LeanImportError
{
  /// <summary>
  /// 权限标识
  /// </summary>
  public string Perms
  {
    get => Key;
    set => Key = value;
  }
}

/// <summary>
/// 菜单导入结果
/// </summary>
public class LeanImportMenuResultDto : LeanImportResult
{
  /// <summary>
  /// 错误信息列表
  /// </summary>
  public new List<LeanImportMenuErrorDto> Errors { get; set; } = new();

  /// <summary>
  /// 添加错误信息
  /// </summary>
  public override void AddError(string perms, string errorMessage)
  {
    base.AddError(perms, errorMessage);
    Errors.Add(new LeanImportMenuErrorDto
    {
      RowIndex = TotalCount,
      Perms = perms,
      ErrorMessage = errorMessage
    });
  }
}

/// <summary>
/// 菜单导出参数
/// </summary>
public class LeanExportMenuDto : LeanQueryMenuDto
{
  /// <summary>
  /// 导出字段列表
  /// </summary>
  public List<string> ExportFields { get; set; } = new();

  /// <summary>
  /// 文件格式
  /// </summary>
  [Required(ErrorMessage = "文件格式不能为空")]
  public string FileFormat { get; set; } = "xlsx";

  /// <summary>
  /// 是否导出全部
  /// </summary>
  public bool IsExportAll { get; set; }

  /// <summary>
  /// 选中的ID列表
  /// </summary>
  public List<long> SelectedIds { get; set; } = new();
}