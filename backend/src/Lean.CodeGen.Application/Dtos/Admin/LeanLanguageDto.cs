using System.ComponentModel.DataAnnotations;
using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Domain.Entities;
using Lean.CodeGen.Common.Excel;

namespace Lean.CodeGen.Application.Dtos.Admin;

/// <summary>
/// 语言查询参数
/// </summary>
public class LeanLanguageQueryDto : LeanPage
{
  /// <summary>
  /// 语言代码
  /// </summary>
  /// <remarks>
  /// 语言的唯一标识符，如：zh-CN、en-US等
  /// </remarks>
  public string? LangCode { get; set; }

  /// <summary>
  /// 语言名称
  /// </summary>
  /// <remarks>
  /// 语言的显示名称，如：简体中文、English等
  /// </remarks>
  public string? LangName { get; set; }

  /// <summary>
  /// 状态
  /// </summary>
  /// <remarks>
  /// 语言状态：0-正常，1-停用
  /// </remarks>
  public int? LangStatus { get; set; }

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
/// 语言创建参数
/// </summary>
public class LeanLanguageCreateDto
{
  /// <summary>
  /// 语言名称
  /// </summary>
  /// <remarks>
  /// 语言的显示名称，如：简体中文、English等
  /// </remarks>
  [Required(ErrorMessage = "语言名称不能为空")]
  [StringLength(50, MinimumLength = 2, ErrorMessage = "语言名称长度必须在2-50个字符之间")]
  public string LangName { get; set; } = string.Empty;

  /// <summary>
  /// 语言代码
  /// </summary>
  /// <remarks>
  /// 语言的唯一标识符，如：zh-CN、en-US等
  /// </remarks>
  [Required(ErrorMessage = "语言代码不能为空")]
  [StringLength(20, MinimumLength = 2, ErrorMessage = "语言代码长度必须在2-20个字符之间")]
  public string LangCode { get; set; } = string.Empty;

  /// <summary>
  /// 语言图标
  /// </summary>
  /// <remarks>
  /// 语言的图标，如：国旗图标等
  /// </remarks>
  [StringLength(100, ErrorMessage = "图标长度不能超过100个字符")]
  public string? LangIcon { get; set; }

  /// <summary>
  /// 是否默认语言
  /// </summary>
  /// <remarks>
  /// 是否为默认语言：No-否，Yes-是
  /// </remarks>
  public int IsDefault { get; set; }

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
/// 语言更新参数
/// </summary>
public class LeanLanguageUpdateDto : LeanLanguageCreateDto
{
  /// <summary>
  /// 语言ID
  /// </summary>
  [Required(ErrorMessage = "语言ID不能为空")]
  public long Id { get; set; }
}

/// <summary>
/// 语言状态变更参数
/// </summary>
public class LeanLanguageChangeStatusDto
{
  /// <summary>
  /// 语言ID
  /// </summary>
  [Required(ErrorMessage = "语言ID不能为空")]
  public long Id { get; set; }

  /// <summary>
  /// 状态
  /// </summary>
  /// <remarks>
  /// 语言状态：0-正常，1-停用
  /// </remarks>
  [Required(ErrorMessage = "状态不能为空")]
  public int LangStatus { get; set; }
}

/// <summary>
/// 语言DTO
/// </summary>
public class LeanLanguageDto : LeanBaseEntity
{
  /// <summary>
  /// 语言名称
  /// </summary>
  /// <remarks>
  /// 语言的显示名称，如：简体中文、English等
  /// </remarks>
  public string LangName { get; set; } = string.Empty;

  /// <summary>
  /// 语言代码
  /// </summary>
  /// <remarks>
  /// 语言的唯一标识符，如：zh-CN、en-US等
  /// </remarks>
  public string LangCode { get; set; } = string.Empty;

  /// <summary>
  /// 语言图标
  /// </summary>
  /// <remarks>
  /// 语言的图标，如：国旗图标等
  /// </remarks>
  public string? LangIcon { get; set; }

  /// <summary>
  /// 是否默认语言
  /// </summary>
  /// <remarks>
  /// 是否为默认语言：No-否，Yes-是
  /// </remarks>
  public int IsDefault { get; set; }

  /// <summary>
  /// 排序号
  /// </summary>
  /// <remarks>
  /// 显示顺序，数值越小越靠前
  /// </remarks>
  public int OrderNum { get; set; }

  /// <summary>
  /// 状态
  /// </summary>
  /// <remarks>
  /// 语言状态：0-正常，1-停用
  /// </remarks>
  public int LangStatus { get; set; }

  /// <summary>
  /// 是否内置
  /// </summary>
  /// <remarks>
  /// 是否为系统内置语言：0-否，1-是
  /// </remarks>
  public int IsBuiltin { get; set; }

  /// <summary>
  /// 翻译列表
  /// </summary>
  /// <remarks>
  /// 语言与翻译的一对多关系
  /// </remarks>
  public List<LeanTranslationDto> Translations { get; set; } = new();
}

/// <summary>
/// 语言导出对象
/// </summary>
public class LeanLanguageExportDto
{
  /// <summary>
  /// 语言名称
  /// </summary>
  /// <remarks>
  /// 语言的显示名称，如：简体中文、English等
  /// </remarks>
  [LeanExcelColumn("语言名称", DataType = LeanExcelDataType.String)]
  public string LangName { get; set; } = default!;

  /// <summary>
  /// 语言代码
  /// </summary>
  /// <remarks>
  /// 语言的唯一标识符，如：zh-CN、en-US等
  /// </remarks>
  [LeanExcelColumn("语言代码", DataType = LeanExcelDataType.String)]
  public string LangCode { get; set; } = default!;

