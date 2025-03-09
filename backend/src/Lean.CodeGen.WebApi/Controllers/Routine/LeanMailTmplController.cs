// -----------------------------------------------------------------------
// <copyright file="LeanMailTmplController.cs" company="Lean">
// Copyright (c) Lean. All rights reserved.
// </copyright>
// <author>Lean</author>
// <created>2024-03-09</created>
// <summary>邮件模板控制器</summary>
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
using Microsoft.AspNetCore.Http;
using NLog;

namespace Lean.CodeGen.WebApi.Controllers.Routine;

/// <summary>
/// 邮件模板控制器
/// </summary>
[ApiController]
[Route("api/[controller]")]
[ApiExplorerSettings(GroupName = "routine")]
[LeanPermission("routine:mail-tmpl", "邮件模板管理")]
public class LeanMailTmplController : LeanBaseController
{
  private readonly ILeanMailTmplService _mailTmplService;
  private readonly NLog.ILogger _logger;

  /// <summary>
  /// 构造函数
  /// </summary>
  public LeanMailTmplController(
      ILeanMailTmplService mailTmplService,
      ILeanLocalizationService localizationService,
      IConfiguration configuration,
      NLog.ILogger logger)
      : base(localizationService, configuration)
  {
    _mailTmplService = mailTmplService;
    _logger = logger;
  }

  /// <summary>
  /// 获取邮件模板列表
  /// </summary>
  [HttpGet]
  [LeanPermission("routine:mail-tmpl:query", "查询邮件模板")]
  public async Task<IActionResult> GetListAsync([FromQuery] LeanMailTmplQueryDto query)
  {
    var result = await _mailTmplService.GetListAsync(query);
    if (!result.Success)
    {
      return await ErrorAsync(result.Message ?? "common.error.query_failed");
    }
    return Success(result.Data, LeanBusinessType.Query);
  }

  /// <summary>
  /// 获取邮件模板详情
  /// </summary>
  [HttpGet("{id}")]
  [LeanPermission("routine:mail-tmpl:query", "查询邮件模板")]
  public async Task<IActionResult> GetAsync(long id)
  {
    var result = await _mailTmplService.GetAsync(id);
    if (!result.Success)
    {
      return await ErrorAsync(result.Message ?? "common.error.get_failed");
    }
    return Success(result.Data, LeanBusinessType.Query);
  }

  /// <summary>
  /// 创建邮件模板
  /// </summary>
  [HttpPost]
  [LeanPermission("routine:mail-tmpl:create", "创建邮件模板")]
  public async Task<IActionResult> CreateAsync([FromBody] LeanMailTmplCreateDto input)
  {
    var result = await _mailTmplService.CreateAsync(input);
    if (!result.Success)
    {
      return await ErrorAsync(result.Message ?? "common.error.create_failed");
    }
    return Success(result.Data, LeanBusinessType.Create);
  }

  /// <summary>
  /// 更新邮件模板
  /// </summary>
  [HttpPut]
  [LeanPermission("routine:mail-tmpl:update", "更新邮件模板")]
  public async Task<IActionResult> UpdateAsync([FromBody] LeanMailTmplUpdateDto input)
  {
    var result = await _mailTmplService.UpdateAsync(input);
    if (!result.Success)
    {
      return await ErrorAsync(result.Message ?? "common.error.update_failed");
    }
    return Success(result.Data, LeanBusinessType.Update);
  }

  /// <summary>
  /// 删除邮件模板
  /// </summary>
  [HttpDelete("{id}")]
  [LeanPermission("routine:mail-tmpl:delete", "删除邮件模板")]
  public async Task<IActionResult> DeleteAsync(long id)
  {
    var result = await _mailTmplService.DeleteAsync(id);
    if (!result.Success)
    {
      return await ErrorAsync(result.Message ?? "common.error.delete_failed");
    }
    return Success<object?>(null, LeanBusinessType.Delete);
  }

