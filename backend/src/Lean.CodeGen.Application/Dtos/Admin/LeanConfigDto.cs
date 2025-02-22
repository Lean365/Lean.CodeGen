using System.ComponentModel.DataAnnotations;
using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Domain.Entities;
using Lean.CodeGen.Common.Excel;

namespace Lean.CodeGen.Application.Dtos.Admin;

/// <summary>
/// 系统配置查询参数
/// </summary>
public class LeanQueryConfigDto : LeanPage
{
    /// <summary>
    /// 配置名称
    /// </summary>
    public string? ConfigName { get; set; }

    /// <summary>
    /// 配置键
    /// </summary>
    public string? ConfigKey { get; set; }

    /// <summary>
    /// 配置类型
    /// </summary>
    public LeanConfigType? ConfigType { get; set; }

    /// <summary>
    /// 配置分组
    /// </summary>
    public string? ConfigGroup { get; set; }

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
/// 系统配置创建参数
/// </summary>
public class LeanCreateConfigDto
{
    /// <summary>
    /// 配置名称
    /// </summary>
    [Required(ErrorMessage = "配置名称不能为空")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "配置名称长度必须在2-100个字符之间")]
    public string ConfigName { get; set; } = string.Empty;

    /// <summary>
    /// 配置键
    /// </summary>
    [Required(ErrorMessage = "配置键不能为空")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "配置键长度必须在2-100个字符之间")]
    public string ConfigKey { get; set; } = string.Empty;

    /// <summary>
    /// 配置值
    /// </summary>
    [Required(ErrorMessage = "配置值不能为空")]
    [StringLength(4000, ErrorMessage = "配置值长度不能超过4000个字符")]
    public string ConfigValue { get; set; } = string.Empty;

    /// <summary>
    /// 配置类型
    /// </summary>
    [Required(ErrorMessage = "配置类型不能为空")]
    public LeanConfigType ConfigType { get; set; }

    /// <summary>
    /// 配置分组
    /// </summary>
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
    public int OrderNum { get; set; }
}

/// <summary>
/// 系统配置更新参数
/// </summary>
public class LeanUpdateConfigDto : LeanCreateConfigDto
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
public class LeanChangeConfigStatusDto
{
    /// <summary>
    /// 主键
    /// </summary>
    [Required(ErrorMessage = "配置ID不能为空")]
    public long Id { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    [Required(ErrorMessage = "状态不能为空")]
    public LeanStatus Status { get; set; }
}

/// <summary>
/// 系统配置DTO
/// </summary>
public class LeanConfigDto : LeanBaseEntity
{
    /// <summary>
    /// 配置名称
    /// </summary>
    public string ConfigName { get; set; } = string.Empty;

    /// <summary>
    /// 配置键
    /// </summary>
    public string ConfigKey { get; set; } = string.Empty;

    /// <summary>
    /// 配置值
    /// </summary>
    public string ConfigValue { get; set; } = string.Empty;

    /// <summary>
    /// 配置类型
    /// </summary>
    public LeanConfigType ConfigType { get; set; }

    /// <summary>
    /// 系统内置
    /// </summary>
    public LeanBuiltinStatus IsBuiltin { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public LeanStatus Status { get; set; }

    /// <summary>
    /// 配置分组
    /// </summary>
    public string? ConfigGroup { get; set; }

    /// <summary>
    /// 排序号
    /// </summary>
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
    [LeanExcelColumn("配置名称", DataType = LeanExcelDataType.String)]
    public string ConfigName { get; set; } = default!;

    /// <summary>
    /// 配置键
    /// </summary>
    [LeanExcelColumn("配置键", DataType = LeanExcelDataType.String)]
    public string ConfigKey { get; set; } = default!;

    /// <summary>
    /// 配置值
    /// </summary>
    [LeanExcelColumn("配置值", DataType = LeanExcelDataType.String)]
    public string ConfigValue { get; set; } = default!;

    /// <summary>
    /// 配置类型
    /// </summary>
    [LeanExcelColumn("配置类型", DataType = LeanExcelDataType.Int)]
    public int ConfigType { get; set; }

    /// <summary>
    /// 系统内置
    /// </summary>
    [LeanExcelColumn("系统内置", DataType = LeanExcelDataType.Int)]
    public int IsBuiltin { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    [LeanExcelColumn("状态", DataType = LeanExcelDataType.Int)]
    public int Status { get; set; }

    /// <summary>
    /// 配置分组
    /// </summary>
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
    [LeanExcelColumn("排序号", DataType = LeanExcelDataType.Int)]
    public int OrderNum { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    [LeanExcelColumn("创建时间", DataType = LeanExcelDataType.DateTime, Format = "yyyy-MM-dd HH:mm:ss")]
    public DateTime CreateTime { get; set; }
}

/// <summary>
/// 系统配置导入对象
/// </summary>
public class LeanConfigImportDto
{
    /// <summary>
    /// 配置名称
    /// </summary>
    [Required(ErrorMessage = "配置名称不能为空")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "配置名称长度必须在2-100个字符之间")]
    [LeanExcelColumn("配置名称", DataType = LeanExcelDataType.String)]
    public string ConfigName { get; set; } = default!;

    /// <summary>
    /// 配置键
    /// </summary>
    [Required(ErrorMessage = "配置键不能为空")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "配置键长度必须在2-100个字符之间")]
    [LeanExcelColumn("配置键", DataType = LeanExcelDataType.String)]
    public string ConfigKey { get; set; } = default!;

    /// <summary>
    /// 配置值
    /// </summary>
    [Required(ErrorMessage = "配置值不能为空")]
    [StringLength(4000, ErrorMessage = "配置值长度不能超过4000个字符")]
    [LeanExcelColumn("配置值", DataType = LeanExcelDataType.String)]
    public string ConfigValue { get; set; } = default!;

    /// <summary>
    /// 配置类型
    /// </summary>
    [Required(ErrorMessage = "配置类型不能为空")]
    [LeanExcelColumn("配置类型", DataType = LeanExcelDataType.Int)]
    public int ConfigType { get; set; } = (int)LeanConfigType.String;

    /// <summary>
    /// 配置分组
    /// </summary>
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
    [LeanExcelColumn("配置名称", DataType = LeanExcelDataType.String)]
    public string ConfigName { get; set; } = "系统配置";

    /// <summary>
    /// 配置键
    /// </summary>
    [LeanExcelColumn("配置键", DataType = LeanExcelDataType.String)]
    public string ConfigKey { get; set; } = "sys.config.key";

    /// <summary>
    /// 配置值
    /// </summary>
    [LeanExcelColumn("配置值", DataType = LeanExcelDataType.String)]
    public string ConfigValue { get; set; } = "配置值";

    /// <summary>
    /// 配置类型
    /// </summary>
    [LeanExcelColumn("配置类型", DataType = LeanExcelDataType.Int)]
    public int ConfigType { get; set; } = (int)LeanConfigType.String;

    /// <summary>
    /// 配置分组
    /// </summary>
    [LeanExcelColumn("配置分组", DataType = LeanExcelDataType.String)]
    public string? ConfigGroup { get; set; } = "系统管理";

    /// <summary>
    /// 备注
    /// </summary>
    [LeanExcelColumn("备注", DataType = LeanExcelDataType.String)]
    public string? Remark { get; set; } = "示例配置";

    /// <summary>
    /// 排序号
    /// </summary>
    [LeanExcelColumn("排序号", DataType = LeanExcelDataType.Int)]
    public int OrderNum { get; set; } = 1;
}