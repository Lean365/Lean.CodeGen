// -----------------------------------------------------------------------
// <copyright file="LeanNotificationDto.cs" company="Lean">
// Copyright (c) Lean. All rights reserved.
// </copyright>
// <author>Lean</author>
// <created>2024-03-09</created>
// <summary>通知数据传输对象</summary>
// -----------------------------------------------------------------------

using Lean.CodeGen.Common.Excel;
using System.ComponentModel.DataAnnotations;

namespace Lean.CodeGen.Application.Dtos.Routine;

#region 查询
/// <summary>
/// 通知查询对象
/// </summary>
public class LeanNotificationQueryDto : LeanPage
{
  /// <summary>
  /// 通知标题
  /// </summary>
  public string? Title { get; set; }

  /// <summary>
  /// 通知类型
  /// </summary>
  public int? Type { get; set; }

  /// <summary>
  /// 状态
  /// </summary>
  public int? Status { get; set; }

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
/// 通知详情对象
/// </summary>
public class LeanNotificationDto : LeanBaseDto
{
  /// <summary>
  /// 通知标题
  /// </summary>
  public string Title { get; set; } = string.Empty;

  /// <summary>
  /// 通知内容
  /// </summary>
  public string Content { get; set; } = string.Empty;

  /// <summary>
  /// 通知类型（1=系统通知，2=业务通知）
  /// </summary>
  public int Type { get; set; } = 1;

  /// <summary>
  /// 优先级（1=低，2=普通，3=高）
  /// </summary>
  public int Priority { get; set; } = 2;

  /// <summary>
  /// 接收者ID列表（以分号分隔）
  /// </summary>
  public string ReceiverIds { get; set; } = string.Empty;

  /// <summary>
  /// 接收者名称列表（以分号分隔）
  /// </summary>
  public string ReceiverNames { get; set; } = string.Empty;

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

  /// <summary>
  /// 状态
  /// </summary>
  public int Status { get; set; } = 1;
}
#endregion

#region 创建
/// <summary>
/// 通知创建对象
/// </summary>
public class LeanNotificationCreateDto
{
  /// <summary>
  /// 通知标题
  /// </summary>
  [Required(ErrorMessage = "通知标题不能为空")]
  [StringLength(200, MinimumLength = 2, ErrorMessage = "通知标题长度必须在2-200个字符之间")]
  public string Title { get; set; } = string.Empty;

  /// <summary>
  /// 通知内容
  /// </summary>
  [Required(ErrorMessage = "通知内容不能为空")]
  [StringLength(4000, MinimumLength = 2, ErrorMessage = "通知内容长度必须在2-4000个字符之间")]
  public string Content { get; set; } = string.Empty;

  /// <summary>
  /// 通知类型（1=系统通知，2=业务通知）
  /// </summary>
  public int Type { get; set; } = 1;

  /// <summary>
  /// 优先级（1=低，2=普通，3=高）
  /// </summary>
  public int Priority { get; set; } = 2;

  /// <summary>
  /// 接收者ID列表（以分号分隔）
  /// </summary>
  [Required(ErrorMessage = "接收者不能为空")]
  [StringLength(4000, ErrorMessage = "接收者ID列表长度不能超过4000个字符")]
  public string ReceiverIds { get; set; } = string.Empty;

  /// <summary>
  /// 接收者名称列表（以分号分隔）
  /// </summary>
  [Required(ErrorMessage = "接收者名称不能为空")]
  [StringLength(4000, ErrorMessage = "接收者名称列表长度不能超过4000个字符")]
  public string ReceiverNames { get; set; } = string.Empty;

  /// <summary>
  /// 接收者邮箱列表（以分号分隔）
  /// </summary>
  [StringLength(4000, ErrorMessage = "接收者邮箱列表长度不能超过4000个字符")]
  public string? ReceiverEmails { get; set; }

  /// <summary>
  /// 是否发送邮件通知
  /// </summary>
  public bool SendEmail { get; set; }

  /// <summary>
  /// 状态
  /// </summary>
  public int Status { get; set; } = 1;
}
#endregion

#region 更新
/// <summary>
/// 通知更新对象
/// </summary>
public class LeanNotificationUpdateDto : LeanNotificationCreateDto
{
  /// <summary>
  /// 主键
  /// </summary>
  public long Id { get; set; }
}
#endregion

#region 导入
/// <summary>
/// 通知导入对象
/// </summary>
public class LeanNotificationImportDto
{
  /// <summary>
  /// 通知标题
  /// </summary>
  [LeanExcelColumn("通知标题")]
  public string Title { get; set; } = string.Empty;

