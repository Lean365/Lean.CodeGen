using Lean.CodeGen.Application.Dtos.Identity.Login;
using System.Threading.Tasks;

namespace Lean.CodeGen.Application.Services.Identity;

/// <summary>
/// 认证服务接口
/// </summary>
public interface ILeanAuthService
{
  /// <summary>
  /// 用户登录
  /// </summary>
  /// <param name="input">登录信息</param>
  /// <returns>登录结果</returns>
  Task<LeanLoginResultDto> LoginAsync(LeanLoginDto input);

  /// <summary>
  /// 刷新令牌
  /// </summary>
  /// <param name="refreshToken">刷新令牌</param>
  /// <returns>登录结果</returns>
  Task<LeanLoginResultDto> RefreshTokenAsync(string refreshToken);

  /// <summary>
  /// 退出登录
  /// </summary>
  Task LogoutAsync();
}