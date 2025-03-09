// -----------------------------------------------------------------------
// <copyright file="LeanNotificationController.cs" company="Lean">
// Copyright (c) Lean. All rights reserved.
// </copyright>
// <author>Lean</author>
// <created>2024-03-09</created>
// <summary>通知控制器</summary>
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
/// 通知控制器
/// </summary>
[ApiController]
[Route("api/[controller]")]
[ApiExplorerSettings(GroupName = "routine")]
[LeanPermission("routine:notification", "通知管理")]
public class LeanNotificationController : LeanBaseController
{
  private readonly ILeanNotificationService _notificationService;
  private readonly NLog.ILogger _logger;

  /// <summary>
  /// 构造函数
  /// </summary>
  public LeanNotificationController(
      ILeanNotificationService notificationService,
      ILeanLocalizationService localizationService,
      IConfiguration configuration,
      NLog.ILogger logger)
      : base(localizationService, configuration)
  {
    _notificationService = notificationService;
    _logger = logger;
  }

  /// <summary>
  /// 获取通知列表
  /// </summary>
  [HttpGet]
  [LeanPermission("routine:notification:query", "查询通知")]
  public async Task<IActionResult> GetListAsync([FromQuery] LeanNotificationQueryDto query)
  {
    var result = await _notificationService.GetListAsync(query);
    if (!result.Success)
    {
      return await ErrorAsync(result.Message ?? "common.error.query_failed");
    }
    return Success(result.Data, LeanBusinessType.Query);
  }

  /// <summary>
  /// 获取通知详情
  /// </summary>
  [HttpGet("{id}")]
  [LeanPermission("routine:notification:query", "查询通知")]
  public async Task<IActionResult> GetAsync(long id)
  {
    var result = await _notificationService.GetAsync(id);
    if (!result.Success)
    {
      return await ErrorAsync(result.Message ?? "common.error.get_failed");
    }
    return Success(result.Data, LeanBusinessType.Query);
  }

  /// <summary>
  /// 创建通知
  /// </summary>
  [HttpPost]
  [LeanPermission("routine:notification:create", "创建通知")]
  public async Task<IActionResult> CreateAsync([FromBody] LeanNotificationCreateDto input)
  {
    var result = await _notificationService.CreateAsync(input);
    if (!result.Success)
    {
      return await ErrorAsync(result.Message ?? "common.error.create_failed");
    }
    return Success(result.Data, LeanBusinessType.Create);
  }

  /// <summary>
  /// 更新通知
  /// </summary>
  [HttpPut]
  [LeanPermission("routine:notification:update", "更新通知")]
  public async Task<IActionResult> UpdateAsync([FromBody] LeanNotificationUpdateDto input)
  {
    var result = await _notificationService.UpdateAsync(input);
    if (!result.Success)
    {
      return await ErrorAsync(result.Message ?? "common.error.update_failed");
    }
    return Success(result.Data, LeanBusinessType.Update);
  }

  /// <summary>
  /// 删除通知
  /// </summary>
  [HttpDelete("{id}")]
  [LeanPermission("routine:notification:delete", "删除通知")]
  public async Task<IActionResult> DeleteAsync(long id)
  {
    var result = await _notificationService.DeleteAsync(id);
    if (!result.Success)
    {
      return await ErrorAsync(result.Message ?? "common.error.delete_failed");
    }
    return Success<object?>(null, LeanBusinessType.Delete);
  }

  /// <summary>
  /// 批量删除通知
  /// </summary>
  [HttpDelete("batch")]
  [LeanPermission("routine:notification:delete", "删除通知")]
  public async Task<IActionResult> BatchDeleteAsync([FromBody] List<long> ids)
  {
    var result = await _notificationService.BatchDeleteAsync(ids);
    if (!result.Success)
    {
      return await ErrorAsync(result.Message ?? "common.error.delete_failed");
    }
    return Success<object?>(null, LeanBusinessType.Delete);
  }