  /// <summary>
  /// 通知内容
  /// </summary>
  [LeanExcelColumn("通知内容")]
  public string Content { get; set; } = string.Empty;

  /// <summary>
  /// 通知类型（1=系统通知，2=业务通知）
  /// </summary>
  [LeanExcelColumn("通知类型")]
  public int Type { get; set; } = 1;

  /// <summary>
  /// 优先级（1=低，2=普通，3=高）
  /// </summary>
  [LeanExcelColumn("优先级")]
  public int Priority { get; set; } = 2;

  /// <summary>
  /// 接收者ID列表（以分号分隔）
  /// </summary>
  [LeanExcelColumn("接收者ID列表")]
  public string ReceiverIds { get; set; } = string.Empty;

  /// <summary>
  /// 接收者名称列表（以分号分隔）
  /// </summary>
  [LeanExcelColumn("接收者名称列表")]
  public string ReceiverNames { get; set; } = string.Empty;

  /// <summary>
  /// 状态
  /// </summary>
  [LeanExcelColumn("状态")]
  public int Status { get; set; } = 1;
}
#endregion

#region 导出
/// <summary>
/// 通知导出对象
/// </summary>
public class LeanNotificationExportDto
{
  /// <summary>
  /// 通知标题
  /// </summary>
  [LeanExcelColumn("通知标题")]
  public string Title { get; set; } = string.Empty;

  /// <summary>
  /// 通知内容
  /// </summary>
  [LeanExcelColumn("通知内容")]
  public string Content { get; set; } = string.Empty;

  /// <summary>
  /// 通知类型（1=系统通知，2=业务通知）
  /// </summary>
  [LeanExcelColumn("通知类型")]
  public int Type { get; set; } = 1;

  /// <summary>
  /// 优先级（1=低，2=普通，3=高）
  /// </summary>
  [LeanExcelColumn("优先级")]
  public int Priority { get; set; } = 2;

  /// <summary>
  /// 接收者名称列表（以分号分隔）
  /// </summary>
  [LeanExcelColumn("接收者名称列表")]
  public string ReceiverNames { get; set; } = string.Empty;

  /// <summary>
  /// 发送状态（0=待发送，1=发送成功，2=发送失败）
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
/// 通知导入模板对象
/// </summary>
public class LeanNotificationImportTemplateDto
{
  /// <summary>
  /// 通知标题
  /// </summary>
  [LeanExcelColumn("通知标题")]
  public string Title { get; set; } = "示例通知";

  /// <summary>
  /// 通知内容
  /// </summary>
  [LeanExcelColumn("通知内容")]
  public string Content { get; set; } = "这是一条示例通知内容";

  /// <summary>
  /// 通知类型（1=系统通知，2=业务通知）
  /// </summary>
  [LeanExcelColumn("通知类型")]
  public int Type { get; set; } = 1;

  /// <summary>
  /// 优先级（1=低，2=普通，3=高）
  /// </summary>
  [LeanExcelColumn("优先级")]
  public int Priority { get; set; } = 2;

  /// <summary>
  /// 接收者ID列表（以分号分隔）
  /// </summary>
  [LeanExcelColumn("接收者ID列表")]
  public string ReceiverIds { get; set; } = "1;2;3";

  /// <summary>
  /// 接收者名称列表（以分号分隔）
  /// </summary>
  [LeanExcelColumn("接收者名称列表")]
  public string ReceiverNames { get; set; } = "用户1;用户2;用户3";

  /// <summary>
  /// 状态
  /// </summary>
  [LeanExcelColumn("状态")]
  public int Status { get; set; } = 1;
}

/// <summary>
/// 通知导入错误
/// </summary>
public class LeanNotificationImportErrorDto : LeanImportError
{
  /// <summary>
  /// 通知标题
  /// </summary>
  public string Title
  {
    get => Key;
    set => Key = value;
  }
}

/// <summary>
/// 通知导入结果
/// </summary>
public class LeanNotificationImportResultDto : LeanImportResult
{
  /// <summary>
  /// 错误列表
  /// </summary>
  public List<LeanNotificationImportErrorDto> Errors { get; set; } = new();

  /// <summary>
  /// 添加错误
  /// </summary>
  public override void AddError(string key, string message)
  {
    Errors.Add(new LeanNotificationImportErrorDto
    {
      Title = key,
      ErrorMessage = message
    });
  }
}

