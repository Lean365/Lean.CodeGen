// -----------------------------------------------------------------------
// <copyright file="LeanNLogSetup.cs" company="Lean">
// Copyright (c) Lean. All rights reserved.
// </copyright>
// <author>Lean</author>
// <created>2024-03-08</created>
// <summary>NLog日志服务配置</summary>
// -----------------------------------------------------------------------

using NLog;
using NLog.Web;

namespace Lean.CodeGen.WebApi.Configurations;

/// <summary>
/// NLog日志服务配置类
/// </summary>
/// <remarks>
/// 用于配置和初始化NLog日志服务，包括：
/// 1. 清除默认日志提供程序
/// 2. 配置NLog服务
/// 3. 创建日志文件目录
/// </remarks>
public static class LeanNLogSetup
{
  /// <summary>
  /// 添加NLog日志服务
  /// </summary>
  /// <param name="builder">Web应用程序构建器</param>
  /// <returns>配置后的Web应用程序构建器</returns>
  /// <remarks>
  /// 此方法执行以下操作：
  /// 1. 清除所有默认的日志提供程序
  /// 2. 使用NLog作为日志提供程序
  /// 3. 在应用程序根目录下创建logs文件夹（如果不存在）
  /// </remarks>
  public static WebApplicationBuilder AddLeanNLog(this WebApplicationBuilder builder)
  {
    // 设置 NLog 配置文件路径
    builder.Logging.ClearProviders();    // 清除默认的日志提供程序
    builder.Host.UseNLog();             // 使用NLog作为日志提供程序

    // 确保日志目录存在
    var logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
    if (!Directory.Exists(logPath))
    {
      Directory.CreateDirectory(logPath);
    }

    return builder;
  }
}