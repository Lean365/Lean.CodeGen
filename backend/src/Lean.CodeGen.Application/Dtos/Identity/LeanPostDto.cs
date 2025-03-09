using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
  /// 0-正常
  /// 1-停用
  /// </summary>
  public int PostStatus { get; set; }

  /// <summary>
  /// 是否内置
  /// 0-否
  /// 1-是
  /// </summary>
  public int IsBuiltin { get; set; }
}

/// <summary>
/// 岗位查询参数
/// </summary>
public class LeanPostQueryDto : LeanPage
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
  /// 0-正常
  /// 1-停用
  /// </summary>
  public int? PostStatus { get; set; }

  /// <summary>
  /// 是否内置
  /// 0-否
  /// 1-是
  /// </summary>
  public int? IsBuiltin { get; set; }

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
public class LeanPostCreateDto
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
  /// 0-正常
  /// 1-停用
  /// </summary>
  public int PostStatus { get; set; } = 0;

  /// <summary>
  /// 是否内置
  /// 0-否
  /// 1-是
  /// </summary>
  public int IsBuiltin { get; set; } = 0;
}

/// <summary>
/// 岗位更新参数
/// </summary>
public class LeanPostUpdateDto
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
  /// 0-正常
  /// 1-停用
  /// </summary>
  public int PostStatus { get; set; }
}

/// <summary>
/// 岗位删除参数
/// </summary>
public class LeanPostDeleteDto
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
public class LeanPostChangeStatusDto
{
  /// <summary>
  /// 岗位ID
  /// </summary>
  [Required(ErrorMessage = "岗位ID不能为空")]
  public long Id { get; set; }

  /// <summary>
  /// 岗位状态
  /// 0-正常
  /// 1-停用
  /// </summary>
  [Required(ErrorMessage = "状态不能为空")]
  public int PostStatus { get; set; }
}

/// <summary>
/// 岗位导入参数
/// </summary>
public class LeanPostImportDto
{
  /// <summary>
  /// 岗位名称
  /// </summary>
  [LeanExcelColumn("岗位名称")]
  [Required(ErrorMessage = "岗位名称不能为空")]
  public string PostName { get; set; } = default!;

  /// <summary>
  /// 岗位编码
  /// </summary>
  [LeanExcelColumn("岗位编码")]
  [Required(ErrorMessage = "岗位编码不能为空")]
  public string PostCode { get; set; } = default!;

  /// <summary>
  /// 显示顺序
  /// </summary>
  [LeanExcelColumn("显示顺序")]
  public int OrderNum { get; set; }

  /// <summary>
  /// 备注
  /// </summary>
  [LeanExcelColumn("备注")]
  public string? Remark { get; set; }
}

/// <summary>
/// 岗位导入模板参数
/// </summary>
public class LeanPostImportTemplateDto
{
  /// <summary>
  /// 岗位名称
  /// </summary>
  [Required(ErrorMessage = "岗位名称不能为空")]
  [StringLength(50, MinimumLength = 2, ErrorMessage = "岗位名称长度必须在2-50个字符之间")]
  public string PostName { get; set; } = default!;

  /// <summary>
  /// 岗位编码
  /// </summary>
  [Required(ErrorMessage = "岗位编码不能为空")]
  [StringLength(50, MinimumLength = 2, ErrorMessage = "岗位编码长度必须在2-50个字符之间")]
  public string PostCode { get; set; } = default!;

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
/// 岗位导入错误参数
/// </summary>
public class LeanPostImportErrorDto : LeanImportError
{
  /// <summary>
  /// 岗位编码
  /// </summary>
  public string PostCode { get; set; } = default!;
}

/// <summary>
/// 岗位导入结果参数
/// </summary>
public class LeanPostImportResultDto : LeanImportResult
{
  /// <summary>
  /// 错误列表
  /// </summary>
  public new List<LeanPostImportErrorDto> Errors { get; set; } = new();

  /// <summary>
  /// 添加错误
  /// </summary>
  /// <param name="postCode">岗位编码</param>
  /// <param name="errorMessage">错误消息</param>
  public override void AddError(string postCode, string errorMessage)
  {
    Errors.Add(new LeanPostImportErrorDto
    {
      PostCode = postCode,
      ErrorMessage = errorMessage
    });
  }
}

/// <summary>
/// 岗位导出查询参数
/// </summary>
public class LeanPostExportQueryDto : LeanPostQueryDto
{
  /// <summary>
  /// 导出字段列表
  /// </summary>
  public List<string> ExportFields { get; set; } = new();

  /// <summary>
  /// 文件格式
  /// </summary>
  [Required(ErrorMessage = "文件格式不能为空")]
  public string FileType { get; set; } = default!;

  /// <summary>
  /// 是否导出全部
  /// 0-否
  /// 1-是
  /// </summary>
  public int IsExportAll { get; set; }

  /// <summary>
  /// 选中的ID列表
  /// </summary>
  public List<long> SelectedIds { get; set; } = new();
}

/// <summary>
/// 岗位导出参数
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
  /// 0-正常
  /// 1-停用
  /// </summary>
  [LeanExcelColumn("岗位状态", DataType = LeanExcelDataType.Int)]
  public int PostStatus { get; set; }

  /// <summary>
  /// 显示顺序
  /// </summary>
  [LeanExcelColumn("显示顺序", DataType = LeanExcelDataType.Int)]
  public int OrderNum { get; set; }

  /// <summary>
  /// 是否内置
  /// 0-否
  /// 1-是
  /// </summary>
  [LeanExcelColumn("是否内置", DataType = LeanExcelDataType.Int)]
  public int IsBuiltin { get; set; }

  /// <summary>
  /// 创建时间
  /// </summary>
  [LeanExcelColumn("创建时间", DataType = LeanExcelDataType.DateTime)]
  public DateTime CreateTime { get; set; }
}