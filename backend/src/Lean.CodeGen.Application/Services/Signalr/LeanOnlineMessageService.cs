using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
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
  private readonly ILeanRepository<LeanOnlineMessage> _repository;
  private readonly ILogger<LeanOnlineMessageService> _logger;

  /// <summary>
  /// 构造函数
  /// </summary>
  /// <param name="repository">在线消息仓储</param>
  /// <param name="sqlSafeService">SQL注入防护服务</param>
  /// <param name="securityOptions">安全选项</param>
  /// <param name="logger">日志记录器</param>
  public LeanOnlineMessageService(
      ILeanRepository<LeanOnlineMessage> repository,
      ILeanSqlSafeService sqlSafeService,
      IOptions<LeanSecurityOptions> securityOptions,
      ILogger<LeanOnlineMessageService> logger)
      : base(sqlSafeService, securityOptions, logger)
  {
    _repository = repository;
    _logger = logger;
  }

  /// <summary>
  /// 发送消息
  /// </summary>
  /// <param name="input">发送消息参数</param>
  /// <returns>发送的消息</returns>
  public async Task<LeanOnlineMessageDto> SendMessageAsync(LeanSendMessageDto input)
  {
    var message = new LeanOnlineMessage
    {
      SenderId = input.SenderId,
      ReceiverId = input.ReceiverId,
      Content = input.Content,
      MessageType = input.MessageType,
      SendTime = DateTime.Now,
      CreateTime = DateTime.Now,
      IsRead = false
    };

    var messageId = await _repository.CreateAsync(message);
    message.Id = messageId;
    return message.Adapt<LeanOnlineMessageDto>();
  }

  /// <summary>
  /// 获取用户未读消息列表
  /// </summary>
  /// <param name="userId">用户ID</param>
  /// <returns>未读消息列表</returns>
  public async Task<List<LeanOnlineMessageDto>> GetUnreadMessagesAsync(string userId)
  {
    var messages = await _repository.GetListAsync(m =>
        m.ReceiverId == userId &&
        !m.IsRead);
    return messages.Adapt<List<LeanOnlineMessageDto>>();
  }

  /// <summary>
  /// 获取用户消息历史
  /// </summary>
  /// <param name="userId">用户ID</param>
  /// <param name="pageSize">每页大小</param>
  /// <param name="pageIndex">页码</param>
  /// <returns>分页消息列表</returns>
  public async Task<LeanPageResult<LeanOnlineMessageDto>> GetMessageHistoryAsync(string userId, int pageSize, int pageIndex)
  {
    Expression<Func<LeanOnlineMessage, bool>> predicate = m =>
        m.ReceiverId == userId || m.SenderId == userId;

    var result = await _repository.GetPageListAsync(
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
  /// 标记消息为已读
  /// </summary>
  /// <param name="messageId">消息ID</param>
  public async Task MarkMessageAsReadAsync(long messageId)
  {
    var message = await _repository.GetByIdAsync(messageId);
    if (message != null && !message.IsRead)
    {
      message.IsRead = true;
      message.UpdateTime = DateTime.Now;
      await _repository.UpdateAsync(message);
    }
  }

  /// <summary>
  /// 批量标记消息为已读
  /// </summary>
  /// <param name="input">批量标记参数</param>
  public async Task MarkMessagesAsReadAsync(LeanMarkMessagesAsReadDto input)
  {
    var messages = await _repository.GetListAsync(m =>
        m.ReceiverId == input.UserId &&
        m.SenderId == input.SenderId &&
        !m.IsRead);

    foreach (var message in messages)
    {
      message.IsRead = true;
      message.UpdateTime = DateTime.Now;
    }

    if (messages.Any())
    {
      await _repository.UpdateRangeAsync(messages);
    }
  }

  /// <summary>
  /// 删除消息
  /// </summary>
  /// <param name="messageId">消息ID</param>
  public async Task DeleteMessageAsync(long messageId)
  {
    await _repository.DeleteAsync(m => m.Id == messageId);
  }

  /// <summary>
  /// 清理过期消息
  /// </summary>
  /// <param name="days">过期天数</param>
  public async Task CleanExpiredMessagesAsync(int days = 30)
  {
    var cutoffTime = DateTime.Now.AddDays(-days);
    await _repository.DeleteAsync(m => m.SendTime < cutoffTime);
  }
}