// -----------------------------------------------------------------------
// <copyright file="LeanMailTmplDto.cs" company="Lean">
// Copyright (c) Lean. All rights reserved.
// </copyright>
// <author>Lean</author>
// <created>2024-03-09</created>
// <summary>邮件模板数据传输对象</summary>
// -----------------------------------------------------------------------

using Lean.CodeGen.Common.Excel;
using System.ComponentModel.DataAnnotations;

namespace Lean.CodeGen.Application.Dtos.Routine;

#region 查询
/// <summary>
/// 邮件模板查询对象
/// </summary>
public class LeanMailTmplQueryDto : LeanPage
{
  /// <summary>
  /// 模板编码
  /// </summary>
  public string? TmplCode { get; set; }

  /// <summary>
  /// 模板名称
  /// </summary>
  public string? TmplName { get; set; }

  /// <summary>
  /// 状态
  /// </summary>
  public int? TmplStatus { get; set; }
}
#endregion

#region 详情
/// <summary>
/// 邮件模板详情对象
/// </summary>
public class LeanMailTmplDto : LeanBaseDto
{
  /// <summary>
  /// 模板编码
  /// </summary>
  public string TmplCode { get; set; } = string.Empty;

  /// <summary>
  /// 模板名称
  /// </summary>
  public string TmplName { get; set; } = string.Empty;

  /// <summary>
  /// 模板主题
  /// </summary>
  public string TmplSubject { get; set; } = string.Empty;

  /// <summary>
  /// 模板内容
  /// </summary>
  public string TmplContent { get; set; } = string.Empty;

  /// <summary>
  /// 模板签名
  /// </summary>
  /// <remarks>
  /// 邮件模板的签名部分，可以包含发件人姓名、职位、联系方式等信息
  /// </remarks>
  public string TmplSignature { get; set; } = string.Empty;

  /// <summary>
  /// 是否HTML格式
  /// </summary>
  public int TmplIsHtml { get; set; } = 1;

  /// <summary>
  /// 优先级
  /// </summary>
  public int TmplPriority { get; set; } = 2;

  /// <summary>
  /// 备注
  /// </summary>
  public string? TmplRemark { get; set; }

  /// <summary>
  /// 状态
  /// </summary>
  public int TmplStatus { get; set; } = 1;
}
#endregion

#region 创建
/// <summary>
/// 邮件模板创建对象
/// </summary>
public class LeanMailTmplCreateDto
{
  /// <summary>
  /// 模板编码
  /// </summary>
  [Required(ErrorMessage = "模板编码不能为空")]
  [StringLength(100, MinimumLength = 2, ErrorMessage = "模板编码长度必须在2-100个字符之间")]
  public string TmplCode { get; set; } = string.Empty;

  /// <summary>
  /// 模板名称
  /// </summary>
  [Required(ErrorMessage = "模板名称不能为空")]
  [StringLength(200, MinimumLength = 2, ErrorMessage = "模板名称长度必须在2-200个字符之间")]
  public string TmplName { get; set; } = string.Empty;

  /// <summary>
  /// 模板主题
  /// </summary>
  [Required(ErrorMessage = "模板主题不能为空")]
  [StringLength(500, MinimumLength = 2, ErrorMessage = "模板主题长度必须在2-500个字符之间")]
  public string TmplSubject { get; set; } = string.Empty;

  /// <summary>
  /// 模板内容
  /// </summary>
  [Required(ErrorMessage = "模板内容不能为空")]
  [StringLength(4000, MinimumLength = 2, ErrorMessage = "模板内容长度必须在2-4000个字符之间")]
  public string TmplContent { get; set; } = string.Empty;

  /// <summary>
  /// 模板签名
  /// </summary>
  /// <remarks>
  /// 邮件模板的签名部分，可以包含发件人姓名、职位、联系方式等信息
  /// </remarks>
  [Required(ErrorMessage = "模板签名不能为空")]
  [StringLength(1000, MinimumLength = 2, ErrorMessage = "模板签名长度必须在2-1000个字符之间")]
  public string TmplSignature { get; set; } = string.Empty;

  /// <summary>
  /// 是否HTML格式
  /// </summary>
  public int TmplIsHtml { get; set; } = 1;

  /// <summary>
  /// 优先级
  /// </summary>
  public int TmplPriority { get; set; } = 2;

  /// <summary>
  /// 备注
  /// </summary>
  [StringLength(500, ErrorMessage = "备注长度不能超过500个字符")]
  public string? TmplRemark { get; set; }

  /// <summary>
  /// 状态
  /// </summary>
  public int TmplStatus { get; set; } = 1;
}
#endregion

#region 更新
/// <summary>
/// 邮件模板更新对象
/// </summary>
public class LeanMailTmplUpdateDto : LeanMailTmplCreateDto
{
  /// <summary>
  /// 主键
  /// </summary>
  public long Id { get; set; }
}
#endregion

#region 导入
/// <summary>
/// 邮件模板导入对象
/// </summary>
public class LeanMailTmplImportDto
{
  /// <summary>
  /// 模板编码
  /// </summary>
  [LeanExcelColumn("模板编码")]
  public string TmplCode { get; set; } = string.Empty;

  /// <summary>
  /// 模板名称
  /// </summary>
  [LeanExcelColumn("模板名称")]
  public string TmplName { get; set; } = string.Empty;

