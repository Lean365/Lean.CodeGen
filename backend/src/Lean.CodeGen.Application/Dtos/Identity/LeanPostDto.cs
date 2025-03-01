using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Common.Excel;
using Lean.CodeGen.Common.Models;

namespace Lean.CodeGen.Application.Dtos.Identity;

/// <summary>
/// 岗位基础信息
/// </summary>
public class LeanPostDto : LeanBaseDto
{
  /// <summary>
  /// 岗位名称
  /// </summary>
  public string PostName { get; set; } = string.Empty;

  /// <summary>
  /// 岗位编码
  /// </summary>
  public string PostCode { get; set; } = string.Empty;

  /// <summary>
  /// 岗位描述
  /// </summary>
  public string? PostDescription { get; set; }

  /// <summary>
  /// 排序号
  /// </summary>
  public int OrderNum { get; set; }

  /// <summary>
  /// 岗位状态
  /// </summary>
  public LeanPostStatus PostStatus { get; set; }

  /// <summary>
  /// 是否内置
  /// </summary>
  public LeanBuiltinStatus IsBuiltin { get; set; }
}

/// <summary>
/// 岗位查询参数
/// </summary>
public class LeanQueryPostDto : LeanPage
{
  /// <summary>
  /// 岗位名称
  /// </summary>
  public string? PostName { get; set; }

  /// <summary>
  /// 岗位编码
  /// </summary>
  public string? PostCode { get; set; }

  /// <summary>
  /// 岗位状态
  /// </summary>
  public LeanPostStatus? PostStatus { get; set; }

  /// <summary>
  /// 是否内置
  /// </summary>
  public LeanBuiltinStatus? IsBuiltin { get; set; }

  /// <summary>
  /// 创建时间范围-开始
  /// </summary>
  public DateTime? StartTime { get; set; }

  /// <summary>
  /// 创建时间范围-结束
  /// </summary>
  public DateTime? EndTime { get; set; }
}

/// <summary>
/// 岗位创建参数
/// </summary>
public class LeanCreatePostDto
{
  /// <summary>
  /// 岗位名称
  /// </summary>
  [Required(ErrorMessage = "岗位名称不能为空")]
  [StringLength(50, MinimumLength = 2, ErrorMessage = "岗位名称长度必须在2-50个字符之间")]
  public string PostName { get; set; } = string.Empty;

  /// <summary>
  /// 岗位编码
  /// </summary>
  [Required(ErrorMessage = "岗位编码不能为空")]
  [StringLength(50, MinimumLength = 2, ErrorMessage = "岗位编码长度必须在2-50个字符之间")]
  public string PostCode { get; set; } = string.Empty;

  /// <summary>
  /// 岗位描述
  /// </summary>
  [StringLength(500, ErrorMessage = "岗位描述长度不能超过500个字符")]
  public string? PostDescription { get; set; }

  /// <summary>
  /// 排序号
  /// </summary>
  public int OrderNum { get; set; }

  /// <summary>
  /// 岗位状态
  /// </summary>
  public LeanPostStatus PostStatus { get; set; } = LeanPostStatus.Normal;

  /// <summary>
  /// 是否内置
  /// </summary>
  public LeanBuiltinStatus IsBuiltin { get; set; } = LeanBuiltinStatus.No;
}

/// <summary>
/// 岗位更新参数
/// </summary>
public class LeanUpdatePostDto
{
  /// <summary>
  /// 岗位ID
  /// </summary>
  [Required(ErrorMessage = "岗位ID不能为空")]
  public long Id { get; set; }

  /// <summary>
  /// 岗位名称
  /// </summary>
  [Required(ErrorMessage = "岗位名称不能为空")]
  [StringLength(50, MinimumLength = 2, ErrorMessage = "岗位名称长度必须在2-50个字符之间")]
  public string PostName { get; set; } = string.Empty;

  /// <summary>
  /// 岗位描述
  /// </summary>
  [StringLength(500, ErrorMessage = "岗位描述长度不能超过500个字符")]
  public string? PostDescription { get; set; }

  /// <summary>
  /// 排序号
  /// </summary>
  public int OrderNum { get; set; }

  /// <summary>
  /// 岗位状态
  /// </summary>
  public LeanPostStatus PostStatus { get; set; }
}

/// <summary>
/// 岗位删除参数
/// </summary>
public class LeanDeletePostDto
{
  /// <summary>
  /// 岗位ID
  /// </summary>
  [Required(ErrorMessage = "岗位ID不能为空")]
  public long Id { get; set; }
}

/// <summary>
/// 岗位状态变更参数
/// </summary>
public class LeanChangePostStatusDto
{
  /// <summary>
  /// 岗位ID
  /// </summary>
  [Required(ErrorMessage = "岗位ID不能为空")]
  public long Id { get; set; }

