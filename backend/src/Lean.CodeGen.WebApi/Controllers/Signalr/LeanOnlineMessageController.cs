using Microsoft.AspNetCore.Mvc;
using Lean.CodeGen.Application.Services.Signalr;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Application.Dtos.Signalr;
using Swashbuckle.AspNetCore.Annotations;
using Lean.CodeGen.WebApi.Controllers;
using Lean.CodeGen.Common.Enums;
using Microsoft.Extensions.Configuration;
using Lean.CodeGen.Application.Services.Admin;
using Lean.CodeGen.Common.Attributes;
using Microsoft.AspNetCore.SignalR;
using Lean.CodeGen.WebApi.Hubs;

namespace Lean.CodeGen.WebApi.Controllers.Signalr;

/// <summary>
/// 在线消息控制器
/// </summary>
[ApiController]
[Route("api/[controller]")]
[ApiExplorerSettings(GroupName = "signalr")]
[LeanPermission("signalr:onlinemessage:list", "在线消息管理")]
public class LeanOnlineMessageController : LeanBaseController
{
  private readonly ILeanOnlineMessageService _onlineMessageService;
  private readonly IHubContext<LeanSignalRHub> _hubContext;

  /// <summary>
  /// 构造函数
  /// </summary>
  public LeanOnlineMessageController(
      ILeanOnlineMessageService onlineMessageService,
      ILeanLocalizationService localizationService,
      IConfiguration configuration,
      IHubContext<LeanSignalRHub> hubContext,
      ILeanUserContext userContext)
      : base(localizationService, configuration, userContext)
  {
    _onlineMessageService = onlineMessageService;
    _hubContext = hubContext;
  }

  /// <summary>
  /// 获取在线消息列表（分页）
  /// </summary>
  [HttpGet]
  [LeanPermission("signalr:onlinemessage:list", "查看在线消息")]
  public async Task<IActionResult> GetPageListAsync([FromQuery] LeanOnlineMessageQueryDto queryDto)
  {
    var result = await _onlineMessageService.GetPageListAsync(queryDto);
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 获取在线消息详情
  /// </summary>
  [HttpGet("{id}")]
  [LeanPermission("signalr:onlinemessage:query", "查询在线消息")]
  public async Task<IActionResult> GetAsync(long id)
  {
    var result = await _onlineMessageService.GetAsync(id);
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 发送消息
  /// </summary>
  [HttpPost("send")]
  [LeanPermission("signalr:onlinemessage:send", "发送消息")]
  public async Task<IActionResult> SendMessageAsync([FromBody] LeanOnlineMessageSendDto input)
  {
    var result = await _onlineMessageService.SendMessageAsync(input);
    return Success(result, LeanBusinessType.Create);
  }

  /// <summary>
  /// 标记消息已读
  /// </summary>
  [HttpPost("read")]
  [LeanPermission("signalr:onlinemessage:read", "标记已读")]
  public async Task<IActionResult> MarkAsReadAsync([FromBody] LeanOnlineMessageMarkAsReadDto input)
  {
    await _onlineMessageService.MarkMessagesAsReadAsync(input);
    return Success(true, LeanBusinessType.Update);
  }

  /// <summary>
  /// 获取未读消息
  /// </summary>
  [HttpGet("unread/{userId}")]
  [LeanPermission("signalr:onlinemessage:unread", "获取未读")]
  public async Task<IActionResult> GetUnreadMessagesAsync(long userId)
  {
    var result = await _onlineMessageService.GetUnreadMessagesAsync(userId);
    await _hubContext.Clients.User(userId.ToString()).SendAsync("ReceiveUnreadMessages", result);
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 获取消息历史
  /// </summary>
  [HttpGet("history/{userId}")]
  [LeanPermission("signalr:onlinemessage:history", "获取历史")]
  public async Task<IActionResult> GetMessageHistoryAsync(long userId, [FromQuery] int pageSize = 20, [FromQuery] int pageIndex = 1)
  {
    var result = await _onlineMessageService.GetMessageHistoryAsync(userId, pageSize, pageIndex);
    await _hubContext.Clients.User(userId.ToString()).SendAsync("ReceiveMessageHistory", result);
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 删除消息
  /// </summary>
  [HttpDelete("{id}")]
  [LeanPermission("signalr:onlinemessage:delete", "删除消息")]
  public async Task<IActionResult> DeleteAsync(long id)
  {
    await _onlineMessageService.DeleteMessageAsync(id);
    return Success(true, LeanBusinessType.Delete);
  }

  /// <summary>
  /// 清理过期消息
  /// </summary>
  [HttpPost("clean")]
  [LeanPermission("signalr:onlinemessage:clean", "清理消息")]
  public async Task<IActionResult> CleanExpiredMessagesAsync([FromQuery] int days = 30)
  {
    await _onlineMessageService.CleanExpiredMessagesAsync(days);
    return Success(true, LeanBusinessType.Delete);
  }
}