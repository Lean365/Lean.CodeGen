using Microsoft.AspNetCore.Mvc;
using Lean.CodeGen.Application.Services.Signalr;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Application.Dtos.Signalr;
using Swashbuckle.AspNetCore.Annotations;
using Lean.CodeGen.WebApi.Controllers;
using Lean.CodeGen.Common.Enums;

namespace Lean.CodeGen.WebApi.Controllers.Signalr;

/// <summary>
/// 在线消息控制器
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Tags("在线管理")]
public class LeanOnlineMessageController : LeanBaseController
{
  private readonly ILeanOnlineMessageService _messageService;

  public LeanOnlineMessageController(ILeanOnlineMessageService messageService)
  {
    _messageService = messageService;
  }

  /// <summary>
  /// 获取未读消息列表
  /// </summary>
  [HttpGet("unread/{userId}")]
  [SwaggerOperation(Summary = "获取未读消息列表")]
  public async Task<IActionResult> GetUnreadMessages(string userId)
  {
    var result = await _messageService.GetUnreadMessagesAsync(userId);
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 获取消息历史
  /// </summary>
  [HttpGet("history/{userId}")]
  [SwaggerOperation(Summary = "获取消息历史")]
  public async Task<IActionResult> GetMessageHistory(
      string userId,
      [FromQuery] int pageSize = 20,
      [FromQuery] int pageIndex = 1)
  {
    var result = await _messageService.GetMessageHistoryAsync(userId, pageSize, pageIndex);
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 标记消息已读
  /// </summary>
  [HttpPost("read/{messageId}")]
  [SwaggerOperation(Summary = "标记消息已读")]
  public async Task<IActionResult> MarkMessageAsRead(long messageId)
  {
    await _messageService.MarkMessageAsReadAsync(messageId);
    return Success(LeanBusinessType.Update);
  }

  /// <summary>
  /// 批量标记消息已读
  /// </summary>
  [HttpPost("read-batch")]
  [SwaggerOperation(Summary = "批量标记消息已读")]
  public async Task<IActionResult> MarkMessagesAsRead([FromBody] LeanMarkMessagesAsReadDto request)
  {
    await _messageService.MarkMessagesAsReadAsync(request);
    return Success(LeanBusinessType.Update);
  }

  /// <summary>
  /// 删除消息
  /// </summary>
  [HttpDelete("{messageId}")]
  [SwaggerOperation(Summary = "删除消息")]
  public async Task<IActionResult> DeleteMessage(long messageId)
  {
    await _messageService.DeleteMessageAsync(messageId);
    return Success(LeanBusinessType.Delete);
  }

  /// <summary>
  /// 清理过期消息
  /// </summary>
  [HttpPost("clean")]
  [SwaggerOperation(Summary = "清理过期消息")]
  public async Task<IActionResult> CleanExpiredMessages([FromQuery] int days = 30)
  {
    await _messageService.CleanExpiredMessagesAsync(days);
    return Success(LeanBusinessType.Delete);
  }
}