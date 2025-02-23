using NLog;

namespace Lean.CodeGen.Infrastructure.Services.Logging;

/// <summary>
/// 日志服务实现
/// </summary>
public class LeanLogService : ILeanLogService
{
  private readonly NLog.ILogger _logger;

  private static readonly string[] LOGO_LINES = new[]
  {
    @"   __                  _____  __  ____       __     _   ",
    @"  / /  ___  __ _ _ __ |___ / / /_| ___|   /\ \ \___| |_ ",
    @" / /  / _ \/ _` | '_ \  |_ \| '_ \___ \  /  \/ / _ \ __|",
    @"/ /__|  __/ (_| | | | |___) | (_) |__) |/ /\  /  __/ |_ ",
    @"\____/\___|\__,_|_| |_|____/ \___/____(_)_\ \/ \___|\__|"
  };

  private static readonly string[] COLORS = new[]
  {
    "\u001b[36m", // 青色
    "\u001b[32m", // 绿色
    "\u001b[33m", // 黄色
    "\u001b[35m", // 紫色
    "\u001b[31m"  // 红色
  };

  private const string SEPARATOR = "===========================================================";
  private const string SWAGGER_URL = "https://localhost:7152/swagger";
  private const string EMAIL = "Lean365@outlook.com";
  private const string WEBSITE = "https://gitee.com/lean365";
  private const string GITHUB = "https://github.com/Lean365/Lean.CodeGen";
  private const string VERSION = "1.0.0";

  public LeanLogService()
  {
    _logger = LogManager.GetCurrentClassLogger();
  }

  /// <summary>
  /// 输出欢迎信息
  /// </summary>
  public void LogWelcomeInfo()
  {
    try
    {
      var consoleWidth = Console.WindowWidth;
      var maxLineLength = Math.Max(
        Math.Max(LOGO_LINES.Max(l => l.Length), SEPARATOR.Length),
        Math.Max(
          Math.Max(SWAGGER_URL.Length + 10, EMAIL.Length + 10),
          Math.Max(WEBSITE.Length + 10, GITHUB.Length + 10)
        )
      );

      // 输出上边框
      var padding = (consoleWidth - SEPARATOR.Length) / 2;
      _logger.Info("\u001b[32m{0}{1}\u001b[0m", new string(' ', padding), SEPARATOR);

      // 输出彩色 Logo
      foreach (var (line, color) in LOGO_LINES.Zip(COLORS))
      {
        padding = (consoleWidth - line.Length) / 2;
        _logger.Info("{0}{1}{2}\u001b[0m", new string(' ', padding), color, line);
      }

      // 输出分隔线
      padding = (consoleWidth - SEPARATOR.Length) / 2;
      _logger.Info("\u001b[32m{0}{1}\u001b[0m", new string(' ', padding), SEPARATOR);

      // 输出版本信息
      var versionInfo = $"Version {VERSION}";
      padding = (consoleWidth - versionInfo.Length) / 2;
      _logger.Info("{0}{1}", new string(' ', padding), versionInfo);

      // 输出分隔线
      padding = (consoleWidth - SEPARATOR.Length) / 2;
      _logger.Info("\u001b[32m{0}{1}\u001b[0m", new string(' ', padding), SEPARATOR);

      // 输出信息行
      padding = (consoleWidth - (SWAGGER_URL.Length + 10)) / 2;
      _logger.Info("{0}\uD83D\uDCDD 文档地址：{1}", new string(' ', padding), SWAGGER_URL);

      padding = (consoleWidth - (EMAIL.Length + 10)) / 2;
      _logger.Info("{0}\uD83D\uDCE7 联系方式：{1}", new string(' ', padding), EMAIL);

      padding = (consoleWidth - (WEBSITE.Length + 10)) / 2;
      _logger.Info("{0}\uD83D\uDD17 码云地址：{1}", new string(' ', padding), WEBSITE);

      padding = (consoleWidth - (GITHUB.Length + 10)) / 2;
      _logger.Info("{0}\uD83D\uDCBB GitHub：{1}", new string(' ', padding), GITHUB);

      // 输出下边框
      padding = (consoleWidth - SEPARATOR.Length) / 2;
      _logger.Info("\u001b[32m{0}{1}\u001b[0m", new string(' ', padding), SEPARATOR);
    }
    catch
    {
      // 如果无法获取控制台宽度，回退到原始输出方式
      _logger.Info("\u001b[32m{0}\u001b[0m", SEPARATOR);
      foreach (var (line, color) in LOGO_LINES.Zip(COLORS))
      {
        _logger.Info("{0}{1}\u001b[0m", color, line);
      }
      _logger.Info("\u001b[32m{0}\u001b[0m", SEPARATOR);
      _logger.Info("Version {0}", VERSION);
      _logger.Info("\u001b[32m{0}\u001b[0m", SEPARATOR);
      _logger.Info("\uD83D\uDCDD 文档地址：{0}", SWAGGER_URL);
      _logger.Info("\uD83D\uDCE7 联系方式：{0}", EMAIL);
      _logger.Info("\uD83D\uDD17 码云地址：{0}", WEBSITE);
      _logger.Info("\uD83D\uDCBB GitHub：{0}", GITHUB);
      _logger.Info("\u001b[32m{0}\u001b[0m", SEPARATOR);
    }
  }
}