  /// <summary>
  /// 批量删除邮件模板
  /// </summary>
  [HttpDelete("batch")]
  [LeanPermission("routine:mail-tmpl:delete", "删除邮件模板")]
  public async Task<IActionResult> BatchDeleteAsync([FromBody] List<long> ids)
  {
    var result = await _mailTmplService.BatchDeleteAsync(ids);
    if (!result.Success)
    {
      return await ErrorAsync(result.Message ?? "common.error.delete_failed");
    }
    return Success<object?>(null, LeanBusinessType.Delete);
  }

  /// <summary>
  /// 导入邮件模板
  /// </summary>
  [HttpPost("import")]
  [LeanPermission("routine:mail_tmpl:import", "导入邮件模板")]
  public async Task<IActionResult> ImportAsync([FromForm] LeanFileInfo file)
  {
    try
    {
      // 验证文件
      if (file == null || file.Stream == null || file.Stream.Length == 0)
      {
        return BadRequest("请选择要导入的文件");
      }

      // 导入数据
      var result = await _mailTmplService.ImportAsync(file);
      return Ok(result);
    }
    catch (Exception ex)
    {
      _logger.Error(ex, "导入邮件模板失败");
      return Problem("导入邮件模板失败：" + ex.Message);
    }
  }

  /// <summary>
  /// 导出邮件模板
  /// </summary>
  [HttpGet("export")]
  [LeanPermission("routine:mail-tmpl:export", "导出邮件模板")]
  public async Task<IActionResult> ExportAsync([FromQuery] LeanMailTmplQueryDto input)
  {
    var bytes = await _mailTmplService.ExportAsync(input);
    var fileName = $"mail_templates_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
    return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
  }

  /// <summary>
  /// 获取导入模板
  /// </summary>
  [HttpGet("template")]
  [LeanPermission("routine:mail-tmpl:import", "导入邮件模板")]
  public async Task<IActionResult> GetImportTemplateAsync()
  {
    var bytes = await _mailTmplService.GetImportTemplateAsync();
    var fileName = $"mail_template_template_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
    return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
  }

  /// <summary>
  /// 启用邮件模板
  /// </summary>
  [HttpPost("{id}/enable")]
  [LeanPermission("routine:mail-tmpl:update", "更新邮件模板")]
  public async Task<IActionResult> EnableAsync(long id)
  {
    var result = await _mailTmplService.EnableAsync(id);
    if (!result.Success)
    {
      return await ErrorAsync(result.Message ?? "common.error.enable_failed");
    }
    return Success<object?>(null, LeanBusinessType.Other);
  }

  /// <summary>
  /// 禁用邮件模板
  /// </summary>
  [HttpPost("{id}/disable")]
  [LeanPermission("routine:mail-tmpl:update", "更新邮件模板")]
  public async Task<IActionResult> DisableAsync(long id)
  {
    var result = await _mailTmplService.DisableAsync(id);
    if (!result.Success)
    {
      return await ErrorAsync(result.Message ?? "common.error.disable_failed");
    }
    return Success<object?>(null, LeanBusinessType.Other);
  }

  /// <summary>
  /// 批量启用邮件模板
  /// </summary>
  [HttpPost("batch/enable")]
  [LeanPermission("routine:mail-tmpl:update", "更新邮件模板")]
  public async Task<IActionResult> BatchEnableAsync([FromBody] List<long> ids)
  {
    var result = await _mailTmplService.BatchEnableAsync(ids);
    if (!result.Success)
    {
      return await ErrorAsync(result.Message ?? "common.error.enable_failed");
    }
    return Success<object?>(null, LeanBusinessType.Other);
  }

  /// <summary>
  /// 批量禁用邮件模板
  /// </summary>
  [HttpPost("batch/disable")]
  [LeanPermission("routine:mail-tmpl:update", "更新邮件模板")]
  public async Task<IActionResult> BatchDisableAsync([FromBody] List<long> ids)
  {
    var result = await _mailTmplService.BatchDisableAsync(ids);
    if (!result.Success)
    {
      return await ErrorAsync(result.Message ?? "common.error.disable_failed");
    }
    return Success<object?>(null, LeanBusinessType.Other);
  }
}