  /// <summary>
  /// 模板主题
  /// </summary>
  [LeanExcelColumn("模板主题")]
  public string TmplSubject { get; set; } = string.Empty;

  /// <summary>
  /// 模板内容
  /// </summary>
  [LeanExcelColumn("模板内容")]
  public string TmplContent { get; set; } = string.Empty;

  /// <summary>
  /// 模板签名
  /// </summary>
  [LeanExcelColumn("模板签名")]
  public string TmplSignature { get; set; } = string.Empty;

  /// <summary>
  /// 是否HTML格式
  /// </summary>
  [LeanExcelColumn("是否HTML格式")]
  public int TmplIsHtml { get; set; } = 1;

  /// <summary>
  /// 优先级
  /// </summary>
  [LeanExcelColumn("优先级")]
  public int TmplPriority { get; set; } = 2;

  /// <summary>
  /// 备注
  /// </summary>
  [LeanExcelColumn("备注")]
  public string? TmplRemark { get; set; }

  /// <summary>
  /// 状态
  /// </summary>
  [LeanExcelColumn("状态")]
  public int TmplStatus { get; set; } = 1;
}
#endregion

#region 导出
/// <summary>
/// 邮件模板导出对象
/// </summary>
public class LeanMailTmplExportDto
{
  /// <summary>
  /// 模板编码
  /// </summary>
  [LeanExcelColumn("模板编码")]
  public string TmplCode { get; set; } = string.Empty;

  /// <summary>
  /// 模板名称
  /// </summary>
  [LeanExcelColumn("模板名称")]
  public string TmplName { get; set; } = string.Empty;

  /// <summary>
  /// 模板主题
  /// </summary>
  [LeanExcelColumn("模板主题")]
  public string TmplSubject { get; set; } = string.Empty;

  /// <summary>
  /// 模板内容
  /// </summary>
  [LeanExcelColumn("模板内容")]
  public string TmplContent { get; set; } = string.Empty;

  /// <summary>
  /// 模板签名
  /// </summary>
  [LeanExcelColumn("模板签名")]
  public string TmplSignature { get; set; } = string.Empty;

  /// <summary>
  /// 是否HTML格式
  /// </summary>
  [LeanExcelColumn("是否HTML格式")]
  public int TmplIsHtml { get; set; } = 1;

  /// <summary>
  /// 优先级
  /// </summary>
  [LeanExcelColumn("优先级")]
  public int TmplPriority { get; set; } = 2;

  /// <summary>
  /// 备注
  /// </summary>
  [LeanExcelColumn("备注")]
  public string? TmplRemark { get; set; }

  /// <summary>
  /// 状态
  /// </summary>
  [LeanExcelColumn("状态")]
  public int TmplStatus { get; set; } = 1;

  /// <summary>
  /// 创建时间
  /// </summary>
  [LeanExcelColumn("创建时间")]
  public DateTime CreateTime { get; set; }
}
#endregion

/// <summary>
/// 邮件模板导入模板对象
/// </summary>
public class LeanMailTmplImportTemplateDto
{
  /// <summary>
  /// 模板编码
  /// </summary>
  [LeanExcelColumn("模板编码")]
  public string TmplCode { get; set; } = "MAIL_TEMPLATE_001";

  /// <summary>
  /// 模板名称
  /// </summary>
  [LeanExcelColumn("模板名称")]
  public string TmplName { get; set; } = "示例邮件模板";

  /// <summary>
  /// 模板主题
  /// </summary>
  [LeanExcelColumn("模板主题")]
  public string TmplSubject { get; set; } = "示例邮件主题";

  /// <summary>
  /// 模板内容
  /// </summary>
  [LeanExcelColumn("模板内容")]
  public string TmplContent { get; set; } = "这是一个示例邮件模板内容";

  /// <summary>
  /// 模板签名
  /// </summary>
  [LeanExcelColumn("模板签名")]
  public string TmplSignature { get; set; } = "示例签名";

  /// <summary>
  /// 是否HTML格式
  /// </summary>
  [LeanExcelColumn("是否HTML格式")]
  public int TmplIsHtml { get; set; } = 1;

  /// <summary>
  /// 优先级
  /// </summary>
  [LeanExcelColumn("优先级")]
  public int TmplPriority { get; set; } = 2;

  /// <summary>
  /// 备注
  /// </summary>
  [LeanExcelColumn("备注")]
  public string? TmplRemark { get; set; } = "这是一个示例邮件模板";

  /// <summary>
  /// 状态
  /// </summary>
  [LeanExcelColumn("状态")]
  public int TmplStatus { get; set; } = 1;
}

/// <summary>
/// 邮件模板导入错误
/// </summary>
public class LeanMailTmplImportErrorDto : LeanImportError
{
  /// <summary>
  /// 模板名称
  /// </summary>
  public string Name
  {
    get => Key;
    set => Key = value;
  }
}

/// <summary>
/// 邮件模板导入结果
/// </summary>
public class LeanMailTmplImportResultDto : LeanImportResult
{
  /// <summary>
  /// 错误列表
  /// </summary>
  public List<LeanMailTmplImportErrorDto> Errors { get; set; } = new();

  /// <summary>
  /// 添加错误
  /// </summary>
  public override void AddError(string key, string message)
  {
    Errors.Add(new LeanMailTmplImportErrorDto
    {
      Name = key,
      ErrorMessage = message
    });
  }
}