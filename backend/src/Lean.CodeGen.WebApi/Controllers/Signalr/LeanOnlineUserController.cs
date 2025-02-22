using Microsoft.AspNetCore.Mvc;
using Lean.CodeGen.Application.Services.Signalr;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Application.Dtos.Signalr;
using Swashbuckle.AspNetCore.Annotations;

namespace Lean.CodeGen.WebApi.Controllers.Signalr;

/// <summary>
/// 在线用户控制器
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class LeanOnlineUserController : ControllerBase
{
  private readonly ILeanOnlineUserService _userService;

  public LeanOnlineUserController(ILeanOnlineUserService userService)
  {
    _userService = userService;
  }

  /// <summary>
  /// 获取在线用户列表
  /// </summary>
  [HttpGet("list")]
  [SwaggerOperation(Summary = "获取在线用户列表")]
  public async Task<LeanApiResult<List<LeanOnlineUserDto>>> GetOnlineUsers()
  {
    var users = await _userService.GetOnlineUsersAsync();
    return LeanApiResult<List<LeanOnlineUserDto>>.Ok(users);
  }

  /// <summary>
  /// 分页查询在线用户
  /// </summary>
  [HttpGet("page")]
  [SwaggerOperation(Summary = "分页查询在线用户")]
  public async Task<LeanApiResult<LeanPageResult<LeanOnlineUserDto>>> GetPageList([FromQuery] LeanQueryOnlineUserDto input)
  {
    var result = await _userService.GetPageListAsync(input);
    return LeanApiResult<LeanPageResult<LeanOnlineUserDto>>.Ok(result);
  }

  /// <summary>
  /// 检查用户是否在线
  /// </summary>
  [HttpGet("check/{userId}")]
  [SwaggerOperation(Summary = "检查用户是否在线")]
  public async Task<LeanApiResult<bool>> IsUserOnline(string userId)
  {
    var isOnline = await _userService.IsUserOnlineAsync(userId);
    return LeanApiResult<bool>.Ok(isOnline);
  }

  /// <summary>
  /// 清理离线用户
  /// </summary>
  [HttpPost("clean")]
  [SwaggerOperation(Summary = "清理离线用户")]
  public async Task<LeanApiResult> CleanOfflineUsers([FromQuery] int timeoutMinutes = 30)
  {
    await _userService.CleanOfflineUsersAsync(timeoutMinutes);
    return LeanApiResult.Ok();
  }

  /// <summary>
  /// 强制退出用户
  /// </summary>
  [HttpPost("logout/{userId}")]
  [SwaggerOperation(Summary = "强制退出用户")]
  public async Task<LeanApiResult> ForceLogout(string userId)
  {
    await _userService.ForceLogoutAsync(userId);
    return LeanApiResult.Ok();
  }
}