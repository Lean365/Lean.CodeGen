using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using NLog;
using Microsoft.Extensions.Options;
using Mapster;
using Lean.CodeGen.Common.Options;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Domain.Entities.Signalr;
using Lean.CodeGen.Domain.Interfaces.Repositories;
using Lean.CodeGen.Application.Services.Base;
using Lean.CodeGen.Application.Services.Security;
using Lean.CodeGen.Application.Dtos.Signalr;
using System.Linq.Expressions;
using System.IO;
using Lean.CodeGen.Common.Extensions;
using Microsoft.Extensions.Logging;

namespace Lean.CodeGen.Application.Services.Signalr;

/// <summary>
/// 在线消息服务实现
/// </summary>
/// <remarks>
/// 提供在线消息管理功能，包括：
/// 1. 消息发送和接收
/// 2. 未读消息管理
/// 3. 消息历史查询
/// 4. 消息状态更新
/// 5. 消息清理
/// </remarks>
public class LeanOnlineMessageService : LeanBaseService, ILeanOnlineMessageService
{
  private readonly ILeanRepository<LeanOnlineMessage> _messageRepository;
  private readonly ILogger<LeanOnlineMessageService> _logger;

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="messageRepository">在线消息仓储接口</param>
  /// <param name="context">基础服务上下文</param>
  /// <param name="logger">日志记录器</param>
  public LeanOnlineMessageService(
      ILeanRepository<LeanOnlineMessage> messageRepository,
      LeanBaseServiceContext context,
      ILogger<LeanOnlineMessageService> logger)
      : base(context)
  {
    _messageRepository = messageRepository;
    _logger = logger;
  }

  /// <summary>
  /// 创建消息
  /// </summary>
  public async Task CreateMessageAsync(LeanOnlineMessage message)
  {
    message.CreateTime = DateTime.Now;
    message.UpdateTime = DateTime.Now;
    await _messageRepository.CreateAsync(message);

    _logger.LogInformation($"创建消息成功，MessageId: {message.Id}, SenderId: {message.SenderId}, ReceiverId: {message.ReceiverId}");
  }

  /// <summary>
  /// 获取未读消息
  /// </summary>
  public async Task<List<LeanOnlineMessageDto>> GetUnreadMessagesAsync(long userId)
  {
    var messages = await _messageRepository.GetListAsync(
      x => x.ReceiverId == userId && x.IsRead == 0
    );

    return messages.OrderByDescending(x => x.CreateTime)
                  .Select(x => x.Adapt<LeanOnlineMessageDto>())
                  .ToList();
  }

  /// <summary>
  /// 标记消息为已读
  /// </summary>
  public async Task MarkMessageAsReadAsync(long messageId)
  {
    var message = await _messageRepository.FirstOrDefaultAsync(x => x.Id == messageId);
    if (message != null)
    {
      message.IsRead = 1;
      message.UpdateTime = DateTime.Now;
      await _messageRepository.UpdateAsync(message);

      _logger.LogInformation($"标记消息已读成功，MessageId: {messageId}");
    }
  }

  /// <summary>
  /// 获取消息详情
  /// </summary>
  public async Task<LeanOnlineMessageDto> GetMessageByIdAsync(long messageId)
  {
    var message = await _messageRepository.FirstOrDefaultAsync(x => x.Id == messageId);
    return message.Adapt<LeanOnlineMessageDto>();
  }

  /// <summary>
  /// 发送消息
  /// </summary>
  /// <param name="input">发送消息参数</param>
  /// <returns>发送的消息</returns>
  public async Task<LeanOnlineMessageDto> SendMessageAsync(LeanOnlineMessageSendDto input)
  {
    var message = new LeanOnlineMessage
    {
      SenderId = input.SenderId,
      ReceiverId = input.ReceiverId ?? 0L,
      Content = input.Content,
      MessageType = input.MessageType,
      SendTime = DateTime.Now,
      CreateTime = DateTime.Now,
      IsRead = 0
    };

    var messageId = await _messageRepository.CreateAsync(message);
    message.Id = messageId;
    return message.Adapt<LeanOnlineMessageDto>();
  }

