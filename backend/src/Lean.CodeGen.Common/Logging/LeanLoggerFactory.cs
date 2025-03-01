using NLog;
using ILogger = NLog.ILogger;

namespace Lean.CodeGen.Common.Logging;

/// <summary>
/// 鏃ュ織宸ュ巶锟?
/// </summary>
public static class LeanLoggerFactory
{
  /// <summary>
  /// 鑾峰彇NLog鐨凩ogger瀹炰緥
  /// </summary>
  /// <typeparam name="T">鏃ュ織鎵€灞炵被锟?/typeparam>
  /// <returns>NLog鐨凩ogger瀹炰緥</returns>
  public static ILogger GetLogger<T>()
  {
    return LogManager.GetLogger(typeof(T).FullName);
  }

  /// <summary>
  /// 浠嶮icrosoft.Extensions.Logging.ILogger杞崲涓篘Log.ILogger
  /// </summary>
  /// <typeparam name="T">鏃ュ織鎵€灞炵被锟?/typeparam>
  /// <param name="logger">Microsoft鐨凩ogger瀹炰緥</param>
  /// <returns>NLog鐨凩ogger瀹炰緥</returns>
  public static ILogger ConvertFromMsLogger<T>(Microsoft.Extensions.Logging.ILogger<T> logger)
  {
    return LogManager.GetLogger(typeof(T).FullName);
  }
}

