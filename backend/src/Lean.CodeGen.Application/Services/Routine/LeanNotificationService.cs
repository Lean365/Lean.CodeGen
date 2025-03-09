// -----------------------------------------------------------------------
// <copyright file="LeanNotificationService.cs" company="Lean">
// Copyright (c) Lean. All rights reserved.
// </copyright>
// <author>Lean</author>
// <created>2024-03-09</created>
// <summary>通知服务实现</summary>
// -----------------------------------------------------------------------

using Lean.CodeGen.Application.Dtos.Routine;
using Lean.CodeGen.Domain.Entities.Routine;
using Lean.CodeGen.Application.Services.Base;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Domain.Interfaces.Repositories;
using Lean.CodeGen.Domain.Validators;
using Lean.CodeGen.Application.Services.Security;
using Lean.CodeGen.Application.Services.Admin;
using Lean.CodeGen.Common.Excel;
using Lean.CodeGen.Application.Hubs;
using Mapster;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Microsoft.AspNetCore.SignalR;

namespace Lean.CodeGen.Application.Services.Routine;

/// <summary>
/// 通知服务实现
/// </summary>
public class LeanNotificationService : LeanBaseService, ILeanNotificationService
{
  private readonly ILeanRepository<LeanNotification> _repository;
  private readonly LeanUniqueValidator<LeanNotification> _uniqueValidator;
  private readonly ILeanLocalizationService _localizationService;
  private readonly ILogger _logger;
  private readonly IHubContext<LeanNotificationHub> _hubContext;
  private readonly ILeanMailService _mailService;

  /// <summary>
  /// 初始化通知服务实现
  /// </summary>
  /// <param name="repository">通知仓储</param>
  /// <param name="context">服务上下文</param>
  /// <param name="localizationService">本地化服务</param>
  /// <param name="hubContext">SignalR Hub上下文</param>
  /// <param name="mailService">邮件服务</param>
  public LeanNotificationService(
      ILeanRepository<LeanNotification> repository,
      LeanBaseServiceContext context,
      ILeanLocalizationService localizationService,
      IHubContext<LeanNotificationHub> hubContext,
      ILeanMailService mailService)
      : base(context)
  {
    _repository = repository;
    _uniqueValidator = new LeanUniqueValidator<LeanNotification>(_repository);
    _localizationService = localizationService;
    _logger = LogManager.GetCurrentClassLogger();
    _hubContext = hubContext;
    _mailService = mailService;
  }

  /// <inheritdoc/>
  public async Task<LeanApiResult<LeanPageResult<LeanNotificationDto>>> GetListAsync(LeanNotificationQueryDto query)
  {
    try
    {
      var result = new LeanPageResult<LeanNotificationDto>();
      var list = await _repository.GetPageListAsync(t =>
          (!string.IsNullOrEmpty(query.Title) ? t.NotificationTitle.Contains(query.Title) : true) &&
          (query.Type.HasValue ? t.NotificationType == query.Type : true) &&
          (query.Status.HasValue ? t.PublishStatus == query.Status : true) &&
          (query.SendTimeStart.HasValue ? t.PublishTime >= query.SendTimeStart : true) &&
          (query.SendTimeEnd.HasValue ? t.PublishTime <= query.SendTimeEnd : true),
          query.PageIndex,
          query.PageSize,
          t => t.CreateTime,
          false);

      result.Items = list.Items.Adapt<List<LeanNotificationDto>>();
      result.Total = list.Total;
      return LeanApiResult<LeanPageResult<LeanNotificationDto>>.Ok(result);
    }
    catch (Exception ex)
    {
      Logger.Error(ex, "获取通知列表失败");
      var langCode = await GetCurrentLanguageAsync();
      var message = await _localizationService.GetTranslationAsync(langCode, "notification.error.get_list_failed");
      return LeanApiResult<LeanPageResult<LeanNotificationDto>>.Error(message);
    }
  }

  /// <inheritdoc/>
  public async Task<LeanApiResult<LeanNotificationDto>> GetAsync(long id)
  {
    try
    {
      var entity = await _repository.FirstOrDefaultAsync(t => t.Id == id);
      if (entity == null)
      {
        var langCode = await GetCurrentLanguageAsync();
        var message = await _localizationService.GetTranslationAsync(langCode, "notification.error.not_found");
        return LeanApiResult<LeanNotificationDto>.Error(message);
      }

      return LeanApiResult<LeanNotificationDto>.Ok(entity.Adapt<LeanNotificationDto>());
    }
    catch (Exception ex)
    {
      Logger.Error(ex, "获取通知详情失败");
      var langCode = await GetCurrentLanguageAsync();
      var message = await _localizationService.GetTranslationAsync(langCode, "notification.error.get_failed");
      return LeanApiResult<LeanNotificationDto>.Error(message);
    }
  }