  /// <summary>
  /// 获取用户消息历史
  /// </summary>
  /// <param name="userId">用户ID</param>
  /// <param name="pageSize">每页大小</param>
  /// <param name="pageIndex">页码</param>
  /// <returns>分页消息列表</returns>
  public async Task<LeanPageResult<LeanOnlineMessageDto>> GetMessageHistoryAsync(long userId, int pageSize, int pageIndex)
  {
    Expression<Func<LeanOnlineMessage, bool>> predicate = m =>
        m.ReceiverId == userId || m.SenderId == userId;

    var result = await _messageRepository.GetPageListAsync(
        predicate,
        pageSize,
        pageIndex,
        m => m.SendTime,
        false);

    return new LeanPageResult<LeanOnlineMessageDto>
    {
      Total = result.Total,
      Items = result.Items.Adapt<List<LeanOnlineMessageDto>>()
    };
  }

  /// <summary>
  /// 批量标记消息为已读
  /// </summary>
  /// <param name="input">批量标记参数</param>
  public async Task MarkMessagesAsReadAsync(LeanOnlineMessageMarkAsReadDto input)
  {
    var messages = await _messageRepository.GetListAsync(m =>
        m.ReceiverId == input.UserId &&
        m.SenderId == input.SenderId &&
        m.IsRead == 0);

    foreach (var message in messages)
    {
      message.IsRead = 1;
      message.UpdateTime = DateTime.Now;
    }

    if (messages.Any())
    {
      await _messageRepository.UpdateRangeAsync(messages);
    }
  }

  /// <summary>
  /// 删除消息
  /// </summary>
  /// <param name="messageId">消息ID</param>
  public async Task DeleteMessageAsync(long messageId)
  {
    await _messageRepository.DeleteAsync(m => m.Id == messageId);
  }

  /// <summary>
  /// 清理过期消息
  /// </summary>
  /// <param name="days">过期天数</param>
  public async Task CleanExpiredMessagesAsync(int days = 30)
  {
    var cutoffTime = DateTime.Now.AddDays(-days);
    await _messageRepository.DeleteAsync(m => m.SendTime < cutoffTime);
  }

  /// <summary>
  /// 获取在线消息列表（分页）
  /// </summary>
  public async Task<LeanPageResult<LeanOnlineMessageDto>> GetPageListAsync(LeanOnlineMessageQueryDto queryDto)
  {
    Expression<Func<LeanOnlineMessage, bool>> predicate = m => true;

    if (queryDto.SenderId.HasValue)
    {
      predicate = predicate.And(m => m.SenderId == queryDto.SenderId.Value);
    }

    if (queryDto.ReceiverId.HasValue)
    {
      predicate = predicate.And(m => m.ReceiverId == queryDto.ReceiverId.Value);
    }

    if (queryDto.IsRead.HasValue)
    {
      predicate = predicate.And(m => m.IsRead == queryDto.IsRead.Value);
    }

    if (!string.IsNullOrEmpty(queryDto.MessageType))
    {
      predicate = predicate.And(m => m.MessageType == queryDto.MessageType);
    }

    if (queryDto.StartTime.HasValue)
    {
      predicate = predicate.And(m => m.SendTime >= queryDto.StartTime.Value);
    }

    if (queryDto.EndTime.HasValue)
    {
      predicate = predicate.And(m => m.SendTime <= queryDto.EndTime.Value);
    }

    var result = await _messageRepository.GetPageListAsync(
        predicate,
        queryDto.PageSize,
        queryDto.PageIndex,
        m => m.SendTime,
        false);

    return new LeanPageResult<LeanOnlineMessageDto>
    {
      Total = result.Total,
      Items = result.Items.Adapt<List<LeanOnlineMessageDto>>()
    };
  }

  /// <summary>
  /// 获取在线消息详情
  /// </summary>
  public async Task<LeanOnlineMessageDto> GetAsync(long id)
  {
    var message = await _messageRepository.GetByIdAsync(id);
    return message?.Adapt<LeanOnlineMessageDto>();
  }

  /// <summary>
  /// 导出在线消息
  /// </summary>
  public async Task<LeanFileResult> ExportAsync(LeanOnlineMessageQueryDto queryDto)
  {
    var messages = await GetPageListAsync(queryDto);
    var stream = new MemoryStream();
    var fileName = $"online-messages-{DateTime.Now:yyyyMMddHHmmss}.xlsx";

    // TODO: 实现导出到Excel的逻辑
    // 这里需要根据实际需求实现Excel导出功能
    // 可以使用EPPlus、NPOI等库来实现

    return new LeanFileResult
    {
      FileName = fileName,
      ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
      Stream = stream
    };
  }
}