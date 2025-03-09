using System.ComponentModel.DataAnnotations;
using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Domain.Entities;
using Lean.CodeGen.Common.Excel;

namespace Lean.CodeGen.Application.Dtos.Admin;

/// <summary>
/// 字典类型查询参数
/// </summary>
public class LeanDictTypeQueryDto : LeanPage
{
  /// <summary>
  /// 字典名称
  /// </summary>
  /// <remarks>
  /// 字典类型的显示名称，如：用户性别、系统状态等
  /// </remarks>
  public string? DictTypeName { get; set; }

  /// <summary>
  /// 字典编码
  /// </summary>
  /// <remarks>
  /// 字典类型的唯一标识符，如：sys_user_sex、sys_normal_disable等
  /// </remarks>
  public string? DictTypeCode { get; set; }

  /// <summary>
  /// 状态
  /// </summary>
  /// <remarks>
  /// 字典类型状态：0-正常，1-停用
  /// </remarks>
  public int? DictTypeStatus { get; set; }

  /// <summary>
  /// 开始时间
  /// </summary>
  public DateTime? StartTime { get; set; }

  /// <summary>
  /// 结束时间
  /// </summary>
  public DateTime? EndTime { get; set; }

  /// <summary>
  /// 是否包含字典数据
  /// </summary>
  /// <remarks>
  /// 是否在查询结果中包含字典数据列表
  /// </remarks>
  public bool IncludeData { get; set; }
}

/// <summary>
/// 根据字典名称查询参数
/// </summary>
public class LeanDictTypeQueryByNameDto
{
  /// <summary>
  /// 字典名称
  /// </summary>
  /// <remarks>
  /// 字典类型的显示名称，如：用户性别、系统状态等
  /// </remarks>
  [Required(ErrorMessage = "字典名称不能为空")]
  [StringLength(100, MinimumLength = 2, ErrorMessage = "字典名称长度必须在2-100个字符之间")]
  public string DictTypeName { get; set; } = string.Empty;

  /// <summary>
  /// 是否包含字典数据
  /// </summary>
  /// <remarks>
  /// 是否在查询结果中包含字典数据列表
  /// </remarks>
  public bool IncludeData { get; set; }
}

/// <summary>
/// 字典类型创建参数
/// </summary>
public class LeanDictTypeCreateDto
{
  /// <summary>
  /// 字典名称
  /// </summary>
  /// <remarks>
  /// 字典类型的显示名称，如：用户性别、系统状态等
  /// </remarks>
  [Required(ErrorMessage = "字典名称不能为空")]
  [StringLength(100, MinimumLength = 2, ErrorMessage = "字典名称长度必须在2-100个字符之间")]
  public string DictTypeName { get; set; } = string.Empty;

  /// <summary>
  /// 字典编码
  /// </summary>
  /// <remarks>
  /// 字典类型的唯一标识符，如：sys_user_sex、sys_normal_disable等
  /// </remarks>
  [Required(ErrorMessage = "字典编码不能为空")]
  [StringLength(100, MinimumLength = 2, ErrorMessage = "字典编码长度必须在2-100个字符之间")]
  public string DictTypeCode { get; set; } = string.Empty;

  /// <summary>
  /// 字典分类
  /// </summary>
  /// <remarks>
  /// 字典类型分类：0-传统类型，1-SQL类型
  /// </remarks>
  public int DictTypeCategory { get; set; }

  /// <summary>
  /// SQL语句
  /// </summary>
  /// <remarks>
  /// 当分类为SQL类型时的查询语句
  /// </remarks>
  public string? SqlStatement { get; set; }

  /// <summary>
  /// 排序号
  /// </summary>
  /// <remarks>
  /// 显示顺序，数值越小越靠前
  /// </remarks>
  public int OrderNum { get; set; }

  /// <summary>
  /// 备注
  /// </summary>
  [StringLength(500, ErrorMessage = "备注长度不能超过500个字符")]
  public string? Remark { get; set; }
}

/// <summary>
/// 字典类型更新参数
/// </summary>
public class LeanDictTypeUpdateDto : LeanDictTypeCreateDto
{
  /// <summary>
  /// 字典类型ID
  /// </summary>
  [Required(ErrorMessage = "字典类型ID不能为空")]
  public long Id { get; set; }
}

/// <summary>
/// 字典类型状态变更参数
/// </summary>
public class LeanDictTypeChangeStatusDto
{
  /// <summary>
  /// 字典类型ID
  /// </summary>
  [Required(ErrorMessage = "字典类型ID不能为空")]
  public long Id { get; set; }

