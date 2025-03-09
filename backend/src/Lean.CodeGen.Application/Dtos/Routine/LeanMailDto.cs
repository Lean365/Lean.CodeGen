// -----------------------------------------------------------------------
// <copyright file="LeanMailDto.cs" company="Lean">
// Copyright (c) Lean. All rights reserved.
// </copyright>
// <author>Lean</author>
// <created>2024-03-09</created>
// <summary>邮件数据传输对象</summary>
// -----------------------------------------------------------------------

using Lean.CodeGen.Common.Excel;
using System.ComponentModel.DataAnnotations;

namespace Lean.CodeGen.Application.Dtos.Routine;

#region 查询
/// <summary>
/// 邮件查询对象
/// </summary>
public class LeanMailQueryDto : LeanPage
{
  /// <summary>
  /// 邮件主题
  /// </summary>
  public string? Subject { get; set; }

  /// <summary>
  /// 发件人邮箱
  /// </summary>
  public string? FromAddress { get; set; }

  /// <summary>
  /// 发件人显示名称
  /// </summary>
  public string? FromName { get; set; }

  /// <summary>
  /// 收件人邮箱
  /// </summary>
  public string? ToAddress { get; set; }

  /// <summary>
  /// 发送状态（0=待发送，1=发送成功，2=发送失败）
  /// </summary>
  public int? SendStatus { get; set; }

  /// <summary>
  /// 发送时间范围开始
  /// </summary>
  public DateTime? SendTimeStart { get; set; }

  /// <summary>
  /// 发送时间范围结束
  /// </summary>
  public DateTime? SendTimeEnd { get; set; }
}
#endregion

#region 详情
/// <summary>
/// 邮件详情对象
/// </summary>
public class LeanMailDto : LeanBaseDto
{
  /// <summary>
  /// 邮件主题
  /// </summary>
  public string Subject { get; set; } = string.Empty;

  /// <summary>
  /// 发件人邮箱
  /// </summary>
  public string FromAddress { get; set; } = string.Empty;

  /// <summary>
  /// 发件人显示名称
  /// </summary>
  public string FromName { get; set; } = string.Empty;

  /// <summary>
  /// 收件人邮箱列表（以分号分隔）
  /// </summary>
  public string ToAddresses { get; set; } = string.Empty;

  /// <summary>
  /// 抄送邮箱列表（以分号分隔）
  /// </summary>
  public string? CcAddresses { get; set; }

  /// <summary>
  /// 密送邮箱列表（以分号分隔）
  /// </summary>
  public string? BccAddresses { get; set; }

  /// <summary>
  /// 邮件正文
  /// </summary>
  public string Body { get; set; } = string.Empty;

  /// <summary>
  /// 是否为HTML格式（0=否，1=是）
  /// </summary>
  public int IsBodyHtml { get; set; } = 1;

  /// <summary>
  /// 优先级（1=低，2=普通，3=高）
  /// </summary>
  public int Priority { get; set; } = 2;

  /// <summary>
  /// 附件列表（以分号分隔的文件ID）
  /// </summary>
  public string? Attachments { get; set; }

  /// <summary>
  /// 发送状态（0=待发送，1=发送成功，2=发送失败）
  /// </summary>
  public int SendStatus { get; set; }

  /// <summary>
  /// 发送时间
  /// </summary>
  public DateTime? SendTime { get; set; }

  /// <summary>
  /// 失败原因
  /// </summary>
  public string? FailureReason { get; set; }

  /// <summary>
  /// 重试次数
  /// </summary>
  public int RetryCount { get; set; }
}
#endregion

#region 创建
/// <summary>
/// 邮件创建对象
/// </summary>
public class LeanMailCreateDto
{
  /// <summary>
  /// 邮件主题
  /// </summary>
  [Required(ErrorMessage = "邮件主题不能为空")]
  [StringLength(255, MinimumLength = 2, ErrorMessage = "邮件主题长度必须在2-255个字符之间")]
  public string Subject { get; set; } = string.Empty;

  /// <summary>
  /// 发件人邮箱
  /// </summary>
  [Required(ErrorMessage = "发件人邮箱不能为空")]
  [EmailAddress(ErrorMessage = "发件人邮箱格式不正确")]
  [StringLength(100, MinimumLength = 6, ErrorMessage = "发件人邮箱长度必须在6-100个字符之间")]
  public string FromAddress { get; set; } = string.Empty;

  /// <summary>
  /// 发件人显示名称
  /// </summary>
  [Required(ErrorMessage = "发件人显示名称不能为空")]
  [StringLength(100, MinimumLength = 2, ErrorMessage = "发件人显示名称长度必须在2-100个字符之间")]
  public string FromName { get; set; } = string.Empty;

  /// <summary>
  /// 收件人邮箱列表（以分号分隔）
  /// </summary>
  [Required(ErrorMessage = "收件人邮箱不能为空")]
  [StringLength(1000, MinimumLength = 6, ErrorMessage = "收件人邮箱列表长度必须在6-1000个字符之间")]
  public string ToAddresses { get; set; } = string.Empty;

  /// <summary>
  /// 抄送邮箱列表（以分号分隔）
  /// </summary>
  [StringLength(1000, ErrorMessage = "抄送邮箱列表长度不能超过1000个字符")]
  public string? CcAddresses { get; set; }

  /// <summary>
  /// 密送邮箱列表（以分号分隔）
  /// </summary>
  [StringLength(1000, ErrorMessage = "密送邮箱列表长度不能超过1000个字符")]
  public string? BccAddresses { get; set; }

