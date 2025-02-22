using System.ComponentModel.DataAnnotations;
using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Domain.Entities;
using Lean.CodeGen.Common.Excel;

namespace Lean.CodeGen.Application.Dtos.Admin;

/// <summary>
/// 语言查询参数
/// </summary>
public class LeanQueryLanguageDto : LeanPage
{
    /// <summary>
    /// 语言代码
    /// </summary>
    public string? LangCode { get; set; }

    /// <summary>
    /// 语言名称
    /// </summary>
    public string? LangName { get; set; }

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
/// 语言创建参数
/// </summary>
public class LeanCreateLanguageDto
{
    /// <summary>
    /// 语言代码
    /// </summary>
    [Required(ErrorMessage = "语言代码不能为空")]
    [StringLength(20, MinimumLength = 2, ErrorMessage = "语言代码长度必须在2-20个字符之间")]
    public string LangCode { get; set; } = string.Empty;

    /// <summary>
    /// 语言名称
    /// </summary>
    [Required(ErrorMessage = "语言名称不能为空")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "语言名称长度必须在2-50个字符之间")]
    public string LangName { get; set; } = string.Empty;

    /// <summary>
    /// 本地名称
    /// </summary>
    [Required(ErrorMessage = "本地名称不能为空")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "本地名称长度必须在2-50个字符之间")]
    public string LocalName { get; set; } = string.Empty;

    /// <summary>
    /// 图标
    /// </summary>
    [StringLength(100, ErrorMessage = "图标长度不能超过100个字符")]
    public string? Icon { get; set; }

    /// <summary>
    /// 排序号
    /// </summary>
    public int OrderNum { get; set; }

    /// <summary>
    /// 是否默认
    /// </summary>
    public LeanYesNo IsDefault { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [StringLength(500, ErrorMessage = "备注长度不能超过500个字符")]
    public string? Remark { get; set; }
}

/// <summary>
/// 语言更新参数
/// </summary>
public class LeanUpdateLanguageDto : LeanCreateLanguageDto
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
public class LeanChangeLanguageStatusDto
{
    /// <summary>
    /// 语言ID
    /// </summary>
    [Required(ErrorMessage = "语言ID不能为空")]
    public long Id { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    [Required(ErrorMessage = "状态不能为空")]
    public LeanStatus Status { get; set; }
}

/// <summary>
/// 语言DTO
/// </summary>
public class LeanLanguageDto : LeanBaseEntity
{
    /// <summary>
    /// 语言代码
    /// </summary>
    public string LangCode { get; set; } = string.Empty;

    /// <summary>
    /// 语言名称
    /// </summary>
    public string LangName { get; set; } = string.Empty;

    /// <summary>
    /// 本地名称
    /// </summary>
    public string LocalName { get; set; } = string.Empty;

    /// <summary>
    /// 图标
    /// </summary>
    public string? Icon { get; set; }

    /// <summary>
    /// 排序号
    /// </summary>
    public int OrderNum { get; set; }

    /// <summary>
    /// 是否默认
    /// </summary>
    public LeanYesNo IsDefault { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public LeanStatus Status { get; set; }
}

/// <summary>
/// 语言导出对象
/// </summary>
public class LeanLanguageExportDto
{
    /// <summary>
    /// 语言代码
    /// </summary>
    [LeanExcelColumn("语言代码", DataType = LeanExcelDataType.String)]
    public string LangCode { get; set; } = default!;

    /// <summary>
    /// 语言名称
    /// </summary>
    [LeanExcelColumn("语言名称", DataType = LeanExcelDataType.String)]
    public string LangName { get; set; } = default!;

    /// <summary>
    /// 本地名称
    /// </summary>
    [LeanExcelColumn("本地名称", DataType = LeanExcelDataType.String)]
    public string LocalName { get; set; } = default!;

    /// <summary>
    /// 图标
    /// </summary>
    [LeanExcelColumn("图标", DataType = LeanExcelDataType.String)]
    public string? Icon { get; set; }

    /// <summary>
    /// 排序号
    /// </summary>
    [LeanExcelColumn("排序号", DataType = LeanExcelDataType.Int)]
    public int OrderNum { get; set; }

    /// <summary>
    /// 是否默认
    /// </summary>
    [LeanExcelColumn("是否默认", DataType = LeanExcelDataType.Int)]
    public int IsDefault { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    [LeanExcelColumn("状态", DataType = LeanExcelDataType.Int)]
    public int Status { get; set; }

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
/// 语言导入对象
/// </summary>
public class LeanLanguageImportDto
{
    /// <summary>
    /// 语言代码
    /// </summary>
    [LeanExcelColumn("语言代码", DataType = LeanExcelDataType.String)]
    public string LangCode { get; set; } = default!;

    /// <summary>
    /// 语言名称
    /// </summary>
    [LeanExcelColumn("语言名称", DataType = LeanExcelDataType.String)]
    public string LangName { get; set; } = default!;

    /// <summary>
    /// 本地名称
    /// </summary>
    [LeanExcelColumn("本地名称", DataType = LeanExcelDataType.String)]
    public string LocalName { get; set; } = default!;

    /// <summary>
    /// 图标
    /// </summary>
    [LeanExcelColumn("图标", DataType = LeanExcelDataType.String)]
    public string? Icon { get; set; }

    /// <summary>
    /// 排序号
    /// </summary>
    [LeanExcelColumn("排序号", DataType = LeanExcelDataType.Int)]
    public int OrderNum { get; set; }

    /// <summary>
    /// 是否默认
    /// </summary>
    [LeanExcelColumn("是否默认", DataType = LeanExcelDataType.Int)]
    public int IsDefault { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    [LeanExcelColumn("状态", DataType = LeanExcelDataType.Int)]
    public int Status { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [LeanExcelColumn("备注", DataType = LeanExcelDataType.String)]
    public string? Remark { get; set; }
}

/// <summary>
/// 语言导入模板对象
/// </summary>
public class LeanLanguageImportTemplateDto
{
    /// <summary>
    /// 语言代码
    /// </summary>
    [LeanExcelColumn("语言代码", DataType = LeanExcelDataType.String)]
    public string LangCode { get; set; } = default!;

    /// <summary>
    /// 语言名称
    /// </summary>
    [LeanExcelColumn("语言名称", DataType = LeanExcelDataType.String)]
    public string LangName { get; set; } = default!;

    /// <summary>
    /// 本地名称
    /// </summary>
    [LeanExcelColumn("本地名称", DataType = LeanExcelDataType.String)]
    public string LocalName { get; set; } = default!;

    /// <summary>
    /// 图标
    /// </summary>
    [LeanExcelColumn("图标", DataType = LeanExcelDataType.String)]
    public string? Icon { get; set; }

    /// <summary>
    /// 排序号
    /// </summary>
    [LeanExcelColumn("排序号", DataType = LeanExcelDataType.Int)]
    public int OrderNum { get; set; }

    /// <summary>
    /// 是否默认
    /// </summary>
    [LeanExcelColumn("是否默认", DataType = LeanExcelDataType.Int)]
    public int IsDefault { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    [LeanExcelColumn("状态", DataType = LeanExcelDataType.Int)]
    public int Status { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [LeanExcelColumn("备注", DataType = LeanExcelDataType.String)]
    public string? Remark { get; set; }
}