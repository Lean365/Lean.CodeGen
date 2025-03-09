// -----------------------------------------------------------------------
// <copyright file="LeanFileDto.cs" company="Lean">
// Copyright (c) Lean. All rights reserved.
// </copyright>
// <author>Lean</author>
// <created>2024-03-09</created>
// <summary>文件数据传输对象</summary>
// -----------------------------------------------------------------------

using Lean.CodeGen.Common.Excel;
using System.ComponentModel.DataAnnotations;

namespace Lean.CodeGen.Application.Dtos.Routine;

#region 查询
/// <summary>
/// 文件查询对象
/// </summary>
public class LeanFileQueryDto : LeanPage
{
  /// <summary>
  /// 文件名称
  /// </summary>
  public string? FileName { get; set; }

  /// <summary>
  /// 文件类型
  /// </summary>
  public string? ContentType { get; set; }

  /// <summary>
  /// 存储类型
  /// </summary>
  public int? StorageType { get; set; }

  /// <summary>
  /// 文件分类
  /// </summary>
  public int? FileType { get; set; }
}
#endregion

#region 详情
/// <summary>
/// 文件详情对象
/// </summary>
public class LeanFileDto : LeanBaseDto
{
  /// <summary>
  /// 文件名称
  /// </summary>
  public string FileName { get; set; } = string.Empty;

  /// <summary>
  /// 文件原始名称
  /// </summary>
  public string OriginalFileName { get; set; } = string.Empty;

  /// <summary>
  /// 文件扩展名
  /// </summary>
  public string Extension { get; set; } = string.Empty;

  /// <summary>
  /// 文件大小（字节）
  /// </summary>
  public long FileSize { get; set; }

  /// <summary>
  /// 文件类型
  /// </summary>
  public string ContentType { get; set; } = string.Empty;

  /// <summary>
  /// 存储路径
  /// </summary>
  public string FilePath { get; set; } = string.Empty;

  /// <summary>
  /// 文件MD5值
  /// </summary>
  public string? FileMD5 { get; set; }

  /// <summary>
  /// 存储类型（1=本地存储，2=阿里云OSS，3=腾讯云COS，4=七牛云）
  /// </summary>
  public int StorageType { get; set; } = 1;

  /// <summary>
  /// 访问地址
  /// </summary>
  public string? AccessUrl { get; set; }

  /// <summary>
  /// 文件分类（1=普通文件，2=图片，3=音频，4=视频，5=文档）
  /// </summary>
  public int FileType { get; set; } = 1;

  /// <summary>
  /// 业务模块
  /// </summary>
  public string? BusinessModule { get; set; }

  /// <summary>
  /// 业务ID
  /// </summary>
  public long? BusinessId { get; set; }

  /// <summary>
  /// 是否临时文件（0=否，1=是）
  /// </summary>
  public int IsTemporary { get; set; }

  /// <summary>
  /// 下载次数
  /// </summary>
  public int DownloadCount { get; set; }
}
#endregion

#region 创建
/// <summary>
/// 文件创建对象
/// </summary>
public class LeanFileCreateDto
{
  /// <summary>
  /// 文件名称
  /// </summary>
  [Required(ErrorMessage = "文件名称不能为空")]
  [StringLength(255, MinimumLength = 2, ErrorMessage = "文件名称长度必须在2-255个字符之间")]
  public string FileName { get; set; } = string.Empty;

  /// <summary>
  /// 文件原始名称
  /// </summary>
  [Required(ErrorMessage = "文件原始名称不能为空")]
  [StringLength(255, MinimumLength = 2, ErrorMessage = "文件原始名称长度必须在2-255个字符之间")]
  public string OriginalFileName { get; set; } = string.Empty;

  /// <summary>
  /// 文件扩展名
  /// </summary>
  [StringLength(50, ErrorMessage = "文件扩展名长度不能超过50个字符")]
  public string Extension { get; set; } = string.Empty;

  /// <summary>
  /// 文件大小（字节）
  /// </summary>
  public long FileSize { get; set; }

  /// <summary>
  /// 文件类型
  /// </summary>
  [StringLength(100, ErrorMessage = "文件类型长度不能超过100个字符")]
  public string ContentType { get; set; } = string.Empty;