  /// <summary>
  /// 邮件正文
  /// </summary>
  [Required(ErrorMessage = "邮件正文不能为空")]
  public string Body { get; set; } = string.Empty;

  /// <summary>
  /// 是否为HTML格式（0=否，1=是）
  /// </summary>
  public int IsBodyHtml { get; set; } = 1;

  /// <summary>
  /// 优先级（1=低，2=普通，3=高）
  /// </summary>
  public int Priority { get; set; } = 2;

  /// <summary>
  /// 附件列表（以分号分隔的文件ID）
  /// </summary>
  [StringLength(1000, ErrorMessage = "附件列表长度不能超过1000个字符")]
  public string? Attachments { get; set; }
}
#endregion

#region 更新
/// <summary>
/// 邮件更新对象
/// </summary>
public class LeanMailUpdateDto : LeanMailCreateDto
{
  /// <summary>
  /// 主键
  /// </summary>
  public long Id { get; set; }
}
#endregion

#region 导入
/// <summary>
/// 邮件导入对象
/// </summary>
public class LeanMailImportDto
{
  /// <summary>
  /// 邮件主题
  /// </summary>
  [LeanExcelColumn("邮件主题")]
  public string Subject { get; set; } = string.Empty;

  /// <summary>
  /// 收件人邮箱列表（以分号分隔）
  /// </summary>
  [LeanExcelColumn("收件人邮箱列表")]
  public string ToAddresses { get; set; } = string.Empty;

  /// <summary>
  /// 抄送邮箱列表（以分号分隔）
  /// </summary>
  [LeanExcelColumn("抄送邮箱列表")]
  public string? CcAddresses { get; set; }

  /// <summary>
  /// 邮件正文
  /// </summary>
  [LeanExcelColumn("邮件正文")]
  public string Body { get; set; } = string.Empty;

  /// <summary>
  /// 是否为HTML格式（0=否，1=是）
  /// </summary>
  [LeanExcelColumn("是否HTML格式")]
  public int IsBodyHtml { get; set; } = 1;
}

/// <summary>
/// 邮件导入错误参数
/// </summary>
public class LeanMailImportErrorDto : LeanImportError
{
  /// <summary>
  /// 邮件主题
  /// </summary>
  public string Subject
  {
    get => Key;
    set => Key = value;
  }
}

/// <summary>
/// 邮件导入结果参数
/// </summary>
public class LeanMailImportResultDto : LeanImportResult
{
  /// <summary>
  /// 错误列表
  /// </summary>
  public new List<LeanMailImportErrorDto> Errors { get; set; } = new();

  /// <summary>
  /// 添加错误
  /// </summary>
  /// <param name="subject">邮件主题</param>
  /// <param name="errorMessage">错误消息</param>
  public override void AddError(string subject, string errorMessage)
  {
    Errors.Add(new LeanMailImportErrorDto
    {
      Subject = subject,
      ErrorMessage = errorMessage
    });
  }
}
#endregion

#region 导出
/// <summary>
/// 邮件导出对象
/// </summary>
public class LeanMailExportDto
{
  /// <summary>
  /// 邮件主题
  /// </summary>
  [LeanExcelColumn("邮件主题")]
  public string Subject { get; set; } = string.Empty;

  /// <summary>
  /// 发件人邮箱
  /// </summary>
  [LeanExcelColumn("发件人邮箱")]
  public string FromAddress { get; set; } = string.Empty;

  /// <summary>
  /// 发件人显示名称
  /// </summary>
  [LeanExcelColumn("发件人显示名称")]
  public string FromName { get; set; } = string.Empty;

  /// <summary>
  /// 收件人邮箱列表
  /// </summary>
  [LeanExcelColumn("收件人邮箱列表")]
  public string ToAddresses { get; set; } = string.Empty;

  /// <summary>
  /// 抄送邮箱列表
  /// </summary>
  [LeanExcelColumn("抄送邮箱列表")]
  public string? CcAddresses { get; set; }

  /// <summary>
  /// 发送状态
  /// </summary>
  [LeanExcelColumn("发送状态")]
  public string SendStatusName { get; set; } = string.Empty;

  /// <summary>
  /// 发送时间
  /// </summary>
  [LeanExcelColumn("发送时间")]
  public DateTime? SendTime { get; set; }

  /// <summary>
  /// 失败原因
  /// </summary>
  [LeanExcelColumn("失败原因")]
  public string? FailureReason { get; set; }

  /// <summary>
  /// 创建时间
  /// </summary>
  [LeanExcelColumn("创建时间")]
  public DateTime CreateTime { get; set; }
}
#endregion

/// <summary>
/// 邮件导入模板对象
/// </summary>
public class LeanMailImportTemplateDto
{
  /// <summary>
  /// 邮件主题
  /// </summary>
  [LeanExcelColumn("邮件主题")]
  public string Subject { get; set; } = "示例邮件";

  /// <summary>
  /// 收件人邮箱列表（以分号分隔）
  /// </summary>
  [LeanExcelColumn("收件人邮箱列表")]
  public string ToAddresses { get; set; } = "example@lean.com";

  /// <summary>
  /// 抄送邮箱列表（以分号分隔）
  /// </summary>
  [LeanExcelColumn("抄送邮箱列表")]
  public string? CcAddresses { get; set; } = "cc@lean.com";

  /// <summary>
  /// 邮件正文
  /// </summary>
  [LeanExcelColumn("邮件正文")]
  public string Body { get; set; } = "这是一封示例邮件";

  /// <summary>
  /// 是否为HTML格式（0=否，1=是）
  /// </summary>
  [LeanExcelColumn("是否HTML格式")]
  public int IsBodyHtml { get; set; } = 1;
}

