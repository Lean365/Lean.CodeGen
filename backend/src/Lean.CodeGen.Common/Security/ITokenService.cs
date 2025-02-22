using System.Security.Claims;

namespace Lean.CodeGen.Common.Security;

/// <summary>
/// Token服务接口
/// </summary>
public interface ITokenService
{
  /// <summary>
  /// 创建访问令牌
  /// </summary>
  /// <param name="claims">声明</param>
  /// <returns>访问令牌</returns>
  string CreateToken(Claim[] claims);

  /// <summary>
  /// 创建刷新令牌
  /// </summary>
  /// <returns>刷新令牌</returns>
  string CreateRefreshToken();

  /// <summary>
  /// 验证令牌
  /// </summary>
  /// <param name="token">令牌</param>
  /// <returns>声明</returns>
  ClaimsPrincipal ValidateToken(string token);
}