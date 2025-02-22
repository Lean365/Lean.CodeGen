using System.ComponentModel.DataAnnotations;
using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Domain.Entities;
using Lean.CodeGen.Common.Excel;

namespace Lean.CodeGen.Application.Dtos.Admin;

/// <summary>
/// 字典数据查询参数
/// </summary>
public class LeanQueryDictDataDto : LeanPage
{
    /// <summary>
    /// 字典类型ID
    /// </summary>
    public long? TypeId { get; set; }

    /// <summary>
    /// 字典标签
    /// </summary>
    public string? DictLabel { get; set; }

    /// <summary>
    /// 字典键值
    /// </summary>
    public string? DictValue { get; set; }

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
/// 字典数据创建参数
/// </summary>
public class LeanCreateDictDataDto
{
    /// <summary>
    /// 字典类型ID
    /// </summary>
    [Required(ErrorMessage = "字典类型ID不能为空")]
    public long TypeId { get; set; }

    /// <summary>
    /// 字典标签
    /// </summary>
    [Required(ErrorMessage = "字典标签不能为空")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "字典标签长度必须在2-100个字符之间")]
    public string DictLabel { get; set; } = string.Empty;

    /// <summary>
    /// 字典键值
    /// </summary>
    [Required(ErrorMessage = "字典键值不能为空")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "字典键值长度必须在1-100个字符之间")]
    public string DictValue { get; set; } = string.Empty;

    /// <summary>
    /// 扩展标签
    /// </summary>
    [StringLength(100, ErrorMessage = "扩展标签长度不能超过100个字符")]
    public string? ExtLabel { get; set; }

    /// <summary>
    /// 扩展键值
    /// </summary>
    [StringLength(100, ErrorMessage = "扩展键值长度不能超过100个字符")]
    public string? ExtValue { get; set; }

    /// <summary>
    /// 样式属性
    /// </summary>
    [StringLength(100, ErrorMessage = "样式属性长度不能超过100个字符")]
    public string? CssClass { get; set; }

    /// <summary>
    /// 排序号
    /// </summary>
    public int OrderNum { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [StringLength(500, ErrorMessage = "备注长度不能超过500个字符")]
    public string? Remark { get; set; }
}

/// <summary>
/// 字典数据更新参数
/// </summary>
public class LeanUpdateDictDataDto : LeanCreateDictDataDto
{
    /// <summary>
    /// 字典数据ID
    /// </summary>
    [Required(ErrorMessage = "字典数据ID不能为空")]
    public long Id { get; set; }
}

/// <summary>
/// 字典数据状态变更参数
/// </summary>
public class LeanChangeDictDataStatusDto
{
    /// <summary>
    /// 字典数据ID
    /// </summary>
    [Required(ErrorMessage = "字典数据ID不能为空")]
    public long Id { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    [Required(ErrorMessage = "状态不能为空")]
    public LeanStatus Status { get; set; }
}

/// <summary>
/// 字典数据DTO
/// </summary>
public class LeanDictDataDto : LeanBaseEntity
{
    /// <summary>
    /// 字典类型ID
    /// </summary>
    public long TypeId { get; set; }

    /// <summary>
    /// 翻译键
    /// </summary>
    public string? TransKey { get; set; }

    /// <summary>
    /// 字典标签
    /// </summary>
    public string DictLabel { get; set; } = string.Empty;

    /// <summary>
    /// 字典键值
    /// </summary>
    public string DictValue { get; set; } = string.Empty;

    /// <summary>
    /// 扩展标签
    /// </summary>
    public string? ExtLabel { get; set; }

    /// <summary>
    /// 扩展键值
    /// </summary>
    public string? ExtValue { get; set; }

    /// <summary>
    /// 样式属性
    /// </summary>
    public string? CssClass { get; set; }

    /// <summary>
    /// 排序号
    /// </summary>
    public int OrderNum { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public LeanStatus Status { get; set; }

    /// <summary>
    /// 是否内置
    /// </summary>
    public LeanBuiltinStatus IsBuiltin { get; set; }

    /// <summary>
    /// 字典类型名称
    /// </summary>
    public string TypeName { get; set; } = string.Empty;

    /// <summary>
    /// 字典类型编码
    /// </summary>
    public string TypeCode { get; set; } = string.Empty;
}

/// <summary>
/// 字典数据导出对象
/// </summary>
public class LeanDictDataExportDto
{
    /// <summary>
    /// 字典类型Id
    /// </summary>
    [LeanExcelColumn("字典类型Id", DataType = LeanExcelDataType.Long)]
    public long TypeId { get; set; }

