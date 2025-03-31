// -----------------------------------------------------------------------
// <copyright file="ILeanFileService.cs" company="Lean">
// Copyright (c) Lean. All rights reserved.
// </copyright>
// <author>Lean</author>
// <created>2024-03-09</created>
// <summary>文件服务接口</summary>
// -----------------------------------------------------------------------

using System.Threading.Tasks;
using System.Collections.Generic;
using Lean.CodeGen.Application.Dtos.Routine;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Common.Excel;

namespace Lean.CodeGen.Application.Services.Routine;

/// <summary>
/// 文件服务接口
/// </summary>
public interface ILeanFileService
{
  /// <summary>
  /// 获取文件列表
  /// </summary>
  /// <param name="input">查询参数</param>
  /// <returns>文件列表</returns>
  Task<List<LeanFileDto>> GetListAsync(LeanFileQueryDto input);

  /// <summary>
  /// 获取文件详情
  /// </summary>
  /// <param name="id">文件ID</param>
  /// <returns>文件详情</returns>
  Task<LeanFileDto> GetAsync(long id);

  /// <summary>
  /// 上传文件
  /// </summary>
  /// <param name="file">文件信息</param>
  /// <returns>上传结果</returns>
  Task<LeanApiResult<LeanFileDto>> UploadAsync(LeanFileInfo file);

  /// <summary>
  /// 下载文件
  /// </summary>
  /// <param name="id">文件ID</param>
  /// <returns>文件流</returns>
  Task<LeanFileResult> DownloadAsync(long id);

  /// <summary>
  /// 删除文件
  /// </summary>
  /// <param name="id">文件ID</param>
  /// <returns>删除结果</returns>
  Task<LeanApiResult> DeleteAsync(long id);

  /// <summary>
  /// 批量删除文件
  /// </summary>
  /// <param name="ids">文件ID列表</param>
  /// <returns>删除结果</returns>
  Task<LeanApiResult> BatchDeleteAsync(List<long> ids);

  /// <summary>
  /// 导出文件
  /// </summary>
  /// <param name="input">查询参数</param>
  /// <returns>导出文件字节数组</returns>
  Task<byte[]> ExportAsync(LeanFileQueryDto input);

  /// <summary>
  /// 导入文件
  /// </summary>
  /// <param name="file">导入文件</param>
  /// <returns>导入结果</returns>
  Task<LeanFileImportResultDto> ImportAsync(LeanFileInfo file);

  /// <summary>
  /// 获取导入模板
  /// </summary>
  /// <returns>模板文件字节数组</returns>
  Task<byte[]> GetTemplateAsync();

  /// <summary>
  /// 获取文件分页列表
  /// </summary>
  /// <param name="input">查询参数</param>
  /// <returns>分页结果</returns>
  Task<LeanApiResult<LeanPageResult<LeanFileDto>>> GetPagedListAsync(LeanFileQueryDto input);
}