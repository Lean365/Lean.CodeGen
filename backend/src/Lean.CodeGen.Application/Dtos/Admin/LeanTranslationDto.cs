using System.ComponentModel.DataAnnotations;
using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Domain.Entities;
using Lean.CodeGen.Common.Excel;

namespace Lean.CodeGen.Application.Dtos.Admin;

/// <summary>
/// 翻译查询参数
/// </summary>
public class LeanTranslationQueryDto : LeanPage
{
  /// <summary>
  /// 语言ID
  /// </summary>
  /// <remarks>
  /// 关联的语言ID
  /// </remarks>
  public long? LangId { get; set; }

  /// <summary>
  /// 语言代码
  /// </summary>
  /// <remarks>
  /// 关联语言的代码，如：zh-CN、en-US等
  /// </remarks>
  public string? LangCode { get; set; }

  /// <summary>
  /// 翻译键名
  /// </summary>
  /// <remarks>
  /// 翻译项的唯一标识符，如：common.ok、common.cancel等
  /// </remarks>
  public string? TransKey { get; set; }

  /// <summary>
  /// 翻译值
  /// </summary>
  /// <remarks>
  /// 翻译项的值，如：确定、取消等
  /// </remarks>
  public string? TransValue { get; set; }

  /// <summary>
  /// 模块名称
  /// </summary>
  /// <remarks>
  /// 所属模块，如：common、system等
  /// </remarks>
  public string? ModuleName { get; set; }

  /// <summary>
  /// 状态
  /// </summary>
  /// <remarks>
  /// 翻译状态：0-正常，1-停用
  /// </remarks>
  public int? TransStatus { get; set; }

  /// <summary>
  /// 开始时间
  /// </summary>
  public DateTime? StartTime { get; set; }

  /// <summary>
  /// 结束时间
  /// </summary>
  public DateTime? EndTime { get; set; }

  /// <summary>
  /// 是否包含语言信息
  /// </summary>
  /// <remarks>
  /// 是否在查询结果中包含语言详细信息
  /// </remarks>
  public bool IncludeLanguage { get; set; }
}

/// <summary>
/// 翻译创建参数
/// </summary>
public class LeanTranslationCreateDto
{
  /// <summary>
  /// 语言ID
  /// </summary>
  /// <remarks>
  /// 关联的语言ID
  /// </remarks>
  [Required(ErrorMessage = "语言ID不能为空")]
  public long LangId { get; set; }

  /// <summary>
  /// 翻译键名
  /// </summary>
  /// <remarks>
  /// 翻译项的唯一标识符，如：common.ok、common.cancel等
  /// </remarks>
  [Required(ErrorMessage = "翻译键不能为空")]
  [StringLength(100, MinimumLength = 2, ErrorMessage = "翻译键长度必须在2-100个字符之间")]
  public string TransKey { get; set; } = string.Empty;

  /// <summary>
  /// 翻译值
  /// </summary>
  /// <remarks>
  /// 翻译项的值，如：确定、取消等
  /// </remarks>
  [Required(ErrorMessage = "翻译值不能为空")]
  [StringLength(4000, ErrorMessage = "翻译值长度不能超过4000个字符")]
  public string TransValue { get; set; } = string.Empty;

  /// <summary>
  /// 模块名称
  /// </summary>
  /// <remarks>
  /// 所属模块，如：common、system等
  /// </remarks>
  [Required(ErrorMessage = "模块名称不能为空")]
  [StringLength(50, ErrorMessage = "模块名称长度不能超过50个字符")]
  public string ModuleName { get; set; } = string.Empty;

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
/// 翻译更新参数
/// </summary>
public class LeanTranslationUpdateDto : LeanTranslationCreateDto
{
  /// <summary>
  /// 主键
  /// </summary>
  [Required(ErrorMessage = "翻译ID不能为空")]
  public long Id { get; set; }
}

/// <summary>
/// 翻译状态变更参数
/// </summary>
public class LeanTranslationChangeStatusDto
{
  /// <summary>
  /// 主键
  /// </summary>
  [Required(ErrorMessage = "翻译ID不能为空")]
  public long Id { get; set; }