  /// <inheritdoc/>
  public async Task<LeanApiResult<LeanNotificationDto>> CreateAsync(LeanNotificationCreateDto input)
  {
    try
    {
      var entity = input.Adapt<LeanNotification>();
      await _repository.CreateAsync(entity);
      var dto = entity.Adapt<LeanNotificationDto>();

      // 发送实时通知
      if (!string.IsNullOrEmpty(input.ReceiverIds))
      {
        var userIds = input.ReceiverIds.Split(';');
        foreach (var userId in userIds)
        {
          await _hubContext.Clients.Group($"User_{userId}")
            .SendAsync("ReceiveNotification", dto);
        }
      }

      // 发送邮件通知
      if (input.SendEmail && !string.IsNullOrEmpty(input.ReceiverEmails))
      {
        var emails = input.ReceiverEmails.Split(';');
        await _mailService.SendAsync(emails, input.Title, input.Content);
      }

      return LeanApiResult<LeanNotificationDto>.Ok(dto);
    }
    catch (Exception ex)
    {
      Logger.Error(ex, "创建通知失败");
      var langCode = await GetCurrentLanguageAsync();
      var message = await _localizationService.GetTranslationAsync(langCode, "notification.error.create_failed");
      return LeanApiResult<LeanNotificationDto>.Error(message);
    }
  }

  /// <inheritdoc/>
  public async Task<LeanApiResult<LeanNotificationDto>> UpdateAsync(LeanNotificationUpdateDto input)
  {
    try
    {
      var entity = await _repository.FirstOrDefaultAsync(t => t.Id == input.Id);
      if (entity == null)
      {
        var langCode = await GetCurrentLanguageAsync();
        var message = await _localizationService.GetTranslationAsync(langCode, "notification.error.not_found");
        return LeanApiResult<LeanNotificationDto>.Error(message);
      }

      input.Adapt(entity);
      await _repository.UpdateAsync(entity);
      return LeanApiResult<LeanNotificationDto>.Ok(entity.Adapt<LeanNotificationDto>());
    }
    catch (Exception ex)
    {
      Logger.Error(ex, "更新通知失败");
      var langCode = await GetCurrentLanguageAsync();
      var message = await _localizationService.GetTranslationAsync(langCode, "notification.error.update_failed");
      return LeanApiResult<LeanNotificationDto>.Error(message);
    }
  }

  /// <inheritdoc/>
  public async Task<LeanApiResult> DeleteAsync(long id)
  {
    try
    {
      await _repository.DeleteAsync(t => t.Id == id);
      return LeanApiResult.Ok();
    }
    catch (Exception ex)
    {
      Logger.Error(ex, "删除通知失败");
      var langCode = await GetCurrentLanguageAsync();
      var message = await _localizationService.GetTranslationAsync(langCode, "notification.error.delete_failed");
      return LeanApiResult.Error(message);
    }
  }

  /// <inheritdoc/>
  public async Task<LeanApiResult> BatchDeleteAsync(List<long> ids)
  {
    try
    {
      await _repository.DeleteAsync(t => ids.Contains(t.Id));
      return LeanApiResult.Ok();
    }
    catch (Exception ex)
    {
      Logger.Error(ex, "批量删除通知失败");
      var langCode = await GetCurrentLanguageAsync();
      var message = await _localizationService.GetTranslationAsync(langCode, "notification.error.batch_delete_failed");
      return LeanApiResult.Error(message);
    }
  }

  /// <inheritdoc/>
  public async Task<LeanNotificationImportResultDto> ImportAsync(LeanFileInfo file)
  {
    var result = new LeanNotificationImportResultDto();

    try
    {
      // 验证文件
      if (file == null || file.Stream == null || file.Stream.Length == 0)
      {
        result.AddError("", "请选择要导入的文件");
        return result;
      }

      // 读取文件内容
      var bytes = new byte[file.Stream.Length];
      await file.Stream.ReadAsync(bytes, 0, (int)file.Stream.Length);

      // 导入Excel数据
      var importResult = LeanExcelHelper.Import<LeanNotificationImportDto>(bytes);

      foreach (var item in importResult.Data)
      {
        try
        {
          // 创建实体
          var entity = item.Adapt<LeanNotification>();
          entity.PublishStatus = 0; // 待发送
          await _repository.CreateAsync(entity);
          result.SuccessCount++;
          result.TotalCount++;
        }
        catch (Exception ex)
        {
          result.AddError(item.Title, ex.Message);
        }
      }

      // 添加Excel导入的错误信息
      foreach (var error in importResult.Errors)
      {
        result.AddError($"第{error.RowIndex}行", error.ErrorMessage);
      }

      return result;
    }
    catch (Exception ex)
    {
      _logger.Error(ex, "导入通知失败");
      result.ErrorMessage = ex.Message;
      return result;
    }
  }

