// -----------------------------------------------------------------------
// <copyright file="LeanFileController.cs" company="Lean">
// Copyright (c) Lean. All rights reserved.
// </copyright>
// <author>Lean</author>
// <created>2024-03-09</created>
// <summary>文件控制器</summary>
// -----------------------------------------------------------------------

using Lean.CodeGen.Application.Dtos.Routine;
using Lean.CodeGen.Application.Services.Routine;
using Lean.CodeGen.Application.Services.Admin;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Common.Excel;
using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Common.Attributes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace Lean.CodeGen.WebApi.Controllers.Routine;

/// <summary>
/// 文件控制器
/// </summary>
[ApiController]
[Route("api/[controller]")]
[ApiExplorerSettings(GroupName = "routine")]
[LeanPermission("routine:file", "文件管理")]
public class LeanFileController : LeanBaseController
{
  private readonly ILeanFileService _fileService;

  /// <summary>
  /// 构造函数
  /// </summary>
  public LeanFileController(
      ILeanFileService fileService,
      ILeanLocalizationService localizationService,
      IConfiguration configuration)
      : base(localizationService, configuration)
  {
    _fileService = fileService;
  }

  /// <summary>
  /// 获取文件分页列表
  /// </summary>
  [HttpGet]
  [LeanPermission("routine:file:query", "查询文件")]
  public async Task<IActionResult> GetPagedListAsync([FromQuery] LeanFileQueryDto input)
  {
    var result = await _fileService.GetPagedListAsync(input);
    if (!result.Success)
    {
      return await ErrorAsync(result.Message ?? "common.error.query_failed");
    }
    return Success(result.Data, LeanBusinessType.Query);
  }

  /// <summary>
  /// 获取文件列表
  /// </summary>
  [HttpGet("list")]
  [LeanPermission("routine:file:query", "查询文件")]
  public async Task<IActionResult> GetListAsync([FromQuery] LeanFileQueryDto input)
  {
    var result = await _fileService.GetListAsync(input);
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 获取文件详情
  /// </summary>
  [HttpGet("{id}")]
  [LeanPermission("routine:file:query", "查询文件")]
  public async Task<IActionResult> GetAsync(long id)
  {
    var result = await _fileService.GetAsync(id);
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 上传文件
  /// </summary>
  [HttpPost("upload")]
  [LeanPermission("routine:file:upload", "上传文件")]
  public async Task<IActionResult> UploadAsync([FromForm] LeanFileInfo file)
  {
    if (file == null || file.Stream == null || file.Stream.Length == 0)
    {
      return await ErrorAsync("common.error.file_required");
    }

    var result = await _fileService.UploadAsync(file);
    return Success(result, LeanBusinessType.Create);
  }

  /// <summary>
  /// 下载文件
  /// </summary>
  [HttpGet("download/{id}")]
  [LeanPermission("routine:file:download", "下载文件")]
  public async Task<IActionResult> DownloadAsync(long id)
  {
    var result = await _fileService.DownloadAsync(id);
    if (result == null)
    {
      return NotFound();
    }

    return File(result.Stream, result.ContentType, result.FileName);
  }

  /// <summary>
  /// 删除文件
  /// </summary>
  [HttpDelete("{id}")]
  [LeanPermission("routine:file:delete", "删除文件")]
  public async Task<IActionResult> DeleteAsync(long id)
  {
    var result = await _fileService.DeleteAsync(id);
    return Success(result, LeanBusinessType.Delete);
  }

  /// <summary>
  /// 批量删除文件
  /// </summary>
  [HttpPost("batch-delete")]
  [LeanPermission("routine:file:delete", "删除文件")]
  public async Task<IActionResult> BatchDeleteAsync([FromBody] List<long> ids)
  {
    var result = await _fileService.BatchDeleteAsync(ids);
    return Success(result, LeanBusinessType.Delete);
  }

  /// <summary>
  /// 导出文件
  /// </summary>
  [HttpGet("export")]
  [LeanPermission("routine:file:export", "导出文件")]
  public async Task<IActionResult> ExportAsync([FromQuery] LeanFileQueryDto input)
  {
    var bytes = await _fileService.ExportAsync(input);
    return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"文件列表_{DateTime.Now:yyyyMMddHHmmss}.xlsx");
  }

  /// <summary>
  /// 导入文件
  /// </summary>
  [HttpPost("import")]
  [LeanPermission("routine:file:import", "导入文件")]
  public async Task<IActionResult> ImportAsync([FromForm] LeanFileInfo file)
  {
    if (file == null || file.Stream == null || file.Stream.Length == 0)
    {
      return await ErrorAsync("common.error.file_required");
    }

    var result = await _fileService.ImportAsync(file);
    return Success(result, LeanBusinessType.Import);
  }

  /// <summary>
  /// 获取导入模板
  /// </summary>
  [HttpGet("import/template")]
  [LeanPermission("routine:file:import", "导入文件")]
  public async Task<IActionResult> GetImportTemplateAsync()
  {
    var bytes = await _fileService.GetImportTemplateAsync();
    return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"文件导入模板.xlsx");
  }
}