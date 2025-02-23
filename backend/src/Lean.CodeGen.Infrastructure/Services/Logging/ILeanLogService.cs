using NLog;

namespace Lean.CodeGen.Infrastructure.Services.Logging;

/// <summary>
/// 日志服务接口
/// </summary>
public interface ILeanLogService
{
  /// <summary>
  /// 输出欢迎信息
  /// </summary>
  void LogWelcomeInfo();
}