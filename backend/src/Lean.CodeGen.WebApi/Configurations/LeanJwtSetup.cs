// -----------------------------------------------------------------------
// <copyright file="LeanJwtSetup.cs" company="Lean">
// Copyright (c) Lean. All rights reserved.
// </copyright>
// <author>Lean</author>
// <created>2024-03-08</created>
// <summary>JWT认证服务配置</summary>
// -----------------------------------------------------------------------

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Lean.CodeGen.WebApi.Configurations;

/// <summary>
/// JWT认证服务配置
/// </summary>
public static class LeanJwtSetup
{
  /// <summary>
  /// 添加JWT认证服务
  /// </summary>
  /// <param name="services">服务集合</param>
  /// <param name="configuration">配置信息</param>
  /// <returns>服务集合</returns>
  public static IServiceCollection AddLeanJwt(this IServiceCollection services, IConfiguration configuration)
  {
    var jwtSettings = configuration.GetSection("JwtSettings");
    var secretKey = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!);

    services.AddAuthentication(options =>
    {
      options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
      options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
      options.TokenValidationParameters = new TokenValidationParameters
      {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(secretKey)
      };
    });

    return services;
  }
}