    /// <summary>
    /// 翻译键
    /// </summary>
    [LeanExcelColumn("翻译键", DataType = LeanExcelDataType.String)]
    public string? TransKey { get; set; }

    /// <summary>
    /// 字典标签
    /// </summary>
    [LeanExcelColumn("字典标签", DataType = LeanExcelDataType.String)]
    public string DictLabel { get; set; } = default!;

    /// <summary>
    /// 字典键值
    /// </summary>
    [LeanExcelColumn("字典键值", DataType = LeanExcelDataType.String)]
    public string DictValue { get; set; } = default!;

    /// <summary>
    /// 扩展标签
    /// </summary>
    [LeanExcelColumn("扩展标签", DataType = LeanExcelDataType.String)]
    public string? ExtLabel { get; set; }

    /// <summary>
    /// 扩展键值
    /// </summary>
    [LeanExcelColumn("扩展键值", DataType = LeanExcelDataType.String)]
    public string? ExtValue { get; set; }

    /// <summary>
    /// 样式属性
    /// </summary>
    [LeanExcelColumn("样式属性", DataType = LeanExcelDataType.String)]
    public string? CssClass { get; set; }

    /// <summary>
    /// 排序号
    /// </summary>
    [LeanExcelColumn("排序号", DataType = LeanExcelDataType.Int)]
    public int OrderNum { get; set; }

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
/// 字典数据导入对象
/// </summary>
public class LeanDictDataImportDto
{
    /// <summary>
    /// 字典类型Id
    /// </summary>
    [LeanExcelColumn("字典类型Id", DataType = LeanExcelDataType.Long)]
    public long TypeId { get; set; }

    /// <summary>
    /// 翻译键
    /// </summary>
    [LeanExcelColumn("翻译键", DataType = LeanExcelDataType.String)]
    public string? TransKey { get; set; }

    /// <summary>
    /// 字典标签
    /// </summary>
    [LeanExcelColumn("字典标签", DataType = LeanExcelDataType.String)]
    public string DictLabel { get; set; } = default!;

    /// <summary>
    /// 字典键值
    /// </summary>
    [LeanExcelColumn("字典键值", DataType = LeanExcelDataType.String)]
    public string DictValue { get; set; } = default!;

    /// <summary>
    /// 扩展标签
    /// </summary>
    [LeanExcelColumn("扩展标签", DataType = LeanExcelDataType.String)]
    public string? ExtLabel { get; set; }

    /// <summary>
    /// 扩展键值
    /// </summary>
    [LeanExcelColumn("扩展键值", DataType = LeanExcelDataType.String)]
    public string? ExtValue { get; set; }

    /// <summary>
    /// 样式属性
    /// </summary>
    [LeanExcelColumn("样式属性", DataType = LeanExcelDataType.String)]
    public string? CssClass { get; set; }

    /// <summary>
    /// 排序号
    /// </summary>
    [LeanExcelColumn("排序号", DataType = LeanExcelDataType.Int)]
    public int OrderNum { get; set; }

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
/// 字典数据导入模板对象
/// </summary>
public class LeanDictDataImportTemplateDto
{
    /// <summary>
    /// 字典类型Id
    /// </summary>
    [LeanExcelColumn("字典类型Id", DataType = LeanExcelDataType.Long)]
    public long TypeId { get; set; }

    /// <summary>
    /// 翻译键
    /// </summary>
    [LeanExcelColumn("翻译键", DataType = LeanExcelDataType.String)]
    public string? TransKey { get; set; }

    /// <summary>
    /// 字典标签
    /// </summary>
    [LeanExcelColumn("字典标签", DataType = LeanExcelDataType.String)]
    public string DictLabel { get; set; } = default!;

    /// <summary>
    /// 字典键值
    /// </summary>
    [LeanExcelColumn("字典键值", DataType = LeanExcelDataType.String)]
    public string DictValue { get; set; } = default!;

    /// <summary>
    /// 扩展标签
    /// </summary>
    [LeanExcelColumn("扩展标签", DataType = LeanExcelDataType.String)]
    public string? ExtLabel { get; set; }

    /// <summary>
    /// 扩展键值
    /// </summary>
    [LeanExcelColumn("扩展键值", DataType = LeanExcelDataType.String)]
    public string? ExtValue { get; set; }

    /// <summary>
    /// 样式属性
    /// </summary>
    [LeanExcelColumn("样式属性", DataType = LeanExcelDataType.String)]
    public string? CssClass { get; set; }

    /// <summary>
    /// 排序号
    /// </summary>
    [LeanExcelColumn("排序号", DataType = LeanExcelDataType.Int)]
    public int OrderNum { get; set; }

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