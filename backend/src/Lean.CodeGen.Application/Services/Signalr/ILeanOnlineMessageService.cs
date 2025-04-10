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
    /// 获取在线消息列表（分页）
    /// </summary>
    Task<LeanPageResult<LeanOnlineMessageDto>> GetPageListAsync(LeanOnlineMessageQueryDto queryDto);

    /// <summary>
    /// 获取在线消息详情
    /// </summary>
    Task<LeanOnlineMessageDto> GetAsync(long id);

    /// <summary>
    /// 导出在线消息
    /// </summary>
    Task<LeanFileResult> ExportAsync(LeanOnlineMessageQueryDto queryDto);

    /// <summary>
    /// 发送消息
    /// </summary>
    Task<LeanOnlineMessageDto> SendMessageAsync(LeanOnlineMessageSendDto input);

    /// <summary>
    /// 获取用户未读消息列表
    /// </summary>
    Task<List<LeanOnlineMessageDto>> GetUnreadMessagesAsync(long userId);

    /// <summary>
    /// 获取用户消息历史
    /// </summary>
    Task<LeanPageResult<LeanOnlineMessageDto>> GetMessageHistoryAsync(long userId, int pageSize, int pageIndex);

    /// <summary>
    /// 标记消息已读
    /// </summary>
    Task MarkMessageAsReadAsync(long messageId);

    /// <summary>
    /// 批量标记消息已读
    /// </summary>
    Task MarkMessagesAsReadAsync(LeanOnlineMessageMarkAsReadDto input);

    /// <summary>
    /// 删除消息
    /// </summary>
    Task DeleteMessageAsync(long messageId);

    /// <summary>
    /// 清理过期消息
    /// </summary>
    Task CleanExpiredMessagesAsync(int days = 30);

    /// <summary>
    /// 创建消息
    /// </summary>
    /// <param name="message">消息实体</param>
    Task CreateMessageAsync(LeanOnlineMessage message);

    /// <summary>
    /// 获取消息详情
    /// </summary>
    /// <param name="messageId">消息ID</param>
    Task<LeanOnlineMessageDto> GetMessageByIdAsync(long messageId);
}