  /// <inheritdoc/>
  public async Task<byte[]> ExportAsync(LeanNotificationQueryDto query)
  {
    try
    {
      var list = await _repository.GetListAsync(t =>
          (!string.IsNullOrEmpty(query.Title) ? t.NotificationTitle.Contains(query.Title) : true) &&
          (query.Type.HasValue ? t.NotificationType == query.Type : true) &&
          (query.Status.HasValue ? t.PublishStatus == query.Status : true) &&
          (query.SendTimeStart.HasValue ? t.PublishTime >= query.SendTimeStart : true) &&
          (query.SendTimeEnd.HasValue ? t.PublishTime <= query.SendTimeEnd : true));

      var exportData = list.Select(x => new LeanNotificationExportDto
      {
        Title = x.NotificationTitle,
        Content = x.NotificationContent,
        Type = x.NotificationType,
        Priority = x.NotificationLevel,
        ReceiverNames = x.ReceiverIds ?? string.Empty,
        SendStatusName = GetSendStatusName(x.PublishStatus),
        SendTime = x.PublishTime,
        FailureReason = string.Empty,
        CreateTime = x.CreateTime
      }).ToList();
      return LeanExcelHelper.Export(exportData);
    }
    catch (Exception ex)
    {
      Logger.Error(ex, "导出通知数据失败");
      throw;
    }
  }

  private string GetSendStatusName(int status)
  {
    return status switch
    {
      0 => "待发送",
      1 => "已发送",
      2 => "已撤回",
      _ => "未知"
    };
  }

  /// <inheritdoc/>
  public async Task<LeanApiResult> PublishAsync(long id)
  {
    try
    {
      var entity = await _repository.FirstOrDefaultAsync(t => t.Id == id);
      if (entity == null)
      {
        var langCode = await GetCurrentLanguageAsync();
        var message = await _localizationService.GetTranslationAsync(langCode, "notification.error.not_found");
        return LeanApiResult.Error(message);
      }

      if (entity.PublishStatus != 0)
      {
        var langCode = await GetCurrentLanguageAsync();
        var message = await _localizationService.GetTranslationAsync(langCode, "notification.error.already_published");
        return LeanApiResult.Error(message);
      }

      entity.PublishStatus = 1;
      entity.PublishTime = DateTime.Now;
      await _repository.UpdateAsync(entity);

      // 发送实时通知
      var dto = entity.Adapt<LeanNotificationDto>();
      if (!string.IsNullOrEmpty(entity.ReceiverIds))
      {
        var userIds = entity.ReceiverIds.Split(';');
        foreach (var userId in userIds)
        {
          await _hubContext.Clients.Group($"User_{userId}")
            .SendAsync("ReceiveNotification", dto);
        }
      }

      return LeanApiResult.Ok();
    }
    catch (Exception ex)
    {
      Logger.Error(ex, "发布通知失败");
      var langCode = await GetCurrentLanguageAsync();
      var message = await _localizationService.GetTranslationAsync(langCode, "notification.error.publish_failed");
      return LeanApiResult.Error(message);
    }
  }

  /// <inheritdoc/>
  public async Task<LeanApiResult> BatchPublishAsync(List<long> ids)
  {
    try
    {
      var entities = await _repository.GetListAsync(t => ids.Contains(t.Id) && t.PublishStatus == 0);
      if (!entities.Any())
      {
        var langCode = await GetCurrentLanguageAsync();
        var message = await _localizationService.GetTranslationAsync(langCode, "notification.error.no_unpublished");
        return LeanApiResult.Error(message);
      }

      foreach (var entity in entities)
      {
        entity.PublishStatus = 1;
        entity.PublishTime = DateTime.Now;
      }

      await _repository.UpdateRangeAsync(entities);
      return LeanApiResult.Ok();
    }
    catch (Exception ex)
    {
      Logger.Error(ex, "批量发布通知失败");
      var langCode = await GetCurrentLanguageAsync();
      var message = await _localizationService.GetTranslationAsync(langCode, "notification.error.batch_publish_failed");
      return LeanApiResult.Error(message);
    }
  }