  /// <summary>
  /// 导入通知
  /// </summary>
  [HttpPost("import")]
  [LeanPermission("routine:notification:import", "导入通知")]
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
      var result = await _notificationService.ImportAsync(file);
      return Ok(result);
    }
    catch (Exception ex)
    {
      _logger.Error(ex, "导入通知失败");
      return Problem("导入通知失败：" + ex.Message);
    }
  }

  /// <summary>
  /// 导出通知
  /// </summary>
  [HttpGet("export")]
  [LeanPermission("routine:notification:export", "导出通知")]
  public async Task<IActionResult> ExportAsync([FromQuery] LeanNotificationQueryDto input)
  {
    var bytes = await _notificationService.ExportAsync(input);
    var fileName = $"notifications_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
    return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
  }

  /// <summary>
  /// 获取导入模板
  /// </summary>
  [HttpGet("template")]
  [LeanPermission("routine:notification:import", "导入通知")]
  public async Task<IActionResult> GetImportTemplateAsync()
  {
    var bytes = await _notificationService.GetImportTemplateAsync();
    var fileName = $"notification_template_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
    return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
  }

  /// <summary>
  /// 发布通知
  /// </summary>
  [HttpPost("{id}/publish")]
  [LeanPermission("routine:notification:publish", "发布通知")]
  public async Task<IActionResult> PublishAsync(long id)
  {
    var result = await _notificationService.PublishAsync(id);
    if (!result.Success)
    {
      return await ErrorAsync(result.Message ?? "common.error.publish_failed");
    }
    return Success<object?>(null, LeanBusinessType.Other);
  }

  /// <summary>
  /// 批量发布通知
  /// </summary>
  [HttpPost("batch/publish")]
  [LeanPermission("routine:notification:publish", "发布通知")]
  public async Task<IActionResult> BatchPublishAsync([FromBody] List<long> ids)
  {
    var result = await _notificationService.BatchPublishAsync(ids);
    if (!result.Success)
    {
      return await ErrorAsync(result.Message ?? "common.error.publish_failed");
    }
    return Success<object?>(null, LeanBusinessType.Other);
  }

  /// <summary>
  /// 撤回通知
  /// </summary>
  [HttpPost("{id}/withdraw")]
  [LeanPermission("routine:notification:withdraw", "撤回通知")]
  public async Task<IActionResult> WithdrawAsync(long id)
  {
    var result = await _notificationService.WithdrawAsync(id);
    if (!result.Success)
    {
      return await ErrorAsync(result.Message ?? "common.error.withdraw_failed");
    }
    return Success<object?>(null, LeanBusinessType.Other);
  }

  /// <summary>
  /// 批量撤回通知
  /// </summary>
  [HttpPost("batch/withdraw")]
  [LeanPermission("routine:notification:withdraw", "撤回通知")]
  public async Task<IActionResult> BatchWithdrawAsync([FromBody] List<long> ids)
  {
    var result = await _notificationService.BatchWithdrawAsync(ids);
    if (!result.Success)
    {
      return await ErrorAsync(result.Message ?? "common.error.withdraw_failed");
    }
    return Success<object?>(null, LeanBusinessType.Other);
  }

  /// <summary>
  /// 阅读通知
  /// </summary>
  [HttpPost("{id}/read")]
  [LeanPermission("routine:notification:read", "阅读通知")]
  public async Task<IActionResult> ReadAsync(long id)
  {
    var result = await _notificationService.ReadAsync(id);
    if (!result.Success)
    {
      return await ErrorAsync(result.Message ?? "common.error.read_failed");
    }
    return Success<object?>(null, LeanBusinessType.Other);
  }

  /// <summary>
  /// 确认通知
  /// </summary>
  [HttpPost("{id}/confirm")]
  [LeanPermission("routine:notification:confirm", "确认通知")]
  public async Task<IActionResult> ConfirmAsync(long id)
  {
    var result = await _notificationService.ConfirmAsync(id);
    if (!result.Success)
    {
      return await ErrorAsync(result.Message ?? "common.error.confirm_failed");
    }
    return Success<object?>(null, LeanBusinessType.Other);
  }
}