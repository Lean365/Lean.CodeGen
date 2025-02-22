using System.ComponentModel.DataAnnotations;
using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Domain.Entities;
using Lean.CodeGen.Common.Excel;

namespace Lean.CodeGen.Application.Dtos.Admin;

/// <summary>
/// 字典类型查询参数
/// </summary>
public class LeanQueryDictTypeDto : LeanPage
{
    /// <summary>
    /// 字典名称
    /// </summary>
    public string? DictName { get; set; }

    /// <summary>
    /// 字典编码
    /// </summary>
    public string? DictCode { get; set; }

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
/// 字典类型创建参数
/// </summary>
public class LeanCreateDictTypeDto
{
    /// <summary>
    /// 字典名称
    /// </summary>
    [Required(ErrorMessage = "字典名称不能为空")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "字典名称长度必须在2-100个字符之间")]
    public string DictName { get; set; } = string.Empty;

    /// <summary>
    /// 字典编码
    /// </summary>
    [Required(ErrorMessage = "字典编码不能为空")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "字典编码长度必须在2-100个字符之间")]
    public string DictCode { get; set; } = string.Empty;

    /// <summary>
    /// 字典分类
    /// </summary>
    public LeanDictTypeCategory DictCategory { get; set; }

    /// <summary>
    /// SQL语句
    /// </summary>
    public string? SqlStatement { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [StringLength(500, ErrorMessage = "备注长度不能超过500个字符")]
    public string? Remark { get; set; }
}

/// <summary>
/// 字典类型更新参数
/// </summary>
public class LeanUpdateDictTypeDto : LeanCreateDictTypeDto
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
public class LeanChangeDictTypeStatusDto
{
    /// <summary>
    /// 字典类型ID
    /// </summary>
    [Required(ErrorMessage = "字典类型ID不能为空")]
    public long Id { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    [Required(ErrorMessage = "状态不能为空")]
    public LeanStatus Status { get; set; }
}

/// <summary>
/// 字典类型DTO
/// </summary>
public class LeanDictTypeDto : LeanBaseEntity
{
    /// <summary>
    /// 字典名称
    /// </summary>
    public string DictName { get; set; } = string.Empty;

    /// <summary>
    /// 字典编码
    /// </summary>
    public string DictCode { get; set; } = string.Empty;

    /// <summary>
    /// 字典分类
    /// </summary>
    public LeanDictTypeCategory DictCategory { get; set; }

    /// <summary>
    /// SQL语句
    /// </summary>
    public string? SqlStatement { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public LeanStatus Status { get; set; }

    /// <summary>
    /// 是否内置
    /// </summary>
    public LeanBuiltinStatus IsBuiltin { get; set; }
}

/// <summary>
/// 字典类型导出对象
/// </summary>
public class LeanDictTypeExportDto
{
    /// <summary>
    /// 字典名称
    /// </summary>
    [LeanExcelColumn("字典名称", DataType = LeanExcelDataType.String)]
    public string DictName { get; set; } = default!;

    /// <summary>
    /// 字典编码
    /// </summary>
    [LeanExcelColumn("字典编码", DataType = LeanExcelDataType.String)]
    public string DictCode { get; set; } = default!;

    /// <summary>
    /// 字典分类
    /// </summary>
    [LeanExcelColumn("字典分类", DataType = LeanExcelDataType.Int)]
    public int DictCategory { get; set; }

    /// <summary>
    /// SQL语句
    /// </summary>
    [LeanExcelColumn("SQL语句", DataType = LeanExcelDataType.String)]
    public string? SqlStatement { get; set; }

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
    /// 是否内置
    /// </summary>
    [LeanExcelColumn("是否内置", DataType = LeanExcelDataType.Int)]
    public int IsBuiltin { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    [LeanExcelColumn("创建时间", DataType = LeanExcelDataType.DateTime, Format = "yyyy-MM-dd HH:mm:ss")]
    public DateTime CreateTime { get; set; }
}

/// <summary>
/// 字典类型导入对象
/// </summary>
public class LeanDictTypeImportDto
{
    /// <summary>
    /// 字典名称
    /// </summary>
    [LeanExcelColumn("字典名称", DataType = LeanExcelDataType.String)]
    public string DictName { get; set; } = default!;

    /// <summary>
    /// 字典编码
    /// </summary>
    [LeanExcelColumn("字典编码", DataType = LeanExcelDataType.String)]
    public string DictCode { get; set; } = default!;

    /// <summary>
    /// 字典分类
    /// </summary>
    [LeanExcelColumn("字典分类", DataType = LeanExcelDataType.Int)]
    public int DictCategory { get; set; }

    /// <summary>
    /// SQL语句
    /// </summary>
    [LeanExcelColumn("SQL语句", DataType = LeanExcelDataType.String)]
    public string? SqlStatement { get; set; }

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
/// 字典类型导入模板对象
/// </summary>
public class LeanDictTypeImportTemplateDto
{
    /// <summary>
    /// 字典名称
    /// </summary>
    [LeanExcelColumn("字典名称", DataType = LeanExcelDataType.String)]
    public string DictName { get; set; } = default!;

    /// <summary>
    /// 字典编码
    /// </summary>
    [LeanExcelColumn("字典编码", DataType = LeanExcelDataType.String)]
    public string DictCode { get; set; } = default!;

    /// <summary>
    /// 字典分类
    /// </summary>
    [LeanExcelColumn("字典分类", DataType = LeanExcelDataType.Int)]
    public int DictCategory { get; set; }

    /// <summary>
    /// SQL语句
    /// </summary>
    [LeanExcelColumn("SQL语句", DataType = LeanExcelDataType.String)]
    public string? SqlStatement { get; set; }

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