using Microsoft.AspNetCore.Mvc;
using Lean.CodeGen.Application.Services.Signalr;
using Lean.CodeGen.Common.Models;
using Lean.CodeGen.Application.Dtos.Signalr;
using Swashbuckle.AspNetCore.Annotations;
using Lean.CodeGen.Common.Enums;
using Lean.CodeGen.Application.Services.Admin;
using Microsoft.Extensions.Configuration;

namespace Lean.CodeGen.WebApi.Controllers.Signalr;

/// <summary>
/// 在线用户控制器
/// </summary>
[ApiController]
[Route("api/[controller]")]
[ApiExplorerSettings(GroupName = "signalr")]
public class LeanOnlineUserController : LeanBaseController
{
    private readonly ILeanOnlineUserService _userService;

    public LeanOnlineUserController(
        ILeanOnlineUserService userService,
        ILeanLocalizationService localizationService,
        IConfiguration configuration)
        : base(localizationService, configuration)
    {
        _userService = userService;
    }

    /// <summary>
    /// 获取在线用户列表
    /// </summary>
    [HttpGet("list")]
    [SwaggerOperation(Summary = "获取在线用户列表")]
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
    public async Task<IActionResult> IsUserOnline(string userId)
    {
        var isOnline = await _userService.IsUserOnlineAsync(userId);
        return Success(isOnline, LeanBusinessType.Query);
    }

    /// <summary>
    /// 清理离线用户
    /// </summary>
    [HttpPost("clean")]
    [SwaggerOperation(Summary = "清理离线用户")]
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
    public async Task<IActionResult> ForceLogout(string userId)
    {
        await _userService.ForceLogoutAsync(userId);
        return Success(LeanBusinessType.Delete);
    }
}