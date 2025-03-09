// -----------------------------------------------------------------------
// <copyright file="LeanMailService.cs" company="Lean">
// Copyright (c) Lean. All rights reserved.
// </copyright>
// <author>Lean</author>
// <created>2024-03-09</created>
// <summary>邮件服务实现</summary>
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
using Lean.CodeGen.Common.Extensions;
using Lean.CodeGen.Common.Exceptions;
using Lean.CodeGen.Common.Helpers;
using Mapster;
using SqlSugar;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Linq.Expressions;
using Lean.CodeGen.Common.Http;
using NLog;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Options;
using Lean.CodeGen.Common.Options;
using Microsoft.Extensions.Configuration;

namespace Lean.CodeGen.Application.Services.Routine;

/// <summary>
/// 邮件服务实现
/// </summary>
public class LeanMailService : LeanBaseService, ILeanMailService
{
  private readonly IConfiguration _configuration;
  private readonly ILogger _logger;
  private readonly ILeanRepository<LeanMail> _repository;
  private readonly LeanUniqueValidator<LeanMail> _uniqueValidator;
  private readonly ILeanLocalizationService _localizationService;
  private readonly ILeanHttpContextAccessor _httpContextAccessor;
  private readonly ILeanRepository<LeanFile> _fileRepository;
  private readonly LeanMailHelper _mailHelper;
  private readonly IOptions<LeanMailOptions> _options;
  private readonly string _smtpServer;
  private readonly int _smtpPort;
  private readonly string _smtpUsername;
  private readonly string _smtpPassword;
  private readonly string _fromAddress;
  private readonly string _fromName;

  /// <summary>
  /// 构造函数
  /// </summary>
  public LeanMailService(
      IConfiguration configuration,
      ILeanRepository<LeanMail> repository,
      LeanBaseServiceContext context,
      ILeanLocalizationService localizationService,
      ILeanRepository<LeanFile> fileRepository,
      ILeanHttpContextAccessor httpContextAccessor,
      LeanMailHelper mailHelper,
      IOptions<LeanMailOptions> options)
      : base(context)
  {
    _configuration = configuration;
    _repository = repository;
    _logger = context.Logger;
    _uniqueValidator = new LeanUniqueValidator<LeanMail>(_repository);
    _localizationService = localizationService;
    _fileRepository = fileRepository;
    _httpContextAccessor = httpContextAccessor;
    _mailHelper = mailHelper;

    // 从配置中读取SMTP设置
    _smtpServer = _configuration["Mail:SmtpServer"] ?? "smtp.office365.com";
    _smtpPort = int.Parse(_configuration["Mail:SmtpPort"] ?? "587");
    _smtpUsername = _configuration["Mail:Username"] ?? "";
    _smtpPassword = _configuration["Mail:Password"] ?? "";
    _fromAddress = _configuration["Mail:FromAddress"] ?? "";
    _fromName = _configuration["Mail:FromName"] ?? "Lean CodeGen";
  }

  private async Task<string> GetCurrentLanguageAsync()
  {
    var langCode = _httpContextAccessor.GetCurrentLanguage();
    var supportedLanguages = await _localizationService.GetSupportedLanguagesAsync();
    return supportedLanguages.Contains(langCode) ? langCode : "en-US";
  }

  /// <summary>
  /// 构建查询条件
  /// </summary>
  private Expression<Func<LeanMail, bool>> BuildQueryPredicate(LeanMailQueryDto input)
  {
    Expression<Func<LeanMail, bool>> predicate = x => true;

    if (!string.IsNullOrEmpty(input.Subject))
    {
      predicate = predicate.And(x => x.Subject.Contains(input.Subject));
    }

    if (!string.IsNullOrEmpty(input.FromAddress))
    {
      predicate = predicate.And(x => x.FromAddress.Contains(input.FromAddress));
    }

    if (!string.IsNullOrEmpty(input.ToAddress))
    {
      predicate = predicate.And(x => x.ToAddresses.Contains(input.ToAddress));
    }

    if (input.SendStatus.HasValue)
    {
      predicate = predicate.And(x => x.SendStatus == input.SendStatus.Value);
    }

    if (input.SendTimeStart.HasValue)
    {
      predicate = predicate.And(x => x.SendTime >= input.SendTimeStart.Value);
    }

    if (input.SendTimeEnd.HasValue)
    {
      predicate = predicate.And(x => x.SendTime <= input.SendTimeEnd.Value);
    }

    return predicate;
  }

