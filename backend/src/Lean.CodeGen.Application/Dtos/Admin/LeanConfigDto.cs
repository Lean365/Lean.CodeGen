using System.ComponentModel.DataAnnotations;
using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Domain.Entities;
using Lean.CodeGen.Common.Excel;

namespace Lean.CodeGen.Application.Dtos.Admin;

/// <summary>
/// 系统配置查询参数
/// </summary>
public class LeanConfigQueryDto : LeanPage
{
  /// <summary>
  /// 配置名称
  /// </summary>
  /// <remarks>
  /// 配置项的显示名称
  /// </remarks>
  public string? ConfigName { get; set; }

  /// <summary>
  /// 配置键
  /// </summary>
  /// <remarks>
  /// 配置项的唯一标识键
  /// </remarks>
  public string? ConfigKey { get; set; }

  /// <summary>
  /// 配置类型
  /// </summary>
  /// <remarks>
  /// 配置值的数据类型：String-字符串，Number-数值，Boolean-布尔值，Json-JSON对象，Other-其他
  /// </remarks>
  public int? ConfigType { get; set; }

  /// <summary>
  /// 配置分组
  /// </summary>
  /// <remarks>
  /// 配置项所属的分组
  /// </remarks>
  public string? ConfigGroup { get; set; }

  /// <summary>
  /// 状态
  /// </summary>
  /// <remarks>
  /// 配置项状态：0-正常，1-停用
  /// </remarks>
  public int? ConfigStatus { get; set; }

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
/// 系统配置创建参数
/// </summary>
public class LeanConfigCreateDto
{
  /// <summary>
  /// 配置名称
  /// </summary>
  /// <remarks>
  /// 配置项的显示名称
  /// </remarks>
  [Required(ErrorMessage = "配置名称不能为空")]
  [StringLength(100, MinimumLength = 2, ErrorMessage = "配置名称长度必须在2-100个字符之间")]
  public string ConfigName { get; set; } = string.Empty;

  /// <summary>
  /// 配置键
  /// </summary>
  /// <remarks>
  /// 配置项的唯一标识键
  /// </remarks>
  [Required(ErrorMessage = "配置键不能为空")]
  [StringLength(100, MinimumLength = 2, ErrorMessage = "配置键长度必须在2-100个字符之间")]
  public string ConfigKey { get; set; } = string.Empty;

  /// <summary>
  /// 配置值
  /// </summary>
  /// <remarks>
  /// 配置项的值
  /// </remarks>
  [Required(ErrorMessage = "配置值不能为空")]
  [StringLength(4000, ErrorMessage = "配置值长度不能超过4000个字符")]
  public string ConfigValue { get; set; } = string.Empty;

  /// <summary>
  /// 配置类型
  /// </summary>
  /// <remarks>
  /// 配置值的数据类型：String-字符串，Number-数值，Boolean-布尔值，Json-JSON对象，Other-其他
  /// </remarks>
  [Required(ErrorMessage = "配置类型不能为空")]
  public int ConfigType { get; set; }

  /// <summary>
  /// 配置分组
  /// </summary>
  /// <remarks>
  /// 配置项所属的分组
  /// </remarks>
  [StringLength(50, ErrorMessage = "配置分组长度不能超过50个字符")]
  public string? ConfigGroup { get; set; }

  /// <summary>
  /// 备注
  /// </summary>
  [StringLength(500, ErrorMessage = "备注长度不能超过500个字符")]
  public string? Remark { get; set; }

  /// <summary>
  /// 排序号
  /// </summary>
  /// <remarks>
  /// 显示顺序，数值越小越靠前
  /// </remarks>
  public int OrderNum { get; set; }
}

/// <summary>
/// 系统配置更新参数
/// </summary>
public class LeanConfigUpdateDto : LeanConfigCreateDto
{
  /// <summary>
  /// 主键
  /// </summary>
  [Required(ErrorMessage = "配置ID不能为空")]
  public long Id { get; set; }
}

/// <summary>
/// 系统配置状态变更参数
/// </summary>
public class LeanConfigChangeStatusDto
{
  /// <summary>
  /// 主键
  /// </summary>
  [Required(ErrorMessage = "配置ID不能为空")]
  public long Id { get; set; }

  /// <summary>
  /// 状态
  /// </summary>
  /// <remarks>
  /// 配置项状态：0-正常，1-停用
  /// </remarks>
  [Required(ErrorMessage = "状态不能为空")]
  public int ConfigStatus { get; set; }
}

/// <summary>
/// 系统配置DTO
/// </summary>
public class LeanConfigDto : LeanBaseEntity
{
  /// <summary>
  /// 配置名称
  /// </summary>
  /// <remarks>
  /// 配置项的显示名称
  /// </remarks>
  public string ConfigName { get; set; } = string.Empty;

