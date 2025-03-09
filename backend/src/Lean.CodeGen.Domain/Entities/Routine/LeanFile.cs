// -----------------------------------------------------------------------
// <copyright file="LeanFile.cs" company="Lean">
// Copyright (c) Lean. All rights reserved.
// </copyright>
// <author>Lean</author>
// <created>2024-03-09</created>
// <summary>文件实体类</summary>
// -----------------------------------------------------------------------

using SqlSugar;

namespace Lean.CodeGen.Domain.Entities.Routine;

/// <summary>
/// 文件实体类
/// </summary>
[SugarTable("lean_rou_file", "文件信息表")]
public class LeanFile : LeanBaseEntity
{
  /// <summary>
  /// 文件名称
  /// </summary>
  [SugarColumn(ColumnDescription = "文件名称", Length = 255, IsNullable = false, ColumnDataType = "nvarchar")]
  public string FileName { get; set; } = string.Empty;

  /// <summary>
  /// 文件原始名称
  /// </summary>
  [SugarColumn(ColumnDescription = "文件原始名称", Length = 255, IsNullable = false, ColumnDataType = "nvarchar")]
  public string OriginalFileName { get; set; } = string.Empty;

  /// <summary>
  /// 文件扩展名
  /// </summary>
  [SugarColumn(ColumnDescription = "文件扩展名", Length = 50, IsNullable = true, ColumnDataType = "nvarchar")]
  public string Extension { get; set; } = string.Empty;

  /// <summary>
  /// 文件大小（字节）
  /// </summary>
  [SugarColumn(ColumnDescription = "文件大小（字节）", IsNullable = false, DefaultValue = "0")]
  public long FileSize { get; set; }

  /// <summary>
  /// 文件类型
  /// </summary>
  [SugarColumn(ColumnDescription = "文件类型", Length = 100, IsNullable = true, ColumnDataType = "nvarchar")]
  public string ContentType { get; set; } = string.Empty;

  /// <summary>
  /// 存储路径
  /// </summary>
  [SugarColumn(ColumnDescription = "存储路径", Length = 500, IsNullable = false, ColumnDataType = "nvarchar")]
  public string FilePath { get; set; } = string.Empty;

  /// <summary>
  /// 文件MD5值
  /// </summary>
  [SugarColumn(ColumnDescription = "文件MD5值", Length = 32, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? FileMD5 { get; set; }

  /// <summary>
  /// 存储类型（1=本地存储，2=阿里云OSS，3=腾讯云COS，4=七牛云）
  /// </summary>
  [SugarColumn(ColumnDescription = "存储类型", IsNullable = false, DefaultValue = "1")]
  public int StorageType { get; set; } = 1;

  /// <summary>
  /// 访问地址
  /// </summary>
  [SugarColumn(ColumnDescription = "访问地址", Length = 500, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? AccessUrl { get; set; }

  /// <summary>
  /// 文件分类（1=普通文件，2=图片，3=音频，4=视频，5=文档）
  /// </summary>
  [SugarColumn(ColumnDescription = "文件分类", IsNullable = false, DefaultValue = "1")]
  public int FileType { get; set; } = 1;

  /// <summary>
  /// 业务模块
  /// </summary>
  [SugarColumn(ColumnDescription = "业务模块", Length = 100, IsNullable = true, ColumnDataType = "nvarchar")]
  public string? BusinessModule { get; set; }

  /// <summary>
  /// 业务ID
  /// </summary>
  [SugarColumn(ColumnDescription = "业务ID", IsNullable = true)]
  public long? BusinessId { get; set; }

  /// <summary>
  /// 是否临时文件（0=否，1=是）
  /// </summary>
  [SugarColumn(ColumnDescription = "是否临时文件（0=否，1=是）", IsNullable = false, DefaultValue = "0")]
  public int IsTemporary { get; set; }

  /// <summary>
  /// 下载次数
  /// </summary>
  [SugarColumn(ColumnDescription = "下载次数", IsNullable = false, DefaultValue = "0")]
  public int DownloadCount { get; set; }
}