using System.ComponentModel.DataAnnotations;
using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Domain.Entities;
using Lean.CodeGen.Common.Excel;

namespace Lean.CodeGen.Application.Dtos.Admin;

/// <summary>
/// 字典数据查询参数
/// </summary>
public class LeanDictDataQueryDto : LeanPage
{
  /// <summary>
  /// 字典类型ID
  /// </summary>
  /// <remarks>
  /// 关联的字典类型ID
  /// </remarks>
  public long? TypeId { get; set; }

  /// <summary>
  /// 字典标签
  /// </summary>
  /// <remarks>
  /// 字典数据的显示名称，如：男、女、正常、停用等
  /// </remarks>
  public string? DictDataLabel { get; set; }

  /// <summary>
  /// 字典键值
  /// </summary>
  /// <remarks>
  /// 字典数据的实际值，如：0、1等
  /// </remarks>
  public string? DictDataValue { get; set; }

  /// <summary>
  /// 状态
  /// </summary>
  /// <remarks>
  /// 字典数据状态：0-正常，1-停用
  /// </remarks>
  public int? DictDataStatus { get; set; }

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
public class LeanDictDataCreateDto
{
  /// <summary>
  /// 字典类型ID
  /// </summary>
  /// <remarks>
  /// 关联的字典类型ID
  /// </remarks>
  [Required(ErrorMessage = "字典类型ID不能为空")]
  public long TypeId { get; set; }

  /// <summary>
  /// 翻译键
  /// </summary>
  /// <remarks>
  /// 用于多语言翻译的键名
  /// </remarks>
  [StringLength(100, ErrorMessage = "翻译键长度不能超过100个字符")]
  public string? TransKey { get; set; }

  /// <summary>
  /// 字典标签
  /// </summary>
  /// <remarks>
  /// 字典数据的显示名称，如：男、女、正常、停用等
  /// </remarks>
  [Required(ErrorMessage = "字典标签不能为空")]
  [StringLength(100, MinimumLength = 2, ErrorMessage = "字典标签长度必须在2-100个字符之间")]
  public string DictDataLabel { get; set; } = string.Empty;

  /// <summary>
  /// 字典键值
  /// </summary>
  /// <remarks>
  /// 字典数据的实际值，如：0、1等
  /// </remarks>
  [Required(ErrorMessage = "字典键值不能为空")]
  [StringLength(100, MinimumLength = 1, ErrorMessage = "字典键值长度必须在1-100个字符之间")]
  public string DictDataValue { get; set; } = string.Empty;

  /// <summary>
  /// 扩展标签
  /// </summary>
  /// <remarks>
  /// 字典数据的扩展显示名称
  /// </remarks>
  [StringLength(100, ErrorMessage = "扩展标签长度不能超过100个字符")]
  public string? ExtDataLabel { get; set; }

  /// <summary>
  /// 扩展键值
  /// </summary>
  /// <remarks>
  /// 字典数据的扩展值
  /// </remarks>
  [StringLength(100, ErrorMessage = "扩展键值长度不能超过100个字符")]
  public string? ExtDataValue { get; set; }

  /// <summary>
  /// 样式属性
  /// </summary>
  /// <remarks>
  /// 用于前端显示的样式，如：primary、success、warning、danger等
  /// </remarks>
  [StringLength(100, ErrorMessage = "样式属性长度不能超过100个字符")]
  public string? CssClass { get; set; }

  /// <summary>
  /// 列表样式
  /// </summary>
  /// <remarks>
  /// 用于前端列表显示的样式，如：primary、success、warning、danger等
  /// </remarks>
  [StringLength(100, ErrorMessage = "列表样式长度不能超过100个字符")]
  public string? ListClass { get; set; }

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
/// 字典数据更新参数
/// </summary>
public class LeanDictDataUpdateDto : LeanDictDataCreateDto
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
public class LeanDictDataChangeStatusDto
{
  /// <summary>
  /// 字典数据ID
  /// </summary>
  [Required(ErrorMessage = "字典数据ID不能为空")]
  public long Id { get; set; }