  /// <summary>
  /// 配置键
  /// </summary>
  /// <remarks>
  /// 配置项的唯一标识键
  /// </remarks>
  public string ConfigKey { get; set; } = string.Empty;

  /// <summary>
  /// 配置值
  /// </summary>
  /// <remarks>
  /// 配置项的值
  /// </remarks>
  public string ConfigValue { get; set; } = string.Empty;

  /// <summary>
  /// 配置类型
  /// </summary>
  /// <remarks>
  /// 配置值的数据类型：String-字符串，Number-数值，Boolean-布尔值，Json-JSON对象，Other-其他
  /// </remarks>
  public int ConfigType { get; set; }

  /// <summary>
  /// 系统内置
  /// </summary>
  /// <remarks>
  /// 是否为系统内置配置项：No-否，Yes-是
  /// 内置配置项不允许删除
  /// </remarks>
  public int IsBuiltin { get; set; }

  /// <summary>
  /// 状态
  /// </summary>
  /// <remarks>
  /// 配置项状态：0-正常，1-停用
  /// </remarks>
  public int ConfigStatus { get; set; }

  /// <summary>
  /// 配置分组
  /// </summary>
  /// <remarks>
  /// 配置项所属的分组
  /// </remarks>
  public string? ConfigGroup { get; set; }

  /// <summary>
  /// 排序号
  /// </summary>
  /// <remarks>
  /// 显示顺序，数值越小越靠前
  /// </remarks>
  public int OrderNum { get; set; }
}

/// <summary>
/// 系统配置导出对象
/// </summary>
public class LeanConfigExportDto
{
  /// <summary>
  /// 配置名称
  /// </summary>
  /// <remarks>
  /// 配置项的显示名称
  /// </remarks>
  [LeanExcelColumn("配置名称", DataType = LeanExcelDataType.String)]
  public string ConfigName { get; set; } = default!;

  /// <summary>
  /// 配置键
  /// </summary>
  /// <remarks>
  /// 配置项的唯一标识键
  /// </remarks>
  [LeanExcelColumn("配置键", DataType = LeanExcelDataType.String)]
  public string ConfigKey { get; set; } = default!;

  /// <summary>
  /// 配置值
  /// </summary>
  /// <remarks>
  /// 配置项的值
  /// </remarks>
  [LeanExcelColumn("配置值", DataType = LeanExcelDataType.String)]
  public string ConfigValue { get; set; } = default!;

  /// <summary>
  /// 配置类型
  /// </summary>
  /// <remarks>
  /// 配置值的数据类型：String-字符串，Number-数值，Boolean-布尔值，Json-JSON对象，Other-其他
  /// </remarks>
  [LeanExcelColumn("配置类型", DataType = LeanExcelDataType.Int)]
  public int ConfigType { get; set; }

  /// <summary>
  /// 系统内置
  /// </summary>
  /// <remarks>
  /// 是否为系统内置配置项：No-否，Yes-是
  /// 内置配置项不允许删除
  /// </remarks>
  [LeanExcelColumn("系统内置", DataType = LeanExcelDataType.Int)]
  public int IsBuiltin { get; set; }

  /// <summary>
  /// 状态
  /// </summary>
  /// <remarks>
  /// 配置项状态：0-正常，1-停用
  /// </remarks>
  [LeanExcelColumn("状态", DataType = LeanExcelDataType.Int)]
  public int ConfigStatus { get; set; }

  /// <summary>
  /// 配置分组
  /// </summary>
  /// <remarks>
  /// 配置项所属的分组
  /// </remarks>
  [LeanExcelColumn("配置分组", DataType = LeanExcelDataType.String)]
  public string? ConfigGroup { get; set; }

  /// <summary>
  /// 备注
  /// </summary>
  [LeanExcelColumn("备注", DataType = LeanExcelDataType.String)]
  public string? Remark { get; set; }

  /// <summary>
  /// 排序号
  /// </summary>
  /// <remarks>
  /// 显示顺序，数值越小越靠前
  /// </remarks>
  [LeanExcelColumn("排序号", DataType = LeanExcelDataType.Int)]
  public int OrderNum { get; set; }
}

/// <summary>
/// 系统配置导入对象
/// </summary>
public class LeanConfigImportDto
{
  /// <summary>
  /// 配置名称
  /// </summary>
  /// <remarks>
  /// 配置项的显示名称
  /// </remarks>
  [Required(ErrorMessage = "配置名称不能为空")]
  [StringLength(100, MinimumLength = 2, ErrorMessage = "配置名称长度必须在2-100个字符之间")]
  [LeanExcelColumn("配置名称", DataType = LeanExcelDataType.String)]
  public string ConfigName { get; set; } = default!;