  /// <summary>
  /// 状态
  /// </summary>
  /// <remarks>
  /// 字典类型状态：0-正常，1-停用
  /// </remarks>
  [Required(ErrorMessage = "状态不能为空")]
  public int DictTypeStatus { get; set; }
}

/// <summary>
/// 字典类型DTO
/// </summary>
public class LeanDictTypeDto : LeanBaseEntity
{
  /// <summary>
  /// 字典名称
  /// </summary>
  /// <remarks>
  /// 字典类型的显示名称，如：用户性别、系统状态等
  /// </remarks>
  public string DictTypeName { get; set; } = string.Empty;

  /// <summary>
  /// 字典编码
  /// </summary>
  /// <remarks>
  /// 字典类型的唯一标识符，如：sys_user_sex、sys_normal_disable等
  /// </remarks>
  public string DictTypeCode { get; set; } = string.Empty;

  /// <summary>
  /// 字典分类
  /// </summary>
  /// <remarks>
  /// 字典类型分类：0-传统类型，1-SQL类型
  /// </remarks>
  public int DictTypeCategory { get; set; }

  /// <summary>
  /// SQL语句
  /// </summary>
  /// <remarks>
  /// 当分类为SQL类型时的查询语句
  /// </remarks>
  public string? SqlStatement { get; set; }

  /// <summary>
  /// 状态
  /// </summary>
  /// <remarks>
  /// 字典类型状态：0-正常，1-停用
  /// </remarks>
  public int DictTypeStatus { get; set; }

  /// <summary>
  /// 排序号
  /// </summary>
  /// <remarks>
  /// 显示顺序，数值越小越靠前
  /// </remarks>
  public int OrderNum { get; set; }

  /// <summary>
  /// 是否内置
  /// </summary>
  /// <remarks>
  /// 是否为系统内置字典类型：0-否，1-是
  /// 内置字典类型不允许删除
  /// </remarks>
  public int IsBuiltin { get; set; }

  /// <summary>
  /// 字典数据列表
  /// </summary>
  /// <remarks>
  /// 字典类型与字典数据的一对多关系
  /// </remarks>
  public List<LeanDictDataDto> DictDataList { get; set; } = new();
}

/// <summary>
/// 字典类型导出对象
/// </summary>
public class LeanDictTypeExportDto
{
  /// <summary>
  /// 字典名称
  /// </summary>
  /// <remarks>
  /// 字典类型的显示名称，如：用户性别、系统状态等
  /// </remarks>
  [LeanExcelColumn("字典名称", DataType = LeanExcelDataType.String)]
  public string DictTypeName { get; set; } = default!;

  /// <summary>
  /// 字典编码
  /// </summary>
  /// <remarks>
  /// 字典类型的唯一标识符，如：sys_user_sex、sys_normal_disable等
  /// </remarks>
  [LeanExcelColumn("字典编码", DataType = LeanExcelDataType.String)]
  public string DictTypeCode { get; set; } = default!;

  /// <summary>
  /// 字典分类
  /// </summary>
  /// <remarks>
  /// 字典类型分类：0-传统类型，1-SQL类型
  /// </remarks>
  [LeanExcelColumn("字典分类", DataType = LeanExcelDataType.Int)]
  public int DictTypeCategory { get; set; }

  /// <summary>
  /// SQL语句
  /// </summary>
  /// <remarks>
  /// 当分类为SQL类型时的查询语句
  /// </remarks>
  [LeanExcelColumn("SQL语句", DataType = LeanExcelDataType.String)]
  public string? SqlStatement { get; set; }

  /// <summary>
  /// 状态
  /// </summary>
  /// <remarks>
  /// 字典类型状态：0-正常，1-停用
  /// </remarks>
  [LeanExcelColumn("状态", DataType = LeanExcelDataType.Int)]
  public int DictTypeStatus { get; set; }

  /// <summary>
  /// 排序号
  /// </summary>
  /// <remarks>
  /// 显示顺序，数值越小越靠前
  /// </remarks>
  [LeanExcelColumn("排序号", DataType = LeanExcelDataType.Int)]
  public int OrderNum { get; set; }

  /// <summary>
  /// 是否内置
  /// </summary>
  /// <remarks>
  /// 是否为系统内置字典类型：0-否，1-是
  /// 内置字典类型不允许删除
  /// </remarks>
  [LeanExcelColumn("是否内置", DataType = LeanExcelDataType.Int)]
  public int IsBuiltin { get; set; }

  /// <summary>
  /// 备注
  /// </summary>
  [LeanExcelColumn("备注", DataType = LeanExcelDataType.String)]
  public string? Remark { get; set; }
}