  /// <summary>
  /// 存储路径
  /// </summary>
  [Required(ErrorMessage = "存储路径不能为空")]
  [StringLength(500, MinimumLength = 2, ErrorMessage = "存储路径长度必须在2-500个字符之间")]
  public string FilePath { get; set; } = string.Empty;
}
#endregion

#region 更新
/// <summary>
/// 文件更新对象
/// </summary>
public class LeanFileUpdateDto : LeanFileCreateDto
{
  /// <summary>
  /// 主键
  /// </summary>
  public long Id { get; set; }
}
#endregion

#region 导入
/// <summary>
/// 文件导入错误参数
/// </summary>
public class LeanFileImportErrorDto : LeanImportError
{
  /// <summary>
  /// 文件名
  /// </summary>
  public string FileName
  {
    get => Key;
    set => Key = value;
  }
}

/// <summary>
/// 文件导入结果参数
/// </summary>
public class LeanFileImportResultDto : LeanImportResult
{
  /// <summary>
  /// 错误列表
  /// </summary>
  public new List<LeanFileImportErrorDto> Errors { get; set; } = new();

  /// <summary>
  /// 添加错误
  /// </summary>
  /// <param name="fileName">文件名</param>
  /// <param name="errorMessage">错误消息</param>
  public override void AddError(string fileName, string errorMessage)
  {
    Errors.Add(new LeanFileImportErrorDto
    {
      FileName = fileName,
      ErrorMessage = errorMessage
    });
  }
}

/// <summary>
/// 文件导入参数
/// </summary>
public class LeanFileImportDto
{
  /// <summary>
  /// 原始文件名
  /// </summary>
  [LeanExcelColumn("原始文件名")]
  public string OriginalFileName { get; set; } = default!;

  /// <summary>
  /// 内容类型
  /// </summary>
  [LeanExcelColumn("内容类型")]
  public string ContentType { get; set; } = default!;
}
#endregion

#region 导出
/// <summary>
/// 文件导出对象
/// </summary>
public class LeanFileExportDto
{
  /// <summary>
  /// 文件名称
  /// </summary>
  [LeanExcelColumn("文件名称")]
  public string FileName { get; set; } = string.Empty;

  /// <summary>
  /// 文件原始名称
  /// </summary>
  [LeanExcelColumn("文件原始名称")]
  public string OriginalFileName { get; set; } = string.Empty;

  /// <summary>
  /// 文件扩展名
  /// </summary>
  [LeanExcelColumn("文件扩展名")]
  public string Extension { get; set; } = string.Empty;

  /// <summary>
  /// 文件大小（字节）
  /// </summary>
  [LeanExcelColumn("文件大小")]
  public long FileSize { get; set; }

  /// <summary>
  /// 文件类型
  /// </summary>
  [LeanExcelColumn("文件类型")]
  public string ContentType { get; set; } = string.Empty;

  /// <summary>
  /// 存储路径
  /// </summary>
  [LeanExcelColumn("存储路径")]
  public string FilePath { get; set; } = string.Empty;

  /// <summary>
  /// 创建时间
  /// </summary>
  [LeanExcelColumn("创建时间")]
  public DateTime CreateTime { get; set; }
}
#endregion

/// <summary>
/// 文件导入模板对象
/// </summary>
public class LeanFileImportTemplateDto
{
  /// <summary>
  /// 文件名称
  /// </summary>
  [LeanExcelColumn("文件名称")]
  public string FileName { get; set; } = "示例文件.txt";

  /// <summary>
  /// 文件原始名称
  /// </summary>
  [LeanExcelColumn("文件原始名称")]
  public string OriginalFileName { get; set; } = "example.txt";

  /// <summary>
  /// 文件扩展名
  /// </summary>
  [LeanExcelColumn("文件扩展名")]
  public string Extension { get; set; } = ".txt";

  /// <summary>
  /// 文件大小（字节）
  /// </summary>
  [LeanExcelColumn("文件大小")]
  public long FileSize { get; set; } = 1024;

  /// <summary>
  /// 文件类型
  /// </summary>
  [LeanExcelColumn("文件类型")]
  public string ContentType { get; set; } = "text/plain";

  /// <summary>
  /// 存储路径
  /// </summary>
  [LeanExcelColumn("存储路径")]
  public string FilePath { get; set; } = "/uploads/example.txt";
}