  /// <summary>
  /// 岗位状态
  /// </summary>
  [Required(ErrorMessage = "岗位状态不能为空")]
  public LeanPostStatus PostStatus { get; set; }
}

/// <summary>
/// 岗位导入DTO
/// </summary>
public class LeanPostImportDto
{
  /// <summary>
  /// 岗位名称
  /// </summary>
  [LeanExcelColumn("岗位名称")]
  [Required]
  public string PostName { get; set; }

  /// <summary>
  /// 岗位编码
  /// </summary>
  [LeanExcelColumn("岗位编码")]
  [Required]
  public string PostCode { get; set; }

  /// <summary>
  /// 显示顺序
  /// </summary>
  [LeanExcelColumn("显示顺序")]
  public int OrderNum { get; set; }

  /// <summary>
  /// 备注
  /// </summary>
  [LeanExcelColumn("备注")]
  public string Remark { get; set; }
}

/// <summary>
/// 岗位导入模板
/// </summary>
public class LeanImportTemplatePostDto
{
  /// <summary>
  /// 岗位名称
  /// </summary>
  [Required(ErrorMessage = "岗位名称不能为空")]
  [StringLength(50, MinimumLength = 2, ErrorMessage = "岗位名称长度必须在2-50个字符之间")]
  public string PostName { get; set; } = string.Empty;

  /// <summary>
  /// 岗位编码
  /// </summary>
  [Required(ErrorMessage = "岗位编码不能为空")]
  [StringLength(50, MinimumLength = 2, ErrorMessage = "岗位编码长度必须在2-50个字符之间")]
  public string PostCode { get; set; } = string.Empty;

  /// <summary>
  /// 岗位描述
  /// </summary>
  [StringLength(500, ErrorMessage = "岗位描述长度不能超过500个字符")]
  public string? PostDescription { get; set; }

  /// <summary>
  /// 排序号
  /// </summary>
  public int OrderNum { get; set; }
}

/// <summary>
/// 岗位导入错误信息
/// </summary>
public class LeanImportPostErrorDto : LeanImportError
{
  /// <summary>
  /// 岗位编码
  /// </summary>
  public string PostCode
  {
    get => Key;
    set => Key = value;
  }
}

/// <summary>
/// 岗位导入结果
/// </summary>
public class LeanImportPostResultDto : LeanImportResult
{
  /// <summary>
  /// 错误信息列表
  /// </summary>
  public new List<LeanImportPostErrorDto> Errors { get; set; } = new();

  /// <summary>
  /// 添加错误信息
  /// </summary>
  public override void AddError(string postCode, string errorMessage)
  {
    base.AddError(postCode, errorMessage);
    Errors.Add(new LeanImportPostErrorDto
    {
      RowIndex = TotalCount,
      PostCode = postCode,
      ErrorMessage = errorMessage
    });
  }
}

/// <summary>
/// 岗位导出参数
/// </summary>
public class LeanExportPostDto : LeanQueryPostDto
{
  /// <summary>
  /// 导出字段列表
  /// </summary>
  public List<string> ExportFields { get; set; } = new();

  /// <summary>
  /// 文件格式
  /// </summary>
  [Required(ErrorMessage = "文件格式不能为空")]
  public string FileFormat { get; set; } = "xlsx";

  /// <summary>
  /// 是否导出全部
  /// </summary>
  public bool IsExportAll { get; set; }

  /// <summary>
  /// 选中的ID列表
  /// </summary>
  public List<long> SelectedIds { get; set; } = new();
}

/// <summary>
/// 岗位导出数据
/// </summary>
public class LeanPostExportDto
{
  /// <summary>
  /// 岗位名称
  /// </summary>
  [LeanExcelColumn("岗位名称", DataType = LeanExcelDataType.String)]
  public string PostName { get; set; } = default!;

  /// <summary>
  /// 岗位编码
  /// </summary>
  [LeanExcelColumn("岗位编码", DataType = LeanExcelDataType.String)]
  public string PostCode { get; set; } = default!;

  /// <summary>
  /// 岗位状态
  /// </summary>
  [LeanExcelColumn("岗位状态", DataType = LeanExcelDataType.Int)]
  public LeanPostStatus PostStatus { get; set; }

  /// <summary>
  /// 显示顺序
  /// </summary>
  [LeanExcelColumn("显示顺序", DataType = LeanExcelDataType.Int)]
  public int OrderNum { get; set; }

  /// <summary>
  /// 是否内置
  /// </summary>
  [LeanExcelColumn("是否内置", DataType = LeanExcelDataType.Int)]
  public LeanBuiltinStatus IsBuiltin { get; set; }

  /// <summary>
  /// 创建时间
  /// </summary>
  [LeanExcelColumn("创建时间", DataType = LeanExcelDataType.DateTime)]
  public DateTime CreateTime { get; set; }
}