  /// <summary>
  /// 状态
  /// </summary>
  /// <remarks>
  /// 字典数据状态：0-正常，1-停用
  /// </remarks>
  [Required(ErrorMessage = "状态不能为空")]
  public int DictDataStatus { get; set; }
}

/// <summary>
/// 字典数据DTO
/// </summary>
public class LeanDictDataDto : LeanBaseEntity
{
  /// <summary>
  /// 字典类型ID
  /// </summary>
  /// <remarks>
  /// 关联的字典类型ID
  /// </remarks>
  public long TypeId { get; set; }

  /// <summary>
  /// 翻译键
  /// </summary>
  /// <remarks>
  /// 用于多语言翻译的键名
  /// </remarks>
  public string? TransKey { get; set; }

  /// <summary>
  /// 字典标签
  /// </summary>
  /// <remarks>
  /// 字典数据的显示名称，如：男、女、正常、停用等
  /// </remarks>
  public string DictDataLabel { get; set; } = string.Empty;

  /// <summary>
  /// 字典键值
  /// </summary>
  /// <remarks>
  /// 字典数据的实际值，如：0、1等
  /// </remarks>
  public string DictDataValue { get; set; } = string.Empty;

  /// <summary>
  /// 扩展标签
  /// </summary>
  /// <remarks>
  /// 字典数据的扩展显示名称
  /// </remarks>
  public string? ExtDataLabel { get; set; }

  /// <summary>
  /// 扩展键值
  /// </summary>
  /// <remarks>
  /// 字典数据的扩展值
  /// </remarks>
  public string? ExtDataValue { get; set; }

  /// <summary>
  /// 样式属性
  /// </summary>
  /// <remarks>
  /// 用于前端显示的样式，如：primary、success、warning、danger等
  /// </remarks>
  public string? CssClass { get; set; }

  /// <summary>
  /// 列表样式
  /// </summary>
  /// <remarks>
  /// 用于前端列表显示的样式，如：primary、success、warning、danger等
  /// </remarks>
  public string? ListClass { get; set; }

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
  /// 字典数据状态：0-正常，1-停用
  /// </remarks>
  public int DictDataStatus { get; set; }

  /// <summary>
  /// 是否内置
  /// </summary>
  /// <remarks>
  /// 是否为系统内置数据：0-否，1-是
  /// </remarks>
  public int IsBuiltin { get; set; }

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
  /// 字典类型ID
  /// </summary>
  /// <remarks>
  /// 关联的字典类型ID
  /// </remarks>
  [LeanExcelColumn("字典类型Id", DataType = LeanExcelDataType.Long)]
  public long TypeId { get; set; }

  /// <summary>
  /// 翻译键
  /// </summary>
  /// <remarks>
  /// 用于多语言翻译的键名
  /// </remarks>
  [LeanExcelColumn("翻译键", DataType = LeanExcelDataType.String)]
  public string? TransKey { get; set; }

  /// <summary>
  /// 字典标签
  /// </summary>
  /// <remarks>
  /// 字典数据的显示名称，如：男、女、正常、停用等
  /// </remarks>
  [LeanExcelColumn("字典标签", DataType = LeanExcelDataType.String)]
  public string DictDataLabel { get; set; } = default!;

  /// <summary>
  /// 字典键值
  /// </summary>
  /// <remarks>
  /// 字典数据的实际值，如：0、1等
  /// </remarks>
  [LeanExcelColumn("字典键值", DataType = LeanExcelDataType.String)]
  public string DictDataValue { get; set; } = default!;

  /// <summary>
  /// 扩展标签
  /// </summary>
  /// <remarks>
  /// 字典数据的扩展显示名称
  /// </remarks>
  [LeanExcelColumn("扩展标签", DataType = LeanExcelDataType.String)]
  public string? ExtDataLabel { get; set; }