  /// <inheritdoc/>
  public async Task<LeanApiResult> WithdrawAsync(long id)
  {
    try
    {
      var entity = await _repository.FirstOrDefaultAsync(t => t.Id == id);
      if (entity == null)
      {
        var langCode = await GetCurrentLanguageAsync();
        var message = await _localizationService.GetTranslationAsync(langCode, "notification.error.not_found");
        return LeanApiResult.Error(message);
      }

      if (entity.PublishStatus != 1)
      {
        var langCode = await GetCurrentLanguageAsync();
        var message = await _localizationService.GetTranslationAsync(langCode, "notification.error.not_published");
        return LeanApiResult.Error(message);
      }

      entity.PublishStatus = 2;
      await _repository.UpdateAsync(entity);
      return LeanApiResult.Ok();
    }
    catch (Exception ex)
    {
      Logger.Error(ex, "撤回通知失败");
      var langCode = await GetCurrentLanguageAsync();
      var message = await _localizationService.GetTranslationAsync(langCode, "notification.error.withdraw_failed");
      return LeanApiResult.Error(message);
    }
  }

  /// <inheritdoc/>
  public async Task<LeanApiResult> BatchWithdrawAsync(List<long> ids)
  {
    try
    {
      var entities = await _repository.GetListAsync(t => ids.Contains(t.Id) && t.PublishStatus == 1);
      if (!entities.Any())
      {
        var langCode = await GetCurrentLanguageAsync();
        var message = await _localizationService.GetTranslationAsync(langCode, "notification.error.no_published");
        return LeanApiResult.Error(message);
      }

      foreach (var entity in entities)
      {
        entity.PublishStatus = 2;
      }

      await _repository.UpdateRangeAsync(entities);
      return LeanApiResult.Ok();
    }
    catch (Exception ex)
    {
      Logger.Error(ex, "批量撤回通知失败");
      var langCode = await GetCurrentLanguageAsync();
      var message = await _localizationService.GetTranslationAsync(langCode, "notification.error.batch_withdraw_failed");
      return LeanApiResult.Error(message);
    }
  }

  /// <inheritdoc/>
  public async Task<LeanApiResult> ReadAsync(long id)
  {
    try
    {
      var entity = await _repository.FirstOrDefaultAsync(t => t.Id == id);
      if (entity == null)
      {
        var langCode = await GetCurrentLanguageAsync();
        var message = await _localizationService.GetTranslationAsync(langCode, "notification.error.not_found");
        return LeanApiResult.Error(message);
      }

      entity.ReadCount++;
      await _repository.UpdateAsync(entity);
      return LeanApiResult.Ok();
    }
    catch (Exception ex)
    {
      Logger.Error(ex, "阅读通知失败");
      var langCode = await GetCurrentLanguageAsync();
      var message = await _localizationService.GetTranslationAsync(langCode, "notification.error.read_failed");
      return LeanApiResult.Error(message);
    }
  }

  /// <inheritdoc/>
  public async Task<LeanApiResult> ConfirmAsync(long id)
  {
    try
    {
      var entity = await _repository.FirstOrDefaultAsync(t => t.Id == id);
      if (entity == null)
      {
        var langCode = await GetCurrentLanguageAsync();
        var message = await _localizationService.GetTranslationAsync(langCode, "notification.error.not_found");
        return LeanApiResult.Error(message);
      }

      entity.ConfirmationCount++;
      await _repository.UpdateAsync(entity);
      return LeanApiResult.Ok();
    }
    catch (Exception ex)
    {
      Logger.Error(ex, "确认通知失败");
      var langCode = await GetCurrentLanguageAsync();
      var message = await _localizationService.GetTranslationAsync(langCode, "notification.error.confirm_failed");
      return LeanApiResult.Error(message);
    }
  }

  /// <inheritdoc/>
  public async Task<byte[]> GetImportTemplateAsync()
  {
    try
    {
      var template = new List<LeanNotificationImportDto>
      {
        new()
        {
          Title = "示例通知",
          Content = "这是一个示例通知内容",
          Type = 1,
          Priority = 2,
          ReceiverIds = "1;2;3",
          ReceiverNames = "用户1;用户2;用户3",
          Status = 1
        }
      };
      return LeanExcelHelper.Export(template);
    }
    catch (Exception ex)
    {
      Logger.Error(ex, "获取导入模板失败");
      throw;
    }
  }

  /// <summary>
  /// 获取当前语言代码
  /// </summary>
  private async Task<string> GetCurrentLanguageAsync()
  {
    return "zh-CN"; // TODO: 从请求上下文获取当前语言代码
  }
}