  /// <summary>
  /// 创建邮件
  /// </summary>
  public async Task<LeanApiResult<LeanMailDto>> CreateAsync(LeanMailCreateDto input)
  {
    try
    {
      var entity = input.Adapt<LeanMail>();
      entity.SendStatus = 0; // 待发送
      await _repository.CreateAsync(entity);

      return LeanApiResult<LeanMailDto>.Ok(entity.Adapt<LeanMailDto>());
    }
    catch (Exception ex)
    {
      _logger.Error(ex, "创建邮件失败");
      var langCode = await GetCurrentLanguageAsync();
      var message = await _localizationService.GetTranslationAsync(langCode, "mail.error.create_failed");
      return LeanApiResult<LeanMailDto>.Error(message);
    }
  }

  /// <summary>
  /// 获取邮件列表
  /// </summary>
  public async Task<LeanApiResult<LeanPageResult<LeanMailDto>>> GetListAsync(LeanMailQueryDto input)
  {
    try
    {
      Expression<Func<LeanMail, bool>> predicate = x => true;

      if (!string.IsNullOrEmpty(input.Subject))
      {
        var subject = CleanInput(input.Subject);
        predicate = LeanExpressionExtensions.And(predicate, x => x.Subject.Contains(subject));
      }

      if (!string.IsNullOrEmpty(input.FromAddress))
      {
        var fromAddress = CleanInput(input.FromAddress);
        predicate = LeanExpressionExtensions.And(predicate, x => x.FromAddress.Contains(fromAddress));
      }

      if (!string.IsNullOrEmpty(input.ToAddress))
      {
        var toAddress = CleanInput(input.ToAddress);
        predicate = LeanExpressionExtensions.And(predicate, x => x.ToAddresses.Contains(toAddress));
      }

      if (input.SendStatus.HasValue)
      {
        predicate = LeanExpressionExtensions.And(predicate, x => x.SendStatus == input.SendStatus.Value);
      }

      if (input.SendTimeStart.HasValue)
      {
        predicate = LeanExpressionExtensions.And(predicate, x => x.SendTime >= input.SendTimeStart.Value);
      }

      if (input.SendTimeEnd.HasValue)
      {
        predicate = LeanExpressionExtensions.And(predicate, x => x.SendTime <= input.SendTimeEnd.Value);
      }

      var (total, items) = await _repository.GetPageListAsync(predicate, input.PageSize, input.PageIndex);
      var dtos = items.Select(t => t.Adapt<LeanMailDto>()).ToList();

      var result = new LeanPageResult<LeanMailDto>
      {
        Total = total,
        Items = dtos,
        PageIndex = input.PageIndex,
        PageSize = input.PageSize
      };

      return LeanApiResult<LeanPageResult<LeanMailDto>>.Ok(result);
    }
    catch (Exception ex)
    {
      _logger.Error(ex, "获取邮件列表失败");
      var langCode = await GetCurrentLanguageAsync();
      var message = await _localizationService.GetTranslationAsync(langCode, "mail.error.query_failed");
      return LeanApiResult<LeanPageResult<LeanMailDto>>.Error(message);
    }
  }