  /// <summary>
  /// 扩展键值
  /// </summary>
  /// <remarks>
  /// 字典数据的扩展值
  /// </remarks>
  [LeanExcelColumn("扩展键值", DataType = LeanExcelDataType.String)]
  public string? ExtDataValue { get; set; }

  /// <summary>
  /// 样式属性
  /// </summary>
  /// <remarks>
  /// 用于前端显示的样式，如：primary、success、warning、danger等
  /// </remarks>
  [LeanExcelColumn("样式属性", DataType = LeanExcelDataType.String)]
  public string? CssClass { get; set; }

  /// <summary>
  /// 列表样式
  /// </summary>
  /// <remarks>
  /// 用于前端列表显示的样式，如：primary、success、warning、danger等
  /// </remarks>
  [LeanExcelColumn("列表样式", DataType = LeanExcelDataType.String)]
  public string? ListClass { get; set; }

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
  /// 字典数据状态：0-正常，1-停用
  /// </remarks>
  [LeanExcelColumn("状态", DataType = LeanExcelDataType.Int)]
  public int DictDataStatus { get; set; }

  /// <summary>
  /// 是否内置
  /// </summary>
  /// <remarks>
  /// 是否为系统内置数据：0-否，1-是
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
/// 字典数据导入对象
/// </summary>
public class LeanDictDataImportDto
{
  /// <summary>
  /// 字典类型ID
  /// </summary>
  /// <remarks>
  /// 关联的字典类型ID
  /// </remarks>
  [Required(ErrorMessage = "字典类型ID不能为空")]
  [LeanExcelColumn("字典类型Id", DataType = LeanExcelDataType.Long)]
  public long TypeId { get; set; }

  /// <summary>
  /// 翻译键
  /// </summary>
  /// <remarks>
  /// 用于多语言翻译的键名
  /// </remarks>
  [StringLength(100, ErrorMessage = "翻译键长度不能超过100个字符")]
  [LeanExcelColumn("翻译键", DataType = LeanExcelDataType.String)]
  public string? TransKey { get; set; }

  /// <summary>
  /// 字典标签
  /// </summary>
  /// <remarks>
  /// 字典数据的显示名称，如：男、女、正常、停用等
  /// </remarks>
  [Required(ErrorMessage = "字典标签不能为空")]
  [StringLength(100, MinimumLength = 2, ErrorMessage = "字典标签长度必须在2-100个字符之间")]
  [LeanExcelColumn("字典标签", DataType = LeanExcelDataType.String)]
  public string DictDataLabel { get; set; } = default!;

  /// <summary>
  /// 字典键值
  /// </summary>
  /// <remarks>
  /// 字典数据的实际值，如：0、1等
  /// </remarks>
  [Required(ErrorMessage = "字典键值不能为空")]
  [StringLength(100, MinimumLength = 1, ErrorMessage = "字典键值长度必须在1-100个字符之间")]
  [LeanExcelColumn("字典键值", DataType = LeanExcelDataType.String)]
  public string DictDataValue { get; set; } = default!;

  /// <summary>
  /// 扩展标签
  /// </summary>
  /// <remarks>
  /// 字典数据的扩展显示名称
  /// </remarks>
  [StringLength(100, ErrorMessage = "扩展标签长度不能超过100个字符")]
  [LeanExcelColumn("扩展标签", DataType = LeanExcelDataType.String)]
  public string? ExtDataLabel { get; set; }

  /// <summary>
  /// 扩展键值
  /// </summary>
  /// <remarks>
  /// 字典数据的扩展值
  /// </remarks>
  [StringLength(100, ErrorMessage = "扩展键值长度不能超过100个字符")]
  [LeanExcelColumn("扩展键值", DataType = LeanExcelDataType.String)]
  public string? ExtDataValue { get; set; }