  /// <summary>
  /// 语言图标
  /// </summary>
  /// <remarks>
  /// 语言的图标，如：国旗图标等
  /// </remarks>
  [LeanExcelColumn("语言图标", DataType = LeanExcelDataType.String)]
  public string? LangIcon { get; set; }

  /// <summary>
  /// 是否默认语言
  /// </summary>
  /// <remarks>
  /// 是否为默认语言：No-否，Yes-是
  /// </remarks>
  [LeanExcelColumn("是否默认", DataType = LeanExcelDataType.Int)]
  public int IsDefault { get; set; }

  /// <summary>
  /// 排序号
  /// </summary>
  /// <remarks>
  /// 显示顺序，数值越小越靠前
  /// </remarks>
  [LeanExcelColumn("排序号", DataType = LeanExcelDataType.Int)]
  public int OrderNum { get; set; }

  /// <summary>
  /// 状态
  /// </summary>
  /// <remarks>
  /// 语言状态：0-正常，1-停用
  /// </remarks>
  [LeanExcelColumn("状态", DataType = LeanExcelDataType.Int)]
  public int LangStatus { get; set; }

  /// <summary>
  /// 是否内置
  /// </summary>
  /// <remarks>
  /// 是否为系统内置语言：0-否，1-是
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
/// 语言导入对象
/// </summary>
public class LeanLanguageImportDto
{
  /// <summary>
  /// 语言名称
  /// </summary>
  /// <remarks>
  /// 语言的显示名称，如：简体中文、English等
  /// </remarks>
  [Required(ErrorMessage = "语言名称不能为空")]
  [StringLength(50, MinimumLength = 2, ErrorMessage = "语言名称长度必须在2-50个字符之间")]
  [LeanExcelColumn("语言名称", DataType = LeanExcelDataType.String)]
  public string LangName { get; set; } = default!;

  /// <summary>
  /// 语言代码
  /// </summary>
  /// <remarks>
  /// 语言的唯一标识符，如：zh-CN、en-US等
  /// </remarks>
  [Required(ErrorMessage = "语言代码不能为空")]
  [StringLength(20, MinimumLength = 2, ErrorMessage = "语言代码长度必须在2-20个字符之间")]
  [LeanExcelColumn("语言代码", DataType = LeanExcelDataType.String)]
  public string LangCode { get; set; } = default!;

  /// <summary>
  /// 语言图标
  /// </summary>
  /// <remarks>
  /// 语言的图标，如：国旗图标等
  /// </remarks>
  [StringLength(100, ErrorMessage = "图标长度不能超过100个字符")]
  [LeanExcelColumn("语言图标", DataType = LeanExcelDataType.String)]
  public string? LangIcon { get; set; }

  /// <summary>
  /// 是否默认语言
  /// </summary>
  /// <remarks>
  /// 是否为默认语言：No-否，Yes-是
  /// </remarks>
  [LeanExcelColumn("是否默认", DataType = LeanExcelDataType.Int)]
  public int IsDefault { get; set; }

  /// <summary>
  /// 排序号
  /// </summary>
  /// <remarks>
  /// 显示顺序，数值越小越靠前
  /// </remarks>
  [LeanExcelColumn("排序号", DataType = LeanExcelDataType.Int)]
  public int OrderNum { get; set; }

  /// <summary>
  /// 状态
  /// </summary>
  /// <remarks>
  /// 语言状态：0-正常，1-停用
  /// </remarks>
  [LeanExcelColumn("状态", DataType = LeanExcelDataType.Int)]
  public int LangStatus { get; set; }

  /// <summary>
  /// 备注
  /// </summary>
  [StringLength(500, ErrorMessage = "备注长度不能超过500个字符")]
  [LeanExcelColumn("备注", DataType = LeanExcelDataType.String)]
  public string? Remark { get; set; }
}

/// <summary>
/// 语言导入模板对象
/// </summary>
public class LeanLanguageImportTemplateDto
{
  /// <summary>
  /// 语言名称
  /// </summary>
  /// <remarks>
  /// 语言的显示名称，如：简体中文、English等
  /// </remarks>
  [LeanExcelColumn("语言名称", DataType = LeanExcelDataType.String)]
  public string LangName { get; set; } = default!;

  /// <summary>
  /// 语言代码
  /// </summary>
  /// <remarks>
  /// 语言的唯一标识符，如：zh-CN、en-US等
  /// </remarks>
  [LeanExcelColumn("语言代码", DataType = LeanExcelDataType.String)]
  public string LangCode { get; set; } = default!;

  /// <summary>
  /// 语言图标
  /// </summary>
  /// <remarks>
  /// 语言的图标，如：国旗图标等
  /// </remarks>
  [LeanExcelColumn("语言图标", DataType = LeanExcelDataType.String)]
  public string? LangIcon { get; set; }

  /// <summary>
  /// 是否默认语言
  /// </summary>
  /// <remarks>
  /// 是否为默认语言：No-否，Yes-是
  /// </remarks>
  [LeanExcelColumn("是否默认", DataType = LeanExcelDataType.Int)]
  public int IsDefault { get; set; }

  /// <summary>
  /// 排序号
  /// </summary>
  /// <remarks>
  /// 显示顺序，数值越小越靠前
  /// </remarks>
  [LeanExcelColumn("排序号", DataType = LeanExcelDataType.Int)]
  public int OrderNum { get; set; }

  /// <summary>
  /// 状态
  /// </summary>
  /// <remarks>
  /// 语言状态：0-正常，1-停用
  /// </remarks>
  [LeanExcelColumn("状态", DataType = LeanExcelDataType.Int)]
  public int LangStatus { get; set; }

  /// <summary>
  /// 备注
  /// </summary>
  [LeanExcelColumn("备注", DataType = LeanExcelDataType.String)]
  public string? Remark { get; set; }
}