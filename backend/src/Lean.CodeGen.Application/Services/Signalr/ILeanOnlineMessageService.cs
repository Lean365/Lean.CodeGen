using System.Threading.Tasks;
using System.Collections.Generic;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Domain.Entities.Signalr;
using Lean.CodeGen.Application.Dtos.Signalr;

namespace Lean.CodeGen.Application.Services.Signalr;

/// <summary>
/// 在线消息服务接口
/// </summary>
public interface ILeanOnlineMessageService
{
  /// <summary>
  /// 发送消息
  /// </summary>
  Task<LeanOnlineMessageDto> SendMessageAsync(LeanSendMessageDto input);

  /// <summary>
  /// 获取用户未读消息列表
  /// </summary>
  Task<List<LeanOnlineMessageDto>> GetUnreadMessagesAsync(string userId);

  /// <summary>
  /// 获取用户消息历史
  /// </summary>
  Task<LeanPageResult<LeanOnlineMessageDto>> GetMessageHistoryAsync(string userId, int pageSize, int pageIndex);

  /// <summary>
  /// 标记消息已读
  /// </summary>
  Task MarkMessageAsReadAsync(long messageId);

  /// <summary>
  /// 批量标记消息已读
  /// </summary>
  Task MarkMessagesAsReadAsync(LeanMarkMessagesAsReadDto input);

  /// <summary>
  /// 删除消息
  /// </summary>
  Task DeleteMessageAsync(long messageId);

  /// <summary>
  /// 清理过期消息
  /// </summary>
  Task CleanExpiredMessagesAsync(int days = 30);
}