  /// <summary>
  /// 样式属性
  /// </summary>
  /// <remarks>
  /// 用于前端显示的样式，如：primary、success、warning、danger等
  /// </remarks>
  [StringLength(100, ErrorMessage = "样式属性长度不能超过100个字符")]
  [LeanExcelColumn("样式属性", DataType = LeanExcelDataType.String)]
  public string? CssClass { get; set; }

  /// <summary>
  /// 列表样式
  /// </summary>
  /// <remarks>
  /// 用于前端列表显示的样式，如：primary、success、warning、danger等
  /// </remarks>
  [StringLength(100, ErrorMessage = "列表样式长度不能超过100个字符")]
  [LeanExcelColumn("列表样式", DataType = LeanExcelDataType.String)]
  public string? ListClass { get; set; }

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
  /// 字典数据状态：0-正常，1-停用
  /// </remarks>
  [LeanExcelColumn("状态", DataType = LeanExcelDataType.Int)]
  public int DictDataStatus { get; set; }

  /// <summary>
  /// 备注
  /// </summary>
  [StringLength(500, ErrorMessage = "备注长度不能超过500个字符")]
  [LeanExcelColumn("备注", DataType = LeanExcelDataType.String)]
  public string? Remark { get; set; }
}

/// <summary>
/// 字典数据导入模板对象
/// </summary>
public class LeanDictDataImportTemplateDto
{
  /// <summary>
  /// 字典类型ID
  /// </summary>
  /// <remarks>
  /// 关联的字典类型ID
  /// </remarks>
  [LeanExcelColumn("字典类型Id", DataType = LeanExcelDataType.Long)]
  public long TypeId { get; set; }

  /// <summary>
  /// 翻译键
  /// </summary>
  /// <remarks>
  /// 用于多语言翻译的键名
  /// </remarks>
  [LeanExcelColumn("翻译键", DataType = LeanExcelDataType.String)]
  public string? TransKey { get; set; }

  /// <summary>
  /// 字典标签
  /// </summary>
  /// <remarks>
  /// 字典数据的显示名称，如：男、女、正常、停用等
  /// </remarks>
  [LeanExcelColumn("字典标签", DataType = LeanExcelDataType.String)]
  public string DictDataLabel { get; set; } = default!;

  /// <summary>
  /// 字典键值
  /// </summary>
  /// <remarks>
  /// 字典数据的实际值，如：0、1等
  /// </remarks>
  [LeanExcelColumn("字典键值", DataType = LeanExcelDataType.String)]
  public string DictDataValue { get; set; } = default!;

  /// <summary>
  /// 扩展标签
  /// </summary>
  /// <remarks>
  /// 字典数据的扩展显示名称
  /// </remarks>
  [LeanExcelColumn("扩展标签", DataType = LeanExcelDataType.String)]
  public string? ExtDataLabel { get; set; }

  /// <summary>
  /// 扩展键值
  /// </summary>
  /// <remarks>
  /// 字典数据的扩展值
  /// </remarks>
  [LeanExcelColumn("扩展键值", DataType = LeanExcelDataType.String)]
  public string? ExtDataValue { get; set; }

  /// <summary>
  /// 样式属性
  /// </summary>
  /// <remarks>
  /// 用于前端显示的样式，如：primary、success、warning、danger等
  /// </remarks>
  [LeanExcelColumn("样式属性", DataType = LeanExcelDataType.String)]
  public string? CssClass { get; set; }

  /// <summary>
  /// 列表样式
  /// </summary>
  /// <remarks>
  /// 用于前端列表显示的样式，如：primary、success、warning、danger等
  /// </remarks>
  [LeanExcelColumn("列表样式", DataType = LeanExcelDataType.String)]
  public string? ListClass { get; set; }

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
  /// 字典数据状态：0-正常，1-停用
  /// </remarks>
  [LeanExcelColumn("状态", DataType = LeanExcelDataType.Int)]
  public int DictDataStatus { get; set; }

  /// <summary>
  /// 备注
  /// </summary>
  [LeanExcelColumn("备注", DataType = LeanExcelDataType.String)]
  public string? Remark { get; set; }
}