  /// <summary>
  /// 获取邮件详情
  /// </summary>
  public async Task<LeanApiResult<LeanMailDto>> GetAsync(long id)
  {
    try
    {
      var entity = await _repository.GetByIdAsync(id);
      if (entity == null)
      {
        var langCode = await GetCurrentLanguageAsync();
        var message = await _localizationService.GetTranslationAsync(langCode, "mail.error.not_found");
        return LeanApiResult<LeanMailDto>.Error(message);
      }

      return LeanApiResult<LeanMailDto>.Ok(entity.Adapt<LeanMailDto>());
    }
    catch (Exception ex)
    {
      _logger.Error(ex, "获取邮件详情失败");
      var langCode = await GetCurrentLanguageAsync();
      var message = await _localizationService.GetTranslationAsync(langCode, "mail.error.get_failed");
      return LeanApiResult<LeanMailDto>.Error(message);
    }
  }

  /// <summary>
  /// 更新邮件
  /// </summary>
  public async Task<LeanApiResult<LeanMailDto>> UpdateAsync(LeanMailUpdateDto input)
  {
    try
    {
      var entity = await _repository.GetByIdAsync(input.Id);
      if (entity == null)
      {
        var langCode = await GetCurrentLanguageAsync();
        var message = await _localizationService.GetTranslationAsync(langCode, "mail.error.not_found");
        return LeanApiResult<LeanMailDto>.Error(message);
      }

      input.Adapt(entity);
      await _repository.UpdateAsync(entity);
      return LeanApiResult<LeanMailDto>.Ok(entity.Adapt<LeanMailDto>());
    }
    catch (Exception ex)
    {
      _logger.Error(ex, "更新邮件失败");
      var langCode = await GetCurrentLanguageAsync();
      var message = await _localizationService.GetTranslationAsync(langCode, "mail.error.update_failed");
      return LeanApiResult<LeanMailDto>.Error(message);
    }
  }

  /// <summary>
  /// 删除邮件
  /// </summary>
  public async Task<LeanApiResult> DeleteAsync(long id)
  {
    try
    {
      var entity = await _repository.GetByIdAsync(id);
      if (entity == null)
      {
        var langCode = await GetCurrentLanguageAsync();
        var message = await _localizationService.GetTranslationAsync(langCode, "mail.error.not_found");
        return LeanApiResult.Error(message);
      }

      await _repository.DeleteAsync(entity);
      return LeanApiResult.Ok();
    }
    catch (Exception ex)
    {
      _logger.Error(ex, "删除邮件失败");
      var langCode = await GetCurrentLanguageAsync();
      var message = await _localizationService.GetTranslationAsync(langCode, "mail.error.delete_failed");
      return LeanApiResult.Error(message);
    }
  }

  /// <summary>
  /// 批量删除邮件
  /// </summary>
  public async Task<LeanApiResult> BatchDeleteAsync(List<long> ids)
  {
    try
    {
      var entities = await _repository.GetListAsync(x => ids.Contains(x.Id));
      if (!entities.Any())
      {
        var langCode = await GetCurrentLanguageAsync();
        var message = await _localizationService.GetTranslationAsync(langCode, "mail.error.not_found");
        return LeanApiResult.Error(message);
      }

      // 批量删除
      foreach (var entity in entities)
      {
        await _repository.DeleteAsync(entity);
      }
      return LeanApiResult.Ok();
    }
    catch (Exception ex)
    {
      _logger.Error(ex, "批量删除邮件失败");
      var langCode = await GetCurrentLanguageAsync();
      var message = await _localizationService.GetTranslationAsync(langCode, "mail.error.batch_delete_failed");
      return LeanApiResult.Error(message);
    }
  }

