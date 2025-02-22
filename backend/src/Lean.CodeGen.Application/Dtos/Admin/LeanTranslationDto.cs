using System.ComponentModel.DataAnnotations;
using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Domain.Entities;
using Lean.CodeGen.Common.Excel;

namespace Lean.CodeGen.Application.Dtos.Admin;

/// <summary>
/// 翻译查询参数
/// </summary>
public class LeanQueryTranslationDto : LeanPage
{
    /// <summary>
    /// 语言ID
    /// </summary>
    public long? LangId { get; set; }

    /// <summary>
    /// 翻译键
    /// </summary>
    public string? TransKey { get; set; }

    /// <summary>
    /// 翻译值
    /// </summary>
    public string? TransValue { get; set; }

    /// <summary>
    /// 模块名称
    /// </summary>
    public string? ModuleName { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public LeanStatus? Status { get; set; }

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
/// 翻译创建参数
/// </summary>
public class LeanCreateTranslationDto
{
    /// <summary>
    /// 语言ID
    /// </summary>
    [Required(ErrorMessage = "语言ID不能为空")]
    public long LangId { get; set; }

    /// <summary>
    /// 翻译键
    /// </summary>
    [Required(ErrorMessage = "翻译键不能为空")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "翻译键长度必须在2-100个字符之间")]
    public string TransKey { get; set; } = string.Empty;

    /// <summary>
    /// 翻译值
    /// </summary>
    [Required(ErrorMessage = "翻译值不能为空")]
    [StringLength(4000, ErrorMessage = "翻译值长度不能超过4000个字符")]
    public string TransValue { get; set; } = string.Empty;

    /// <summary>
    /// 模块名称
    /// </summary>
    [StringLength(50, ErrorMessage = "模块名称长度不能超过50个字符")]
    public string? ModuleName { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [StringLength(500, ErrorMessage = "备注长度不能超过500个字符")]
    public string? Remark { get; set; }
}

/// <summary>
/// 翻译更新参数
/// </summary>
public class LeanUpdateTranslationDto : LeanCreateTranslationDto
{
    /// <summary>
    /// 主键
    /// </summary>
    [Required(ErrorMessage = "翻译ID不能为空")]
    public long Id { get; set; }
}

/// <summary>
/// 翻译DTO
/// </summary>
public class LeanTranslationDto : LeanBaseEntity
{
    /// <summary>
    /// 语言ID
    /// </summary>
    public long LangId { get; set; }

    /// <summary>
    /// 翻译键
    /// </summary>
    public string TransKey { get; set; } = string.Empty;

    /// <summary>
    /// 翻译值
    /// </summary>
    public string TransValue { get; set; } = string.Empty;

    /// <summary>
    /// 模块名称
    /// </summary>
    public string? ModuleName { get; set; }

    /// <summary>
    /// 是否内置
    /// </summary>
    public LeanYesNo IsBuiltin { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public LeanStatus Status { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }
}

/// <summary>
/// 翻译导出对象
/// </summary>
public class LeanTranslationExportDto
{
    /// <summary>
    /// 语言ID
    /// </summary>
    [LeanExcelColumn("语言ID", DataType = LeanExcelDataType.Int)]
    public long LangId { get; set; }

    /// <summary>
    /// 翻译键
    /// </summary>
    [LeanExcelColumn("翻译键", DataType = LeanExcelDataType.String)]
    public string TransKey { get; set; } = default!;

    /// <summary>
    /// 翻译值
    /// </summary>
    [LeanExcelColumn("翻译值", DataType = LeanExcelDataType.String)]
    public string TransValue { get; set; } = default!;

    /// <summary>
    /// 模块名称
    /// </summary>
    [LeanExcelColumn("模块名称", DataType = LeanExcelDataType.String)]
    public string? ModuleName { get; set; }

    /// <summary>
    /// 是否内置
    /// </summary>
    [LeanExcelColumn("是否内置", DataType = LeanExcelDataType.Int)]
    public int IsBuiltin { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    [LeanExcelColumn("状态", DataType = LeanExcelDataType.Int)]
    public int Status { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    [LeanExcelColumn("排序", DataType = LeanExcelDataType.Int)]
    public int Sort { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [LeanExcelColumn("备注", DataType = LeanExcelDataType.String)]
    public string? Remark { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    [LeanExcelColumn("创建时间", DataType = LeanExcelDataType.DateTime, Format = "yyyy-MM-dd HH:mm:ss")]
    public DateTime CreateTime { get; set; }
}

/// <summary>
/// 翻译导入对象
/// </summary>
public class LeanTranslationImportDto
{
    /// <summary>
    /// 语言ID
    /// </summary>
    [Required(ErrorMessage = "语言ID不能为空")]
    [LeanExcelColumn("语言ID", DataType = LeanExcelDataType.Int)]
    public long LangId { get; set; }

    /// <summary>
    /// 翻译键
    /// </summary>
    [Required(ErrorMessage = "翻译键不能为空")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "翻译键长度必须在2-100个字符之间")]
    [LeanExcelColumn("翻译键", DataType = LeanExcelDataType.String)]
    public string TransKey { get; set; } = default!;

    /// <summary>
    /// 翻译值
    /// </summary>
    [Required(ErrorMessage = "翻译值不能为空")]
    [StringLength(4000, ErrorMessage = "翻译值长度不能超过4000个字符")]
    [LeanExcelColumn("翻译值", DataType = LeanExcelDataType.String)]
    public string TransValue { get; set; } = default!;

    /// <summary>
    /// 模块名称
    /// </summary>
    [StringLength(50, ErrorMessage = "模块名称长度不能超过50个字符")]
    [LeanExcelColumn("模块名称", DataType = LeanExcelDataType.String)]
    public string? ModuleName { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    [LeanExcelColumn("排序", DataType = LeanExcelDataType.Int)]
    public int Sort { get; set; }

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
    [LeanExcelColumn("语言ID", DataType = LeanExcelDataType.Int)]
    public long LangId { get; set; } = 1;

    /// <summary>
    /// 翻译键
    /// </summary>
    [LeanExcelColumn("翻译键", DataType = LeanExcelDataType.String)]
    public string TransKey { get; set; } = "sys.translation.key";

    /// <summary>
    /// 翻译值
    /// </summary>
    [LeanExcelColumn("翻译值", DataType = LeanExcelDataType.String)]
    public string TransValue { get; set; } = "翻译值";

    /// <summary>
    /// 模块名称
    /// </summary>
    [LeanExcelColumn("模块名称", DataType = LeanExcelDataType.String)]
    public string? ModuleName { get; set; } = "系统管理";

    /// <summary>
    /// 排序
    /// </summary>
    [LeanExcelColumn("排序", DataType = LeanExcelDataType.Int)]
    public int Sort { get; set; } = 1;

    /// <summary>
    /// 备注
    /// </summary>
    [LeanExcelColumn("备注", DataType = LeanExcelDataType.String)]
    public string? Remark { get; set; } = "示例翻译";
}

/// <summary>
/// 翻译状态修改参数
/// </summary>
public class LeanChangeTranslationStatusDto
{
    /// <summary>
    /// 主键
    /// </summary>
    [Required(ErrorMessage = "翻译ID不能为空")]
    public long Id { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    [Required(ErrorMessage = "状态不能为空")]
    public LeanStatus Status { get; set; }
}