  /// <summary>
  /// 配置键
  /// </summary>
  /// <remarks>
  /// 配置项的唯一标识键
  /// </remarks>
  [Required(ErrorMessage = "配置键不能为空")]
  [StringLength(100, MinimumLength = 2, ErrorMessage = "配置键长度必须在2-100个字符之间")]
  [LeanExcelColumn("配置键", DataType = LeanExcelDataType.String)]
  public string ConfigKey { get; set; } = default!;

  /// <summary>
  /// 配置值
  /// </summary>
  /// <remarks>
  /// 配置项的值
  /// </remarks>
  [Required(ErrorMessage = "配置值不能为空")]
  [StringLength(4000, ErrorMessage = "配置值长度不能超过4000个字符")]
  [LeanExcelColumn("配置值", DataType = LeanExcelDataType.String)]
  public string ConfigValue { get; set; } = default!;

  /// <summary>
  /// 配置类型
  /// </summary>
  /// <remarks>
  /// 配置值的数据类型：String-字符串，Number-数值，Boolean-布尔值，Json-JSON对象，Other-其他
  /// </remarks>
  [Required(ErrorMessage = "配置类型不能为空")]
  [LeanExcelColumn("配置类型", DataType = LeanExcelDataType.Int)]
  public int ConfigType { get; set; }

  /// <summary>
  /// 配置分组
  /// </summary>
  /// <remarks>
  /// 配置项所属的分组
  /// </remarks>
  [StringLength(50, ErrorMessage = "配置分组长度不能超过50个字符")]
  [LeanExcelColumn("配置分组", DataType = LeanExcelDataType.String)]
  public string? ConfigGroup { get; set; }

  /// <summary>
  /// 备注
  /// </summary>
  [StringLength(500, ErrorMessage = "备注长度不能超过500个字符")]
  [LeanExcelColumn("备注", DataType = LeanExcelDataType.String)]
  public string? Remark { get; set; }

  /// <summary>
  /// 排序号
  /// </summary>
  /// <remarks>
  /// 显示顺序，数值越小越靠前
  /// </remarks>
  [LeanExcelColumn("排序号", DataType = LeanExcelDataType.Int)]
  public int OrderNum { get; set; }
}

/// <summary>
/// 系统配置导入模板对象
/// </summary>
public class LeanConfigImportTemplateDto
{
  /// <summary>
  /// 配置名称
  /// </summary>
  /// <remarks>
  /// 配置项的显示名称
  /// </remarks>
  [LeanExcelColumn("配置名称", DataType = LeanExcelDataType.String)]
  public string ConfigName { get; set; } = default!;

  /// <summary>
  /// 配置键
  /// </summary>
  /// <remarks>
  /// 配置项的唯一标识键
  /// </remarks>
  [LeanExcelColumn("配置键", DataType = LeanExcelDataType.String)]
  public string ConfigKey { get; set; } = default!;

  /// <summary>
  /// 配置值
  /// </summary>
  /// <remarks>
  /// 配置项的值
  /// </remarks>
  [LeanExcelColumn("配置值", DataType = LeanExcelDataType.String)]
  public string ConfigValue { get; set; } = default!;

  /// <summary>
  /// 配置类型
  /// </summary>
  /// <remarks>
  /// 配置值的数据类型：String-字符串，Number-数值，Boolean-布尔值，Json-JSON对象，Other-其他
  /// </remarks>
  [LeanExcelColumn("配置类型", DataType = LeanExcelDataType.Int)]
  public int ConfigType { get; set; }

  /// <summary>
  /// 配置分组
  /// </summary>
  /// <remarks>
  /// 配置项所属的分组
  /// </remarks>
  [LeanExcelColumn("配置分组", DataType = LeanExcelDataType.String)]
  public string? ConfigGroup { get; set; }

  /// <summary>
  /// 备注
  /// </summary>
  [LeanExcelColumn("备注", DataType = LeanExcelDataType.String)]
  public string? Remark { get; set; }

  /// <summary>
  /// 排序号
  /// </summary>
  /// <remarks>
  /// 显示顺序，数值越小越靠前
  /// </remarks>
  [LeanExcelColumn("排序号", DataType = LeanExcelDataType.Int)]
  public int OrderNum { get; set; }
}