  /// <summary>
  /// 导入邮件
  /// </summary>
  public async Task<LeanMailImportResultDto> ImportAsync(LeanFileInfo file)
  {
    var result = new LeanMailImportResultDto();
    try
    {
      // 验证文件
      if (file == null || file.Stream == null || file.Stream.Length == 0)
      {
        var langCode = await GetCurrentLanguageAsync();
        var message = await _localizationService.GetTranslationAsync(langCode, "common.error.file_required");
        result.AddError("", message);
        return result;
      }

      // 读取文件内容
      var bytes = new byte[file.Stream.Length];
      await file.Stream.ReadAsync(bytes, 0, (int)file.Stream.Length);

      // 导入Excel数据
      var importResult = LeanExcelHelper.Import<LeanMailImportDto>(bytes);

      foreach (var item in importResult.Data)
      {
        try
        {
          // 创建实体
          var entity = item.Adapt<LeanMail>();
          entity.SendStatus = 0; // 待发送
          await _repository.CreateAsync(entity);
          result.SuccessCount++;
          result.TotalCount++;
        }
        catch (Exception ex)
        {
          result.AddError(item.Subject, ex.Message);
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
      _logger.Error(ex, "导入邮件失败");
      result.ErrorMessage = ex.Message;
      return result;
    }
  }

  /// <summary>
  /// 导出邮件
  /// </summary>
  public async Task<byte[]> ExportAsync(LeanMailQueryDto input)
  {
    try
    {
      // 构建查询条件
      var predicate = BuildQueryPredicate(input);

      // 查询数据
      var list = await _repository.GetListAsync(predicate);
      var dtos = list.Select(t => t.Adapt<LeanMailExportDto>()).ToList();

      // 导出Excel
      return LeanExcelHelper.Export(dtos);
    }
    catch (Exception ex)
    {
      _logger.Error(ex, "导出邮件失败");
      throw new LeanException("导出邮件失败：" + ex.Message);
    }
  }

  /// <summary>
  /// 获取导入模板
  /// </summary>
  public Task<byte[]> GetImportTemplateAsync()
  {
    try
    {
      return Task.FromResult(LeanExcelHelper.GetImportTemplate<LeanMailImportDto>());
    }
    catch (Exception ex)
    {
      _logger.Error(ex, "获取导入模板失败");
      throw new LeanException("获取导入模板失败：" + ex.Message);
    }
  }

  /// <summary>
  /// 发送邮件
  /// </summary>
  public async Task<LeanApiResult> SendAsync(string to, string subject, string body, bool isHtml = true)
  {
    return await SendAsync(new[] { to }, subject, body, isHtml);
  }

  /// <summary>
  /// 发送邮件
  /// </summary>
  public async Task<LeanApiResult> SendAsync(string[] to, string subject, string body, bool isHtml = true)
  {
    try
    {
      using var client = new SmtpClient();
      await client.ConnectAsync(_smtpServer, _smtpPort, SecureSocketOptions.StartTls);
      await client.AuthenticateAsync(_smtpUsername, _smtpPassword);

      var message = new MimeMessage();
      message.From.Add(new MailboxAddress(_fromName, _fromAddress));
      message.Subject = subject;

      foreach (var recipient in to)
      {
        message.To.Add(MailboxAddress.Parse(recipient));
      }

      message.Body = new TextPart(isHtml ? "html" : "plain")
      {
        Text = body
      };

      await client.SendAsync(message);
      await client.DisconnectAsync(true);
      return LeanApiResult.Ok();
    }
    catch (Exception ex)
    {
      _logger.Error(ex, "发送邮件失败");
      return LeanApiResult.Error("发送邮件失败：" + ex.Message);
    }
  }

  /// <summary>
  /// 批量发送邮件
  /// </summary>
  public async Task<LeanApiResult> BatchSendAsync(List<long> ids)
  {
    try
    {
      // 获取邮件列表
      var mails = await _repository.GetListAsync(x => ids.Contains(x.Id));
      if (!mails.Any())
      {
        throw new LeanException("未找到要发送的邮件");
      }

      // 检查状态
      var invalidMails = mails.Where(x => x.SendStatus != 0).ToList();
      if (invalidMails.Any())
      {
        throw new LeanException($"以下邮件已发送: {string.Join(",", invalidMails.Select(x => x.Id))}");
      }

      // 发送邮件
      var errors = new List<string>();
      foreach (var mail in mails)
      {
        try
        {
          // 获取附件
          string[] attachmentPaths = Array.Empty<string>();
          if (!string.IsNullOrEmpty(mail.AttachmentIds))
          {
            var attachmentIds = mail.AttachmentIds.Split(',').Select(long.Parse).ToList();
            var attachments = await _fileRepository.GetListAsync(x => attachmentIds.Contains(x.Id));
            attachmentPaths = attachments.Select(x => Path.Combine(_httpContextAccessor.WebRootPath, x.FilePath))
              .Where(File.Exists)
              .ToArray();
          }

          // 发送邮件
          bool success;
          if (attachmentPaths.Length > 0)
          {
            success = await _mailHelper.SendWithAttachmentsAsync(
              mail.FromAddress,
              mail.FromName,
              mail.ToAddresses,
              mail.Subject,
              mail.Body,
              attachmentPaths,
              mail.IsBodyHtml == 1);
          }
          else
          {
            success = await _mailHelper.SendAsync(
              mail.FromAddress,
              mail.FromName,
              mail.ToAddresses,
              mail.Subject,
              mail.Body,
              mail.IsBodyHtml == 1);
          }

          // 更新状态
          mail.SendStatus = success ? 1 : 2;
          mail.SendTime = success ? DateTime.Now : null;
          mail.FailureReason = success ? null : "发送失败";
          await _repository.UpdateAsync(mail);

          if (!success)
          {
            errors.Add($"邮件 {mail.Id}: 发送失败");
          }
        }
        catch (Exception ex)
        {
          _logger.Error(ex, $"发送邮件失败: {mail.Id}");
          mail.SendStatus = 2;
          mail.FailureReason = ex.Message;
          await _repository.UpdateAsync(mail);
          errors.Add($"邮件 {mail.Id}: {ex.Message}");
        }
      }

      if (errors.Any())
      {
        return LeanApiResult.Error($"部分邮件发送失败: {string.Join("; ", errors)}");
      }

      return LeanApiResult.Ok();
    }
    catch (Exception ex)
    {
      _logger.Error(ex, "批量发送邮件失败");
      throw new LeanException("批量发送邮件失败：" + ex.Message);
    }
  }

  /// <summary>
  /// 撤回邮件
  /// </summary>
  public async Task<LeanApiResult> WithdrawAsync(long id)
  {
    try
    {
      // 获取邮件
      var mail = await _repository.GetByIdAsync(id);
      if (mail == null)
      {
        throw new LeanException("邮件不存在");
      }

      // 检查状态
      if (mail.SendStatus == 0)
      {
        throw new LeanException("邮件未发送");
      }

      // 更新状态
      mail.SendStatus = 0;
      mail.SendTime = null;
      mail.FailureReason = null;
      await _repository.UpdateAsync(mail);

      return LeanApiResult.Ok();
    }
    catch (Exception ex)
    {
      _logger.Error(ex, "撤回邮件失败");
      throw new LeanException("撤回邮件失败：" + ex.Message);
    }
  }

  /// <summary>
  /// 批量撤回邮件
  /// </summary>
  public async Task<LeanApiResult> BatchWithdrawAsync(List<long> ids)
  {
    try
    {
      // 获取邮件列表
      var mails = await _repository.GetListAsync(x => ids.Contains(x.Id));
      if (!mails.Any())
      {
        throw new LeanException("未找到要撤回的邮件");
      }

      // 检查状态
      var invalidMails = mails.Where(x => x.SendStatus == 0).ToList();
      if (invalidMails.Any())
      {
        throw new LeanException($"以下邮件未发送: {string.Join(",", invalidMails.Select(x => x.Id))}");
      }

      // 更新状态
      foreach (var mail in mails)
      {
        mail.SendStatus = 0;
        mail.SendTime = null;
        mail.FailureReason = null;
        await _repository.UpdateAsync(mail);
      }

      return LeanApiResult.Ok();
    }
    catch (Exception ex)
    {
      _logger.Error(ex, "批量撤回邮件失败");
      throw new LeanException("批量撤回邮件失败：" + ex.Message);
    }
  }
}