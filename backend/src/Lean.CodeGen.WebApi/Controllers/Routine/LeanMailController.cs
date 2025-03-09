// -----------------------------------------------------------------------
// <copyright file="LeanMailController.cs" company="Lean">
// Copyright (c) Lean. All rights reserved.
// </copyright>
// <author>Lean</author>
// <created>2024-03-09</created>
// <summary>邮件控制器</summary>
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
using System.Linq;
using Microsoft.AspNetCore.Http;
using NLog;

namespace Lean.CodeGen.WebApi.Controllers.Routine;

/// <summary>
/// 邮件控制器
/// </summary>
[ApiController]
[Route("api/[controller]")]
[ApiExplorerSettings(GroupName = "routine")]
[LeanPermission("routine:mail", "邮件管理")]
public class LeanMailController : LeanBaseController
{
  private readonly ILeanMailService _mailService;
  private readonly NLog.ILogger _logger;

  /// <summary>
  /// 构造函数
  /// </summary>
  public LeanMailController(
      ILeanMailService mailService,
      ILeanLocalizationService localizationService,
      IConfiguration configuration)
      : base(localizationService, configuration)
  {
    _mailService = mailService;
    _logger = LogManager.GetCurrentClassLogger();
  }

  /// <summary>
  /// 获取邮件列表
  /// </summary>
  [HttpGet]
  [LeanPermission("routine:mail:query", "查询邮件")]
  public async Task<IActionResult> GetListAsync([FromQuery] LeanMailQueryDto query)
  {
    var result = await _mailService.GetListAsync(query);
    if (!result.Success)
    {
      return await ErrorAsync(result.Message ?? "common.error.query_failed");
    }
    return Success(result.Data, LeanBusinessType.Query);
  }

  /// <summary>
  /// 获取邮件详情
  /// </summary>
  [HttpGet("{id}")]
  [LeanPermission("routine:mail:query", "查询邮件")]
  public async Task<IActionResult> GetAsync(long id)
  {
    var result = await _mailService.GetAsync(id);
    if (!result.Success)
    {
      return await ErrorAsync(result.Message ?? "common.error.get_failed");
    }
    return Success(result.Data, LeanBusinessType.Query);
  }

  /// <summary>
  /// 创建邮件
  /// </summary>
  [HttpPost]
  [LeanPermission("routine:mail:create", "创建邮件")]
  public async Task<IActionResult> CreateAsync([FromBody] LeanMailCreateDto input)
  {
    var result = await _mailService.CreateAsync(input);
    if (!result.Success)
    {
      return await ErrorAsync(result.Message ?? "common.error.create_failed");
    }
    return Success(result.Data, LeanBusinessType.Create);
  }

  /// <summary>
  /// 更新邮件
  /// </summary>
  [HttpPut]
  [LeanPermission("routine:mail:update", "更新邮件")]
  public async Task<IActionResult> UpdateAsync([FromBody] LeanMailUpdateDto input)
  {
    var result = await _mailService.UpdateAsync(input);
    if (!result.Success)
    {
      return await ErrorAsync(result.Message ?? "common.error.update_failed");
    }
    return Success(result.Data, LeanBusinessType.Update);
  }

  /// <summary>
  /// 删除邮件
  /// </summary>
  [HttpDelete("{id}")]
  [LeanPermission("routine:mail:delete", "删除邮件")]
  public async Task<IActionResult> DeleteAsync(long id)
  {
    var result = await _mailService.DeleteAsync(id);
    if (!result.Success)
    {
      return await ErrorAsync(result.Message ?? "common.error.delete_failed");
    }
    return Success<object?>(null, LeanBusinessType.Delete);
  }

  /// <summary>
  /// 批量删除邮件
  /// </summary>
  [HttpDelete("batch")]
  [LeanPermission("routine:mail:delete", "删除邮件")]
  public async Task<IActionResult> BatchDeleteAsync([FromBody] List<long> ids)
  {
    var result = await _mailService.BatchDeleteAsync(ids);
    if (!result.Success)
    {
      return await ErrorAsync(result.Message ?? "common.error.delete_failed");
    }
    return Success<object?>(null, LeanBusinessType.Delete);
  }

  /// <summary>
  /// 导入邮件
  /// </summary>
  [HttpPost("import")]
  [LeanPermission("routine:mail:import", "导入邮件")]
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
      var result = await _mailService.ImportAsync(file);
      return Ok(result);
    }
    catch (Exception ex)
    {
      _logger.Error(ex, "导入邮件失败");
      return Problem("导入邮件失败：" + ex.Message);
    }
  }

  /// <summary>
  /// 导出邮件
  /// </summary>
  [HttpGet("export")]
  [LeanPermission("routine:mail:export", "导出邮件")]
  public async Task<IActionResult> ExportAsync([FromQuery] LeanMailQueryDto input)
  {
    var bytes = await _mailService.ExportAsync(input);
    var fileName = $"mails_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
    return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
  }

  /// <summary>
  /// 获取导入模板
  /// </summary>
  [HttpGet("template")]
  [LeanPermission("routine:mail:import", "导入邮件")]
  public async Task<IActionResult> GetImportTemplateAsync()
  {
    var bytes = await _mailService.GetImportTemplateAsync();
    var fileName = $"mail_template_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
    return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
  }

  /// <summary>
  /// 发送邮件
  /// </summary>
  [HttpPost("{id}/send")]
  [LeanPermission("routine:mail:send", "发送邮件")]
  public async Task<IActionResult> SendAsync(long id)
  {
    var result = await _mailService.SendAsync(id);
    if (!result.Success)
    {
      return await ErrorAsync(result.Message ?? "common.error.send_failed");
    }
    return Success<object?>(null, LeanBusinessType.Other);
  }

  /// <summary>
  /// 批量发送邮件
  /// </summary>
  [HttpPost("batch/send")]
  [LeanPermission("routine:mail:send", "发送邮件")]
  public async Task<IActionResult> BatchSendAsync([FromBody] List<long> ids)
  {
    var result = await _mailService.BatchSendAsync(ids);
    if (!result.Success)
    {
      return await ErrorAsync(result.Message ?? "common.error.send_failed");
    }
    return Success<object?>(null, LeanBusinessType.Other);
  }

  /// <summary>
  /// 撤回邮件
  /// </summary>
  [HttpPost("{id}/withdraw")]
  [LeanPermission("routine:mail:withdraw", "撤回邮件")]
  public async Task<IActionResult> WithdrawAsync(long id)
  {
    var result = await _mailService.WithdrawAsync(id);
    if (!result.Success)
    {
      return await ErrorAsync(result.Message ?? "common.error.withdraw_failed");
    }
    return Success<object?>(null, LeanBusinessType.Other);
  }

  /// <summary>
  /// 批量撤回邮件
  /// </summary>
  [HttpPost("batch/withdraw")]
  [LeanPermission("routine:mail:withdraw", "撤回邮件")]
  public async Task<IActionResult> BatchWithdrawAsync([FromBody] List<long> ids)
  {
    var result = await _mailService.BatchWithdrawAsync(ids);
    if (!result.Success)
    {
      return await ErrorAsync(result.Message ?? "common.error.withdraw_failed");
    }
    return Success<object?>(null, LeanBusinessType.Other);
  }
}