/// <summary>
/// 字典类型导入对象
/// </summary>
public class LeanDictTypeImportDto
{
  /// <summary>
  /// 字典名称
  /// </summary>
  /// <remarks>
  /// 字典类型的显示名称，如：用户性别、系统状态等
  /// </remarks>
  [Required(ErrorMessage = "字典名称不能为空")]
  [StringLength(100, MinimumLength = 2, ErrorMessage = "字典名称长度必须在2-100个字符之间")]
  [LeanExcelColumn("字典名称", DataType = LeanExcelDataType.String)]
  public string DictTypeName { get; set; } = default!;

  /// <summary>
  /// 字典编码
  /// </summary>
  /// <remarks>
  /// 字典类型的唯一标识符，如：sys_user_sex、sys_normal_disable等
  /// </remarks>
  [Required(ErrorMessage = "字典编码不能为空")]
  [StringLength(100, MinimumLength = 2, ErrorMessage = "字典编码长度必须在2-100个字符之间")]
  [LeanExcelColumn("字典编码", DataType = LeanExcelDataType.String)]
  public string DictTypeCode { get; set; } = default!;

  /// <summary>
  /// 字典分类
  /// </summary>
  /// <remarks>
  /// 字典类型分类：0-传统类型，1-SQL类型
  /// </remarks>
  [LeanExcelColumn("字典分类", DataType = LeanExcelDataType.Int)]
  public int DictTypeCategory { get; set; }

  /// <summary>
  /// SQL语句
  /// </summary>
  /// <remarks>
  /// 当分类为SQL类型时的查询语句
  /// </remarks>
  [LeanExcelColumn("SQL语句", DataType = LeanExcelDataType.String)]
  public string? SqlStatement { get; set; }

  /// <summary>
  /// 状态
  /// </summary>
  /// <remarks>
  /// 字典类型状态：0-正常，1-停用
  /// </remarks>
  [LeanExcelColumn("状态", DataType = LeanExcelDataType.Int)]
  public int DictTypeStatus { get; set; }

  /// <summary>
  /// 排序号
  /// </summary>
  /// <remarks>
  /// 显示顺序，数值越小越靠前
  /// </remarks>
  [LeanExcelColumn("排序号", DataType = LeanExcelDataType.Int)]
  public int OrderNum { get; set; }

  /// <summary>
  /// 备注
  /// </summary>
  [StringLength(500, ErrorMessage = "备注长度不能超过500个字符")]
  [LeanExcelColumn("备注", DataType = LeanExcelDataType.String)]
  public string? Remark { get; set; }
}

/// <summary>
/// 字典类型导入模板对象
/// </summary>
public class LeanDictTypeImportTemplateDto
{
  /// <summary>
  /// 字典名称
  /// </summary>
  /// <remarks>
  /// 字典类型的显示名称，如：用户性别、系统状态等
  /// </remarks>
  [LeanExcelColumn("字典名称", DataType = LeanExcelDataType.String)]
  public string DictTypeName { get; set; } = default!;

  /// <summary>
  /// 字典编码
  /// </summary>
  /// <remarks>
  /// 字典类型的唯一标识符，如：sys_user_sex、sys_normal_disable等
  /// </remarks>
  [LeanExcelColumn("字典编码", DataType = LeanExcelDataType.String)]
  public string DictTypeCode { get; set; } = default!;

  /// <summary>
  /// 字典分类
  /// </summary>
  /// <remarks>
  /// 字典类型分类：0-传统类型，1-SQL类型
  /// </remarks>
  [LeanExcelColumn("字典分类", DataType = LeanExcelDataType.Int)]
  public int DictTypeCategory { get; set; }

  /// <summary>
  /// SQL语句
  /// </summary>
  /// <remarks>
  /// 当分类为SQL类型时的查询语句
  /// </remarks>
  [LeanExcelColumn("SQL语句", DataType = LeanExcelDataType.String)]
  public string? SqlStatement { get; set; }

  /// <summary>
  /// 状态
  /// </summary>
  /// <remarks>
  /// 字典类型状态：0-正常，1-停用
  /// </remarks>
  [LeanExcelColumn("状态", DataType = LeanExcelDataType.Int)]
  public int DictTypeStatus { get; set; }

  /// <summary>
  /// 排序号
  /// </summary>
  /// <remarks>
  /// 显示顺序，数值越小越靠前
  /// </remarks>
  [LeanExcelColumn("排序号", DataType = LeanExcelDataType.Int)]
  public int OrderNum { get; set; }

  /// <summary>
  /// 备注
  /// </summary>
  [LeanExcelColumn("备注", DataType = LeanExcelDataType.String)]
  public string? Remark { get; set; }
}