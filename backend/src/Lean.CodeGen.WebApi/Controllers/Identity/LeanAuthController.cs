using Microsoft.AspNetCore.Mvc;
using Lean.CodeGen.Application.Dtos.Identity.Login;
using Lean.CodeGen.Application.Services.Identity;
using System.Threading.Tasks;
using Lean.CodeGen.Common.Models;
namespace Lean.CodeGen.WebApi.Controllers.Identity;

/// <summary>
/// 认证控制器
/// </summary>
[ApiController]
[Route("api/auth")]
[ApiExplorerSettings(GroupName = "identity")]
public class LeanAuthController : LeanBaseController
{
  private readonly ILeanAuthService _authService;

  public LeanAuthController(ILeanAuthService authService)
  {
    _authService = authService;
  }

  /// <summary>
  /// 用户登录
  /// </summary>
  /// <param name="input">登录信息</param>
  /// <returns>登录结果</returns>
  [HttpPost("login")]
  public async Task<LeanApiResult<LeanLoginResultDto>> LoginAsync([FromBody] LeanLoginDto input)
  {
    var result = await _authService.LoginAsync(input);
    return LeanApiResult<LeanLoginResultDto>.Ok(result);
  }

  /// <summary>
  /// 刷新令牌
  /// </summary>
  /// <param name="refreshToken">刷新令牌</param>
  /// <returns>登录结果</returns>
  [HttpPost("refresh-token")]
  public async Task<LeanApiResult<LeanLoginResultDto>> RefreshTokenAsync([FromBody] string refreshToken)
  {
    var result = await _authService.RefreshTokenAsync(refreshToken);
    return LeanApiResult<LeanLoginResultDto>.Ok(result);
  }

  /// <summary>
  /// 退出登录
  /// </summary>
  [HttpPost("logout")]
  public async Task<LeanApiResult> LogoutAsync()
  {
    await _authService.LogoutAsync();
    return LeanApiResult.Ok();
  }
}