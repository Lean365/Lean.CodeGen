using Microsoft.AspNetCore.Mvc;
using Lean.CodeGen.Application.Dtos.Identity.Login;
using Lean.CodeGen.Application.Services.Identity;
using System.Threading.Tasks;

namespace Lean.CodeGen.WebApi.Controllers.Identity;

/// <summary>
/// 认证控制器
/// </summary>
[ApiController]
[Route("api/auth")]
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
  public async Task<IActionResult> LoginAsync([FromBody] LeanLoginDto input)
  {
    var result = await _authService.LoginAsync(input);
    return Ok(result);
  }

  /// <summary>
  /// 刷新令牌
  /// </summary>
  /// <param name="refreshToken">刷新令牌</param>
  /// <returns>登录结果</returns>
  [HttpPost("refresh-token")]
  public async Task<IActionResult> RefreshTokenAsync([FromBody] string refreshToken)
  {
    var result = await _authService.RefreshTokenAsync(refreshToken);
    return Ok(result);
  }

  /// <summary>
  /// 退出登录
  /// </summary>
  [HttpPost("logout")]
  public async Task<IActionResult> LogoutAsync()
  {
    await _authService.LogoutAsync();
    return Ok();
  }
}