  /// <summary>
  /// 状态
  /// </summary>
  /// <remarks>
  /// 翻译状态：0-正常，1-停用
  /// </remarks>
  [Required(ErrorMessage = "状态不能为空")]
  public int TransStatus { get; set; }
}

/// <summary>
/// 翻译DTO
/// </summary>
public class LeanTranslationDto : LeanBaseEntity
{
  /// <summary>
  /// 语言ID
  /// </summary>
  /// <remarks>
  /// 关联的语言ID
  /// </remarks>
  public long LangId { get; set; }

  /// <summary>
  /// 语言代码
  /// </summary>
  /// <remarks>
  /// 关联语言的代码，如：zh-CN、en-US等
  /// </remarks>
  public string LangCode { get; set; } = string.Empty;

  /// <summary>
  /// 语言名称
  /// </summary>
  /// <remarks>
  /// 关联语言的名称，如：简体中文、English等
  /// </remarks>
  public string LangName { get; set; } = string.Empty;

  /// <summary>
  /// 翻译键名
  /// </summary>
  /// <remarks>
  /// 翻译项的唯一标识符，如：common.ok、common.cancel等
  /// </remarks>
  public string TransKey { get; set; } = string.Empty;

  /// <summary>
  /// 翻译值
  /// </summary>
  /// <remarks>
  /// 翻译项的值，如：确定、取消等
  /// </remarks>
  public string TransValue { get; set; } = string.Empty;

  /// <summary>
  /// 模块名称
  /// </summary>
  /// <remarks>
  /// 所属模块，如：common、system等
  /// </remarks>
  public string ModuleName { get; set; } = string.Empty;

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
  /// 翻译状态：0-正常，1-停用
  /// </remarks>
  public int TransStatus { get; set; }

  /// <summary>
  /// 是否内置
  /// </summary>
  /// <remarks>
  /// 是否为系统内置翻译：0-否，1-是
  /// </remarks>
  public int IsBuiltin { get; set; }

  /// <summary>
  /// 所属语言
  /// </summary>
  /// <remarks>
  /// 翻译与语言的多对一关系
  /// </remarks>
  public LeanLanguageDto Language { get; set; } = null!;
}

/// <summary>
/// 翻译导出对象
/// </summary>
public class LeanTranslationExportDto
{
  /// <summary>
  /// 语言ID
  /// </summary>
  /// <remarks>
  /// 关联的语言ID
  /// </remarks>
  [LeanExcelColumn("语言ID", DataType = LeanExcelDataType.Int)]
  public long LangId { get; set; }

  /// <summary>
  /// 语言代码
  /// </summary>
  /// <remarks>
  /// 关联语言的代码，如：zh-CN、en-US等
  /// </remarks>
  [LeanExcelColumn("语言代码", DataType = LeanExcelDataType.String)]
  public string LangCode { get; set; } = default!;

  /// <summary>
  /// 语言名称
  /// </summary>
  /// <remarks>
  /// 关联语言的名称，如：简体中文、English等
  /// </remarks>
  [LeanExcelColumn("语言名称", DataType = LeanExcelDataType.String)]
  public string LangName { get; set; } = default!;

  /// <summary>
  /// 翻译键名
  /// </summary>
  /// <remarks>
  /// 翻译项的唯一标识符，如：common.ok、common.cancel等
  /// </remarks>
  [LeanExcelColumn("翻译键", DataType = LeanExcelDataType.String)]
  public string TransKey { get; set; } = default!;

  /// <summary>
  /// 翻译值
  /// </summary>
  /// <remarks>
  /// 翻译项的值，如：确定、取消等
  /// </remarks>
  [LeanExcelColumn("翻译值", DataType = LeanExcelDataType.String)]
  public string TransValue { get; set; } = default!;

  /// <summary>
  /// 模块名称
  /// </summary>
  /// <remarks>
  /// 所属模块，如：common、system等
  /// </remarks>
  [LeanExcelColumn("模块名称", DataType = LeanExcelDataType.String)]
  public string ModuleName { get; set; } = default!;

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
  /// 翻译状态：0-正常，1-停用
  /// </remarks>
  [LeanExcelColumn("状态", DataType = LeanExcelDataType.Int)]
  public int TransStatus { get; set; }

  /// <summary>
  /// 是否内置
  /// </summary>
  /// <remarks>
  /// 是否为系统内置翻译：0-否，1-是
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
/// 翻译导入对象
/// </summary>
public class LeanTranslationImportDto
{
  /// <summary>
  /// 语言ID
  /// </summary>
  /// <remarks>
  /// 关联的语言ID
  /// </remarks>
  [Required(ErrorMessage = "语言ID不能为空")]
  [LeanExcelColumn("语言ID", DataType = LeanExcelDataType.Int)]
  public long LangId { get; set; }

