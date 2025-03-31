using Microsoft.AspNetCore.Mvc;
using Lean.CodeGen.Application.Services.Signalr;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Application.Dtos.Signalr;
using Swashbuckle.AspNetCore.Annotations;
using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Application.Services.Admin;
using Microsoft.Extensions.Configuration;
using Lean.CodeGen.Common.Attributes;
using Lean.CodeGen.Common.Helpers;

namespace Lean.CodeGen.WebApi.Controllers.Signalr;

/// <summary>
/// 在线用户控制器
/// </summary>
[ApiController]
[Route("api/[controller]")]
[ApiExplorerSettings(GroupName = "signalr")]
[LeanPermission("signalr:onlineuser:list", "在线用户管理")]
public class LeanOnlineUserController : LeanBaseController
{
  private readonly ILeanOnlineUserService _userService;

  public LeanOnlineUserController(
      ILeanOnlineUserService userService,
      ILeanLocalizationService localizationService,
      IConfiguration configuration,
      ILeanUserContext userContext)
      : base(localizationService, configuration, userContext)
  {
    _userService = userService;
  }

  /// <summary>
  /// 获取在线用户列表
  /// </summary>
  [HttpGet("list")]
  [SwaggerOperation(Summary = "获取在线用户列表")]
  [LeanPermission("signalr:onlineuser:list", "查看在线用户")]
  public async Task<IActionResult> GetOnlineUsers()
  {
    var users = await _userService.GetOnlineUsersAsync();
    return Success(users, LeanBusinessType.Query);
  }

  /// <summary>
  /// 分页查询在线用户
  /// </summary>
  [HttpGet("page")]
  [SwaggerOperation(Summary = "分页查询在线用户")]
  [LeanPermission("signalr:onlineuser:query", "查询在线用户")]
  public async Task<IActionResult> GetPageList([FromQuery] LeanOnlineUserQueryDto input)
  {
    var result = await _userService.GetPageListAsync(input);
    return Success(result, LeanBusinessType.Query);
  }

  /// <summary>
  /// 检查用户是否在线
  /// </summary>
  [HttpGet("check/{userId}")]
  [SwaggerOperation(Summary = "检查用户是否在线")]
  [LeanPermission("signalr:onlineuser:query", "查询在线用户")]
  public async Task<IActionResult> IsUserOnline(long userId, [FromQuery] string deviceFingerprint, [FromQuery] string userAgent, [FromQuery] string deviceType)
  {
    Console.WriteLine("========== 检查用户在线状态 ==========");
    Console.WriteLine($"用户ID: {userId}");
    Console.WriteLine($"设备指纹: {deviceFingerprint}");
    Console.WriteLine($"用户代理: {userAgent}");
    Console.WriteLine($"设备类型: {deviceType}");

    // 使用 LeanDeviceHelper 计算设备唯一ID
    var deviceId = LeanDeviceHelper.GenerateDeviceId(deviceFingerprint, userAgent, deviceType);
    Console.WriteLine($"生成的设备ID: {deviceId}");

    var isOnline = await _userService.IsUserOnlineAsync(userId, deviceId);
    Console.WriteLine($"用户在线状态: {isOnline}");
    Console.WriteLine("================================");

    return Success(isOnline, LeanBusinessType.Query);
  }

  /// <summary>
  /// 清理离线用户
  /// </summary>
  [HttpPost("clean")]
  [SwaggerOperation(Summary = "清理离线用户")]
  [LeanPermission("signalr:onlineuser:delete", "删除在线用户")]
  public async Task<IActionResult> CleanOfflineUsers([FromQuery] int timeoutMinutes = 30)
  {
    await _userService.CleanOfflineUsersAsync(timeoutMinutes);
    return Success(LeanBusinessType.Delete);
  }

  /// <summary>
  /// 强制退出用户
  /// </summary>
  [HttpPost("logout/{userId}")]
  [SwaggerOperation(Summary = "强制退出用户")]
  [LeanPermission("signalr:onlineuser:delete", "删除在线用户")]
  public async Task<IActionResult> ForceLogout(long userId)
  {
    await _userService.ForceLogoutAsync(userId);
    return Success(LeanBusinessType.Delete);
  }
}