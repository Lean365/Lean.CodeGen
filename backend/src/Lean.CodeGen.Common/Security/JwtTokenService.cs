using System;
using System.Text;
using System.Security.Claims;
using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Lean.CodeGen.Common.Options;

namespace Lean.CodeGen.Common.Security;

/// <summary>
/// JWT Token服务
/// </summary>
public class JwtTokenService : ITokenService
{
  private readonly LeanJwtOptions _jwtOptions;

  public JwtTokenService(IOptions<LeanSecurityOptions> securityOptions)
  {
    _jwtOptions = securityOptions.Value.Jwt;
  }

  /// <summary>
  /// 创建访问令牌
  /// </summary>
  /// <param name="claims">声明</param>
  /// <returns>访问令牌</returns>
  public string CreateToken(Claim[] claims)
  {
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));
    var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(
        issuer: _jwtOptions.Issuer,
        audience: _jwtOptions.Audience,
        claims: claims,
        expires: DateTime.Now.AddMinutes(_jwtOptions.ExpireMinutes),
        signingCredentials: credentials);

    return new JwtSecurityTokenHandler().WriteToken(token);
  }

  /// <summary>
  /// 创建刷新令牌
  /// </summary>
  /// <returns>刷新令牌</returns>
  public string CreateRefreshToken()
  {
    var randomNumber = new byte[32];
    using var rng = RandomNumberGenerator.Create();
    rng.GetBytes(randomNumber);
    return Convert.ToBase64String(randomNumber);
  }

  /// <summary>
  /// 验证令牌
  /// </summary>
  /// <param name="token">令牌</param>
  /// <returns>声明</returns>
  public ClaimsPrincipal ValidateToken(string token)
  {
    var tokenHandler = new JwtSecurityTokenHandler();
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));

    var parameters = new TokenValidationParameters
    {
      ValidateIssuerSigningKey = true,
      IssuerSigningKey = key,
      ValidateIssuer = true,
      ValidIssuer = _jwtOptions.Issuer,
      ValidateAudience = true,
      ValidAudience = _jwtOptions.Audience,
      ValidateLifetime = true,
      ClockSkew = TimeSpan.Zero
    };

    return tokenHandler.ValidateToken(token, parameters, out _);
  }
}