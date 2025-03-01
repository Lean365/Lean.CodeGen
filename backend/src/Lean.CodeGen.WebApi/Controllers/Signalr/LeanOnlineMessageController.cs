using Microsoft.AspNetCore.Mvc;
using Lean.CodeGen.Application.Services.Signalr;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Application.Dtos.Signalr;
using Swashbuckle.AspNetCore.Annotations;
using Lean.CodeGen.WebApi.Controllers;
using Lean.CodeGen.Common.Enums;
using Microsoft.Extensions.Configuration;
using Lean.CodeGen.Application.Services.Admin;

namespace Lean.CodeGen.WebApi.Controllers.Signalr;

/// <summary>
/// 在线消息控制器
/// </summary>
[Route("api/online/messages")]
[ApiController]
[Tags("在线管理")]
public class LeanOnlineMessageController : LeanBaseController
{
  private readonly ILeanOnlineMessageService _onlineMessageService;

  /// <summary>
  /// 构造函数
  /// </summary>
  public LeanOnlineMessageController(
      ILeanOnlineMessageService onlineMessageService,
      ILeanLocalizationService localizationService,
      IConfiguration configuration)
      : base(localizationService, configuration)
  {
    _onlineMessageService = onlineMessageService;
  }

  /// <summary>
  /// 获取在线消息列表（分页）
  /// </summary>
  [HttpGet]
  public async Task<IActionResult> GetPageListAsync([FromQuery] LeanQueryOnlineMessageDto queryDto)
  {
    var result = await _onlineMessageService.GetPageListAsync(queryDto);
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 获取在线消息详情
  /// </summary>
  [HttpGet("{id}")]
  public async Task<IActionResult> GetAsync(long id)
  {
    var result = await _onlineMessageService.GetAsync(id);
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 发送在线消息
  /// </summary>
  [HttpPost]
  public async Task<IActionResult> SendAsync([FromBody] LeanSendMessageDto input)
  {
    var result = await _onlineMessageService.SendMessageAsync(input);
    return result != null ? Success(result, LeanBusinessType.Create) : await ErrorAsync("signalr.error.send_failed");
  }

  /// <summary>
  /// 导出在线消息
  /// </summary>
  [HttpGet("export")]
  public async Task<IActionResult> ExportAsync([FromQuery] LeanQueryOnlineMessageDto queryDto)
  {
    var result = await _onlineMessageService.ExportAsync(queryDto);
    return File(result.Stream, result.ContentType, result.FileName);
  }

  /// <summary>
  /// 获取未读消息列表
  /// </summary>
  [HttpGet("unread/{userId}")]
  [SwaggerOperation(Summary = "获取未读消息列表")]
  public async Task<IActionResult> GetUnreadMessages(string userId)
  {
    var result = await _onlineMessageService.GetUnreadMessagesAsync(userId);
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
    var result = await _onlineMessageService.GetMessageHistoryAsync(userId, pageSize, pageIndex);
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 标记消息已读
  /// </summary>
  [HttpPost("read/{messageId}")]
  [SwaggerOperation(Summary = "标记消息已读")]
  public async Task<IActionResult> MarkMessageAsRead(long messageId)
  {
    await _onlineMessageService.MarkMessageAsReadAsync(messageId);
    return Success(LeanBusinessType.Update);
  }

  /// <summary>
  /// 批量标记消息已读
  /// </summary>
  [HttpPost("read-batch")]
  [SwaggerOperation(Summary = "批量标记消息已读")]
  public async Task<IActionResult> MarkMessagesAsRead([FromBody] LeanMarkMessagesAsReadDto request)
  {
    await _onlineMessageService.MarkMessagesAsReadAsync(request);
    return Success(LeanBusinessType.Update);
  }

  /// <summary>
  /// 删除消息
  /// </summary>
  [HttpDelete("{messageId}")]
  [SwaggerOperation(Summary = "删除消息")]
  public async Task<IActionResult> DeleteMessage(long messageId)
  {
    await _onlineMessageService.DeleteMessageAsync(messageId);
    return Success(LeanBusinessType.Delete);
  }

  /// <summary>
  /// 清理过期消息
  /// </summary>
  [HttpPost("clean")]
  [SwaggerOperation(Summary = "清理过期消息")]
  public async Task<IActionResult> CleanExpiredMessages([FromQuery] int days = 30)
  {
    await _onlineMessageService.CleanExpiredMessagesAsync(days);
    return Success(LeanBusinessType.Delete);
  }
}