  /// <summary>
  /// 翻译键名
  /// </summary>
  /// <remarks>
  /// 翻译项的唯一标识符，如：common.ok、common.cancel等
  /// </remarks>
  [Required(ErrorMessage = "翻译键不能为空")]
  [StringLength(100, MinimumLength = 2, ErrorMessage = "翻译键长度必须在2-100个字符之间")]
  [LeanExcelColumn("翻译键", DataType = LeanExcelDataType.String)]
  public string TransKey { get; set; } = default!;

  /// <summary>
  /// 翻译值
  /// </summary>
  /// <remarks>
  /// 翻译项的值，如：确定、取消等
  /// </remarks>
  [Required(ErrorMessage = "翻译值不能为空")]
  [StringLength(4000, ErrorMessage = "翻译值长度不能超过4000个字符")]
  [LeanExcelColumn("翻译值", DataType = LeanExcelDataType.String)]
  public string TransValue { get; set; } = default!;

  /// <summary>
  /// 模块名称
  /// </summary>
  /// <remarks>
  /// 所属模块，如：common、system等
  /// </remarks>
  [Required(ErrorMessage = "模块名称不能为空")]
  [StringLength(50, ErrorMessage = "模块名称长度不能超过50个字符")]
  [LeanExcelColumn("模块名称", DataType = LeanExcelDataType.String)]
  public string ModuleName { get; set; } = default!;

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
/// 翻译导入模板对象
/// </summary>
public class LeanTranslationImportTemplateDto
{
  /// <summary>
  /// 语言ID
  /// </summary>
  /// <remarks>
  /// 关联的语言ID
  /// </remarks>
  [LeanExcelColumn("语言ID", DataType = LeanExcelDataType.Int)]
  public long LangId { get; set; }

  /// <summary>
  /// 翻译键名
  /// </summary>
  /// <remarks>
  /// 翻译项的唯一标识符，如：common.ok、common.cancel等
  /// </remarks>
  [LeanExcelColumn("翻译键", DataType = LeanExcelDataType.String)]
  public string TransKey { get; set; } = default!;

  /// <summary>
  /// 翻译值
  /// </summary>
  /// <remarks>
  /// 翻译项的值，如：确定、取消等
  /// </remarks>
  [LeanExcelColumn("翻译值", DataType = LeanExcelDataType.String)]
  public string TransValue { get; set; } = default!;

  /// <summary>
  /// 模块名称
  /// </summary>
  /// <remarks>
  /// 所属模块，如：common、system等
  /// </remarks>
  [LeanExcelColumn("模块名称", DataType = LeanExcelDataType.String)]
  public string ModuleName { get; set; } = default!;

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

/// <summary>
/// 转置的翻译DTO
/// </summary>
public class LeanTranslationTransposeDto
{
  /// <summary>
  /// 翻译键
  /// </summary>
  public string TransKey { get; set; }

  /// <summary>
  /// 模块名称
  /// </summary>
  public string ModuleName { get; set; }

  /// <summary>
  /// 状态
  /// </summary>
  public int Status { get; set; }

  /// <summary>
  /// 是否内置
  /// </summary>
  public int IsBuiltin { get; set; }

  /// <summary>
  /// 各语言的翻译值
  /// </summary>
  public Dictionary<string, string> Translations { get; set; }
}

/// <summary>
/// 创建转置翻译DTO
/// </summary>
public class LeanTranslationTransposeCreateDto
{
  /// <summary>
  /// 翻译键
  /// </summary>
  public string TransKey { get; set; }

  /// <summary>
  /// 模块名称
  /// </summary>
  public string ModuleName { get; set; }

  /// <summary>
  /// 各语言的翻译值
  /// </summary>
  public Dictionary<string, string> Translations { get; set; }
}

/// <summary>
/// 更新转置翻译DTO
/// </summary>
public class LeanTranslationTransposeUpdateDto
{
  /// <summary>
  /// 翻译键
  /// </summary>
  public string TransKey { get; set; }

  /// <summary>
  /// 模块名称
  /// </summary>
  public string ModuleName { get; set; }

  /// <summary>
  /// 各语言的翻译值
  /// </summary>
  public Dictionary<string, string> Translations { get; set; }
}